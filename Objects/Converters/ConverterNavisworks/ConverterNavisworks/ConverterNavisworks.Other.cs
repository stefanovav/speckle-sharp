using System;

namespace Objects.Converter.Navisworks;

// ReSharper disable once UnusedType.Global
public partial class ConverterNavisworks
{
  private static void ConsoleLog(string message, ConsoleColor color = ConsoleColor.Blue)
  {
    Console.WriteLine(message, color);
  }

  private static void ErrorLog(string errorMessage)
  {
    ConsoleLog(errorMessage, ConsoleColor.DarkRed);
  }
}
