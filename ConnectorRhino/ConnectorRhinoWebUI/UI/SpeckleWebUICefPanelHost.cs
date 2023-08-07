using System;
using System.Runtime.InteropServices;


namespace ConnectorRhinoWebUI.UI
{
  [Guid("55B9125D-E8CA-4F65-B016-60DA932AB694")]
  public class SpeckleWebUICefPanelHost : RhinoWindows.Controls.WpfElementHost
  {
    public SpeckleWebUICefPanelHost(uint docSn)
      : base(new SpeckleWebUIPanelCef(), null)
    {
      //ConnectorRhinoWebUIPlugin.Instance.Hosts.Add(typeof(SpeckleWebUICefPanelHost).GUID, this);
    }
  }
}
