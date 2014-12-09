﻿using System;
using System.Globalization;
using System.Linq;

namespace P1Reader
{
    public static class ExtensionsToP1Envelope
    {


        public static string RemoveUnit(this string value, string unit)
        {
            return value.Replace("*" + unit, "");
        }

        public static P1Envelope SetTimestamp(this P1Envelope envelope, string timestampValue)
        {
            var dst = timestampValue.Last();
            timestampValue = timestampValue.Substring(0, timestampValue.Length - 1);
            var dt = DateTime.ParseExact(timestampValue, "yyMMddHHmmss", null, DateTimeStyles.None);
            envelope.Timestamp = dt;
            envelope.Dst = (dst == 'S');

            return envelope;
        }

        public static P1Envelope SetPowerMeter1(this P1Envelope envelope, string meterValue)
        {
            var unitless = meterValue.RemoveUnit("kWh");
            envelope.PowerMeter1 = float.Parse(unitless, CultureInfo.InvariantCulture);
            return envelope;
        }

        public static P1Envelope SetPowerMeter2(this P1Envelope envelope, string meterValue)
        {
            envelope.PowerMeter2 = float.Parse(meterValue.RemoveUnit("kWh"), CultureInfo.InvariantCulture);
            return envelope;
        }

        public static P1Envelope SetPowerDelivery(this P1Envelope envelope, string meterValue)
        {
            envelope.PowerDelivery = float.Parse(meterValue.RemoveUnit("kW"), CultureInfo.InvariantCulture);
            return envelope;
        }

        public static P1Envelope SetTariff(this P1Envelope envelope, string value)
        {
            envelope.Tariff = Convert.ToByte(value);
            return envelope;
        }

        public static string[] GetValuesForReference(this string[] lines, string reference)
        {
            return lines.Single(s => s.StartsWith(reference)).GetValues();
        }

        public static P1Envelope AsP1(this string[] lines)
        {
            var result = new P1Envelope();

            result.SetTimestamp(lines.GetValuesForReference(P1Definition.Timestamp).First());
            result.SetPowerMeter1(lines.GetValuesForReference(P1Definition.PowerMeter1).First());
            result.SetPowerMeter2(lines.GetValuesForReference(P1Definition.PowerMeter2).First());
            result.SetPowerDelivery(lines.GetValuesForReference(P1Definition.PowerDelivery).First());
            result.SetTariff(lines.GetValuesForReference(P1Definition.PowerTariff).First());


            return result;
        }

        public static string[] GetValues(this string line)
        {
            line = line.Substring(line.IndexOf('(') + 1);
            var subs = line.Split('(');
            subs = subs.Select(v => v.Split(')')[0]).ToArray();
            return subs;
        }

        public static long AsLong(this string value)
        {
            return long.Parse(value);
        }
    }
}