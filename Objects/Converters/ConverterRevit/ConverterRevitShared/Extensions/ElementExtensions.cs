#nullable enable
using System.Collections.Generic;
using System.Linq;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Electrical;
using Autodesk.Revit.DB.Mechanical;
using Autodesk.Revit.DB.Plumbing;
using Objects.Converter.Revit;

namespace ConverterRevitShared.Extensions
{
  public static class ElementExtensions
  {
    public static IEnumerable<Connector> GetConnectorSet(this Element element)
    {
      var empty = Enumerable.Empty<Connector>();
      return element switch
      {
        FamilyInstance o => o.MEPModel?.ConnectorManager?.Connectors?.Cast<Connector>() ?? empty,
        MEPCurve o => o.ConnectorManager?.Connectors?.Cast<Connector>() ?? empty,
        _ => empty,
      };
    }
    public static MEPSystem? GetMEPSystem(this Element element)
    {
      switch (element)
      {
        case MEPCurve o:
          return o.MEPSystem;
        case FamilyInstance o:
          var systemName = ConverterRevit.GetParamValue<string>(element, BuiltInParameter.RBS_SYSTEM_NAME_PARAM);
          foreach (var connector in o.MEPModel?.ConnectorManager?.Connectors.Cast<Connector>())
          {
            if (connector.MEPSystem != null && string.Equals(connector.MEPSystem.Name, systemName))
            {
              return connector.MEPSystem;
            }
          }
          break;
      }
      return null;
    }
  }
}
