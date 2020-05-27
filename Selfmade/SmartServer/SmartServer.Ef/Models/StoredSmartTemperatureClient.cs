using System;
using System.Collections.Generic;
using System.Text;
using SmartServer.Common.Models;

namespace SmartServer.Ef.Models
{
  public class StoredSmartTemperatureClient : StoredSmartClient
  {
    public DateTime LastDataUpdate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }

    public StoredSmartTemperatureClient() : base()
    {
    }

    public static StoredSmartTemperatureClient FromSmartTemperatureClient(SmartTemperatureClient client)
    {
      var storedClient = new StoredSmartTemperatureClient();
      storedClient.Discovered = client.Discovered;
      storedClient.ChipId = client.ChipId;
      storedClient.Humidity = client.Humidity;
      storedClient.LastDataUpdate = client.LastDataUpdate;
      storedClient.Temperature = client.Temperature;
      return storedClient;
    }
  }
}
