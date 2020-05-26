using System;
using System.Collections.Generic;
using System.Text;
using SmartServer.Common.Models;

namespace SmartServer.Ef.Abstraction
{
  public interface ITemperatureDataSource
  {
    public List<SmartTemperatureClient> GetSmartTemperatureClients();
    public SmartTemperatureClient GetSmartTemperatureClientByChipId(string chipId);
    public SmartTemperatureClient AddOrUpdateSmartTemperatureClient(SmartTemperatureClient client);
    public bool DeleteSmartTemperatureClient(string chipId);
  }
}
