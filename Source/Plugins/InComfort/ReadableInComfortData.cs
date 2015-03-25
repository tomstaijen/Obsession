using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InComfort
{
    public class ReadableInComfortData
    {
        public static IDictionary<short, string> DISPLAY_CODES = new Dictionary<short, string>
            {
                {85, "sensortest"},
                {170, "service"},
                {204, "tapwater"},
                {51, "tapwater int."},
                {240, "boiler int."},
                {15, "boiler ext."},
                {153, "postrun boiler"},
                {102, "central heating"},
                {0, "opentherm"},
                {255, "buffer"},
                {24, "frost"},
                {231, "pustrun ch"},
                {126, "standby"},
                {37, "central heating rf"}
            };

        public ReadableInComfortData(RawInComfortData raw)
        {
            RoomTemp1 = FromBytes(raw.RoomTemp1Msb, raw.RoomTemp1Lsb);
            RoomSetpoint1 = FromBytes(raw.RoomTempSet1Msb, raw.RoomTempSet1Lsb);
            RoomSetpointOverride1 = FromBytes(raw.RoomSetOvr1Msb, raw.RoomSetOvr1Lsb);

            HeaterTemp = FromBytes(raw.HeaterTempMsb, raw.HeaterTempLsb);
            TapWaterTemp = FromBytes(raw.TapTempMsb, raw.TapTempLsb);

            Pressure = FromBytes(raw.PressureMsb, raw.PressureLsb);
            Lockout = (raw.IO & 1) == 1;
            Pump = (raw.IO & 2) == 1;
            TapFunction = (raw.IO & 4) == 1;
            Burner = (raw.IO & 8) == 1;

            if (DISPLAY_CODES.ContainsKey(raw.DisplayCode))
                DisplayCode = DISPLAY_CODES[raw.DisplayCode];
        }

        private decimal FromBytes(short msb, short lsb)
        {
            return (Convert.ToDecimal(msb)*256 + Convert.ToDecimal(lsb))/100;
        }

        public decimal RoomTemp1 { get; set; }
        public decimal RoomSetpoint1 { get; set; }
        public decimal RoomSetpointOverride1 { get; set; }

        public decimal HeaterTemp { get; set; }
        public decimal TapWaterTemp { get; set; }
        public decimal Pressure { get; set; }

        public bool Burner { get; set; }
        public bool Pump { get; set; }
        public bool Lockout { get; set; }
        public bool TapFunction { get; set; }

        public string DisplayCode { get; set; }

    }
}
