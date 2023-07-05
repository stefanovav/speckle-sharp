using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Credentials;
using Speckle.Core.Plugins;
using Speckle.Newtonsoft.Json;

namespace SpeckleRhino.UiController.Actions
{
  public class GetLocalAccounts : IAction
  {
    public IAppState UpdateState(IAppState state)
    {
      List<(string, string)> newMessageQueue = state.MessageQueue.ToList();
      IEnumerable<Account> accounts = AccountManager.GetAccounts();
      string seralizedAccounts = JsonConvert.SerializeObject(accounts);
      newMessageQueue.Add(("loadAccounts", seralizedAccounts));
      return state.WithMessageQueue(newMessageQueue);
    }
  }
}
