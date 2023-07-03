using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.Dui3App;
using SpeckleRhino.UiController.Commands;

namespace SpeckleRhino.UiController.Views
{
    public interface IView
    {
      public Guid Id { get; }
      public string Name { get; }
      public SpeckleApp App { get; }
      public IDictionary<string, ICommand> Commands { get; }
      public void UpdateView();
    }
}
