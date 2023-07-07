using System;
using Speckle.Core.Plugins;
using Speckle.Core.Connectors;

namespace RevitMapper
{
  public class RevitMapperPlugin : SpecklePlugin
  {
    public override string Name => "Revit Mapper";

    public override Guid Id => typeof(RevitMapperView).GUID;

    public override string Author => "";

    public override string Url => "";

    public override IView OnLoad(IApp speckleApp)
    {
      return new RevitMapperView(speckleApp);
    }
  }
}
