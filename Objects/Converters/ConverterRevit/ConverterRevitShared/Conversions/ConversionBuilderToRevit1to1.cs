using System.Collections.Generic;
using Autodesk.Revit.DB;
using ConverterRevitShared.Extensions.RevitObjects;
using Microsoft.Extensions.Caching.Memory;
using Speckle.Core.Models;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilderToRevit1to1<TConvertable, TRevit, TSelf> : ConversionBuilderToRevit<TConvertable, TRevit, TSelf>
    where TConvertable : Base
    where TRevit : Element
    where TSelf : ConversionBuilderToRevit1to1<TConvertable, TRevit, TSelf>
  {
    public ConversionBuilderToRevit1to1(TConvertable speckleObject, Dictionary<string, string> settings, IMemoryCache cache) : base(speckleObject, settings, cache)
    {
    }
    public TRevit RevitObject => ReturnObject;
    public override Document Doc => ReturnObject.Document;
    public override Transform ReferencePointTransform => Doc.GetDocReferencePointTransform(Settings, MemoryCache);
  }
}
