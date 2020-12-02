using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Business.Classes
{
    public static class Encript
    {
        public static string _key = string.Empty;

        public static string Encrypt(string text)
        {
            byte[] Results;
            MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
            byte[] TDESKey = HashProvider.ComputeHash(new UTF8Encoding().GetBytes(_key));
            TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = TDESKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };

            byte[] DataToEncrypt = new UTF8Encoding().GetBytes(text);

            try
            {
                ICryptoTransform Encryptor = TDESAlgorithm.CreateEncryptor();
                Results = Encryptor.TransformFinalBlock(DataToEncrypt, 0, DataToEncrypt.Length);
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }

            return Convert.ToBase64String(Results);
        }

        public static string Decrypt(string textCript)
        {
            byte[] Results;

            try
            {
                MD5CryptoServiceProvider HashProvider = new MD5CryptoServiceProvider();
                TripleDESCryptoServiceProvider TDESAlgorithm = new TripleDESCryptoServiceProvider
                {
                    Key = HashProvider.ComputeHash(new UTF8Encoding().GetBytes(_key)),
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                };

                byte[] DataToDecrypt = Convert.FromBase64String(textCript);
                try
                {
                    ICryptoTransform Decryptor = TDESAlgorithm.CreateDecryptor();
                    Results = Decryptor.TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length);
                }
                finally
                {
                    TDESAlgorithm.Clear();
                    HashProvider.Clear();
                }
            }
            catch (Exception er)
            {
                throw er;
            }

            return new UTF8Encoding().GetString(Results);
        }

        public static string HashValue(string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);

            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
            {
                hashedInputStringBuilder.Append(b.ToString("X2"));
            }

            return hashedInputStringBuilder.ToString();
        }
    }
}
