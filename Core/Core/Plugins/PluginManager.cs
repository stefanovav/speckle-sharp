using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Speckle.Core.Helpers;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace Speckle.Core.Plugins
{
  public static class PluginManager
  {
    private static string? _pluginsFolder;

    public static readonly AssemblyName SpeckleAssemblyName = typeof(Base).GetTypeInfo().Assembly.GetName();

    /// <summary>
    /// Local installations store plugins in C:\Users\USERNAME\AppData\Roaming\Speckle\Plugins
    /// Admin/System-wide installations in C:\ProgramData\Speckle\Plugins
    /// </summary>
    public static string PluginsFolder
    {
      get => _pluginsFolder ??= SpecklePathProvider.PluginsFolderPath;
      set => _pluginsFolder = value;
    }

    public static IEnumerable<IView> GetPluginViews(IApp app, string hostApp)
    {
      string hostAppPluginsFolder = Path.Combine(PluginsFolder, hostApp);

      if (!Directory.Exists(hostAppPluginsFolder))
      {
        return Enumerable.Empty<IView>();
      }

      List<string> directories = Directory.GetDirectories(hostAppPluginsFolder).ToList();

      if (directories.Count == 0)
      {
        return Enumerable.Empty<IView>();
      }

      List<IView> views = new List<IView>();

      foreach (var directory in directories)
      {
        foreach (var assemblyPath in Directory.EnumerateFiles(directory, "*.spl"))
        {
          Assembly assembly = Assembly.LoadFile(assemblyPath);

          Type pluginType = GetPluginClass(assembly);

          SpecklePlugin plugin = (SpecklePlugin)Activator.CreateInstance(pluginType);
          views.Add(plugin.OnLoad(app));
        }
      }

      return views;
    }

    public static IEnumerable<IView> GetPluginViews2(IApp app)
    {
      List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
      assemblies.AddRange(KitManager.GetReferencedAssemblies());

      List<IView> views = new List<IView>();

      foreach (var assembly in assemblies)
      {
        if (assembly.IsDynamic || assembly.ReflectionOnly)
          continue;
        if (!assembly.IsReferencing(SpeckleAssemblyName))
          continue;

        var pluginClass = GetPluginClass(assembly);
        if (pluginClass == null)
          continue;

        if (pluginClass.IsAbstract)
        {
          continue;
        }

        if (Activator.CreateInstance(pluginClass) is SpecklePlugin specklePlugin)
          views.Add(specklePlugin.OnLoad(app));
      }
      return views;
    }

    private static Type? GetPluginClass(Assembly assembly)
    {
      try
      {
        var pluginClass = assembly
          .GetTypes()
          .FirstOrDefault(type =>
          {
            return type.GetInterfaces().Any(interf4ce => interf4ce.Name == nameof(IPlugin));
          });

        return pluginClass;
      }
      catch
      {
        // this will be a ReflectionTypeLoadException and is expected. we don't need to care!
        return null;
      }
    }
  }
}
