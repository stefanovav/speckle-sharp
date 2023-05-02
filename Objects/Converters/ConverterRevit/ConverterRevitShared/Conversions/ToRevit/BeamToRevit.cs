using System;
using Objects.BuiltElements;
using Objects.Converter.Revit;
using DB = Autodesk.Revit.DB;
using ConverterRevitShared.Classes.Abstract;
using ConverterRevitShared.Interfaces;
using Autodesk.Revit.DB;
using Speckle.Core.Models;
using ConverterRevitShared.ConversionSteps;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Autodesk.Revit.DB.Structure;
using Objects.BuiltElements.Revit;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal sealed class BeamToRevit : ConversionBuilder<DB.FamilyInstance, BeamToRevit>,
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
      return Do(new ValidateBeamToRevit(this))
        .GetRevitType()
        .GetExistingRevitObject()
        .Do(new DefineProperties(this))
        .TryDefaultUpdate()
        .TryDefaultCreate()
        .Convert();
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
  internal sealed class ValidateBeamToRevit : ConversionStep<BeamToRevit>
  {
    public ValidateBeamToRevit(BeamToRevit conversionBuilder) : base(conversionBuilder) { }
    public override BeamToRevit Handle()
    {
      if (Builder.SpeckleObject.baseLine == null)
      {
        throw new Exception("Beam baseline cannot be null");
      }
      return Builder;
    }
  }
  internal sealed class DefineProperties : ConversionStep<BeamToRevit>
  {
    public DefineProperties(BeamToRevit conversionBuilder) : base(conversionBuilder) { }
    public override BeamToRevit Handle()
    {
      Builder.BaseCurve = Builder.Converter.CurveToNative(Builder.SpeckleObject.baseLine).get_Item(0);

      if (Builder.SpeckleObject is RevitBeam speckleRevitBeam && speckleRevitBeam.level != null)
      {
        //level = Converter.GetLevelByName(speckleRevitBeam.level.name);
        Builder.Level = Builder.Converter.ConvertLevelToRevit(speckleRevitBeam.level, out _);
      }
      return Builder;
    }
  }
  //internal sealed class BeamToRevit : SpeckleToRevitDefault<Beam, DB.FamilyInstance, DB.FamilySymbol>,
  //  IDefineProperties<Beam>,
  //  ICreate<Beam, DB.FamilyInstance>,
  //  IUpdate<Beam, DB.FamilyInstance>,
  //  ISpecificParameters,
  //  IDisposable
  //{
  //  public BeamToRevit(ConverterRevit converter) : base(converter)
  //  {
  //  }
  //  private DB.Curve BaseCurve { get; set; }
  //  private DB.Level Level { get; set; }
  //  public override void ValidateToRevit(Beam speckleBeam)
  //  {
  //    if (speckleBeam.baseLine == null)
  //    {
  //      throw new Exception("Beam baseline cannot be null");
  //    }
  //  }
  //  public void DefineProperties(Beam speckleBeam)
  //  {
  //    //using var baseLine = Converter.CurveToNative(speckleBeam.baseLine);
  //    BaseCurve = Converter.CurveToNative(speckleBeam.baseLine).get_Item(0);

  //    if (speckleBeam is RevitBeam speckleRevitBeam && speckleRevitBeam.level != null)
  //    {
  //      //level = Converter.GetLevelByName(speckleRevitBeam.level.name);
  //      Level = Converter.ConvertLevelToRevit(speckleRevitBeam.level, out _);
  //    }

  //    Level ??= Converter.ConvertLevelToRevit(Converter.LevelFromCurve(BaseCurve), out _);
  //  }

  //  public DB.FamilyInstance Create(Beam speckleBeam)
  //  {
  //    var revitBeam = Doc.Create.NewFamilyInstance(BaseCurve, RevitType, Level, StructuralType.Beam);

  //    // check for disallow join for beams in user settings
  //    // currently, this setting only applies to beams being created
  //    if (Converter.Settings.TryGetValue("disallow-join", out string value) && !string.IsNullOrEmpty(value))
  //    {
  //      List<string> joinSettings = new List<string>(Regex.Split(Converter.Settings["disallow-join"], @"\,\ "));
  //      if (joinSettings.Contains(ConverterRevit.StructuralFraming))
  //      {
  //        StructuralFramingUtils.DisallowJoinAtEnd(revitBeam, 0);
  //        StructuralFramingUtils.DisallowJoinAtEnd(revitBeam, 1);
  //      }
  //    }
  //    return revitBeam;
  //  }

  //  public void Update(DB.FamilyInstance revitBeam, Beam speckleElement)
  //  {
  //    var revitType = Doc.GetElement(revitBeam.GetTypeId()) as DB.ElementType;

  //    // if family changed, tough luck. delete and let us create a new one.
  //    if (RevitType.FamilyName != revitType.FamilyName)
  //    {
  //      Doc.Delete(revitBeam.Id);
  //    }
  //    else
  //    {
  //      (revitBeam.Location as DB.LocationCurve).Curve = BaseCurve;

  //      // check for a type change
  //      if (RevitType != revitType)
  //      {
  //        revitBeam.ChangeTypeId(RevitType.Id);
  //      }
  //    }
  //  }

  //  public void Dispose()
  //  {
  //    BaseCurve?.Dispose();
  //    Level?.Dispose();
  //  }

  //  public override bool NeedsType => true;
  //  public override bool CanHostElements => true;
  //  public Dictionary<DB.BuiltInParameter, (object, string)> Parameters => new()
  //  {
  //    { DB.BuiltInParameter.INSTANCE_REFERENCE_LEVEL_PARAM, (Level, "") }
  //  };
  //}
}
