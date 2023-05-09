using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Microsoft.Extensions.Caching.Memory;

namespace ConverterRevitShared.Extensions.RevitObjects
{
  internal static class XYZExtensions
  {
    public static XYZ ToInternalCoordinates(this XYZ p, Transform docReferencePointTransform, bool isPoint)
    {
      return (isPoint) ? docReferencePointTransform.OfPoint(p) : docReferencePointTransform.OfVector(p);
    }
    //public static XYZ ToInternalCoordinates(this XYZ p, bool isPoint)
    //{
    //  return p;
    //}
  }
}
