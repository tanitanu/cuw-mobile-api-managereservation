using System.Threading.Tasks;
using United.Mobile.Model.GooglePay;

namespace United.Mobile.Services.GooglePay.Domain
{
    public interface IGooglePayBusiness
    {
        Task<MOBGooglePayFlightResponse> InsertFlight(MOBGooglePayFlightRequest request);
        Task<MOBGooglePayFlightResponse> UpdateFlightFromRequest(MOBGooglePayFlightRequest request);
    }
}
