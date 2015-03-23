using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using Core;
using Nest;
using Obsession.Core;

namespace P1Reader
{
    public class P1ServiceServiceModule : IServiceModule
    {
        private readonly P1ReaderService _service;

        public P1ServiceServiceModule(P1ReaderService service)
        {
            _service = service;
        }

        public IModuleInstance GetInstance(Configuration configuration)
        {
            return new P1ServiceModuleInstance(configuration, _service);
        }

        public TimeSpan GetInterval(Configuration configuration)
        {
            return TimeSpan.FromSeconds(0);
        }

        public StateValues GetState(Configuration configuration)
        {
            return GetInstance(configuration).GetState();
        }

        /// <summary>
        /// State is pushed periodically, so anything existing is good.
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public bool IsActual(Configuration configuration, StateValues current)
        {
            return true;
        }
    }

    public class P1ServiceModuleInstance : IModuleInstance
    {
        private readonly Configuration _configuration;
        private readonly P1ReaderService _service;

        public P1ServiceModuleInstance(Configuration configuration, P1ReaderService service)
        {
            _configuration = configuration;
            _service = service;
        }

        public StateValues GetState()
        {
            var host = _configuration.Values["Hostname"] as string;
            if( host == null )
                throw new Exception("Broken config");
            var p1 = _service.Read(host);

            if (p1 == null)
                return null;

            return new StateValues(_configuration.ModuleName, _configuration.ObjectName, p1);
        }

        public IDictionary<string, Delegate> GetActions()
        {
            return null;
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