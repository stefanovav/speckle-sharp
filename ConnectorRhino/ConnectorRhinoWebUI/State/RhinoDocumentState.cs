using System.Collections.Generic;
using DUI3.Models;
using DUI3.State;
using Rhino;
using Speckle.Newtonsoft.Json;

namespace ConnectorRhinoWebUI.State
{
  public class RhinoDocumentState : IDocumentState
  {
    [JsonIgnore]
    public RhinoDoc Doc { get; }

    public RhinoDocumentState(RhinoDoc doc)
    {
      Doc = doc;
    }

    public List<ModelCard> ReadModelCards()
    {
      throw new System.NotImplementedException();
    }

    public void WriteModelCard(ModelCard modelCard)
    {
      throw new System.NotImplementedException();
    }
  }
}
