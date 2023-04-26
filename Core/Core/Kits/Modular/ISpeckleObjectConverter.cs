using System;
using System.Collections.Generic;

namespace Speckle.Core.Kits.Modular;

public interface ISpeckleObjectConverter<in TInput, out TOutput> : ISpeckleObjectConverter
{
  TOutput Convert(TInput input);
}

public interface ISpeckleObjectConverter
{
  object Convert<TInput>(TInput inputObject);
  TOutput Convert<TInput, TOutput>(TInput inputObject);
  bool CanConvert<TInput, TOutput>();
  IEnumerable<Type> CanConvert<TInput>();
}
