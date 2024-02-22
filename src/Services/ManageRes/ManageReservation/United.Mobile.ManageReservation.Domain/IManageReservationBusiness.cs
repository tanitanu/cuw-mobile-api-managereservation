using System.Threading.Tasks;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;

namespace United.Mobile.ManageReservation.Domain
{
    public interface IManageReservationBusiness
    {
        Task<MOBPNRByRecordLocatorResponse> GetPNRByRecordLocator(MOBPNRByRecordLocatorRequest request);
        Task<MOBPNRByRecordLocatorResponse> PerformInstantUpgrade(MOBInstantUpgradeRequest request);
        Task<MOBOneClickEnrollmentResponse> GetOneClickEnrollmentDetailsForPNR(MOBPNRByRecordLocatorRequest request);
        string GetTripDetailRedirect3dot0Url(string cn, string ln, string ac, int timestampvalidity = 0, string channel = "mobile",
            string languagecode = "en/US", string trips = "", string travelers = "", string ddate = "",
            string guid = "", bool isAward = false);
        Task<MOBMileageAndStatusOptionsResponse> GetMileageAndStatusOptions(MOBMileageAndStatusOptionsRequest request);
        Task<MOBReceiptByEmailResponse> RequestReceiptByEmail(MOBReceiptByEmailRequest request);
        Task<United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse> ConfirmScheduleChange(United.Mobile.Model.ReShop.MOBConfirmScheduleChangeRequest request);

        Task<MOBGetActionDetailsForOffersResponse> GetActionDetailsForOffers(MOBGetActionDetailsForOffersRequest request);
        Task<TravelOptionsResponse> GetProductOfferAndDetails(TravelOptionsRequest request);
        void PostBaggageEventMessage(dynamic request);
    }
}
