using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.Shopping;
using United.Services.FlightShopping.Common.FlightReservation;
using MOBItem = United.Mobile.Model.Common.MOBItem;
using MOBMobileCMSContentMessages = United.Mobile.Model.Common.MOBMobileCMSContentMessages;
using SeatChangeState = United.Mobile.Model.Shopping.SeatChangeState;

namespace United.Common.Helper.Merchandize
{
    public interface IProductInfoHelper
    {
        Task<List<ProdDetail>> ConfirmationPageProductInfo(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isPost, MOBApplication application, SeatChangeState state = null, string flow = "VIEWRES", string sessionId = "");
        Task<List<MOBMobileCMSContentMessages>> GetProductBasedTermAndConditions(string sessionId, United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isPost);
        Task<List<MOBItem>> GetCaptions(string key);
        void AddCouponDetails(List<ProdDetail> prodDetails, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse cslFlightReservationResponse, bool isPost, string flow, MOBApplication application);
        bool IsEnableOmniCartMVP2Changes(int applicationId, string appVersion, bool isDisplayCart);

        bool IsBundleProductSelected(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse);
        MOBItem AddBundleCaptionForQMEvent(FlightReservationResponse flightReservationResponse, List<ProdDetail> products);
    }
}
