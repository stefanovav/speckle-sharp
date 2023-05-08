using Objects.Converter.Revit;

namespace ConverterRevitShared.Interfaces
{
  internal interface IHasRevitConverter
  {
    public ConverterRevit Converter { get; }
  }
}
