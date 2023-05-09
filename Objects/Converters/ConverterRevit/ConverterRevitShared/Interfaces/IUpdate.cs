using System;
using System.Collections.Generic;
using System.Text;

namespace ConverterRevitShared.Interfaces
{
  internal interface IUpdate<TExisting>
  {
    public void Update(TExisting existing);
  }
}
