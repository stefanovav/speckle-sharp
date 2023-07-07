using System.Collections.Generic;
using Speckle.Core.Connectors;

namespace SpeckleRhino.State
{
  /// <inheritdoc cref="IRhinoAppState"/>
  public class AppState : IRhinoAppState
  {
    /// <inheritdoc cref="IUserState"/>
    public IUserState UserState { get; }

    /// <inheritdoc cref="IRhinoState"/>
    public IRhinoState RhinoState { get; }

    /// <inheritdoc cref="ISpeckleState"/>
    public ISpeckleState SpeckleState { get; }

    /// <inheritdoc/>
    public IEnumerable<UIMessage> MessageQueue { get; }

    public AppState(IUserState userState, IRhinoState rhinoState, ISpeckleState speckleState, IEnumerable<UIMessage> messageQueue)
    {
      this.UserState = userState;
      this.RhinoState = rhinoState;
      this.SpeckleState = speckleState;
      this.MessageQueue = messageQueue;
    }

    /// <inheritdoc/>
    public IAppState WithMessageQueue(IEnumerable<UIMessage> newMessageQueue)
    {
      return new AppState(this.UserState, this.RhinoState, this.SpeckleState, newMessageQueue);
    }
  }
}
