#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using ConverterRevitShared.Extensions.RevitObjects;
using ConverterRevitShared.Utils;
using Microsoft.Extensions.Caching.Memory;
using OG = Objects.Geometry;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class PointExtensions
  {
    public static XYZ ToRevit(this OG.Point pt, Transform docReferencePointTransform)
    {
      var units = UnitsUtils.UnitsToRevit(pt.units);
      return pt.ToRevit(units, docReferencePointTransform);
    }
#if REVIT2020
    public static XYZ ToRevit(this OG.Point pt, DisplayUnitType unitsTypeId, Transform docReferencePointTransform)
    {
      var revitPoint = new XYZ(
        UnitsUtils.ScaleToRevit(pt.x, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.y, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.z, unitsTypeId)
      );
      var intPt = revitPoint.ToInternalCoordinates(docReferencePointTransform, true);
      return intPt;
    }
#else
    public static XYZ ToRevit(this OG.Point pt, ForgeTypeId unitsTypeId, Transform docReferencePointTransform)
    {
      var revitPoint = new XYZ(
        UnitsUtils.ScaleToRevit(pt.x, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.y, unitsTypeId),
        UnitsUtils.ScaleToRevit(pt.z, unitsTypeId)
      );
      var intPt = revitPoint.ToInternalCoordinates(docReferencePointTransform, true);
      return intPt;
    }
#endif
  }
}
