using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Objects.Converter.Revit;

namespace ConverterRevitShared.Interfaces
{
  internal interface IRevitContext
  {
    public Document Doc { get; }
  }
}
