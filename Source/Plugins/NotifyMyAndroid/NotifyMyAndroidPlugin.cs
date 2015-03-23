using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core;
using Newtonsoft.Json;
using Obsession.Core;
using Obsession.Core.Extensions;

namespace NotifyMyAndroid
{
    public class NmaServiceModule : IServiceModule
    {
        public static string ApiKeyKey = "NmaApiKey";
        public static string ApplicationKey = "NmaApplicationKey";
        public static string Message = "NmaMessageKey";


        public StateValues GetState(Configuration configuration)
        {
            return null;
        }

        public bool IsActual(Configuration configuration, StateValues current)
        {
            return false;
        }

        public IModuleInstance GetInstance(Configuration configuration)
        {
            return new NmaModuleInstance(configuration);
        }

        public TimeSpan GetInterval(Configuration configuration)
        {
            return TimeSpan.FromDays(1);
        }
    }

    public class NmaModuleInstance : IModuleInstance
    {
        private readonly Configuration _configuration;

        public NmaModuleInstance(Configuration configuration)
        {
            _configuration = configuration;
        }

        public StateValues GetState()
        {
            return null;
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return new Dictionary<string, Delegate>
                {
                    {"nma", (Action<string>) Notify }
                };
        }

        public bool IsActual(StateValues stateValues)
        {
            return true;
        }

        [RuleActionAttribute("notifyMyAndroid")]
        public void Notify(string message)
        {
            new NotifyMyAndroidService().Notify(
                _configuration.Values.GetValueOrDefault(NmaServiceModule.ApiKeyKey) as string,
                _configuration.Values.GetValueOrDefault(NmaServiceModule.ApplicationKey) as string,
                _configuration.Values.GetValueOrDefault(NmaServiceModule.Message) as string,
                message
                );
        }
    }

    public class NotifyMyAndroidService
    {
        public bool Verify(string apiKey)
        {
            var client = new HttpClient();
            var response = client.GetAsync("https://www.notifymyandroid.com/publicapi/verify?apikey=" + apiKey);
            var statuscode = response.Result.StatusCode;

            var content = response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(content.ToString());

            return statuscode == HttpStatusCode.OK;
        }

        public bool Notify(string apikey, string application, string message, string description)
        {
            HttpClient client = new HttpClient();

            var dict = new Dictionary<string, string>();
            dict.Add("apikey", apikey);
            dict.Add("application", application);
            dict.Add("event", message);
            dict.Add("description", description);

            var load = new FormUrlEncodedContent(dict);

            var response = client.PostAsync("https://www.notifymyandroid.com/publicapi/notify", load);

            var content = response.Result.Content.ReadAsStringAsync();
            Console.WriteLine(content.ToString());

            var statuscode = response.Result.StatusCode;
            return statuscode == HttpStatusCode.OK;
        }
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
