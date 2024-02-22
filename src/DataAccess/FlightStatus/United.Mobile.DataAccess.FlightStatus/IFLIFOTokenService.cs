using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightStatus
{
    public interface IFLIFOTokenService
    {
        Task<string> GetFLIFOTokenService(string token, string sessionId);
    }
}
