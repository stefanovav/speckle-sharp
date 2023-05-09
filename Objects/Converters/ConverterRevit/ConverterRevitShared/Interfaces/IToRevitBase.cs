using System;
using System.Collections.Generic;
using System.Text;
using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  //internal interface IToRevitBase<TSpeckleObject, TRevitObject> : IHasSpeckleObject<TSpeckleObject>, IHasRevitObject<TRevitObject>
  //  where TSpeckleObject : Base
  //{
  //}
  internal interface IToRevitBase<TSpeckleObject, TReturn> : IConversion<TSpeckleObject, TReturn>
    where TReturn : class
    where TSpeckleObject : Base
  {
  }
}
