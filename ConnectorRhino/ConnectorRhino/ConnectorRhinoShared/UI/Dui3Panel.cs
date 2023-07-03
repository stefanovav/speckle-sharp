using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Eto.Drawing;
using Eto.Forms;
using Rhino;
using SpeckleRhino.Dui3App;
using SpeckleRhino.State;
using SpeckleRhino.UiController;
using SpeckleRhino.UiController.Views;

namespace SpeckleRhino.UI;

[Guid("AE26F9DD-ACCC-4E14-9552-18DC1BF7D7EF")]
public class Dui3Panel: Panel
{
  public uint DocUint { get; }

  public WebView WebView { get; }

  public RhinoDoc Doc { get; }

  /// <summary>
  /// Provide easy access to the SampleCsEtoPanel.GUID
  /// </summary>
  public static Guid PanelId => typeof(Dui3Panel).GUID;

  public Dui3Panel(uint docUint)
  {
    this.DocUint = docUint;
    this.Doc = RhinoDoc.FromRuntimeSerialNumber(docUint);

    this.SizeChanged += OnSizeChanged;

    UserState userState = new UserState();
    RhinoState rhinoState = new RhinoState(this.Doc);
    SpeckleState speckleSpeckle = new SpeckleState();

    AppState appState = new AppState(userState, rhinoState, speckleSpeckle, new List<(string, string)>());
    SpeckleUiController uiController = new SpeckleUiController();

    Eto.Wpf.Forms.Controls.WebView2Loader.InstallMode = Eto.Wpf.Forms.Controls.WebView2InstallMode.Manual;
    Eto.Wpf.Forms.Controls.WebView2Handler.GetCoreWebView2Environment = () =>
    {
      var userDataFolder = Rhino.RhinoApp.GetDataDirectory(true, true);
      return Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
    };

    this.WebView = new WebView();
    this.WebView.Width = this.Size.Width;
    this.WebView.Height = this.Size.Height;

    this.WebView.DocumentLoading += (sender, e) =>
    {
      var webView2 = (Microsoft.Web.WebView2.Wpf.WebView2)this.WebView.ControlObject;
      if (webView2 != null)
      {
        var coreWebView2 = webView2.CoreWebView2;
        if (coreWebView2 != null)
        {
          uiController.CoreWebView2 = coreWebView2;
          coreWebView2.AddHostObjectToScript("webviewBindings", uiController);
        }
      }
    };

#if DEBUG
    this.WebView.Url = new Uri("http://localhost:3003/");
#else
    this.WebView.Url = new Uri("https://dashing-haupia-e8f6e3.netlify.app/");☻
    // we will set here exact dui3 url later.
#endif

    SpeckleApp app = new SpeckleApp(appState, uiController);

    SpeckleAppView speckleAppView = new SpeckleAppView(app);
    app.UiController.Views.Add(speckleAppView);

    var layout = new DynamicLayout { DefaultSpacing = new Size(2, 2), Padding = new Padding(2) };
    layout.AddSeparateRow(this.WebView, null);
    layout.Add(null);
    this.Content = layout;
  }

  private void OnSizeChanged(object sender, EventArgs e)
  {
    RhinoApp.WriteLine("DUI3 Size Changed >>> Width: {0} - Height: {1}", this.Size.Width, this.Size.Height);
    this.WebView.Width = this.Size.Width;
    this.WebView.Height = this.Size.Height;
  }
}
