using System;
using RevitMapper.Actions;
using Speckle.Core.Connectors;

namespace RevitMapper.Commands
{
  public class InitMapper : ICommand
  {
    public string Name => "init_mapper";

    public Guid ResolveId { get; }

    public IApp App { get; }

    public InitMapper(IApp app, Guid resolveId)
    {
      this.App = app;
      this.ResolveId = resolveId;
    }

    public void Execute(object data)
    {
      IAction action = new InitMapperAction();
      this.App.UpdateState(action, this.ResolveId);
    }
  }
}
