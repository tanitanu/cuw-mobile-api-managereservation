using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Payment
{
    public interface IFlightClassService
    {
        Task<string> InsertFlightClass(string token, string request, string sessionId, string path);

        Task<string> UpdateFlightClass(string token, string request, string sessionId, string path);
    }
}
