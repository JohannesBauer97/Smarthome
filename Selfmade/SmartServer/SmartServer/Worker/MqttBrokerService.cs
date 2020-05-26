using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttBrokerService : IHostedService
  {
    private readonly ILogger<MqttBrokerService> _logger;
    private readonly IMqttClientService _mqttClientService;
    private readonly IAutodiscoverService _autodiscoverService;
    private readonly IMqttServer _mqttServer;

    public MqttBrokerService(ILogger<MqttBrokerService> logger, IMqttClientService mqttClientService, IAutodiscoverService autodiscoverService)
    {
      _logger = logger;
      _mqttClientService = mqttClientService;
      _autodiscoverService = autodiscoverService;
      _mqttServer = new MqttFactory().CreateMqttServer();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Starting MqttBrokerService");
      var optionsBuilder = new MqttServerOptionsBuilder().WithDefaultEndpoint();
      await _mqttServer.StartAsync(optionsBuilder.Build());
      _mqttServer.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(args =>
      {
        _logger.LogInformation("connected " + args.ClientId);
      });
      _mqttServer.UseClientConnectedHandler(args =>
      {
        _logger.LogInformation("connected " + args.ClientId);
      });
      await _mqttClientService.StartAsync();
      await _autodiscoverService.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Stopping MqttBrokerService");
      await _mqttServer.StopAsync();
    }
  }
}