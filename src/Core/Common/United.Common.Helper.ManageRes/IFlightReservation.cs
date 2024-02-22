using System.Collections.ObjectModel;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Service.Presentation.ReservationRequestModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using Reservation = United.Service.Presentation.ReservationModel.Reservation;


namespace United.Common.Helper.ManageRes
{
    public interface IFlightReservation
    {
        Task<(MOBPNR pnr, ReservationDetail response)> GetPNRByRecordLocatorFromCSL(string transactionId, string deviceId, string recordLocator, string lastName, string languageCode, int applicationId, string appVersion, bool forWallet, Session session, ReservationDetail response, bool isOTFConversion = false, string mpNumber = "");
        Task<SeatOffer> GetSeatOffer_CFOP(MOBPNR pnr, MOBRequest request, Reservation cslReservation, string token, string flowType, string sessionId, Session session);
        Task<MOBIRROPSChange> ValidateIRROPSStatus(MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response,
           ReservationDetail cslReservationDetail, Session session);
        string GetTravelType(Collection<ReservationFlightSegment> flightSegments);

        Task<string> RetrievePnrDetailsFromCsl(int applicationId, string TransactionId, RetrievePNRSummaryRequest request);
        string GetCharactersticValue(System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Characteristic> Characteristic, string code);
        Task<MOBPNRSegment> GetPnrSegment(string languageCode, string appVersion, int applicationId, ReservationFlightSegment segment, int lowestEliteLevel);
        void GetPassengerDetails
         (MOBPNR pnr, ReservationDetail response, ref bool isSpaceAvailblePassRider, ref bool isPositiveSpace, int applicationId = 0, string appVersion = "");
        MOBPNRAdvisory PopulateConfigContent(string displaycontent, string splitchar);
        Task<MOBFutureFlightCredit> GetFutureFlightCreditMessages(int applicationId, string appVersion);
    }
}
