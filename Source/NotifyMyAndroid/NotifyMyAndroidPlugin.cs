using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;

namespace NotifyMyAndroid
{
    public class NotifyMyAndroidPlugin : INotificationPlugin
    {
        public bool Verify(string apiKey)
        {
            var client = new HttpClient();
            var response = client.GetAsync("https://www.notifymyandroid.com/publicapi/verify?apikey=" + apiKey);
            var statuscode = response.Result.StatusCode;
            return statuscode == HttpStatusCode.OK;
        }

        public bool Notify(string message)
        {
            HttpClient client = new HttpClient();

            var n = new Notify
                {
                    ApiKey = "9bdad31063771a8b755088ed25ac4aa00034aad44442d0a3",
                    Application = "Obsession.NET",
                    Event = "Hi",
                    Description = message
                };

            var theData = JsonConvert.SerializeObject(n );

            var response = client.PostAsync("https://www.notifymyandroid.com/publicapi/notify",
                             new StringContent(theData));

            var statuscode = response.Result.StatusCode;
            return statuscode == HttpStatusCode.OK;
        }
    }

    public class Notify
    {
        [JsonProperty("apikey")]
        public string ApiKey { get; set; }

        [JsonProperty("application")]
        public string Application { get; set; }

        [JsonProperty("event")]
        public string Event { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("priority")]
        public Priority Priority { get; set; }

        [JsonProperty("developerkey")]
        public string DeveloperKey { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("content-type")]
        public string ContentType { get; set; }
    }

    public enum Priority : short
    {
        VeryLow = -2,
        Moderate = -1,
        Normal = 0,
        High = 1,
        Emergency = 2
    }
}
