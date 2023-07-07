using Rhino;

namespace SpeckleRhino.State
{
  /// <summary>
  /// State of the Rhino.
  /// Collects RhinoDoc.
  /// TODO: Might be more implementations for Rhino related objects and events.. 
  /// </summary>
  public interface IRhinoState
  {
    /// <inheritdoc cref="RhinoDoc"/>
    public RhinoDoc Doc { get; }
  }
}
