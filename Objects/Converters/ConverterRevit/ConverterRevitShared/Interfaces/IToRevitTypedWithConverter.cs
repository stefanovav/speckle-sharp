using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  internal interface IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF> :
    IToRevitBase<TSpeckleObject, TRevitObject>,
    IHasRevitType<TRevitObjectType>,
    IHasRevitConverter,
    IHasApplicationObject
    where TSpeckleObject : Base
    where TRevitObjectType : ElementType
    where SELF : IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
  {
  }
}
