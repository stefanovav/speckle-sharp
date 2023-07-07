using Speckle.Core.Connectors;

namespace SpeckleRhino.UiController
{
  /// <summary>
  /// Interface to control UI operations between running Rhino and Web based UI.
  /// </summary>
  public interface IRhinoUIController : IUIController
  {
    /// <summary>
    /// Gets or sets Core WebView2 to communicate with UI.
    /// </summary>
    public Microsoft.Web.WebView2.Core.CoreWebView2 CoreWebView2 { get; set; }
  }
}
