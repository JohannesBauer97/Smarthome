using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartServer.Common;
using SmartServer.Common.Models;
using SmartServer.Ef.Abstraction;
using SmartServer.Worker.Abstraction;

namespace SmartServer.Worker
{
  public class AutodiscoverService : IAutodiscoverService, IDisposable
  {
    private readonly IConfiguration _configuration;
    private readonly ILogger<AutodiscoverService> _logger;
    private IPEndPoint _ipEndPoint = new IPEndPoint(IPAddress.Broadcast, Constants.AUTODISCOVER_PORT);
    private UdpClient _udpClient = new UdpClient(Constants.AUTODISCOVER_PORT);

    public AutodiscoverService(ILogger<AutodiscoverService> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
    }

    public Task StartAsync()
    {
      _logger.LogInformation("Starting AutodiscoverService");
      Task worker = Task.Run(DoWork);
      return worker.IsCanceled ? worker : Task.CompletedTask;
    }

    private void DoWork()
    {
      _udpClient ??= new UdpClient(Constants.AUTODISCOVER_PORT);
      try
      {
        while (true)
        {

          var bytes = _udpClient.Receive(ref _ipEndPoint);
          var msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
          if (msg.StartsWith("autodiscover"))
          {
            var discoverData = msg.Split(':');
            if (discoverData.Length < 3) return;

            _logger.LogInformation("Got autodiscover message from {0}", _ipEndPoint.Address);

            var type = discoverData[1];
            var chipId = discoverData[2];
            byte[] responseMsg;

            switch (discoverData[1])
            {
              case "temperature":
                responseMsg = CreateTemperatureAutodiscoverResponse(chipId);
                break;
              default:
                _logger.LogWarning("Unknown type in autodiscover message: {0}", type);
                return;
            }

            _udpClient.Send(responseMsg, responseMsg.Length, _ipEndPoint);
          }
        }
      }
      catch (SocketException socketException)
      {
        _logger.LogError(socketException.Message);
      }
      finally
      {
        _udpClient.Close();
      }
    }

    private byte[] CreateTemperatureAutodiscoverResponse(string chipId)
    {
      var address = _configuration.GetValue<string>("BrokerAddress");
      if (string.IsNullOrEmpty(address)) throw new Exception("BrokerAddress is not configured.");

      return Encoding.ASCII.GetBytes(address);
    }

    public void Dispose()
    {
      _logger.LogInformation("Stopping AutodiscoverService");
      _udpClient?.Close();
    }
  }
}