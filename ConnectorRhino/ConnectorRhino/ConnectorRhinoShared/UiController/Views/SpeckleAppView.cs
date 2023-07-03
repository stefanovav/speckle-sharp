using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using SpeckleRhino.Dui3App;
using SpeckleRhino.UiController.Commands;

namespace SpeckleRhino.UiController.Views
{
  [Guid("2612BAEC-0766-42CB-891A-B61D7EB95569")]
  public class SpeckleAppView : IView
  {
    public Guid Id => typeof(SpeckleAppView).GUID;

    public string Name => "SpeckleApp";

    public SpeckleApp App { get; }

    public SpeckleAppView(SpeckleApp app)
    {
      this.App = app;
    }

    public void UpdateView()
    {
      
    }

    public IDictionary<string, ICommand> Commands => new Dictionary<string, ICommand>()
    {
      { "init_local_accounts", new InitLocalAccounts(this.App) }
    };
  }
}
