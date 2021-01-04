namespace AISDecoder.Model.Messages
{
    /// <summary>
    /// Unsupported message.
    /// </summary>
    public class UnsupportedMessage : AisMessage
    {
        public UnsupportedMessage(byte type, long timeStamp) : base(type, timeStamp)
        {
        }
    }
}
