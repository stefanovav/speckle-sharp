using System.Collections.Generic;

namespace Speckle.Core.Plugins
{
  public interface IAppState
  {
    public ISpeckleState SpeckleState { get; }
    public IUserState UserState { get; }
    public IEnumerable<(string, string)> MessageQueue { get; }
    public IAppState WithMessageQueue(IEnumerable<(string, string)> newMessageQueue);
  }
}
