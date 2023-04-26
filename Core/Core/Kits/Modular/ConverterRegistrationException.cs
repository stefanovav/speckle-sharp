using System;

namespace Speckle.Core.Kits.Modular;

public class ConverterRegistrationException : Exception
{
  public ConverterRegistrationException(string message)
    : base(message) { }

  public ConverterRegistrationException(string message, Exception innerException)
    : base(message, innerException) { }

  public ConverterRegistrationException() { }
}

public class NonSupportedConversionException : Exception
{
  public NonSupportedConversionException(string message)
    : base(message) { }

  public NonSupportedConversionException(string message, Exception innerException)
    : base(message, innerException) { }

  public NonSupportedConversionException() { }
}
