using System;
using System.Collections.Generic;
using System.Text;

namespace Speckle.Core.Plugins
{
  public interface IAction
    {
      public IAppState UpdateState(IAppState state);
    }
}
