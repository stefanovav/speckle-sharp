namespace SpeckleRhino.UiController
{
  public interface IRhinoUiController : Speckle.Core.Plugins.IUiController
  {
    public Microsoft.Web.WebView2.Core.CoreWebView2 CoreWebView2 { get; set; }
  }
}
