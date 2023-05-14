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
  internal class ExtensibleStorageCache : IReceivedObjectsCache, IDisposable
  {
    Schema Schema { get; set; }
    public ExtensibleStorageCache() 
    {
      using var schemaBuilder = new SchemaBuilder(new Guid("b5305bdb-8877-4cd8-b2f4-a8f704038afc"));

      schemaBuilder.SetReadAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetWriteAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetVendorId("Speckle!");
      schemaBuilder.SetSchemaName("ExtensibleStorageSchema");

      var fieldBuilder = schemaBuilder.AddMapField("ElementsDict", typeof(string), typeof(string));
      Schema = schemaBuilder.Finish();
    }
    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId)
    {
      using var entity = new Entity(Schema);
      var dict = entity.Get<IDictionary<string,string>>(Schema.GetField("ElementsDict"));
      
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
}
