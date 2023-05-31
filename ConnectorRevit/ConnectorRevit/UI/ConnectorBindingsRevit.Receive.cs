using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Avalonia.Threading;
using ConnectorRevit.Extensions;
using ConnectorRevit.Revit;
using ConnectorRevit.Storage;
using DesktopUI2;
using DesktopUI2.Models;
using DesktopUI2.Models.Settings;
using DesktopUI2.ViewModels;
using Revit.Async;
using RevitSharedResources.Interfaces;
using Serilog;
using Speckle.Core.Api;
using Speckle.Core.Kits;
using Speckle.Core.Logging;
using Speckle.Core.Models;
using Speckle.Core.Models.GraphTraversal;

namespace Speckle.ConnectorRevit.UI
{

  public partial class ConnectorBindingsRevit
  {
    /// <summary>
    /// Receives a stream and bakes into the existing revit file.
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    ///
    public override async Task<StreamState> ReceiveStream(StreamState state, ProgressViewModel progress)
    {
      //make sure to instance a new copy so all values are reset correctly
      var converter = ConnectorRevitUtils.CreateConverter(Converter.GetType(), CurrentDoc.Document, state.Settings);

      Commit myCommit = await ConnectorHelpers.GetCommitFromState(progress.CancellationToken, state);
      state.LastCommit = myCommit;
      Base commitObject = await ConnectorHelpers.ReceiveCommit(myCommit, state, progress);
      await ConnectorHelpers.TryCommitReceived(progress.CancellationToken, state, myCommit, ConnectorRevitUtils.RevitAppName);
      

      var previousObjects = new StreamStateCache(state);
      _ = await BakeFlattenedCommit(converter, commitObject, previousObjects, state.Settings, state.ReceiveMode, state.StreamId, myCommit.sourceApplication, progress, TryBakeObject).ConfigureAwait(false);

      return state;
    }

    public static async Task<IConvertedObjectsCache<Base, Element>> BakeFlattenedCommit(
      ISpeckleConverter converter, 
      Base commitObject,
      IReceivedObjectIdMap<Base, Element> previousObjects, 
      List<ISetting> settings, 
      ReceiveMode receiveMode, 
      string streamId,
      string sourceApplication,
      ProgressViewModel progress,
      Func<ISpeckleConverter, Base, ApplicationObject, Task<object>> tryBake)
    {
      var storedObjects = new Dictionary<string, Base>();
      var preview = FlattenCommitObject(commitObject, converter, storedObjects);
      foreach (var previewObj in preview)
        progress.Report.Log(previewObj);

      try
      {
        await RevitTask.RunAsync(() => UpdateForCustomMapping(progress, sourceApplication, settings, preview, storedObjects)).ConfigureAwait(false);
      }
      catch (Exception ex)
      {
        SpeckleLog.Logger.Warning(ex, "Could not update receive object with user types");
        progress.Report.LogOperationError(new Exception("Could not update receive object with user types. Using default mapping.", ex));
      }

      var (convertedObjects, exception) = await RevitTask.RunAsync(async app =>
      {
        string transactionName = $"Baking stream {streamId}";
        using var g = new TransactionGroup(CurrentDoc.Document, transactionName);
        using var t = new Transaction(CurrentDoc.Document, transactionName);

        g.Start();
        var failOpts = t.GetFailureHandlingOptions();
        failOpts.SetFailuresPreprocessor(new ErrorEater(converter));
        failOpts.SetClearAfterRollback(true);
        t.SetFailureHandlingOptions(failOpts);
        t.Start();

        try
        {
          converter.InitializeForReceive(preview, receiveMode, previousObjects, t);

          var convertedObjects = await ConvertReceivedObjects(converter, progress, settings, preview, storedObjects, tryBake).ConfigureAwait(false);

          if (receiveMode == ReceiveMode.Update)
            DeleteObjects(previousObjects, convertedObjects);

          previousObjects.AddConvertedElements(convertedObjects);
          t.Commit();
          g.Assimilate();
          return (convertedObjects, null);
        }
        catch (Exception ex)
        {
          SpeckleLog.Logger.Error(ex, "Rolling back connector transaction {transactionName} {transactionType}", transactionName, t.GetType());

          string message = $"Fatal Error: {ex.Message}";
          if (ex is OperationCanceledException) message = "Receive cancelled";
          progress.Report.LogOperationError(new Exception($"{message} - Changes have been rolled back", ex));

          t.RollBack();
          g.RollBack();
          return (default(IConvertedObjectsCache<Base, Element>), ex); //We can't throw exceptions in from RevitTask, but we can return it along with a success status
        }
      }).ConfigureAwait(false);

      if (exception != null)
      {
        //Don't wrap cancellation token (if it's ours!)
        if (exception is OperationCanceledException && progress.CancellationToken.IsCancellationRequested) throw exception;
        throw new SpeckleException(exception.Message, exception);
      }

      return convertedObjects;
    }

