using Autodesk.Revit.DB;
using Objects.Converter.Revit;
using Speckle.Core.Models;

namespace ConverterRevitShared.Interfaces
{
  internal interface ISpeckleObjectConverter<in TSpeckleObject, out TRevitObject>
  {
    public TRevitObject Convert(TSpeckleObject @object);
  }
  internal interface ISpeckleObject<TSpeckleObject>
    where TSpeckleObject : Base
  {
    public TSpeckleObject SpeckleObject { get; }
  }
  internal interface IRevitObject<TRevitObject>
  {
    public TRevitObject RevitObject { get; set; }
  }
  internal interface IRevitType<TRevitObjectType>
    where TRevitObjectType : ElementType
  {
    public TRevitObjectType RevitObjectType { get; set; }
  }
  internal interface IApplicationObject
  {
    public ApplicationObject AppObj { get; }
  }
  internal interface IConverterObject
  {
    public ConverterRevit Converter { get; }
  }
  internal interface IToRevitBase<TSpeckleObject, TRevitObject> : ISpeckleObjectConverter<TSpeckleObject, TRevitObject>, ISpeckleObject<TSpeckleObject>, IRevitObject<TRevitObject>
    where TSpeckleObject : Base
  {
  }
  internal interface IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF> :
    IToRevitBase<TSpeckleObject, TRevitObject>,
    IRevitType<TRevitObjectType>,
    IConverterObject,
    IApplicationObject
    where TSpeckleObject : Base
    where TRevitObjectType : ElementType
    where SELF : IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
  {
  }
  internal interface IToRevitTypedWithConverter1to1<TSpeckleObject, TRevitObject, TRevitObjectType, SELF> :
    IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
    where TSpeckleObject : Base
    where TRevitObject: Element
    where TRevitObjectType : ElementType
    where SELF : IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
  {
  }

  internal interface IDefaultCreate<TRevit>
  {
    public TRevit Create();
  }
  internal interface IDefaultUpdate
  {
    public void Update();
  }
  ////internal interface IToRevitBase<TSpeckleObject, TRevitObject> : ISpeckleObjectConverter<TSpeckleObject, TRevitObject>
  ////where TSpeckleObject : Base
  ////{
  ////  public TSpeckleObject SpeckleObject { get; }
  ////  public TRevitObject RevitObject { get; set; }
  ////}
  //internal interface IToRevitBase<TSpeckleObject, TRevitObject, SELF> : IToRevitBase<TSpeckleObject, TRevitObject>
  //where TSpeckleObject : Base
  //{
  //  public TSpeckleObject SpeckleObject { get; }
  //  public TRevitObject RevitObject { get; set; }
  //}
  //internal interface IToRevitOneToOne<TSpeckleObject, TRevitObject> : IToRevitBase<TSpeckleObject, TRevitObject>
  //where TSpeckleObject : Base
  //where TRevitObject : Element
  //{
  //  public TRevitObject RevitObject { get; set; }
  //}
  //internal interface IToRevitTyped<TSpeckleObject, TRevitObject, TRevitObjectType> : IToRevitBase<TSpeckleObject, TRevitObject>
  //  where TSpeckleObject : Base
  //  where TRevitObject : Element
  //  where TRevitObjectType : ElementType
  //{
  //  public TRevitObjectType RevitObjectType { get; set; }
  //}
  //internal interface IToRevitTyped<TSpeckleObject, TRevitObject, TRevitObjectType, SELF> : IToRevitBase<TSpeckleObject, TRevitObject, SELF>
  //  where TSpeckleObject : Base
  //  where TRevitObject : Element
  //  where TRevitObjectType : ElementType
  //{
  //  public TRevitObjectType RevitObjectType { get; set; }
  //}
  //internal interface IToRevitTyped<TRevitObjectType, SELF>
  //  where TRevitObjectType : ElementType
  //{
  //  public TRevitObjectType RevitObjectType { get; set; }
  //}

  //internal interface IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType> : IToRevitTyped<TSpeckleObject, TRevitObject, TRevitObjectType>
  //  where TSpeckleObject : Base
  //  where TRevitObject : Element
  //  where TRevitObjectType : ElementType
  //{
  //  public ConverterRevit Converter { get; }
  //  public ApplicationObject AppObj { get; }
  //}
  //internal interface IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF> : IToRevitTyped<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
  //  where TSpeckleObject : Base
  //  where TRevitObject : Element
  //  where TRevitObjectType : ElementType
  //  where SELF : IToRevitTypedWithConverter<TSpeckleObject, TRevitObject, TRevitObjectType, SELF>
  //{
  //  public ConverterRevit Converter { get; }
  //  public ApplicationObject AppObj { get; }
  //}
}
