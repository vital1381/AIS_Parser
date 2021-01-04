using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory5 : IAisMessageFactory
    {
        private const byte c_type = 5;
        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type)
            {
                throw new Exception(string.Format("Incorrect type, expected 5 but present:{0}", type));
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            byte aisVersion = payload.DecodeByte(38, 39);
            uint imo = payload.DecodeUInt32(40, 69); ;
            string callsign = payload.DecodeString(70, 111).Trim();
            string vesselName = payload.DecodeString(112, 231).Trim();
            ShipType shipType = (ShipType)payload.DecodeByte(232, 239);
            uint bow = payload.DecodeUInt32(240, 248);
            uint stern = payload.DecodeUInt32(249, 257);
            byte port = payload.DecodeByte(258, 263);
            byte starboard = payload.DecodeByte(264, 269); 
            EpfdFixType positionFixType = (EpfdFixType)payload.DecodeByte(270, 273);
            byte etaMonth = payload.DecodeByte(274, 277);
            byte etaDay = payload.DecodeByte(278, 282);
            byte etaHour = payload.DecodeByte(283, 287);
            byte etaMinute = payload.DecodeByte(288, 293);
            float draught = (float)payload.DecodeUInt32(294, 301) / 10;
            string destination = payload.DecodeString(302, 421).Trim();
            bool dataTerminalReady = payload.DecodeBool(422);
            bool spare = payload.DecodeBool(423);

            return new ClassAStaticAndVoyageDataMesage(
                type,
                timeStamp,
                repeatIndicator,
                mmsi,
                aisVersion,
                imo,
                callsign,
                vesselName,
                shipType,
                bow,
                stern,
                port,
                starboard,
                positionFixType,
                etaMonth,
                etaDay,
                etaHour,
                etaMinute,
                draught,
                destination,
                dataTerminalReady,
                spare);
        }
    }
}
