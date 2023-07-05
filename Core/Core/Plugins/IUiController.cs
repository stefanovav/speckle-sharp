using System.Collections.Generic;

namespace Speckle.Core.Plugins
{
  public interface IUiController
  {
    public List<IView> Views { get; }
    public void UpdateUI(IAppState state);
    public void ExecuteScript(string eventName, string eventMessage);
  }
}
