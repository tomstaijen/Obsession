using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Bluetooth.Factory;
using InTheHand.Net.Sockets;
using Obsession.Core;

namespace Bluetooth
{
    public class BluetoothService : IServiceModule
    {
        private readonly Configuration _configuration;

        public BluetoothService(Configuration configuration)
        {
            _configuration = configuration;
        }

        public StateValues GetState()
        {
            var client = new BluetoothClient();
            var devices = client.DiscoverDevices();

            return new StateValues(_configuration.ModuleName, _configuration.ObjectName)
                .AddValue("devices", devices.ToDictionary(d => d.DeviceAddress, d => d.DeviceName));
        }

        public bool IsActual(StateValues current)
        {
            return true;
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(1);
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return null;
        }
    }
}
