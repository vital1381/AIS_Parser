using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 24 Part A.
    /// </summary>
    [DataContract]
    public class ClassBStaticAndVoyageDataMesageTypeA : AisMessage
    {
        public ClassBStaticAndVoyageDataMesageTypeA(byte type, long timeStamp, byte repeatIndicator, uint mmsi, byte partNumber, string vesselName) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            PartNumber = partNumber;
            VesselName = vesselName;
        }

        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public byte PartNumber { get; private set; }

        [DataMember]
        public string VesselName { get; private set; }
    }
}
