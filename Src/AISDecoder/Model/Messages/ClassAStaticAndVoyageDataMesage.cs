using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 5.
    /// </summary>
    [DataContract]
    public class ClassAStaticAndVoyageDataMesage : AisMessage
    {
        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public byte AisVersion { get; private set; }

        [DataMember]
        public uint Imo { get; private set; }

        [DataMember]
        public string Callsign { get; private set; }

        [DataMember]
        public string VesselName { get; private set; }

        [DataMember]
        public ShipType ShipType { get; private set; }

        [DataMember]
        public uint Bow { get; private set; }

        [DataMember]
        public uint Stern { get; private set; }

        [DataMember]
        public byte Port { get; private set; }

        [DataMember]
        public byte Starboard { get; private set; }

        [DataMember]
        public EpfdFixType PositionFixType { get; private set; }

        [DataMember]
        public byte EtaMonth { get; private set; }

        [DataMember]
        public byte EtaDay { get; private set; }

        [DataMember]
        public byte EtaHour { get; private set; }

        [DataMember]
        public byte EtaMinute { get; private set; }

        [DataMember]
        public float Draught { get; private set; }

        [DataMember]
        public string Destination { get; private set; }

        [DataMember]
        public bool DataTerminalReady { get; private set; }

        [DataMember]
        public bool Spare { get; private set; }

        public ClassAStaticAndVoyageDataMesage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, byte aisVersion, uint imo, string callsign, string vesselName, ShipType shipType, uint bow, uint stern, byte port, byte starboard, EpfdFixType positionFixType, byte etaMonth, byte etaDay, byte etaHour, byte etaMinute, float draught, string destination, bool dataTerminalReady, bool spare) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            AisVersion = aisVersion;
            Imo = imo;
            Callsign = callsign;
            VesselName = vesselName;
            ShipType = shipType;
            Bow = bow;
            Stern = stern;
            Port = port;
            Starboard = starboard;
            PositionFixType = positionFixType;
            EtaMonth = etaMonth;
            EtaDay = etaDay;
            EtaHour = etaHour;
            EtaMinute = etaMinute;
            Draught = draught;
            Destination = destination;
            DataTerminalReady = dataTerminalReady;
            Spare = spare;
        }
    }
}
