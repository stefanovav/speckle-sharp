#nullable enable
using Autodesk.Revit.DB;
using ConverterRevitShared.Extensions;
using Objects.Other;
using Objects.Other.Revit;
using RevitSharedResources.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using DB = Autodesk.Revit.DB;

namespace Objects.Converter.Revit
{
  public partial class ConverterRevit
  {
    public IEnumerable<Objects.Other.MaterialQuantity> MaterialQuantitiesToSpeckle(
      DB.Element element, 
      string units)
    {
      // Convert revit interal units to speckle commit units
      double factor = ScaleToSpeckle(1);

      var speckleMaterialCache = revitDocumentAggregateCache
          .GetOrInitializeEmptyCacheOfType<Objects.Other.Material>(out _);

      var quanities = TryGetQuantitiesForElement(element, units, factor, speckleMaterialCache);
      if (!quanities.Any())
      {
        quanities = TryGetQuantitiesFromSolids(element, units, factor, speckleMaterialCache);
      }
      return quanities;

      //if (element.GetMEPSystem() is MEPSystem mepSystem
      //  && element.Document.GetElement(mepSystem.GetTypeId()) is MEPSystemType mechType
      //  && mechType.MaterialId != ElementId.InvalidElementId)
      //{
      //  var speckleMaterial = speckleMaterialCache
      //    .GetOrAdd(
      //      mechType.MaterialId.ToString(),
      //      () => MaterialToSpeckle((DB.Material)element.Document.GetElement(mechType.MaterialId)), out _
      //  );

      //  var (area, volume) = GetAreaAndVolumeOfMaterialInElement(element, mechType.MaterialId, solids);

      //  var quantities = new Objects.Other.MaterialQuantity(speckleMaterial, volume, area, units);
      //}

      //if (quantities != null)
      //{
      //  return materialQuantities;
      //}



      //return materialQuantities;
    }

    private IEnumerable<MaterialQuantity> TryGetQuantitiesFromSolids(
      Element element, 
      string units, 
      double factor, 
      IRevitObjectCache<Other.Material> speckleMaterialCache)
    {
      var (solids, _) = GetSolidsAndMeshes(element);

      foreach (var id in GetMaterialIdsFromGeometry(solids))
      {
        var speckleMaterial = speckleMaterialCache
          .GetOrAdd(
            id.ToString(),
            () => MaterialToSpeckle((DB.Material)element.Document.GetElement(id)), out _
          );
        var (area, volume) = GetAreaAndVolumeOfMaterialInElement(element, id, solids, factor);
        yield return new Objects.Other.MaterialQuantity(speckleMaterial, volume, area, units);
      }
    }

    private IEnumerable<MaterialQuantity> TryGetQuantitiesForElement(
      Element element, 
      string units, 
      double factor,
      IRevitObjectCache<Other.Material> speckleMaterialCache)
    {
      foreach (var matId in element.GetMaterialIds(false) ?? new List<ElementId>())
      {
        double volume = element.GetMaterialVolume(matId) * Math.Pow(factor, 3);
        double area = element.GetMaterialArea(matId, false) * Math.Pow(factor, 2); 

        var speckleMaterial = speckleMaterialCache
          .GetOrAdd(
            matId.ToString(),
            () => MaterialToSpeckle((DB.Material)element.Document.GetElement(matId)), out _
          );
        var materialQuantity = new Objects.Other.MaterialQuantity(speckleMaterial, volume, area, units);

        if (LocationToSpeckle(element) is ICurve curve)
          materialQuantity["length"] = curve.length;

        yield return materialQuantity;
      }
    }

    public (double area, double volume) GetAreaAndVolumeOfMaterialInElement(
      Element element, 
      ElementId materialId,
      List<Solid> solids, 
      double factor)
    {
      var filteredSolids = solids
        .Where(solid => solid.Volume > 0 
          && !solid.Faces.IsEmpty 
          && solid.Faces.get_Item(0).MaterialElementId == materialId)
        .ToList();

      var volume = filteredSolids.Sum(solid => solid.Volume * Math.Pow(factor, 3));
      var area = filteredSolids
        .Select(solid => solid.Faces.Cast<Face>()
          .Select(face => face.Area)
          .Max())
        .Sum(a => a * Math.Pow(factor, 2));

      return (area, volume);
    }

    private IEnumerable<DB.ElementId> GetMaterialIdsFromGeometry(List<Solid> solids)
    {
      return solids
        .Where(solid => solid.Volume > 0 && !solid.Faces.IsEmpty)
        .Select(m => m.Faces.get_Item(0).MaterialElementId)
        .Where(id => id != ElementId.InvalidElementId)
        .Distinct();
    }
  }
}
