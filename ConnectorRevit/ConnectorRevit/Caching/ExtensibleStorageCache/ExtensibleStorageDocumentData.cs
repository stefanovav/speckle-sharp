using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace ConnectorRevit.Caching.ExtensibleStorageCache
{
  public class ExtensibleStorageDocumentData
  {
    private Document document;

    public ExtensibleStorageDocumentData(Document document)
    {
      this.document = document;
    }

    private Dictionary<string, ExtensibleStorageDocumentStreamData> dataStore = new();

    public ExtensibleStorageDocumentStreamData Stream(string streamId)
    {
      if (dataStore.TryGetValue(streamId, out var streamData))
      {
        return streamData;
      }
      else
      {
        dataStore[streamId] = new ExtensibleStorageDocumentStreamData(document, streamId);
        return dataStore[streamId];
      }
    }

    public ICollection<ExtensibleStorageDocumentStreamData> Streams()
    {
      return dataStore.Values;
    }
  }
}
