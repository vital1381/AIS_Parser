using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory123 : IAisMessageFactory
    {
        private const byte c_type1 = 1;
        private const byte c_type2 = 2;
        private const byte c_type3 = 3;

        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type1 && type != c_type2 && type != c_type3)
            {
                throw new Exception(string.Format("Incorrect type, expected 1,2,3 but present:{0}",type));
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            NavigationStatus navStatus = (NavigationStatus)payload.DecodeByte(38, 41);
            int rot = payload.DecodeInt32(42, 49);
            float sog = (float)payload.DecodeUInt32(50, 59) / 10;
            bool posAccuracy = payload.DecodeBool(60);
            float longtitude = (float)payload.DecodeFloat(61, 88) / 600000;
            float latitude = (float)payload.DecodeFloat(89, 115) / 600000;
            float cog = (float)payload.DecodeUInt32(116, 127) / 10;
            ushort heading = payload.DecodeUShort(128, 136);
            byte utcSeconds = payload.DecodeByte(137, 142);
            ManeuverIndicator manuevrIndicator = (ManeuverIndicator)payload.DecodeByte(143, 144);
            byte spare = payload.DecodeByte(145, 147);
            bool raim = payload.DecodeBool(148);
            uint radioStatus = payload.DecodeUInt32(149, 167);

            return new ClassAPositionReportMessage(
                type,
                timeStamp,
                repeatIndicator,
                mmsi,
                navStatus, 
                rot, 
                sog,
                posAccuracy,
                longtitude, 
                latitude, 
                cog,
                heading,
                utcSeconds, 
                manuevrIndicator, 
                spare, 
                raim,
                radioStatus);

        }
    }
}
