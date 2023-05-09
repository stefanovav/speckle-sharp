using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterRevitShared.Interfaces
{
  internal interface IHasExistingObject<TExisting>
  {
    public TExisting ExistingObject { get; set; }
  }
}
