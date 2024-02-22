using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightSearchResult
{
    public interface IFlightSearchResultService
    {
        Task<string> GetFlightSearchResult(string token,string endPoint, string requestData, string sessonId);
    }
}
