using System;
using System.IO;
using System.Linq;
using Xunit;

namespace cryptopals
{
    public class Set1
    {
        [Fact]
        public void Exercise1()
        {
            var hex = "49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            var expectedResult = "SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

            var bytes = HexToByteArray(hex);
            var actualResult = Convert.ToBase64String(bytes);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Exercise2()
        {
            var string1 = "1c0111001f010100061a024b53535009181c";
            var string2 = "686974207468652062756c6c277320657965";
            var expectedResult = "746865206b696420646f6e277420706c6179";

            var bytes1 = HexToByteArray(string1);
            var bytes2 = HexToByteArray(string2);
            var actualBytes = bytes1
                                .Zip(bytes2, (a, b) => a ^ b)
                                .Select(b => b.ToString("x2"));

            var actualResult = string.Join("", actualBytes);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Exersise3()
        {
            var cipherText = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";

            var result = SolveSingleXORKey(cipherText);

            Assert.Equal("Cooking MC's like a pound of bacon", result.PlainText);
        }


        [Fact]
        public void Exersise4()
        {
            var cipherTexts = File.ReadAllLines(@"FileForSet1_Exercise4.txt");

            var bestResult = new SolveSingleXORKeyResult();
            foreach (var line in cipherTexts)
            {
                var result = SolveSingleXORKey(line);
                if (result.Score > bestResult.Score)
                    bestResult = result;
            }

            Assert.Equal("Now that the party is jumping\n", bestResult.PlainText);
        }

        private static SolveSingleXORKeyResult SolveSingleXORKey(string cipherText)
        {
            var result = new SolveSingleXORKeyResult();
            var cipherBytes = HexToByteArray(cipherText);

            for (var key = 1; key < 128; key++)
            {
                var possibleAnswer = (string.Join("", cipherBytes.Select(b => (char)(b ^ key))));
                var numberOfEnglishLetters = possibleAnswer.Count(c => char.IsLetter(c) || c == ' ');
                if (numberOfEnglishLetters > result.Score)
                {
                    result.Score = numberOfEnglishLetters;
                    result.PlainText = possibleAnswer;
            //        result.Key = key;
                }
            }

            return result;
        }

        private static byte[] HexToByteArray(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length / 2; i++)
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            return bytes;
        }

        private class SolveSingleXORKeyResult
        {
            public string PlainText { get; set; }
          //  public char Key { get; set; }
            public int Score { get; set; }
        }
    }
}
