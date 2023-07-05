using Speckle.Core.Plugins;
using SpeckleRhino.Preferences;

namespace SpeckleRhino.State
{
  public class UserState : IUserState
  {
    public UserPreferences UserPreferences { get; }

    public UserState()
    {
        this.UserPreferences = UserPreferences.Load();
    }
  }
}
