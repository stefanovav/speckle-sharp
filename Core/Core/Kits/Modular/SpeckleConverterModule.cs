using System;
using System.Collections.Generic;
using System.Linq;

namespace Speckle.Core.Kits.Modular;

public abstract class SpeckleConverterModule : SpeckleObjectConverter, ISpeckleConverterModule
{
  protected readonly IList<ISpeckleObjectConverter> Converters;

  protected SpeckleConverterModule()
  {
    Converters = new List<ISpeckleObjectConverter>();
  }

  public abstract string Description { get; }
  public abstract string Name { get; }
  public abstract Guid Id { get; }

  public override object Convert<TInput>(TInput inputObject)
  {
    var converter = Converters.FirstOrDefault(c => c.CanConvert<TInput, object>());
    return converter != null ? converter.Convert(inputObject) : base.Convert(inputObject);
  }

  public override TOutput Convert<TInput, TOutput>(TInput inputObject)
  {
    var converter = Converters.FirstOrDefault(c => c.CanConvert<TInput, TOutput>());
    return converter != null
      ? converter.Convert<TInput, TOutput>(inputObject)
      : base.Convert<TInput, TOutput>(inputObject);
  }

  public override bool CanConvert<TInput, TOutput>()
  {
    return Converters.Any(converter => converter.CanConvert<TInput, TOutput>()) || base.CanConvert<TInput, TOutput>();
  }

  public override IEnumerable<Type> CanConvert<TInput>()
  {
    return Converters.SelectMany(converter => converter.CanConvert<TInput>()).Concat(base.CanConvert<TInput>());
  }

  public ISpeckleConverterModule WithConverter(ISpeckleObjectConverter converter)
  {
    Converters.Add(converter);
    return this;
  }
}
