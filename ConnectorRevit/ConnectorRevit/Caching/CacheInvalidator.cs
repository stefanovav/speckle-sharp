using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace ConnectorRevit.Caching
{
  internal class CacheInvalidator : IUpdater
  {
    private UpdaterId updaterId;

    public CacheInvalidator(AddInId addInId)
    {
      updaterId = new UpdaterId(addInId, new Guid("27c1808a-8dea-48e5-800e-6b91704b3e9a"));
    }

    public void Register(Document doc)
    {
      if (!UpdaterRegistry.IsUpdaterRegistered(updaterId))
      {
        UpdaterRegistry.RegisterUpdater(this, doc);
      }
    }

    public void Execute(UpdaterData data)
    {
      //var cache = new ExtensibleStorageCache.ExtensibleStorageCache();
      //Document doc = data.GetDocument();
      //var modifiedElements = data.GetModifiedElementIds();
      //doc.Regenerate();
    }

    public string GetAdditionalInformation()
    {
      throw new NotImplementedException();
    }

    public ChangePriority GetChangePriority()
    {
      throw new NotImplementedException();
    }

    public UpdaterId GetUpdaterId()
    {
      throw new NotImplementedException();
    }

    public string GetUpdaterName()
    {
      throw new NotImplementedException();
    }
  }
}
