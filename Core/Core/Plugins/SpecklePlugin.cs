using System;

namespace Speckle.Core.Plugins
{
  public abstract class SpecklePlugin : IPlugin
  {
    public Guid Id { get; }
    public string Description { get; }
    public string Name { get; }
    public string Author { get; }
    public string WebsiteOrEmail { get; }
    public string Url { get; }
    public abstract IView OnLoad(IApp app);
  }
}
