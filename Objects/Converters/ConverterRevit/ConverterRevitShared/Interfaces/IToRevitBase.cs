using System;
using System.Collections.Generic;
using System.Text;
using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  internal interface IToRevitBase<TSpeckleObject, TReturn, SELF> : IConversion<TSpeckleObject, TReturn>
    where TReturn : class
    where TSpeckleObject : Base
    where SELF : IToRevitBase<TSpeckleObject, TReturn, SELF>
  {
  }
}
