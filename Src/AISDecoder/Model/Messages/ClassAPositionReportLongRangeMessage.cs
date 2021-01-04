using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Message type: 27.
    /// </summary>
    [DataContract]
    public class ClassAPositionReportLongRangeMessage : AisMessage
    {
        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public bool PositionAcuracy { get; private set; }

        [DataMember]
        public bool RaimFlag { get; private set; }

        [DataMember]
        public NavigationStatus NavigationStatus { get; private set; }

        [DataMember]
        public float Longtitude { get; private set; }

        [DataMember]
        public float Latitude { get; private set; }

        [DataMember]
        public float SpeedOverGround { get; private set; }

        [DataMember]
        public float CourseOverGround { get; private set; }

        [DataMember]
        public bool GnssPosition { get; private set; }

        [DataMember]
        public bool Spare { get; private set; }

        public ClassAPositionReportLongRangeMessage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, bool positionAcuracy, bool raimFlag, NavigationStatus navigationStatus, float longtitude, float latitude, float speedOverGround, float courseOverGround, bool gnssPosition, bool spare) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            PositionAcuracy = positionAcuracy;
            RaimFlag = raimFlag;
            NavigationStatus = navigationStatus;
            Longtitude = longtitude;
            Latitude = latitude;
            SpeedOverGround = speedOverGround;
            CourseOverGround = courseOverGround;
            GnssPosition = gnssPosition;
            Spare = spare;
        }
    }
}
