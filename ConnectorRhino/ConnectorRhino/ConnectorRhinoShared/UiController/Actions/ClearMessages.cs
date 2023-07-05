using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Plugins;

namespace SpeckleRhino.UiController.Actions
{
  public class ClearMessages : IAction
  {
    public IAppState UpdateState(IAppState state)
    {
      List<(string, string)> newMessageQueue = state.MessageQueue.ToList();
      newMessageQueue.Clear();
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
