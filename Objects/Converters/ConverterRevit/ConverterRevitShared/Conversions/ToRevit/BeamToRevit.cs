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
using ConverterRevitShared.Extensions.SpeckleObjects;
using Microsoft.Extensions.Caching.Memory;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal sealed class BeamToRevit : ConversionBuilderToRevit1to1<Beam, DB.FamilyInstance, BeamToRevit>,
    IToRevitTypedWithConverter<Beam, DB.FamilyInstance, DB.FamilySymbol, BeamToRevit>,
    IValidateToRevit<Beam>,
    IHasExistingObject<DB.FamilyInstance>,
    IUpdate<DB.FamilyInstance>,
    ICreate<DB.FamilyInstance>
  {
    public BeamToRevit(ConverterRevit converter, Beam speckleObject, Dictionary<string,string> settings, IMemoryCache memoryCache) : base(speckleObject, settings, memoryCache)
    {
      Converter = converter;
      AppObj = new ApplicationObject(SpeckleObject.id, SpeckleObject.speckle_type) { applicationId = SpeckleObject.applicationId };
    }
    public ConverterRevit Converter { get; }
    public FamilySymbol RevitObjectType { get; set; }
    public ApplicationObject AppObj { get; }
    internal DB.Curve BaseCurve { get; set; }
    internal DB.Level Level { get; set; }
    public DB.FamilyInstance ExistingObject { get; set; }

    public DB.FamilyInstance Convert(Beam @base)
    {
      return this.ValidateToRevit()
        .GetRevitType()
        .GetExistingObjectByAppId()
        .Do(DefineProperties)
        .TryUpdateExistingObjectThenAssignReturnObject()
        .TryCreateIfDefaultReturnObjectThenThrowIfStillDefault()
        .Convert();
    }
    public void ValidateToRevit(Beam @base)
    {
      if (@base.baseLine == null)
      {
        throw new Exception("Beam baseline cannot be null");
      }
    }
    public void DefineProperties()
    {
      //using var baseLine = Converter.CurveToNative(speckleBeam.baseLine);
      //BaseCurve = Converter.CurveToNative(SpeckleObject.baseLine).get_Item(0);

      var baseLine = SpeckleObject.baseLine.ToRevit(ReferencePointTransform);
      BaseCurve = baseLine.get_Item(0);

      if (SpeckleObject is RevitBeam speckleRevitBeam && speckleRevitBeam.level != null)
      {
        //level = Converter.GetLevelByName(speckleRevitBeam.level.name);
        Level = Converter.ConvertLevelToRevit(speckleRevitBeam.level, out _);
      }

      Level ??= Converter.ConvertLevelToRevit(Converter.LevelFromCurve(BaseCurve), out _);
    }

    public DB.FamilyInstance Create()
    {
      var revitBeam = Doc.Create.NewFamilyInstance(BaseCurve, RevitObjectType, Level, StructuralType.Beam);

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

    public void Update(DB.FamilyInstance existingObject)
    {
      var revitType = Doc.GetElement(existingObject.GetTypeId()) as DB.ElementType;

      // if family changed, tough luck. delete and let us create a new one.
      if (RevitObjectType.FamilyName != revitType.FamilyName)
      {
        Doc.Delete(existingObject.Id);
      }
      else
      {
        (existingObject.Location as DB.LocationCurve).Curve = BaseCurve;

        // check for a type change
        if (RevitObjectType != revitType)
        {
          existingObject.ChangeTypeId(RevitObjectType.Id);
        }
      }
    }
  }
}