    //delete previously sent object that are no more in this stream
    private static void DeleteObjects(IReceivedObjectIdMap<Base, Element> previousObjects, IConvertedObjectsCache<Base, Element> convertedObjects)
    {
      var previousAppIds = previousObjects.GetAllConvertedIds().ToList();
      for (var i = previousAppIds.Count - 1; i >=0; i--)
      {
        var appId = previousAppIds[i];
        if (string.IsNullOrEmpty(appId) || convertedObjects.HasConvertedObjectWithId(appId))
          continue;

        var elementIdToDelete = previousObjects.GetCreatedIdsFromConvertedId(appId);

        foreach (var elementId in elementIdToDelete)
        {
          var elementToDelete = CurrentDoc.Document.GetElement(elementId);

          if (elementToDelete != null) CurrentDoc.Document.Delete(elementToDelete.Id);
          previousObjects.RemoveConvertedId(appId);
        }
      }
    }

    private static async Task<IConvertedObjectsCache<Base, Element>> ConvertReceivedObjects(ISpeckleConverter converter, ProgressViewModel progress, List<ISetting> settings, List<ApplicationObject> preview, Dictionary<string, Base> storedObjects, Func<ISpeckleConverter, Base, ApplicationObject, Task<object>> tryBake)
    {
      var convertedObjectsCache = new ConvertedObjectsCache();
      var conversionProgressDict = new ConcurrentDictionary<string, int>();
      conversionProgressDict["Conversion"] = 1;

      // Get setting to skip linked model elements if necessary
      var receiveLinkedModelsSetting = settings.FirstOrDefault(x => x.Slug == "linkedmodels-receive") as CheckBoxSetting;
      var receiveLinkedModels = receiveLinkedModelsSetting != null ? receiveLinkedModelsSetting.IsChecked : false;
      foreach (var obj in preview)
      {
        var @base = storedObjects[obj.OriginalId];
        progress.CancellationToken.ThrowIfCancellationRequested();

        conversionProgressDict["Conversion"]++;
        progress.Update(conversionProgressDict);

        YeildToUIThread(TimeSpan.FromMilliseconds(1));
        RefreshView();

        //skip element if is from a linked file and setting is off
        if (!receiveLinkedModels && @base["isRevitLinkedModel"] != null && bool.Parse(@base["isRevitLinkedModel"].ToString()))
          continue;

        var convRes = await tryBake(converter, @base, obj).ConfigureAwait(true);

        switch (convRes)
        {
          case ApplicationObject o:
            if (o.Converted.Cast<Element>().ToList() is List<Element> typedList && typedList.Count >= 1)
            {
              convertedObjectsCache.AddConvertedObjects(@base, typedList);
            }
            obj.Update(status: o.Status, createdIds: o.CreatedIds, converted: o.Converted, log: o.Log);
            break;
          default:
            break;
        }
        progress.Report.UpdateReportObject(obj);
      }

      return convertedObjectsCache;
    }

    private static async Task<object> TryBakeObject(ISpeckleConverter converter, Base @base, ApplicationObject obj)
    {
      object converted = null;
      try
      {
        converted = converter.ConvertToNative(@base);
      }
      catch (Exception e)
      {
        SpeckleLog.Logger.Warning("Failed to convert ");
        obj.Update(status: ApplicationObject.State.Failed, logItem: e.Message);
      }
      return converted;
    }

    private static void RefreshView()
    {
      //regenerate the document and then implement a hack to "refresh" the view
      CurrentDoc.Document.Regenerate();

      // get the active ui view
      var view = CurrentDoc.ActiveGraphicalView ?? CurrentDoc.Document.ActiveView;
      if (view is TableView)
      {
        return;
      }

      var uiView = CurrentDoc.GetOpenUIViews().FirstOrDefault(uv => uv.ViewId.Equals(view.Id));

      // "refresh" the active view
      uiView.Zoom(1);
    }

    /// <summary>
    /// Traverses the object graph, returning objects to be converted.
    /// </summary>
    /// <param name="obj">The root <see cref="Base"/> object to traverse</param>
    /// <param name="converter">The converter instance, used to define what objects are convertable</param>
    /// <returns>A flattened list of objects to be converted ToNative</returns>
    public static List<ApplicationObject> FlattenCommitObject(Base obj, ISpeckleConverter converter, Dictionary<string, Base> storedObjects)
    {

      ApplicationObject CreateApplicationObject(Base current)
      {
        if (!converter.CanConvertToNative(current)) return null;

        var appObj = new ApplicationObject(current.id, ConnectorRevitUtils.SimplifySpeckleType(current.speckle_type))
        {
          applicationId = current.applicationId,
          Convertible = true
        };
        if (storedObjects.ContainsKey(current.id))
          return null;

        storedObjects.Add(current.id, current);
        return appObj;
      }

      var traverseFunction = DefaultTraversal.CreateRevitTraversalFunc(converter);

      var objectsToConvert = traverseFunction.Traverse(obj)
        .Select(tc => CreateApplicationObject(tc.current))
        .Where(appObject => appObject != null)
        .Reverse()
        .ToList();

      return objectsToConvert;
    }

  }
}
