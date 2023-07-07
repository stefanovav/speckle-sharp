namespace Speckle.Core.Connectors
{
  /// <summary>
  /// Message for UI.
  /// </summary>
  public readonly struct UIMessage
  {
    /// <summary>
    /// Gets view id (resolveId) of the message.
    /// </summary>
    public string ViewId { get; }

    /// <summary>
    /// Gets name of the function.
    /// </summary>
    public string FunctionName { get; }

    /// <summary>
    /// Gets the serialized arguments of the function.
    /// </summary>
    public string Data { get; }

    public UIMessage(string id, string name, string data)
    {
      this.ViewId = id;
      this.FunctionName = name;
      this.Data = data;
    }
  }
}
