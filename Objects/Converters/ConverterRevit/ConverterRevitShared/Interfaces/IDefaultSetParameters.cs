using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;

namespace ConverterRevitShared.Interfaces
{
  internal struct ParameterInfo
  {
    public BuiltInParameter BuiltInParameter { get; set; }
    public StorageType StorageType { get; set; }
    public string StringValue { get; set; }
    public int IntegerValue { get; set; }
    public double DoubleValue { get; set; }
    public ElementId ElementIdValue { get; set; }
    public string Units { get; set; }
  }
  internal interface IDefaultSetParameters
  {
    public Dictionary<BuiltInParameter, ParameterInfo> Parameters { get; }
  }
}
