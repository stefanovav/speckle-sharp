using System;
using Speckle.Core.Connectors;

namespace Speckle.Core.Plugins
{
  public abstract class SpecklePlugin : IPlugin
  {
    public abstract Guid Id { get; }
    public abstract string Name { get; }
    public abstract string Url { get; }
    public abstract string Author { get; }
    public virtual string Description { get; } = "";
    public virtual string WebsiteOrEmail { get; } = "";
    public abstract IView OnLoad(IApp app);
  }
}
