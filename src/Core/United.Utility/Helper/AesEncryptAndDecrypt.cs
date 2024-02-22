using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace United.Utility.Helper
{
    public class AesEncryptAndDecrypt
    {
        private static byte[] _secretKey;
        private static byte[] _iv;

        public AesEncryptAndDecrypt()
        {
            string aesKey = "8080808080808080";

            if (aesKey.Length > 16)
                aesKey = aesKey.Substring(0, 16);
            if (aesKey.Length > 16)
                aesKey = aesKey.Substring(0, 16);
            if (aesKey.Length < 16)
                aesKey = aesKey.PadRight(16);
            _secretKey = Encoding.UTF8.GetBytes(aesKey);
            _iv = Encoding.UTF8.GetBytes(aesKey);
        }
        public string Encrypt(string employeeId)
        {
            byte[] cipherText;
            using (Aes aes = Aes.Create())
            {
                aes.Key = _secretKey;
                aes.IV = _iv;
                //aes.KeySize = 128;
                //aes.Padding = PaddingMode.PKCS7;
                //aes.Mode = CipherMode.CBC;               

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(employeeId);
                        }
                    }
                    cipherText = ms.ToArray();

                }
            }
            return Convert.ToBase64String(cipherText).TrimStart().TrimEnd();
        }

        public string Decrypt(string encryptedEmployeeId)
        {

            byte[] cipherText = Convert.FromBase64String(encryptedEmployeeId);
            string plainText;
            using (Aes aes = Aes.Create())
            {
                aes.Key = _secretKey;
                aes.IV = _iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plainText = sr.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }
    }
}
