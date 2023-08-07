using System;
using System.Collections.Generic;
using ConnectorRhinoWebUI.UI;
using Rhino;
using Rhino.PlugIns;
using Rhino.UI;

namespace ConnectorRhinoWebUI
{
  ///<summary>
  /// <para>Every RhinoCommon .rhp assembly must have one and only one PlugIn-derived
  /// class. DO NOT create instances of this class yourself. It is the
  /// responsibility of Rhino to create an instance of this class.</para>
  /// <para>To complete plug-in information, please also see all PlugInDescription
  /// attributes in AssemblyInfo.cs (you might need to click "Project" ->
  /// "Show All Files" to see it in the "Solution Explorer" window).</para>
  ///</summary>
  public class ConnectorRhinoWebUIPlugin : Rhino.PlugIns.PlugIn
  {
    public ConnectorRhinoWebUIPlugin()
    {
      Instance = this;
    }

    public Dictionary<Guid, RhinoWindows.Controls.WpfElementHost> Hosts { get; set; } = new Dictionary<Guid, RhinoWindows.Controls.WpfElementHost>();

    ///<summary>Gets the only instance of the ConnectorRhinoWebUIPlugin plug-in.</summary>
    public static ConnectorRhinoWebUIPlugin Instance { get; private set; }

    // You can override methods here to change the plug-in behavior on
    // loading and shut down, add options pages to the Rhino _Option command
    // and maintain plug-in wide options in a document.
    protected override LoadReturnCode OnLoad(ref string errorMessage)
    {
      // Register Webview2 Panel whenever plugin load.
      Panels.RegisterPanel(
        this,
        typeof(SpeckleWebUIWebView2PanelHost),
        "SpeckleWebUIWebView2",
        System.Drawing.SystemIcons.Information,
        PanelType.System
      );

      // Register CEF Panel whenever plugin load.
      Panels.RegisterPanel(
        this,
        typeof(SpeckleWebUICefPanelHost),
        "SpeckleWebUICef",
        System.Drawing.SystemIcons.Information,
        PanelType.System
      );

      // Add an event handler so we know when documents are read.
      RhinoDoc.EndOpenDocument += new EventHandler<DocumentOpenEventArgs>(this.OnEndOpenDocument);

      // Add an event handler so we know when document is starting to open.
      RhinoDoc.BeginOpenDocument += new EventHandler<DocumentOpenEventArgs>(this.OnBeginOpenDocument);

      // Add an event handler so we know when documents are closed.
      RhinoDoc.CloseDocument += new EventHandler<DocumentEventArgs>(this.OnCloseDocument);

      return LoadReturnCode.Success;
    }

    private void OnCloseDocument(object sender, DocumentEventArgs e)
    {
      RhinoApp.WriteLine("Plugin -> OnCloseDocument");
    }

    private void OnBeginOpenDocument(object sender, DocumentOpenEventArgs e)
    {
      RhinoApp.WriteLine("Plugin -> OnBeginOpenDocument");
    }

    private void OnEndOpenDocument(object sender, DocumentOpenEventArgs e)
    {
      RhinoApp.WriteLine("Plugin -> OnEndOpenDocument");
    }
  }
}
