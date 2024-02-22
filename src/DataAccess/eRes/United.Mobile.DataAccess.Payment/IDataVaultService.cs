using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Payment
{
    public interface IDataVaultService
    {
        Task<string> GetDataVault(string token, string requestData, string sessionId);

    }
}
