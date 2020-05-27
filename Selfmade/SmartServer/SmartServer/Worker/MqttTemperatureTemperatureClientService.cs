using System;
using System.Globalization;
using System.Linq;
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
  public class MqttTemperatureTemperatureClientService: IMqttTemperatureClientService, IDisposable
  {
    private readonly ILogger<MqttTemperatureTemperatureClientService> _logger;
    private readonly ITemperatureDataSource _temperatureDataSource;
    private IMqttClient _mqttClient;

    public MqttTemperatureTemperatureClientService(ILogger<MqttTemperatureTemperatureClientService> logger, ITemperatureDataSource temperatureDataSource)
    {
      _logger = logger;
      _temperatureDataSource = temperatureDataSource;
    }

    public async Task StartAsync()
    {
      _logger.LogInformation("Starting MqttTemperatureTemperatureClientService");
      _mqttClient = new MqttFactory().CreateMqttClient();
      var options = new MqttClientOptionsBuilder()
        .WithKeepAlivePeriod(TimeSpan.FromSeconds(3))
        .WithClientId(Constants.MQTT_CLIENT_NAME)
        .WithTcpServer("127.0.0.1", 1883)
        .Build();
      _mqttClient.UseApplicationMessageReceivedHandler(IncomingMessageHandler);
      await _mqttClient.ConnectAsync(options);
      await _mqttClient.SubscribeAsync("/iot/#");
    }

    private Task IncomingMessageHandler(MqttApplicationMessageReceivedEventArgs arg)
    {
      string topic = arg.ApplicationMessage.Topic;
      string message = Encoding.ASCII.GetString(arg.ApplicationMessage.Payload);
      _logger.LogTrace("Incoming Message: {0} --- {1}", topic, message);

      if (topic.StartsWith(Constants.MQTT_TEMPERATURE_TOPIC))
      {
        HandleIncomingTemperatureMessage(topic, message);
      }

      return Task.CompletedTask;
    }

    public void HandleIncomingTemperatureMessage(string topic, string message)
    {
      string chipId = topic.Split('/').Last();
      if (chipId == null)
      {
        return;
      }
      string[] msgParts = message.Split(';');
      double newTemperature, newHumidity;
      if (!Double.TryParse(msgParts.First(),NumberStyles.Any, CultureInfo.InvariantCulture, out newTemperature) || !Double.TryParse(msgParts.Last(), NumberStyles.Any, CultureInfo.InvariantCulture, out newHumidity))
      {
        return;
      }

      if (double.IsNaN(newHumidity) || double.IsNaN(newTemperature))
      {
        _logger.LogWarning("Temperature/Humidity Chip {0} is pushing NaN values.", chipId);
        return;
      }
      SmartTemperatureClient client = _temperatureDataSource.GetSmartTemperatureClientByChipId(chipId);
      if (client == null)
      {
        client = new SmartTemperatureClient(chipId);
      }
      client.Temperature = newTemperature;
      client.Humidity = newHumidity;
      client.LastDataUpdate = DateTime.Now;
      _temperatureDataSource.AddOrUpdateSmartTemperatureClient(client);
    }

    public async void Dispose()
    {
      _logger.LogInformation("Stopping MqttTemperatureTemperatureClientService");
      if (_mqttClient != null)
      {
        await _mqttClient.DisconnectAsync();
      }
    }
  }
}