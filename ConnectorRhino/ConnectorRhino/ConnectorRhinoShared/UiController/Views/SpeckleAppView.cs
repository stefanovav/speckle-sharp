using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Speckle.Core.Connectors;
using SpeckleRhino.UiController.Commands;

namespace SpeckleRhino.UiController.Views
{
  /// <summary>
  /// Collects core Speckle commands.
  /// </summary>
  [Guid("2612BAEC-0766-42CB-891A-B61D7EB95569")]
  public class SpeckleAppView : IView
  {
    /// <inheritdoc/>
    public Guid Id => typeof(SpeckleAppView).GUID;

    /// <inheritdoc/>
    public string Name => "SpeckleApp";

    /// <inheritdoc/>
    public IApp App { get; }

    /// <inheritdoc/>
    public IDictionary<string, ICommand> Commands => new Dictionary<string, ICommand>()
    {
      { "init_local_accounts", new InitLocalAccounts(this.App, this.Id) }
    };

    public SpeckleAppView(IApp app)
    {
      this.App = app;
    }
  }
}
