using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerSearchService
    {
        Task<string> Search(string token, string sessionId, string path = "");
    }
}
