using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.Revit.DB;
using RevitSharedResources.Interfaces;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace ConverterRevitShared.Classes
{
  internal class StateStoredPreviouslyReceivedObjects : IReceivedObjectsCache
  {
    public Dictionary<string, ApplicationObject> PreviousContextObjects;
    public Document Document;
    public StateStoredPreviouslyReceivedObjects(List<ApplicationObject> objects, Document doc)
    {
      PreviousContextObjects = new(objects.Count);
      foreach (var ao in objects)
      {
        var key = ao.applicationId ?? ao.OriginalId;
        if (PreviousContextObjects.ContainsKey(key))
          continue;
        PreviousContextObjects.Add(key, ao);
      }
      Document = doc;
    }
    public List<Element> GetElementsWithMappedUniqueId(string applicationId)
    {
      var elements = new List<Element>();
      if (!PreviousContextObjects.ContainsKey(applicationId))
      {
        //element was not cached in a PreviousContex but might exist in the model
        //eg: user sends some objects, moves them, receives them 
        var revElement = Document.GetElement(applicationId);
        if (revElement != null)
          elements.Add(revElement);
      }
      else
      {
        var @ref = PreviousContextObjects[applicationId];
        //return the cached objects, if they are still in the model
        foreach (var id in @ref.CreatedIds)
        {
          var revElement = Document.GetElement(id);
          if (revElement != null)
            elements.Add(revElement);
        }
      }

      return elements;
    }

    public Element GetElementWithMappedUniqueId(string applicationId)
    {
      Element element = null;
      if (!PreviousContextObjects.ContainsKey(applicationId))
      {
        //element was not cached in a PreviousContex but might exist in the model
        //eg: user sends some objects, moves them, receives them 
        element = Document.GetElement(applicationId);
      }
      else
      {
        var @ref = PreviousContextObjects[applicationId];
        //return the cached object, if it's still in the model
        if (@ref.CreatedIds.Any())
          element = Document.GetElement(@ref.CreatedIds.First());
      }

      return element;
    }
  }
}
