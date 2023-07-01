using System;
using Rhino;
using Rhino.Commands;
using Rhino.UI;
using Speckle.Core.Models.Extensions;

namespace SpeckleRhino;

#if !MAC
public class SpeckleWebUICommandWin : Command
{
  public SpeckleWebUICommandWin()
  {
    Eto.Wpf.Forms.Controls.WebView2Loader.InstallMode = Eto.Wpf.Forms.Controls.WebView2InstallMode.Manual;
    Eto.Wpf.Forms.Controls.WebView2Handler.GetCoreWebView2Environment = () =>
    {
      var userDataFolder = Rhino.RhinoApp.GetDataDirectory(true, true);
      return Microsoft.Web.WebView2.Core.CoreWebView2Environment.CreateAsync(userDataFolder: userDataFolder);
    };
    Instance = this;
  }

  public static SpeckleWebUICommandWin Instance { get; private set; }

  public override string EnglishName => "SpeckleDUI3";

  protected override Result RunCommand(RhinoDoc doc, RunMode mode)
  {
    try
    {
      Panels.OpenPanel(typeof(SpeckleRhino.Dui3Panel).GUID);
      return Result.Success;
    }
    catch (Exception e)
    {
      RhinoApp.CommandLineOut.WriteLine($"Speckle Error - {e.ToFormattedString()}");
      return Result.Failure;
    }
  }
}
#endif
