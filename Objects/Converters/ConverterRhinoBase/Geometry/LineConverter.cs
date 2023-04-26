using System;
using Speckle.Core.Kits.Modular;

namespace ConverterRhinoBase;

public class LineConverter
  : SpeckleObjectConverter,
    ISpeckleObjectConverter<OG.Line, RG.Line>,
    ISpeckleObjectConverter<RG.Line, OG.Line>
{
  public RG.Line Convert(OG.Line input)
  {
    throw new NotImplementedException();
  }

  public OG.Line Convert(RG.Line input)
  {
    throw new NotImplementedException();
  }

  public static LineConverter Instance { get; } = new();
}
