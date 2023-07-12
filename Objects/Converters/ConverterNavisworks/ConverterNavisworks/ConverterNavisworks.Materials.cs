using System;
using System.Collections.Generic;
using Autodesk.Navisworks.Api;
using Autodesk.Navisworks.Gui.Common.View;
using Objects.Other;
using Color = System.Drawing.Color;

namespace Objects.Converter.Navisworks;

// ReSharper disable once UnusedType.Global
public partial class ConverterNavisworks
{
  private static Color NavisworksColorToColor(Autodesk.Navisworks.Api.Color color)
  {
    return Color.FromArgb(
      Convert.ToInt32(color.R * 255),
      Convert.ToInt32(color.G * 255),
      Convert.ToInt32(color.B * 255)
    );
  }

  private static Color NavisworksColorToColor(IReadOnlyDictionary<string, DataProperty> color)
  {
    Color colorFromArgb = new();
    try
    {
      colorFromArgb = Color.FromArgb(
        Convert.ToInt32(color["Red"].Value.ToDouble() * 255),
        Convert.ToInt32(color["Green"].Value.ToDouble() * 255),
        Convert.ToInt32(color["Blue"].Value.ToDouble() * 255)
      );
    }
    catch (NullReferenceException)
    {
      _ = 0;
    }

    return colorFromArgb;
  }

  private static RenderMaterial TranslateMaterial(ModelItem geom)
  {
    // Already there anticipating other options becoming possible and set in the settings.
    // var colorMode;
    var hasColorMode = Settings.TryGetValue("color-mode", out var colorMode);

    if (hasColorMode == false || colorMode == null)
      colorMode = "rendered";

    RenderMaterial renderMaterial;

    Color renderColor = colorMode switch
    {
      "original" => NavisworksColorToColor(geom.Geometry.OriginalColor),
      "active" => NavisworksColorToColor(geom.Geometry.ActiveColor),
      "permanent" => NavisworksColorToColor(geom.Geometry.PermanentColor),
      _ => NavisworksColorToColor(geom.Geometry.OriginalColor)
    };

    double transparency = colorMode switch
    {
      "original" => geom.Geometry.OriginalTransparency,
      "active" => geom.Geometry.ActiveTransparency,
      "permanent" => geom.Geometry.PermanentTransparency,
      _ => geom.Geometry.OriginalTransparency
    };

    var materialName = $"NavisworksMaterial_{ColorToInt(renderColor)}";

    var itemCategory = geom.PropertyCategories.FindCategoryByDisplayName("Item");
    if (itemCategory != null)
    {
      var itemProperties = itemCategory.Properties;
      var itemMaterial = itemProperties.FindPropertyByDisplayName("Material");
      if (itemMaterial != null && !string.IsNullOrEmpty(itemMaterial.DisplayName))
        materialName = itemMaterial.Value.ToDisplayString();
    }

    var materialPropertyCategory = geom.PropertyCategories.FindCategoryByDisplayName("Material");
    if (materialPropertyCategory != null)
    {
      var material = materialPropertyCategory.Properties;
      var name = material.FindPropertyByDisplayName("Name");
      if (name != null && !string.IsNullOrEmpty(name.DisplayName))
        materialName = name.Value.ToDisplayString();
    }

    if (colorMode == "rendered")
    {
      renderMaterial = NavisworksRenderMaterialToSpeckle(geom);
    }
    else
    {
      var black = Color.FromArgb(Convert.ToInt32(0), Convert.ToInt32(0), Convert.ToInt32(0));
      renderMaterial = new RenderMaterial(1 - transparency, 0, 1, renderColor, black) { name = materialName };
    }

    return renderMaterial;
  }

  private static RenderMaterial NavisworksRenderMaterialToSpeckle(ModelItem item)
  {
    var renderMaterial = new RenderMaterial();
    var materialPropertyCategory = item.PropertyCategories.FindCategoryByDisplayName("Material");
    var materialProperties = materialPropertyCategory?.Properties;
    if (materialProperties == null)
      return null;

    var properties = CategoriseMaterialProperties(materialProperties);

    var diffuse = NavisworksColorToColor(properties["Diffuse"]);
    var ambient = NavisworksColorToColor(properties["Ambient"]);

    Color combined;

    combined = Color.FromArgb(
      Convert.ToInt32((diffuse.R + ambient.R) / 2),
      Convert.ToInt32((diffuse.G + ambient.G) / 2),
      Convert.ToInt32((diffuse.B + ambient.B) / 2)
    );

    renderMaterial.diffuse = ColorToInt(combined);
    // renderMaterial.diffuse = ColorToInt(NavisworksColorToColor(properties["Diffuse"]));
    renderMaterial.emissive = ColorToInt(NavisworksColorToColor(properties["Emissive"]));

    renderMaterial.metalness =
      (
        properties["Specular"]["Red"].Value.ToDouble()
        + properties["Specular"]["Green"].Value.ToDouble()
        + properties["Specular"]["Blue"].Value.ToDouble()
      ) / 3;

    renderMaterial.opacity = 1 - properties["Special"]["Transparency"].Value.ToDouble();
    renderMaterial.roughness = 1 - properties["Special"]["Shininess"].Value.ToDouble();

    return renderMaterial;
  }

  private static int ColorToInt(Color color)
  {
    var abs = Math.Abs(color.ToArgb());
    var i = color.ToArgb();

    return i;
  }

  /// <summary>
  /// Categorises material properties into a structured dictionary.
  /// </summary>
  /// <param name="materialProperties">The collection of material properties to categorise.</param>
  /// <returns>A nested dictionary where the outer dictionary's keys are categories ("Ambient", "Diffuse", "Specular", "Emissive", and "Special"),
  /// and the inner dictionary's keys are property names ("Red", "Green", "Blue", "Shininess", and "Transparency").</returns>
  private static Dictionary<string, Dictionary<string, DataProperty>> CategoriseMaterialProperties(
    DataPropertyCollection materialProperties
  )
  {
    var properties = new Dictionary<string, Dictionary<string, DataProperty>>();
    var categories = new[] { "Ambient", "Diffuse", "Specular", "Emissive" };
    var propertyNames = new[] { "Red", "Green", "Blue" };

    foreach (var category in categories)
    {
      properties[category] = new Dictionary<string, DataProperty>();
      foreach (var name in propertyNames)
      {
        properties[category][name] = materialProperties?.FindPropertyByDisplayName($"{category}.{name}");
      }
    }

    // Special cases
    properties["Special"] = new Dictionary<string, DataProperty>()
    {
      { "Shininess", materialProperties?.FindPropertyByDisplayName("Shininess") },
      { "Transparency", materialProperties?.FindPropertyByDisplayName("Transparency") }
    };

    return properties;
  }
}
