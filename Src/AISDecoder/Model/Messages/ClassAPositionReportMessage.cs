using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 1,2,3.
    /// </summary>
    [DataContract]
    public class ClassAPositionReportMessage : AisMessage
    {
        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public NavigationStatus NavigationStatus { get; private set; }

        [DataMember]
        public int RateOfTurn { get; private set; }

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
        public ManeuverIndicator ManeuverIndicator { get; private set; }

        [DataMember]
        public byte Spare { get; private set; }

        [DataMember]
        public bool RaimFlag { get; private set; }

        [DataMember]
        public uint RadioStatus { get; private set; }


        public ClassAPositionReportMessage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, NavigationStatus navigationStatus, int rateOfTurn, float speedOverGround, bool positionAcuracy, float longtitude, float latitude, float courseOverGround, ushort trueHeading, byte utcSeconds, ManeuverIndicator maneuverIndicator, byte spare, bool raimFlag, uint radioStatus) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            NavigationStatus = navigationStatus;
            RateOfTurn = rateOfTurn;
            SpeedOverGround = speedOverGround;
            PositionAcuracy = positionAcuracy;
            Longtitude = longtitude;
            Latitude = latitude;
            CourseOverGround = courseOverGround;
            TrueHeading = trueHeading;
            UtcSeconds = utcSeconds;
            ManeuverIndicator = maneuverIndicator;
            Spare = spare;
            RaimFlag = raimFlag;
            RadioStatus = radioStatus;
        }
    }
}
