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
      Task.Run(DoWork);
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      _udpClient.Close();
      _udpClient = null;
      return Task.CompletedTask;
    }

    public void Dispose()
    {
      _udpClient.Close();
      _udpClient = null;
    }

    private void DoWork()
    {
      if (_udpClient == null)
      {
        _udpClient = new UdpClient(Constants.AUTODISCOVER_PORT);
      }
      try
      {
        while (true)
        {
          byte[] bytes = _udpClient.Receive(ref _ipEndPoint);
          string msg = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
          if (msg == "autodiscover")
          {
            _logger.LogInformation("Got autodiscover message from {0}", _ipEndPoint.Address);
            var address = _configuration.GetValue<string>("BrokerAddress");
            if (address != null)
            {
              byte[] brokerData = Encoding.ASCII.GetBytes(address);
              _udpClient.Send(brokerData, brokerData.Length, _ipEndPoint);
            }
            else
            {
              _logger.LogError("BrokerAddress not configured");
            }
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
  }

}
