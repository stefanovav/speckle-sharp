using ConverterRevitShared.Interfaces;
using Speckle.Core.Models;

namespace ConverterRevitShared.Extensions
{
  internal static class IToRevitBaseExtensions
  {
    public static TRevit ReturnRevitObject<TSpeckle, TRevit>
      (this IToRevitBase<TSpeckle, TRevit> self)
      where TSpeckle : Base
    {
      return self.RevitObject;
    }
  }
}
