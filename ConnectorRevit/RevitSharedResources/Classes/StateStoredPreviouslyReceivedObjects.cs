#nullable enable
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace RevitSharedResources.Classes
{
  public sealed class StateStoredPreviouslyReceivedObjects : IReceivedObjectsCache
  {
    public Dictionary<string, ApplicationObject> PreviousContextObjects;
    public StateStoredPreviouslyReceivedObjects(List<ApplicationObject> objects)
    {
      PreviousContextObjects = new(objects.Count);
      foreach (var ao in objects)
      {
        var key = ao.applicationId ?? ao.OriginalId;
        if (PreviousContextObjects.ContainsKey(key))
          continue;
        PreviousContextObjects.Add(key, ao);
      }
    }

    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId)
    {
      if (PreviousContextObjects.TryGetValue(applicationId, out var appId))
      {
        foreach (var id in appId.CreatedIds)
        {
          yield return doc.GetElement(id);
        }
      }
      yield return doc.GetElement(applicationId);
    }

    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId)
    {
      if (PreviousContextObjects.TryGetValue(applicationId, out var appId))
      {
        return doc.GetElement(appId.CreatedIds.FirstOrDefault());
      }
      return doc.GetElement(applicationId);
    }

    public void AddElementToCache(Base @base, Element element)
    {
      throw new System.NotImplementedException();
    }

    public void SaveCache()
    {
      throw new System.NotImplementedException();
    }

    public void RemoveBaseFromCache(Document doc, string applicationId)
    {
      throw new System.NotImplementedException();
    }

    public IEnumerable<string> GetAllApplicationIds(Document doc)
    {
      throw new System.NotImplementedException();
    }

    ICollection<string> IReceivedObjectsCache.GetAllApplicationIds(Document doc)
    {
      throw new System.NotImplementedException();
    }
  }
}
