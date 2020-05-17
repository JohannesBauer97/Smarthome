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
using SmartServer.Common;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttClientService: IMqttClientService, IDisposable
  {
    private readonly ILogger<MqttClientService> _logger;
    private readonly IMqttClient _mqttClient;
    public bool IsReady { get; private set; }

    public MqttClientService(ILogger<MqttClientService> logger)
    {
      _logger = logger;
      _mqttClient = new MqttFactory().CreateMqttClient();
      _mqttClient.UseApplicationMessageReceivedHandler(IncomingMessageHandler);
    }

    public async Task StartAsync()
    {
      _logger.LogInformation("Starting MqttClientService");
      var optionsBuilder = new MqttClientOptionsBuilder().WithClientId(Constants.MQTT_CLIENT_NAME)
        .WithTcpServer("127.0.0.1");
      var authResult = await _mqttClient.ConnectAsync(optionsBuilder.Build());
      if (authResult.ResultCode != MqttClientConnectResultCode.Success)
        throw new MqttConnectingFailedException(authResult);

      IsReady = true;
    }

    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient)
    {
      var foo = "/iot/temperature/" + smartTemperatureClient.ChipId;
      var bar = _mqttClient.SubscribeAsync(foo);
      var lol = bar.Result;
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
      await _mqttClient.DisconnectAsync(new MqttClientDisconnectOptions());
      IsReady = false;
    }
  }
}