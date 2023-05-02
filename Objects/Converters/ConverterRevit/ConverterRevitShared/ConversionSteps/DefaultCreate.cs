using System;
using Autodesk.Revit.DB;
using ConverterRevitShared.Classes.Abstract;
using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.ConversionSteps
{
  internal interface IDefaultCreate<TRevitObject>
  {
    public TRevitObject Create();
  }
  internal sealed class DefaultCreate<TSpeckle, TRevit, TRevitType, TConverter> : ConversionStep<TConverter>
    where TSpeckle : Base
    where TRevit : class
    where TRevitType : ElementType
    where TConverter : ConversionBuilder<TRevit, TConverter>,
      ISpeckleObject<TSpeckle>,
      IRevitType<TRevitType>,
      IConverterObject,
      IApplicationObject,
      IDefaultCreate<TRevit>
  {
    public DefaultCreate(TConverter conversionBuilder) : base(conversionBuilder)
    {
    }

    public override void Handle()
    {
      if (Builder.RevitObject is Element element)
      {
        Builder.Converter.Doc.Delete(element.Id);
      }
      Builder.RevitObject = Builder.Create();

      if (Builder.RevitObject == default(TRevit))
      {
        throw new Exception("");
      }
      Builder.AppObj.Status = ApplicationObject.State.Created;
    }
  }
}
