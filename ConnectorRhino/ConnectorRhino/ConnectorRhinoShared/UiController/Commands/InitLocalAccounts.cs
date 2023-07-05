using Speckle.Core.Plugins;
using SpeckleRhino.UiController.Actions;

namespace SpeckleRhino.UiController.Commands
{
  public class InitLocalAccounts : ICommand
  {
    public string Name => "init_local_accounts";

    public IApp App { get; }

    public InitLocalAccounts(IApp app)
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
