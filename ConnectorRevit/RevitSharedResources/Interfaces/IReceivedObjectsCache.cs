#nullable enable
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace RevitSharedResources.Interfaces
{
  internal interface IReceivedObjectsCache
  {
    public string? GetRevitIdFromApplicationId(string applicationId);
    public List<string> GetRevitIdsFromApplicationId(string applicationId);
  }
}
