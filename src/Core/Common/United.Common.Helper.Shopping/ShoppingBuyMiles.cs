using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Services.FlightShopping.Common;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using MOBSHOPPrice = United.Mobile.Model.Shopping.MOBSHOPPrice;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using Session = United.Mobile.Model.Common.Session;


namespace United.Common.Helper.Shopping
{
    public class ShoppingBuyMiles : IShoppingBuyMiles
    {
        private readonly IConfiguration _configuration;

        public ShoppingBuyMiles(IConfiguration configuration
           )
        {
            _configuration = configuration;
        }


        public void UpdatePricesForBuyMiles(List<MOBSHOPPrice> displayPrices, FlightReservationResponse shopBookingDetailsResponse, List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> displayFees = null)
        {
            if (displayFees?.Where(a => a.Type == "MPF") != null)
            {
                UpdateGrandTotalBuyMiles(displayPrices, displayFees);
                UpdateTaxPriceTypeDescriptionForBuyMiles(displayPrices);
                AddFarewayDescriptionForMultipaxForBuyMiles(displayPrices);
            }
        }

        private void AddFarewayDescriptionForMultipaxForBuyMiles(List<MOBSHOPPrice> displayPrices)
        {
            var miles = displayPrices.FirstOrDefault(a => a.DisplayType == "MILES");
            if (miles != null && displayPrices?.Count > 0)
            {
                MOBSHOPPrice travelrPriceMPF = new MOBSHOPPrice();
                travelrPriceMPF.DisplayType = "TRAVELERPRICE_MPF";
                travelrPriceMPF.CurrencyCode = miles.CurrencyCode;
                travelrPriceMPF.DisplayValue = miles.DisplayValue;
                travelrPriceMPF.Value = miles.Value;
                travelrPriceMPF.PaxTypeCode = miles.PaxTypeCode;
                travelrPriceMPF.PriceTypeDescription = "Fare";
                travelrPriceMPF.FormattedDisplayValue = miles.FormattedDisplayValue;
                travelrPriceMPF.PaxTypeDescription = miles.PaxTypeDescription;
                displayPrices.Add(travelrPriceMPF);
            }
        }

        private void UpdateTaxPriceTypeDescriptionForBuyMiles(List<MOBSHOPPrice> displayPrices)
        {
            var mpfIndex = displayPrices.FindIndex(a => a.DisplayType == "TAX");
            if (mpfIndex >= 0)
                displayPrices[mpfIndex].PriceTypeDescription = "Taxes and fees";
        }


        private void UpdateGrandTotalBuyMiles(List<MOBSHOPPrice> displayPrices, List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> displayFees = null, bool isCommonMethod = false)
        {
            var grandTotalIndex = displayPrices.FindIndex(a => a.DisplayType == "GRAND TOTAL");
            if (grandTotalIndex >= 0)
            {
                double extraMilePurchaseAmount = (displayFees?.Where(a => a.Type == "MPF")?.FirstOrDefault()?.Amount != null) ?
                                         Convert.ToDouble(displayFees?.Where(a => a.Type == "MPF")?.FirstOrDefault()?.Amount) : 0;
                string priceTypeDescription = displayFees?.Where(a => a.Type == "MPF")?.FirstOrDefault()?.Description;
                if (extraMilePurchaseAmount > 0 && (priceTypeDescription == null || priceTypeDescription?.Contains("Additional") == false))
                {
                    displayPrices[grandTotalIndex].Value += extraMilePurchaseAmount;
                    CultureInfo ci = null;
                    ci = TopHelper.GetCultureInfo(displayPrices[grandTotalIndex].CurrencyCode);
                    displayPrices[grandTotalIndex].DisplayValue = displayPrices[grandTotalIndex].Value.ToString();
                    displayPrices[grandTotalIndex].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(displayPrices[grandTotalIndex].Value.ToString(), ci, false);
                }
            }
        }

        public bool IsBuyMilesFeatureEnabled(int appId, string version, List<MOBItem> catalogItems = null, bool isNotSelectTripCall = false)
        {
            if (_configuration.GetValue<bool>("EnableBuyMilesFeature") == false) return false;
            if ((catalogItems != null && catalogItems.Count > 0 &&
                   catalogItems.FirstOrDefault(a => a.Id == _configuration.GetValue<string>("Android_EnableBuyMilesFeatureCatalogID") || a.Id == _configuration.GetValue<string>("iOS_EnableBuyMilesFeatureCatalogID"))?.CurrentValue == "1")
                   || isNotSelectTripCall)
                return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_BuyMilesFeatureSupported_AppVersion"), _configuration.GetValue<string>("IPhone_BuyMilesFeatureSupported_AppVersion"));
            else
                return false;

        }

        public void UpdateGrandTotal(MOBSHOPReservation reservation, bool isCommonMethod = false)
        {
            var grandTotalIndex = reservation.Prices.FindIndex(a => a.PriceType == "GRAND TOTAL");
            if (grandTotalIndex >= 0)
            {
                double extraMilePurchaseAmount = (reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value != null) ?
                                         Convert.ToDouble(reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.Value) : 0;
                string priceTypeDescription = reservation?.Prices?.Where(a => a.DisplayType == "MPF")?.FirstOrDefault()?.PriceTypeDescription;
                if (extraMilePurchaseAmount > 0 && (priceTypeDescription == null || priceTypeDescription?.Contains("Additional") == false))
                {
                    reservation.Prices[grandTotalIndex].Value += extraMilePurchaseAmount;
                    CultureInfo ci = null;
                    ci = TopHelper.GetCultureInfo(reservation?.Prices[grandTotalIndex].CurrencyCode);
                    reservation.Prices[grandTotalIndex].DisplayValue = reservation.Prices[grandTotalIndex].Value.ToString();
                    reservation.Prices[grandTotalIndex].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(reservation?.Prices[grandTotalIndex].Value.ToString(), ci, false);
                }
            }
        }

    }
}
