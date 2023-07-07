using System;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Action has a basic functionality to only update state immutably.
  /// Actions might call actions in UpdateState function nestedly.
  /// </summary>
  public interface IAction
  {
    /// <summary>
    /// Gets an old state and returns new one (or same).
    /// </summary>
    /// <param name="state"> State to update (or not).</param>
    /// <param name="resolveId"> Id to understand which command is calling.</param>
    /// <returns> Updated AppState instance.</returns>
    public IAppState UpdateState(IAppState state, Guid resolveId);
  }
}
