#nullable enable
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace RevitSharedResources.Interfaces
{
  public interface IReceivedObjectsCache
  {
    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId);
    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId);
    public void AddElementToCache(Base @base, Element element);
    public void RemoveBaseFromCache(Document doc, string applicationId);
    public void SaveCache();
    public ICollection<string> GetAllApplicationIds(Document doc);
  }
}
