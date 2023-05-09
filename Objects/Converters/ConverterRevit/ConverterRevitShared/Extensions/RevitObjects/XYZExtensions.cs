using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;

namespace ConverterRevitShared.Extensions.RevitObjects
{
  internal static class XYZExtensions
  {
    //public static XYZ ToInternalCoordinates(this XYZ p, Document doc, bool isPoint)
    //{
    //  var rpt = doc.GetDocReferencePointTransform();
    //  return (isPoint) ? rpt.OfPoint(p) : rpt.OfVector(p);
    //}
    public static XYZ ToInternalCoordinates(this XYZ p, bool isPoint)
    {
      return p;
    }
  }
}
