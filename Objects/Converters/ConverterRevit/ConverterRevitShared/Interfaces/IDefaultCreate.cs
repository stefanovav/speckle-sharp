using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterRevitShared.Interfaces
{
  internal interface IDefaultCreate<TRevit>
  {
    public TRevit Create();
  }
}
