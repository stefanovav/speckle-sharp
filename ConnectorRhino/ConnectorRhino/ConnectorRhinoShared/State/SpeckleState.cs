using System.Collections.Generic;
using Speckle.Core.Connectors;

namespace SpeckleRhino.State
{
  public class SpeckleState : ISpeckleState 
  {
    public List<object> SpeckleObjects { get; }

    public SpeckleState()
    {
      this.SpeckleObjects = new List<object>();
    }
  }
}
