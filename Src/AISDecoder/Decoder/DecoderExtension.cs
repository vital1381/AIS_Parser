using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AISDecoder.Common;

namespace AISDecoder.Decoder
{
    internal static class DecoderExtension
    {
        public static int DecodeInt32(this BitArray bits, int from, int to)
        {
            if (from < 0 || to < 0 || to - from > 32)
            {
                throw new ArgumentOutOfRangeException();
            }
            int value = 0;

            for (var i = from; i <= to; i++)
            {
                if (bits.Get(i))
                {
                    value |= (int)(1 << to - i);
                }
            }

            return value;
        }

        public static uint DecodeUInt32(this BitArray bits, int from, int to)
        {
            if (from < 0 || to < 0 || to - from > 32)
            {
                throw new ArgumentOutOfRangeException();
            }
            uint value = 0;

            for (var i = from; i <= to; i++)
            {
                if (bits.Get(i))
                {
                    value |= (uint)(1 << to - i);
                }
            }

            return value;
        }

        public static ushort DecodeUShort(this BitArray bits, int from, int to)
        {
            if (from < 0 || to < 0 || to - from > 16)
            {
                throw new ArgumentOutOfRangeException();
            }
            ushort value = 0;

            for (var i = from; i <= to; i++)
            {
                if (bits.Get(i))
                {
                    value |= (ushort)(1 << to - i);
                }
            }

            return value;
        }

        public static double DecodeFloat(this BitArray bits, int from, int to)
        {
            int value = 0;

            for (var i = from; i <= to; i++)
            {
                if (bits.Get(i))
                {
                    value |= (int)(1 << to - i);
                }
            }

            double ret = value;

            if (bits.Get(from))
            {
                ret -= Math.Pow(2, to - from);
            }

            return ret;
        }

        public static byte DecodeByte(this BitArray bits, int from, int to)
        {
            if (from < 0 || to < 0 || to - from > 8)
            {
                throw new ArgumentOutOfRangeException();
            }

            byte value = 0;

            for (var i = from; i <= to; i++)
            {
                if (bits.Get(i))
                {
                    value |= (byte)(1 << to - i);
                }
            }

            return value;
        }

        public static string DecodeString(this BitArray bits, int from, int to)
        {
            if (from < 0 || to < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var bytes = new List<byte>();

            byte value = 0;
            var index = 0;

            for (var i = from; i <= to; i++)
            {
                index++;

                if (bits.Get(i))
                {
                    value |= (byte)(1 << 6 - index);
                }

                if (index % 6 != 0)
                {
                    continue;
                }

                bytes.Add(value);
                value = 0;
                index = 0;
            }

            if (index % 6 != 0)
            {
                bytes.Add(value);
            }

            return new string(bytes.Select(AisEncoding.GetChar).ToArray());
        }

        public static bool DecodeBool(this BitArray bits, int from)
        {
            return from <= bits.Count && bits.Get(from);
        }
    }
}
