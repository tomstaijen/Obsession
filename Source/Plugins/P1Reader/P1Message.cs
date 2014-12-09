using System;
using Newtonsoft.Json;

namespace P1Reader
{
    public class P1Message
    {
        [JsonIgnore]
        public DateTime Timestamp { get; set; }

        [JsonProperty("@timestamp")]
        public string TimestampISO
        {
            get
            {
                return Timestamp.ToUniversalTime().ToString("o");
            }
        }

        [JsonProperty("data")]
        public P1Envelope Data { get; set; }
    }
}