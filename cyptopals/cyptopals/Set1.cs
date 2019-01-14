using System;
using System.Collections.Generic;
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

            var solution = new Solution();
            var bytes = solution.HexToByteArray(hex);
            var actualResult = Convert.ToBase64String(bytes);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Exercise2()
        {
            var string1 = "1c0111001f010100061a024b53535009181c";
            var string2 = "686974207468652062756c6c277320657965";
            var expectedResult = "746865206b696420646f6e277420706c6179";

            var solution = new Solution();
            var bytes1 = solution.HexToByteArray(string1);
            var bytes2 = solution.HexToByteArray(string2);
            var actualBytes = bytes1
                                .Zip(bytes2, (a, b) => a ^ b)
                                .Select(b => b.ToString("x2"));

            var actualResult = actualBytes.ConcatStrings();

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Exersise3()
        {
            var cipherText = "1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";

            var solution = new Solution();
            var result = solution.SolveSingleXORKey(cipherText);

            Assert.Equal("Cooking MC's like a pound of bacon", result.PlainText);
        }

        [Fact]
        public void Exersise4()
        {
            var cipherTexts = File.ReadAllLines(@"FileForSet1_Exercise4.txt");

            var solution = new Solution();
            var bestResult = new SolveSingleXORKeyResult();
            foreach (var line in cipherTexts)
            {
                var result = solution.SolveSingleXORKey(line);
                if (result.Score > bestResult.Score)
                    bestResult = result;
            }

            Assert.Equal("Now that the party is jumping\n", bestResult.PlainText);
        }

        [Fact]
        public void Exersise5()
        {
            var plainText = "Burning 'em, if you ain't quick and nimble\nI go crazy when I hear a cymbal";
            var expectedResult = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f";
            var key = "ICE";

            var solution = new Solution();
            var actualResult = solution.EncryptText(plainText, key);

            Assert.Equal(expectedResult, actualResult);
        }

        [Fact]
        public void Exercise6()
        {
            var cyphertext = File.ReadAllText("FileForSet1_Exercise6.txt");
            var bytes = Convert.FromBase64String(cyphertext);

            var solution = new Solution();

            var smallestDistance = double.MaxValue;
            var bestKeySize = 0;
            for (int keysize = 5; keysize <= 40; keysize++)
            {
                var first = bytes.Take(keysize);
                var second = bytes.Skip(keysize * 1).Take(keysize);
                var third = bytes.Skip(keysize * 2).Take(keysize);
                var fourth = bytes.Skip(keysize * 3).Take(keysize);

                var hammingDistance =
                    ((solution.ComputeHammingDistance(first, second) / (double)keysize) +
                    (solution.ComputeHammingDistance(second, third) / (double)keysize) +
                    (solution.ComputeHammingDistance(third, fourth) / (double)keysize)) / 3;

                if (hammingDistance < smallestDistance)
                {
                    smallestDistance = hammingDistance;
                    bestKeySize = keysize;
                }
            }

            Assert.Equal(29, bestKeySize);

            var blocks = solution.SplitIntoBlocks(bytes, bestKeySize);
            var key = "";
            for (int offset = 1; offset <= bestKeySize; offset++)
            {
                var block1 = blocks.Where(b => b.Count() >= offset).Select(b => b.Skip(offset - 1).First()).ToArray();
                key += solution.SolveSingleXORKey(block1).Key;
            }

            Assert.Equal("Terminator X: Bring the noise", key);

            var plainText =
                         bytes.Select((c, offset) => (char)(c ^ key[offset % bestKeySize]))
                         .ConcatStrings();

            Assert.StartsWith("I'm back and I'm ringin' the bell", plainText);


        }

        [Fact]
        public void Exercise7()
        {
            var base64 = File.ReadAllText("FileForSet1_Exercise7.txt");
            var cipherText = Convert.FromBase64String(base64);
            var key = "YELLOW SUBMARINE".Select(b => (byte)b).ToArray();

            var aes = new AES();
            var plainText = aes.DecryptStringFromBytes_Aes(cipherText, key);
            Assert.StartsWith("I'm back and I'm ringin", plainText);
        }

        [Fact]
        public void Exercise8()
        {
            var hexLines = File.ReadAllLines("FileForSet1_Exercise8.txt");
            var results = new HashSet<int>();
            var lineNumber = 0;
            foreach (var hexLine in hexLines)
            {
                for (int offset = 0; offset < hexLine.Length - 64; offset++)
                {
                    var lhs = hexLine.Substring(offset, 32);
                    var index = hexLine.IndexOf(lhs, offset + 32);
                    if (index >= 0 && ((index - offset) % 32 == 0)) results.Add(lineNumber);
                }

                lineNumber++;
            }

            Assert.Single(results);
            Assert.Equal(132, results.First());
        }





        [Fact]
        public void HammingDistance()
        {
            var string1 = "this is a test";
            var string2 = "wokka wokka!!!";

            var solution = new Solution();
            var hammingDistance = solution.ComputeHammingDistance(string1, string2);

            Assert.Equal(37, hammingDistance);
        }


    }
}
