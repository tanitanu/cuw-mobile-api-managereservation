using System.Threading.Tasks;

namespace United.Mobile.DataAccess.PreOrderMeals
{
    public interface IGetPNRByRecordLocatorService
    {
        Task<string> GetPNRByRecordLocator(string request, string transactionId, string path);
    }
}