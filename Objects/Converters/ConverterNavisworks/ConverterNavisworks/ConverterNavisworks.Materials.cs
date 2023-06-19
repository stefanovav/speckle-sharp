using System;
using System.Collections.Generic;
using Autodesk.Navisworks.Api;
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

  private static RenderMaterial TranslateMaterial(ModelItem geom)
  {
    // Already there anticipating other options becoming possible and set in the settings.
    // var colorMode;
    var hasColorMode = Settings.TryGetValue("color-mode", out var colorMode);

    if (hasColorMode == false || colorMode == null)
      colorMode = "rendered";

    if (colorMode == "rendered")
    {
      RenderMaterial renderMaterial = NavisworksRenderMaterialToSpeckle(geom);
    }

    Color renderColor = colorMode switch
    {
      "original" => NavisworksColorToColor(geom.Geometry.OriginalColor),
      "active" => NavisworksColorToColor(geom.Geometry.ActiveColor),
      "permanent" => NavisworksColorToColor(geom.Geometry.PermanentColor),
      _ => new Color()
    };

    double transparency = colorMode switch
    {
      "original" => geom.Geometry.OriginalTransparency,
      "active" => geom.Geometry.ActiveTransparency,
      "permanent" => geom.Geometry.PermanentTransparency,
      _ => 0
    };

    var materialName = $"NavisworksMaterial_{Math.Abs(renderColor.ToArgb())}";

    var black = Color.FromArgb(Convert.ToInt32(0), Convert.ToInt32(0), Convert.ToInt32(0));

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

    var r = new RenderMaterial(1 - transparency, 0, 1, renderColor, black) { name = materialName };

    return r;
  }

  private static RenderMaterial NavisworksRenderMaterialToSpeckle(ModelItem item)
  {
    var renderMaterial = new RenderMaterial();
    var itemCategory = item.PropertyCategories.FindCategoryByDisplayName("Item");
    var itemProperties = itemCategory?.Properties;
    var itemMaterial = itemProperties?.FindPropertyByDisplayName("Material");
    
    var materialPropertyCategory = item.PropertyCategories.FindCategoryByDisplayName("Material");
    var materialProperties = materialPropertyCategory?.Properties;

    return renderMaterial;
  }
}
