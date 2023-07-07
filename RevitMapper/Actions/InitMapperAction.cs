using System;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Connectors;

namespace RevitMapper.Actions
{
  public class InitMapperAction : IAction
  {
    public IAppState UpdateState(IAppState state, Guid resolveId)
    {
      List<UIMessage> newMessageQueue = state.MessageQueue.ToList();
      newMessageQueue.Add(new UIMessage(resolveId.ToString(), "console.log", "'Message comes from Revit Mapper Plugin'"));
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
