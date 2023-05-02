using System;
using System.Collections.Generic;
using Objects.BuiltElements;
using Objects.Converter.Revit;
using Speckle.Core.Models;
using OBR = Objects.BuiltElements.Revit;
using DB = Autodesk.Revit.DB;
using Autodesk.Revit.DB;
using Objects.BuiltElements.Revit.RevitRoof;
using Objects.Geometry;
using System.Linq;
using ConverterRevitShared.Classes.Abstract;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal class RoofToRevit : SpeckleToRevitDefault<Roof, DB.RoofBase, DB.RoofType>,
    ICreate<Roof, DB.RoofBase>,
    ISpecificParameters
  {
    public RoofToRevit(ConverterRevit converter) : base(converter)
    {
    }

    public override bool NeedsType => true;
    public override bool CanHostElements => true;
    public Dictionary<BuiltInParameter, (object, string)> Parameters => new();
    public override void ValidateToRevit(Roof roof)
    {
      if (roof.outline == null)
      {
        throw new Exception("Roof outline cannot be null");
      }
    }

    public DB.RoofBase Create(Roof speckleRoof)
    {
      DB.RoofBase revitRoof = null;
      DB.Level level = null;
      var outline = Converter.CurveToNative(speckleRoof.outline);

      if (speckleRoof is RevitRoof speckleRevitRoof)
        level = Converter.ConvertLevelToRevit(speckleRevitRoof.level, out _);
      else
        level = Converter.ConvertLevelToRevit(Converter.LevelFromCurve(outline.get_Item(0)), out _);

      switch (speckleRoof)
      {
        case RevitExtrusionRoof speckleExtrusionRoof:
          {
            var referenceLine = Converter.LineToNative(speckleExtrusionRoof.referenceLine);
            var norm = Converter.GetPerpendicular(referenceLine.GetEndPoint(0) - referenceLine.GetEndPoint(1)).Negate();
            ReferencePlane plane = Doc.Create.NewReferencePlane(referenceLine.GetEndPoint(0),
              referenceLine.GetEndPoint(1),
              norm,
              Doc.ActiveView);
            //create floor without a type
            var start = Converter.ScaleToNative(speckleExtrusionRoof.start, speckleExtrusionRoof.units);
            var end = Converter.ScaleToNative(speckleExtrusionRoof.end, speckleExtrusionRoof.units);
            revitRoof = Doc.Create.NewExtrusionRoof(outline, plane, level, RevitType, start, end);
            break;
          }
        case RevitFootprintRoof speckleFootprintRoof:
          {
            ModelCurveArray curveArray = new ModelCurveArray();
            var revitFootprintRoof = Doc.Create.NewFootPrintRoof(outline, level, RevitType, out curveArray);

            // if the roof is a curtain roof then set the mullions at the borders
            if (revitFootprintRoof.CurtainGrids != null && speckleFootprintRoof["elements"] is List<Base> elements && elements.Count != 0)
            {
              // TODO: Create a new type instead of overriding the type. This could affect other elements
              var param = RevitType.get_Parameter(BuiltInParameter.AUTO_MULLION_BORDER1_GRID1);
              var type = Doc.GetElement(param.AsElementId());
              if (type == null)
              {
                // assuming first mullion is the desired mullion for the whole roof...
                Converter.GetElementType<MullionType>(elements.Where(b => b is OBR.FamilyInstance f).First(), new ApplicationObject("", ""), out MullionType mullionType);
                Converter.TrySetParam(RevitType, BuiltInParameter.AUTO_MULLION_BORDER1_GRID1, mullionType);
                Converter.TrySetParam(RevitType, BuiltInParameter.AUTO_MULLION_BORDER1_GRID2, mullionType);
                Converter.TrySetParam(RevitType, BuiltInParameter.AUTO_MULLION_BORDER2_GRID1, mullionType);
                Converter.TrySetParam(RevitType, BuiltInParameter.AUTO_MULLION_BORDER2_GRID2, mullionType);
              }
            }
            var poly = speckleFootprintRoof.outline as Polycurve;
            bool hasSlopedSide = false;
            if (poly != null)
            {
              for (var i = 0; i < curveArray.Size; i++)
              {
                var isSloped = ((Base)poly.segments[i])["isSloped"] as bool?;
                var slopeAngle = ((Base)poly.segments[i])["slopeAngle"] as double?;
                var offset = ((Base)poly.segments[i])["offset"] as double?;

                if (isSloped != null)
                {
                  revitFootprintRoof.set_DefinesSlope(curveArray.get_Item(i), isSloped == true);
                  if (slopeAngle != null && isSloped == true)
                  {
                    // slope is set using actual slope (rise / run) for this method
                    revitFootprintRoof.set_SlopeAngle(curveArray.get_Item(i), Math.Tan((double)slopeAngle * Math.PI / 180));
                    hasSlopedSide = true;
                  }
                }

                if (offset != null)
                  revitFootprintRoof.set_Offset(curveArray.get_Item(i), Converter.ScaleToNative((double)offset, speckleFootprintRoof.units));
              }
            }

            //this is for schema builder specifically
            //if no roof edge has a slope defined but a slope angle is defined on the roof
            //set each edge to have that slope
            if (!hasSlopedSide && speckleFootprintRoof.slope != null && speckleFootprintRoof.slope != 0)
            {
              for (var i = 0; i < curveArray.Size; i++)
                revitFootprintRoof.set_DefinesSlope(curveArray.get_Item(i), true);

              Parameters[DB.BuiltInParameter.ROOF_SLOPE]= ((double)speckleFootprintRoof.slope, "");
            }

            if (speckleFootprintRoof.cutOffLevel != null)
            {
              var cutOffLevel = Converter.ConvertLevelToRevit(speckleFootprintRoof.cutOffLevel, out _);
              Parameters[DB.BuiltInParameter.ROOF_UPTO_LEVEL_PARAM] = (cutOffLevel, "");
            }

            revitRoof = revitFootprintRoof;
            break;
          }
        default:
          throw new Exception($"Roof type, {speckleRoof.GetType()}, is not supported, please try with RevitExtrusionRoof or RevitFootprintRoof");
      }

      Doc.Regenerate();
      Converter.CreateVoids(revitRoof, speckleRoof);
      Doc.Regenerate();
      return revitRoof;
    }
  }
}
