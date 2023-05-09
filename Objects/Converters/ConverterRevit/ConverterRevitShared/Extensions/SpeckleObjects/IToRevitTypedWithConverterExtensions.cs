using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using ConverterRevitShared.Conversions;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class IToRevitTypedWithConverterExtensions
  {
    public static SELF GetExistingObjectByAppId<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : Element
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TSpeckle, TRevit, SELF>, 
        IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
        IHasExistingObject<TRevit>
    {
      var builder = (SELF)self;

      void GetExistingObjectByAppIdDefinition()
      {
        builder.ExistingObject = builder.Converter.GetExistingElementByApplicationId<TRevit>(builder.SpeckleObject.applicationId);

        // skip if element already exists in doc & receive mode is set to ignore
        if (builder.Converter.IsIgnore(builder.ReturnObject, builder.AppObj, out _))
        {
          throw new Exception("");
        }
      }

      builder.Do(GetExistingObjectByAppIdDefinition);
      return builder;
    }
    public static SELF GetRevitType<TSpeckle, TReturn, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TReturn, TRevitType, SELF> self)
      where TSpeckle : Base
      where TReturn : class
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TSpeckle, TReturn, SELF>, IToRevitTypedWithConverter<TSpeckle, TReturn, TRevitType, SELF>
    {
      var builder = (SELF)self;

      void GetRevitTypeDefinition()
      {
        builder.RevitObjectType = builder.Converter.GetElementType<TRevitType>(builder.SpeckleObject, builder.AppObj, out bool isExactMatch);
        if (builder.RevitObjectType == null)
        {
          throw new Exception("");
        }
      }

      builder.Do(GetRevitTypeDefinition);
      return builder;
    }
    //public static SELF DefaultSetParameters<TSpeckle, TRevit, TRevitType, SELF>
    //  (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
    //  where TSpeckle : Base
    //  where TRevit : class
    //  where TRevitType : ElementType
    //  where SELF : ConversionBuilderToRevit<TSpeckle,TRevit, SELF>,
    //    IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
    //    IDefaultSetParameters
    //{
    //  var builder = (SELF)self;

    //  void DefaultSetParametersDefinition()
    //  {
    //    var exclusions = new List<string>();
    //    foreach (var kvp in builder.Parameters)
    //    {
    //      exclusions.Add(kvp.Key.ToString());
    //      if (@object is Element el)
    //      {
    //        builder.Converter.TrySetParam(builder.ReturnObject, kvp.Key, el);
    //      }
    //      else
    //      {
    //        builder.Converter.TrySetParam(builder.ReturnObject, kvp.Key, kvp.Value.Value, units);
    //      }
    //    }
    //    Converter.SetInstanceParameters(nativeObjectToCreate, @base, exclusions);
    //  }

    //  builder.Do(DefaultSetParametersDefinition);
    //  return builder;
    //}
  }
}
