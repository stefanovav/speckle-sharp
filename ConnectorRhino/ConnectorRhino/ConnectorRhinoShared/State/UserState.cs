using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.Preferences;

namespace SpeckleRhino.State
{
  public class UserState
  {
    public UserPreferences UserPreferences { get; }

    public UserState()
    {
        this.UserPreferences = UserPreferences.Load();
    }
  }
}
