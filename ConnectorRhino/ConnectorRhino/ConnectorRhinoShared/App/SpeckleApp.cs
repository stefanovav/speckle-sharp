using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Plugins;
using SpeckleRhino.UiController.Actions;
using SpeckleRhino.UiController.Views;

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
      this.InitAppView();
      this.RegisterPlugins();
    }

    public void InitAppView()
    {
      IView appView = new SpeckleAppView(this);
      this.UiController.Views.Add(appView);
    }

    public void RegisterPlugins()
    {
      IEnumerable<IView> pluginViews = PluginManager.GetPluginViews(this, "Rhino");
      this.UiController.Views.AddRange(pluginViews);
    }

    /// <summary>
    /// Send messages to the UI via UiController.
    /// TODO: Later add view id information to the message to execute it with correct plugin.
    /// </summary>
    public void SendMessages()
    {
      foreach ((string, string) message in this.State.MessageQueue)
      {
        this.UiController.ExecuteScript(message.Item1, message.Item2);
      }

      // Clear messages after messages sent.
      IAction action = new ClearMessages();
      UpdateState(action);
    }

    public void UpdateState(IAction action)
    {
      // Get old state to compare later
      IAppState oldState = this.State;

      // Call action to get new state.
      this.State = action.UpdateState(oldState);

      // Check messages to send UI after state update.
      if (this.State.MessageQueue.Any())
      {
        this.SendMessages();
      }

      // Compare old and new state to Update UI views if needed.
      if (oldState != this.State)
      {
        this.UiController.UpdateUI(this.State);
      }
    }
  }
}
