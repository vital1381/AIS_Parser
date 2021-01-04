using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory19 : IAisMessageFactory
    {
        private const byte c_type = 19;

        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type)
            {
                throw new Exception(string.Format("Incorrect type, expected 19 but present:{0}",type));
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            float sog = (float)payload.DecodeUInt32(46, 55) / 10;
            bool posAccuracy = payload.DecodeBool(56);
            float longtitude = (float)payload.DecodeFloat(57, 84) / 600000;
            float latitude = (float)payload.DecodeFloat(85, 111) / 600000;
            float cog = (float)payload.DecodeUInt32(112, 123) / 10;
            ushort heading = payload.DecodeUShort(124, 132);
            byte utcSeconds = payload.DecodeByte(133, 138);
            string vesselName = payload.DecodeString(143, 262).Trim();
            ShipType shipType = (ShipType)payload.DecodeByte(263, 270);
            uint bow = payload.DecodeUInt32(271, 279);
            uint stern = payload.DecodeUInt32(280, 288);
            byte port = payload.DecodeByte(289, 294);
            byte starboard = payload.DecodeByte(295, 300);
            EpfdFixType positionFixType = (EpfdFixType)payload.DecodeByte(301, 304);
            bool raim = payload.DecodeBool(305);
            bool dataTerminalReady = payload.DecodeBool(306);
            bool assigned = payload.DecodeBool(307);
            byte spare = payload.DecodeByte(308, 311);

            return new ClassBExtendedPositionReportMessage(
                type,
                timeStamp,
                repeatIndicator,
                mmsi,
                sog,
                posAccuracy,
                longtitude, 
                latitude, 
                cog,
                heading,
                utcSeconds,
                vesselName,
                shipType,
                bow,
                stern,
                port,
                starboard,
                positionFixType,
                raim,
                dataTerminalReady,
                assigned,
                spare);
        }
    }
}
