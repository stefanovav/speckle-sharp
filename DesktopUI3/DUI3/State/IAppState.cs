namespace DUI3.State
{
  public interface IAppState
  {
    public UserState UserState { get; }
    public ISpeckleState SpeckleState { get; }
  }
}
