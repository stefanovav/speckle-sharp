using System;
using System.Collections.Generic;
using System.Text;
using DUI3.State;

namespace DUI3.App
{
  public interface IApp
  {
    public List<IBinding> Bindings { get; }
    public IAppState AppState { get; }
  }
}
