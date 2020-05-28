using System;
using System.ComponentModel.DataAnnotations;
using SmartServer.Common.Models;

namespace SmartServer.Ef.Models
{
  public class StoredSmartTemperatureClient : IStoredSmartClient
  {
    public DateTime LastDataUpdate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Name { get; set; }
    [Key]
    public string ChipId { get; set; }

    public static StoredSmartTemperatureClient FromSmartTemperatureClient(SmartTemperatureClient client)
    {
      var storedClient = new StoredSmartTemperatureClient();
      storedClient.ChipId = client.ChipId;
      storedClient.Humidity = client.Humidity;
      storedClient.LastDataUpdate = client.LastDataUpdate;
      storedClient.Temperature = client.Temperature;
      storedClient.Name = client.Name;
      return storedClient;
    }
  }
}