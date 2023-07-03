using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.Dui3App;
using SpeckleRhino.UiController.Actions;

namespace SpeckleRhino.UiController.Commands
{
  public class InitLocalAccounts : ICommand
  {
    public string Name => "init_local_accounts";

    public SpeckleApp App { get; }

    public InitLocalAccounts(SpeckleApp app)
    {
      this.App = app;
    }

    public void Execute(object data)
    {
      IAction action = new GetLocalAccounts();
      this.App.UpdateState(action);
    }
  }
}
