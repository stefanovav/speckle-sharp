using Autodesk.Revit.DB;
using ConverterRevitShared.Conversions;
using System;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.Extensions.SpeckleObjects
{
  internal static class IToRevitBaseExtensions
  {
    public static SELF ValidateToRevit<TSpeckle, TReturn, SELF>(this IToRevitBase<TSpeckle, TReturn, SELF> self)
      where TSpeckle : Base
      where TReturn : class
      where SELF : ConversionBuilderToRevit<TSpeckle, TReturn, SELF>,
        IToRevitBase<TSpeckle, TReturn, SELF>,
        IValidateToRevit<TSpeckle>
    {
      var builder = (SELF)self;

      void ValidateToRevitDefinition()
      {
        builder.ValidateToRevit(builder.ConvertableObject);
      }

      builder.Do(ValidateToRevitDefinition);
      return builder;
    }
    public static SELF TryCreateIfDefaultReturnObjectThenThrowIfStillDefault<TSpeckle, TReturn, SELF>
      (this IToRevitBase<TSpeckle, TReturn, SELF> self)
      where TSpeckle : Base
      where TReturn : class
      where SELF : ConversionBuilderToRevit<TSpeckle, TReturn, SELF>,
        IToRevitBase<TSpeckle, TReturn, SELF>,
        ICreate<TReturn>,
        IHasApplicationObject
    {
      var builder = (SELF)self;

      void TryCreateIfDefaultReturnObjectThenThrowIfStillDefaultDefinition()
      {
        if (builder.ReturnObject is Element element)
        {
          builder.Doc.Delete(element.Id);
        }
        builder.ReturnObject = builder.Create();

        if (builder.ReturnObject == default(TReturn))
        {
          throw new Exception("");
        }
        builder.AppObj.Status = ApplicationObject.State.Created;
      }

      builder.Do(TryCreateIfDefaultReturnObjectThenThrowIfStillDefaultDefinition);
      return builder;
    }
    public static SELF TryUpdateExistingObjectThenAssignReturnObject<TSpeckle, TReturn, SELF>
      (this IToRevitBase<TSpeckle, TReturn, SELF> self)
      where TSpeckle : Base
      where TReturn : class
      where SELF : ConversionBuilderToRevit<TSpeckle, TReturn, SELF>,
        IToRevitBase<TSpeckle, TReturn, SELF>,
        IHasExistingObject<TReturn>,
        IUpdate<TReturn>,
        IHasApplicationObject
    {
      var builder = (SELF)self;

      void TryDefaultUpdateDefinition()
      {
        if (builder.ExistingObject != default(TReturn))
        {
          builder.Update(builder.ExistingObject);
          builder.ReturnObject = builder.ExistingObject;
          builder.AppObj.Status = ApplicationObject.State.Updated;
        }
      }

      builder.Do(TryDefaultUpdateDefinition);
      return builder;
    }
  }
}
