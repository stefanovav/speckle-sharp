using System;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Commands handle functions that comes from UI in corresponding IView.
  /// TBD: We might here do error handling according to command is succesfull or not.
  ///   We might log some staff..... blllla blllla blllla...
  /// </summary>
  public interface ICommand
  {
    /// <summary>
    /// Gets name of the command.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets resolve id of the command which corresponds to the view id.
    /// </summary>
    public Guid ResolveId { get; }

    /// <summary>
    /// Gets reference of the application to execute command on it.
    /// </summary>
    public IApp App { get; }

    /// <summary>
    /// Executes the command.
    /// </summary>
    /// <param name="data"></param>
    public void Execute(object data);
  }
}
