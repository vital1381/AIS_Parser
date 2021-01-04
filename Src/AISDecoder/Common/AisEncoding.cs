using System.Text;

namespace AISDecoder.Common
{
    //ASCII 6bit encoding
    internal static class AisEncoding
    {
        private static readonly char[] s_chars;

        static AisEncoding()
        {
            s_chars = new[]
            {
                '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O',
                'P', 'Q', 'R', 'S', 'T', 'U', 'V','W','X','Y','Z', '[','\\',']','^','_',
                ' ','!','\"','#','$','%','&','\'','(',')','*','+',',','-','.','/',
                '0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?'


            };
        }

        public static string GetString(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            for (var i = 0; i < bytes.Length; i++)
            {
                chars[i] = GetChar(bytes[i]);
            }
            return new string(chars);
        }

        public static char GetChar(byte b)
        {
            return b < s_chars.Length ? s_chars[b] : Encoding.ASCII.GetChars(new[] {b})[0];
        }

        public static byte GetAsciiDecimalFromByte(byte code)
        {
            if (code >= 48 && code < 88)
            {
                //40 chars 0 - W
                code -= 48;
            }
            else if (code >= 96 && code < 121)
            {
                //24 chars `,a - w
                code -= 56;
            }
            else
            {
                code = 0; //ignore any ather chars
            }
            return code;
        }
    }
}
