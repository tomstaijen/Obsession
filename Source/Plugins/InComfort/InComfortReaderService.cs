using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using Obsession.Core;
using Obsession.Core.Extensions;

namespace InComfort
{
    public class InComfortReaderService : IServiceModule
    {
        private readonly Configuration _configuration;

        public InComfortReaderService(Configuration configuration)
        {
            _configuration = configuration;
        }

        public StateValues GetState()
        {
            return new StateValues(_configuration.ModuleName, _configuration.ObjectName, new InComfortService().Read(_configuration.Values.GetValueOrDefault("Hostname") as string));
        }

        public bool IsActual(StateValues current)
        {
            return true;
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return new Dictionary<string, Delegate>()
                {
                    {"setTemp", (Action<float>) SetTemperature }
                };
        }

        public void SetTemperature(float temp)
        {
            new InComfortService().SetTemperature(_configuration.GetValue<string>("Hostname"), temp);
        }

        public TimeSpan GetInterval()
        {
            return TimeSpan.FromMinutes(_configuration.GetValue<short>("IntervalInMinutes", 5));
        }
    }

    public class InComfortService {

        public static DateTime Epoch = new DateTime(1970, 1, 1);

        public void SetTemperature(string hostname, float temp)
        {
            var client = new HttpClient();

            var setPoint = Convert.ToInt32((temp-5)*10);
            var timestamp = Convert.ToInt64((DateTime.Now - Epoch).TotalMilliseconds);
            var result = client.GetAsync(string.Format("http://{0}/data.json?heater=0&setpoint={1}&thermostat=0&timestamp={2}", hostname, setPoint, timestamp));
            Console.WriteLine(result.Result.StatusCode);
        }

        public ReadableInComfortData Read(string hostname)
        {
            var client = new HttpClient();
            var response = client.GetAsync(string.Format("http://{0}/data.json?heater=0", hostname));

            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                var data = response.Result.Content.ReadAsStringAsync();
                var raw = JsonConvert.DeserializeObject<RawInComfortData>(data.Result);

                
                return new ReadableInComfortData(raw);
            }
            return null;
        }
    }
}