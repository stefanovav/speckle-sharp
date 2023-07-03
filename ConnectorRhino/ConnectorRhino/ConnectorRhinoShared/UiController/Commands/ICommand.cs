using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.Dui3App;

namespace SpeckleRhino.UiController.Commands
{
    public interface ICommand
    {
      public string Name { get; }

      public SpeckleApp App { get; }

      public void Execute(object data);
    }
}
