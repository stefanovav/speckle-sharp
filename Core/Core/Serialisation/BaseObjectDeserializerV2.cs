#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Speckle.Core.Logging;
using Speckle.Core.Models;
using Speckle.Core.Transports;
using Speckle.Newtonsoft.Json;
using Speckle.Newtonsoft.Json.Linq;

namespace Speckle.Core.Serialisation;

public sealed class BaseObjectDeserializerV2
{
  private bool _busy;
  private readonly object _callbackLock = new();

  // id -> Base if already deserialized or id -> Task<object> if was handled by a bg thread
  private Dictionary<string, object?>? _deserializedObjects;

  /// <summary>
  /// Property that describes the type of the object.
  /// </summary>
  public const string TypeDiscriminator = nameof(Base.speckle_type);

  private DeserializationWorkerThreads? _workerThreads;

  public CancellationToken CancellationToken { get; set; }

  /// <summary>
  /// The sync transport. This transport will be used synchronously.
  /// </summary>
  public ITransport ReadTransport { get; set; }

  public Action<string, int>? OnProgressAction { get; set; }

  public Action<string, Exception>? OnErrorAction { get; set; }

  public string? BlobStorageFolder { get; set; }
  public TimeSpan Elapsed { get; private set; }

  /// <param name="rootObjectJson">The JSON string of the object to be deserialized <see cref="Base"/></param>
  /// <returns>A <see cref="Base"/> typed object deserialized from the <paramref name="rootObjectJson"/></returns>
  /// <exception cref="InvalidOperationException">Thrown when <see cref="_busy"/></exception>
  /// <exception cref="ArgumentException">Thrown when <paramref name="rootObjectJson"/> deserializes to a type other than <see cref="Base"/></exception>
  public Base Deserialize(string rootObjectJson)
  {
    if (_busy)
      throw new InvalidOperationException(
        "A deserializer instance can deserialize only 1 object at a time. Consider creating multiple deserializer instances"
      );

    try
    {
      _busy = true;
      var stopwatch = Stopwatch.StartNew();
      _deserializedObjects = new();
      _workerThreads = new DeserializationWorkerThreads(this);
      _workerThreads.Start();

      List<(string, int)> closures = GetClosures(rootObjectJson);
      closures.Sort((a, b) => b.Item2.CompareTo(a.Item2));
      foreach (var closure in closures)
      {
        string objId = closure.Item1;
        // pausing for getting object from the transport
        stopwatch.Stop();
        string objJson = ReadTransport.GetObject(objId);
        stopwatch.Start();
        object? deserializedOrPromise = DeserializeTransportObjectProxy(objJson);
        lock (_deserializedObjects)
          _deserializedObjects[objId] = deserializedOrPromise;
      }

      object ret = DeserializeTransportObject(rootObjectJson);

      stopwatch.Stop();
      Elapsed += stopwatch.Elapsed;
      if (ret is Base b)
        return b;
      else
        throw new Exception(
          $"Expected {nameof(rootObjectJson)} to be deserialized to type {nameof(Base)} but was {ret}"
        );
    }
    finally
    {
      _deserializedObjects = null;
      _workerThreads.Dispose();
      _workerThreads = null;
      _busy = false;
    }
  }

  private List<(string, int)> GetClosures(string rootObjectJson)
  {
    try
    {
      List<(string, int)> closureList = new();
      JObject doc1 = JObject.Parse(rootObjectJson);

      if (!doc1.ContainsKey("__closure"))
        return new List<(string, int)>();
      foreach (JToken prop in doc1["__closure"])
      {
        string childId = ((JProperty)prop).Name;
        int childMinDepth = (int)((JProperty)prop).Value;
        closureList.Add((childId, childMinDepth));
      }
      return closureList;
    }
    catch
    {
      return new List<(string, int)>();
    }
  }

  private object? DeserializeTransportObjectProxy(string objectJson)
  {
    // Try background work
    Task<object?> bgResult = _workerThreads.TryStartTask(WorkerThreadTaskType.Deserialize, objectJson); //BUG: Because we don't guarantee this task will ever be awaited, this may lead to unobserved exceptions!
    if (bgResult != null)
      return bgResult;

    // SyncS
    return DeserializeTransportObject(objectJson);
  }

  public object? DeserializeTransportObject(string objectJson)
  {
    if (objectJson is null)
      throw new ArgumentNullException(nameof(objectJson), $"Cannot deserialize {nameof(objectJson)}, value was null");
    // Apparently this automatically parses DateTimes in strings if it matches the format:
    // JObject doc1 = JObject.Parse(objectJson);

    // This is equivalent code that doesn't parse datetimes:
    JObject doc1;
    using (JsonReader reader = new JsonTextReader(new StringReader(objectJson)))
    {
      reader.DateParseHandling = DateParseHandling.None;
      doc1 = JObject.Load(reader);
    }
    
    object? converted = ConvertJsonElement(doc1);
    
    lock (_callbackLock)
      OnProgressAction?.Invoke("DS", 1);
    return converted;
  }

  public object? ConvertJsonElement(JToken doc)
  {
    CancellationToken.ThrowIfCancellationRequested();

