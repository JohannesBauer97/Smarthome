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
      throw new NotImplementedException();
    }

    public SmartTemperatureClient AddOrUpdateSmartTemperatureClient(SmartTemperatureClient client)
    {
      throw new NotImplementedException();
    }
  }
}
