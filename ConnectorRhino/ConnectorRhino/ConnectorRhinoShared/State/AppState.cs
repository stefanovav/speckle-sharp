using System;
using System.Collections.Generic;
using System.Text;

namespace SpeckleRhino.State
{
  public class AppState
  {
    public UserState UserState { get; }
    public RhinoState RhinoState { get; }
    public SpeckleState SpeckleState { get; }

    public AppState(UserState userState, RhinoState rhinoState, SpeckleState speckleState)
    {
      this.UserState = userState;
      this.RhinoState = rhinoState;
      this.SpeckleState = speckleState;
    }
  }
}
