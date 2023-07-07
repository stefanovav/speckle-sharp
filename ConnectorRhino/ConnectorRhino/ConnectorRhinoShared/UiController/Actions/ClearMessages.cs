using System;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Connectors;

namespace SpeckleRhino.UiController.Actions
{
  public class ClearMessages : IAction
  {
    public IAppState UpdateState(IAppState state, Guid resolveId)
    {
      List<UIMessage> newMessageQueue = state.MessageQueue.ToList();
      newMessageQueue.Clear();
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
