using System.Threading.Tasks;
using SmartServer.Common;
using SmartServer.Common.Models;

namespace SmartServer.Worker.Abstraction
{
  public interface IMqttTemperatureClientService
  {
    public Task StartAsync();
  }
}