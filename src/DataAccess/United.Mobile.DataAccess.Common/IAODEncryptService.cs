using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IAODEncryptService
    {
        Task<string> GetAODEncryptedString(string jsonString);
    }
}
