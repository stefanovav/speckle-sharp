using Rhino;

namespace SpeckleRhino.State
{
  public class RhinoState : IRhinoState
  {
    public RhinoDoc Doc { get; }

    public RhinoState(RhinoDoc doc)
    {
        this.Doc = doc;
    }
  }
}
