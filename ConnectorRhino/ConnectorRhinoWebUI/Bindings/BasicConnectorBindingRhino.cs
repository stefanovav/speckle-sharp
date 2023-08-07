using System.Collections.Generic;
using System.Linq;
using ConnectorRhinoWebUI.App;
using DUI3;
using DUI3.App;
using DUI3.Bindings;
using DUI3.Models;
using DUI3.State;
using Rhino;
using Speckle.Core.Credentials;
using Speckle.Newtonsoft.Json;

namespace ConnectorRhinoWebUI.Bindings;

public class BasicConnectorBindingRhino : IBasicConnectorBinding
{
  public string Name { get; set; } = "baseBinding";
  public IBridge Parent { get; set; }
  public SpeckleApp App { get; }

  private DocumentState _documentState;
  private List<ModelCard> _modelCards;
  
  public BasicConnectorBindingRhino(SpeckleApp app)
  {
    this.App = app;

    RhinoDoc.BeginSaveDocument += (_, _) => WriteDocState();
    RhinoDoc.CloseDocument += (_, _) => WriteDocState();
    RhinoDoc.EndOpenDocumentInitialViewUpdate += (sender, e) =>
    {
      RhinoApp.WriteLine("BasicConnectorBindingRhino -> EndOpenDocumentInitialViewUpdate");
      if (e.Merge) return;
      if (e.Document == null) return;
      ReadDocState(e.Document);
      Parent?.SendToBrowser(BasicConnectorBindingEvents.DocumentChanged);
    };
    
    // NOTE: this fires quite a few times. We should debounce it
    RhinoDoc.LayerTableEvent += (sender, e) =>
    {
      Parent?.SendToBrowser(BasicConnectorBindingEvents.FiltersNeedRefresh);
    };
    _modelCards = app.RhinoAppState.SpeckleState.ModelCards;
    _documentState = new DocumentState();
    _documentState.Models = _modelCards;
  }

  public string GetSourceApplicationName()
  {
    return "Rhino";
  }

  public string GetSourceApplicationVersion()
  {
    return "7";
  }

  public Account[] GetAccounts()
  {
    return this.App.RhinoAppState.UserState.Accounts.ToArray();
  }

  public DocumentInfo GetDocumentInfo()
  {
    return new DocumentInfo
    {
      Location = RhinoDoc.ActiveDoc.Path,
      Name = RhinoDoc.ActiveDoc.Name,
      Id = RhinoDoc.ActiveDoc.RuntimeSerialNumber.ToString()
    };
  }

  public DocumentState GetDocumentState()
  {
    return _documentState;
  }

  public void AddModelToDocumentState(ModelCard model)
  {
    _documentState.Models.Add(model);
    WriteDocState();
  }

  public void UpdateModelInDocumentState(ModelCard model)
  {
    // var idx = _documentState.Models.FindIndex(m => model.Id == m.Id);
    // _documentState.Models[idx] = model;
    // TODO: implement
    WriteDocState();
  }
  
  public void RemoveModelFromDocumentState(ModelCard model)
  {
    var index = _documentState.Models.FindIndex(m => m.Id == model.Id);
    _documentState.Models.RemoveAt(index);
    WriteDocState();
  }

  public Dictionary<string, object> GetSendFilters_OLD()
  {
    var dict = new Dictionary<string, object>()
    {
      { KnownSendFilterTypeKeyNames.Everything, new RhinoEverythingFilter() },
      { KnownSendFilterTypeKeyNames.Selection, new RhinoSelectionFilter() },
      { KnownSendFilterTypeKeyNames.Layers, new RhinoLayerFilter() },
    };

    return dict;
  }

  public List<SendFilter> GetSendFilters()
  {
    return new List<SendFilter>()
    {
      new RhinoEverythingFilter() { Name = "Everything" },
      new RhinoSelectionFilter() { Name = "Selection" },
      new RhinoLayerFilter() { Name = "Layers" },
      new RhinoBlocksFilter() { Name = "Test Filter", Summary = "Not usable, do not implement." }
    };
  }

  private const string SpeckleKey = "Speckle_DUI3";
  /// <summary>
  /// Writes the _documentState to the current document info.
  /// </summary>
  private void WriteDocState()
  {
    if (RhinoDoc.ActiveDoc == null)
    {
      return; // Should throw
    }
    RhinoDoc.ActiveDoc?.Strings.Delete(SpeckleKey);
    var serializedState = _documentState.Serialize();
    RhinoDoc.ActiveDoc?.Strings.SetString(SpeckleKey, _documentState.Serialize());
  }
  
  /// <summary>
  /// Populates the _documentState from the current document info.
  /// </summary>
  private void ReadDocState(RhinoDoc doc)
  {
    var strings = RhinoDoc.ActiveDoc.Strings.GetValue(SpeckleKey);
    if(strings==null || strings.Length < 1) return;
    var state = DocumentState.Deserialize(strings);
    _documentState = state;
  }
}

