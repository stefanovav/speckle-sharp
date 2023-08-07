using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace Objects.Converter.Revit
{
  public partial class ConverterRevit
  {

    private List<Base> RevitLinkInstanceToSpeckle(RevitLinkInstance revitLinkInstance, out List<string> notes)
    {
      var objects = new List<Base>();




      var linkInstanceTransform = revitLinkInstance.GetTotalTransform();
      var referencePointTransform = linkInstanceTransform.Inverse.Multiply(DocReferencePointTransform);

      notes = new List<string>();

      return objects;
    }
  }
}
