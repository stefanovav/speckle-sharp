using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Objects.BuiltElements;
using Objects.Converter.Revit;
using Speckle.Core.Models;
using Autodesk.Revit.DB.Structure;
using Objects.BuiltElements.Revit;
using DB = Autodesk.Revit.DB;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal sealed class BeamToRevit : ToRevitBase<Beam, DB.FamilyInstance, DB.FamilySymbol>,
    IShareResources<Beam>,
    ICanCreate<Beam, DB.FamilyInstance>,
    ICanUpdate<Beam, DB.FamilyInstance>,
    ISetSpecificParams,
    IDisposable
  {
    public BeamToRevit(ConverterRevit converter) : base(converter)
    {
    }
    private DB.Curve BaseCurve { get; set; }
    private DB.Level Level { get; set; }
    public override void ValidateToRevit(Beam speckleBeam, ApplicationObject appObj)
    {
      if (speckleBeam.baseLine == null)
      {
        throw new Exception("Beam baseline cannot be null");
      }
    }
    public void ComputeSharedResources(Beam speckleBeam)
    {
      //using var baseLine = Converter.CurveToNative(speckleBeam.baseLine);
      BaseCurve = Converter.CurveToNative(speckleBeam.baseLine).get_Item(0);

      if (speckleBeam is RevitBeam speckleRevitBeam && speckleRevitBeam.level != null)
      {
        //level = Converter.GetLevelByName(speckleRevitBeam.level.name);
        Level = Converter.ConvertLevelToRevit(speckleRevitBeam.level, out _);
      }

      Level ??= Converter.ConvertLevelToRevit(Converter.LevelFromCurve(BaseCurve), out _);
    }

    public DB.FamilyInstance Create(Beam speckleBeam)
    {
      var revitBeam = Doc.Create.NewFamilyInstance(BaseCurve, RevitType, Level, StructuralType.Beam);

      // check for disallow join for beams in user settings
      // currently, this setting only applies to beams being created
      if (Converter.Settings.TryGetValue("disallow-join", out string value) && !string.IsNullOrEmpty(value))
      {
        List<string> joinSettings = new List<string>(Regex.Split(Converter.Settings["disallow-join"], @"\,\ "));
        if (joinSettings.Contains(ConverterRevit.StructuralFraming))
        {
          StructuralFramingUtils.DisallowJoinAtEnd(revitBeam, 0);
          StructuralFramingUtils.DisallowJoinAtEnd(revitBeam, 1);
        }
      }
      return revitBeam;
    }

    public void Update(DB.FamilyInstance revitBeam, Beam speckleElement)
    {
      var revitType = Doc.GetElement(revitBeam.GetTypeId()) as DB.ElementType;

      // if family changed, tough luck. delete and let us create a new one.
      if (RevitType.FamilyName != revitType.FamilyName)
      {
        Doc.Delete(revitBeam.Id);
      }
      else
      {
        (revitBeam.Location as DB.LocationCurve).Curve = BaseCurve;

        // check for a type change
        if (RevitType != revitType)
        {
          revitBeam.ChangeTypeId(RevitType.Id);
        }
      }
    }

    public void Dispose()
    {
      BaseCurve?.Dispose();
      Level?.Dispose();
    }

    public override bool NeedsType => true;
    public override bool CanHostElements => true;
    public Dictionary<DB.BuiltInParameter, (object, string)> Parameters => new()
    {
      { DB.BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, (Level, "") }
    };
  }
}
