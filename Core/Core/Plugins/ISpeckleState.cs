using System.Collections.Generic;

namespace Speckle.Core.Plugins
{
  public interface ISpeckleState
  {
    public List<object> SpeckleObjects { get; }
  }
}
