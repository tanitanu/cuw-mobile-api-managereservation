using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Payment
{
    public interface IGenerateHashService
    {
        Task<string> GenerateHash(string token, string sessionId);
    }
}
