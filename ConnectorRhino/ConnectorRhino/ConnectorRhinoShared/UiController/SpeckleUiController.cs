using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Speckle.Core.Connectors;

namespace SpeckleRhino.UiController
{
  /// <summary>
  /// UIController instance to bind C# commands/actions to the JS functions.
  /// </summary>
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [ComVisible(true)]
  public class SpeckleUIController : IRhinoUIController
  {
    /// <inheritdoc/>
    public Microsoft.Web.WebView2.Core.CoreWebView2 CoreWebView2 { get; set; }

    /// <inheritdoc/>
    public List<IView> Views { get; }

    public SpeckleUIController()
    {
      this.Views = new List<IView>();
    }

    /// <inheritdoc/>
    public void UpdateUI(IAppState state)
    {
      // Check messages to send UI after state update.
      if (state.MessageQueue.Any())
      {
        foreach (UIMessage message in state.MessageQueue)
        {
          this.ExecuteScript(message);
        }
      }
    }

    /// <summary>
    /// Execute function called by UI.
    /// </summary>
    /// <param name="id"> Command owner id. </param>
    /// <param name="name"> Command name. </param>
    /// <param name="data"> Command data. </param>
    public async void Exec(string id, string name, object data)
    {
      // Find which view we will execute command.
      IView viewToCallCommand = this.Views.FirstOrDefault(view => view.Id.Equals(Guid.Parse(id)));

      if (viewToCallCommand != null)
      {
        // Find upcoming command on view.
        if (viewToCallCommand.Commands.TryGetValue(name, out ICommand value))
        {
          value.Execute(data);
        }
      }
      else
      {
        // TODO: Handle here logging errors.
        this.ExecuteScript(new UIMessage(string.Empty, "console.log", "Command not found in any view."));
      }
    }

    /// <summary>
    /// Sends an event to the UI, bound to the global EventBus.
    /// </summary>
    /// <param name="eventName">The event's name.</param>
    /// <param name="eventMessage">The event args, which will be serialised to a string.</param>
    public void ExecuteScript(UIMessage message)
    {
      // TBD: For now we do not pass any resolveId that related to any view.
      var script = string.Format("window.{0}({1})", message.FunctionName, message.Data);
      this.CoreWebView2.ExecuteScriptAsync(script);
    }
  }
}
