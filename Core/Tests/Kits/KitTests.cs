using NUnit.Framework;
using Speckle.Core.Kits.Modular;
using Speckle.Core.Models;

namespace Speckle.Core.Tests.Kits.Modular;

public class KitTests
{
  [Test]
  public void ModularConverterTest()
  {
    var objectConverter = new TestObjectConverter();

    var res1 = objectConverter.Convert<string, double>("1000");
    Assert.That(res1, Is.InstanceOf(typeof(double)));
    Assert.Throws<NonSupportedConversionException>(() => objectConverter.Convert<Fake, OtherFake>(new Fake()));

    var canConvert = objectConverter.CanConvert<string, object>();
    Assert.That(canConvert, Is.True);
  }
}

public class Fake
{
  public string Hey;
}

public class OtherFake
{
  public string OtherHey;
}
