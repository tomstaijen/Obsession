using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;

namespace Bluetooth
{
    public class BluetoothService
    {
        public void Test()
        {
            var client = new BluetoothClient();
            var devices = client.DiscoverDevices();
            foreach (var d in devices)
            {
                Console.WriteLine("Hi " + d.DeviceName);
            }
        }
    }
}
