using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace United.Mobile.DataAccess.Common
{
    public class DataSecurity : IDataSecurity
    {
        private readonly IConfiguration _configuration;
        public DataSecurity(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public  Tuple<string, string> EncryptData(string plainText, string keyBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.GenerateIV();               
                // Create encryptor object
                ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                byte[] encryptedData;

                //Encryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(plainText);
                        }
                        encryptedData = ms.ToArray();
                    }
                }
                return Tuple.Create(Convert.ToBase64String(encryptedData), Convert.ToBase64String(aesAlgorithm.IV));
            }
        }

        public  string DecryptData(string cipherText, string keyBase64, string vectorBase64)
        {
            using (Aes aesAlgorithm = Aes.Create())
            {
                aesAlgorithm.Key = Convert.FromBase64String(keyBase64);
                aesAlgorithm.IV = Convert.FromBase64String(vectorBase64);
                // Create decryptor object
                 ICryptoTransform decryptor = aesAlgorithm.CreateDecryptor();
                byte[] cipher = Convert.FromBase64String(cipherText);
                //Decryption will be done in a memory stream through a CryptoStream object
                using (MemoryStream ms = new MemoryStream(cipher))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}
