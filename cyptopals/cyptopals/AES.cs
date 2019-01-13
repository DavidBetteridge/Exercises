using System.IO;
using System.Security.Cryptography;

namespace cryptopals
{
    internal class AES
    {
        internal string DecryptStringFromBytes_Aes(byte[] cipherText, byte[] Key)
        {
            string plaintext = null;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Mode = CipherMode.ECB;
                aesAlg.Key = Key;
                //saesAlg.IV = ;

                // Create a decryptor to perform the stream transform.
                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;

        }
    }
}
