using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using DesktopUI2.Models;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace ConnectorRevit.Storage
{
  public class StreamStateCache : IReceivedObjectIdMap<Base, Element>
  {
    private StreamState streamState;
    private Dictionary<string, ApplicationObject> previousContextObjects;

    public StreamStateCache(StreamState state)
    {
      streamState = state;
      var previousObjects = state.ReceivedObjects;
      previousContextObjects = new(previousObjects.Count);
      foreach (var ao in previousObjects)
      {
        var key = ao.applicationId ?? ao.OriginalId;
        if (previousContextObjects.ContainsKey(key))
          continue;
        previousContextObjects.Add(key, ao);
      }
    }

    public void AddConvertedElements(IConvertedObjectsCache<Base, Element> convertedObjects)
    {
      var newContextObjects = new List<ApplicationObject>();
      foreach (var @base in convertedObjects.GetConvertedObjects())
      {
        var elements = convertedObjects.GetCreatedObjectsFromConvertedId(@base.applicationId).ToList();
        var appObj = new ApplicationObject(@base.id, @base.speckle_type);

        newContextObjects.Add(
          new ApplicationObject(@base.id, @base.speckle_type)
          {
            applicationId = @base.applicationId,
            CreatedIds = elements
              .Select(element =>
              {
                try
                {
                  return element.UniqueId;
                }
                catch (Exception e)
                {
                  // UniqueId can throw if the element it references is no longer in the document database.
                  // We don't want to add these to the cache regardless, so we're filtering them out here.
                  return null;
                }
              })
              .Where(e => e != null)
              .ToList(),
            Converted = elements.Cast<object>().ToList()
          }
        );
      }
      streamState.ReceivedObjects = newContextObjects;
    }

    public IEnumerable<string> GetAllConvertedIds()
    {
      return previousContextObjects.Keys;
    }

    public IEnumerable<string> GetCreatedIdsFromConvertedId(string id)
    {
      if (previousContextObjects.TryGetValue(id, out var appObj) && appObj.CreatedIds.Count > 0)
      {
        return appObj.CreatedIds;
      }
      return new[] { id };
    }

    public void RemoveConvertedId(string id)
    {
      // no need to remove as this cache get written over by a new one after each receive
      //previousContextObjects.Remove(id);
    }
  }
}
