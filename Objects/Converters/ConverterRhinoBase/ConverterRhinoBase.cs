using System;
using Speckle.Core.Kits;
using Speckle.Core.Kits.Modular;

namespace ConverterRhinoBase;

public class RhinoGeometryConverter : SpeckleConverterModule
{
  public override string Description => "Converts geometry entities to-from Rhino<->Speckle";
  public override string Name => "Rhino.Geometry Converter";
  public override Guid Id { get; }

  public RhinoGeometryConverter()
  {
    WithConverter(VectorConverter.Instance)
      .WithConverter(LineConverter.Instance)
      .WithConverter(CurveConverter.Instance);
  }
}
