using System.Threading.Tasks;
using United.Mobile.Model.Internal.Airports;

namespace United.Mobile.DataAccess.Airports
{
    public interface IAirportsService
    {
        Task<AirportsResponse> GetAirports(string token);
    }
}
