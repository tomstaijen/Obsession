using System;

namespace P1Reader
{
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

        /// <summary>
        /// m3
        /// </summary>
        public float Gass { get; set; }
    }
}