using System;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Connectors;
using Speckle.Core.Plugins;
using SpeckleRhino.UiController.Actions;
using SpeckleRhino.UiController.Views;

namespace SpeckleRhino.Dui3App
{
  /// <summary>
  /// Speckle App is a top Speckle object on the Rhino.
  /// It responsible to update it's state.
  /// </summary>
  public class SpeckleApp : IApp
  {
    /// <inheritdoc cref="IAppState"/>
    public IAppState State { get; private set; }

    /// <inheritdoc cref="IUIController"/>
    public IUIController UiController { get; }

    public SpeckleApp(IAppState state, IUIController uiController) 
    { 
      this.State = state;
      this.UiController = uiController;
      this.InitAppView();
      this.RegisterPlugins();
    }

    /// <inheritdoc/>
    public void InitAppView()
    {
      IView appView = new SpeckleAppView(this);
      this.UiController.Views.Add(appView);
    }

    /// <inheritdoc/>
    public void RegisterPlugins()
    {
      IEnumerable<IView> pluginViews = PluginManager.GetPluginViews(this, "Rhino");
      this.UiController.Views.AddRange(pluginViews);
    }

    /// <inheritdoc/>
    public void UpdateState(IAction action, Guid resolveId)
    {
      // Get old state to compare later
      IAppState oldState = this.State;

      // Call action to get new state.
      this.State = action.UpdateState(oldState, resolveId);

      // Compare old and new state to Update UI views if needed.
      if (oldState != this.State)
      {
        this.UiController.UpdateUI(this.State);
      }

      if (this.State.MessageQueue.Any())
      {
        // Clear messages after messages sent.
        IAction clearMessages = new ClearMessages();
        UpdateState(clearMessages, Guid.Empty);
      }      
    }
  }
}
