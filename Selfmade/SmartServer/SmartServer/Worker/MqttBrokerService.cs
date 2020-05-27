using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using SmartServer.Ef;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttBrokerService : IHostedService
  {
    private readonly ILogger<MqttBrokerService> _logger;
    private readonly IMqttTemperatureClientService _mqttTemperatureClientService;
    private readonly IAutodiscoverService _autodiscoverService;
    private readonly IServiceProvider _serviceProvider;
    private readonly IMqttServer _mqttServer;

    public MqttBrokerService(ILogger<MqttBrokerService> logger, IMqttTemperatureClientService mqttTemperatureClientService, IAutodiscoverService autodiscoverService, IServiceProvider serviceProvider)
    {
      _logger = logger;
      _mqttTemperatureClientService = mqttTemperatureClientService;
      _autodiscoverService = autodiscoverService;
      _serviceProvider = serviceProvider;
      _mqttServer = new MqttFactory().CreateMqttServer();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      using (var db = _serviceProvider.CreateScope().ServiceProvider.GetService<SmartServerContext>())
      {
        await db.Database.MigrateAsync(cancellationToken: cancellationToken);
      }
      _logger.LogInformation("Starting MqttBrokerService");
      var optionsBuilder = new MqttServerOptionsBuilder().WithDefaultEndpoint();
      await _mqttServer.StartAsync(optionsBuilder.Build());
      await _mqttTemperatureClientService.StartAsync();
      await _autodiscoverService.StartAsync();
      _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(args => _logger.LogDebug("Client {0} connected.", args.ClientId));
      _mqttServer.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(args => _logger.LogDebug("Client {0} disconnected ({1}).", args.ClientId, args.DisconnectType));
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Stopping MqttBrokerService");
      await _mqttServer.StopAsync();
    }
  }
}