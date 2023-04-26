using System.Collections.Generic;
using Speckle.Core.Models;

namespace Speckle.Core.Kits.Modular;

public abstract class SpeckleModularConverter : SpeckleConverterModule, ISpeckleConverter
{
  public abstract string Author { get; }
  public abstract string WebsiteOrEmail { get; }
  public abstract ProgressReport Report { get; }
  public abstract ReceiveMode ReceiveMode { get; set; }

  public Base ConvertToSpeckle(object @object)
  {
    throw new System.NotImplementedException();
  }

  public List<Base> ConvertToSpeckle(List<object> objects)
  {
    throw new System.NotImplementedException();
  }

  public bool CanConvertToSpeckle(object @object)
  {
    throw new System.NotImplementedException();
  }

  public object ConvertToNative(Base @object)
  {
    throw new System.NotImplementedException();
  }

  public List<object> ConvertToNative(List<Base> objects)
  {
    throw new System.NotImplementedException();
  }

  public bool CanConvertToNative(Base @object)
  {
    throw new System.NotImplementedException();
  }

  public IEnumerable<string> GetServicedApplications()
  {
    throw new System.NotImplementedException();
  }

  public void SetContextDocument(object doc)
  {
    throw new System.NotImplementedException();
  }

  public void SetContextObjects(List<ApplicationObject> objects)
  {
    throw new System.NotImplementedException();
  }

  public void SetPreviousContextObjects(List<ApplicationObject> objects)
  {
    throw new System.NotImplementedException();
  }

  public void SetConverterSettings(object settings)
  {
    throw new System.NotImplementedException();
  }
}
