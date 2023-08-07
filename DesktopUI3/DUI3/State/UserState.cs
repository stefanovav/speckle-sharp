using System;
using System.Collections.Generic;
using System.Text;
using Speckle.Core.Credentials;
using Speckle.Core.Transports;
using System.Text.Json;

namespace DUI3.State
{
  public class UserState
  {
    private static readonly SQLiteTransport ConfigStorage = new(scope: "Config");
    public IEnumerable<Account> Accounts { get; }
    public UserState()
    {
      this.Accounts = AccountManager.GetAccounts();
    }

    public Config GetConfig()
    {
      try
      {
        var config = ConfigStorage.GetObject("configDUI3");
        if (string.IsNullOrEmpty(config)) return new Config();
        return JsonSerializer.Deserialize<Config>(config);
      }
      catch (Exception _)
      {
        // TODO: Log error
        return new Config();
      }
    }
  }

  public class Config
  {
    public bool DarkTheme { set; get; }
    /**
     * Meant to keep track of whether the v0 onboarding has been completed or not, separated by host app. E.g.:
     * { "Rhino" : true, "Revit": false }
     */
    public Dictionary<string, bool> OnboardingV0 { get; set; } = new Dictionary<string, bool>();
  }
}
