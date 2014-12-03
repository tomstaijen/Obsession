using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace InComfort
{
    public class InComfortReader
    {
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
    }


    /// <summary>
    /// 
    /// </summary>
    public class RawInComfortData
    {
        [JsonProperty("room_temp_1_lsb")]
        public short RoomTemp1Lsb { get; set; }
        [JsonProperty("room_temp_1_msb")]
        public short RoomTemp1Msb { get; set; }

        [JsonProperty("room_temp_set_1_lsb")]
        public short RoomTempSet1Lsb { get; set; }
        
        [JsonProperty("room_temp_set_1_msb")]
        public short RoomTempSet1Msb { get; set; }

        [JsonProperty("room_set_ovr_1_lsb")]
        public short RoomSetOvr1Lsb { get; set; }

        [JsonProperty("room_set_ovr_1_msb")]
        public short RoomSetOvr1Msb { get; set; }

        [JsonProperty("ch_temp_msb")]
        public short HeaterTempMsb { get; set; }

        [JsonProperty("ch_temp_lsb")]
        public short HeaterTempLsb { get; set; }

        [JsonProperty("tap_temp_msb")]
        public short TapTempMsb { get; set; }

        [JsonProperty("tap_temp_lsb")]
        public short TapTempLsb { get; set; }

        [JsonProperty("ch_pressure_msb")]
        public short PressureMsb { get; set; }

        [JsonProperty("ch_pressure_lsb")]
        public short PressureLsb { get; set; }

        [JsonProperty("IO")]
        public byte IO { get; set; }

        [JsonProperty("displ_code")]
        public short DisplayCode { get; set; }
    }

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
            RoomTemp1 = FromBytes(raw.RoomTemp1Msb, raw.RoomTemp1Msb);
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
