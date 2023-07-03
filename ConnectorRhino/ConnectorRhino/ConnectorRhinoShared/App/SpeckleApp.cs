using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpeckleRhino.State;
using SpeckleRhino.UiController;
using SpeckleRhino.UiController.Actions;

namespace SpeckleRhino.Dui3App
{
  public class SpeckleApp
  {
    public AppState State { get; private set; }
    public SpeckleUiController UiController { get; }

    public SpeckleApp(AppState state, SpeckleUiController uiController) 
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
      AppState oldState = this.State;
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
