using System;
using Speckle.Core.Connectors;
using SpeckleRhino.UiController.Actions;

namespace SpeckleRhino.UiController.Commands
{
  public class InitLocalAccounts : ICommand
  {
    public string Name => "init_local_accounts";

    public IApp App { get; }

    public Guid ResolveId { get; }

    public InitLocalAccounts(IApp app, Guid resolveId)
    {
      this.App = app;
      this.ResolveId = resolveId;
    }

    public void Execute(object data)
    {
      IAction action = new GetLocalAccounts();
      this.App.UpdateState(action, this.ResolveId);
    }
  }
}
