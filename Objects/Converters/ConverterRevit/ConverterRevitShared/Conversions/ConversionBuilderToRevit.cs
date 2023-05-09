using System;
using Autodesk.Revit.DB;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilderToRevit<TConvertable, TReturn, TSelf> : ConversionBuilder<TConvertable, TReturn, TSelf>,
    IRevitContext
    where TConvertable : Base
    where TSelf : ConversionBuilderToRevit<TConvertable, TReturn, TSelf>
  {
    public TConvertable SpeckleObject => ConvertableObject;
    public TReturn ReturnObject 
    {
      get => ConversionResult;
      set => ConversionResult = value; 
    }
    
    public abstract Document Doc { get; }
  }
}
