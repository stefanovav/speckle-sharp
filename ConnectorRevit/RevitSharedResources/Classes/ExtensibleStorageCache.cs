#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using GraphQL;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace RevitSharedResources.Classes
{
  public sealed class ExtensibleStorageCache : IReceivedObjectsCache, IDisposable
  {
    private string fieldName { get; }
    private List<Document> Documents = new List<Document>();
    private Schema Schema { get; set; }

    private Dictionary<string, IDictionary<string, string>> speckleAppIdToRevitUniqueIdMap = new();
    public ExtensibleStorageCache(string streamId) 
    {
      fieldName = GetFieldName(streamId);
      Schema = ExtensibleStorageCacheSchema.GetSchema(fieldName);
    }
    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId)
    {
      var dict = GetIdMapFromDoc(doc);
      
      if (dict.TryGetValue(applicationId, out var elementId)) 
      { 
        return doc.GetElement(elementId);
      }
      return null;
    }

    private IDictionary<string, string> GetIdMapFromDoc(Document doc)
    {
      if (speckleAppIdToRevitUniqueIdMap.TryGetValue(doc.PathName, out IDictionary<string, string> value))
      {
        return value;
      }
      else
      {
        using var entity = doc.ProjectInformation.GetEntity(Schema);
        try
        {
          speckleAppIdToRevitUniqueIdMap[doc.PathName] = entity.Get<IDictionary<string, string>>(Schema.GetField(fieldName));
        }
        catch (Autodesk.Revit.Exceptions.ArgumentException)
        {
          // schema has not been set yet so the cache is empty
          speckleAppIdToRevitUniqueIdMap[doc.PathName] = new Dictionary<string, string>();
        }
        Documents.Add(doc);
        return speckleAppIdToRevitUniqueIdMap[doc.PathName];
      }
    }

    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId)
    {
      throw new NotImplementedException();
    }

    public void AddElementToCache(Base @base, Element element)
    {
      var idMap = GetIdMapFromDoc(element.Document);

      if (!string.IsNullOrEmpty(@base.applicationId))
      {
        idMap[@base.applicationId] = element.UniqueId;
      }
    }

    public void SaveCache()
    {
      foreach (var doc in Documents)
      {
        using var entity = new Entity(Schema);
        var field = Schema.GetField(fieldName);
        var idMap = GetIdMapFromDoc(doc);

        entity.Set<IDictionary<string, string>>(field, idMap);
        doc.ProjectInformation.SetEntity(entity);
      }
    }

    private static string GetFieldName(string streamId)
    {
      return streamId;

      //TODO: it would be nice to add the branchId as below
      //return $"{streamId}-{branchId}";
    }

    public void Dispose()
    {
      Schema.Dispose();
    }
  }

  /// <summary>
  /// Unique schema for extensible storage
  /// </summary>
  static class ExtensibleStorageCacheSchema
  {
    static readonly Guid schemaGuid = new Guid("b5305bdb-8877-4cd8-b2f4-a8f704038afc");

    public static Schema GetSchema(string fieldName)
    {
      Schema schema = Schema.Lookup(schemaGuid);

      if (schema != null)
        return schema;

      var schemaBuilder = new SchemaBuilder(schemaGuid);
      schemaBuilder.SetReadAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetWriteAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetVendorId("speckle");
      schemaBuilder.SetSchemaName("ExtensibleStorageSchema");

      var fieldBuilder = schemaBuilder.AddMapField(fieldName, typeof(string), typeof(string));
      return schemaBuilder.Finish();
    }
  }
}
