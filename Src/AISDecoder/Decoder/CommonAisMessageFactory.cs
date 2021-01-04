using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AISDecoder.Common;
using AISDecoder.Decoder.MessageFactories;
using AISDecoder.Model;
using AISDecoder.Model.Messages;

namespace AISDecoder.Decoder
{
    /// <summary>
    /// Factory produces ais messages from one or more ais sentences.
    /// The next types are supported: 1,2,3,4,5,18,19,24,27.
    /// </summary>
    public class CommonAisMessageFactory
    {
        private readonly IDictionary<byte, IAisMessageFactory> m_factories = new Dictionary<byte, IAisMessageFactory>();
        private readonly SentenceBuffer m_buffer = new SentenceBuffer();

        public CommonAisMessageFactory()
        {
            var f123 = new AisMessageFactory123();
            m_factories.Add(1, f123);
            m_factories.Add(2, f123);
            m_factories.Add(3, f123);
            m_factories.Add(4, new AisMessageFactory4());
            m_factories.Add(5, new AisMessageFactory5());
            m_factories.Add(18, new AisMessageFactory18());
            m_factories.Add(19, new AisMessageFactory19());
            m_factories.Add(24, new AisMessageFactory24());
            m_factories.Add(27, new AisMessageFactory27());
        }

        /// <summary>
        /// Creates ais message from sentence.
        /// </summary>
        /// <param name="sentence">Ais sentence produced by Sentence factory.</param>
        /// <param name="isFragmented">True if message is fragment, false if message is complete.</param>
        /// <returns>Instance of supported ais message class, null if passed sentence is fragment, isFragmented is true in this case.</returns>
        public AisMessage CreateAisMessage(AisSentence sentence, out bool isFragmented)
        {
            if (sentence.FragmentCount <= 1)
            {
                isFragmented = false;
                return CreateAisMessageFromSentence(sentence);
            }

            
            var record = m_buffer.AddSentence(sentence);

            if (record.IsComplete())
            {
                isFragmented = false;
                return CreateAisMessageFromFragments(record.Sentences);
            }

            isFragmented = true;
            return null;
        }

        private AisMessage CreateAisMessageFromFragments(IEnumerable<AisSentence> sentences)
        {
            var list = new List<BitArray>();
            var totalBytes = 0;

            foreach (var aisSentence in sentences)
            {
                totalBytes += aisSentence.Payload.Length;

                var asciiDecimal = aisSentence.Payload.Select(AisEncoding.GetAsciiDecimalFromByte).ToArray();
                var bitArray = Utils.ConvertToMsb6Bit(asciiDecimal);
                
                if (aisSentence.FillBits != 0)
                {
                    PadPayLoad(bitArray, aisSentence.FillBits);
                }

                list.Add(bitArray);
            }

            var fullLoad = new BitArray(totalBytes * 8);
            var index = 0;
            
            foreach (var bitArray in list)
            {
                foreach (var bit in bitArray)
                {
                    fullLoad.Set(index, (bool)bit);
                    index++;
                }
            }

            return CreateAisMessageFromBitPayload(fullLoad);
        }

        private AisMessage CreateAisMessageFromSentence(AisSentence sentence)
        {
            var asciiDecimal = sentence.Payload.Select(AisEncoding.GetAsciiDecimalFromByte).ToArray();

            var payLoad = Utils.ConvertToMsb6Bit(asciiDecimal);
            
            if (sentence.FillBits != 0)
            {
                PadPayLoad(payLoad, sentence.FillBits);  
            }

            return CreateAisMessageFromBitPayload(payLoad);
        }

        private AisMessage CreateAisMessageFromBitPayload(BitArray payload)
        {
            var messageType = payload.DecodeByte(0, 5);
            IAisMessageFactory factory;

            if (!m_factories.TryGetValue(messageType, out factory))
            {
                return new UnsupportedMessage(messageType, DateTime.UtcNow.ToFileTimeUtc());
            }

            return factory.CreateMessage(payload);
        }

       

        private static void PadPayLoad(BitArray bitArray, byte fillBits)
        {
            var len = bitArray.Count;

            for (var i = 1; i <= fillBits; i++)
            {
                bitArray.Set(len - i, false);
            }
        }
    }
}
