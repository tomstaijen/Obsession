using System.Threading.Tasks;

namespace P1Reader
{
    public static class P1Definition
    {
        public static string Timestamp = "0-0:1.0.0";
        public static string PowerMeter1 = "1-0:1.8.1";
        public static string PowerMeter2 = "1-0:1.8.2";
        public static string PowerDelivery = "1-0:1.7.0";
        public static string PowerTariff = "0-0:96.14.0";

        /// <summary>
        /// "0-1:24.2.1(150309140000W)(01122.585*m3)\r\n"
        /// </summary>
        public static string GassMeter = "0-1:24.2.1";

    }
}
