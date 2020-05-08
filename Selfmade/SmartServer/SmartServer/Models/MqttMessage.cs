using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartServer.Models
{
  public class MqttMessage
  {
    public DateTime Received { get; set; } = DateTime.Now;
    public string Message { get; set; }
  }
}
