using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Bundles;
using United.Mobile.Model.ShopTrips;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using United.Persist;
using United.Mobile.Model.Shopping.Misc;
using System.Threading.Tasks;

namespace United.Common.Helper.Shopping
{
    public class OmniCart : IOmniCart
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<OmniCart> _logger;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IShoppingUtility _shoppingUtility;
        public OmniCart(IConfiguration configuration
            , ICacheLog<OmniCart> logger
            , ISessionHelperService sessionHelperService
            , IShoppingUtility shoppingUtility)
        {
            _configuration = configuration;
            _logger = logger;
            _sessionHelperService = sessionHelperService;
            _shoppingUtility = shoppingUtility;
        }
        #region OmniCart MVP2 Changes
        public int GetCartItemsCount(MOBShoppingCart shoppingcart)
        {
            int itemsCount = 0;
            if (shoppingcart?.Products != null && shoppingcart.Products.Count > 0)
            {
                shoppingcart.Products.ForEach(product =>
                {
                    if (!string.IsNullOrEmpty(product.ProdTotalPrice) && Decimal.TryParse(product.ProdTotalPrice, out decimal totalprice) && totalprice > 0)
                    {
                        if (product?.Segments != null && product.Segments.Count > 0)
                        {
                            product.Segments.ForEach(segment =>
                            {
                                segment.SubSegmentDetails.ForEach(subSegment =>
                                {
                                    if (subSegment != null)
                                    {
                                        if (product.Code == "SEATASSIGNMENTS")
                                        {
                                            itemsCount += subSegment.PaxDetails.Count();
                                        }
                                        else
                                        {
                                            itemsCount += 1;
                                        }
                                    }
                                });

                            });
                            return;
                        }
                        itemsCount += 1;
                    }
                });
            }
            return itemsCount;
        }
      
        public MOBItem GetTotalPrice(List<ProdDetail> products, MOBSHOPReservation reservation)
        {
            if (products != null && reservation != null)
            {
                return new MOBItem
                {
                    Id = IsFarelock(products) ? _configuration.GetValue<string>("FarelockTotalPriceLabelText") : _configuration.GetValue<string>("TotalPriceLabelText")
                ,
                    CurrentValue = IsFarelock(products) ? GetFareLockPrice(products) : GetGrandTotalPrice(reservation)
                };
            }
            return null;
        }

