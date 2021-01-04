using System.Runtime.Serialization;

namespace AISDecoder.Model
{
    [DataContract]
    public abstract class AisMessage
    {
        [DataMember]
        public long TimeStamp { get; private set; }

        [DataMember]
        public byte Type { get; private set; }

        protected AisMessage(byte type, long timeStamp)
        {
            TimeStamp = timeStamp;
            Type = type;
        }
    }
}
