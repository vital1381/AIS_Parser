using System.Text;
using AISDecoder.Model;

namespace AISDecoder.Decoder
{
    public static class Extensions
    {
        private const string c_separator = ",";
        private const string c_fillBitSeparator = "*";
        private const string c_leadChar = "!";

        public static string ToNmeaString(this AisSentence sentence)
        {
            return new StringBuilder()
                .Append(c_leadChar)
                .Append(sentence.TalkedId)
                .Append(c_separator)
                .Append(sentence.FragmentCount.ToString("X"))
                .Append(c_separator)
                .Append(sentence.FragmentNumber.ToString("X"))
                .Append(c_separator)
                .Append(sentence.SentenceId == 0 ? string.Empty : sentence.SentenceId.ToString("X"))
                .Append(c_separator)
                .Append(sentence.Channel)
                .Append(c_separator)
                .Append(Encoding.ASCII.GetString(sentence.Payload))
                .Append(c_separator)
                .Append(sentence.FillBits)
                .Append(c_fillBitSeparator)
                .Append(sentence.CheckSum.ToString("X")).ToString();
        }
    }
}
