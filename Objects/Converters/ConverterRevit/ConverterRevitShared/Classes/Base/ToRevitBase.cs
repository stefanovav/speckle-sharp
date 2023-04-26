using System;
using System.Collections.Generic;
using Autodesk.Revit.DB;
using Speckle.Core.Models;
using Objects.Converter.Revit;

namespace ConverterRevitShared.Classes
{
  internal abstract class ToRevitBase<TSpeckleObject, TRevitObject, TRevitObjectType>
    where TSpeckleObject: Base
    where TRevitObject : Element
    where TRevitObjectType : ElementType
  {
    public ToRevitBase(ConverterRevit converter)
    {
      Converter = converter;
      //SpeckleObject = @base;
    }

    public ConverterRevit Converter { get; set; }
    //public TSpeckleObject SpeckleObject { get; set; }
    public Document Doc => Converter.Doc;
    //public TRevitObject ExistingRevitObject { get; set; }
    //public TRevitObject ConvertedRevitObject { get; set; }
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

      ValidateToRevit(@base, appObj);

      if (NeedsType)
      {
        if (!Converter.GetElementType<TRevitObjectType>(@base, appObj, out var nativeObjectType))
        {
          appObj.Update(status: ApplicationObject.State.Failed);
          return appObj;
        }
        RevitType = nativeObjectType;
      }

      if (this is IShareResources<TSpeckleObject> thisIShareReasources)
      {
        thisIShareReasources.ComputeSharedResources(@base);
      }

      if (this is ICanUpdate<TSpeckleObject, TRevitObject> updatable && existingRevitObject != default(TRevitObject))
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

      if (this is ICanCreate<TSpeckleObject, TRevitObject> creatable && nativeObjectToCreate == default(TRevitObject))
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

      if (this is ISetSpecificParams setSpecificParams)
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

    public abstract void ValidateToRevit(TSpeckleObject @base, ApplicationObject appObj);
    public abstract bool NeedsType { get; }
    public abstract bool CanHostElements { get; }
  }

  internal interface ICanUpdate<TSpeckleObject, TRevitObject>
  {
    /// <summary>
    /// Update an existing revit element
    /// </summary>
    /// <param name="revitElement">Non-null Revit element that needs to be updated</param>
    /// <returns>A bool that is true if the update was successful or false if the object needs to be deleted and re-created</returns>
    public void Update(TRevitObject revitElement, TSpeckleObject speckleElement);
  }
  internal interface ICanCreate<TSpeckleObject, TRevitObject>
  {
    public TRevitObject Create(TSpeckleObject speckleElement);
  }

  internal interface ISetSpecificParams
  {
    public Dictionary<BuiltInParameter, (object, string)> Parameters { get; }
  }

  internal interface IShareResources<TSpeckleObject>
  {
    public void ComputeSharedResources(TSpeckleObject speckleElement);
  }
}
