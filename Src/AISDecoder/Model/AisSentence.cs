using System.Runtime.Serialization;

namespace AISDecoder.Model
{
    [DataContract]
    public sealed class AisSentence
    {
        [DataMember]
        public string TalkedId { get; private set; }

        [DataMember]
        public byte FragmentCount { get; private set; }

        [DataMember]
        public byte FragmentNumber { get; private set; }

        [DataMember]
        public byte SentenceId { get; private set; }

        [DataMember]
        public char Channel { get; private set; }

        [DataMember]
        public byte[] Payload { get; private set; }

        [DataMember]
        public byte FillBits{ get; private set; }

        [DataMember]
        public ushort CheckSum { get; private set; }
        
        public AisSentence(string talkerId, byte fragmentCount, byte fragmentNumber, byte sentenceId, char channel, byte[] payload, byte fillBits, ushort checkSum)
        {
            TalkedId = talkerId;
            FragmentCount = fragmentCount;
            FragmentNumber = fragmentNumber;
            SentenceId = sentenceId;
            Channel = channel;
            Payload = payload;
            FillBits = fillBits;
            CheckSum = checkSum;
        }
    }
}
