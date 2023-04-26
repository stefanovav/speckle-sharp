using Speckle.Core.Kits.Modular;

namespace Speckle.Core.Tests.Kits.Modular;

public class TestObjectModule : SpeckleConverterModule
{
  public override string Description => "My first test module";
  public override string Name => "Test Object Module";
  public override Guid Id => new("09F4D7C2-B5A3-4AE1-A87F-66B7BA571591");

  public TestObjectModule()
    : base()
  {
    WithConverter(new TestObjectConverter());
  }
}
