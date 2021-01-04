using System.Collections.Generic;
using System.Linq;

namespace AISDecoder.Common
{
    internal static class Crc8
    {
        public static ushort Calculate(IEnumerable<byte> data)
        {
            return data.Aggregate((byte)0, (b, b1) => b ^= b1);
        }
    }
}
