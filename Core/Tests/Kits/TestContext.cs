using Speckle.Core.Kits.Modular;

namespace Speckle.Core.Tests.Kits.Modular;

public class TestContext : State<TestContext>
{
  public string Units => Core.Kits.Units.Meters;
  public double AngleTolerance => 0.0001;
  public double Tolerance => 0.0001;
}
