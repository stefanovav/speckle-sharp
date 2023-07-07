using System;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Credentials;
using Speckle.Core.Connectors;
using Speckle.Newtonsoft.Json;

namespace SpeckleRhino.UiController.Actions
{
  public class GetLocalAccounts : IAction
  {
    public IAppState UpdateState(IAppState state, Guid resolveId)
    {
      List<UIMessage> newMessageQueue = state.MessageQueue.ToList();
      IEnumerable<Account> accounts = AccountManager.GetAccounts();
      string seralizedAccounts = JsonConvert.SerializeObject(accounts);
      newMessageQueue.Add(new UIMessage(resolveId.ToString(), "loadAccounts", seralizedAccounts));
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
