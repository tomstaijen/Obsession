using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;

namespace P1Reader
{
    public static class P1Definition
    {
        public static string Timestamp = "0-0:1.0.0";
        public static string PowerMeter1 = "1-0:1.8.1";
        public static string PowerMeter2 = "1-0:1.8.2";
        public static string PowerDelivery = "1-0:1.7.0";
        public static string PowerTariff = "0-0:96.14.0";
    }

     public class P1Envelope
        {
            /// <summary>
            /// Daylight saving time
            /// </summary>
            public bool Dst { get; set; }   
            
            public DateTime Timestamp { get; set; }

            /// <summary>
            /// kWh
            /// </summary>
            public float PowerMeter1 { get; set; }

            /// <summary>
            /// kWh
            /// </summary>
            public float PowerMeter2 { get; set; }
            
            /// <summary>
            /// 1/2
            /// </summary>
            public byte Tariff { get; set; }

            /// <summary>
            /// kW
            /// </summary>
            public float PowerDelivery { get; set; }

        }

        public static class ExtensionsToStream
        {
            private static string ReadLine(BinaryReader reader)
            {
                byte[] bytes;
                string result = "";
                string str = null;
                do
                {
                    bytes = reader.ReadBytes(1);
                    str = Encoding.UTF8.GetString(bytes);
                    result += str;
                } while (str != "\r");

                bytes = reader.ReadBytes(1);
                str = Encoding.UTF8.GetString(bytes);
                result += str;

                return result;
            }

            public static bool IsFooter(string line)
            {
                return line[0] == '!';
            }

            public static bool ReadP1(this BinaryReader reader, Action<P1Envelope> callback)
            {
                string line;
                var lines = new List<string>();
                string message = "";
                do
                {
                    line = ReadLine(reader);
                    lines.Add(line);
                } while (!IsFooter(line));

                var p1 = lines.ToArray().AsP1();

                callback(p1);

                return true;
            }
        }


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

        public static class ExtensionsToP1Envelope
        {


            public static string RemoveUnit(this string value, string unit)
            {
                return value.Replace("*" + unit, "");
            }

            public static P1Envelope SetTimestamp(this P1Envelope envelope, string timestampValue)
            {
                var dst = timestampValue.Last();
                timestampValue = timestampValue.Substring(0, timestampValue.Length - 1);
                var dt = DateTime.ParseExact(timestampValue, "yyMMddHHmmss", null, DateTimeStyles.None);
                envelope.Timestamp = dt;
                envelope.Dst = (dst == 'S');

                return envelope;
            }

            public static P1Envelope SetPowerMeter1(this P1Envelope envelope, string meterValue)
            {
                var unitless = meterValue.RemoveUnit("kWh");
                envelope.PowerMeter1 = float.Parse(unitless, CultureInfo.InvariantCulture);
                return envelope;
            }

            public static P1Envelope SetPowerMeter2(this P1Envelope envelope, string meterValue)
            {
                envelope.PowerMeter2 = float.Parse(meterValue.RemoveUnit("kWh"), CultureInfo.InvariantCulture);
                return envelope;
            }

            public static P1Envelope SetPowerDelivery(this P1Envelope envelope, string meterValue)
            {
                envelope.PowerDelivery = float.Parse(meterValue.RemoveUnit("kW"), CultureInfo.InvariantCulture);
                return envelope;
            }

            public static P1Envelope SetTariff(this P1Envelope envelope, string value)
            {
                envelope.Tariff = Convert.ToByte(value);
                return envelope;
            }

            public static string[] GetValuesForReference(this string[] lines, string reference)
            {
                return lines.Single(s => s.StartsWith(reference)).GetValues();
            }

            public static P1Envelope AsP1(this string[] lines)
            {
                var result = new P1Envelope();

                result.SetTimestamp(lines.GetValuesForReference(P1Definition.Timestamp).First());
                result.SetPowerMeter1(lines.GetValuesForReference(P1Definition.PowerMeter1).First());
                result.SetPowerMeter2(lines.GetValuesForReference(P1Definition.PowerMeter2).First());
                result.SetPowerDelivery(lines.GetValuesForReference(P1Definition.PowerDelivery).First());
                result.SetTariff(lines.GetValuesForReference(P1Definition.PowerTariff).First());


                return result;
            }

            public static string[] GetValues(this string line)
            {
                line = line.Substring(line.IndexOf('(')+1);
                var subs = line.Split('(');
                subs = subs.Select(v => v.Split(')')[0]).ToArray();
                return subs;
            }

            public static long AsLong(this string value)
            {
                return long.Parse(value);
            }
        }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    var client = new TcpClient();
                    client.Connect("192.168.3.29", 8899);

                    var reader = new BinaryReader(client.GetStream());
                    var writer = new BinaryWriter(client.GetStream());

                    writer.Write("\x00");

                    var node = new Uri("http://localhost:9200");

                    var settings = new ConnectionSettings(
                        node,
                        defaultIndex: "obsession"
                    );

                    var esClient = new ElasticClient(settings);

                    while (reader.ReadP1(p1 => esClient.Index(new P1Message
                    {
                        Timestamp = p1.Timestamp,
                        Data = p1
                    })))
                    {
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
