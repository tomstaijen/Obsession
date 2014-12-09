using System;
using System.IO;
using System.Net.Sockets;
using Core;
using Nest;

namespace P1Reader
{
    public class P1ReaderService : IService
    {
        public string Host { get; set; }

        private bool _continue = true;
        private TcpClient _client;


        public void Start()
        {
            while (_continue)
            {
                try
                {
                    _client = new TcpClient();
                    _client.Connect("192.168.3.29", 8899);

                    var reader = new BinaryReader(_client.GetStream());
                    var writer = new BinaryWriter(_client.GetStream());

                    writer.Write("\x00");

                    var node = new Uri("http://localhost:9200");

                    var settings = new ConnectionSettings(
                        node,
                        defaultIndex: "obsession"
                        );

                    var esClient = new ElasticClient(settings);

                    while (_continue)
                    {
                        reader.ReadP1(p1 => esClient.Index(new P1Message { Timestamp = p1.Timestamp, Data = p1 }));
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        public void Stop()
        {
            _continue = false;
            _client.Close();
        }
    }
}