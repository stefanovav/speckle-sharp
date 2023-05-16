#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using ConnectorRevit.Caching.ExtensibleStorageCache;
using RevitSharedResources.Interfaces;
using Speckle.Core.Models;

namespace ConnectorRevit.Storage
{
  public class ExtensibleStorageCache : IReceivedObjectsCache
  {
    public string StreamId;
    private Dictionary<string, ExtensibleStorageDocumentData> dataStore = new();

    public ExtensibleStorageDocumentData Document(Document doc)
    {
      if (dataStore.TryGetValue(DocumentId(doc), out var documentData))
      {
        return documentData;
      }
      else
      {
        dataStore[DocumentId(doc)] = new ExtensibleStorageDocumentData(doc);
        return dataStore[DocumentId(doc)];
      }
    }
    public ICollection<ExtensibleStorageDocumentData> Documents()
    {
      return dataStore.Values;
    }

    public static string DocumentId(Document doc)
    {
      return doc.ProjectInformation.UniqueId;
    }

    public Element? GetExistingElementFromApplicationId(Document doc, string applicationId)
    {
      var revitId = Document(doc).Stream(StreamId).GetRevitIdFromSpeckleId(applicationId);
      if (!string.IsNullOrEmpty(revitId))
      {
        return doc.GetElement(revitId);
      }
      return null;
    }

    public IEnumerable<Element> GetExistingElementsFromApplicationId(Document doc, string applicationId)
    {
      var revitIds = Document(doc).Stream(StreamId).GetRevitIdsFromSpeckleId(applicationId);
      foreach (var revitId in revitIds)
      {
        if (!string.IsNullOrEmpty(revitId))
        {
          yield return doc.GetElement(revitId);
        }
      }
    }

    public void AddElementToCache(Element element, Base @base)
    {
      Document(element.Document).Stream(StreamId).Add1to1(@base, element);
    }

    public void RemoveSpeckleIdFromCache(Document doc, string applicationId)
    {
      Document(doc).Stream(StreamId).RemoveSpeckleIdFromCache(applicationId);
    }

    public void SaveCache()
    {
      foreach (var doc in Documents())
      {
        foreach (var stream in doc.Streams())
        {
          stream.SaveCache();
        }
      }
    }
    public void SaveSingleCache(Document doc)
    {
      Document(doc).Stream(StreamId).SaveCache();
    }

    public ICollection<string> GetAllApplicationIds(Document doc)
    {
      throw new NotImplementedException();
    }
    public IEnumerable<string> GetApplicationIds(Document doc, string streamId)
    {
      return Document(doc).Stream(streamId).GetAllSpeckleIds();
    }
  }
}
