using System.Collections;

namespace AISDecoder.Common
{
    internal static class Utils
    {
        public static BitArray ConvertToMsb6Bit(byte[] bytes)
        {
            var result = new BitArray(bytes.Length * 6);
            int index = 0;

            foreach (var b in bytes)
            {
                FillBitArray(result, b, index);
                index += 6;
            }
            
            return result;
        }

        private static void FillBitArray(BitArray result, byte data, int index)
        {
            BitArray array = new BitArray(new []{data});
            for (var i = 5; i >= 0; i--)
            {
                result.Set(index + 5-i, array.Get(i));
            }
        }
    }
}
