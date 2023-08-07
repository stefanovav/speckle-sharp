using System.Collections.Generic;
using ConnectorRhinoWebUI.State;
using DUI3;
using DUI3.App;
using DUI3.State;

namespace ConnectorRhinoWebUI.App
{
  public class SpeckleApp : IApp
  {
    public List<IBinding> Bindings { get; } = new List<IBinding>();

    public IRhinoAppState RhinoAppState { get; }

    IAppState IApp.AppState
    {
      get
      {
        return RhinoAppState;
      }
    }

    public SpeckleApp(IRhinoAppState appState)
    {
      this.RhinoAppState = appState;
    }
  }
}
