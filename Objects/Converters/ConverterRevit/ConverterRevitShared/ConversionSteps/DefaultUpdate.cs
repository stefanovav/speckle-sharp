using System;
using Autodesk.Revit.DB;
using ConverterRevitShared.Classes.Abstract;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Api;
using Speckle.Core.Models;

namespace ConverterRevitShared.ConversionSteps
{
  internal interface IDefaultUpdate
  {
    public void Update();
  }
  internal sealed class DefaultUpdate<TSpeckle, TRevit, TRevitType, TConverter> : ConversionStep<TConverter>
    where TSpeckle : Base
    where TRevit : class
    where TRevitType : ElementType
    where TConverter : ConversionBuilder<TRevit, TConverter>,
      ISpeckleObject<TSpeckle>,
      IRevitType<TRevitType>,
      IConverterObject,
      IApplicationObject,
      IDefaultUpdate
  {
    public DefaultUpdate(TConverter conversionBuilder) : base(conversionBuilder)
    {
    }

    public override TConverter Handle()
    {
      if (Builder.RevitObject != default(TRevit))
      {
        Builder.Update();
        Builder.AppObj.Status = ApplicationObject.State.Updated;
      }
      return Builder;
    }
  }
}
