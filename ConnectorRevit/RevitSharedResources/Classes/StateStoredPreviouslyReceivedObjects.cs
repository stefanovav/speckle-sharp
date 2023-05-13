using System.Collections.Generic;
using System.Linq;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace RevitSharedResources.Classes
{
  internal sealed class StateStoredPreviouslyReceivedObjects : IReceivedObjectsCache
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
    public List<string> GetRevitIdsFromApplicationId(string applicationId)
    {
      if (PreviousContextObjects.TryGetValue(applicationId, out var appId))
      {
        return appId.CreatedIds;
      }
      return new List<string>();
    }

    public string? GetRevitIdFromApplicationId(string applicationId)
    {
      if (PreviousContextObjects.TryGetValue(applicationId, out var appId))
      {
        return appId.CreatedIds.FirstOrDefault();
      }
      return null;
    }
  }
}
