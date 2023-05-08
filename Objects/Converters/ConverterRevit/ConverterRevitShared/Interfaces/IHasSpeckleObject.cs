using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  internal interface IHasSpeckleObject<TSpeckleObject>
    where TSpeckleObject : Base
  {
    public TSpeckleObject SpeckleObject { get; }
  }
}
