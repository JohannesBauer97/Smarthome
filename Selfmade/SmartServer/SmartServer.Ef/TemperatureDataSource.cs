using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartServer.Common.Models;
using SmartServer.Ef.Abstraction;
using SmartServer.Ef.Models;

namespace SmartServer.Ef
{
  public class TemperatureDataSource : ITemperatureDataSource
  {
    private readonly ILogger<TemperatureDataSource> _logger;
    private readonly IServiceProvider _serviceProvider;

    public TemperatureDataSource(ILogger<TemperatureDataSource> logger, IServiceProvider serviceProvider)
    {
      _logger = logger;
      _serviceProvider = serviceProvider;
    }
    public List<SmartTemperatureClient> GetSmartTemperatureClients()
    {
      var scope = _serviceProvider.CreateScope();
      using (SmartServerContext db = scope.ServiceProvider.GetService<SmartServerContext>())
      {
        return db.SmartTemperatureClients.Select(client => client.ConvertToSmartClient()).ToList();
      }
    }

    public SmartTemperatureClient GetSmartTemperatureClientByChipId(string chipId)
    {
      var scope = _serviceProvider.CreateScope();
      using (SmartServerContext db = scope.ServiceProvider.GetService<SmartServerContext>())
      {
        return db.SmartTemperatureClients.FirstOrDefault(client => client.ChipId == chipId).ConvertToSmartClient();
      }
    }

    public SmartTemperatureClient AddOrUpdateSmartTemperatureClient(SmartTemperatureClient client)
    {
      var scope = _serviceProvider.CreateScope();
      using (SmartServerContext db = scope.ServiceProvider.GetService<SmartServerContext>())
      {
        var storedClient = db.SmartTemperatureClients.FirstOrDefault(c => c.ChipId == client.ChipId);
        if (storedClient == null)
        {
          db.Add(StoredSmartTemperatureClient.FromSmartTemperatureClient(client));
        }
        else
        {
          storedClient.Temperature = client.Temperature;
          storedClient.Humidity = client.Humidity;
          storedClient.LastDataUpdate = client.LastDataUpdate;
        }

        if (db.SaveChanges() <= 0)
        {
          return null;
        }

        return db.SmartTemperatureClients.FirstOrDefault(c => c.ChipId == client.ChipId).ConvertToSmartClient();
      }
    }

    public bool DeleteSmartTemperatureClient(string chipId)
    {
      var scope = _serviceProvider.CreateScope();
      using (SmartServerContext db = scope.ServiceProvider.GetService<SmartServerContext>())
      {
        var client = db.SmartTemperatureClients.FirstOrDefault(c => c.ChipId == chipId);
        if (client == null)
        {
          return false;
        }

        db.SmartTemperatureClients.Remove(client);
        db.SaveChanges();
        return true;
      }
    }
  }
}
