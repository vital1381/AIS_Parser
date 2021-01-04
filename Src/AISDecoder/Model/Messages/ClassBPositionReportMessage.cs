using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 18.
    /// </summary>
    [DataContract]
    public class ClassBPositionReportMessage : AisMessage
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
        public CsUnitType CsUnit { get; private set; }

        [DataMember]
        public bool DisplayFlag { get; private set; }

        [DataMember]
        public bool DscFlag { get; private set; }

        [DataMember]
        public bool BandFlag { get; private set; }

        [DataMember]
        public bool Message22Flag { get; private set; }

        [DataMember]
        public bool AssignedMode { get; private set; }

        [DataMember]
        public bool RaimFlag { get; private set; }

        [DataMember]
        public uint RadioStatus { get; private set; }

        public ClassBPositionReportMessage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, float speedOverGround, bool positionAcuracy, float longtitude, float latitude, float courseOverGround, ushort trueHeading, byte utcSeconds, bool raimFlag, uint radioStatus, CsUnitType csUnit, bool displayFlag, bool dscFlag, bool bandFlag, bool message22Flag, bool assignedMode) : base(type, timeStamp)
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
            RaimFlag = raimFlag;
            RadioStatus = radioStatus;
            CsUnit = csUnit;
            DisplayFlag = displayFlag;
            DscFlag = dscFlag;
            BandFlag = bandFlag;
            Message22Flag = message22Flag;
            AssignedMode = assignedMode;
        }
    }
}
