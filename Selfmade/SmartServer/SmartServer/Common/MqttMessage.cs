using System;

namespace SmartServer.Common
{
  public class MqttMessage
  {
    public DateTime Received { get; set; } = DateTime.Now;
    public string Message { get; set; }
  }
}