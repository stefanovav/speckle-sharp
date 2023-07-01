using System;
using System.Collections.Generic;
using System.Text;
using SpeckleRhino.State;
using SpeckleRhino.UiController;

namespace SpeckleRhino.Dui3App
{
  public class SpeckleApp
  {
    public AppState State { get; }
    public SpeckleUiController UiController { get; }

    public SpeckleApp(AppState state, SpeckleUiController uiController) 
    { 
      this.State = state;
      this.UiController = uiController;
    }
  }
}
