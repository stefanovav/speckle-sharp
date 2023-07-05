using System.Linq;
using System.Windows.Documents;
using Speckle.Core.Plugins;

namespace SpeckleRhino.Dui3App
{
  public class SpeckleApp : IApp
  {
    public IAppState State { get; private set; }
    public IUiController UiController { get; }

    public SpeckleApp(IAppState state, IUiController uiController) 
    { 
      this.State = state;
      this.UiController = uiController;
    }

    public void SendMessages()
    {
      foreach ((string, string) message in this.State.MessageQueue)
      {
        this.UiController.ExecuteScript(message.Item1, message.Item2);
      }
    }

    public void UpdateState(IAction action)
    {
      IAppState oldState = this.State;
      this.State = action.UpdateState(oldState);
      if (this.State.MessageQueue.Any())
      {
        this.SendMessages();
      }
      if (oldState != this.State)
      {
        this.UiController.UpdateUI(this.State);
      }
    }
  }
}
