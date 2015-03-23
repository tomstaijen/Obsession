using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace P1Reader
{
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

        public static P1Envelope ReadP1(this BinaryReader reader) 
        {
            string line;
            var lines = new List<string>();
            string message = "";
            do
            {
                line = ReadLine(reader);
                lines.Add(line);
            } while (!IsFooter(line));

            var a = lines.ToArray();

            if (a.IsValid())
            {
                return a.AsP1();
            }
            else
            {
                Console.WriteLine("Invalid P1 message");
                return null;
            }
        }

        public static bool ReadP1(this BinaryReader reader, Action<P1Envelope> callback)
        {
            var p1 = ReadP1(reader);

            callback(p1);

            return true;
        }
    }
}