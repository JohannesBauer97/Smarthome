using System;

namespace SmartServer.Common.Models
{
  public class SmartTemperatureClient : ISmartClient
  {
    public DateTime LastDataUpdate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public string Name { get; set; }
    public string ChipId { get; set; }
  }
}