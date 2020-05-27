using System;

namespace SmartServer.Common.Models
{
  public class SmartTemperatureClient : SmartClient
  {
    public DateTime LastDataUpdate { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
    public SmartTemperatureClient(string chipId) : base(chipId)
    {
    }
  }
}