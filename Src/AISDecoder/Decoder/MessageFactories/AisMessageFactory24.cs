using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory24 : IAisMessageFactory
    {
        private const byte c_type = 24;
        
        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type)
            {
                throw new Exception(string.Format("Incorrect type, expected 24 but present:{0}", type));
            }

            var timeStamp = DateTime.UtcNow.ToFileTimeUtc();
            byte repeatIndicator = payload.DecodeByte(6, 7);
            uint mmsi = payload.DecodeUInt32(8, 37);
            byte partNumber = payload.DecodeByte(38, 39);

            if (partNumber == 0)
            {
                string vesselName = payload.DecodeString(40, 159).Trim();    
                return new ClassBStaticAndVoyageDataMesageTypeA(
                    type,
                    timeStamp, 
                    repeatIndicator,
                    mmsi,
                    partNumber, 
                    vesselName);
            }

            if (partNumber != 1)
            {
                throw new Exception(string.Format("Incorrect part number, only 0 and 1 allowed, but present:{0}", partNumber));
            }

            //TypeB
            ShipType shipType = (ShipType)payload.DecodeByte(40, 47);
            string vendorId = payload.DecodeString(48, 65).Trim();
            byte unitModel = payload.DecodeByte(66, 69);
            uint serial = payload.DecodeUInt32(70, 89);
            string callsign = payload.DecodeString(90, 131).Trim();
            uint bow = payload.DecodeUInt32(132, 140);
            uint stern = payload.DecodeUInt32(141, 149);
            byte port = payload.DecodeByte(150, 155);
            byte starboard = payload.DecodeByte(156, 161);
            uint motherShipMmmsi = payload.DecodeUInt32(132, 161);

            return new ClassBStaticAndVoyageDataMesageTypeB(
                type, 
                timeStamp, 
                repeatIndicator,
                mmsi, 
                partNumber,
                shipType,
                vendorId,
                unitModel,
                serial,
                callsign,
                bow,
                stern,
                port,
                starboard,
                motherShipMmmsi);
        }
    }
}
