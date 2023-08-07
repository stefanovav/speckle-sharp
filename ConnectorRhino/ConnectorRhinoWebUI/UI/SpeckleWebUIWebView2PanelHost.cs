using System;
using System.Runtime.InteropServices;
using Rhino;

namespace ConnectorRhinoWebUI.UI
{
  [Guid("39BC44A4-C9DC-4B0A-9A51-4C31ACBCD76A")]
  public class SpeckleWebUIWebView2PanelHost : RhinoWindows.Controls.WpfElementHost
  {
    public SpeckleWebUIWebView2PanelHost(uint docSn)
      : base(new SpeckleWebUiPanelWebView2(), null)
    {
      
    }
  }
}
