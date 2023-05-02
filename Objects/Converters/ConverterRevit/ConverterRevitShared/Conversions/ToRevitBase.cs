using DB = Autodesk.Revit.DB;
using Autodesk.Revit.DB;
using Speckle.Core.Models;
using ConverterRevitShared.Interfaces;
using System;
using ConverterRevitShared.ConversionSteps;

namespace ConverterRevitShared.Classes.Abstract
{
  internal abstract class ConversionBuilder<TRevit, TSelf> : IRevitObject<TRevit>
    where TSelf : ConversionBuilder<TRevit, TSelf>
  {
    private ConversionStep<TSelf> conversionRoot;

    public TRevit RevitObject { get; set; }

    public TSelf Do(ConversionStep<TSelf> step)
    {
      if (conversionRoot == null)
      {
        conversionRoot = step;
      }
      else
      {
        conversionRoot.Add(step); 
      }

      return (TSelf)this;
    }

    public TRevit Convert()
    {
      var currentStep = conversionRoot;
      while (currentStep != null)
      {
        try
        {
          currentStep.Handle();
        }
        catch (Exception e)
        {
          throw;
        }
        currentStep = currentStep.Next;
      }
      return RevitObject;
    }
  }

  internal static class ToRevitFluentExtensions
  {   
    public static SELF GetExistingRevitObject<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : Element
      where TRevitType : ElementType
      where SELF : ConversionBuilder<TRevit, SELF>, IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>
    {
      var builder = (ConversionBuilder<TRevit, SELF>)self;
      var typedBuilder = (SELF)builder;
      builder.Do(new GetExistingRevitObject<TSpeckle, TRevit, TRevitType, SELF>(typedBuilder));
      return typedBuilder;
    }
    public static SELF GetRevitType<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevitType : ElementType
      where SELF : ConversionBuilder<TRevit, SELF>, IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>
    {
      var builder = (ConversionBuilder<TRevit, SELF>)self;
      var typedBuilder = (SELF)builder;
      builder.Do(new GetRevitType<TSpeckle, TRevit, TRevitType, SELF>(typedBuilder));
      return typedBuilder;
    }
    public static SELF TryDefaultCreate<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : class
      where TRevitType : ElementType
      where SELF : ConversionBuilder<TRevit, SELF>, 
        IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
        IDefaultCreate<TRevit>
    {
      var builder = (ConversionBuilder<TRevit, SELF>)self;
      var typedBuilder = (SELF)builder;
      builder.Do(new DefaultCreate<TSpeckle, TRevit, TRevitType, SELF>(typedBuilder));
      return typedBuilder;
    }
    public static SELF TryDefaultUpdate<TSpeckle, TRevit, TRevitType, SELF>
      (this IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF> self)
      where TSpeckle : Base
      where TRevit : class
      where TRevitType : ElementType
      where SELF : ConversionBuilder<TRevit, SELF>, 
        IToRevitTypedWithConverter<TSpeckle, TRevit, TRevitType, SELF>,
        IDefaultUpdate
    {
      var builder = (ConversionBuilder<TRevit, SELF>)self;
      var typedBuilder = (SELF)builder;
      builder.Do(new DefaultUpdate<TSpeckle, TRevit, TRevitType, SELF>(typedBuilder));
      return typedBuilder;
    }
    public static TRevit ReturnRevitObject<TSpeckle, TRevit>
      (this IToRevitBase<TSpeckle, TRevit> self)
      where TSpeckle : Base
    {
      return self.RevitObject;
    }
  }
}
