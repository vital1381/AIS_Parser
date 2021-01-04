using System;
using System.Collections;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder.MessageFactories
{
    internal class AisMessageFactory18 : IAisMessageFactory
    {
        private const byte c_type = 18;

        public AisMessage CreateMessage(BitArray payload)
        {
            var type = payload.DecodeByte(0, 5);

            if (type != c_type)
            {
                throw new Exception(string.Format("Incorrect type, expected 18 but present:{0}",type));
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
            CsUnitType csUnit = payload.DecodeBool(141) ? CsUnitType.ClassBCarrierSense : CsUnitType.ClassBSotdma;
            bool display = payload.DecodeBool(142);
            bool dsc = payload.DecodeBool(143);
            bool band = payload.DecodeBool(144);
            bool message22 = payload.DecodeBool(145);
            bool assigned = payload.DecodeBool(146);
            bool raim = payload.DecodeBool(147);
            uint radioStatus = payload.DecodeUInt32(148, 167);

            return new ClassBPositionReportMessage(
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
                raim,
                radioStatus,
                csUnit,
                display,
                dsc,
                band,
                message22,
                assigned);
        }
    }
}
