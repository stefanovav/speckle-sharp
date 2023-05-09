namespace ConverterRevitShared.Interfaces
{
  /// <summary>
  /// Conversion result can be a single value such as a Revit element or a Base object,
  /// or a list of those objects (or something else)
  /// </summary>
  /// <typeparam name="TConversionResult"></typeparam>
  internal interface IHasConversionResult<TConversionResult>
  {
    public TConversionResult ConversionResult { get; set; }
  }
}
