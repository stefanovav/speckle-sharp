using System.Collections.Generic;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Interface to control operations between host application and Web based UI.<br></br>
  /// Each host application must have interface/class that inherited from this interface as base and it must connected to Web communication interface.
  /// <list type="table"><listheader><em><strong>Possible Implementations</strong></em></listheader>
  /// </list>
  /// <list type="bullet">
  /// <item><term>WebView2</term> It is a way to embed web content (HTML, CSS, and JavaScript) in your native applications with Microsoft Edge WebView2.</item>
  /// <item><term>CEF</term> A simple framework for embedding Chromium-based browsers in other applications to run web content (HTML, CSS, and JavaScript).</item>
  /// </list>
  /// </summary>
  public interface IUIController
  {
    /// <summary>
    /// Gets views that under responsibility of the UiController.
    /// </summary>
    public List<IView> Views { get; }

    /// <summary>
    /// It is called by App if old state and new state is not equal each other at the end of the any Command.
    /// It sends messages to the UI back if anything collected while actions under the message queue on AppState.
    /// </summary>
    /// <param name="state"></param>
    public void UpdateUI(IAppState state);

    /// <summary>
    /// Execute scripts to the Web based UI as Javascript function.
    /// UIMessage struct basically collects info about id, name and data of the script.<br/>
    /// <example>
    /// <br/><br/>
    /// Example call,
    /// <code>
    ///     var message = new UIMessage("---Guid---", "initLocalAccounts", "{....}");
    ///     var jsMessage = string.format("window.getEventBus({0}).{1}({2})", message.id, message.name, message.data); // TBD!
    ///     
    ///     // This function basically responsible to call UI framework as below example with WebView2
    ///     CoreWebView2.ExecuteScriptAsync(jsMessage);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="message"></param>
    public void ExecuteScript(UIMessage message);
  }
}
