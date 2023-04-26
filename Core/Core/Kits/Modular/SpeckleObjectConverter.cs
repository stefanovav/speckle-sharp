using System;
using System.Collections.Generic;
using System.Linq;
using Speckle.Core.Models;

namespace Speckle.Core.Kits.Modular;

public abstract class SpeckleObjectConverter : ISpeckleObjectConverter
{
  public virtual object Convert<TInput>(TInput inputObject)
  {
    return Convert<TInput, object>(inputObject);
  }

  public virtual TOutput Convert<TInput, TOutput>(TInput inputObject)
  {
    ISpeckleObjectConverter<TInput, TOutput> converter;
    try
    {
      converter = (ISpeckleObjectConverter<TInput, TOutput>)this;
    }
    catch (InvalidCastException e)
    {
      throw new NonSupportedConversionException(
        $"Converter does not support {typeof(TInput)} to {typeof(TOutput)} conversion",
        e
      );
    }
    return converter.Convert(inputObject);
  }

  public virtual bool CanConvert<TInput, TOutput>()
  {
    try
    {
      var _ = (ISpeckleObjectConverter<TInput, object>)this;
      return true;
    }
    catch (InvalidCastException)
    {
      return false;
    }
  }

  public virtual IEnumerable<Type> CanConvert<TInput>()
  {
    var matchingOutputTypes = GetType()
      .GetInterfaces()
      .Where(
        i =>
          i.IsGenericType
          && i.GetGenericTypeDefinition() == typeof(ISpeckleObjectConverter<,>)
          && i.GetGenericArguments()[0] == typeof(TInput)
      )
      .Select(i => i.GetGenericArguments()[1])
      .ToList();

    var duplicateTypes = matchingOutputTypes.GroupBy(x => x).Where(g => g.Count() > 1).Select(g => g.Key);

    return duplicateTypes;
  }
}
