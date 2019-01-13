using System;
using System.Linq;

namespace cryptopals
{
    internal class Solution
    {
        public SolveSingleXORKeyResult SolveSingleXORKey(string cipherText)
        {
            var result = new SolveSingleXORKeyResult();
            var cipherBytes = HexToByteArray(cipherText);

            for (var key = 1; key < 128; key++)
            {
                var possibleAnswer = cipherBytes
                                            .Select(b => (char)(b ^ key))
                                            .ConcatStrings();
                var numberOfEnglishLetters = possibleAnswer.Count(c => char.IsLetter(c) || c == ' ');
                if (numberOfEnglishLetters > result.Score)
                {
                    result.Score = numberOfEnglishLetters;
                    result.PlainText = possibleAnswer;
                }
            }

            return result;
        }

        public byte[] HexToByteArray(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }

        internal string EncryptText(string text, string key)
        {
            return text
                    .Select((c, offset) => (c ^ key[offset % 3]).ToString("x2"))
                    .ConcatStrings();
        }


    }
}
