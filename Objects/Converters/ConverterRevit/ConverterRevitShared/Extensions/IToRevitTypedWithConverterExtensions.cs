using System;
using Autodesk.Revit.DB;
using ConverterRevitShared.Conversions;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.Extensions
{
  internal static class IToRevitTypedWithConverterExtensions
  {
    public static SELF GetExistingRevitObject<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : Element
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TRevit, SELF>, IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>
    {
      var builder = (SELF)self;

      void GetExistingRevitObjectDefinition()
      {
        builder.RevitObject = builder.Converter.GetExistingElementByApplicationId<TRevit>(builder.SpeckleObject.applicationId);

        // skip if element already exists in doc & receive mode is set to ignore
        if (builder.Converter.IsIgnore(builder.RevitObject, builder.AppObj, out _))
        {
          throw new Exception("");
        }
      }

      builder.Do(GetExistingRevitObjectDefinition);
      return builder;
    }
    public static SELF GetRevitType<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TRevit, SELF>, IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>
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
    public static SELF TryDefaultCreate<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : class
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TRevit, SELF>,
        IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
        IDefaultCreate<TRevit>
    {
      var builder = (SELF)self;

      void TryDefaultCreateDefinition()
      {
        if (self.RevitObject is Element element)
        {
          self.Converter.Doc.Delete(element.Id);
        }
        self.RevitObject = builder.Create();

        if (self.RevitObject == default(TRevit))
        {
          throw new Exception("");
        }
        self.AppObj.Status = ApplicationObject.State.Created;
      }

      builder.Do(TryDefaultCreateDefinition);
      return builder;
    }
    public static SELF TryDefaultUpdate<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : class
      where TRevitType : ElementType
      where SELF : ConversionBuilderToRevit<TRevit, SELF>,
        IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
        IDefaultUpdate
    {
      var builder = (SELF)self;

      void TryDefaultUpdateDefinition()
      {
        if (builder.RevitObject != default(TRevit))
        {
          builder.Update();
          builder.AppObj.Status = ApplicationObject.State.Updated;
        }
      }

      builder.Do(TryDefaultUpdateDefinition);
      return builder;
    }
  }
}
