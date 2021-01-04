using System.Collections;
using AISDecoder.Model;

namespace AISDecoder.Decoder
{
    public interface IAisMessageFactory
    {
        AisMessage CreateMessage(BitArray payload);
    }
}
