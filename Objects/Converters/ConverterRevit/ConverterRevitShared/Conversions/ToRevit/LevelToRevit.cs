using System;
using System.Collections.Generic;
using System.Text;
using Autodesk.Revit.DB;
using ConverterRevitShared.Extensions.SpeckleObjects;
using ConverterRevitShared.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Objects.Converter.Revit;
using Speckle.Core.Models;
using DB = Autodesk.Revit.DB;
using OB = Objects.BuiltElements;

namespace ConverterRevitShared.Conversions.ToRevit
{
  internal class LevelToRevit : ConversionBuilderToRevit1to1<OB.Level, DB.Level, LevelToRevit>,
    IToRevitBase<OB.Level, DB.Level, LevelToRevit>,
    IValidateToRevit<OB.Level>,
    IHasExistingObject<DB.Level>,
    ICreate<DB.Level>,
    IHasApplicationObject
  {
    public LevelToRevit(OB.Level speckleObject, Dictionary<string, string> settings, IMemoryCache cache) : base(speckleObject, settings, cache)
    {
      AppObj = new ApplicationObject(SpeckleObject.id, SpeckleObject.speckle_type) { applicationId = SpeckleObject.applicationId };
    }

    public ApplicationObject AppObj { get; }

    public Level ExistingObject { get; set; }

    public Level Create()
    {
      throw new NotImplementedException();
    }

    public DB.Level Convert(OB.Level level)
    {
      return this.ValidateToRevit()
        //.GetExistingObjectByApplicationId()
        .Do(GetExistingObject)
        .Convert();
    }

    private void GetExistingObject()
    {
      throw new NotImplementedException();
    }

    public void ValidateToRevit(OB.Level @base)
    {
      if (@base == null)
      {
        throw new Exception("");
      }
    }
  }
}
