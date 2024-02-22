using System.Threading.Tasks;

namespace United.Mobile.DataAccess.InFlightAmenity
{
    public interface IAircraftAmenitiesService
    {
        Task<string> GetAircraftAmenities(string token, string sessionId, string flightNumber, string legDepartureDate, string legDepartureStation, string legArrivalStation, string shipNumber, string equipmentCode);
    }
}