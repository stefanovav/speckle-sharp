using System;
using System.Collections.Generic;
using System.Text;

namespace Speckle.Core.Plugins
{
  public interface IApp
  {
    public IAppState State { get; }
    public IUiController UiController { get; }
    public void UpdateState(IAction action);
  }
}
