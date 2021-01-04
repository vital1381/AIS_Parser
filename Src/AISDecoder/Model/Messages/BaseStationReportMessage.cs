using System.Runtime.Serialization;

namespace AISDecoder.Model.Messages
{
    [DataContract]
    public class BaseStationReportMessage : AisMessage
    {
        [DataMember]
        public byte RepeatIndicator { get; private set; }

        [DataMember]
        public uint Mmsi { get; private set; }

        [DataMember]
        public uint Year { get; private set; }

        [DataMember]
        public byte Month { get; private set; }

        [DataMember]
        public byte Day { get; private set; }

        [DataMember]
        public byte Hour { get; private set; }

        [DataMember]
        public byte Minute { get; private set; }

        [DataMember]
        public byte Second { get; private set; }

        [DataMember]
        public bool FixQuality { get; private set; }

        [DataMember]
        public float Longtitude { get; private set; }

        [DataMember]
        public float Latitude { get; private set; }

        [DataMember]
        public EpfdFixType PositionFixType { get; private set; }

        [DataMember]
        public bool RaimFlag { get; private set; }

        [DataMember]
        public uint RadioStatus { get; private set; }

        public BaseStationReportMessage(byte type, long timeStamp, byte repeatIndicator, uint mmsi, uint year, byte month, byte day, byte hour, byte minute, byte second, bool fixQuality, float longtitude, float latitude, EpfdFixType positionFixType, bool raimFlag, uint radioStatus) : base(type, timeStamp)
        {
            RepeatIndicator = repeatIndicator;
            Mmsi = mmsi;
            Year = year;
            Month = month;
            Day = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            FixQuality = fixQuality;
            Longtitude = longtitude;
            Latitude = latitude;
            PositionFixType = positionFixType;
        
            RaimFlag = raimFlag;
            RadioStatus = radioStatus;
        }
    }
}
