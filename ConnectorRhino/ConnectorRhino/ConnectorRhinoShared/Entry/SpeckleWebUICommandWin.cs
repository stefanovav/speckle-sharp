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
    Instance = this;
  }

  public static SpeckleWebUICommandWin Instance { get; private set; }

  public override string EnglishName => "SpeckleDUI3";

  protected override Result RunCommand(RhinoDoc doc, RunMode mode)
  {
    try
    {
      Panels.OpenPanel(typeof(UI.Dui3Panel).GUID);
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
