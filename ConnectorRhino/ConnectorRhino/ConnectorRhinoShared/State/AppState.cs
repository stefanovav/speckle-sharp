using System.Collections.Generic;

namespace SpeckleRhino.State
{
  public class AppState
  {
    public UserState UserState { get; }
    public RhinoState RhinoState { get; }
    public SpeckleState SpeckleState { get; }
    public List<(string, string)> MessageQueue { get; }

    public AppState(UserState userState, RhinoState rhinoState, SpeckleState speckleState, List<(string, string)> messageQueue)
    {
      this.UserState = userState;
      this.RhinoState = rhinoState;
      this.SpeckleState = speckleState;
      this.MessageQueue = messageQueue;
    }

    public AppState WithMessageQueue(List<(string, string)> newMessageQueue)
    {
      return new AppState(this.UserState, this.RhinoState, this.SpeckleState, newMessageQueue);
    }
  }
}
