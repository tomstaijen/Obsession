using Newtonsoft.Json;

namespace InComfort
{
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
}