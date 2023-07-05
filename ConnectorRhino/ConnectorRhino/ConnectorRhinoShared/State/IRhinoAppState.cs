using System;
using Speckle.Core.Plugins;

namespace SpeckleRhino.State
{
  public interface IRhinoAppState : IAppState
  {
    public IRhinoState RhinoState { get; }
  }
}
