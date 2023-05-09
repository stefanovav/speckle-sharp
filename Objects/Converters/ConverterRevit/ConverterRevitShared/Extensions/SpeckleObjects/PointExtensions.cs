#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using ConverterRevitShared.Extensions.RevitObjects;
using ConverterRevitShared.Utils;
using OG = Objects.Geometry;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class PointExtensions
  {
    public static XYZ ToRevit(this OG.Point pt)
    {
      var units = UnitsUtils.UnitsToRevit(pt.units);
      return pt.ToRevit(units);
    }
#if REVIT2020
    public static XYZ ToRevit(this OG.Point pt, DisplayUnitType unitsTypeId)
    {
      var revitPoint = new XYZ(
        UnitsUtils.ScaleToRevit(pt.x, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.y, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.z, unitsTypeId)
      );
      var intPt = revitPoint.ToInternalCoordinates(true);
      return intPt;
    }
#else
    public static XYZ ToRevit(this OG.Point pt, ForgeTypeId unitsTypeId)
    {
      var revitPoint = new XYZ(
        UnitsUtils.ScaleToRevit(pt.x, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.y, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.z, unitsTypeId)
      );
      var intPt = revitPoint.ToInternalCoordinates(true);
      return intPt;
    }
#endif
  }
}
