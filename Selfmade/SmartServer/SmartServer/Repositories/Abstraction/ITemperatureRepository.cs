using System.Collections.Generic;
using SmartServer.Common.Models;

namespace SmartServer.Repositories.Abstraction
{
  public interface ITemperatureRepository
  {
    public List<SmartTemperatureClient> GetAllDevices();

    public SmartTemperatureClient UpdateSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient);
  }
}