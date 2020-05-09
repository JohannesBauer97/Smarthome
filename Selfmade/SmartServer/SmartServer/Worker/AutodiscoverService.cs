using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SmartServer.Worker
{
  public class AutodiscoverService : IHostedService, IDisposable
  {
    private readonly ILogger<AutodiscoverService> _logger;
    private readonly IConfiguration _configuration;
    private UdpClient _udpClient = new UdpClient(Constants.AUTODISCOVER_PORT);
    private IPEndPoint _ipEndPoint = new IPEndPoint(IPAddress.Broadcast, Constants.AUTODISCOVER_PORT);

    public AutodiscoverService(ILogger<AutodiscoverService> logger, IConfiguration configuration)
    {
      _logger = logger;
      _configuration = configuration;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
      Task.Run(DoWork, cancellationToken);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      Task.Run(() =>
      {
        _udpClient.Close();
        _udpClient = null;
      }, cancellationToken);
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _udpClient.Close();
      _udpClient = null;
    }

    private void DoWork()
    {
      _udpClient ??= new UdpClient(Constants.AUTODISCOVER_PORT);
      try
      {
        while (true)
        {
          byte[] bytes = _udpClient.Receive(ref _ipEndPoint);
          string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
          if (msg.StartsWith("autodiscover"))
          {
            string[] discoverData = msg.Split(':');
            if (discoverData.Length < 3)
            {
              return;
            }

            _logger.LogInformation("Got autodiscover message from {0}", _ipEndPoint.Address);

            string type = discoverData[1];
            string chipId = discoverData[2];
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
      catch (Exception exception)
      {
        _logger.LogError(exception.Message);
      }
      finally
      {
        _udpClient.Close();
      }
    }

    private byte[] CreateTemperatureAutodiscoverResponse(string chipId)
    {
      var address = _configuration.GetValue<string>("BrokerAddress");
      if (String.IsNullOrEmpty(address))
      {
        throw new Exception("BrokerAddress is not configured.");
      }
      return Encoding.ASCII.GetBytes(address);
    }
  }

}
