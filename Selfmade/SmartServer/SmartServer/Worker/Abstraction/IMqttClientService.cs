using System.Threading.Tasks;
using SmartServer.Common;

namespace SmartServer.Worker.Abstraction
{
  public interface IMqttClientService
  {
    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient);
    public bool IsReady { get; }
    public Task StartAsync();
  }
}