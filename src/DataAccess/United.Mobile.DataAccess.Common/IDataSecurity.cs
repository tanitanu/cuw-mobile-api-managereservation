using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.DataAccess.Common
{
    public interface IDataSecurity
    {
        Tuple<string, string> EncryptData(string plainText, string keyBase64);
        string DecryptData(string cipherText, string keyBase64, string vectorBase64);
    }
}
