using System;
using System.Collections.Generic;
using ConverterRevitShared.Interfaces;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilder<TConvertable, TReturn, TSelf> : IConversion<TConvertable, TReturn>
    where TSelf : ConversionBuilder<TConvertable, TReturn, TSelf>
  {
    protected ConversionBuilder(TConvertable convertable) 
    { 
      ConvertableObject = convertable;
    }
    internal List<Action> actions = new List<Action>();
    public TReturn ConversionResult { get; set; }
    public TConvertable ConvertableObject { get; }

    public TSelf Do(Action action)
    {
      actions.Add(action);
      return (TSelf)this;
    }

    public TReturn Convert()
    {
      foreach (var action in actions)
      {
        try
        {
          action();
        }
        catch (Exception e)
        {
          throw;
        }
      }
      return ConversionResult;
    }
  }
}
