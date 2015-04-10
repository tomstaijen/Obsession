using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Xbmc
{
    public class XbmcConfiguration
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }

    public class XbmcApi
    {
        private readonly XbmcConfiguration _configuration;

        public XbmcApi(XbmcConfiguration configuration)
        {
            _configuration = configuration;
        }
    }

    public class JsonRpcRequest
    {
        public JsonRpcRequest()
        {
            Version = "2.0";
            Id = Guid.NewGuid();
            Params = new Dictionary<string, object>();
        }

        public JsonRpcRequest(string method) : this()
        {
            Method = method;
        }

        [JsonProperty("jsonrpc")]
        public string Version { get; private set; }

        [JsonProperty("method")]
        public string Method { get; private set; }

        [JsonProperty("params")]
        public Dictionary<string, object> Params { get; private set; }

        [JsonProperty("id")]
        public object Id { get; private set; }
    }

    public class Communicator
    {
        private readonly string _hostname;
        private readonly string _user;
        private readonly string _pass;

        public static string MediaTypeJson = "application/json";

        public Communicator(string hostname, string user, string pass)
        {
            _hostname = hostname;
            _user = user;
            _pass = pass;
        }

        protected string JsonRpcUrl
        {
            get { return string.Format("http://{0}:8080/jsonrpc", _hostname); }
        }

        public dynamic DoIt(JsonRpcRequest request)
        {
            //var message = new StringContent("{\"jsonrpc\": \"2.0\", \"method\": \"Player.GetActivePlayers\", \"id\": 1}",Encoding.UTF8, "application/json");
            //var message = "{ \"jsonrpc\": \"2.0\", \"method\": \"JSONRPC.Introspect\", \"params\": { \"filter\": { \"id\": \"AudioLibrary.GetAlbums\", \"type\": \"method\" } }, \"id\": 1 }";

            var message = JsonConvert.SerializeObject(request);

            var client = new HttpClient();
            // auth
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(string.Format("{0}:{1}", _user, _pass))));
            // content-type
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeJson));
            // do it!
            var result = client.PostAsync(JsonRpcUrl, new StringContent(message, Encoding.UTF8, MediaTypeJson)).Result;

            if( result.StatusCode != HttpStatusCode.OK )
                throw new Exception(result.StatusCode.ToString());

            var resultValue = JsonConvert.DeserializeObject(result.Content.ReadAsStringAsync().Result) as dynamic;

            if (!Guid.Parse((resultValue.id as JValue).ToString()).Equals(request.Id))
            {
                throw new Exception("Invalid response id");
            }
            if (resultValue.error != null)
            {
                throw new Exception(resultValue.error.ToString());
            }
            return resultValue.result;
        }

        public dynamic GetActivePlayers()
        {
            return DoIt(new JsonRpcRequest("Player.GetActivePlayers"));
        }

        public dynamic GetItemVideo(long playerId)
        {
            var request = 
                new JsonRpcRequest("Player.GetItem")
                .WithParameter("playerid", playerId)
                .WithParameter("properties",
                               new[]
                                   {
                                       "title", "album", "artist", "season", "episode", "duration", "showtitle", "tvshowid"
                                       , "thumbnail", "file", "fanart", "streamdetails"
                                   });

            return DoIt(request);
        }

        public dynamic GetPlayerProperties(long playerId)
        {
            var request = new JsonRpcRequest("Player.GetProperties")
            .WithParameter("playerid", playerId)
            .WithParameter("properties",
                               new[]
                                   {
                                       "time", "totaltime","speed","percentage", "position", "type", 
                                   });

            return DoIt(request);
        }

        public dynamic PlayPause(long playerId)
        {
            return DoIt(new JsonRpcRequest("Player.PlayPause")
                            .WithParameter("playerid", playerId));
        }

        public INavigate Navigate()
        {
            return new Navigator(this);
        }
    }

    public class Navigator : CommunicatorModule, INavigate
    {
        public Navigator(Communicator communicator) : base(communicator)
        {
        }

        public void Left()
        {
            Fire(new JsonRpcRequest("Input.Left"));
        }

        public void Right()
        {
            Fire(new JsonRpcRequest("Input.Right"));
        }

        public void Home()
        {
            Fire(new JsonRpcRequest("Input.Home"));
        }

        public void Info()
        {
            Fire(new JsonRpcRequest("Input.Info"));
        }

        public void Select()
        {
            Fire(new JsonRpcRequest("Input.Select"));
        }
    }

    public abstract class CommunicatorModule
    {
        private readonly Communicator _communicator;

        protected CommunicatorModule(Communicator communicator)
        {
            _communicator = communicator;
        }

        protected void Fire(JsonRpcRequest request)
        {
            _communicator.DoIt(request);
        }
    }

    public interface INavigate
    {
        void Left();
        void Right();
        void Home();
        void Info();
        void Select();
    }

    public static class ExtensionsToJsonRpcRequest
    {
        public static JsonRpcRequest WithParameter(this JsonRpcRequest request, string key, object value)
        {
            request.Params.Add(key, value);
            return request;
        }

        public static void AddFilter(this JsonRpcRequest request, string key, string value)
        {
            if (!request.Params.ContainsKey("filter"))
                request.Params["filter"] = new Dictionary<string, string>();

            var filters = request.Params["filter"] as Dictionary<string, string>;
            filters.Add("filter", value);
        }
    }
}
