using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace SmartServer.Worker
{
  public class MqttService : IHostedService, IDisposable
  {
    private readonly ILogger<MqttService> _logger;
    private readonly IConfiguration _configuration;
    private MqttClient _mqttClient;
    private readonly string _brokerAddress;

    public MqttService(ILogger<MqttService> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
      _brokerAddress = _configuration.GetValue<string>("BrokerAddress");
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
      Task.Run(() =>
      {
        _mqttClient = new MqttClient(_brokerAddress);
        _mqttClient.Connect(Constants.MQTT_CLIENT_NAME);
        _mqttClient.Subscribe(new[] { "/home/data" }, new[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
        _mqttClient.MqttMsgPublishReceived += MqttClientOnMqttMsgPublishReceived;
      }, cancellationToken);
      return Task.CompletedTask;
    }

    private void MqttClientOnMqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
    {
      string message = Encoding.ASCII.GetString(e.Message);
      _logger.LogInformation("Incoming message on topic {0}: {1}", e.Topic, message);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Task.Run(() =>
      {
        _mqttClient.Disconnect();
      }, cancellationToken);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _mqttClient.Disconnect();
    }
  }
}