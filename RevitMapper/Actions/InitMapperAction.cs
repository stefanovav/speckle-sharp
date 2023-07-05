using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Plugins;

namespace RevitMapper.Actions
{
  public class InitMapperAction : IAction
  {
    public IAppState UpdateState(IAppState state)
    {
      List<(string, string)> newMessageQueue = state.MessageQueue.ToList();
      newMessageQueue.Add(("console.log", "'Message comes from Revit Mapper Plugin'"));
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
