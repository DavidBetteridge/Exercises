using System;
using System.Collections.Generic;
using System.Linq;

namespace cryptopals
{
    internal class Solution
    {
        public SolveSingleXORKeyResult SolveSingleXORKey(string cipherText)
        {
            var cipherBytes = HexToByteArray(string.Join("",cipherText));
            return SolveSingleXORKey(cipherBytes);
        }

        public SolveSingleXORKeyResult SolveSingleXORKey(IEnumerable<byte> cipherBytes)
        {
            var result = new SolveSingleXORKeyResult();

            for (var key = 1; key < 128; key++)
            {
                var possibleAnswer = cipherBytes
                                            .Select(b => (char)(b ^ key))
                                            .ConcatStrings();
                var numberOfEnglishLetters = possibleAnswer.Count(c => char.IsLetterOrDigit(c) || c == ' ');
                if (numberOfEnglishLetters > result.Score)
                {
                    result.Score = numberOfEnglishLetters;
                    result.PlainText = possibleAnswer;
                    result.Key = (char)key;
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

        internal int ComputeHammingDistance(string string1, string string2)
        {
            var differences = string1.Zip(string2, (a, b) => a ^ b);
            var hammingDistance = differences.Sum(s => CountBits(s));
            return hammingDistance;
        }

        internal int CountBits(int value)
        {
            var count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
        }

        internal int ComputeHammingDistance(IEnumerable<byte> first, IEnumerable<byte> second)
        {
            var differences = first.Zip(second, (a, b) => a ^ b);
            var hammingDistance = differences.Sum(s => CountBits(s));
            return hammingDistance;
        }
    }
}
