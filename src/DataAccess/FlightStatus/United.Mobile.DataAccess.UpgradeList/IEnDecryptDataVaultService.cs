using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UpgradeList
{
    public interface IEnDecryptDataVaultService
    {
        Task<string> GetDecryptedTextFromDataVault(string token, string sessionId, string jsonRequest);
    }
}
