using System;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Rhino;

namespace SpeckleRhino;

[Guid("AE26F9DD-ACCC-4E14-9552-18DC1BF7D7EF")]
public class Dui3Panel: Panel
{
  public uint DocUint { get; }

  public RhinoDoc Doc { get; }

  public WebView WebView { get; }

  /// <summary>
  /// Provide easy access to the SampleCsEtoPanel.GUID
  /// </summary>
  public static Guid PanelId => typeof(Dui3Panel).GUID;

  public Dui3Panel(uint docUint)
  {
    this.DocUint = docUint;
    this.Doc = RhinoDoc.FromRuntimeSerialNumber(docUint);
    
    this.WebView = new WebView();
    this.WebView.Width = 640;
    this.WebView.Height = 480;

    this.WebView.DocumentLoading += (sender, e) =>
    {
      Microsoft.Web.WebView2.Wpf.WebView2 webView2 = (Microsoft.Web.WebView2.Wpf.WebView2)this.WebView.ControlObject;
      if (webView2 != null)
      {
        Microsoft.Web.WebView2.Core.CoreWebView2 coreWebView2 = webView2.CoreWebView2;
        if (coreWebView2 != null)
        {
          // TODO: Register bindings/uicontroller here
          // coreWebView2.AddHostObjectToScript("ExecHost", App.UiController);
        }
      }
    };

#if DEBUG
    this.WebView.Url = new Uri("http://localhost:3003/");
#else
    // we will set here exact dui3 url later.
#endif

    var layout = new DynamicLayout { DefaultSpacing = new Size(5, 5), Padding = new Padding(10) };
    layout.AddSeparateRow(this.WebView, null);
    layout.Add(null);
    Content = layout;
  }
}
