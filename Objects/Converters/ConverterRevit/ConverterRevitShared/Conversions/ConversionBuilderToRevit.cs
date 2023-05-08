using System;
using ConverterRevitShared.Interfaces;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilderToRevit<TRevit, TSelf> : ConversionBuilder<TRevit,TSelf>, IHasRevitObject<TRevit>
    where TSelf : ConversionBuilderToRevit<TRevit, TSelf>
  {
    public TRevit RevitObject 
    {
      get => ReturnObject;
      set => ReturnObject = value; 
    }
  }
}
