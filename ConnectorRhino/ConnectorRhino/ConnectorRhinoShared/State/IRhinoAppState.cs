using Speckle.Core.Connectors;

namespace SpeckleRhino.State
{
  /// <summary>
  /// AppState for Rhino.
  /// Implement here if you have Rhino specific states to handle.
  /// </summary>
  public interface IRhinoAppState : IAppState
  {
    /// <inheritdoc cref="IRhinoState"/> 
    public IRhinoState RhinoState { get; }
  }
}
