using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Autodesk.Revit.UI;
using CefSharp;
using CefSharp.Wpf;
using Sentry.Protocol;
using Speckle.ConnectorRevit.UI;
using WebUI;

namespace Speckle.ConnectorRevit
{
  public partial class WebUIPanel : Page, Autodesk.Revit.UI.IDockablePaneProvider
  {

#if DEBUG
    public WebUIPanel(string address = "http://localhost:3003")
#else
    public WebUIPanel(string address = "https://dashing-haupia-e8f6e3.netlify.app/")
#endif
    {
      // InitializeCef();
      InitializeComponent();
      Browser.FrameLoadEnd += Browser_FrameLoadEnd; ;
      // webUIBindings.Browser = Browser;

      var executeScriptAsyncMethod = (string script) => {
        Debug.WriteLine(script);
        Browser.EvaluateScriptAsync(script);
      };
      var showDevToolsMethod = () => Browser.ShowDevTools();

      var baseBindings = new RevitBaseBindings(); // They don't need to be created here, but wherever it makes sense in the app
      var baseBindingsBridge = new DUI3.BrowserBridge(Browser, baseBindings, executeScriptAsyncMethod, showDevToolsMethod);

      // old method
      CefSharpSettings.LegacyJavascriptBindingEnabled = true;
      Browser.RegisterAsyncJsObject(baseBindingsBridge.FrontendBoundName, baseBindingsBridge, options: BindingOptions.DefaultBinder);

      // 
      // // new method
      // Browser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
      // Browser.JavascriptObjectRepository.Register(baseBindingsBridge.FrontendBoundName, baseBindingsBridge, true, options: BindingOptions.DefaultBinder);

      Browser.Address = address;
    }

    private void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
    {
      Browser.ShowDevTools();
    }

    // Note: Dynamo ships with cefsharp too, so we need to be careful around initialising cefsharp.
    private void InitializeCef()
    {
      if (Cef.IsInitialized) return;

      Cef.EnableHighDPISupport();

      var assemblyLocation = Assembly.GetExecutingAssembly().Location;
      var assemblyPath = System.IO.Path.GetDirectoryName(assemblyLocation);
      var pathSubprocess = System.IO.Path.Combine(assemblyPath, "CefSharp.BrowserSubprocess.exe");
      var settings = new CefSettings
      {
        BrowserSubprocessPath = pathSubprocess,
        RemoteDebuggingPort = 9222
      };

      Cef.Initialize(settings);
    }

    public void SetupDockablePane(Autodesk.Revit.UI.DockablePaneProviderData data)
    {
      data.FrameworkElement = this as FrameworkElement;
      data.InitialState = new Autodesk.Revit.UI.DockablePaneState();
      data.InitialState.DockPosition = DockPosition.Tabbed;
      data.InitialState.TabBehind = Autodesk.Revit.UI.DockablePanes.BuiltInDockablePanes.ProjectBrowser;
    }
  }
}
