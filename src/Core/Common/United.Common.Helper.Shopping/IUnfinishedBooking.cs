using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.SSR;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.UnfinishedBooking;
using United.Service.Presentation.ReferenceDataResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common;
using United.Services.FlightShopping.Common.DisplayCart;



namespace United.Common.Helper.Shopping
{
    public interface IUnfinishedBooking
    {
        string GetFlightShareMessage(MOBSHOPReservation reservation, string cabinType);
        bool IsOneFlexibleSegmentExist(List<MOBSHOPTrip> trips);
        Task<MOBResReservation> PopulateReservation(Session session, Service.Presentation.ReservationModel.Reservation reservation);
        void AssignMissingPropertiesfromRegisterFlightsResponse(United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReserationResponse, United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse registerFlightsResponse);
        ShopRequest BuildShopPinDownDetailsRequest(MOBSHOPSelectUnfinishedBookingRequest request, string cartId = "");
        Task<bool> SaveAnUnfinishedBooking(Session session, MOBRequest request, MOBSHOPUnfinishedBooking ub);
        Task<List<MOBSHOPUnfinishedBooking>> GetSavedUnfinishedBookingEntries(Session session, MOBRequest request, bool isCatalogOnForTravelerTypes = false);
        Task<bool> UpdateAnUnfinishedBooking(Session session, MOBRequest request, MOBSHOPUnfinishedBooking ubTobeUpdated);
        Task<United.Services.FlightShopping.Common.FlightReservation.FlightReservationResponse> GetShopPinDown(Session session, string appVersion, ShopRequest shopRequest);
        List<MOBSHOPPrice> GetPrices(List<DisplayPrice> displayPrices);
        //FareLock GetFareLockOptions(Service.Presentation.ProductResponseModel.ProductOffer cslFareLock, List<DisplayPrice> prices, request.SelectedUnfinishBooking?.CatalogItems, request.Application.Id, request.Application.Version.Major);
        ShopRequest BuildShopPinDownRequest(MOBSHOPUnfinishedBooking unfinishedBooking, string mpNumber, string languageCode, int appID = -1, string appVer = "", bool isCatalogOnForTravelerTypes = false);
        List<MOBSHOPTax> GetTaxAndFees(List<DisplayPrice> prices, int numPax, bool isReshopChange = false);
        List<Mobile.Model.Shopping.TravelOption> GetTravelOptions(DisplayCart displayCart);
        Task<MultiCallResponse> GetSpecialNeedsReferenceDataFromFlightShopping(Session session, int appId, string appVersion, string deviceId, string languageCode);
        Task<TravelSpecialNeeds> GetItineraryAvailableSpecialNeeds(Session session, int appId, string appVersion, string deviceId, IEnumerable<ReservationFlightSegment> segments, string languageCode,
            MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null);
    }
}