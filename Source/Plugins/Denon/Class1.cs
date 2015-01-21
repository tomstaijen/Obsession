using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Denon
{
    public class DenonController
    {

    }

    public class DenonConnection
    {
        private TcpClient _tcpClient;
        public void Connect(string ip, int port = 23)
        {
            _tcpClient = new TcpClient();
            _tcpClient.Connect(ip, port);
            Action a = () => Receive();
            a.BeginInvoke(a.EndInvoke, null);
        }

        public void Receive()
        {
            var stream = _tcpClient.GetStream();
            while (true)
            {
                if (stream.DataAvailable)
                {
                    byte[] bytes = new byte[_tcpClient.ReceiveBufferSize];

                    stream.Read(bytes, 0, _tcpClient.ReceiveBufferSize);

                    var s = Encoding.UTF8.GetString(bytes);
                    s = s.Trim('\0');

                    if (!string.IsNullOrEmpty(s))
                        Console.WriteLine(s);
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        public void Write(string command)
        {
            var bytes = Encoding.UTF8.GetBytes(command + '\r');
            _tcpClient.GetStream().Write(bytes, 0, bytes.Length);
        }
    }
}
