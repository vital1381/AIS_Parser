using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 19.
    /// </summary>
    [DataContract]
    public class ClassBExtendedPositionReportMessage : AisMessage
    {

        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public float SpeedOverGround { get; private set; }

        [DataMember]
        public bool PositionAcuracy { get; private set; }

        [DataMember]
        public float Longtitude { get; private set; }

        [DataMember]
        public float Latitude { get; private set; }

        [DataMember]
        public float CourseOverGround { get; private set; }

        [DataMember]
        public ushort TrueHeading { get; private set; }

        [DataMember]
        public byte UtcSeconds { get; private set; }

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
        public bool RaimFlag { get; private set; }

        [DataMember]
        public bool DataTerminalReady { get; private set; }

        [DataMember]
        public bool AssignedMode { get; private set; }

        [DataMember]
        public byte Spare { get; private set; }

        public ClassBExtendedPositionReportMessage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, float speedOverGround, bool positionAcuracy, float longtitude, float latitude, float courseOverGround, ushort trueHeading, byte utcSeconds, string vesselName, ShipType shipType, uint bow, uint stern, byte port, byte starboard, EpfdFixType positionFixType, bool raimFlag, bool dataTerminalReady, bool assignedMode, byte spare) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            SpeedOverGround = speedOverGround;
            PositionAcuracy = positionAcuracy;
            Longtitude = longtitude;
            Latitude = latitude;
            CourseOverGround = courseOverGround;
            TrueHeading = trueHeading;
            UtcSeconds = utcSeconds;
            VesselName = vesselName;
            ShipType = shipType;
            Bow = bow;
            Stern = stern;
            Port = port;
            Starboard = starboard;
            PositionFixType = positionFixType;
            RaimFlag = raimFlag;
            DataTerminalReady = dataTerminalReady;
            AssignedMode = assignedMode;
            Spare = spare;
        }
    }
}
