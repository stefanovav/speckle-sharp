#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace RevitSharedResources.Classes
{
  public class ExtensibleStorageCache : IReceivedObjectsCache, IDisposable
  {
    Schema Schema { get; set; }
    public ExtensibleStorageCache() 
    {
      Schema = ExtensibleStorageCacheSchema.GetSchema();
    }
    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId)
    {
      using var entity = doc.ProjectInformation.GetEntity(Schema);

      IDictionary<string, string> dict;
      try
      {
        dict = entity.Get<IDictionary<string,string>>(Schema.GetField("ElementsDict"));
      }
      catch
      {
        return null;
      }
      
      if (dict.TryGetValue(applicationId, out var elementId)) 
      { 
        return doc.GetElement(elementId);
      }
      return null;
    }

    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId)
    {
      throw new NotImplementedException();
    }

    public void AddElementToCache(Base @base, Element element)
    {
      using var entity = new Entity(Schema);
      var field = Schema.GetField("ElementsDict");

      IDictionary<string, string> idMap = new Dictionary<string, string>();
      idMap.Add(@base.applicationId, element.UniqueId);

      entity.Set<IDictionary<string, string>>(field, idMap);

      element.Document.ProjectInformation.SetEntity(entity);
    }

    public void Dispose()
    {
      Schema.Dispose();
    }
  }

  /// <summary>
  /// Unique schema for... something ¯\_(ツ)_/¯
  /// </summary>
  static class ExtensibleStorageCacheSchema
  {
    static readonly Guid schemaGuid = new Guid("b5305bdb-8877-4cd8-b2f4-a8f704038afc");

    public static Schema GetSchema()
    {
      Schema schema = Schema.Lookup(schemaGuid);

      if (schema != null)
        return schema;

      //SchemaBuilder schemaBuilder = new SchemaBuilder(schemaGuid);

      //schemaBuilder.SetSchemaName("DataStorageUniqueId");

      //schemaBuilder.AddSimpleField("Id", typeof(Guid));

      //return schemaBuilder.Finish();
      var schemaBuilder = new SchemaBuilder(schemaGuid);
      schemaBuilder.SetReadAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetWriteAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetVendorId("speckle");
      schemaBuilder.SetSchemaName("ExtensibleStorageSchema");

      var fieldBuilder = schemaBuilder.AddMapField("ElementsDict", typeof(string), typeof(string));
      return schemaBuilder.Finish();
    }
  }
}
