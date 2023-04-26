using System;
using Speckle.Core.Models;

namespace Speckle.Core.Kits.Modular;

public interface ISpeckleConverterInfo
{
  string Description { get; }
  string Name { get; }
  Guid Id { get; }
}
