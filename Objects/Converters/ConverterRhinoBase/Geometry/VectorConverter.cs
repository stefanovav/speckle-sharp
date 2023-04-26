using System;
using Speckle.Core.Kits;
using Speckle.Core.Kits.Modular;

namespace ConverterRhinoBase;

public class VectorConverter
  : SpeckleObjectConverter,
    ISpeckleObjectConverter<RG.Vector3d, OG.Vector>,
    ISpeckleObjectConverter<OG.Vector, RG.Vector3d>,
    ISpeckleObjectConverter<RG.Point3d, OG.Point>,
    ISpeckleObjectConverter<OG.Point, RG.Point3d>
{
  public OG.Vector Convert(RG.Vector3d input)
  {
    return new OG.Vector(input.X, input.Y, input.Z, Context.Peek.Units);
  }

  public RG.Vector3d Convert(OG.Vector input)
  {
    var f = Context.Peek.ConversionFactor(input.units);
    return new RG.Vector3d(input.x * f, input.y * f, input.z * f);
  }

  public OG.Point Convert(RG.Point3d input)
  {
    return new OG.Point(input.X, input.Y, input.Z, Context.Peek.Units);
  }

  public RG.Point3d Convert(OG.Point input)
  {
    var f = Context.Peek.ConversionFactor(input.units);
    return new RG.Point3d(input.x * f, input.y * f, input.z * f);
  }

  public static VectorConverter Instance { get; } = new();
}
