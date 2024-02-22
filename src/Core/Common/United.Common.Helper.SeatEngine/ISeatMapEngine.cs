using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using MOBSeatMap = United.Mobile.Model.Shopping.MOBSeatMap;

namespace United.Common.HelperSeatEngine
{
    public interface ISeatMapEngine
    {
        //Task<List<MOBSeatMap>> GetCSL30SeatMapForRecordLocatorWithLastName(string sessionId, string recordLocator, int segmentIndex, string languageCode, string bookingCabin, string lastName, bool cogStop, string origin, string destination, int applicationId, string appVersion, bool isELF, bool isIBE, int noOfTravelersWithNoSeat1, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, List<TripSegment> tripSegments, string operatingCarrierCode, string deviceId, List<MOBBKTraveler> BookingTravelerInfo, string flow, bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false, bool isSeatFocusEnabled = false, List<MOBItem> catalog = null);
        //void GetOANoSeatAvailableMessage(List<TripSegment> segments);
        //void GetInterlineRedirectLink(List<TripSegment> segments, string pointOfSale, MOBRequest mobRequest, string recordLocator, string lastname, List<MOBItem> catalog);
        //bool SupressLMX(int appId);
        //string GetFareBasicCodeFromBundles(List<TravelOption> travelOptions, int tripId, string defaultFareBasisCode, string destination, string origin);
        //bool ShowSeatMapForCarriers(string operatingCarrier);
        //Task<string> ShowOaSeatMapAvailabilityDisclaimerText();
        //void EconomySeatsForBUSService(Mobile.Model.Shopping.MOBSeatMap seats, bool operated = false);
        //bool IsCabinMatchedCSL(string pcuCabin, string seatmapCabin);
        //Task<List<MOBItem>> GetPcuCaptions(string travelerNames, string recordLocator);
        //void CountNoOfFreeSeatsAndPricedSeats(MOBSeatB seat, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats);
        //string GetOperatedByText(string marketingCarrier, string flightNumber, string operatingCarrierDescription);
        //Task<string> GetDocumentTextFromDataBase(string title);
    }
}
