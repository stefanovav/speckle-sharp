using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Speckle.ConnectorRevit;

namespace ConnectorRevit.Extensions
{
  internal static class DocumentExtensions
  {
    public static List<Element> SupportedElements(this Document doc)
    {
      //get element types of supported categories
      var categoryFilter = new LogicalOrFilter(
        ConnectorRevitUtils.GetCategories(doc).Select(x => new ElementCategoryFilter(x.Value.Id)).Cast<ElementFilter>().ToList()
      );

      List<Element> elements = new FilteredElementCollector(doc)
        .WhereElementIsNotElementType()
        .WhereElementIsViewIndependent()
        .WherePasses(categoryFilter)
        .ToList();

      return elements;
    }

    public static List<Element> SupportedTypes(this Document doc)
    {
      //get element types of supported categories
      var categoryFilter = new LogicalOrFilter(
        ConnectorRevitUtils.GetCategories(doc).Select(x => new ElementCategoryFilter(x.Value.Id)).Cast<ElementFilter>().ToList()
      );

      List<Element> elements = new FilteredElementCollector(doc)
        .WhereElementIsElementType()
        .WherePasses(categoryFilter)
        .ToList();

      return elements;
    }

    public static List<View> Views2D(this Document doc)
    {
      List<View> views = new FilteredElementCollector(doc)
        .WhereElementIsNotElementType()
        .OfCategory(BuiltInCategory.OST_Views)
        .Cast<View>()
        .Where(
          x =>
            x.ViewType == ViewType.CeilingPlan
            || x.ViewType == ViewType.FloorPlan
            || x.ViewType == ViewType.Elevation
            || x.ViewType == ViewType.Section
        )
        .ToList();

      return views;
    }

    public static List<View> Views3D(this Document doc)
    {
      List<View> views = new FilteredElementCollector(doc)
        .WhereElementIsNotElementType()
        .OfCategory(BuiltInCategory.OST_Views)
        .Cast<View>()
        .Where(x => x.ViewType == ViewType.ThreeD)
        .ToList();

      return views;
    }

    public static List<Element> Levels(this Document doc)
    {
      List<Element> levels = new FilteredElementCollector(doc)
        .WhereElementIsNotElementType()
        .OfCategory(BuiltInCategory.OST_Levels)
        .ToList();

      return levels;
    }

    public static List<string> GetCategoryNames(this Document doc)
    {
      return ConnectorRevitUtils.GetCategories(doc).Keys.OrderBy(x => x).ToList();
    }

    public static List<string> GetViewFilterNames(this Document doc)
    {
      return ConnectorRevitUtils.GetFilters(doc).Select(x => x.Name).ToList();
    }

    public static List<string> GetWorksets(this Document doc)
    {
      return new FilteredWorksetCollector(doc)
        .Where(x => x.Kind == WorksetKind.UserWorkset)
        .Select(x => x.Name)
        .ToList();
    }
  }
}
