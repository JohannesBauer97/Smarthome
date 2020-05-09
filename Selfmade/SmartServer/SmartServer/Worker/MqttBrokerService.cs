using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Server;

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
      _mqttServer.UseApplicationMessageReceivedHandler(args =>
      {
        _logger.LogInformation(args.ApplicationMessage.Topic);
      });
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Starting MqttBrokerService");
      var optionsBuilder = new MqttServerOptionsBuilder().WithDefaultEndpoint();
      return _mqttServer.StartAsync(optionsBuilder.Build());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Stopping MqttBrokerService");
      return _mqttServer.StopAsync();
    }
  }
}