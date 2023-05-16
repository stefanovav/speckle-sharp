using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace ConnectorRevit.Caching.ExtensibleStorageCache
{
  public class ExtensibleStorageDocumentStreamData
  {
    private const char delimiter = ',';
    private Document document;
    private string streamId;

    private IDictionary<string, string> speckleIdToRevitId1to1;
    private IDictionary<string, string> revitIdToSpeckleId1to1;

    private IDictionary<string, List<string>> speckleIdToRevitId1toMany;
    private IDictionary<string, List<string>> revitIdToSpeckleId1toMany;

    List<string> fields = new()
      {
        nameof(speckleIdToRevitId1to1),
        nameof(speckleIdToRevitId1toMany),
        nameof(revitIdToSpeckleId1to1),
        nameof(revitIdToSpeckleId1toMany),
      };
    #region construction and init
    public ExtensibleStorageDocumentStreamData(Document document, string streamId)
    {
      this.document = document;
      this.streamId = streamId;
      Initialize();
    }

    private void Initialize()
    {
      using var schema = ExtensibleStorageCacheSchema.GetSchema(fields);
      using var entity = document.ProjectInformation.GetEntity(schema);

      speckleIdToRevitId1to1 = Initialize<string>(nameof(speckleIdToRevitId1to1), schema, entity);
      speckleIdToRevitId1toMany = Initialize<List<string>>(nameof(speckleIdToRevitId1toMany), schema, entity);
      revitIdToSpeckleId1to1 = Initialize<string>(nameof(revitIdToSpeckleId1to1), schema, entity);
      revitIdToSpeckleId1toMany = Initialize<List<string>>(nameof(revitIdToSpeckleId1toMany), schema, entity);
    }

    private static IDictionary<string, T> Initialize<T>(string name, Schema schema, Entity entity)
      where T : class
    {
      IDictionary<string, string> tempDict;
      try
      {
        if (typeof(T) == typeof(List<string>))
        {
          tempDict = entity.Get<IDictionary<string, string>>(schema.GetField(name));
          return tempDict
            .Select(kvp => new KeyValuePair<string, T>(kvp.Key, kvp.Value.Split(delimiter).ToList() as T))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
        else if (typeof(T) == typeof(string))
        {
          return entity.Get<IDictionary<string, T>>(schema.GetField(name));
        }
        else
        {
          throw new Exception($"Unexpected type {typeof(T)}, was initialized");
        }
      }
      catch (Autodesk.Revit.Exceptions.ArgumentException)
      {
        // schema has not been set yet so the cache is empty
        return new Dictionary<string, T>();
      }
    }
    #endregion

    public void Add1to1(Base @base, Element element)
    {
      Add1to1(@base.applicationId, element.UniqueId);
    }
    public void Add1to1(string baseAppId, string elementUniqueId)
    {
      speckleIdToRevitId1to1[baseAppId] = elementUniqueId;
      revitIdToSpeckleId1to1[elementUniqueId] = baseAppId;
    }
    public void Add1BaseToManyElement(string baseAppId, List<string> elementUniqueIds)
    {
      speckleIdToRevitId1toMany[baseAppId] = elementUniqueIds;
      foreach (var id in elementUniqueIds)
      {
        if (revitIdToSpeckleId1toMany.TryGetValue(id, out var ids))
        {
          ids.Add(baseAppId);
        }
        else if (revitIdToSpeckleId1to1.TryGetValue(id, out var currentId))
        {
          revitIdToSpeckleId1to1.Remove(id);
          revitIdToSpeckleId1toMany[id] = new List<string>() { currentId, baseAppId };
        }
        else
        {
          revitIdToSpeckleId1to1[id] = baseAppId;
        }
      }
    }
    //public void Add1ElementToManyBase(string elementUniqueId, List<string> baseAppIds)
    //{
    //  revitUniqueIdToSpeckleAppIdsMap[elementUniqueId] = baseAppIds;
    //  foreach (var id in baseAppIds)
    //  {
    //    if (speckleAppIdToRevitUniqueIdsMap.TryGetValue(id, out var ids))
    //    {
    //      ids.Add(elementUniqueId);
    //    }
    //    else if (revitUniqueIdToSpeckleAppIdMap.TryGetValue(id, out var currentId))
    //    {
    //      speckleAppIdToRevitUniqueIdMap.Remove(id);
    //      speckleAppIdToRevitUniqueIdsMap[id] = new List<string>() { currentId, elementUniqueId };
    //    }
    //    else
    //    {
    //      speckleAppIdToRevitUniqueIdMap[id] = elementUniqueId;
    //    }
    //  }
    //}
    public string GetRevitIdFromSpeckleId(string appId)
    {
      if (speckleIdToRevitId1to1.TryGetValue(appId, out var id))
      {
        return id;
      }
      return null;
    }
    public List<string> GetRevitIdsFromSpeckleId(string appId)
    {
      if (speckleIdToRevitId1toMany.TryGetValue(appId, out var ids))
      {
        return ids;
      }
      return new List<string>();
    }

    public string GetSpeckleIdFromRevitId(string uniqueId)
    {
      return revitIdToSpeckleId1to1[uniqueId];
    }
    public IEnumerable<string> GetAllRevitIds()
    {
      return revitIdToSpeckleId1to1.Keys.Concat(revitIdToSpeckleId1toMany.Keys);
    }
    public IEnumerable<string> GetAllSpeckleIds()
    {
      return speckleIdToRevitId1to1.Keys.Concat(speckleIdToRevitId1toMany.Keys);
    }
    public void SaveCache()
    {
      using var schema = ExtensibleStorageCacheSchema.GetSchema(fields);
      using var entity = new Entity(schema);
      //using var entity = document.ProjectInformation.GetEntity(schema);

      SaveSingleCache(speckleIdToRevitId1to1, nameof(speckleIdToRevitId1to1), schema, entity);
      SaveSingleCache(speckleIdToRevitId1toMany, nameof(speckleIdToRevitId1toMany), schema, entity);
      SaveSingleCache(revitIdToSpeckleId1to1, nameof(revitIdToSpeckleId1to1), schema, entity);
      SaveSingleCache(revitIdToSpeckleId1toMany, nameof(revitIdToSpeckleId1toMany), schema, entity);
    }
    private void SaveSingleCache<T>(IDictionary<string, T> dict, string name, Schema schema, Entity entity)
      where T : class
    {
      var field = schema.GetField(name);
      IDictionary<string, string> storageDict;
      if (typeof(T) == typeof(List<string>))
      {
        storageDict = dict
          .Select(kvp => new KeyValuePair<string, string>(kvp.Key, string.Join(delimiter.ToString(), kvp.Value as List<string>)))
          .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
      }
      else if (dict is Dictionary<string, string> typed)
      {
        storageDict = typed;
      }
      else
      {
        throw new Exception($"Unexpected type {typeof(T)}, was initialized");
      }
      entity.Set<IDictionary<string, string>>(field, storageDict);
      document.ProjectInformation.SetEntity(entity);
    }
    public void RemoveSpeckleIdFromCache(string applicationId)
    {
      if (speckleIdToRevitId1to1.TryGetValue(applicationId, out var revitId))
      {
        revitIdToSpeckleId1to1.Remove(revitId);
        speckleIdToRevitId1to1.Remove(applicationId);
      }
      else if (speckleIdToRevitId1toMany.TryGetValue(applicationId, out var revitIds))
      {
        //TODO
      }
    }
  }
}
