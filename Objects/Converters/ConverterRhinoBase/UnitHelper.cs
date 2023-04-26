using Rhino;
using Speckle.Core.Kits;
using Speckle.Core.Logging;

namespace ConverterRhinoBase;

public static class UnitHelper
{
  public static string UnitToSpeckle(UnitSystem us)
  {
    return us switch
    {
      UnitSystem.Millimeters => Units.Millimeters,
      UnitSystem.Centimeters => Units.Centimeters,
      UnitSystem.Meters => Units.Meters,
      UnitSystem.None => Units.Meters,
      UnitSystem.Unset => Units.Meters,
      UnitSystem.Kilometers => Units.Kilometers,
      UnitSystem.Inches => Units.Inches,
      UnitSystem.Feet => Units.Feet,
      UnitSystem.Yards => Units.Yards,
      UnitSystem.Miles => Units.Miles,
      _ => throw new SpeckleException($"The Unit System \"{us}\" is unsupported.")
    };
  }
}
