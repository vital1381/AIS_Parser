using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 24 Part B.
    /// </summary>
    [DataContract]
    public class ClassBStaticAndVoyageDataMesageTypeB : AisMessage
    {

        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public byte PartNumber { get; private set; }

        [DataMember]
        public ShipType ShipType { get; private set; }

        [DataMember]
        public string VendorId { get; private set; }

        [DataMember]
        public byte UnitModelCode { get; private set; }

        [DataMember]
        public uint SerialNumber { get; private set; }

        [DataMember]
        public string Callsign { get; private set; }

        [DataMember]
        public uint Bow { get; private set; }

        [DataMember]
        public uint Stern { get; private set; }

        [DataMember]
        public byte Port { get; private set; }

        [DataMember]
        public byte Starboard { get; private set; }

        [DataMember]
        public uint MotherShipMmsi { get; private set; }

        public ClassBStaticAndVoyageDataMesageTypeB(byte type, long timeStamp, byte repeatIndicator, uint mmsi, byte partNumber, ShipType shipType, string vendorId, byte unitModelCode, uint serialNumber, string callsign, uint bow, uint stern, byte port, byte starboard, uint motherShipMmsi) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            PartNumber = partNumber;
            ShipType = shipType;
            VendorId = vendorId;
            UnitModelCode = unitModelCode;
            SerialNumber = serialNumber;
            Callsign = callsign;
            Bow = bow;
            Stern = stern;
            Port = port;
            Starboard = starboard;
            MotherShipMmsi = motherShipMmsi;
        }
    }
}
