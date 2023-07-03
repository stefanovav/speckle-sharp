using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.State;

namespace SpeckleRhino.UiController.Actions
{
    public interface IAction
    {
      public AppState UpdateState(AppState state);
    }
}
