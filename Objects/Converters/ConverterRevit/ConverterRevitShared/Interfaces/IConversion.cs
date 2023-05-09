using System;
using System.Collections.Generic;
using System.Text;
using Speckle.Core.Api;

namespace ConverterRevitShared.Interfaces
{
  internal interface IConversion<TConvertable, TConversion> : IHasConvertableObject<TConvertable>, IHasConversionResult<TConversion>
  {
  }
}
