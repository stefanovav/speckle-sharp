using System.Collections.Generic;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Single source of truth about state of the application.
  /// Inherited class should be managed immutably always because Application will be compare reference of the objects to switch them.
  /// Update state approach via this object instances has a similar manner on updating Virtual DOM with the Browser DOM by comparison.
  /// So AppState instance will only updated on App if there is any change at the end of the command that called by UI.
  /// </summary>
  public interface IAppState
  {
    /// <inheritdoc cref="ISpeckleState"/>
    public ISpeckleState SpeckleState { get; }

    /// <inheritdoc cref="IUserState"/>
    public IUserState UserState { get; }

    /// <summary>
    /// Gets messages to be ready to send UI after commands done.
    /// </summary>
    public IEnumerable<UIMessage> MessageQueue { get; }

    /// <summary>
    /// Immutable function to replace message queue with a new AppState object.
    /// </summary>
    /// <param name="newMessageQueue"> new message queue to replace. </param>
    /// <returns> New AppState instance that created with new message queue. </returns>
    public IAppState WithMessageQueue(IEnumerable<UIMessage> newMessageQueue);
  }
}