    switch (doc.Type)
    {
      case JTokenType.Undefined:
      case JTokenType.Null:
      case JTokenType.None:
        return null;
      case JTokenType.Boolean:
        return (bool)doc;
      case JTokenType.Integer:
        try
        {
          return (long)doc;
        }
        catch (OverflowException ex)
        {
          var v = (object)(double)doc;
          SpeckleLog.Logger.Debug(
            ex,
            "Json property {tokenType} failed to deserialize {value} to {targetType}, will be deserialized as {fallbackType}",
            doc.Type,
            v,
            typeof(long),
            typeof(double)
          );
          return v;
        }
      case JTokenType.Float:
        return (double)doc;
      case JTokenType.String:
        return (string)doc;
      case JTokenType.Date:
        return (DateTime)doc;
      case JTokenType.Array:
        JArray docAsArray = (JArray)doc;
        List<object?> jsonList = new(docAsArray.Count);
        int retListCount = 0;
        foreach (JToken value in docAsArray)
        {
          object convertedValue = ConvertJsonElement(value);
          retListCount += convertedValue is DataChunk chunk ? chunk.data.Count : 1;
          jsonList.Add(convertedValue);
        }

        List<object?> retList = new(retListCount);
        foreach (object jsonObj in jsonList)
          if (jsonObj is DataChunk chunk)
            retList.AddRange(chunk.data);
          else
            retList.Add(jsonObj);

        return retList;
      case JTokenType.Object:
        Dictionary<string, object?> dict = new();

        foreach (JToken propJToken in doc)
        {
          JProperty prop = (JProperty)propJToken;
          if (prop.Name == "__closure")
            continue;
          dict[prop.Name] = ConvertJsonElement(prop.Value);
        }

        if (!dict.ContainsKey(TypeDiscriminator))
          return dict;

        if (dict[TypeDiscriminator] as string == "reference" && dict.ContainsKey("referencedId"))
        {
          var objId = (string)dict["referencedId"]!;
          object deserialized = null;
          lock (_deserializedObjects)
            if (_deserializedObjects.TryGetValue(objId, out object? o))
              deserialized = o;
          if (deserialized is Task<object> task)
          {
            try
            {
              deserialized = task.Result;
            }
            catch (AggregateException ex)
            {
              throw new Exception("Failed to deserialize reference object", ex);
            }
            lock (_deserializedObjects)
              _deserializedObjects[objId] = deserialized;
          }

          if (deserialized != null)
            return deserialized;

          // This reference was not already deserialized. Do it now in sync mode
          string objectJson = ReadTransport.GetObject(objId);
          if(objectJson is null) throw new Exception($"Failed to fetch object id {objId} from {ReadTransport} ");
          deserialized = DeserializeTransportObject(objectJson);
          
          lock (_deserializedObjects)
            _deserializedObjects[objId] = deserialized;
          return deserialized;
        }

        return Dict2Base(dict);
      default:
        throw new ArgumentException("Json value not supported: " + doc.Type, nameof(doc));
    }
  }

  private Base Dict2Base(Dictionary<string, object?> dictObj)
  {
    string typeName = (string)dictObj[TypeDiscriminator]!;
    Type type = SerializationUtilities.GetType(typeName);
    Base baseObj = (Base)Activator.CreateInstance(type);

    dictObj.Remove(TypeDiscriminator);
    dictObj.Remove("__closure");

    Dictionary<string, PropertyInfo> staticProperties = SerializationUtilities.GetTypePropeties(typeName);
    List<MethodInfo> onDeserializedCallbacks = SerializationUtilities.GetOnDeserializedCallbacks(typeName);

    foreach (var entry in dictObj)
    {
      string lowerPropertyName = entry.Key.ToLower();
      if (staticProperties.TryGetValue(lowerPropertyName, out PropertyInfo value) && value.CanWrite)
      {
        PropertyInfo property = staticProperties[lowerPropertyName];
        if (entry.Value == null)
        {
          // Check for JsonProperty(NullValueHandling = NullValueHandling.Ignore) attribute
          JsonPropertyAttribute attr = property.GetCustomAttribute<JsonPropertyAttribute>(true);
          if (attr != null && attr.NullValueHandling == NullValueHandling.Ignore)
            continue;
        }

        Type targetValueType = property.PropertyType;
        bool conversionOk = ValueConverter.ConvertValue(targetValueType, entry.Value, out object convertedValue);
        if (conversionOk)
          property.SetValue(baseObj, convertedValue);
        else
          // Cannot convert the value in the json to the static property type
          throw new Exception(
            $"Cannot deserialize {entry.Value.GetType().FullName} to {targetValueType.FullName}"
          );
      }
      else
      {
        // No writable property with this name
        CallSiteCache.SetValue(entry.Key, baseObj, entry.Value);
      }
    }

    if (baseObj is Blob bb && BlobStorageFolder != null)
      bb.filePath = bb.getLocalDestinationPath(BlobStorageFolder);

    foreach (MethodInfo onDeserialized in onDeserializedCallbacks)
      onDeserialized.Invoke(baseObj, new object?[] { null });

    return baseObj;
  }
}