        public string GetGrandTotalPrice(MOBSHOPReservation reservation)
        {
            if (reservation?.Prices != null)
            {
                var grandTotalPrice = reservation.Prices.Exists(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL"))
                                ? reservation.Prices.Where(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL")).First()
                                : reservation.Prices.Where(p => p.DisplayType.ToUpper().Equals("TOTAL")).First();
                if (_configuration.GetValue<bool>("EnableLivecartForAwardTravel") && reservation.AwardTravel)
                {
                    var totalDue = string.Empty;
                    var awardPrice = reservation.Prices.FirstOrDefault(p => string.Equals("miles", p.DisplayType, StringComparison.OrdinalIgnoreCase));
                    if (awardPrice != null)
                    {
                        totalDue = FormatedMilesValueAndText(awardPrice.Value);
                    }
                    if (grandTotalPrice != null)
                    {
                        totalDue = string.IsNullOrWhiteSpace(totalDue)
                                    ? grandTotalPrice.FormattedDisplayValue
                                    : $"{totalDue} + {grandTotalPrice.FormattedDisplayValue}";
                    }
                    return totalDue;
                }
                else
                {
                    if (grandTotalPrice != null)
                    {
                        return grandTotalPrice.FormattedDisplayValue;
                    }
                }
            }
            return string.Empty;
        }

        private static string FormatedMilesValueAndText(double milesValue)
        {
            if (milesValue >= 1000)
                return (milesValue / 1000D).ToString("0.#" + "K miles");
            else if (milesValue > 0)
                return milesValue.ToString("0,# miles");
            else
                return string.Empty;
        }

        public bool IsFarelock(List<ProdDetail> products)
        {
            if (products != null)
            {
                if (products.Any(p => p.Code.ToUpper() == "FARELOCK" || p.Code.ToUpper() == "FLK"))
                {
                    return true;
                }
            }
            return false;
        }

        public string GetFareLockPrice(List<ProdDetail> products)
        {
            return products.Where(p => p.Code.ToUpper() == "FARELOCK" || p.Code.ToUpper() == "FLK").First().ProdDisplayTotalPrice;
        }

        public void BuildCartTotalPrice(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation)
        {
            if (shoppingCart.OmniCart == null)
            {
                shoppingCart.OmniCart = new Cart();
            }
            shoppingCart.OmniCart.CartItemsCount = GetCartItemsCount(shoppingCart);
            shoppingCart.OmniCart.TotalPrice = GetTotalPrice(shoppingCart?.Products, reservation);
            shoppingCart.OmniCart.PayLaterPrice = GetPayLaterAmount(shoppingCart?.Products, reservation);
            //TODO
            shoppingCart.OmniCart.FOPDetails = GetFOPDetails(reservation);

            AssignUpliftText(shoppingCart, reservation);                //Assign message text and link text to the Uplift
        }

        public MOBItem GetPayLaterAmount(List<ProdDetail> products, MOBSHOPReservation reservation)
        {
            if (products != null && reservation != null)
            {
                if (IsFarelock(products))
                {
                    return new MOBItem { Id = _configuration.GetValue<string>("PayDueLaterLabelText"), CurrentValue = GetGrandTotalPrice(reservation) };
                }
            }
            return null;
        }

        private  List<MOBSection> GetFOPDetails(MOBSHOPReservation reservation)
        {
            var mobSection = default(MOBSection);
            if (reservation?.Prices?.Count > 0)
            {
                var travelCredit = reservation.Prices.FirstOrDefault(price => new[] { "TB", "CERTIFICATE", "FFC" }.Any(credit => string.Equals(price.PriceType, credit, StringComparison.OrdinalIgnoreCase)));
                if (travelCredit != null)
                {
                    if (string.Equals(travelCredit.PriceType, "TB", StringComparison.OrdinalIgnoreCase))
                    {
                        mobSection = new MOBSection();
                        mobSection.Text1 = !string.IsNullOrEmpty(_configuration.GetValue<string>("UnitedTravelBankCashLabelText")) ? _configuration.GetValue<string>("UnitedTravelBankCashLabelText") : "United TravelBank cash";
                        mobSection.Text2 = !string.IsNullOrEmpty(_configuration.GetValue<string>("TravelBankCashAppliedLabelText")) ? _configuration.GetValue<string>("TravelBankCashAppliedLabelText") : "TravelBank cash applied";
                        mobSection.Text3 = travelCredit.FormattedDisplayValue;
                    }
                    else
                    {
                        mobSection = new MOBSection();
                        mobSection.Text1 = !string.IsNullOrEmpty(_configuration.GetValue<string>("TravelCreditsLabelText")) ? _configuration.GetValue<string>("TravelCreditsLabelText") : "Travel credits";
                        mobSection.Text2 = !string.IsNullOrEmpty(_configuration.GetValue<string>("CreditKeyLabelText")) ? _configuration.GetValue<string>("CreditKeyLabelText") : "Credit";
                        mobSection.Text3 = travelCredit.FormattedDisplayValue;

                    }
                }

            }
            return mobSection != null ? new List<MOBSection> { mobSection } : null;
        }

        private void AssignUpliftText(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation)
        {                    

            //if (_shoppingUtility.IsEligibileForUplift(reservation, shoppingCart) && Shoppingcart?.Form)                //Check if eligible for Uplift
            if (_shoppingUtility.IsEligibileForUplift(reservation, shoppingCart) && shoppingCart?.FormofPaymentDetails?.MoneyPlusMilesCredit?.SelectedMoneyPlusMiles == null) //Check if eligible for Uplift
            {
               shoppingCart.OmniCart.IsUpliftEligible = true;      //Set property to true, if Uplift is eligible                
            }
            else //Set Uplift properties to false / empty as Uplift isn't eligible
            {
               shoppingCart.OmniCart.IsUpliftEligible = false;                
            }
        }


        public bool IsEnableOmniCartMVP2Changes(int applicationId, string appVersion, bool isDisplayCart)
        {
            if (_configuration.GetValue<bool>("EnableOmniCartMVP2Changes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartMVP2Changes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartMVP2Changes_AppVersion" )) && isDisplayCart)
            {
                return true;
            }
            return false;
        }
        #endregion OmniCart MVP2 Changes 
        public async Task<(bool returnValue, BookingBundlesResponse bundleResponse)> IsOmniCartFlow_BundlesAlreadyLoaded(BookingBundlesResponse bundleResponse, Reservation persistedReservation, MOBRequest request, string sessionId)
        {
            if (IsEnableOmniCartReleaseCandidateTwoChanges_Bundles(request.Application.Id, request.Application.Version.Major) && persistedReservation?.ShopReservationInfo2?.IsOmniCartSavedTripFlow == true)
            {
                bundleResponse = await _sessionHelperService.GetSession<BookingBundlesResponse>(sessionId, bundleResponse.ObjectName, new List<string>() { sessionId, bundleResponse.ObjectName }).ConfigureAwait(false);
                var isPersistNotNull = bundleResponse != null;
                bundleResponse = bundleResponse ?? new BookingBundlesResponse(_configuration);
                return (isPersistNotNull, bundleResponse);
            }
            return (false,bundleResponse);
        }

        public bool IsEnableOmniCartReleaseCandidateTwoChanges_Bundles(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOmniCartReleaseCandidateTwoChanges_Bundles") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartReleaseCandidateTwoChanges_Bundles_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartReleaseCandidateTwoChanges_Bundles_AppVersion"));
        }

        public bool IsEnableOmniCartReleaseCandidateThreeChanges_Seats(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOmniCartReleaseCandidateThreeChanges_Seats") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartReleaseCandidateThreeChanges_Seats_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartReleaseCandidateThreeChanges_Seats_AppVersion"));
        }
        public bool IsEnableOmniCartHomeScreenChanges(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOmniCartHomeScreenChanges") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartHomeScreenChanges_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartHomeScreenChanges_AppVersion"));
        }
        public void RemoveProductOfferIfAlreadyExists(GetOffers productOffers, string product)
        {
            if (product == "Bundle")
            {
                //If it is bundles remove all the bundle products in the offers.
                if (productOffers?.Offers != null)
                {
                    var products = productOffers.Offers.First()
                          .ProductInformation
                          .ProductDetails
                          .Where(productDetail =>
                                     productDetail?.Product?.SubProducts != null
                                     && productDetail.Product.SubProducts.Any(subProduct => subProduct.GroupCode == "BE"));//Get all products with groupCode as BE (This would be Be only for bundle products)
                    if (products != null)
                    {
                        foreach (var bundleProduct in products.ToList())
                        {
                            productOffers.Offers.First().ProductInformation.ProductDetails.Remove(bundleProduct);
                        }

                    }
                }
            }

        }
        public  List<MOBTravelerType> GetMOBTravelerTypes(List<DisplayTraveler> displayTravelers)
        {
            return displayTravelers?.Select(t => new MOBTravelerType()
            {
                Count = t.TravelerCount,
                TravelerType = ShopStaticUtility.GetTType(t.PaxTypeCode).ToString()
            }).ToList();

        }
        public async Task<FlightReservationResponse> GetFlightReservationResponseByCartId(string sessionId, string cartId)
        {
            var persistData =await _sessionHelperService.GetSession<List<CCEFlightReservationResponseByCartId>>(sessionId, new CCEFlightReservationResponseByCartId().ObjectName, new List<string> { sessionId, new CCEFlightReservationResponseByCartId().ObjectName }).ConfigureAwait(false);
            var flightReservationResponse = persistData?.FirstOrDefault(p => string.Equals(p.CartId, cartId, StringComparison.OrdinalIgnoreCase));
            var response = default(FlightReservationResponse);
            if (flightReservationResponse != null)
            {
                response = flightReservationResponse.CslFlightReservationResponse;
            }

            return response;
        }
        public bool IsClearAllSavedTrips(bool isSavedTrip, int appId, String appVersion, bool isRemoveAll)
        {
            if (IsEnableOmniCartHomeScreenChanges(appId, appVersion))
            {
                if (isSavedTrip)
                {
                    if (isRemoveAll)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return true;
                }
            }
            return true;
        }


        public async Task<bool> IsNonUSPointOfSale(string sessionId, string cartId, Mobile.Model.TripPlannerGetService.MOBSHOPSelectTripResponse response)
        {
            FlightReservationResponse flightReservationResponse =await GetFlightReservationResponseByCartId(sessionId, cartId);

            if (flightReservationResponse!= null && String.CompareOrdinal(flightReservationResponse?.Reservation?.PointOfSale?.Country?.CountryCode, "US") != 0)
            {
                response.NonUSPOSAlertMessage = new MOBOnScreenAlert()
                {
                    Title = _configuration.GetValue<string>("POSAlertMessageTitle")?.ToString(),
                    Message = _configuration.GetValue<string>("POSAlertMessageBodyText")?.ToString(),
                    Actions = new List<MOBOnScreenActions> {
                            new MOBOnScreenActions
                            {
                                ActionTitle = _configuration.GetValue<string>("POSContinueButtonText")?.ToString(),
                                ActionURL = flightReservationResponse.VendorQueryReturnUrl,
                                WebSessionShareUrl= _configuration.GetValue<string>("DotcomSSOUrl")
                            },
                            new MOBOnScreenActions
                            {
                                 ActionTitle = _configuration.GetValue<string>("POSStartNewSearchButtonText")?.ToString()
                            },
                        }
                };
                return true;
            }
            return false;
        }

        public void BuildOmniCart(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation)
        {            
            shoppingCart.OmniCart.CartItemsCount = GetCartItemsCount(shoppingCart);
            shoppingCart.OmniCart.TotalPrice = GetTotalPrice(shoppingCart?.Products, reservation);
            shoppingCart.OmniCart.PayLaterPrice = GetPayLaterAmount(shoppingCart?.Products, reservation);
            shoppingCart.OmniCart.FOPDetails = GetFOPDetails(reservation);
            if (_configuration.GetValue<bool>("EnableShoppingCartPhase2Changes"))
            {
                shoppingCart.OmniCart.CostBreakdownFareHeader = GetCostBreakdownFareHeader(reservation?.ShopReservationInfo2?.TravelType);

            }
            if (_configuration.GetValue<bool>("EnableLivecartForAwardTravel") && reservation.AwardTravel)
            {
                shoppingCart.OmniCart.AdditionalMileDetail = GetAdditionalMileDetail(reservation);
            }

            AssignUpliftText(shoppingCart, reservation);                //Assign message text and link text to the Uplift
        }

        private string GetCostBreakdownFareHeader(string travelType)
        {
            string fareHeader = "Fare";
            if (!string.IsNullOrEmpty(travelType))
            {
                travelType = travelType.ToUpper();
                if (travelType == TravelType.CB.ToString())
                {
                    fareHeader = "Corporate fare";
                }
                else if (travelType == TravelType.CLB.ToString())
                {
                    fareHeader = "Break from Business fare";
                }
            }
            return fareHeader;
        }

        private MOBSection GetAdditionalMileDetail(MOBSHOPReservation reservation)
        {
            var additionalMilesPrice = reservation?.Prices?.FirstOrDefault(price => string.Equals("MPF", price?.DisplayType, StringComparison.OrdinalIgnoreCase));
            if (additionalMilesPrice != null)
            {
                var returnObject = new MOBSection();
                returnObject.Text1 = !string.IsNullOrEmpty(_configuration.GetValue<string>("AdditionalMilesLabelText")) ? _configuration.GetValue<string>("AdditionalMilesLabelText") : "Additional Miles";
                returnObject.Text2 = additionalMilesPrice.PriceTypeDescription?.Replace("Additional", String.Empty).Trim();
                returnObject.Text3 = additionalMilesPrice.FormattedDisplayValue;

                return returnObject;
            }
            return null;
        }

        public string GetCCEOmnicartRepricingCharacteristicValue(int applicationId, string appVersion)
        {
            string CCEOmnicartRepricingCharacteristicsValue = "";
            if (IsEnableOmniCartReleaseCandidateTwoChanges_Bundles(applicationId, appVersion))
            {
                CCEOmnicartRepricingCharacteristicsValue = _configuration.GetValue<string>("CCEOmnicartCharacteristicValue");
            }

            if (IsEnableOmniCartReleaseCandidateThreeChanges_Seats(applicationId, appVersion))
            {
                CCEOmnicartRepricingCharacteristicsValue = _configuration.GetValue<string>("CCEOmnicartRepricingCharacteristicValueAfterSeats");
            }
            return CCEOmnicartRepricingCharacteristicsValue;
        }

        public bool IsEnableOmniCartReleaseCandidate4CChanges(int applicationId, string appVersion, List<MOBItem> catalogItems = null)
        {
            if (string.IsNullOrEmpty(_configuration.GetValue<string>("EnableOmniCartReleaseCandidate4CChanges")) && (catalogItems != null && catalogItems.Count > 0 &&
            catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableOmnicartRC4CChanges).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableOmnicartRC4CChanges).ToString())?.CurrentValue == "1"))
                return GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartReleaseCandidate4CChanges_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartReleaseCandidate4CChanges_AppVersion"));
            else
                return _configuration.GetValue<bool>("EnableOmniCartReleaseCandidate4CChanges") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartReleaseCandidate4CChanges_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartReleaseCandidate4CChanges_AppVersion"));
        }
        public bool IsOmniCartSavedTripAndNavigateToFinalRTI(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart)
        {
            return reservation?.ShopReservationInfo2?.IsOmniCartSavedTrip == true && shoppingCart?.OmniCart?.NavigateToScreen == MOBNavigationToScreen.FINALRTI.ToString();
        }

    }
}
