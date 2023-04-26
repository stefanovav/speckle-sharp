namespace Speckle.Core.Kits.Modular;

public interface ISpeckleConverterModule : ISpeckleObjectConverter, ISpeckleConverterInfo
{
  ISpeckleConverterModule WithConverter(ISpeckleObjectConverter converter);
}
