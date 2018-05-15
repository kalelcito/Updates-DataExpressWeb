// ***********************************************************************
// Assembly         : Control
// Author           : Sergio
// Created          : 01-30-2017
//
// Last Modified By : Sergio
// Last Modified On : 01-30-2017
// ***********************************************************************
// <copyright file="Security.cs" company="DataExpress Latinoamérica">
//     Copyright © DataExpress Latinoamérica ®
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Control
{
    /// <summary>
    /// Class Security.
    /// </summary>
    public class Security
    {
        /// <summary>
        /// Encryptions the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public string Encryption(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = Encoding.UTF8.GetBytes(text);

            byte[] baSalt = GetRandomBytes();
            byte[] baEncrypted = new byte[baSalt.Length + baText.Length];

            // Combine Salt + Text
            for (int i = 0; i < baSalt.Length; i++)
                baEncrypted[i] = baSalt[i];
            for (int i = 0; i < baText.Length; i++)
                baEncrypted[i + baSalt.Length] = baText[i];

            baEncrypted = Encryption(baEncrypted, baPwdHash);

            string result = ByteArrayToString(baEncrypted);//Convert.ToBase64String(baEncrypted);
            return result;
        }

        /// <summary>
        /// Encryptions the specified bytes to be encrypted.
        /// </summary>
        /// <param name="bytesToBeEncrypted">The bytes to be encrypted.</param>
        /// <param name="passwordBytes">The password bytes.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] Encryption(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 4, 6, 8, 10, 12, 14, 16 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        /// <summary>
        /// Decryptions the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public string Decryption(string text, string password)
        {
            byte[] baPwd = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            byte[] baPwdHash = SHA256Managed.Create().ComputeHash(baPwd);

            byte[] baText = StringToByteArray(text);//Convert.FromBase64String(text);

            byte[] baDecrypted = Decryption(baText, baPwdHash);

            // Remove salt
            int saltLength = GetSaltLength();
            byte[] baResult = new byte[baDecrypted.Length - saltLength];
            for (int i = 0; i < baResult.Length; i++)
                baResult[i] = baDecrypted[i + saltLength];

            string result = Encoding.UTF8.GetString(baResult);
            return result;
        }

        /// <summary>
        /// Decryptions the specified bytes to be decrypted.
        /// </summary>
        /// <param name="bytesToBeDecrypted">The bytes to be decrypted.</param>
        /// <param name="passwordBytes">The password bytes.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] Decryption(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            byte[] saltBytes = new byte[] { 2, 4, 6, 8, 10, 12, 14, 16 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged aes = new RijndaelManaged())
                {
                    aes.KeySize = 256;
                    aes.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    aes.Key = key.GetBytes(aes.KeySize / 8);
                    aes.IV = key.GetBytes(aes.BlockSize / 8);

                    aes.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <summary>
        /// Encrypts the file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="fileEncrypted">The file encrypted.</param>
        /// <param name="password">The password.</param>
        public void EncryptFile(string file, string fileEncrypted, string password)
        {
            byte[] bytesToBeEncrypted = File.ReadAllBytes(file);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = Encryption(bytesToBeEncrypted, passwordBytes);

            File.WriteAllBytes(fileEncrypted, bytesEncrypted);
        }

        /// <summary>
        /// Decrypts the file.
        /// </summary>
        /// <param name="fileEncrypted">The file encrypted.</param>
        /// <param name="file">The file.</param>
        /// <param name="password">The password.</param>
        public void DecryptFile(string fileEncrypted, string file, string password)
        {
            byte[] bytesToBeDecrypted = File.ReadAllBytes(fileEncrypted);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = Decryption(bytesToBeDecrypted, passwordBytes);

            File.WriteAllBytes(file, bytesDecrypted);
        }

        /// <summary>
        /// Gets the random bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        private byte[] GetRandomBytes()
        {
            int saltLength = GetSaltLength();
            byte[] ba = new byte[saltLength];
            RNGCryptoServiceProvider.Create().GetBytes(ba);
            return ba;
        }

        /// <summary>
        /// Gets the length of the salt.
        /// </summary>
        /// <returns>System.Int32.</returns>
        private int GetSaltLength()
        {
            return 8;
        }

        /// <summary>
        /// Bytes the array to string.
        /// </summary>
        /// <param name="ba">The ba.</param>
        /// <returns>System.String.</returns>
        public string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (var b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        /// <summary>
        /// Strings to byte array.
        /// </summary>
        /// <param name="hex">The hexadecimal.</param>
        /// <returns>System.Byte[].</returns>
        public byte[] StringToByteArray(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }
}
