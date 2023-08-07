using System.Collections.Generic;
using DUI3.Models;

namespace DUI3.State
{
  public interface ISpeckleState
  {
    public List<ModelCard> ModelCards { get; }
    public Dictionary<string, object> SpeckleObjects { get; }
  }
}
