using Autodesk.Revit.DB;
using ConverterRevitShared.Classes.Abstract;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;
using System;

namespace ConverterRevitShared.ConversionSteps
{
  internal abstract class ConversionStep<TBuilder>
  {
    protected TBuilder Builder;
    public ConversionStep<TBuilder> Next;
    public ConversionStep(TBuilder conversionBuilder)
    {
      this.Builder = conversionBuilder;
    }

    public virtual void Handle()
    {
      if (Next != default)
      {
        Next.Handle();
      }
    }
    public void Add(ConversionStep<TBuilder> step)
    {
      if (Next != null) Next.Add(step);
      else Next = step;
    }
  }
  internal sealed class GetExistingRevitObject<TSpeckle, TRevit, TRevitType, TConverter> : ConversionStep<TConverter>
    where TSpeckle : Base
    where TRevit : Element
    where TRevitType : ElementType
    where TConverter : ConversionBuilder<TRevit, TConverter>,
      ISpeckleObject<TSpeckle>,
      IRevitType<TRevitType>,
      IConverterObject,
      IApplicationObject
  {
    public GetExistingRevitObject(TConverter conversionBuilder) : base(conversionBuilder)
    {
    }

    public override void Handle()
    {
      Builder.RevitObject = Builder.Converter.GetExistingElementByApplicationId<TRevit>(Builder.SpeckleObject.applicationId);

      // skip if element already exists in doc & receive mode is set to ignore
      if (Builder.Converter.IsIgnore(Builder.RevitObject, Builder.AppObj, out _))
      {
        throw new Exception("");
      }
    }
  }
  internal sealed class GetRevitType<TSpeckle, TRevit, TRevitType, TConverter> : ConversionStep<TConverter>
    where TSpeckle : Base
    where TRevitType : ElementType
    where TConverter : ConversionBuilder<TRevit, TConverter>,
      ISpeckleObject<TSpeckle>,
      IRevitType<TRevitType>,
      IConverterObject,
      IApplicationObject
  {
    public GetRevitType(TConverter conversionBuilder) : base(conversionBuilder)
    {
    }

    public override void Handle()
    {
      Builder.RevitObjectType = Builder.Converter.GetElementType<TRevitType>(Builder.SpeckleObject, Builder.AppObj, out bool isExactMatch);
      if (Builder.RevitObjectType == null)
      {
        throw new Exception("");
        //appObj.Update(status: ApplicationObject.State.Failed);
        //return appObj;
      }
    }
  }
}
