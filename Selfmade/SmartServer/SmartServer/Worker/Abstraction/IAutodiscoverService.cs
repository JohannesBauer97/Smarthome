using System.Collections.Generic;
using System.Threading.Tasks;
using SmartServer.Common;

namespace SmartServer.Worker.Abstraction
{
  public interface IAutodiscoverService
  {
    public Task StartAsync();
  }
}