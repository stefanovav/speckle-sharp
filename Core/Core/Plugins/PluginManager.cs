using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Speckle.Core.Kits;
using Speckle.Core.Models;

namespace Speckle.Core.Plugins
{
  public static class PluginManager
  {
    private static string? _pluginsFolder;

    public static readonly AssemblyName SpeckleAssemblyName = typeof(Base).GetTypeInfo().Assembly.GetName();

    public static IEnumerable<IView> GetPluginViews(IApp app)
    {
      var revitMapperPath = "C:\\Users\\oguzh\\Documents\\Git\\Speckle\\speckle-sharp\\RevitMapper\\bin\\Debug\\RevitMapper.dll";

      List<string> paths = new List<string>()
      {
        revitMapperPath
      };

      List<IView> views = new List<IView>();

      foreach (var path in paths)
      {
        Assembly assembly = Assembly.LoadFile(path);

        Type pluginType = assembly.GetTypes()[2];

        SpecklePlugin plugin = (SpecklePlugin)Activator.CreateInstance(pluginType);
        views.Add(plugin.OnLoad(app));
      }
      return views;
    }

    public static IEnumerable<IView> GetPluginViews2(IApp app)
    {
      List<Assembly> assemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
      assemblies.AddRange(KitManager.GetReferencedAssemblies());

      foreach (var assembly in assemblies)
      {
        if (assembly.IsDynamic || assembly.ReflectionOnly)
          continue;
        if (!assembly.IsReferencing(SpeckleAssemblyName))
          continue;

        var kitClass = GetPluginClass(assembly);
        if (kitClass == null)
          continue;

        if (Activator.CreateInstance(kitClass) is SpecklePlugin specklePlugin)
          yield return specklePlugin.OnLoad(app);
      }
    }

    private static Type? GetPluginClass(Assembly assembly)
    {
      try
      {
        var pluginClass = assembly
          .GetTypes()
          .FirstOrDefault(type =>
          {
            return type.GetConstructors().Any(constructor => constructor.Name == nameof(SpecklePlugin));
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
