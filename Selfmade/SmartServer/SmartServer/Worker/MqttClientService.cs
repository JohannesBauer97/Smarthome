using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;
using SmartServer.Common;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttClientService: IMqttClientService, IDisposable
  {
    private readonly ILogger<MqttClientService> _logger;
    private IMqttClient _mqttClient;

    public MqttClientService(ILogger<MqttClientService> logger)
    {
      _logger = logger;
    }

    public async Task StartAsync()
    {
      _logger.LogInformation("Starting MqttClientService");
      _mqttClient = new MqttFactory().CreateMqttClient();
      var options = new MqttClientOptionsBuilder()
        .WithKeepAlivePeriod(TimeSpan.FromSeconds(3))
        .WithClientId(Constants.MQTT_CLIENT_NAME)
        .WithTcpServer("127.0.0.1", 1883)
        .Build();
      await _mqttClient.ConnectAsync(options);
      _mqttClient.UseApplicationMessageReceivedHandler(IncomingMessageHandler);
    }

    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient)
    {
      var foo = "/iot/temperature/" + smartTemperatureClient.ChipId;
      var bar = _mqttClient.SubscribeAsync(foo);
    }

    private Task IncomingMessageHandler(MqttApplicationMessageReceivedEventArgs arg)
    {
      _logger.LogInformation("Incoming Message: {0} --- {1}", arg.ApplicationMessage.Topic,
        Encoding.ASCII.GetString(arg.ApplicationMessage.Payload));
      return Task.CompletedTask;
    }

    public async void Dispose()
    {
      _logger.LogInformation("Stopping MqttClientService");
      await _mqttClient.DisconnectAsync();
    }
  }
}