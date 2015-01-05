﻿using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using Core;
using Nest;
using Newtonsoft.Json;

namespace InComfort
{
    public class InComfortReaderService : IService
    {
        private bool _continue = true;

        public ReadableInComfortData Read()
        {
            var client = new HttpClient();
            var response = client.GetAsync("http://192.168.3.55/data.json?heater=0");

            if (response.Result.StatusCode == HttpStatusCode.OK)
            {
                var data = response.Result.Content.ReadAsStringAsync();
                var raw = JsonConvert.DeserializeObject<RawInComfortData>(data.Result);

                
                return new ReadableInComfortData(raw);
            }
            return null;
        }

        public void Start()
        {
            while (_continue)
            {
                try
                {
                    var node = new Uri("http://localhost:9200");

                    var settings = new ConnectionSettings(
                        node,
                        defaultIndex: "obsession"
                        );

                    var esClient = new ElasticClient(settings);

                    while (_continue)
                    {
                        var data = Read();

                        esClient.Index(data);

                        // TODO do this with a stopwatch? or move the loop to some ServiceRunner?
                        Thread.Sleep(10000);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public void Stop()
        {
            _continue = false;
        }
    }
}