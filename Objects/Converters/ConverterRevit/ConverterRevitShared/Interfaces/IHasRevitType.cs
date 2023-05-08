using Autodesk.Revit.DB;

namespace ConverterRevitShared.Interfaces
{
  internal interface IHasRevitType<TRevitObjectType>
    where TRevitObjectType : ElementType
  {
    public TRevitObjectType RevitObjectType { get; set; }
  }
}
