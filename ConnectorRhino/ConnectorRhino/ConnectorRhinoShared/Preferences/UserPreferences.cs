using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using DesktopUI2.Models;
using Speckle.Core.Transports;
using Speckle.Newtonsoft.Json;

namespace SpeckleRhino.Preferences
{
  public class UserPreferences
  {
    private static SQLiteTransport ConfigStorage = new(scope: "Config");

    public bool DarkTheme { set; get; }

    /// <summary>
    /// Gets string that combined %AppData% path with Speckle and get the path where we save user preferences.
    /// </summary>
    private static string LocalPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Speckle\\Config.db");

    public static void Save(UserPreferences config)
    {
      ConfigStorage.UpdateObject("configDUI", JsonConvert.SerializeObject(config));
    }

    public static UserPreferences Load()
    {
      try
      {
        //dui and manager were sharing the same config!
        //splitting them to avoid overwriting settings
        var oldConfig = ConfigStorage.GetObject("config");
        var newConfig = ConfigStorage.GetObject("configDUI");

        if (!string.IsNullOrEmpty(newConfig))
          return JsonConvert.DeserializeObject<UserPreferences>(newConfig);

        return JsonConvert.DeserializeObject<UserPreferences>(oldConfig);
      }
      catch (Exception e) { }
      return new UserPreferences();
    }

    /// <summary>
    /// Read user preferences from local.rhino.json file.
    /// If the file <see cref="LocalPath"/> does not exist, return empty <see cref="UserPreferences"/> object.
    /// </summary>
    /// <returns> Read user preferences.</returns>
    public static UserPreferences Read()
    {
      UserPreferences userPreferences = new UserPreferences();
      if (File.Exists(LocalPath))
      {
        try
        {
          // Read from json
          string? localJSON = File.ReadAllText(LocalPath);

          // Deserialize string to UserPreferences object
          userPreferences = JsonConvert.DeserializeObject<UserPreferences>(localJSON);
        }
        catch (IOException e)
        {
          Console.WriteLine("Error reading from {0}. Message = {1}", LocalPath, e.Message);
        }
        catch (JsonSerializationException e)
        {
          Console.WriteLine("Error converting JSON from {0}. Message = {1}", LocalPath, e.Message);
        }
      }

      return userPreferences;
    }

    /// <summary>
    /// Write user preferences to local.rhino.json file.
    /// </summary>
    private void Save()
    {
      // Serialize object
      string serializedUserPreferencesJSON = JsonConvert.SerializeObject(this);

      // Write serialized string to file
      File.WriteAllText(LocalPath, serializedUserPreferencesJSON);
    }
  }
}
