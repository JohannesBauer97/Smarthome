using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using SmartServer.Common;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttClientService : IHostedService, IMqttClientService
  {
    private readonly ILogger<MqttClientService> _logger;
    private readonly IMqttClient _mqttClient;

    public MqttClientService(ILogger<MqttClientService> logger)
    {
      _logger = logger;
      _mqttClient = new MqttFactory().CreateMqttClient();
      _mqttClient.UseApplicationMessageReceivedHandler(IncomingMessageHandler);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Starting MqttClientService");
      var optionsBuilder = new MqttClientOptionsBuilder().WithClientId(Constants.MQTT_CLIENT_NAME)
        .WithTcpServer("localhost");
      return _mqttClient.ConnectAsync(optionsBuilder.Build(), cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _logger.LogInformation("Stopping MqttClientService");
      return _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions(), cancellationToken);
    }

    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient)
    {
      _mqttClient.SubscribeAsync("/iot/temperature/" + smartTemperatureClient.ChipId);
    }

    private void IncomingMessageHandler(MqttApplicationMessageReceivedEventArgs arg)
    {
      _logger.LogInformation("Incoming Message: {0} --- {1}", arg.ApplicationMessage.Topic, Encoding.ASCII.GetString(arg.ApplicationMessage.Payload));
    }
  }
}