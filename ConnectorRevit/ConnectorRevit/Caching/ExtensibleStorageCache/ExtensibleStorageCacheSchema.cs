using System;
using System.Collections.Generic;
using Autodesk.Revit.DB.ExtensibleStorage;

namespace ConnectorRevit.Caching.ExtensibleStorageCache
{
  static class ExtensibleStorageCacheSchema
  {
    static readonly Guid schemaGuid = new Guid("b5305bdb-8877-4cd8-b2f4-a8f704038afc");

    public static Schema GetSchema(List<string> fields)
    {
      Schema schema = Schema.Lookup(schemaGuid);

      if (schema != null)
        return schema;

      var schemaBuilder = new SchemaBuilder(schemaGuid);
      schemaBuilder.SetReadAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetWriteAccessLevel(AccessLevel.Vendor);
      schemaBuilder.SetVendorId("speckle");
      schemaBuilder.SetSchemaName("ExtensibleStorageSchema");

      foreach (var field in fields)
      {
        schemaBuilder.AddMapField(field, typeof(string), typeof(string));
      }
      return schemaBuilder.Finish();
    }
  }
}
