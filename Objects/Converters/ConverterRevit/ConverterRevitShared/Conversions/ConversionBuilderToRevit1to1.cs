using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilderToRevit1to1<TConvertable, TRevit, TSelf> : ConversionBuilderToRevit<TConvertable, TRevit, TSelf>
    where TConvertable : Base
    where TRevit : Element
    where TSelf : ConversionBuilderToRevit1to1<TConvertable, TRevit, TSelf>
  {
    public TRevit RevitObject => ReturnObject;
    public override Document Doc => ReturnObject.Document;
  }
}
