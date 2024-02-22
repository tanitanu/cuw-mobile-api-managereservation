using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerDataService
    {
        Task<(T response, long callDuration)> GetCustomerData<T>(string token, string sessionId, string jsonRequest);
        Task<T> InsertMPEnrollment<T>(string token, string request, string sessionId, string path);
        Task<string> GetOnlyEmpID(string token, string requestData, string sessionId, string path = "");
        Task<(T response, long callDuration)> GetMileagePluses<T>(string token, string sessionId, string jsonRequest);
        Task<T> GetProfile<T>(string token, string request, string sessionId, string path);
        Task<T> InsertTraveler<T>(string token, string request, string sessionId);
    }
}
