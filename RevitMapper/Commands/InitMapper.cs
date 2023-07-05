using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RevitMapper.Actions;
using Speckle.Core.Plugins;

namespace RevitMapper.Commands
{
  public class InitMapper : ICommand
  {
    public string Name => "init_mapper";

    public IApp App { get; }

    public InitMapper(IApp app)
    {
      this.App = app;
    }

    public void Execute(object data)
    {
      IAction action = new InitMapperAction();
      this.App.UpdateState(action);
    }
  }
}
