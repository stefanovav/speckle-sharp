using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using Speckle.Core.Plugins;

namespace SpeckleRhino.UiController
{
  [ClassInterface(ClassInterfaceType.AutoDual)]
  [ComVisible(true)]
  public class SpeckleUiController : IRhinoUiController
  {
    public Microsoft.Web.WebView2.Core.CoreWebView2 CoreWebView2 { get; set; }
    public List<IView> Views { get; }

    public SpeckleUiController()
    {
      this.Views = new List<IView>();
    }

    public void UpdateUI(IAppState state)
    {
      foreach (IView view in this.Views)
      {
        view.UpdateView();
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
        this.ExecuteScript("console.log", "Command not found in any view.");
      }
    }

    /// <summary>
    /// Sends an event to the UI, bound to the global EventBus.
    /// </summary>
    /// <param name="eventName">The event's name.</param>
    /// <param name="eventMessage">The event args, which will be serialised to a string.</param>
    public void ExecuteScript(string eventName, string eventMessage)
    {
      var script = string.Format("window.{0}({1})", eventName, eventMessage);
      this.CoreWebView2.ExecuteScriptAsync(script);
    }
  }
}
