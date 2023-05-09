using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  internal interface IToRevitTypedWithConverter<TSpeckleObject, TReturnObject, TRevitObjectType, SELF> :
    IToRevitBase<TSpeckleObject, TReturnObject>,
    IHasRevitType<TRevitObjectType>,
    IHasRevitConverter,
    IHasApplicationObject
    where TSpeckleObject : Base
    where TReturnObject : class
    where TRevitObjectType : ElementType
    where SELF : IToRevitTypedWithConverter<TSpeckleObject, TReturnObject, TRevitObjectType, SELF>
  {
  }
}
