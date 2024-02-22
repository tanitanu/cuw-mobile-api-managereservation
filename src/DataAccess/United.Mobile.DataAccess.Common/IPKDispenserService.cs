using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IPKDispenserService
    {
        Task<T> GetPkDispenserPublicKey<T>(string token, string sessionId, string path);
    }
}
