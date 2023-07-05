using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Speckle.Core.Plugins;

namespace RevitMapper
{
  public class RevitMapperPlugin : SpecklePlugin
  {
    public override IView OnLoad(IApp speckleApp)
    {
      return new RevitMapperView(speckleApp);
    }
  }
}
