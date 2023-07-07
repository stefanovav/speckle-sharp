using System.Collections.Generic;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Speckle state is a bridge between host app and speckle related info.
  /// </summary>
  public interface ISpeckleState
  {
    /// <summary>
    /// Gets speckle objects that converted before.
    /// TBD: We somehow should have here a definition of object that holds also info about it is VALID or INVALID.
    ///   I guess similar idea with ApplicationObject.
    /// </summary>
    public List<object> SpeckleObjects { get; }
  }
}
