using System;
using System.Collections.Generic;
using System.Text;

namespace Speckle.Core.Plugins
{
  public interface ICommand
    {
      public string Name { get; }

      public IApp App { get; }

      public void Execute(object data);
    }
}
