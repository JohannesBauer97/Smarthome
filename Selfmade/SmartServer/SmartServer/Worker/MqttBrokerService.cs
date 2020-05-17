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
    private readonly IMqttServer _mqttServer;

    public MqttBrokerService(ILogger<MqttBrokerService> logger)
    {
      _logger = logger;
      _mqttServer = new MqttFactory().CreateMqttServer();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Starting MqttBrokerService");
      var optionsBuilder = new MqttServerOptionsBuilder().WithDefaultEndpoint();
      await _mqttServer.StartAsync(optionsBuilder.Build());
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Stopping MqttBrokerService");
      await _mqttServer.StopAsync();
    }
  }
}