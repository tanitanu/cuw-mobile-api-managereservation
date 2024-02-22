using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Common.Helper
{
    public interface ISSOTokenKeyHelper
    {
        Task<string> DecryptData(string encryptedData, MOBRequest mobRequest, string token);
        Task<string> EncryptData(string plainText, string token);
    }
}