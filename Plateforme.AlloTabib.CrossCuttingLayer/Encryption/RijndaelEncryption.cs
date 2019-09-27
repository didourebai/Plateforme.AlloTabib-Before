using System;
using System.IO;
using System.Security.Cryptography;

namespace Plateforme.AlloTabib.CrossCuttingLayer.Encryption
{
    public class RijndaelEncryption
    {
        #region Private Fields

        private static readonly byte[] _RijndaelKey = new byte[]
                                                          {
                                                              172, 113, 138, 95, 185, 0, 34, 57, 49, 58, 253, 114, 194,
                                                              240
                                                              , 6, 130, 19, 27, 229, 69, 66, 183, 207, 44, 87, 193, 165,
                                                              205, 62, 90, 72, 163
                                                          };

        private static readonly byte[] _RijndaelIV = new byte[]
                                                         {
                                                             154, 4, 42, 187, 253, 110, 174, 9, 204, 113, 88, 240, 173,
                                                             245
                                                             , 225, 166
                                                         };

        #endregion

        #region Public Methods

        public static string Decrypt(byte[] data)
        {
            return DecryptStringFromBytes(data, _RijndaelKey, _RijndaelIV);
        }

        public static byte[] Encrypt(string data)
        {
            return EncryptStringToBytes(data, _RijndaelKey, _RijndaelIV);
        }

        #endregion

        #region Private Methods

        private static byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("key");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an Rijndael object
            // with the specified key and IV.
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return plaintext;
        }

        #endregion
    }
}