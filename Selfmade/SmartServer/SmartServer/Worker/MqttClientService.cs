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
using SmartServer.Common.Models;
using SmartServer.Ef.Abstraction;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class MqttClientService: IMqttClientService, IDisposable
  {
    private readonly ILogger<MqttClientService> _logger;
    private readonly ITemperatureDataSource _temperatureDataSource;
    private IMqttClient _mqttClient;

    public MqttClientService(ILogger<MqttClientService> logger, ITemperatureDataSource temperatureDataSource)
    {
      _logger = logger;
      _temperatureDataSource = temperatureDataSource;
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
      _mqttClient.UseApplicationMessageReceivedHandler(IncomingMessageHandler);
      await _mqttClient.ConnectAsync(options);
      await ReSubscribeToClientsFromDb();
    }

    public void SubscribeToSmartTemperatureClient(SmartTemperatureClient smartTemperatureClient)
    {
      _mqttClient.SubscribeAsync("/iot/temperature/" + smartTemperatureClient.ChipId);
    }

    private Task IncomingMessageHandler(MqttApplicationMessageReceivedEventArgs arg)
    {
      _logger.LogTrace("Incoming Message: {0} --- {1}", arg.ApplicationMessage.Topic,
        Encoding.ASCII.GetString(arg.ApplicationMessage.Payload));
      return Task.CompletedTask;
    }

    /// <summary>
    /// This method should be called once at app startup.
    /// It will subscribe to the clients saved in the db.
    /// </summary>
    /// <returns></returns>
    private Task ReSubscribeToClientsFromDb()
    {
      return Task.Run(() =>
      {
        var temperatureClients = _temperatureDataSource.GetSmartTemperatureClients();
        foreach (var smartTemperatureClient in temperatureClients)
        {
          SubscribeToSmartTemperatureClient(smartTemperatureClient);
        }
      });
    }

    public async void Dispose()
    {
      _logger.LogInformation("Stopping MqttClientService");
      await _mqttClient.DisconnectAsync();
    }
  }
}