using System;
using System.Collections.Generic;
using System.Text;
using SmartServer.Common.Models;

namespace SmartServer.Ef.Models
{
  public class StoredSmartTemperatureClient : StoredSmartClient
  {
    public StoredSmartTemperatureClient() : base()
    {
    }

    public static StoredSmartTemperatureClient FromSmartTemperatureClient(SmartTemperatureClient client)
    {
      var storedClient = new StoredSmartTemperatureClient();
      storedClient.Discovered = client.Discovered;
      storedClient.ChipId = client.ChipId;
      return storedClient;
    }
  }
}
