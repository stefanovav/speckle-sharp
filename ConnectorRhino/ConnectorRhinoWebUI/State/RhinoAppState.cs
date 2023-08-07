using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUI3.State;

namespace ConnectorRhinoWebUI.State
{
  public class RhinoAppState : IRhinoAppState
  {
    public UserState UserState { get; }

    public RhinoDocumentState RhinoDocumentState { get; }

    public ISpeckleState SpeckleState { get; }

    public RhinoAppState(UserState userState, ISpeckleState speckleState, RhinoDocumentState rhinoDocumentState)
    {
      SpeckleState = speckleState;
      UserState = userState;
      RhinoDocumentState = rhinoDocumentState;
    }
  }
}
