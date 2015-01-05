using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
            var tcpClient = new TcpClient();
            tcpClient.Connect(ip, port);
            tcpClient = _tcpClient;
        }


    }
}
