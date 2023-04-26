using Rhino;
using Speckle.Core.Kits.Modular;

namespace ConverterRhinoBase;

public class Context : State<Context>
{
  public string Units { get; set; } = Speckle.Core.Kits.Units.Meters;
  public double AngleTolerance { get; set; } = 0.0001;
  public double Tolerance { get; set; } = 0.0001;

  public void SetDocumentInfo(RhinoDoc document)
  {
    Units = UnitHelper.UnitToSpeckle(document.ModelUnitSystem);
    AngleTolerance = document.ModelAngleToleranceRadians;
    Tolerance = document.ModelAbsoluteTolerance;
  }

  public double ConversionFactor(string units) => Speckle.Core.Kits.Units.GetConversionFactor(units, Units);
}
