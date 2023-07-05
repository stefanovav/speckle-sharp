using System;

namespace Speckle.Core.Plugins
{
  public interface IPlugin
  {
    /// <summary>
    /// Gets this Plugin's id.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Gets this Plugin's description.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets this Plugin's name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets this Plugin's author.
    /// </summary>
    string Author { get; }

    /// <summary>
    /// Gets the website (or email) to contact the Plugin's author.
    /// </summary>
    string WebsiteOrEmail { get; }

    /// <summary>
    /// Gets the UI url of the Plugin.
    /// </summary>
    string Url { get; }
  }
}
