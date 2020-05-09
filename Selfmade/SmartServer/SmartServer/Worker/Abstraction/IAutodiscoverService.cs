using System.Collections.Generic;
using SmartServer.Common;

namespace SmartServer.Worker.Abstraction
{
  public interface IAutodiscoverService
  {
    public HashSet<SmartClient> SmartClients { get; }
  }
}