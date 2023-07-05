using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using RevitMapper.Commands;
using Speckle.Core.Plugins;

namespace RevitMapper
{
  [Guid("5FD2C1C2-473B-4E12-A3F4-AA61979647E9")]
  public class RevitMapperView : IView
  {
    public Guid Id => typeof(RevitMapperView).GUID;

    public string Name { get; }

    public IApp App { get; }

    public IDictionary<string, ICommand> Commands => new Dictionary<string, ICommand>()
    {
      { "init_mapper", new InitMapper(this.App) }
    };

    public RevitMapperView(IApp speckleApp)
    {
      this.App = speckleApp;
    }

    public void UpdateView()
    {
      
    }
  }
}
