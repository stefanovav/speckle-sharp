using System.Collections.Generic;
using Speckle.Core.Plugins;

namespace SpeckleRhino.State
{
  public class AppState : IRhinoAppState
  {
    public IUserState UserState { get; }
    public IRhinoState RhinoState { get; }
    public ISpeckleState SpeckleState { get; }
    public IEnumerable<(string, string)> MessageQueue { get; }

    public AppState(IUserState userState, IRhinoState rhinoState, ISpeckleState speckleState, IEnumerable<(string, string)> messageQueue)
    {
      this.UserState = userState;
      this.RhinoState = rhinoState;
      this.SpeckleState = speckleState;
      this.MessageQueue = messageQueue;
    }

    public IAppState WithMessageQueue(IEnumerable<(string, string)> newMessageQueue)
    {
      return new AppState(this.UserState, this.RhinoState, this.SpeckleState, newMessageQueue);
    }
  }
}
