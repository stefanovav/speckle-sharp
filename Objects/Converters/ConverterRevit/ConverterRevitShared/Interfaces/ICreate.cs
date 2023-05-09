using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterRevitShared.Interfaces
{
  internal interface ICreate<TRevit>
  {
    public TRevit Create();
  }
}
