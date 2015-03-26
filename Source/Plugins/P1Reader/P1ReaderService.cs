using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Obsession.Core;

namespace P1Reader
{
    public class P1ServiceServiceModule : IServiceModule
    {
        private readonly Configuration _configuration;
        private readonly P1ReaderService _service;

        public P1ServiceServiceModule(Configuration configuration)
        {
            _configuration = configuration;
            _service = new P1ReaderService();
        }


        public TimeSpan GetInterval()
        {
            return TimeSpan.FromSeconds(0);
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return null;
        }

        public StateValues GetState()
        {
            var host = _configuration.Values["Hostname"] as string;
            if (host == null)
                throw new Exception("Broken config");
            var p1 = _service.Read(host);

            if (p1 == null)
                return null;

            return new StateValues(_configuration.ModuleName, _configuration.ObjectName, p1);
        }

        /// <summary>
        /// State is pushed periodically, so anything existing is good.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool IsActual(StateValues current)
        {
            return true;
        }
    }

    public class P1ReaderService : IDisposable
    {
        public string Host { get; set; }

        private bool _continue = true;
        private TcpClient _client;


        public P1Envelope Read(string hostname)
        {
            _client = new TcpClient();
            _client.Connect(hostname, 8899);

            var reader = new BinaryReader(_client.GetStream());
            var writer = new BinaryWriter(_client.GetStream());

            writer.Write("\x00");

            return reader.ReadP1();
        }

        public void Dispose()
        {
            if (_client != null && _client.Connected)
            {
                _client.Close();
                _client = null;
            }
        }
    }
}