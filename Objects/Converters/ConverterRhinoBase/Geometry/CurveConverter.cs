using System;
using Speckle.Core.Kits.Modular;

namespace ConverterRhinoBase;

public class CurveConverter
  : SpeckleObjectConverter,
    ISpeckleObjectConverter<OG.Curve, RG.Curve>,
    ISpeckleObjectConverter<RG.Curve, OG.Curve>
{
  public RG.Curve Convert(OG.Curve input)
  {
    throw new NotImplementedException();
  }

  public OG.Curve Convert(RG.Curve input)
  {
    throw new NotImplementedException();
  }

  public static CurveConverter Instance { get; } = new();
}
