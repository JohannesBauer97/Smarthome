using SmartServer.Common;

namespace SmartServer.Worker.Abstraction
{
  public interface IMqttClientService
  {
    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient);
  }
}