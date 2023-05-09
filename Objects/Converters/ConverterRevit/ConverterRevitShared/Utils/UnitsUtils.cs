#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Speckle.Newtonsoft.Json.Linq;

namespace ConverterRevitShared.Utils
{
  internal static class UnitsUtils
  {
    #region UnitsToRevit
#if REVIT2020
    public static DisplayUnitType UnitsToRevit(string units)
    {
      return units switch
      {
        Speckle.Core.Kits.Units.Millimeters => DisplayUnitType.DUT_MILLIMETERS,
        Speckle.Core.Kits.Units.Centimeters => DisplayUnitType.DUT_CENTIMETERS,
        Speckle.Core.Kits.Units.Meters => DisplayUnitType.DUT_METERS,
        Speckle.Core.Kits.Units.Inches => DisplayUnitType.DUT_DECIMAL_INCHES,
        Speckle.Core.Kits.Units.Feet => DisplayUnitType.DUT_DECIMAL_FEET,
        _ => throw new Speckle.Core.Logging.SpeckleException($"The Unit System \"{units}\" is unsupported."),
      };
    }
#else
    public static ForgeTypeId UnitsToRevit(string units)
    {
      return units switch
      {
        Speckle.Core.Kits.Units.Millimeters => UnitTypeId.Millimeters,
        Speckle.Core.Kits.Units.Centimeters => UnitTypeId.Centimeters,
        Speckle.Core.Kits.Units.Meters => UnitTypeId.Meters,
        Speckle.Core.Kits.Units.Inches => UnitTypeId.Inches,
        Speckle.Core.Kits.Units.Feet => UnitTypeId.Feet,
        _ => throw new Speckle.Core.Logging.SpeckleException($"The Unit System \"{units}\" is unsupported."),
      };
    }
#endif
    #endregion

    #region ScaleToRevit
    public static double ScaleToRevit(double value, string units)
    {
      return ScaleToRevit(value, UnitsToRevit(units));
    }
#if REVIT2020
    public static double ScaleToRevit(double value, DisplayUnitType unitsTypeId)
    {
      return UnitUtils.ConvertToInternalUnits(value, unitsTypeId);
    }
#else
    public static double ScaleToRevit(double value, ForgeTypeId unitsTypeId)
    {
      return UnitUtils.ConvertToInternalUnits(value, unitsTypeId);
    }
#endif
    #endregion

    //public XYZ ToInternalCoordinates(XYZ p, bool isPoint)
    //{
    //  var rpt = GetDocReferencePointTransform(Doc);
    //  return (isPoint) ? rpt.OfPoint(p) : rpt.OfVector(p);
    //}
  }
}
