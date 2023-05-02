using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Speckle.Core.Models;
using Objects.Converter.Revit;

namespace ConverterRevitShared.Classes.Abstract
{
  internal abstract class SpeckleToRevitDefault<TSpeckleObject, TRevitObject, TRevitObjectType> 
    //: IToRevitTyped<TSpeckleObject, TRevitObject, TRevitObjectType>
    where TSpeckleObject: Base
    where TRevitObject : Element
    where TRevitObjectType : ElementType
  {
    public SpeckleToRevitDefault(ConverterRevit converter)
    {
      Converter = converter;
    }

    public ConverterRevit Converter { get; set; }
    public Document Doc => Converter.Doc;
    public TRevitObjectType RevitType { get; set; }
    public ApplicationObject Convert(TSpeckleObject @base)
    {
      TRevitObject nativeObjectToCreate = default;

      bool isUpdate = false;
      var appObj = new ApplicationObject(@base.id, @base.speckle_type) { applicationId = @base.applicationId };
      var existingRevitObject = Converter.GetExistingElementByApplicationId<TRevitObject>(@base.applicationId);

      // skip if element already exists in doc & receive mode is set to ignore
      if (Converter.IsIgnore(existingRevitObject, appObj, out appObj))
      {
        return appObj;
      }

      try
      {
        ValidateToRevit(@base);
      }
      catch (Exception e)
      {
        appObj.Update(status: ApplicationObject.State.Failed, logItem: e.Message);
      }


      if (NeedsType)
      {
        RevitType = Converter.GetElementType<TRevitObjectType>(@base, appObj, out bool isExactMatch);
        if (RevitType == null)
        {
          appObj.Update(status: ApplicationObject.State.Failed);
          return appObj;
        }
      }

      if (this is IDefineProperties<TSpeckleObject> thisIDefineProperties)
      {
        thisIDefineProperties.DefineProperties(@base);
      }

      if (this is IUpdate<TSpeckleObject, TRevitObject> updatable && existingRevitObject != default(TRevitObject))
      {
        try
        {
          updatable.Update(existingRevitObject, @base);
          nativeObjectToCreate = existingRevitObject;
          isUpdate = true;
        }
        catch (Exception ex)
        {

        }
      }

      if (this is ICreate<TSpeckleObject, TRevitObject> creatable && nativeObjectToCreate == default(TRevitObject))
      {
        if (existingRevitObject is Element element)
        {
          Converter.Doc.Delete(element.Id);
        }
        nativeObjectToCreate = creatable.Create(@base);
        isUpdate = false;
      }

      if (nativeObjectToCreate == default(TRevitObject))
      {
        appObj.Update(status: ApplicationObject.State.Failed, logItem: $"Conversion of Speckle object of type {@base.GetType()} was unsuccessful");
        return appObj;
      }

      if (this is IModifyAfterCreateOrUpdate<TSpeckleObject, TRevitObject> thisModify)
      {
        thisModify.ModifyAfterCreateOrUpdate(@base, nativeObjectToCreate);
      }

      if (this is ISpecificParameters setSpecificParams)
      {
        var exclusions = new List<string>();
        foreach (var kvp in setSpecificParams.Parameters)
        {
          exclusions.Add(kvp.Key.ToString());
          var (@object, units) = kvp.Value;
          if (@object is Element el)
          {
            Converter.TrySetParam(nativeObjectToCreate, kvp.Key, el);
          }
          else
          {
            Converter.TrySetParam(nativeObjectToCreate, kvp.Key, @object, units);
          }
        }
        Converter.SetInstanceParameters(nativeObjectToCreate, @base, exclusions);
      }
      else
      {
        Converter.SetInstanceParameters(nativeObjectToCreate, @base);
      }
      
      if (CanHostElements)
      {
        Converter.SetHostedElements(@base, nativeObjectToCreate, appObj);
      }

      var state = isUpdate ? ApplicationObject.State.Updated : ApplicationObject.State.Created;
      appObj.Update(status: state, createdId: nativeObjectToCreate.UniqueId, convertedItem: nativeObjectToCreate);
      return appObj;
    }

    public abstract void ValidateToRevit(TSpeckleObject @base);
    public abstract bool NeedsType { get; }
    public abstract bool CanHostElements { get; }
  }

  internal interface IUpdate<TSpeckleObject, TRevitObject>
  {
    /// <summary>
    /// Update an existing revit element
    /// </summary>
    /// <param name="revitElement">Non-null Revit element that needs to be updated</param>
    /// <returns>A bool that is true if the update was successful or false if the object needs to be deleted and re-created</returns>
    public void Update(TRevitObject revitElement, TSpeckleObject speckleElement);
  }
  internal interface ICreate<TSpeckleObject, TRevitObject>
  {
    public TRevitObject Create(TSpeckleObject speckleElement);
  }
  internal interface IModifyAfterCreateOrUpdate<TSpeckleObject, TRevitObject>
  {
    public void ModifyAfterCreateOrUpdate(TSpeckleObject speckleElement, TRevitObject revitElement);
  }

  internal interface ISpecificParameters
  {
    public Dictionary<BuiltInParameter, (object, string)> Parameters { get; }
  }

  internal interface IDefineProperties<TSpeckleObject>
  {
    public void DefineProperties(TSpeckleObject speckleElement);
  }
}
