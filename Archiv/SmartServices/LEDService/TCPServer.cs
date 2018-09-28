using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LEDService
{
    public class TCPServer
    {
        private TcpListener _listener { get; set; }
        private readonly int _port;
        private readonly IPAddress _ip;
        public event EventHandler<TcpClient> ClientConnected;

        public TCPServer(string ip = "127.0.0.1", int port = 5555)
        {
            _port = port;
            _ip = IPAddress.Parse(ip);
        }

        public void StartServer()
        {
            _listener = new TcpListener(_ip, _port);
            _listener.Start();
            Console.WriteLine("Started TCP Server on " + _ip + ":" + _port);

            Task waitForClients = new Task(new Action(() =>
            {
                while (true)
                {
                    var clientTask = _listener.AcceptTcpClientAsync();
                    if (clientTask.Result != null)
                    {
                        TcpClient client = clientTask.Result;
                        ClientConnected.Invoke(this, client);
                        Console.WriteLine(client.Client.RemoteEndPoint + " connected.");
                    }
                }
            }));

            waitForClients.Start();
        }

        public async Task<string> SendCommand(TcpClient client, string command, int responseBufferSize = 1024)
        {
            byte[] cmdData = Encoding.ASCII.GetBytes(command);
            await client.GetStream().WriteAsync(cmdData, 0, cmdData.Length);

            byte[] buffer = new byte[responseBufferSize];
            await client.GetStream().ReadAsync(buffer, 0, buffer.Length);

            return Encoding.ASCII.GetString(buffer);
        }
    }
}
