using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.FlightReservation;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;

namespace United.Common.Helper.FOP
{
    public interface IPaymentUtility
    {
        bool GetIsMilesFOPEnabled(MOBShoppingCart shoppingCart);
        string FirstLetterToUpperCase(string value);
        ULTripInfo GetUpliftTripInfo(ReservationDetail reservationDetail, string totalPrice, List<ProdDetail> products);
        bool IsCorporateTraveler(Collection<Characteristic> characteristics);
        List<MOBSHOPPrice> AddGrandTotalIfNotExist(List<MOBSHOPPrice> prices, double amount, string flow);
        MOBItem GetFareLockViewResPaymentCaptions(string id, string currentValue);
        Task<List<Section>> GetPaymentMessagesForWLPNRViewRes(FlightReservationResponse flightReservationResponse, Collection<ReservationFlightSegment> FlightSegments, string Flow);
        FormofPaymentOption UpliftAsFormOfPayment(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart);
        bool IsCheckFareLockUsingProductCode(MOBShoppingCart shoppingCart);
        bool isPCUUPPWaitListSegment(List<SubItem> PCUTravelOptions, Collection<ReservationFlightSegment> FlightSegments);
        List<SubItem> getPCUSegments(FlightReservationResponse flightReservationResponse, string Flow);
        List<Section> AssignRefundMessage(List<MOBItem> refundMessages);
        Task<List<MOBItem>> GetFareLockCaptions(United.Service.Presentation.ReservationModel.Reservation reservation, string email);
        string GetFlightShareMessageViewRes(Service.Presentation.ReservationModel.Reservation reservation);
        List<MOBMobileCMSContentMessages> getEtcBalanceAttentionConfirmationMessages(double? balanceAmount);
        void AssignTotalAndCertificateItemsToPrices(MOBShoppingCart shoppingcart);
        Task<MOBVormetricKeys> GetVormetricPersistentTokenForBooking(MOBCreditCard requestCreditCard, string sessionId, string token);
        Task<MOBVormetricKeys> GetVormetricPersistentTokenForViewRes(MOBCreditCard persistedCreditCard, string sessionId, string token);
        Task<MOBVormetricKeys> AssignPersistentTokenToCC(string accountNumberToken, string persistentToken, string securityCodeToken, string cardType, string sessionId, string action, int appId, string deviceID);
        double getPCUProductPrice(MOBShoppingCart shoppingcart);
        Task<MOBVormetricKeys> GetPersistentTokenUsingAccountNumberToken(string accountNumberToke, string sessionId, string token);
        bool IncludeTravelBankFOP(int appId, string appVersion);
        Task<double> GetTravelBankBalance(string sessionId);
        bool IncludeTravelCredit(int appId, string appVersion);
        Task<MOBCPTraveler> GetProfileOwnerTravelerCSL(string sessionID);
        Dictionary<string, string> GetAdditionalHeadersForMosaic(string flow);
        bool IsBundleFlow(string flow);
    }
}
