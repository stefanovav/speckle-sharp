using System.Collections.Generic;
using System.Linq;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using DesktopUI2;
using DesktopUI2.Models;
using DesktopUI2.Models.Settings;
using Speckle.ConnectorAutocadCivil.Storage;
using Speckle.Core.Kits;
using Speckle.Core.Models;

#if ADVANCESTEEL2023
using ASFilerObject = Autodesk.AdvanceSteel.CADAccess.FilerObject;
#endif

namespace Speckle.ConnectorAutocadCivil.UI
{
  public partial class ConnectorBindingsAutocad : ConnectorBindings
  {
    public static Document Doc => Application.DocumentManager.MdiActiveDocument;

    /// <summary>
    /// Stored Base objects from commit flattening on receive: key is the Base id
    /// </summary>
    public Dictionary<string, Base> StoredObjects = new Dictionary<string, Base>();

    /// <summary>
    /// Stored document line types used for baking objects on receive
    /// </summary>
    public Dictionary<string, ObjectId> LineTypeDictionary = new Dictionary<string, ObjectId>();

    public List<string> GetLayers()
    {
      var layers = new List<string>();
      foreach (var docLayer in Application.UIBindings.Collections.Layers)
      {
        var name = docLayer.GetProperties().Find("Name", true).GetValue(docLayer);
        layers.Add(name as string);
      }
      return layers;
    }

    // AutoCAD API should only be called on the main thread.
    // Not doing so results in botched conversions for any that require adding objects to Document model space before modifying (eg adding vertices and faces for meshes)
    // There's no easy way to access main thread from document object, therefore we are creating a control during Connector Bindings constructor (since it's called on main thread) that allows for invoking worker threads on the main thread
    public System.Windows.Forms.Control Control;

    public ConnectorBindingsAutocad()
      : base()
    {
      Control = new System.Windows.Forms.Control();
      Control.CreateControl();
    }

    public override List<ReceiveMode> GetReceiveModes()
    {
      return new List<ReceiveMode> { ReceiveMode.Create, ReceiveMode.Update };
    }

    #region local streams
    public override void WriteStreamsToFile(List<StreamState> streams)
    {
      SpeckleStreamManager.WriteStreamStateList(Doc, streams);
    }

    public override List<StreamState> GetStreamsInFile()
    {
      var streams = new List<StreamState>();
      if (Doc != null)
        streams = SpeckleStreamManager.ReadState(Doc);
      return streams;
    }
    #endregion

    public override string GetHostAppNameVersion() =>
      Utils.VersionedAppName
        .Replace("AutoCAD", "AutoCAD ")
        .Replace("Civil3D", "Civil 3D ")
        .Replace("AdvanceSteel", "Advance Steel "); //hack for ADSK store;

    public override string GetHostAppName() => Utils.Slug;

    private string GetDocPath(Document doc) =>
      HostApplicationServices.Current.FindFile(doc?.Name, doc?.Database, FindFileHint.Default);

    public override string GetDocumentId()
    {
      string path = null;
      try
      {
        path = GetDocPath(Doc);
      }
      catch { }
      var docString = $"{(path != null ? path : "")}{(Doc != null ? Doc.Name : "")}";
      var hash = !string.IsNullOrEmpty(docString)
        ? Core.Models.Utilities.HashString(docString, Core.Models.Utilities.HashingFunctions.MD5)
        : null;
      return hash;
    }

    public override string GetDocumentLocation() => GetDocPath(Doc);

    public override string GetFileName() => (Doc != null) ? System.IO.Path.GetFileName(Doc.Name) : string.Empty;

    public override string GetActiveViewName() => "Entire Document";

    private List<ISetting> CurrentSettings { get; set; } // used to store the Stream State settings when sending/receiving

    // CAUTION: these strings need to have the same values as in the converter
    const string InternalOrigin = "Internal Origin (default)";
    const string UCS = "Current User Coordinate System";

    public override List<ISetting> GetSettings()
    {
      List<string> referencePoints = new List<string>() { InternalOrigin };

      // add the current UCS if it exists
      if (Doc.Editor.CurrentUserCoordinateSystem != null)
        referencePoints.Add(UCS);

      // add any named UCS if they exist
      var namedUCS = new List<string>();
      using (Transaction tr = Doc.Database.TransactionManager.StartTransaction())
      {
        var UCSTable = tr.GetObject(Doc.Database.UcsTableId, OpenMode.ForRead) as UcsTable;
        foreach (var entry in UCSTable)
        {
          var ucs = tr.GetObject(entry, OpenMode.ForRead) as UcsTableRecord;
          namedUCS.Add(ucs.Name);
        }
        tr.Commit();
      }
      if (namedUCS.Any())
        referencePoints.AddRange(namedUCS);

      return new List<ISetting>
      {
        new ListBoxSetting
        {
          Slug = "reference-point",
          Name = "Reference Point",
          Icon = "LocationSearching",
          Values = referencePoints,
          Selection = InternalOrigin,
          Description = "Sends or receives stream objects in relation to this document point"
        },
      };
    }

    //TODO
    public override List<MenuItem> GetCustomStreamMenuItems()
    {
      return new List<MenuItem>();
    }

    public override void ResetDocument()
    {
      Doc.Editor.SetImpliedSelection(new ObjectId[0]);
      Autodesk.AutoCAD.Internal.Utils.FlushGraphics();
    }
  }
}
