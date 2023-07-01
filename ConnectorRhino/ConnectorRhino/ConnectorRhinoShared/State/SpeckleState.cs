using System;
using System.Collections.Generic;

namespace SpeckleRhino.State
{
  public class SpeckleState
  {
    public List<object> SpeckleObjects { get; }

    public SpeckleState()
    {
      this.SpeckleObjects = new List<object>();
    }
  }
}
