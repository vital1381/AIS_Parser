using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory27 : IAisMessageFactory
    {
        private const byte c_type1 = 27;

        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type1)
            {
                throw new Exception(string.Format("Incorrect type, expected 27 but present:{0}",type));
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            bool posAccuracy = payload.DecodeBool(38);
            bool raim = payload.DecodeBool(39);
            NavigationStatus navStatus = (NavigationStatus)payload.DecodeByte(40, 43);
            float longtitude = (float)payload.DecodeFloat(44, 61) / 600000;
            float latitude = (float)payload.DecodeFloat(62, 78) / 600000;
            float sog = (float)payload.DecodeUInt32(79, 84) / 10;
            float cog = (float)payload.DecodeUInt32(85, 93) / 10;
            bool gnss = payload.DecodeBool(94);
            bool spare = payload.DecodeBool(95);
            


            return new ClassAPositionReportLongRangeMessage(
                type,
                timeStamp,
                repeatIndicator,
                mmsi,
                posAccuracy,
                raim,
                navStatus,
                longtitude,
                latitude,
                sog,
                cog,
                gnss,
                spare);
        }
    }
}
