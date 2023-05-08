using System;
using System.Collections.Generic;

namespace ConverterRevitShared.Conversions
{
  internal abstract class ConversionBuilder<TReturn, TSelf>
    where TSelf : ConversionBuilder<TReturn, TSelf>
  {
    internal List<Action> actions = new List<Action>();
    internal TReturn ReturnObject { get; set; }
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
      return ReturnObject;
    }
  }
}
