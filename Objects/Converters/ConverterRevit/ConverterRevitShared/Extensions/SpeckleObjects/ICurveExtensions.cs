using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Objects;
using OG = Objects.Geometry;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class ICurveExtensions
  {
    /// <summary>
    /// Recursively creates an ordered list of curves from a polycurve/polyline.
    /// Please note that a polyline is broken down into lines.
    /// </summary>
    /// <param name="crv">A speckle curve.</param>
    /// <returns></returns>
    public static CurveArray ToRevit(this ICurve crv, Transform docReferencePointTransform, bool splitIfClosed = false)
    {
      CurveArray curveArray = new CurveArray();
      switch (crv)
      {
        case OG.Line line:
          curveArray.Append(line.ToRevit(docReferencePointTransform));
          return curveArray;

        //case OG.Arc arc:
        //  curveArray.Append(ArcToNative(arc));
        //  return curveArray;

        //case OG.Circle circle:
        //  curveArray.Append(CircleToNative(circle));
        //  return curveArray;

        //case OG.Ellipse ellipse:
        //  curveArray.Append(EllipseToNative(ellipse));
        //  return curveArray;

        //case OG.Spiral spiral:
        //  return PolylineToNative(spiral.displayValue);

        //case OG.Curve nurbs:
        //  var n = CurveToNative(nurbs);
        //  if (IsCurveClosed(n) && splitIfClosed)
        //  {
        //    var split = SplitCurveInTwoHalves(n);
        //    curveArray.Append(split.Item1);
        //    curveArray.Append(split.Item2);
        //  }
        //  else
        //  {
        //    curveArray.Append(n);
        //  }
        //  return curveArray;

        //case OG.Polyline poly:
        //  return PolylineToNative(poly);

        //case OG.Polycurve plc:
        //  foreach (var seg in plc.segments)
        //  {
        //    // Enumerate all curves in the array to ensure polylines get fully converted.
        //    var crvEnumerator = CurveToNative(seg).GetEnumerator();
        //    while (crvEnumerator.MoveNext() && crvEnumerator.Current != null)
        //      curveArray.Append(crvEnumerator.Current as DB.Curve);
        //  }
        //  return curveArray;
        default:
          throw new Speckle.Core.Logging.SpeckleException("The provided geometry is not a valid curve");
      }
    }
  }
}
