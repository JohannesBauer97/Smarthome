using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using SmartServer.Common.Models;
using SmartServer.Ef.Abstraction;
using SmartServer.Repositories.Abstraction;

namespace SmartServer.Repositories
{
  public class TemperatureRepository : ITemperatureRepository
  {
    private readonly ILogger<TemperatureRepository> _logger;
    private readonly ITemperatureDataSource _temperatureDataSource;

    public TemperatureRepository(ILogger<TemperatureRepository> logger, ITemperatureDataSource temperatureDataSource)
    {
      _logger = logger;
      _temperatureDataSource = temperatureDataSource;
    }
    public List<SmartTemperatureClient> GetAllDevices()
    {
      return _temperatureDataSource.GetSmartTemperatureClients();
    }
  }
}