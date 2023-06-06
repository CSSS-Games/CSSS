//  CSSSSupportLibrary - CyberSecurity Scoring System Support Library
//  
//  License: CC-By-SA
//  See: http://stackoverflow.com/a/10177020
//  
//  A few changes have been made to the code from StackOverflow,
//  mainly to not require a password to encrypt or decrypt the
//  string passed, but to use the name of the computer

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace SupportLibrary.Encryption
{
    /// <summary>
    /// Allows the issue check JSON files to be encrypted or
    /// decrypted to prevent competitors being able to read
    /// the issues
    /// </summary>
    public class String
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 128;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        private static byte[] IV =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };

        private static byte[] Key
        {
            get
            {
                var emptySalt = Array.Empty<byte>();
                var desiredKeyLength = 16;
                var hashMethod = HashAlgorithmName.SHA384;
                return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(Environment.MachineName),
                                                 emptySalt,
                                                 DerivationIterations,
                                                 hashMethod,
                                                 desiredKeyLength);
            }
        }

        /// <summary>
        /// Encrypts the specified plaintext
        /// </summary>
        /// <returns>The encrypted string</returns>
        /// <param name="plainText">The plaintext message</param>
        public static string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Key;
            aes.IV = IV;

            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.UTF8.GetBytes(plainText));
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(output.ToArray());
        }

        /// <summary>
        /// Decrypts the specified ciphertext
        /// </summary>
        /// <returns>The decrypted cipertext</returns>
        /// <param name="cipherText">The encrypted ciphertext</param>
        public static string Decrypt(string cipherText)
        {
            using Aes aes = Aes.Create();
            aes.Key = Key;
            aes.IV = IV;
            using MemoryStream input = new(Convert.FromBase64String(cipherText));
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            cryptoStream.CopyTo(output);
            return Encoding.UTF8.GetString(output.ToArray());
        }
    }
}
