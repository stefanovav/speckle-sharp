using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace ConverterRevitShared.Classes.Abstract
{
  internal interface ISpeckleObjectConverter<in TInput, out TOutput>
  {
    public ApplicationObject Convert(TInput @object);
  }
  internal interface ISpeckleToRevitBase<in TInput, out TOutput> : ISpeckleObjectConverter<TInput, TOutput>
    where TInput : Base
    where TOutput : Element
  {
  }
  internal interface ISpeckleToRevitTyped<in TInput, out TOutput, TRevitType> : ISpeckleToRevitBase<TInput, TOutput>
    where TInput : Base
    where TOutput : Element
    where TRevitType : ElementType
  {
  }
}
