using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DUI3.State;

namespace ConnectorRhinoWebUI.State
{
  /// <summary>
  /// AppState for Rhino.
  /// Implement here if you have Rhino specific states to handle.
  /// </summary>
  public interface IRhinoAppState : IAppState
  {
    /// <inheritdoc cref="State.RhinoDocumentState"/> 
    public RhinoDocumentState RhinoDocumentState { get; }
  }
}
