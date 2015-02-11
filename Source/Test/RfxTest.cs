using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Obsession.Rfxcom;

namespace Test
{
    [TestFixture]
    public class RfxTest
    {
        [Test]
        public void SerialTest()
        {
            var sp = new SerialPort("COM5", 38400, Parity.None, 8, StopBits.One);
            sp.ReadTimeout = 0;
            sp.WriteTimeout = 1000;
            sp.Open();
            // send reset
            //sp.Write(new byte[] { 0x07, 0x0A, 0x02, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 }, 0, 14);
            while (true)
            {
                Console.WriteLine("");
                List<byte> bytes = new List<byte>();
                if (sp.BytesToRead > 0)
                {
                    while (sp.BytesToRead > 0)
                    {
                        bytes.Add((byte)sp.ReadByte());
                    }
                }
                Console.WriteLine(BitConverter.ToString(bytes.ToArray()));
            }

            //Thread.Sleep(1000);

            sp.Write(new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'I', 0x05, 0x01, 0x00 }, 0, 8);

            Console.ReadLine();
        }

        public void DataReceived(object sender, SerialDataReceivedEventArgs args)
        {
            
        }


        public void Print(byte[] bytes)
        {
//            Console.WriteLine(string.Concat(bytes.Select(b => b.ToString("X2")).ToArray()));

            for (int i = 0; i < bytes.Length; i++ )
                Console.Write(Convert.ToString(bytes[i], 16) + ": " + Convert.ToString(bytes[i], 2) + " ");

            Console.WriteLine("");
        }

        [Test]
        public void Test()
        {
            Print(new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'I', 0x05, 0x01, 0x00 });
            new RF().Read(Print, new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'I', 0x05, 0x01, 0x00 });
            Print(new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'I', 0x05, 0x00, 0x00 });
            new RF().Read(Print, new byte[] {0x07, 0x10, 0x02, 0x10, (byte)'I', 0x05, 0x00, 0x00});
            //   new RF().Read(Print);

            bool on = true;

            while (true)
            {
                new RF().Read(Print, new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'P', 13, (byte) (on? 0x01 : 0x00), 0x00 });
                new RF().Read(Print, new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'P', 10, (byte)(on ? 0x01 : 0x00), 0x00 });
                new RF().Read(Print, new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'P', 11, (byte)(on ? 0x01 : 0x00), 0x00 });
                new RF().Read(Print, new byte[] { 0x07, 0x10, 0x02, 0x10, (byte)'P', 25, (byte)(on ? 0x01 : 0x00), 0x00 });
                
                // toggle
                on = !on;
                Thread.Sleep(2000);
            }
        }
    }
}
