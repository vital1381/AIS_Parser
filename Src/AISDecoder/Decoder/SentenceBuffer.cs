using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AISDecoder.Model;

namespace AISDecoder.Decoder
{
    internal class SentenceBuffer
    {
        private const byte c_bufferSize = 100;
        private const byte c_bufferRetentionPercent = 20;
        private static volatile int m_seqId = 0;

        public class Record
        {
            private byte m_count = 1;
            private readonly List<AisSentence> m_sentences = new List<AisSentence>();

            public IEnumerable<AisSentence> Sentences
            {
                get { return m_sentences; }
            }

            public int RecordId { get; private set; }
            

            public Record()
            {
                RecordId = m_seqId++;
               
            }

            public bool IsComplete()
            {
                return (m_count == m_sentences.Count);
            }

            public Record AddSentence(AisSentence sentence)
            {
                m_count = sentence.FragmentCount;
                m_sentences.Add(sentence);
                return this;
            }
        }

        private readonly IDictionary<byte, Record> m_buffer = new Dictionary<byte, Record>();

        public Record AddSentence(AisSentence sentence)
        {
            Record record;

            if (m_buffer.TryGetValue(sentence.SentenceId, out record))
            {
                return record.AddSentence(sentence);
            }

            CheckBufferLimit();
            record = new Record().AddSentence(sentence);
            m_buffer.Add(sentence.SentenceId, record);
            return record;
        }

        private void CheckBufferLimit()
        {
            if (m_buffer.Count < c_bufferSize)
            {
                return;
            }

            //Clean buffer
            const int elementCountToClean = c_bufferSize*c_bufferRetentionPercent / 100;
            var sortedIds = m_buffer.Keys.OrderBy(b => b).Take(elementCountToClean).ToArray();
            
            foreach (var id in sortedIds)
            {
                if (m_buffer.ContainsKey(id))
                {
                    m_buffer.Remove(id);    
                }
            }
        }
    }
}
