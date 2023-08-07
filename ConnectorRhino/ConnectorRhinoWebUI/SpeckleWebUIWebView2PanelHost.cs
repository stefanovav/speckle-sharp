using ConnectorRhinoWebUI.UI;
using Rhino;
using Rhino.Commands;
using Rhino.Input.Custom;
using Rhino.UI;

namespace ConnectorRhinoWebUI
{
  public class SpeckleWebUIWebView2Command : Command
  {
    public SpeckleWebUIWebView2Command()
    {
      Instance = this;
    }

    public static SpeckleWebUIWebView2Command Instance { get; private set; }

    public override string EnglishName => "SpeckleWebUIWebView2";

    protected override Result RunCommand(RhinoDoc doc, RunMode mode)
    {
      var panel_id = typeof(SpeckleWebUIWebView2PanelHost).GUID;

      if (mode == RunMode.Interactive)
      {
        Panels.OpenPanel(panel_id);
        return Result.Success;
      }

      var panel_visible = Panels.IsPanelVisible(panel_id);

      var prompt = (panel_visible)
        ? "SpeckleWebUIWebView2 panel is visible. New value"
        : "SpeckleWebUIWebView2 panel is hidden. New value";

      using var go = new GetOption();
      go.SetCommandPrompt(prompt);
      var hide_index = go.AddOption("Hide");
      var show_index = go.AddOption("Show");
      var toggle_index = go.AddOption("Toggle");
      go.Get();

      if (go.CommandResult() != Result.Success)
        return go.CommandResult();

      var option = go.Option();
      if (null == option)
        return Result.Failure;

      var index = option.Index;
      if (index == hide_index)
      {
        if (panel_visible)
          Panels.ClosePanel(panel_id);
      }
      else if (index == show_index)
      {
        if (!panel_visible)
          Panels.OpenPanel(panel_id);
      }
      else if (index == toggle_index)
      {
        if (panel_visible)
          Panels.ClosePanel(panel_id);
        else
          Panels.OpenPanel(panel_id);
      }
      return Result.Success;
    }
  }
}
