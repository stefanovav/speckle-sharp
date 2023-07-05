using System;
using System.Collections.Generic;
using System.Text;

namespace Speckle.Core.Plugins
{
  public interface IView
    {
      public Guid Id { get; }
      public string Name { get; }
      public IApp App { get; }
      public IDictionary<string, ICommand> Commands { get; }
      public void UpdateView();
    }
}
