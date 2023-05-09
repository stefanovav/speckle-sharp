using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using OG = Objects.Geometry;
using Objects.Structural.Analysis;
using DB = Autodesk.Revit.DB;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class LineExtensions
  {
    public static DB.Line ToRevit(this OG.Line line, Transform docReferencePointTransform)
    {
      return DB.Line.CreateBound(
        line.start.ToRevit(docReferencePointTransform),
        line.end.ToRevit(docReferencePointTransform));
    }
  }
}
