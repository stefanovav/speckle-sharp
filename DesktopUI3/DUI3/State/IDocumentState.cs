using System;
using System.Collections.Generic;
using System.Text;
using DUI3.Models;

namespace DUI3.State
{
  public interface IDocumentState
  {
    public List<ModelCard> ReadModelCards();
    public void WriteModelCard(ModelCard modelCard);
  }
}
