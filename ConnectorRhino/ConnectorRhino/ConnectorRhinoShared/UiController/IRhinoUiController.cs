using Speckle.Core.Plugins;

namespace SpeckleRhino.UiController
{
  public interface IRhinoUiController : IUiController
  {
    public Microsoft.Web.WebView2.Core.CoreWebView2 CoreWebView2 { get; set; }
  }
}
