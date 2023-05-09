namespace ConverterRevitShared.Interfaces
{
  internal interface IHasConvertableObject<TConvertable>
  {
    public TConvertable ConvertableObject { get; }
  }
}
