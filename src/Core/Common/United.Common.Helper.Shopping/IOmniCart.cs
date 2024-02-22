using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Bundles;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.FlightReservation;
using Reservation = United.Mobile.Model.Shopping.Reservation;

namespace United.Common.Helper.Shopping
{
    public interface IOmniCart
    {
        bool IsEnableOmniCartMVP2Changes(int applicationId, string appVersion, bool isDisplayCart);
        void BuildCartTotalPrice(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation);     
        Task<(bool returnValue, BookingBundlesResponse bundleResponse)> IsOmniCartFlow_BundlesAlreadyLoaded(BookingBundlesResponse bundleResponse, Reservation persistedReservation, MOBRequest request, string sessionId);
        bool IsEnableOmniCartReleaseCandidateTwoChanges_Bundles(int applicationId, string appVersion);
        bool IsEnableOmniCartReleaseCandidateThreeChanges_Seats(int applicationId, string appVersion);
        bool IsEnableOmniCartHomeScreenChanges(int applicationId, string appVersion);
        void RemoveProductOfferIfAlreadyExists(GetOffers productOffers, string product);
        Task<FlightReservationResponse> GetFlightReservationResponseByCartId(string sessionId, string cartId);
        List<MOBTravelerType> GetMOBTravelerTypes(List<DisplayTraveler> displayTravelers);
        bool IsClearAllSavedTrips(bool isSavedTrip, int appId, String appVersion, bool isRemoveAll);
        bool IsEnableOmniCartReleaseCandidate4CChanges(int applicationId, string appVersion, List<MOBItem> catalogItems = null);
        string GetCCEOmnicartRepricingCharacteristicValue(int applicationId, string appVersion);
        void BuildOmniCart(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation);
        Task<bool> IsNonUSPointOfSale(string sessionId, string cartId, Mobile.Model.TripPlannerGetService.MOBSHOPSelectTripResponse response);
        bool IsOmniCartSavedTripAndNavigateToFinalRTI(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart);
    }
}

