using System.Collections.Generic;
using DUI3.Models;
using DUI3.Utils;
using Speckle.Newtonsoft.Json;

namespace DUI3.State;

public class DocumentState
{
  public List<ModelCard> Models { get; set; } = new List<ModelCard>();

  private static readonly JsonSerializerSettings SerializerOptions = SerializationSettingsFactory.GetSerializerSettings();

  public string Serialize()
  {
    var serialized = JsonConvert.SerializeObject(this, SerializerOptions);
    return serialized;
  }

  public static DocumentState Deserialize(string state)
  {
    var docState = JsonConvert.DeserializeObject<DocumentState>(state, SerializerOptions);
    return docState;
  }
}


