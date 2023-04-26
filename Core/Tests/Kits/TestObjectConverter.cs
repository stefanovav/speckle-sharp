using Speckle.Core.Kits.Modular;

namespace Speckle.Core.Tests.Kits.Modular;

public class TestObjectConverter
  : SpeckleObjectConverter,
    ISpeckleObjectConverter<string, double>,
    ISpeckleObjectConverter<double, string>
{
  public string Convert(double input)
  {
    try
    {
      return System.Convert.ToString(input);
    }
    catch (Exception e)
    {
      throw new Exception($"Unable to convert ${typeof(string)} to ${typeof(double)}", e);
    }
  }

  public double Convert(string input)
  {
    try
    {
      return System.Convert.ToDouble(input);
    }
    catch (Exception e)
    {
      throw new Exception($"Unable to convert ${typeof(string)} to ${typeof(double)}", e);
    }
  }
}
