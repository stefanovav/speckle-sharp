using System;

namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Speckle application interface that represents top object instance on Host Application.
  /// Each connector must have a class that created from this interface.
  /// Created IApp instance on host application code base is responsible to initialize views to call commands and manage state accordingly.
  /// </summary>
  public interface IApp
  {
    /// <inheritdoc cref="IAppState"/>
    public IAppState State { get; }

    /// <inheritdoc cref="IUIController"/>
    public IUIController UiController { get; }

    /// <summary>
    /// Update state of the application by calling action.
    /// </summary>
    /// <param name="action"> Action to call UpdateState function.</param>
    /// <param name="resolveId"> Resolve id that comes from caller command. </param>
    public void UpdateState(IAction action, Guid resolveId);

    /// <summary>
    /// Init main app view to handle core Speckle commands.
    /// </summary>
    public void InitAppView();

    /// <summary>
    /// Register plugin views if any.
    /// </summary>
    public void RegisterPlugins();
  }
}
