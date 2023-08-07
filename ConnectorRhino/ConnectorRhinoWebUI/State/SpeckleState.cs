using System.Collections.Generic;
using DUI3.Models;
using DUI3.State;

namespace ConnectorRhinoWebUI.State
{
  public class SpeckleState : ISpeckleState
  {
    public List<ModelCard> ModelCards { get; } = new List<ModelCard>();
    public Dictionary<string, object> SpeckleObjects { get; } = new Dictionary<string, object>();
  }
}
