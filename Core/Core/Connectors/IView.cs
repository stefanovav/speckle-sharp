using System;
using System.Collections.Generic;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Interface to collect and navigate commands into related scopes.
  /// </summary>
  public interface IView
  {
    /// <summary>
    /// Gets id of the view.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Gets name of the view.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets reference of the application to be able to pass it to the commands to run them under the App hood.
    /// </summary>
    public IApp App { get; }

    /// <summary>
    /// Gets registered commands.
    /// </summary>
    public IDictionary<string, ICommand> Commands { get; }
  }
}
