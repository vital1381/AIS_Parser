using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using AISDecoder.Common;
using AISDecoder.Model;


namespace AISDecoder.Decoder
{
    /// <summary>
    /// Factory produces ais sentence object.
    /// </summary>
    public class SentenceFactory
    {
        private const byte c_startByte = 33;
        private const byte c_separator = 44;
        private const byte c_blockCount = 7;
        private const byte c_fillBitSeparator = 42;
        private const byte c_endByte1 = 13;
        private const byte c_endByte2 = 10;

        /// <summary>
        /// Is checksum check enabled, if true then sentences with incorrect checksum will not be processed.
        /// ArgumenentException with message "Checksum doesn't match" is thrown.
        /// if checksum check enabled is set to false then factory returns sentence object even if checksum is incorrect.
        /// </summary>
        public bool IsCrcEnabled { get; set; }

        /// <summary>
        /// Creates enumerable of ais sentences.
        /// </summary>
        /// <param name="raw">Array of bytes containing one or more ais sentences separated by bytes 10 13.</param>
        /// <returns></returns>
        public IEnumerable<AisSentence> ProcessStream(byte[] raw)
        {
            var sentenceRaw = new List<byte>();
            var e1 = false;

            foreach (var b in raw)
            {
                sentenceRaw.Add(b);

                if (b == c_endByte1)
                {
                    e1 = true;
                    continue;
                }

                if (!e1 || b != c_endByte2)
                {
                    continue;
                }

                sentenceRaw.RemoveAt(sentenceRaw.Count - 1);
                sentenceRaw.RemoveAt(sentenceRaw.Count - 1);
                yield return CreateSentence(sentenceRaw.ToArray());
              
                sentenceRaw.Clear();
                e1 = false;
            }
        }

        
        
        /// <summary>
        /// Creates instance of AisSentence class from raw data array of bytes.
        /// </summary>
        /// <param name="rawData">Array of bytes, containing single sentence. For example: !AIVDM,1,1,,A,13HOI:0P0000VOHLCnHQKwvL05Ip,0*23 </param>
        /// <returns>Instance of AisSentence.</returns>
        public AisSentence CreateSentence(byte[] rawData)
        {
            int lenght = rawData.Length;

            if (lenght == 0)
            {
                throw new ArgumentException("Raw data array has 0 lenght");
            }

            var blocks = SplitByBlocks(rawData);

            if (blocks.Count != c_blockCount)
            {
                throw new ArgumentException(string.Format("Block count is not equal to {0}", c_blockCount));
            }

            byte fillBits;
            ushort crc;
            GetFillBitAndCrc(blocks[6], out fillBits, out crc);

            if (IsCrcEnabled)
            {
                var computedCrc = Crc8.Calculate(rawData.Skip(1).TakeWhile(b => b != c_fillBitSeparator));

                if (crc != computedCrc)
                {
                    throw new ArgumentException("Checksum doesn't match.");
                }
            }

            var talkerId = Encoding.ASCII.GetString(blocks[0]);
            var fragmentCount = byte.Parse(Encoding.ASCII.GetString(blocks[1]), NumberStyles.HexNumber);
            var fragmentNumber = byte.Parse(Encoding.ASCII.GetString(blocks[2]), NumberStyles.HexNumber);
            var sentenceId = (byte) (blocks[3].Length != 0 ? byte.Parse(Encoding.ASCII.GetString(blocks[3]),NumberStyles.HexNumber) : 0);
            var channel = Encoding.ASCII.GetChars(blocks[4]).FirstOrDefault();
            
            return new AisSentence(talkerId, fragmentCount, fragmentNumber, sentenceId,channel, blocks[5], fillBits, crc);
        }

        private static void GetFillBitAndCrc(byte[] bytes, out byte fillBit, out ushort crc)
        {
            if (bytes.Length != 4 && bytes.Length != 3)
            {
                throw new ArgumentException("Block 7 must be 4 or 3 bytes: FillBit*CRC, 0*0C 01");
            }

            if (bytes[1] != c_fillBitSeparator)
            {
                throw new ArgumentException("Block 7 seconds bit is not *");
            }

            fillBit = byte.Parse(AisEncoding.GetString(new[]{ bytes[0] }), NumberStyles.HexNumber);
            var crcStr = AisEncoding.GetString(bytes.Skip(2).ToArray());
            crc = ushort.Parse(crcStr,NumberStyles.HexNumber);
        }

        private static List<byte[]> SplitByBlocks(byte[] rawData)
        {
            var blocks = new List<byte[]>();
            var block = new List<byte>();
            
            foreach (var b in rawData)
            {
                //Skip start byte
                if (b == c_startByte)
                {
                    continue;
                }

                if (b == c_separator)
                {
                    blocks.Add(block.ToArray());
                    block = new List<byte>();
                    continue;
                }

                block.Add(b);
            }

            blocks.Add(block.ToArray());
            return blocks;
        }
    }
}
