#nullable enable
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitSharedResources.Interfaces
{
  internal interface IReceivedObjectsCache
  {
    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId);
    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId);
  }
}
