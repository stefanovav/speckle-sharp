using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterRevitShared.Interfaces
{
  internal interface IValidateToRevit<TSpeckle>
  {
    public void ValidateToRevit(TSpeckle @base);
  }
}
