using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Services.FlightShopping.Common.FlightReservation;

namespace United.Common.Helper.Shopping
{
    public interface IShoppingBuyMiles
    {
        void UpdatePricesForBuyMiles(List<MOBSHOPPrice> displayPrices, FlightReservationResponse shopBookingDetailsResponse, List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> displayFees = null);

        bool IsBuyMilesFeatureEnabled(int appId, string version, List<MOBItem> catalogItems = null, bool isNotSelectTripCall = false);
        void UpdateGrandTotal(MOBSHOPReservation reservation, bool isCommonMethod = false);
    }
}
