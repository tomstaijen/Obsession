using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FTD2XX_NET;

namespace Obsession.Rfxcom
{
    public class RfDevice
    {
        public string SerialNumber { get; set; }
    }


    public class RfException : Exception
    {
        public RfException(string message) : base(message)
        {
        }

    }

    public class RF
    {
        public byte[] Read(Action<byte[]> channel, byte[] sendBytes = null) 
        {
            UInt32 ftdiDeviceCount = 0;
            FTDI.FT_STATUS ftStatus = FTDI.FT_STATUS.FT_OK;
            FTDI myFtdiDevice = new FTDI();

            ftStatus = myFtdiDevice.GetNumberOfDevices(ref ftdiDeviceCount);

            FTDI.FT_DEVICE_INFO_NODE[] ftdiDeviceList = new FTDI.FT_DEVICE_INFO_NODE[ftdiDeviceCount];

            // Populate our device list
            ftStatus = myFtdiDevice.GetDeviceList(ftdiDeviceList);

        // Open first device in our list by serial number
            ftStatus = myFtdiDevice.OpenBySerialNumber(ftdiDeviceList[0].SerialNumber);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new RfException("Failed to open device (error " + ftStatus.ToString() + ")");
            }

            // Set up device data parameters
            // Set Baud rate to 9600
            ftStatus = myFtdiDevice.SetBaudRate(38400);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new RfException("Failed to set Baud rate (error " + ftStatus.ToString() + ")");
            }

            // Set data characteristics - Data bits, Stop bits, Parity
            ftStatus = myFtdiDevice.SetDataCharacteristics(FTDI.FT_DATA_BITS.FT_BITS_8, FTDI.FT_STOP_BITS.FT_STOP_BITS_1, FTDI.FT_PARITY.FT_PARITY_NONE);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new RfException("Failed to set data characteristics (error " + ftStatus.ToString() + ")");
            }

            // Set flow control - set RTS/CTS flow control
            ftStatus = myFtdiDevice.SetFlowControl(FTDI.FT_FLOW_CONTROL.FT_FLOW_RTS_CTS, 0x11, 0x13);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new RfException("Failed to set flow control (error " + ftStatus.ToString() + ")");
            }

            // Set read timeout to 5 seconds, write timeout to infinite
            ftStatus = myFtdiDevice.SetTimeouts(5000, 0);
            if (ftStatus != FTDI.FT_STATUS.FT_OK)
            {
                // Wait for a key press
                throw new RfException("Failed to set timeouts (error " + ftStatus.ToString() + ")");
            }

//            // Perform loop back - make sure loop back connector is fitted to the device
//            // Write string data to the device
//            string dataToWrite = "Hello world!";
//            UInt32 numBytesWritten = 0;
//            // Note that the Write method is overloaded, so can write string or byte array data
//            ftStatus = myFtdiDevice.Write(dataToWrite, dataToWrite.Length, ref numBytesWritten);
//            if (ftStatus != FTDI.FT_STATUS.FT_OK)
//            {
//                // Wait for a key press
//                Console.WriteLine("Failed to write to device (error " + ftStatus.ToString() + ")");
//                Console.ReadKey();
//                return;
//            }

            if (sendBytes != null)
            {
                UInt32 numBytesWritten = 0;
                ftStatus = myFtdiDevice.Write(sendBytes, sendBytes.Length, ref numBytesWritten);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Wait for a key press
                    Console.WriteLine("Failed to write to device (error " + ftStatus.ToString() + ")");
                }

                myFtdiDevice.Close();
                return null;
            }

            while (true)
            {
                // Check the amount of data available to read
                // In this case we know how much data we are expecting, 
                // so wait until we have all of the bytes we have sent.
                UInt32 numBytesAvailable = 0;
                do
                {
                    ftStatus = myFtdiDevice.GetRxBytesAvailable(ref numBytesAvailable);
                    if (ftStatus != FTDI.FT_STATUS.FT_OK)
                    {
                        // Wait for a key press
                        throw new RfException("Failed to get number of bytes available to read (error " + ftStatus.ToString() + ")");
                    }
                    Thread.Sleep(10);
                } while (numBytesAvailable == 0);

                // Now that we have the amount of data we want available, read it
                var bytes = new byte[numBytesAvailable];
                UInt32 numBytesRead = 0;
                // Note that the Read method is overloaded, so can read string or byte array data
                ftStatus = myFtdiDevice.Read(bytes, numBytesAvailable, ref numBytesRead);
                if (ftStatus != FTDI.FT_STATUS.FT_OK)
                {
                    // Wait for a key press
                    throw new RfException("Failed to read data (error " + ftStatus.ToString() + ")");
                }

                myFtdiDevice.GetRxBytesAvailable(ref numBytesAvailable);

                

                channel(bytes);

                myFtdiDevice.Close();
            }
            ftStatus = myFtdiDevice.Close();
        }

    }
}
