using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using DesktopUI2.Models.Settings;
using RevitSharedResources.Interfaces;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace ConnectorRevit.Extensions
{
  public static class ISpeckleConverterExtensions
  {
    private static void Initialize(this ISpeckleConverter converter, List<ApplicationObject> contextObjects, params object[] contextDocs)
    {
      converter.SetContextObjects(contextObjects);
      foreach (var obj in contextDocs)
      {
        converter.SetContextDocument(obj);
      }
    }

    public static void InitializeForReceive(this ISpeckleConverter converter, List<ApplicationObject> contextObjects, ReceiveMode receiveMode, IReceivedObjectIdMap<Base, Element> map, Transaction transaction)
    {
      converter.Initialize(contextObjects, map, transaction);
      converter.ReceiveMode = receiveMode;
    }

    public static void InitializeForSend(this ISpeckleConverter converter, List<ApplicationObject> contextObjects, params object[] contextDocs)
    {
      converter.Initialize(contextObjects, contextDocs);
    }
  }
}
