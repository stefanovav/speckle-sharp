using System;
using System.Collections.Generic;
using System.Text;
using Rhino;

namespace SpeckleRhino.State
{
  public class RhinoState
  {
    public RhinoDoc Doc { get; }

    public RhinoState(RhinoDoc doc)
    {
        this.Doc = doc;
    }
  }
}
