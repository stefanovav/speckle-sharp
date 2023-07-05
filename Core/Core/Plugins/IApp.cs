namespace Speckle.Core.Plugins
{
  /// <summary>
  /// Speckle application interface that represents top object instance on Host Application.
  /// Each connector must have a class that created from this interface.
  /// Created IApp instance on host application code base is responsible to manage state and UI communication.
  /// </summary>
  public interface IApp
  {
    public IAppState State { get; }
    public IUiController UiController { get; }
    public void UpdateState(IAction action);
    public void InitAppView();
    public void RegisterPlugins();
  }
}
