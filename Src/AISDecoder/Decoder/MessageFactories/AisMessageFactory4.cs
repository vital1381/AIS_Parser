using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory4 : IAisMessageFactory
    {
        private const byte c_type1 = 4;

        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type1)
            {
                throw new Exception($"Incorrect type, expected 4 but present:{type}");
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            uint year = payload.DecodeUInt32(38, 51);
            byte month = payload.DecodeByte(52, 55);
            byte day = payload.DecodeByte(56, 60);
            byte hour = payload.DecodeByte(61, 65);
            byte minute = payload.DecodeByte(66, 71);
            byte second = payload.DecodeByte(72, 77);
            bool posAccuracy = payload.DecodeBool(78);
            float longtitude = (float)payload.DecodeFloat(79, 106) / 600000;
            float latitude = (float)payload.DecodeFloat(107, 133) / 600000;
            EpfdFixType positionFixType = (EpfdFixType)payload.DecodeByte(134, 137);
            bool raim = payload.DecodeBool(148);
            uint radioStatus = payload.DecodeUInt32(149, 167);

                       
           

            return new BaseStationReportMessage(
                type,
                timeStamp,
                repeatIndicator,
                mmsi,
                year,
                month,
                day,
                hour,
                minute,
                second,
                posAccuracy,
                longtitude,
                latitude,
                positionFixType,
                raim,
                radioStatus);
        }
    }
}
