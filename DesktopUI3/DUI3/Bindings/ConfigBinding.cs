using System;
using System.Collections.Generic;
using System.Text.Json;
using DUI3.App;
using DUI3.State;
using Speckle.Core.Transports;

namespace DUI3.Bindings;

public class ConfigBinding : IBinding
{
  public IApp App { get; }

  public ConfigBinding(IApp speckleApp)
  {
    this.App = speckleApp;
  }

  public string Name { get; set; } = "configBinding";
  public IBridge Parent { get; set; }

  private static readonly SQLiteTransport ConfigStorage = new(scope: "Config");
  
  public Config GetConfig()
  {
    return this.App.AppState.UserState.GetConfig();
  }

  public void UpdateConfig(Config config)
  {
    try
    {
      ConfigStorage.UpdateObject("configDUI3", JsonSerializer.Serialize(config));
    }
    catch (Exception e)
    {
      // TODO: Log error
    }
  }
}
