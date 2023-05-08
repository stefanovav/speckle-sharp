namespace ConverterRevitShared.Interfaces
{
  internal interface IHasRevitObject<TRevitObject>
  {
    public TRevitObject RevitObject { get; set; }
  }
}
