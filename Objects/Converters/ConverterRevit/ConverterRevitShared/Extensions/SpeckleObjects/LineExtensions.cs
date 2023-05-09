using System;
using System.Collections.Generic;
using System.Text;
using Objects.Geometry;
using DB = Autodesk.Revit.DB;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class LineExtensions
  {
    public static DB.Line ToRevit(this Line line)
    {
      return DB.Line.CreateBound(
        line.start.ToRevit(),
        line.end.ToRevit());
    }
  }
}
