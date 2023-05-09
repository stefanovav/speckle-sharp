using System.Collections.Generic;
using Autodesk.Revit.DB;
using ConverterRevitShared.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Speckle.Core.Models;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilderToRevit<TConvertable, TReturn, TSelf> : ConversionBuilder<TConvertable, TReturn, TSelf>,
    IRevitContext
    where TConvertable : Base
    where TSelf : ConversionBuilderToRevit<TConvertable, TReturn, TSelf>
  {
    public ConversionBuilderToRevit(TConvertable speckleObject, Dictionary<string,string> settings, IMemoryCache cache) : base(speckleObject)
    {
      Settings = settings;
      MemoryCache = cache;
    }
    public TConvertable SpeckleObject => ConvertableObject;
    public TReturn ReturnObject 
    {
      get => ConversionResult;
      set => ConversionResult = value; 
    }
    public abstract Document Doc { get; }
    public abstract Transform ReferencePointTransform { get; }
    public Dictionary<string, string> Settings { get; set; }
    public IMemoryCache MemoryCache { get; }
  }
}
