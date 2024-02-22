using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using Seat = United.Mobile.Model.Shopping.Misc.Seat;

namespace United.Common.HelperSeatEngine
{
    public interface ISeatEngine
    {
        Task<List<string>> GetPolarisCabinBranding(string authenticationToken, string flightNumber, string departureAirportCode, string flightDate, string arrivalAirportCode, string cabinCount, string languageCode, string sessionId, string operatingCarrier = "UA", string marketingCarrier = "UA");
        Task<List<MOBSeatMap>> GetSeatMapDetail(string sessionId, string destination, string origin, int applicationId, string appVersion, string deviceId, bool returnPolarisLegendforSeatMap);
        Task<bool> IsEnablePCUSeatMapPurchaseManageRes(int appId, string appVersion, int numberOfTravelers, List<United.Mobile.Model.Common.MOBItem> catalogs = null);
        void EconomySeatsForBUSService(MOBSeatMap seats, bool operated = false);
        Task<string> ShowNoFreeSeatsAvailableMessage(int noOfTravelersWithoutSeat, int noOfFreeEplusEligibleRemaining, int noOfFreeSeats, int noOfPricedSeats, bool isBasicEconomy);
        bool IsPreferredSeatProgramCode(string program);
        bool ShowSeatMapForCarriers(string operatingCarrier);
        bool IsInChecKInWindow(string departTimeString);
        Task<(string jsonResponse, string token)> GetPnrDetailsFromCSL(string transactionId, string recordLocator, string lastName, int applicationId, string appVersion, string actionName, string token, bool usedRecall = false);

        List<MOBSeatMap> GetSeatMapWithPreAssignedSeats(List<MOBSeatMap> seatMap, List<Seat> existingSeats, bool isPreferredZoneEnabled);

        public List<MOBBKTraveler> GetBookingTravelerInfoWithSeatLocation(List<MOBSeatMap> seatMap, List<MOBBKTraveler> bookingTravelerInfo);

        string GetOperatedByText(string marketingCarrier, string flightNumber, string operatingCarrierDescription);
        Task<Mobile.Model.MPRewards.SeatEngine> GetFlightReservationCSL_CFOP(MOBSeatChangeInitializeRequest request, Mobile.Model.MPRewards.SeatEngine seatEngine, bool isVerticalSeatMapEnabled);
        int GetNoOfFreeEplusEligibleRemaining(List<MOBBKTraveler> travelers, string orgin, string destination, int totalEplusEligible, bool isElf, out int noOfTravelersWithNoSeat);
        void CheckSegmentToRaiseExceptionForElf(List<TripSegment> segments);
        Task<(int, MOBSeatChangeInitializeResponse response, int ePlusSubscriberCount)> PopulateEPlusSubscriberSeatMessage(MOBSeatChangeInitializeResponse response, int applicationID, string sessionID, int ePlusSubscriberCount, bool isVerticalSeatMapEnabled, bool isEnablePreferredZoneSubscriptionMessages = false);
        int GetNoFreeSeatCompanionCount(List<MOBBKTraveler> travelers, List<United.Mobile.Model.Shopping.Booking.MOBBKTrip> trips);
        Task<(int, MOBSeatChangeInitializeResponse response, int ePlusSubscriberCount, bool hasEliteAboveGold, bool doNotShowEPlusSubscriptionMessage, bool showEPUSubscriptionMessage)> PopulateEPlusSubscriberAndMPMemeberSeatMessage(MOBSeatChangeInitializeResponse response, int applicationID, string sessionID, int ePlusSubscriberCount, bool hasEliteAboveGold, bool doNotShowEPlusSubscriptionMessage, bool showEPUSubscriptionMessage);
        void PopulateEPAEPlusSeatMessage(ref MOBSeatChangeInitializeResponse response, int noFreeSeatCompanionCount, ref bool doNotShowEPlusSubscriptionMessage);
        bool IsMatchedFlight(TripSegment segment, OfferRequestData flightDetails, List<TripSegment> segments);
        bool HasEconomySegment(List<United.Mobile.Model.Shopping.Booking.MOBBKTrip> trips);
        bool ValidateResponse(MOBSeatChangeInitializeResponse response);
        Task<string> GetPolarisSeatMapLegendId(string from, string to, int numberOfCabins, List<string> polarisCabinBrandingDescriptions, int applicationId = -1, string appVersion = "", bool isBERecommendedSeatsAvailable = false);
        bool IsPreferredSeat(string DisplaySeatType, string program);
        bool IsMatchedFlight(TripSegment segment, MOBSeatFocus seatFocusSegment, List<TripSegment> segments);
        Task<string> ShowNoFreeSeatsAvailableMessage(United.Mobile.Model.Shopping.Reservation persistedReservation, int noOfFreeSeats, int noOfPricedSeats, bool isInCheckInWindow, bool isFirstSegment);
        Task<bool> HasPCUOfferState(MOBSeatChangeInitializeRequest request);
        TravelOptionsRequest BuildTravelOptionsRequest(MOBSeatChangeInitializeRequest request);
        bool SupressLMX(int appId);
        void CountNoOfFreeSeatsAndPricedSeats(MOBSeatB seat, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats);

        /* DeadCode Removed
        Task<List<MOBSeatMap>> GetSeatMapForRecordLocatorWithLastNameCSL(string sessionId,
          string recordLocator, int segmentIndex, string languageCode, string bookingCabin,
          bool cogStop, string origin, string flightnumber, string MarketingCarrier, string OperatingCarrier,
          string flightdate, string destination, string appVersion, bool isOaSeatMapSegment, bool isBasicEconomy,
          int noOfTravelersWithNoSeat1, int noOfFreeEplusEligibleRemaining, List<TripSegment> tripSegments, string deviceId, int applicationId = -1, bool returnPolarisLegendforSeatMap = false);
        */
    }
}
