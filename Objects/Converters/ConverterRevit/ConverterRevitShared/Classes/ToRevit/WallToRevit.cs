using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using System.Text.RegularExpressions;
using ConverterRevitShared.Classes.Abstract;
using Objects.Converter.Revit;
using DB = Autodesk.Revit.DB;
using OBE = Objects.BuiltElements;
using OBER = Objects.BuiltElements.Revit;
using Objects.BuiltElements.Revit;
using Objects.BuiltElements;
using Objects;

namespace ConverterRevitShared.Classes.ToRevit
{
  internal class WallToRevit : SpeckleToRevitDefault<OBE.Wall, DB.Wall, DB.WallType>,
    IDefineProperties<OBE.Wall>,
    ICreate<OBE.Wall, DB.Wall>,
    IUpdate<OBE.Wall, DB.Wall>,
    IModifyAfterCreateOrUpdate<OBE.Wall, DB.Wall>,
    ISpecificParameters
  {
    public WallToRevit(ConverterRevit converter) : base(converter)
    {
    }
    public override bool NeedsType => true;
    public override bool CanHostElements => true;
    public Dictionary<DB.BuiltInParameter, (object, string)> Parameters => new()
    {
      { DB.BuiltInParameter.WALL_BASE_CONSTRAINT, (Level, string.Empty)}
    };
    private DB.Curve BaseCurve { get; set; }
    private List<string> JoinSettings { get; set; }
    private DB.Level Level { get; set; }
    private bool Structural { get; set; }
    public override void ValidateToRevit(OBE.Wall speckleWall)
    {
      if (speckleWall.baseLine == null)
      {
        throw new Exception("Wall does not have a valid baseline");
      }
    }

    public void DefineProperties(OBE.Wall speckleWall)
    {
      using var curveArray = Converter.CurveToNative(speckleWall.baseLine);
      var baseCurve = curveArray.get_Item(0);
      if (speckleWall is OBER.RevitWall speckleRevitWall)
      {
        Level = Converter.ConvertLevelToRevit(speckleRevitWall.level, out _);
        Structural = speckleRevitWall.structural;
      }
      else
      {
        Level = Converter.ConvertLevelToRevit(Converter.LevelFromCurve(baseCurve), out _);
      }
    }

    public DB.Wall Create(OBE.Wall speckleWall)
    {
      var revitWall = DB.Wall.Create(Doc, BaseCurve, Level.Id, Structural);
      if (JoinSettings.Contains(ConverterRevit.StructuralWalls) && Structural)
      {
        WallUtils.DisallowWallJoinAtEnd(revitWall, 0);
        WallUtils.DisallowWallJoinAtEnd(revitWall, 1);
      }
      if (JoinSettings.Contains(ConverterRevit.ArchitecturalWalls) && !Structural)
      {
        WallUtils.DisallowWallJoinAtEnd(revitWall, 0);
        WallUtils.DisallowWallJoinAtEnd(revitWall, 1);
      }
      return revitWall;
    }

    public void ModifyAfterCreateOrUpdate(OBE.Wall speckleWall, DB.Wall revitWall)
    {
      if (speckleWall is RevitWall spklRevitWall)
      {
        if (spklRevitWall.flipped != revitWall.Flipped)
          revitWall.Flip();

        if (spklRevitWall.topLevel != null)
        {
          var topLevel = Converter.ConvertLevelToRevit(spklRevitWall.topLevel, out _);
          Parameters[DB.BuiltInParameter.WALL_HEIGHT_TYPE] = (topLevel, null);
        }
        else
        {
          Parameters[DB.BuiltInParameter.WALL_USER_HEIGHT_PARAM] = (speckleWall.height, speckleWall.units);
        }
        Parameters[DB.BuiltInParameter.WALL_BASE_OFFSET] = (spklRevitWall.baseOffset, speckleWall.units);
        Parameters[DB.BuiltInParameter.WALL_TOP_OFFSET] = (spklRevitWall.topOffset, speckleWall.units);
      }
      else // Set wall unconnected height.
      {
        Parameters[DB.BuiltInParameter.WALL_USER_HEIGHT_PARAM] = (speckleWall.height, speckleWall.units);
      }

      Converter.SetWallVoids(revitWall, speckleWall);
    }

    public void Update(DB.Wall revitWall, OBE.Wall speckleElement)
    {
      // walls behave very strangly while joined
      // if a wall is joined and you try to move it to a location where it isn't touching the joined wall,
      // then it will move only one end of the wall and the other will stay joined
      // therefore, disallow joins while changing the wall and then reallow the joins if need be
      DB.WallUtils.DisallowWallJoinAtEnd(revitWall, 0);
      DB.WallUtils.DisallowWallJoinAtEnd(revitWall, 1);

      //NOTE: updating an element location can be buggy if the baseline and level elevation don't match
      //Let's say the first time an element is created its base point/curve is @ 10m and the Level is @ 0m
      //the element will be created @ 0m
      //but when this element is updated (let's say with no changes), it will jump @ 10m (unless there is a level change)!
      //to avoid this behavior we're moving the base curve to match the level elevation
      var newz = BaseCurve.GetEndPoint(0).Z;
      var offset = Level.Elevation - newz;
      var newCurve = BaseCurve;
      if (Math.Abs(offset) > ConverterRevit.TOLERANCE) // level and curve are not at the same height
      {
        newCurve = BaseCurve.CreateTransformed(Transform.CreateTranslation(new XYZ(0, 0, offset)));
      }
      ((LocationCurve)revitWall.Location).Curve = newCurve;

      // now that we've moved the wall, rejoin the wall ends
      List<string> joinSettings = new List<string>();

      if (Converter.Settings.TryGetValue("disallow-join", out string value) && !string.IsNullOrEmpty(value))
      {
        joinSettings = new List<string>(Regex.Split(Converter.Settings["disallow-join"], @"\,\ "));
      }
      if (!joinSettings.Contains(ConverterRevit.StructuralWalls) && Structural)
      {
        DB.WallUtils.AllowWallJoinAtEnd(revitWall, 0);
        DB.WallUtils.AllowWallJoinAtEnd(revitWall, 1);
      }
      if (!joinSettings.Contains(ConverterRevit.ArchitecturalWalls) && !Structural)
      {
        DB.WallUtils.AllowWallJoinAtEnd(revitWall, 0);
        DB.WallUtils.AllowWallJoinAtEnd(revitWall, 1);
      }
    }
  }
}
