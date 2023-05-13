#nullable enable
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitSharedResources.Interfaces
{
  internal interface IReceivedObjectsCache
  {
    public Element? GetElementWithMappedUniqueId(string id);
    public List<Element> GetElementsWithMappedUniqueId(string id);
  }
}
