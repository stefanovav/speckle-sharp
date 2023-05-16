#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace ConnectorRevit.Caching.ExtensibleStorageCache
{
  internal interface IExtensibleStorageDataStore
  {
    public string? GetRevitIdInDocFromSpeckleId(Document doc, string speckleId);
  }
}
