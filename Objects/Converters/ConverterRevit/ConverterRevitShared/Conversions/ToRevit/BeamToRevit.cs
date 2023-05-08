using System;
using Objects.BuiltElements;
using Objects.Converter.Revit;
using DB = Autodesk.Revit.DB;
using ConverterRevitShared.Interfaces;
using Autodesk.Revit.DB;
using Speckle.Core.Models;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Autodesk.Revit.DB.Structure;
using Objects.BuiltElements.Revit;
using ConverterRevitShared.Conversions;
using ConverterRevitShared.Extensions;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal sealed class BeamToRevit : ConversionBuilderToRevit<DB.FamilyInstance, BeamToRevit>,
    IToRevitTypedWithConverter<Beam, DB.FamilyInstance, DB.FamilySymbol, BeamToRevit>,
    IDefaultUpdate,
    IDefaultCreate<DB.FamilyInstance>
  {
    public BeamToRevit(ConverterRevit converter, Beam speckleObject)
    {
      SpeckleObject = speckleObject;
      Converter = converter;
      AppObj = new ApplicationObject(SpeckleObject.id, SpeckleObject.speckle_type) { applicationId = SpeckleObject.applicationId };
    }
    public Beam SpeckleObject { get; }
    public ConverterRevit Converter { get; }
    public FamilySymbol RevitObjectType { get; set; }
    public ApplicationObject AppObj { get; }
    internal DB.Curve BaseCurve { get; set; }
    internal DB.Level Level { get; set; }

    public DB.FamilyInstance Convert(Beam @base)
    {
      return Do(ValidateToRevit)
        .GetRevitType()
        .GetExistingRevitObject()
        .Do(DefineProperties)
        .TryDefaultUpdate()
        .TryDefaultCreate()
        .Convert();
    }

    public void ValidateToRevit()
    {
      if (SpeckleObject.baseLine == null)
      {
        throw new Exception("Beam baseline cannot be null");
      }
    }
    public void DefineProperties()
    {
      //using var baseLine = Converter.CurveToNative(speckleBeam.baseLine);
      BaseCurve = Converter.CurveToNative(SpeckleObject.baseLine).get_Item(0);

      if (SpeckleObject is RevitBeam speckleRevitBeam && speckleRevitBeam.level != null)
      {
        //level = Converter.GetLevelByName(speckleRevitBeam.level.name);
        Level = Converter.ConvertLevelToRevit(speckleRevitBeam.level, out _);
      }

      Level ??= Converter.ConvertLevelToRevit(Converter.LevelFromCurve(BaseCurve), out _);
    }

    public DB.FamilyInstance Create()
    {
      var revitBeam = Converter.Doc.Create.NewFamilyInstance(BaseCurve, RevitObjectType, Level, StructuralType.Beam);

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

    public void Update()
    {
      var revitType = Converter.Doc.GetElement(RevitObject.GetTypeId()) as DB.ElementType;

      // if family changed, tough luck. delete and let us create a new one.
      if (RevitObjectType.FamilyName != revitType.FamilyName)
      {
        Converter.Doc.Delete(RevitObject.Id);
      }
      else
      {
        (RevitObject.Location as DB.LocationCurve).Curve = BaseCurve;

        // check for a type change
        if (RevitObjectType != revitType)
        {
          RevitObject.ChangeTypeId(RevitObjectType.Id);
        }
      }
    }
  }
}
