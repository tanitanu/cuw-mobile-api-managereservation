using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Bundles;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.UnfinishedBooking;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common;
using United.Services.FlightShopping.Common.DisplayCart;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using AdvisoryType = United.Mobile.Model.Common.AdvisoryType;
using ContentType = United.Mobile.Model.Common.ContentType;
using FlowType = United.Utility.Enum.FlowType;
using MOBBKTraveler = United.Mobile.Model.Shopping.Booking.MOBBKTraveler;
using Trip = United.Services.FlightShopping.Common.Trip;

namespace United.Common.Helper
{
    public class ConfigUtility : IConfigUtility
    {
        private static IConfiguration _configuration { get; set; }
        public ConfigUtility(IConfiguration configuration)
        {
            ConfigUtility._configuration = configuration;
        }
        public static void UtilityInitialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public static bool IsSeatMapSupportedOa(string operatingCarrier, string MarketingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier)) return false;
            var seatMapSupportedOa = _configuration.GetValue<string>("SeatMapSupportedOtherAirlines");
            if (string.IsNullOrEmpty(seatMapSupportedOa)) return false;

            var seatMapEnabledOa = seatMapSupportedOa.Split(',');
            if (seatMapEnabledOa.Any(s => s == operatingCarrier.ToUpper().Trim()))
                return true;
            else if (_configuration.GetValue<string>("SeatMapSupportedOtherAirlinesMarketedBy") != null)
            {
                return _configuration.GetValue<string>("SeatMapSupportedOtherAirlinesMarketedBy").Split(',').ToList().Any(m => m == MarketingCarrier + "-" + operatingCarrier);
            }

            return false;
        }

        public static bool EnablePreferredZone(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("isEnablePreferredZone")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPreferredSeatVersion", "iOSPreferredSeatVersion", "", "", true, _configuration);
            }
            return false;
        }

        public static bool IsVerticalSeatMapEnabledAppVersion(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidVerticalSeatMapSupportedVersion", "iOSVerticalSeatMapSupportedVersion", "", "", true, _configuration);
            }
            return false;
        }

        public static bool IsVerticalSeatMapCatalogEnabled(List<MOBItem> catalogItems)
        {
            return catalogItems != null &&
                   catalogItems.Count > 0 &&
                   catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableVerticalSeatMap).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableVerticalSeatMap).ToString())?.CurrentValue == "1";
        }
        public static bool IsAddPetMREnabled(List<MOBItem> catalogItems)
        {
            return catalogItems != null &&
                   catalogItems.Count > 0 &&
                   catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableAddPetMR).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableAddPetMR).ToString())?.CurrentValue == "1";
        }

        public static bool IsMFOPCatalogEnabled(List<MOBItem> catalogItems)
        {
            return catalogItems != null &&
                   catalogItems.Count > 0 &&
                   catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableMilesFOPForPaidSeats).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableMilesFOPForPaidSeats).ToString())?.CurrentValue == "1";
        }

        public static bool IsUPPSeatMapSupportedVersion(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableUPPSeatmap")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidUPPSeatmapVersion", "iPhoneUPPSeatmapVersion", "", "", true, _configuration);
            }

            return false;
        }

        public static bool OaSeatMapExceptionVersion(int applicationId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "AndroidOaSeatMapExceptionVersion", "iPhoneOaSeatMapExceptionVersion", "", "", true, _configuration);
        }



        public static bool IsIBE(Reservation persistedReservation)
        {
            if (_configuration.GetValue<bool>("EnablePBE") && (persistedReservation.ShopReservationInfo2 != null))
            {
                return persistedReservation.ShopReservationInfo2.IsIBE;
            }
            return false;
        }

        public static bool IsEMinusSeat(string programCode)
        {
            if (!_configuration.GetValue<bool>("EnableSSA") || programCode.IsNullOrEmpty())
                return false;
            programCode = programCode.ToUpper().Trim();
            return programCode.Equals("ASA") || programCode.Equals("BSA");
        }

        public static bool EnableUnfinishedBookings(BookingBundlesRequest request)
        {

            return _configuration.GetValue<bool>("EnableUnfinishedBookings")
                    && GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidEnableUnfinishedBookingsVersion", "iPhoneEnableUnfinishedBookingsVersion", "", "", true, _configuration);
        }

        public static bool OaSeatMapSupportedVersion(int applicationId, string appVersion, string carrierCode, string MarketingCarrier = "")
        {
            var supportedOA = false;
            if (IsSeatMapSupportedOa(carrierCode, MarketingCarrier))
            {
                switch (carrierCode)
                {
                    case "AC":
                        {
                            supportedOA = EnableAirCanada(applicationId, appVersion);
                            break;
                        }
                    default:
                        {
                            supportedOA = GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "AndroidOaSeatMapVersion", "iPhoneOaSeatMapVersion", "", "", true, _configuration);
                            break;
                        }
                }
            }
            return supportedOA;
        }

        public static bool EnableAirCanada(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAirCanada")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidAirCanadaVersion", "iPhoneAirCanadaVersion", "", "", true, _configuration);
        }

        public static bool IsRecommendedSeatingSupportedVersion(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableMRRecommendedSeating")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidRecommendedSeatingVersion", "iOSRecommendedSeatingVersion", "", "", true, _configuration);
            }

            return false;
        }
        public static bool EnableTravelerTypes(int appId, string appVersion, bool reshop = false)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableTravelerTypes") && !reshop
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTravelerTypesVersion", "iPhoneTravelerTypesVersion", "", "", true, _configuration);
            }
            return false;
        }

        public static bool ShopTimeOutCheckforAppVersion(int appID, string appVersion)
        {
            bool _isDisable = false;
            try
            {
                if (_configuration.GetValue<string>("AppVesrionsTimeOutApp2_1_22") != null)
                {
                    //“1~2.1.22|2~2.1.22”
                    foreach (var appVersionWithID in _configuration.GetValue<string>("AppVesrionsTimeOutApp2_1_22").ToString().Split('|'))
                    {
                        if (appVersionWithID.Split('~')[0].ToString().Trim() == appID.ToString().Trim() && appVersionWithID.Split('~')[1].ToString().Trim() == appVersion.Trim())
                        {
                            _isDisable = true; break;
                        }
                    }

                }
            }
            catch { }
            return _isDisable;
        }



        public static bool IsPosRedirectInShopEnabled(MOBSHOPShopRequest request)
        {
            bool isPosEnabledInShop = false;
            if (!_configuration.GetValue<bool>("PosRedirectEnabledInShop"))
            {
                var isPosVersionGreater = GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidIPosShopVersion", "iPhoneIPosShopVersion", "", "", true, _configuration);

                if (isPosVersionGreater)
                {
                    if (!_configuration.GetValue<string>("PosMobileSupportedCountryList").Contains(request.CountryCode))
                    {
                        request.CountryCode = "US";
                    }
                }
            }
            else
            {
                var isPosVersionGreater = GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidIPosShopVersion", "iPhoneIPosShopVersion", "", "", true, _configuration);

                if (isPosVersionGreater && !_configuration.GetValue<string>("PosMobileSupportedCountryList").Contains(request.CountryCode))
                {
                    isPosEnabledInShop = true;
                }
                else if (!_configuration.GetValue<string>("PosMobileSupportedCountryList").Contains(request.CountryCode))
                {
                    request.CountryCode = "US";
                }
            }

            return isPosEnabledInShop;
        }

        public static string BuildDotComRedirectUrl(Mobile.Model.Shopping.MOBSHOPShopRequest request)
        {
            string awdOrRev = request.AwardTravel ? "awd?&at=1&rm=1" : "rev?";

            string redirectUrlToDotCom = string.Format(_configuration.GetValue<string>("PosRedirectUrl"), request.CountryCode, awdOrRev);

            try
            {
                string stops = string.Empty, fareType = string.Empty, fareClass = string.Empty, totalPax = string.Empty;
                if (request.SearchType == "OW" && request.Trips != null && request.Trips.Count > 0)
                {
                    string OriginDestAndDate = $"&tt={1}&f={request.Trips[0].Origin}&t={request.Trips[0].Destination}&d={UtilityHelper.FormatDate(request.Trips[0].DepartDate)}";

                    //stops
                    //stops = request.GetNonStopFlightsOnly ? "&sc=1" : request.GetFlightsWithStops ? "&sc=7" : string.Empty;

                    //nearby 100 miles
                    string nearbyAirporMiles = (request.Trips[0].SearchNearbyOriginAirports && request.Trips[0].SearchNearbyDestinationAirports) ? "&cbm=100&cbm2=100" : request.Trips[0].SearchNearbyOriginAirports ? "&cbm=100" : request.Trips[0].SearchNearbyDestinationAirports ? "&cbm2=100" : "&cbm=-1&cbm2=-1";

                    //Ebusiness or First
                    string cabinType = (request.Trips[0].Cabin == "first" || request.Trips[0].Cabin == "businessFirst") ? "&ct=1" : string.Empty;

                    UtilityHelper.GetDotComRedirectParameters(request, out fareType, out fareClass, out totalPax);

                    redirectUrlToDotCom = $"{redirectUrlToDotCom}{OriginDestAndDate}{cabinType}{nearbyAirporMiles}{stops}{fareType}{fareClass}{totalPax}";
                }
                else if (request.SearchType == "RT" && request.Trips != null && request.Trips.Count > 1)
                {
                    string OriginDestAndDate = $"&f={request.Trips[0].Origin}&t={request.Trips[0].Destination}&d={UtilityHelper.FormatDate(request.Trips[0].DepartDate)}&r={UtilityHelper.FormatDate(request.Trips[1].DepartDate)}";

                    //stops
                    //stops = request.GetNonStopFlightsOnly ? "&sc=1,1" : request.GetFlightsWithStops ? "&sc=7,7" : string.Empty;

                    //nearby 100 miles
                    string nearbyAirporMiles = (request.Trips[0].SearchNearbyOriginAirports && request.Trips[1].SearchNearbyDestinationAirports) ? "&cbm=100&cbm2=100" : request.Trips[0].SearchNearbyOriginAirports ? "&cbm=100" : request.Trips[1].SearchNearbyDestinationAirports ? "&cbm2=100" : "&cbm=-1&cbm2=-1";

                    //Ebusiness or First
                    string cabinType = (request.Trips[0].Cabin == "first" || request.Trips[0].Cabin == "businessFirst") ? "&ct=1" : string.Empty;

                    UtilityHelper.GetDotComRedirectParameters(request, out fareType, out fareClass, out totalPax);

                    redirectUrlToDotCom = $"{redirectUrlToDotCom}{OriginDestAndDate}{cabinType}{nearbyAirporMiles}{stops}{fareType}{fareClass}{totalPax}";
                }
                else if (request.SearchType == "MD" && request.Trips != null && request.Trips.Count > 0)
                {
                    StringBuilder numberOfStops = new StringBuilder("&sc=");

                    //itip=IAH_ORD_2020-04-30*ORD_EWR_2020-05-03*EWR_IAH_2020-05-10
                    StringBuilder tripsSb = new StringBuilder("&itip=");
                    foreach (var trip in request.Trips)
                    {
                        tripsSb.Append(trip.Origin);
                        tripsSb.Append("_");
                        tripsSb.Append(trip.Destination);
                        tripsSb.Append("_");
                        tripsSb.Append(UtilityHelper.FormatDate(trip.DepartDate));
                        tripsSb.Append("*");

                    }
                    if (tripsSb != null && tripsSb.ToString().Length > 1 && tripsSb.ToString().EndsWith("*"))
                    {
                        tripsSb.Length--;
                    }
                    string OriginDestAndDate = $"&tt={2}{tripsSb}";

                    //stops
                    //stops = request.GetNonStopFlightsOnly ? "&sc=1" : request.GetFlightsWithStops ? "&sc=7" : string.Empty;

                    bool isAllTripsAreFirstOrBusiness = request.Trips.All(trip => trip.Cabin.Equals("first", StringComparison.OrdinalIgnoreCase) || trip.Cabin.Equals("businessFirst", StringComparison.OrdinalIgnoreCase));
                    //Cabin type if all trip are not first or business then default to econ 
                    string cabinType = isAllTripsAreFirstOrBusiness ? "&ct=1" : string.Empty;
                    UtilityHelper.GetDotComRedirectParameters(request, out fareType, out fareClass, out totalPax);

                    redirectUrlToDotCom = $"{redirectUrlToDotCom}{OriginDestAndDate}{cabinType}{stops}{fareType}{fareClass}{totalPax}";
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('#');
                // if (traceSwitch.TraceInfo)
                {
                    ExceptionWrapper exceptionWrapper = new ExceptionWrapper(ex);
                    exceptionWrapper.Message = messages[0];
                    // logEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBExceptionWrapper>(request.SessionId, "Shop", "Exception", request.Application.Id, request.Application.Version.Major, request.DeviceId, exceptionWrapper, true, false));
                    //logEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBExceptionWrapper>(request.SessionId, "ShopiPOS", "DotComRedirectUrlBuildError", request.Application.Id, request.Application.Version.Major, request.DeviceId, exceptionWrapper, true, false));
                }
            }

            return redirectUrlToDotCom;
        }

        //added-Kriti
        public static bool EnableRoundTripPricing(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("Shopping - bPricingBySlice")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableRoundTripPricingVersion", "iPhoneEnableRoundTripPricingVersion", "", "", true, _configuration);
        }

        public static bool EnableYoungAdult(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableYoungAdultBooking") && !isReshop;
        }

        public static bool EnableAirportDecodeToCityAllAirports(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableAirportDecodeToCityAllAirports") && !isReshop;
        }

        public static bool EnableSSA(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSSA") && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidSSAVersion", "iPhoneSSAVersion", "", "", true, _configuration);
        }

        public static bool EnableMileageBalance(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableMileageBalance") && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableMileageBalanceVersion", "iPhoneEnableMileageBalanceVersion", "", "", true, _configuration);
        }


        public static bool EnableFSRAlertMessages(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableFSRAlertMessages")
                     && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableFSRAlertMessagesVersion", "iPhoneEnableFSRAlertMessagesVersion", "", "", true, _configuration);
        }

        public static bool CheckFSRRedesignFromShopRequest(MOBSHOPShopRequest request)
        {
            return (request != null && _configuration.GetValue<bool>("IsEnableNewFSRRedesign") && !(request.IsReshop || request.IsReshopChange)
                 && !request.IsCorporateBooking && string.IsNullOrEmpty(request.EmployeeDiscountId)
                 && request.Experiments != null && request.Experiments.Any() && request.Experiments.Contains(ShoppingExperiments.FSRRedesignA.ToString())
                 && GeneralHelper.IsApplicationVersionGreaterorEqual(request.Application.Id, request.Application.Version.Major, _configuration.GetValue<string>("FSRRedesignAndroidversion"), _configuration.GetValue<string>("FSRRedesigniOSversion"))
                 && request.TravelType != TravelType.CLB.ToString() && !request.AwardTravel && !request.IsYoungAdultBooking);
        }


        public static bool IsETCchangesEnabled(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("ETCToggle") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_ETC_AppVersion"), _configuration.GetValue<string>("iPhone_ETC_AppVersion")))
            {
                return true;
            }
            return false;
        }

        public static bool EnableFareDisclouserCopyMessage(bool reshop = false)
        {
            return _configuration.GetValue<bool>("EnableFareDisclosureCopyMessage") && !reshop;
        }

        public static string GetFeeWaiverMessage()
        {
            return _configuration.GetValue<string>("ChangeFeeWaiver_Message");
        }

        public static MOBSearchFilters SetSearchFiltersOutDefaults(MOBSearchFilters shopSearchFiltersOut)
        {
            #region
            shopSearchFiltersOut.SortTypes = new List<MOBSearchFilterItem>();
            List<string> sortTypesList = _configuration.GetValue<string>("SearchFiletersSortTypes").ToString().Split('|').ToList();
            foreach (string sortType in sortTypesList)
            {
                var item = new MOBSearchFilterItem();
                item.Key = sortType.Split('~')[0].ToString();
                item.Value = sortType.Split('~')[1].ToString();
                item.DisplayValue = sortType.Split('~')[1].ToString();
                shopSearchFiltersOut.SortTypes.Add(item);
            }
            shopSearchFiltersOut.AmenityTypes = new List<MOBSearchFilterItem>();
            List<string> amenityTypesList = _configuration.GetValue<string>("SearchFiletersAmenityTypes").ToString().Split('|').ToList();
            foreach (string amenityType in amenityTypesList)
            {
                var item = new MOBSearchFilterItem();
                item.Key = amenityType.Split('~')[0].ToString();
                item.Value = amenityType.Split('~')[1].ToString();
                item.DisplayValue = amenityType.Split('~')[1].ToString();
                item.IsSelected = true;
                shopSearchFiltersOut.AmenityTypes.Add(item);
            }
            shopSearchFiltersOut.CarrierTypes = new List<MOBSearchFilterItem>();
            List<string> carrierTypesList = _configuration.GetValue<string>("SearchFiletersCarrierTypes").ToString().Split('|').ToList();
            foreach (string carrierType in carrierTypesList)
            {
                var item = new MOBSearchFilterItem();
                item.Key = carrierType.Split('~')[0].ToString();
                item.Value = carrierType.Split('~')[1].ToString();
                item.DisplayValue = carrierType.Split('~')[1].ToString();
                item.IsSelected = true;
                shopSearchFiltersOut.CarrierTypes.Add(item);
            }
            shopSearchFiltersOut.AircraftCabinTypes = new List<MOBSearchFilterItem>();
            List<string> aircraftCabinTypesList = _configuration.GetValue<string>("SearchFiletersAircraftCabinTypes").ToString().Split('|').ToList();
            foreach (string aircraftCabinType in aircraftCabinTypesList)
            {
                var item = new MOBSearchFilterItem();
                item.Key = aircraftCabinType.Split('~')[0].ToString();
                item.Value = aircraftCabinType.Split('~')[1].ToString();
                item.DisplayValue = aircraftCabinType.Split('~')[1].ToString();
                item.IsSelected = true;
                shopSearchFiltersOut.AircraftCabinTypes.Add(item);
            }
            return shopSearchFiltersOut;
            #endregion
        }

        public static bool EnableIBEFull()
        {
            return _configuration.GetValue<bool>("EnableIBE");
        }

        public static bool EnableIBELite()
        {
            return _configuration.GetValue<bool>("EnableIBELite");
        }

        public static bool EnableFSRLabelTexts(int appID, string appVersion, bool isReshop = false)
        {
            var version1 = _configuration.GetValue<string>("FSRLabelTextsAndroidversion");
            var version2 = _configuration.GetValue<string>("FSRLabelTextsiOSversion");
            return _configuration.GetValue<bool>("EnableFSRLabelTexts") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, version1, version2);
        }

        public static bool EnableCovidTestFlightShopping(int appID, string appVersion, bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableCovidTestFlightShopping") && EnableRtiMandateContentsToDisplayByMarket(appID, appVersion, isReshop);
        }

        public static bool EnableRtiMandateContentsToDisplayByMarket(int appID, string appVersion, bool isReshop)
        {
            return _configuration.GetValue<bool>("EnableRtiMandateContentsToDisplayByMarket") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("CovidTestAndroidversion"), _configuration.GetValue<string>("CovidTestiOSversion"));
        }

        public static void CheckTripsDepartDateAndAssignPreviousTripDateIfItLesser(Services.FlightShopping.Common.ShopRequest request)
        {
            if (_configuration.GetValue<bool>("BugFixToggleFor17M") && request != null && request.Trips != null && request.Trips.Count > 0)
            {
                for (int i = 0; i < request.Trips.Count; i++)
                {
                    if (i < request.Trips.Count - 1 && DateTime.Parse(request.Trips[i].DepartDate) > DateTime.Parse(request.Trips[i + 1].DepartDate))
                    {
                        request.Trips[i + 1].DepartDate = request.Trips[i].DepartDate;
                    }
                }
            }
        }

        public static string GetCurrencySymbol(CultureInfo ci)
        {
            string result = string.Empty;

            try
            {
                int amt = 0;
                CultureInfo tempCi = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = ci;
                result = amt.ToString("c0").Substring(0, 1);
                if (string.IsNullOrEmpty(result))
                    result = "$";
                Thread.CurrentThread.CurrentCulture = tempCi;
            }
            catch { }

            return result;
        }

        public static bool EnableOptoutScreenHyperlinkSupportedContent(int appID, string version)
        {
            var version1 = _configuration.GetValue<string>("AndroidOptOutHyperlinkSupportedVersion");
            var version2 = _configuration.GetValue<string>("iPhoneOptOutHyperlinkSupportedVersion");
            return _configuration.GetValue<bool>("EnableOptOutHyperlinkSupportedContent") && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, version, version1, version2);
        }

        public static bool IsIBeLiteFare(Product prod)
        {
            return EnableIBELite() && !string.IsNullOrWhiteSpace(prod.ProductCode) && _configuration.GetValue<string>("IBELiteShoppingProductCodes").IndexOf(prod.ProductCode.Trim().ToUpper()) > -1;
        }

        public static bool FeatureVersionCheck(int appId, string appVersion, string featureName, string androidVersion, string iosVersion)
        {
            if (string.IsNullOrEmpty(appVersion) || string.IsNullOrEmpty(featureName))
                return false;
            return _configuration.GetValue<bool>(featureName) && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, androidVersion, iosVersion, "", "", true, _configuration);
        }



        public static string formatAllCabinAwardAmountForDisplay(string amt, string cabinType, bool truncate = true, string price = null)
        {
            string newAmt = amt;
            decimal amount = 0;
            decimal.TryParse(amt, out amount);

            try
            {
                if (amount > -1)
                {
                    if (truncate)
                    {
                        //int newTempAmt = (int)decimal.Ceiling(amount);
                        try
                        {
                            if (amount > 999)
                            {
                                amount = amount / 1000;
                                if (amount % 1 > 0)
                                    newAmt = string.Format("{0:n1}", amount) + "K";
                                else
                                    newAmt = string.Format("{0:n0}", amount) + "K";
                            }
                            else if (amount == 0)
                            {
                                newAmt = "---";
                            }
                            else
                            {
                                newAmt = string.Format("{0:n0}", amount);
                            }
                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            newAmt = string.Format("{0:n0}", amount) + " miles";
                        }
                        catch { }
                    }
                }

                if (cabinType.Trim().ToUpper().Contains("FIRST"))
                {
                    newAmt += " First";
                }
                else if (cabinType.Trim().ToUpper().Contains("BUS"))
                {
                    newAmt += " Bus";
                }
                else
                {
                    newAmt += " Eco";
                }

                if (_configuration.GetValue<bool>("EnableAwardPricesForAllProducts"))
                {
                    newAmt = CabinButtonTextWithPrice(price, newAmt);
                }
            }
            catch { }

            return newAmt;
        }

        private static string CabinButtonTextWithPrice(string price, string newAmt)
        {
            if (!String.IsNullOrEmpty(newAmt))
            {
                if (!string.IsNullOrEmpty(price) && !price.ToUpper().Equals("NOT AVAILABLE"))
                {
                    newAmt = newAmt + " " + price.Replace(" ", string.Empty);
                }
                newAmt = newAmt.Replace(" ", "\n");
            }

            return newAmt;
        }
        public static string GetConfigEntriesWithDefault(string configKey, string defaultReturnValue)
        {
            var configString = _configuration.GetValue<string>(configKey);
            if (!string.IsNullOrEmpty(configString))
            {
                return configString;
            }

            return defaultReturnValue;
        }
        public static string GetCSLCallDuration(object logEntries)
        {
            throw new NotImplementedException();
        }

        public static bool IsEnableOmniCartMVP2Changes(int applicationId, string appVersion, bool isDisplayCart)
        {
            if (_configuration.GetValue<bool>("EnableOmniCartMVP2Changes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartMVP2Changes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartMVP2Changes_AppVersion")) && isDisplayCart)
            {
                return true;
            }
            return false;
        }


        public static bool IsFeewaiverEnabled(bool isReshop)
        {
            return _configuration.GetValue<bool>("ChangeFeeWaiverMessagingToggle") && !isReshop;
        }

        public static List<InfoWarningMessages> UpdateFeeWaiverMessage(List<InfoWarningMessages> infoWarningMessages)
        {
            List<InfoWarningMessages> tmpList = infoWarningMessages;

            try
            {
                if (infoWarningMessages.Count == 0)
                {
                    infoWarningMessages.Add(new InfoWarningMessages() { IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString(), Messages = new List<string>() { GetFeeWaiverMessageSoftRTI() }, Order = MOBINFOWARNINGMESSAGEORDER.BEFAREINVERSION.ToString() });
                }

                if (infoWarningMessages.Count > 0 && !(infoWarningMessages.Count == 1 && infoWarningMessages[0].Messages[0].Equals(GetFeeWaiverMessageSoftRTI())))
                {
                    foreach (var message in infoWarningMessages.ToList())
                    {
                        if (message.Messages != null && message.Messages.Count > 0 && message.Messages[0].Equals(GetFeeWaiverMessageSoftRTI()))
                        {
                            infoWarningMessages.Remove(message);
                        }
                    }

                    if (!infoWarningMessages.Any(m => m.Messages[0].Contains(GetFeeWaiverMessageSoftRTI())))
                    {
                        var msg = infoWarningMessages.FirstOrDefault(m => m.IconType == MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString());

                        if (msg != null)
                        {
                            if (!string.IsNullOrEmpty(msg.Messages[0]))
                            {
                                msg.Messages[0] += System.Environment.NewLine + System.Environment.NewLine;
                                msg.Messages[0] = string.Concat(msg.Messages[0], GetFeeWaiverMessageSoftRTI());
                            }
                            else
                            {
                                msg.Messages[0] = GetFeeWaiverMessageSoftRTI();
                            }
                        }
                        else
                        {
                            var msgFst = infoWarningMessages.First();
                            msgFst.Messages[0] += System.Environment.NewLine + System.Environment.NewLine;
                            msgFst.Messages[0] = string.Concat(msgFst.Messages[0], GetFeeWaiverMessageSoftRTI());
                        }
                    }
                    else
                    {
                        if (infoWarningMessages.Count == 1 || (infoWarningMessages[0].IconType.Equals(MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString()) && infoWarningMessages[0].Messages[0].Contains(GetFeeWaiverMessageSoftRTI()))
                           || (infoWarningMessages.Any(m => m.IconType.Equals(MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString())) && infoWarningMessages.First(m => m.IconType.Equals(MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString())).Messages[0].Contains(GetFeeWaiverMessageSoftRTI()))
                           || !infoWarningMessages.Any(m => m.IconType.Equals(MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString())))
                        {
                            return infoWarningMessages;
                        }
                        else
                        {
                            bool updated = false;
                            foreach (var msgToUpdate in infoWarningMessages.ToList())
                            {
                                if (msgToUpdate.IconType.Equals(MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString()) && !msgToUpdate.Messages[0].Contains(GetFeeWaiverMessageSoftRTI()) && !updated)
                                {
                                    msgToUpdate.Messages[0] += System.Environment.NewLine + System.Environment.NewLine;
                                    msgToUpdate.Messages[0] = string.Concat(msgToUpdate.Messages[0], GetFeeWaiverMessageSoftRTI());
                                    updated = true;
                                }
                                else
                                {
                                    if (msgToUpdate.Messages[0].IndexOf(GetFeeWaiverMessageSoftRTI()) > 0)
                                    {
                                        msgToUpdate.Messages[0] = msgToUpdate.Messages[0].Remove(msgToUpdate.Messages[0].IndexOf(GetFeeWaiverMessageSoftRTI(), GetFeeWaiverMessageSoftRTI().Length)).Trim();
                                    }
                                }
                            }
                        }
                    }
                }
                return infoWarningMessages;
            }
            catch (Exception)
            {
                return tmpList;
            }
        }

        public static string GetFeeWaiverMessageSoftRTI()
        {
            return _configuration.GetValue<string>("ChangeFeeWaiver_Message_SoftRTI");
        }

        public static bool EnableAwardFareRules(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAwardFareRules")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableAwardFareRulesVersion", "iPhoneEnableAwardFareRulesVersion", "", "", true, _configuration);
        }

        public static bool IsForceSeatMapforEPlus(bool isReShop, bool isElf, bool is24HoursWindow, int appid, string appversion)
        {
            if (!isReShop && !isElf && is24HoursWindow && EnableForceEPlus(appid, appversion))
            {
                return true;
            }

            return false;
        }

        public static bool EnableForceEPlus(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableForceEPlus")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidForceEPlusVersion", "iPhoneForceEPlusVersion", "", "", true, _configuration);
        }

        public static bool IsEnabledNationalityAndResidence(bool isReShop, int appid, string appversion)
        {
            if (!isReShop && EnableNationalityResidence(appid, appversion))
            {
                return true;
            }

            return false;
        }

        public static bool EnableNationalityResidence(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableNationalityAndCountryOfResidence")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidiPhonePriceChangeVersion", "AndroidiPhonePriceChangeVersion", "", "", true, _configuration);
        }

        public static bool EnableSpecialNeeds(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSpecialNeeds")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableSpecialNeedsVersion", "iPhoneEnableSpecialNeedsVersion", "", "", true, _configuration);
        }

        public static bool EnableInflightContactlessPayment(int appID, string appVersion, bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableInflightContactlessPayment") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("InflightContactlessPaymentAndroidVersion"), _configuration.GetValue<string>("InflightContactlessPaymentiOSVersion"));
        }

        public static bool AllowElfMetaSearchUpsell(int appId, string version)
        {
            var isSupportedAppVersion = GeneralHelper.IsApplicationVersionGreater(appId, version, "AndroidELFMetaSearchUpsellVersion", "iPhoneELFMetaSearchUpsellVersion", "", "", true, _configuration);
            if (isSupportedAppVersion)
            {
                return _configuration.GetValue<bool>("AllowELFMetaSearchUpsell");
            }
            return false;
        }

        public static bool EnableUnfinishedBookings(MOBRequest request)
        {
            return _configuration.GetValue<bool>("EnableUnfinishedBookings")
                    && GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidEnableUnfinishedBookingsVersion", "iPhoneEnableUnfinishedBookingsVersion", "", "", true, _configuration);
        }

        public static MOBSHOPUnfinishedBookingTrip MapToMOBSHOPUnfinishedBookingTrip(United.Services.Loyalty.Preferences.Common.SerializableSavedItinerary.Trip csTrip)
        {
            return new MOBSHOPUnfinishedBookingTrip
            {
                DepartDate = csTrip.DepartDate,
                DepartTime = csTrip.DepartTime,
                Destination = csTrip.Destination,
                Origin = csTrip.Origin,
                Flights = csTrip.Flights.Select(MapToMOBSHOPUnfinishedBookingFlight).ToList()
            };
        }

        public static MOBSHOPUnfinishedBookingFlight MapToMOBSHOPUnfinishedBookingFlight(United.Services.Loyalty.Preferences.Common.SerializableSavedItinerary.Flight cslFlight)
        {
            var ubMOBFlight = new MOBSHOPUnfinishedBookingFlight
            {
                BookingCode = cslFlight.BookingCode,
                DepartDateTime = cslFlight.DepartDateTime,
                Origin = cslFlight.Origin,
                Destination = cslFlight.Destination,
                FlightNumber = cslFlight.FlightNumber,
                MarketingCarrier = cslFlight.MarketingCarrier,
                ProductType = cslFlight.ProductType,
            };

            if (_configuration.GetValue<bool>("EnableShareTripDotComConnectionIssueFix"))
            {
                if (cslFlight.Connections == null || cslFlight.Connections.Count == 0)
                    return ubMOBFlight;

                foreach (var conn in cslFlight.Connections)
                {
                    if (ubMOBFlight.Connections == null)
                        ubMOBFlight.Connections = new List<MOBSHOPUnfinishedBookingFlight>();
                    ubMOBFlight.Connections.Add(MapToMOBSHOPUnfinishedBookingFlight(conn));
                }
            }
            else
            {
                if (ubMOBFlight.Connections == null || ubMOBFlight.Connections.Count == 0)
                    return ubMOBFlight;

                cslFlight.Connections.ForEach(x => ubMOBFlight.Connections.Add(MapToMOBSHOPUnfinishedBookingFlight(x)));
            }

            return ubMOBFlight;
        }

        public static bool EnableSavedTripShowChannelTypes(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableUnfinishedBookings") // feature toggle
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableUnfinishedBookingsVersion", "iPhoneEnableUnfinishedBookingsVersion", "", "", true, _configuration)

                    && _configuration.GetValue<bool>("EnableSavedTripShowChannelTypes") // story toggle
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableSavedTripShowChannelTypesVersion", "iPhoneEnableSavedTripShowChannelTypesVersion", "", "", true, _configuration);
        }

        public static List<MOBTypeOption> GetFopOptions(int applicationID, string appVersion)
        {
            #region
            List<MOBTypeOption> fopOptionsLocal = new List<MOBTypeOption>();
            string[] fopTypesByLatestVersion = null;
            if (applicationID == 1)
            {
                #region
                fopTypesByLatestVersion = _configuration.GetValue<string>("iOSFOPOptionsFromLatestVersion").Split('~');
                if (fopTypesByLatestVersion != null && fopTypesByLatestVersion.Count() > 0)
                {
                    fopOptionsLocal = GetAppsFOPOptions(appVersion, fopTypesByLatestVersion);
                }
                // Sample Value for AndriodFOPOptionsFromLatestVersion = "2.1.14|FOPOption2|FOPOption3~2.1.17|FOPOption2|FOPOption3|FOPOption4" and make sure the "FOPCount" is greater than or equal to 4 like <add key="FOPCount" value="3" /> value is 4 and 
                //there is value define for "FOPOption4" key like <add key="FOPOption4" value="MasterCardCheckOut|Master Check Out" />
                #endregion
            }
            else if (applicationID == 2)
            {
                #region
                fopTypesByLatestVersion = _configuration.GetValue<string>("AndriodFOPOptionsFromLatestVersion").ToString().Split('~');
                if (fopTypesByLatestVersion != null && fopTypesByLatestVersion.Count() > 0)
                {
                    fopOptionsLocal = GetAppsFOPOptions(appVersion, fopTypesByLatestVersion);
                }
                // Sample Value for AndriodFOPOptionsFromLatestVersion = "2.1.13|FOPOption2|FOPOption3~2.1.17|FOPOption2|FOPOption3|FOPOption4" and make sure the "FOPCount" is greater than or equal to 4 like <add key="FOPCount" value="3" /> value is 4 and 
                //there is value define for "FOPOption4" key like <add key="FOPOption4" value="MasterCardCheckOut|Master Check Out" />
                #endregion
            }
            else if (applicationID == 16)
            {
                #region
                fopTypesByLatestVersion = _configuration.GetValue<string>("MWebFOPOptions").Split('|');
                // Sample Value for <add key="mWebOPOptions" value="FOPOption2|FOPOption3" /> 
                foreach (string fopType in fopTypesByLatestVersion)
                {
                    fopOptionsLocal.Add(GetAvailableFopOptions(fopType));
                }
                #endregion
            }
            return fopOptionsLocal;
            #endregion
            #region
            #endregion
        }

        public static List<MOBTypeOption> GetAppsFOPOptions(string appVersion, string[] fopTypesByLatestVersion)
        {
            #region
            List<MOBTypeOption> fopOptionsLocal = new List<MOBTypeOption>();
            foreach (string fopOptionsList in fopTypesByLatestVersion) // fopTypesByLatestVersion = { "2.1.13|FOPOption2|FOPOption3","2.1.17|FOPOption2|FOPOption3|FOPOption4"}
            {
                #region
                string latestAppVersion = fopOptionsList.Split('|')[0].ToString(); // latestAppVersion = fopOtionsList.Split('|') = {"2.1.13","FOPOption2","FOPOption3","FOPOption4"} , fopOtionsList.Split('|')[0] = "2.1.13"  if appVersion = 2.1.17
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("", regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                bool returnFOPOptions = false;
                if (appVersion == latestAppVersion)
                {
                    returnFOPOptions = true;
                }
                else
                {
                    returnFOPOptions = GeneralHelper.IsVersion1Greater(appVersion, latestAppVersion);
                }
                if (returnFOPOptions)
                {
                    fopOptionsLocal = new List<MOBTypeOption>();
                    for (int i = 1; i < fopOptionsList.Split('|').Count(); i++) // fopOtionsList = "2.1.13|FOPOption2|FOPOption3" and fopOtionsList.Split('|') = "2.1.17|FOPOption2|FOPOption3|FOPOption4"
                    {
                        fopOptionsLocal.Add(GetAvailableFopOptions(fopOptionsList.Split('|')[i].ToString()));
                    }
                }
                #endregion
            }
            #endregion
            return fopOptionsLocal;
        }

        public static MOBTypeOption GetAvailableFopOptions(string fopType)
        {
            MOBTypeOption fopOption = new MOBTypeOption();  // <add key="FOPOption1" value="ApplePay|Apple Pay" />
            fopOption.Key = string.IsNullOrEmpty(_configuration.GetValue<string>(fopType)) ? "" : _configuration.GetValue<string>(fopType).Split('|')[0];
            fopOption.Value = string.IsNullOrEmpty(_configuration.GetValue<string>(fopType)) ? "" : _configuration.GetValue<string>(fopType).Split('|')[1];
            return fopOption;
        }

        public static bool IsDisplayCart(Session session, string travelTypeConfigKey = "DisplayCartTravelTypes")
        {
            string[] travelTypes = _configuration.GetValue<string>(travelTypeConfigKey).Split('|');//"Revenue|YoungAdult"
            bool isDisplayCart = false;
            if (session.IsAward && travelTypes.Contains("Award"))
            {
                isDisplayCart = true;
            }
            else if (!string.IsNullOrEmpty(session.EmployeeId) && travelTypes.Contains("UADiscount"))
            {
                isDisplayCart = true;
            }
            else if (session.IsYoungAdult && travelTypes.Contains("YoungAdult"))
            {
                isDisplayCart = true;
            }
            else if (session.IsCorporateBooking && travelTypes.Contains("Corporate"))
            {
                isDisplayCart = true;
            }
            else if (session.TravelType == TravelType.CLB.ToString() && travelTypes.Contains("CorporateLeisure"))
            {
                isDisplayCart = true;
            }
            else if (!session.IsAward && string.IsNullOrEmpty(session.EmployeeId) && !session.IsYoungAdult && !session.IsCorporateBooking && session.TravelType != TravelType.CLB.ToString() && travelTypes.Contains("Revenue"))
            {
                isDisplayCart = true;
            }

            return isDisplayCart;
        }

        public static string GetCSSPublicKeyPersistSessionStaticGUID(int applicationId)
        {
            #region Get Aplication and Profile Ids
            string[] cSSPublicKeyPersistSessionStaticGUIDs = _configuration.GetValue<string>("CSSPublicKeyPersistSessionStaticGUID").Split('|');
            List<string> applicationDeviceTokenSessionIDList = new List<string>();
            foreach (string applicationSessionGUID in cSSPublicKeyPersistSessionStaticGUIDs)
            {
                #region
                if (Convert.ToInt32(applicationSessionGUID.Split('~')[0].ToString().ToUpper().Trim()) == applicationId)
                {
                    return applicationSessionGUID.Split('~')[1].ToString().Trim();
                }
                #endregion
            }
            return "1CSSPublicKeyPersistStatSesion4IphoneApp";
            #endregion
        }

        public static List<List<MOBSHOPTax>> GetTaxAndFeesAfterPriceChange(List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> prices, bool isReshopChange = false, int appId = 0, string appVersion = "", string travelType = null)
        {
            List<List<MOBSHOPTax>> taxsAndFees = new List<List<MOBSHOPTax>>();
            CultureInfo ci = null;
            decimal taxTotal = 0.0M;
            decimal subTaxTotal = 0.0M;
            bool isTravelerPriceDirty = false;
            bool isEnableOmniCartMVP2Changes = _configuration.GetValue<bool>("EnableOmniCartMVP2Changes");

            foreach (var price in prices)
            {
                List<MOBSHOPTax> tmpTaxsAndFees = new List<MOBSHOPTax>();

                subTaxTotal = 0;

                if (price.SubItems != null && price.SubItems.Count > 0 && (!isReshopChange || (isReshopChange && price.Type.ToUpper() == "TRAVELERPRICE" && !isTravelerPriceDirty))) // Added by Hasnan - # 167553 - 10/04/2017
                {
                    foreach (var subItem in price.SubItems)
                    {
                        if (ci == null)
                        {
                            ci = TopHelper.GetCultureInfo(subItem.Currency);
                        }
                        MOBSHOPTax taxNfee = new MOBSHOPTax();
                        taxNfee = new MOBSHOPTax();
                        taxNfee.CurrencyCode = subItem.Currency;
                        taxNfee.Amount = subItem.Amount;
                        taxNfee.DisplayAmount = TopHelper.FormatAmountForDisplay(taxNfee.Amount, ci, false);
                        taxNfee.TaxCode = subItem.Type;
                        taxNfee.TaxCodeDescription = subItem.Description;
                        isTravelerPriceDirty = true;
                        tmpTaxsAndFees.Add(taxNfee);

                        subTaxTotal += taxNfee.Amount;
                    }
                }

                if (tmpTaxsAndFees != null && tmpTaxsAndFees.Count > 0)
                {
                    //add new label as first item for UI
                    MOBSHOPTax tnf = new MOBSHOPTax();
                    tnf.CurrencyCode = tmpTaxsAndFees[0].CurrencyCode;
                    tnf.Amount = subTaxTotal;
                    tnf.DisplayAmount = TopHelper.FormatAmountForDisplay(tnf.Amount, ci, false);
                    tnf.TaxCode = "PERPERSONTAX";
                    if (EnableYADesc(isReshopChange) && price.PricingPaxType != null && price.PricingPaxType.ToUpper().Equals("UAY"))
                    {
                        tnf.TaxCodeDescription = string.Format("{0} {1}: {2} per person", price.Count, "young adult (18-23)", tnf.DisplayAmount);
                    }
                    else
                    {
                        string description = price?.Description;
                        if (EnableShoppingcartPhase2ChangesWithVersionCheck(appId, appVersion) && !isReshopChange && !string.IsNullOrEmpty(travelType) && (travelType == TravelType.CB.ToString() || travelType == TravelType.CLB.ToString()))
                        {
                            description = BuildPaxTypeDescription(price?.PaxTypeCode, price?.Description, price.Count);
                        }
                        tnf.TaxCodeDescription = string.Format("{0} {1}: {2} per person", price.Count, description.ToLower(), tnf.DisplayAmount);
                    }
                    if (isEnableOmniCartMVP2Changes)
                    {
                        tnf.TaxCodeDescription = tnf.TaxCodeDescription.Replace(" per ", "/");
                    }
                    tmpTaxsAndFees.Insert(0, tnf);
                }
                taxTotal += subTaxTotal * price.Count;
                if (tmpTaxsAndFees.Count > 0)
                {
                    taxsAndFees.Add(tmpTaxsAndFees);
                }
            }
            if (taxsAndFees != null && taxsAndFees.Count > 0)
            {
                //add grand total for all taxes
                List<MOBSHOPTax> lstTnfTotal = new List<MOBSHOPTax>();
                MOBSHOPTax tnfTotal = new MOBSHOPTax();
                tnfTotal.CurrencyCode = taxsAndFees[0][0].CurrencyCode;
                tnfTotal.Amount += taxTotal;
                tnfTotal.DisplayAmount = TopHelper.FormatAmountForDisplay(tnfTotal.Amount, ci, false);
                tnfTotal.TaxCode = "TOTALTAX";
                tnfTotal.TaxCodeDescription = "Taxes and fees total";
                lstTnfTotal.Add(tnfTotal);
                taxsAndFees.Add(lstTnfTotal);
            }

            return taxsAndFees;
        }

        public static bool EnableYADesc(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableYoungAdultBooking") && _configuration.GetValue<bool>("EnableYADesc") && !isReshop;
        }

        public static bool IsEnableTaxForAgeDiversification(bool isReShop, int appid, string appversion)
        {
            if (!isReShop && EnableTaxForAgeDiversification(appid, appversion))
            {
                return true;
            }
            return false;
        }

        public static bool EnableTaxForAgeDiversification(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableTaxForAgeDiversification")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidiPhoneTaxForAgeDiversificationVersion", "AndroidiPhoneTaxForAgeDiversificationVersion", "", "", true, _configuration);
        }

        public static void SetELFUpgradeMsg(MOBSHOPAvailability availability, int ID)
        {
            if (_configuration.GetValue<bool>("ByPassSetUpUpgradedFromELFMessages"))
            {
                if (availability?.Reservation?.IsUpgradedFromEntryLevelFare ?? false)
                {
                    if (availability.Reservation.ShopReservationInfo2.InfoWarningMessages == null)
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages = new List<InfoWarningMessages>();

                    availability.Reservation.ShopReservationInfo2.InfoWarningMessages.Add(BuildUpgradeFromELFInfoMessage(ID));
                    availability.Reservation.ShopReservationInfo2.InfoWarningMessages = availability.Reservation.ShopReservationInfo2.InfoWarningMessages.OrderBy(c => (int)((MOBINFOWARNINGMESSAGEORDER)Enum.Parse(typeof(MOBINFOWARNINGMESSAGEORDER), c.Order))).ToList();
                }
            }
        }

        public static InfoWarningMessages BuildUpgradeFromELFInfoMessage(int ID)
        {
            var infoWarningMessages = new InfoWarningMessages
            {
                Order = MOBINFOWARNINGMESSAGEORDER.INHIBITBOOKING.ToString(), // Using existing order for sorting. 
                IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString(),
                HeaderMessage = (ID == 1) ? _configuration.GetValue<string>("UpgradedFromElfTitle") : string.Empty,
                Messages = new List<string>
                {
                   (ID==1)?_configuration.GetValue<string>("UpgradedFromElfText"):_configuration.GetValue<string>("UpgradedFromElfTextWithHtml")
                }
            };

            return infoWarningMessages;
        }

        public static InfoWarningMessages GetBEMessage()
        {
            var message = _configuration.GetValue<string>("BEFareInversionMessage") as string ?? string.Empty;
            return UtilityHelper.BuildInfoWarningMessages(message);
        }

        public static InfoWarningMessages GetBoeingDisclaimer()
        {
            InfoWarningMessages boeingDisclaimerMessage = new InfoWarningMessages();

            boeingDisclaimerMessage.Order = MOBINFOWARNINGMESSAGEORDER.BOEING737WARNING.ToString();
            if (_configuration.GetValue<string>("737DisclaimerMessageType").Equals("WARNING"))
            {
                boeingDisclaimerMessage.IconType = MOBINFOWARNINGMESSAGEICON.WARNING.ToString();
            }
            else
            {
                boeingDisclaimerMessage.IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString();
            }

            boeingDisclaimerMessage.Messages = new List<string>();
            boeingDisclaimerMessage.Messages.Add((_configuration.GetValue<string>("BOEINGDISCLAIMERMESSAGE") as string) ?? string.Empty);

            return boeingDisclaimerMessage;
        }

        public static bool IsBoeingDisclaimer(List<DisplayTrip> trips)
        {
            bool isBoeingDisclaimer = false;

            foreach (var trip in trips)
            {
                if (trip != null && trip.Flights != null)
                {
                    foreach (var flight in trip.Flights)
                    {
                        if (flight.EquipmentDisclosures != null && IsMaxBoeing(flight.EquipmentDisclosures.EquipmentType))
                        {
                            isBoeingDisclaimer = true;
                            break;
                        }

                        if (flight.Connections != null && flight.Connections.Count > 0)
                        {
                            isBoeingDisclaimer = IsConBoeingDisclaimer(flight);
                        }

                        if (isBoeingDisclaimer)
                            break;
                    }
                }
                if (isBoeingDisclaimer)
                    break;
            }

            return isBoeingDisclaimer;
        }

        public static bool IsMaxBoeing(string boeingType)
        {
            bool isMaxBoeing = false;

            if (!string.IsNullOrEmpty(boeingType))
            {
                string boeingList = _configuration.GetValue<string>("Boeing7MaxCodeList");
                if (boeingList != null)
                {
                    string[] list = boeingList.Split(',');
                    isMaxBoeing = list.Any(l => l.ToUpper().Equals(boeingType.ToUpper()));
                }
            }

            return isMaxBoeing;
        }

        public static bool IsConBoeingDisclaimer(Flight flight)
        {
            bool isBoeingDisclaimer = false;

            foreach (var connection in flight.Connections)
            {
                if (connection.EquipmentDisclosures != null && IsMaxBoeing(connection.EquipmentDisclosures.EquipmentType))
                {
                    isBoeingDisclaimer = true;
                    break;
                }

                if (connection.Connections != null && connection.Connections.Count > 0)
                {
                    isBoeingDisclaimer = IsConBoeingDisclaimer(connection);
                }

                if (isBoeingDisclaimer)
                    break;
            }

            return isBoeingDisclaimer;
        }

        public static bool EnableBoeingDisclaimer(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("ENABLEBOEINGDISCLOUSER") && !isReshop;
        }

        public static InfoWarningMessages GetInhibitMessage(string bookingCutOffMinutes)
        {
            InfoWarningMessages inhibitMessage = new InfoWarningMessages();

            inhibitMessage.Order = MOBINFOWARNINGMESSAGEORDER.INHIBITBOOKING.ToString();
            inhibitMessage.IconType = MOBINFOWARNINGMESSAGEICON.WARNING.ToString();

            inhibitMessage.Messages = new List<string>();

            if (!_configuration.GetValue<bool>("TurnOffBookingCutoffMinsFromCSL") && !string.IsNullOrEmpty(bookingCutOffMinutes))
            {
                inhibitMessage.Messages.Add(string.Format(_configuration.GetValue<string>("InhibitMessageV2"), bookingCutOffMinutes));
            }
            else
            {
                inhibitMessage.Messages.Add((_configuration.GetValue<string>("InhibitMessage") as string) ?? string.Empty);
            }
            return inhibitMessage;
        }

        public static bool IsIBEFullFare(DisplayCart displayCart)
        {
            return EnableIBEFull() &&
                    displayCart != null &&
                    IsIBEFullFare(displayCart.ProductCode);
        }

        public static bool IsIBEFullFare(string productCode)
        {
            var iBEFullProductCodes = _configuration.GetValue<string>("IBEFullShoppingProductCodes");
            return EnableIBEFull() && !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBEFullProductCodes) &&
                   iBEFullProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        public static bool IsIBELiteFare(DisplayCart displayCart)
        {
            return EnableIBELite() &&
                    displayCart != null &&
                    IsIBELiteFare(displayCart.ProductCode);
        }

        public static bool IsIBELiteFare(string productCode)
        {
            var iBELiteProductCodes = _configuration.GetValue<string>("IBELiteShoppingProductCodes");
            return !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBELiteProductCodes) &&
                   iBELiteProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        public static bool EnablePBE()
        {
            return _configuration.GetValue<bool>("EnablePBE");
        }

        public static void GetFlattedFlightsForCOGorThruFlights(Trip trip)
        {
            if (_configuration.GetValue<bool>("SavedTripThruOrCOGFlightBugFix"))
            {
                if (trip.Flights.Any()
                    && trip.Flights.GroupBy(x => x.FlightNumber).Any(g => g.Count() > 1))
                {
                    for (int i = 0; i < trip.Flights.Count - 1; i++)
                    {
                        if (trip.Flights[i].FlightNumber == trip.Flights[i + 1].FlightNumber)
                        {
                            trip.Flights[i].Destination = trip.Flights[i + 1].Destination;
                            trip.Flights.RemoveAt(i + 1);
                            i = -1;
                        }
                    }
                }
            }
            else if (_configuration.GetValue<bool>("UnfinishedBookingCOGFlightsCheck"))
            {
                for (int i = 0; i < trip.Flights.Count - 1; i++)
                {
                    if (trip.Flights[i].FlightNumber == trip.Flights[i + 1].FlightNumber)
                    {
                        trip.Flights[i].Destination = trip.Flights[i + 1].Destination;
                        trip.Flights.RemoveAt(i + 1);
                    }
                }
            }
        }

        public static TripShare IsShareTripValid(SelectTripResponse selectTripResponse)
        {
            var tripShare = new TripShare();
            var reservation = selectTripResponse?.Availability?.Reservation;
            if (reservation != null && (reservation.AwardTravel
                 || reservation.IsEmp20
                 || (reservation.ShopReservationInfo != null && reservation.ShopReservationInfo.IsCorporateBooking)
                 || (reservation.ShopReservationInfo2 != null && reservation.ShopReservationInfo2.IsYATravel)
                 || reservation.IsReshopChange
                 || (_configuration.GetValue<bool>("EnableCorporateLeisure") && reservation?.ShopReservationInfo2?.TravelType == TravelType.CLB.ToString())))
            {
                return tripShare = null;
            }
            else if (selectTripResponse != null && selectTripResponse.Availability != null && reservation != null
                    && reservation.Trips.Count > 0
                    && reservation.FareLock != null && reservation.FareLock.FareLockProducts != null && reservation.FareLock.FareLockProducts.Count > 0)
            {
                tripShare.ShowShareTrip = true;
            }

            return tripShare;
        }

        public static bool EnableReshopCubaTravelReasonVersion(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneEnableReshopCubaTravelReasonVersion", "AndroidEnableReshopCubaTravelReasonVersion", "", "", true, _configuration);
        }

        public static bool IsETCCombinabilityEnabled(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("CombinebilityETCToggle") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCCombinability_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCCombinability_AppVersion")))
            {
                return true;
            }

            return false;
        }

        public static void AssignIsOtherFOPRequired(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, bool IsSecondaryFOP = false, bool isRemoveAll = false)
        {
            var grandTotalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL"));
            //if(grandTotalPrice == null)
            //{
            //    var totalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("TOTAL"));
            //    grandTotalPrice = BuildGrandTotalPriceForReservation(totalPrice.Value);
            //    prices.Add(grandTotalPrice);
            //}
            formofPaymentDetails.IsOtherFOPRequired = (grandTotalPrice.Value > 0);

            //need to update only when travelcertificate is added as formofpayment.
            //Need to update formofpaymentype only when travel certificate is not added as other fop or all the certficates are removed
            if (formofPaymentDetails?.TravelCertificate?.Certificates?.Count > 0 || isRemoveAll)
            {
                if (!formofPaymentDetails.IsOtherFOPRequired && !IsSecondaryFOP)
                {
                    formofPaymentDetails.FormOfPaymentType = MOBFormofPayment.ETC.ToString();
                    if (!_configuration.GetValue<bool>("DisableBugMOBILE9122Toggle") &&
                        !string.IsNullOrEmpty(formofPaymentDetails.CreditCard?.Message) &&
                        _configuration.GetValue<string>("CreditCardDateExpiredMessage").IndexOf(formofPaymentDetails.CreditCard?.Message, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        formofPaymentDetails.CreditCard = null;
                    }
                }
                else
                {
                    formofPaymentDetails.FormOfPaymentType = MOBFormofPayment.CreditCard.ToString();
                }
            }
        }

        public static void UpdateCertificateAmountInTotalPrices(List<MOBSHOPPrice> prices, List<ProdDetail> scProducts, double certificateTotalAmount, bool isShoppingCartProductsGotRefresh = false)
        {
            var certificatePrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "CERTIFICATE");
            var scRESProduct = scProducts.Find(p => p.Code == "RES");
            //var total = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "TOTAL");
            var grandtotal = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "GRAND TOTAL");
            if (certificatePrice == null && certificateTotalAmount > 0)
            {
                certificatePrice = new MOBSHOPPrice();
                UpdateCertificatePrice(certificatePrice, certificateTotalAmount);
                prices.Add(certificatePrice);
            }
            else if (certificatePrice != null)
            {
                //this two lines adding certificate price back to total for removing latest certificate amount in next lines
                if (!isShoppingCartProductsGotRefresh)
                {
                    UtilityHelper.UpdateCertificateRedeemAmountInSCProductPrices(scRESProduct, certificatePrice.Value, false);
                }
                UtilityHelper.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificatePrice.Value, false);
                if (_configuration.GetValue<bool>("MTETCToggle"))
                {
                    UpdateCertificatePrice(certificatePrice, certificateTotalAmount);
                }
            }

            if (certificateTotalAmount == 0 && certificatePrice != null)
            {
                prices.Remove(certificatePrice);
            }

            //UpdateCertificateRedeemAmountFromTotal(total, certificateTotalAmount);
            UtilityHelper.UpdateCertificateRedeemAmountInSCProductPrices(scRESProduct, certificateTotalAmount);
            UtilityHelper.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificateTotalAmount);
        }

        public static MOBSHOPPrice UpdateCertificatePrice(MOBSHOPPrice certificatePrice, double totalAmount)
        {
            certificatePrice.CurrencyCode = "USD";
            certificatePrice.DisplayType = "Certificate";
            certificatePrice.PriceType = "Certificate";
            certificatePrice.PriceTypeDescription = "Electronic travel certificate";
            if (_configuration.GetValue<bool>("MTETCToggle"))
            {
                certificatePrice.Value = totalAmount;
            }
            else
            {
                certificatePrice.Value += totalAmount;
            }
            certificatePrice.Value = Math.Round(certificatePrice.Value, 2, MidpointRounding.AwayFromZero);
            certificatePrice.FormattedDisplayValue = "-" + (certificatePrice.Value).ToString("C2", CultureInfo.CurrentCulture);
            certificatePrice.DisplayValue = string.Format("{0:#,0.00}", certificatePrice.Value);

            return certificatePrice;
        }

        public static bool IsMilesFOPEnabled()
        {
            Boolean isMilesFOP;
            Boolean.TryParse(_configuration.GetValue<string>("EnableMilesAsPayment"), out isMilesFOP);
            return isMilesFOP;
        }

        public static void AssignCertificateTravelers(MOBShoppingCart shoppingCart, FOPTravelerCertificateResponse persistedTravelCertifcateResponse, List<MOBSHOPPrice> prices, MOBApplication application)
        {
            List<MOBFOPCertificateTraveler> certTravelersCopy = null;
            if (persistedTravelCertifcateResponse?.ShoppingCart?.CertificateTravelers != null)
            {
                certTravelersCopy = persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers;
            }

            if (shoppingCart?.SCTravelers != null)
            {
                shoppingCart.CertificateTravelers = new List<MOBFOPCertificateTraveler>();
                if (shoppingCart.SCTravelers.Count > 1)
                {
                    UtilityHelper.AddAllTravelersOptionInCertificateTravelerList(shoppingCart);
                }
                foreach (var traveler in shoppingCart.SCTravelers)
                {
                    if (traveler.IndividualTotalAmount > 0)
                    {
                        MOBFOPCertificateTraveler certificateTraveler = new MOBFOPCertificateTraveler();
                        certificateTraveler.Name = traveler.FirstName + " " + traveler.LastName;
                        certificateTraveler.TravelerNameIndex = traveler.TravelerNameIndex;
                        certificateTraveler.PaxId = traveler.PaxID;
                        MOBFOPCertificateTraveler persistTraveler = certTravelersCopy?.Find(ct => ct.Name == traveler.FirstName + " " + traveler.LastName && traveler.PaxID == ct.PaxId);
                        if (persistTraveler != null)
                        {
                            certificateTraveler.IsCertificateApplied = persistTraveler.IsCertificateApplied;
                        }
                        else
                        {
                            certificateTraveler.IsCertificateApplied = false;
                        }
                        certificateTraveler.IndividualTotalAmount = traveler.IndividualTotalAmount;
                        shoppingCart.CertificateTravelers.Add(certificateTraveler);
                    }
                }

                if (!IsETCCombinabilityEnabled(application.Id, application.Version.Major) &&
                    persistedTravelCertifcateResponse?.ShoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates != null && shoppingCart.SCTravelers.Count > 1 &&
                    persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Exists(c => c.CertificateTraveler != null &&
                                                                                                      !string.IsNullOrEmpty(c.CertificateTraveler.TravelerNameIndex))
                    )
                {
                    UtilityHelper.ClearUnmatchedCertificatesAfterEditTravelers(shoppingCart, persistedTravelCertifcateResponse, prices);
                }
            }
        }

        public static InfoWarningMessages GetIBELiteNonCombinableMessage()
        {
            var message = _configuration.GetValue<string>("IBELiteNonCombinableMessage");
            return UtilityHelper.BuildInfoWarningMessages(message);
        }

        public static bool IncludeReshopFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableReshopFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }

        public static WorkFlowType GetWorkFlowType(string flow, string productCode = "")
        {
            switch (flow)
            {
                case "CHECKIN":
                    return WorkFlowType.CheckInProductsPurchase;

                case "BOOKING":
                    return WorkFlowType.InitialBooking;

                case "VIEWRES":
                case "POSTBOOKING":
                case "VIEWRES_SEATMAP":
                    if (productCode == "RES")
                        return WorkFlowType.FareLockPurchase;
                    else if (IsPOMOffer(productCode))
                        return WorkFlowType.PreOrderMeals;
                    else
                        return WorkFlowType.PostPurchase;

                case "RESHOP":
                    return WorkFlowType.Reshop;

                case "FARELOCK":
                    return WorkFlowType.FareLockPurchase;
            }
            return WorkFlowType.UnKnown;
        }

        public static bool IsPOMOffer(string productCode)
        {
            if (!_configuration.GetValue<bool>("EnableInflightMealsRefreshment")) return false;
            if (string.IsNullOrEmpty(productCode)) return false;
            return (productCode == _configuration.GetValue<string>("InflightMealProductCode"));
        }

        public static bool EnableReshopMixedPTC(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidVersion_ReshopEnableMixedPTC", "iphoneVersion_ReshopEnableMixedPTC", "", "", true, _configuration);
        }

        public static bool IncludeFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }

        private static void AssignCertificateTravelers(MOBShoppingCart shoppingCart)
        {
            if (shoppingCart?.SCTravelers != null)
            {
                shoppingCart.CertificateTravelers = new List<MOBFOPCertificateTraveler>();

                foreach (var traveler in shoppingCart.SCTravelers)
                {
                    if (traveler.IndividualTotalAmount > 0)
                    {
                        MOBFOPCertificateTraveler certificateTraveler = new MOBFOPCertificateTraveler();
                        certificateTraveler.Name = traveler.FirstName + " " + traveler.LastName;
                        certificateTraveler.TravelerNameIndex = traveler.TravelerNameIndex;
                        certificateTraveler.PaxId = traveler.PaxID;
                        certificateTraveler.IndividualTotalAmount = traveler.IndividualTotalAmount;
                        shoppingCart.CertificateTravelers.Add(certificateTraveler);
                    }
                }
            }
        }

        public static bool IncludeMoneyPlusMiles(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableMilesPlusMoney")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidMilesPlusMoneyVersion", "iPhoneMilesPlusMoneyVersion", "", "", true, _configuration);
        }
        
        public static double GetAlowedETCAmount(List<ProdDetail> products, string flow)
        {
            string allowedETCAncillaryProducts = string.Empty;
            if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && flow == United.Utility.Enum.FlowType.VIEWRES.ToString())
            {
                allowedETCAncillaryProducts = _configuration.GetValue<string>("VIewResETCEligibleProducts");
            }
            else
            {
                allowedETCAncillaryProducts = _configuration.GetValue<string>("CombinebilityETCAppliedAncillaryCodes");
            }
            double maximumAllowedETCAmount = Convert.ToDouble(_configuration.GetValue<string>("CombinebilityMaxAmountOfETCsAllowed"));
            double allowedETCAmount = products == null ? 0 : products.Where(p => (p.Code == "RES" || allowedETCAncillaryProducts.IndexOf(p.Code) > -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            if (_configuration.GetValue<bool>("ETCForAllProductsToggle"))
            {
                allowedETCAmount += GetBundlesAmount(products, flow);
            }
            if (allowedETCAmount > maximumAllowedETCAmount)
            {
                allowedETCAmount = maximumAllowedETCAmount;
            }
            return allowedETCAmount;
        }

        public static bool IsCorporateLeisureFareSelected(List<MOBSHOPTrip> trips)
        {
            string corporateFareText = _configuration.GetValue<string>("FSRLabelForCorporateLeisure");
            if (trips != null)
            {
                return trips.Any(
                   x =>
                       x.FlattenedFlights.Any(
                           f =>
                               f.Flights.Any(
                                   fl =>
                                       fl.CorporateFareIndicator ==
                                       corporateFareText.ToString())));
            }

            return false;
        }

        public static void UpdateCertificateAmountInTotalPrices(List<MOBSHOPPrice> prices, double certificateTotalAmount)
        {
            var certificatePrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "CERTIFICATE");
            var grandtotal = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "GRAND TOTAL");

            if (certificatePrice == null && certificateTotalAmount > 0)
            {
                certificatePrice = new Mobile.Model.Shopping.MOBSHOPPrice();
                UpdateCertificatePrice(certificatePrice, certificateTotalAmount);
                prices.Add(certificatePrice);
            }
            else if (certificatePrice != null)
            {
                UtilityHelper.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificatePrice.Value, false);
                UpdateCertificatePrice(certificatePrice, certificateTotalAmount);
            }

            if (certificateTotalAmount == 0 && certificatePrice != null)
            {
                prices.Remove(certificatePrice);
            }

            UtilityHelper.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificateTotalAmount);
        }

        private static double GetNotAlowedETCAmount(List<ProdDetail> products, string flow)
        {
            return products.Sum(a => Convert.ToDouble(a.ProdTotalPrice)) - GetAlowedETCAmount(products, flow);
        }

        public static double GetBundlesAmount(List<ProdDetail> products, string flow)
        {
            string nonBundleProductCode = _configuration.GetValue<string>("NonBundleProductCode");
            double bundleAmount = products == null ? 0 : products.Where(p => (nonBundleProductCode.IndexOf(p.Code) == -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            return bundleAmount;
        }

        public static List<FormofPaymentOption> BuildEligibleFormofPaymentsResponse(List<FormofPaymentOption> response, MOBShoppingCart shoppingCart, MOBRequest request)
        {
            bool isTravelCertificateAdded = shoppingCart.FormofPaymentDetails?.TravelCertificate != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Count > 0;
            if (_configuration.GetValue<bool>("EnableEtcinManageresforPreviewTesting"))
            {
                string allowedETCAncillaryProducts = _configuration.GetValue<string>("VIewResETCEligibleProducts");
                if (shoppingCart.Products.Any(p => allowedETCAncillaryProducts.IndexOf(p.Code) > -1))
                {
                    FormofPaymentOption elgibileOption = new FormofPaymentOption();
                    elgibileOption.Category = "CERT";
                    elgibileOption.Code = "ETC";
                    elgibileOption.FoPDescription = "Travel Certificate";
                    elgibileOption.FullName = "Electronic travel certificate";
                    response.Add(elgibileOption);
                }
            }
            if (isTravelCertificateAdded)
            {
                if (shoppingCart?.FormofPaymentDetails?.TravelCertificate?.AllowedETCAmount > shoppingCart?.FormofPaymentDetails?.TravelCertificate?.TotalRedeemAmount
                    && shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count < shoppingCart?.FormofPaymentDetails.TravelCertificate?.MaxNumberOfETCsAllowed)
                {
                    response = response.Where(x => x.Category == "CC" || x.Category == "CERT").ToList();
                }
                else
                {
                    response = response.Where(x => x.Category == "CC").ToList();
                }
            }

            if (shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.ApplePay.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.PayPal.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.PayPalCredit.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.Masterpass.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.Uplift.ToString())
            {
                response = response.Where(x => x.Category != "CERT").ToList();
            }

            if (response.Exists(x => x.Category == "CERT"))
            {
                response.Where(x => x.Category == "CERT").FirstOrDefault().FullName = _configuration.GetValue<string>("ETCFopFullName");
            }

            return response;
        }

        public static List<FormofPaymentOption> BuildEligibleFormofPaymentsResponse(List<FormofPaymentOption> response, MOBShoppingCart shoppingCart, Session session, MOBRequest request, bool isMetaSearch = false)
        {
            //Metasearch
            if (!_configuration.GetValue<bool>("EnableETCFopforMetaSearch") && isMetaSearch && _configuration.GetValue<bool>("CreditCardFOPOnly_MetaSearch"))
            {
                return response;
            }
            if (_configuration.GetValue<bool>("EnableFFCinBookingforPreprodTesting"))
            {
                if (!response.Exists(x => x.Category == "CERT" && x.Code == "FF"))
                {
                    FormofPaymentOption elgibileOption = new FormofPaymentOption();
                    elgibileOption.Category = "CERT";
                    elgibileOption.Code = "FF";
                    elgibileOption.FoPDescription = "Future flight credit";
                    elgibileOption.FullName = "Future flight credit";
                    response.Add(elgibileOption);
                }
            }
            bool isMultiTraveler = shoppingCart.SCTravelers?.Count > 1;
            bool isTravelCertificateAdded = shoppingCart.FormofPaymentDetails.TravelCertificate != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Count > 0;
            bool isETCCombinabilityChangeEnabled = IsETCCombinabilityEnabled(request.Application.Id, request.Application.Version.Major);
            bool isFFCAdded = shoppingCart.FormofPaymentDetails.TravelFutureFlightCredit != null && shoppingCart.FormofPaymentDetails.TravelFutureFlightCredit.FutureFlightCredits != null && shoppingCart.FormofPaymentDetails.TravelFutureFlightCredit.FutureFlightCredits.Count > 0;
            if (IsETCEligibleTravelType(session) && !isMultiTraveler && !isETCCombinabilityChangeEnabled)//Check whether ETC Eligible booking Type
            {
                //If travel certificate is added only creditcard should be allowed
                if (isTravelCertificateAdded)
                {
                    response = response.Where(x => x.Category == "CC").ToList();
                }
            }
            else if (IsETCEligibleTravelType(session) && isMultiTraveler && !isETCCombinabilityChangeEnabled)
            {
                if (IsETCEnabledforMultiTraveler(request.Application.Id, request.Application.Version.Major.ToString()) && isTravelCertificateAdded && shoppingCart.IsMultipleTravelerEtcFeatureClientToggleEnabled)
                {
                    //Entire reservation price is covered with ETC..it doesnt matter whether Ancillary is added or not we need to show only credit card
                    if (shoppingCart.Products != null && shoppingCart.Products.Exists(x => x.Code == "RES") && Convert.ToDecimal(shoppingCart.Products?.Where(x => x.Code == "RES").FirstOrDefault().ProdTotalPrice) == 0)
                    {
                        response = response.Where(x => x.Category == "CC").ToList();
                    }//There is residual amount left on reservation and Ancillary products amount
                    else if (shoppingCart.Products != null && shoppingCart.Products.Exists(x => x.Code == "RES") && Convert.ToDecimal(shoppingCart.Products?.Where(x => x.Code == "RES").FirstOrDefault().ProdTotalPrice) > 0)
                    {
                        //If there is residual amount left on reservation but apply for all traveler option is selected it doesnt matter whether we added ancillary or not we need to show only credit card
                        if (shoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates[0]?.IsForAllTravelers == true)
                        {
                            response = response.Where(x => x.Category == "CC").ToList();
                        }
                        else
                        {
                            if (UtilityHelper.IsCertificatesAppliedforAllIndividualTravelers(shoppingCart))
                            {
                                response = response.Where(x => x.Category == "CC").ToList();
                            }
                            else
                            {
                                response = response.Where(x => x.Category == "CC" || x.Category == "CERT").ToList();
                            }
                        }
                    }
                }
            }
            else if (IsETCEligibleTravelType(session) && isETCCombinabilityChangeEnabled && isTravelCertificateAdded)
            {
                if (shoppingCart?.FormofPaymentDetails?.TravelCertificate?.AllowedETCAmount > shoppingCart?.FormofPaymentDetails?.TravelCertificate?.TotalRedeemAmount &&
                    (_configuration.GetValue<bool>("ETCMaxCountCheckToggle") ? shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count < shoppingCart?.FormofPaymentDetails.TravelCertificate?.MaxNumberOfETCsAllowed : true))
                {
                    response = response.Where(x => x.Category == "CC" || (x.Category == "CERT" && x.Code == "ETC")).ToList();
                }
                else
                {
                    response = response.Where(x => x.Category == "CC").ToList();
                }
            }
            else if (IsETCEligibleTravelType(session, "FFCEligibleTravelTypes") && isFFCAdded && IncludeFFCResidual(request.Application.Id, request.Application.Version.Major))
            {
                if (shoppingCart?.FormofPaymentDetails?.TravelFutureFlightCredit?.AllowedFFCAmount > shoppingCart?.FormofPaymentDetails?.TravelFutureFlightCredit?.TotalRedeemAmount)
                {
                    response = response.Where(x => x.Category == "CC" || (x.Category == "CERT" && x.Code == "FF")).ToList();
                }
                else
                {
                    response = response.Where(x => x.Category == "CC").ToList();
                }
            }
            else if ((!IsETCEligibleTravelType(session) || !IsETCEligibleTravelType(session, "FFCEligibleTravelTypes"))) //ETC Shouldn't be allowed for ineligible travel types
            {
                if (!IncludeFFCResidual(request.Application.Id, request.Application.Version.Major))
                {
                    response = response.Where(x => x.Category != "CERT").ToList();
                }
                else
                {
                    if (!IsETCEligibleTravelType(session))
                    {
                        var etcFOP = response.Where(x => x.Category == "CERT" && x.Code == "ETC").FirstOrDefault();
                        if (etcFOP != null)
                            response.Remove(etcFOP);
                    }
                    if (!IsETCEligibleTravelType(session, "FFCEligibleTravelTypes"))
                    {
                        var ffcFOP = response.Where(x => x.Category == "CERT" && x.Code == "FF").FirstOrDefault();
                        if (ffcFOP != null)
                            response.Remove(ffcFOP);
                    }
                }
            }
            if ((!_configuration.GetValue<bool>("EnableETCFopforMetaSearch") ? !isMetaSearch : true)//to enable ETC for metasearch               
                && /*IsShoppingCarthasOnlyFareLockProduct(shoppingCart)*/
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.ApplePay.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.PayPal.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.PayPalCredit.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.Masterpass.ToString() ||
                shoppingCart?.FormofPaymentDetails?.FormOfPaymentType == MOBFormofPayment.Uplift.ToString() ||
                (!(IsETCEnabledforMultiTraveler(request.Application.Id, request.Application.Version.Major.ToString()) && shoppingCart.IsMultipleTravelerEtcFeatureClientToggleEnabled) ? (isMultiTraveler) : false))
            {
                response = response.Where(x => x.Category != "CERT").ToList();
                if (IncludeMoneyPlusMiles(request.Application.Id, request.Application.Version.Major))
                {
                    response = response.Where(x => x.Category != "MILES").ToList();
                }
                if (IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                {
                    response = response.Where(x => x.Category != "TRAVELBANK").ToList();
                }
                if (IncludeTravelCredit(request.Application.Id, request.Application.Version.Major))
                {
                    response = response.Where(x => x.Category != "TRAVELCREDIT").ToList();
                }
            }

            if (response.Exists(x => x.Category == "CERT" && x.Code == "ETC"))
            {
                response.Where(x => x.Category == "CERT" && x.Code == "ETC").FirstOrDefault().FullName = _configuration.GetValue<string>("ETCFopFullName");
            }
            if (response.Exists(x => x.Category == "CERT" && x.Code == "FF"))
            {
                response.Where(x => x.Category == "CERT" && x.Code == "FF").FirstOrDefault().FullName = _configuration.GetValue<string>("FFCFopFullName");
            }

            return response;
        }

        public static bool IsETCEnabledforMultiTraveler(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("MTETCToggle") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCForMultiTraveler_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCForMultiTraveler_AppVersion")))
            {
                return true;
            }
            return false;
        }

        public static bool IsETCEligibleTravelType(Session session, string travelTypeConfigKey = "ETCEligibleTravelTypes")
        {
            string[] travelTypes = _configuration.GetValue<string>(travelTypeConfigKey).Split('|');//"Revenue|YoungAdult"
            bool isEligible = false;
            if (session.IsAward && travelTypes.Contains("Award"))
            {
                isEligible = true;
            }
            else if (!string.IsNullOrEmpty(session.EmployeeId) && travelTypes.Contains("UADiscount"))
            {
                isEligible = true;
            }
            else if (session.IsYoungAdult && travelTypes.Contains("YoungAdult"))
            {
                isEligible = true;
            }
            else if (session.IsCorporateBooking && travelTypes.Contains("Corporate"))
            {
                isEligible = true;
            }
            else if (session.TravelType == TravelType.CLB.ToString() && travelTypes.Contains("CorporateLeisure"))
            {
                isEligible = true;
            }
            else if (!session.IsAward && string.IsNullOrEmpty(session.EmployeeId) && !session.IsYoungAdult && !session.IsCorporateBooking && session.TravelType != TravelType.CLB.ToString() && travelTypes.Contains("Revenue"))
            {
                isEligible = true;
            }
            return isEligible;
        }

        public static Collection<FOPProduct> GetProductsForEligibleFopRequest(MOBShoppingCart shoppingCart, SeatChangeState state = null)
        {
            if (shoppingCart == null || shoppingCart.Products == null || !shoppingCart.Products.Any())
                return null;

            var products = shoppingCart.Products.GroupBy(k => new { k.Code, k.ProdDescription }).Select(x => new FOPProduct { Code = x.Key.Code, ProductDescription = x.Key.ProdDescription }).ToCollection();
            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes")))
            {
                if (!_configuration.GetValue<bool>("ByPassAddingPCUProductToEligibleFopRequest"))
                {
                    UtilityHelper.AddPCUToRequestWhenPCUSeatIsSelected(state, ref products);
                }
            }

            return products;
        }

        public static bool IsEligibileForUplift(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart)
        {
            if (shoppingCart?.Flow?.ToUpper() == United.Utility.Enum.FlowType.VIEWRES.ToString().ToUpper())
            {
                return HasEligibleProductsForUplift(shoppingCart.TotalPrice, shoppingCart.Products);
            }

            if (!_configuration.GetValue<bool>("EnableUpliftPayment"))
                return false;

            if (reservation == null || reservation.Prices == null || shoppingCart == null || shoppingCart?.Flow != United.Utility.Enum.FlowType.BOOKING.ToString())
                return false;

            if (reservation.ShopReservationInfo?.IsCorporateBooking ?? false)
                return false;

            if (shoppingCart.Products?.Any(p => p?.Code == "FLK") ?? false)
                return false;

            if (!_configuration.GetValue<bool>("DisableFixForUpliftFareLockDefect"))
            {
                if (shoppingCart.Products?.Any(p => p?.Code?.ToUpper() == "FARELOCK") ?? false)
                    return false;
            }

            if (reservation.Prices.Any(p => "TOTALPRICEFORUPLIFT".Equals(p.DisplayType, StringComparison.CurrentCultureIgnoreCase) && p.Value >= MinimumPriceForUplift && p.Value <= MaxmimumPriceForUplift) &&
               (shoppingCart?.SCTravelers?.Any(t => t?.TravelerTypeCode == "ADT" || t?.TravelerTypeCode == "SNR") ?? false))
            {
                return true;
            }
            return false;
        }

        public static bool HasEligibleProductsForUplift(string totalPrice, List<ProdDetail> products)
        {
            decimal.TryParse(totalPrice, out decimal price);
            if (price >= MinimumPriceForUplift && price <= MaxmimumPriceForUplift)
            {
                var eligibleProductsForUplift = _configuration.GetValue<string>("EligibleProductsForUpliftInViewRes").Split(',');
                if (eligibleProductsForUplift.Any())
                {
                    return products.Any(p => eligibleProductsForUplift.Contains(p.Code));
                }
            }

            return false;
        }

        public static int MinimumPriceForUplift
        {
            get
            {
                var minimumAmountForUplift = _configuration.GetValue<string>("MinimumPriceForUplift");
                if (string.IsNullOrEmpty(minimumAmountForUplift))
                    return 300;

                int.TryParse(minimumAmountForUplift, out int upliftMinAmount);
                return upliftMinAmount;
            }
        }

        public static int MaxmimumPriceForUplift
        {
            get
            {
                var maximumAmountForUplift = _configuration.GetValue<string>("MaximumPriceForUplift");
                if (string.IsNullOrEmpty(maximumAmountForUplift))
                    return 150000;

                int.TryParse(maximumAmountForUplift, out int upliftMaxAmount);
                return upliftMaxAmount;
            }
        }


        public static bool IncludeMOBILE12570ResidualFix(int appId, string appVersion)
        {
            bool isApplicationGreater = GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidMOBILE12570ResidualVersion", "iPhoneMOBILE12570ResidualVersion", "", "", true, _configuration);
            return (_configuration.GetValue<bool>("eableMOBILE12570Toggle") && isApplicationGreater);
        }


        public static bool IsManageResETCEnabled(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCManageRes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCManageRes_AppVersion")))
            {
                return true;
            }
            return false;
        }

        public static void UpdateSavedCertificate(MOBShoppingCart shoppingcart)
        {
            if (_configuration.GetValue<bool>("SavedETCToggle") && shoppingcart != null)
            {
                var shoppingCaartCertificates = shoppingcart.ProfileTravelerCertificates;
                var appliedCertificates = shoppingcart.FormofPaymentDetails?.TravelCertificate?.Certificates;
                if (shoppingCaartCertificates?.Count > 0 && appliedCertificates != null)
                {
                    foreach (var shoppingCaartCertificate in shoppingCaartCertificates)
                    {
                        var appliedCertificate = appliedCertificates.Exists(c => c.Index == shoppingCaartCertificate.Index);
                        shoppingCaartCertificate.IsCertificateApplied = appliedCertificate;
                        if (appliedCertificate)
                        {
                            appliedCertificates.Find(c => c.Index == shoppingCaartCertificate.Index).IsProfileCertificate = appliedCertificate;
                        }
                    }
                }
            }
        }
        //public static  List<MOBItem> GetCaptions(string key)
        //{
        //    //return !string.IsNullOrEmpty(key) ? GetCaptions(new List<string> { key }, true) : null;
        //}

        //public static  List<MOBItem> GetCaptions(List<string> keyList, bool isTnC)
        //{
        //    var docs = GetNewLegalDocumentsForTitles(keyList, isTnC);
        //    if (docs == null || !docs.Any()) return null;

        //    var captions = new List<MOBItem>();

        //    captions.AddRange(
        //        docs.Select(doc => new MOBItem
        //        {
        //            Id = doc.Title,
        //            CurrentValue = doc.Document
        //        }));
        //    return captions;
        //}

        public static string BuilTripShareEmailBodyTripText(string tripType, List<MOBSHOPTrip> trips, bool isHtml)
        {
            string emailBodyTripText = string.Empty;
            string originCityOnly, destinationCityOnly;


            if (string.IsNullOrEmpty(trips[0].OriginDecodedWithCountry) || string.IsNullOrEmpty(trips[0].DestinationDecodedWithCountry))
            {
                originCityOnly = trips[0].OriginDecoded.Split(',')[0].Trim();
                destinationCityOnly = trips[0].DestinationDecoded.Split(',')[0].Trim();
            }
            else
            {
                originCityOnly = trips[0].OriginDecodedWithCountry.Split(',')[0].Trim();
                destinationCityOnly = trips[0].DestinationDecodedWithCountry.Split(',')[0].Trim();
            }

            if (tripType == "OW")
            {
                if (isHtml)
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyTripText");
                }
                else
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIPlaceholderTitleText");
                }
                emailBodyTripText = emailBodyTripText.Replace("{tripType}", "One-way");
                emailBodyTripText = emailBodyTripText.Replace("{originWithStateCode}", originCityOnly);
                emailBodyTripText = emailBodyTripText.Replace("{destinationWithStateCode}", destinationCityOnly);
            }
            else if (tripType == "RT")
            {
                if (isHtml)
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyTripText");
                }
                else
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIPlaceholderTitleText");
                }
                emailBodyTripText = emailBodyTripText.Replace("{tripType}", "Roundtrip");
                emailBodyTripText = emailBodyTripText.Replace("{originWithStateCode}", originCityOnly);
                emailBodyTripText = emailBodyTripText.Replace("{destinationWithStateCode}", destinationCityOnly);
            }
            else if (tripType == "MD")
            {
                if (isHtml)
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyTripMultiSegmentText");
                }
                else
                {
                    emailBodyTripText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyTripMultiSegmentTextNonHtml");
                }

                string numberOfFlights = $"{trips.Count - 1}";

                emailBodyTripText = emailBodyTripText.Replace("{originWithStateCode}", originCityOnly);
                emailBodyTripText = emailBodyTripText.Replace("{destinationWithStateCode}", destinationCityOnly);
                emailBodyTripText = emailBodyTripText.Replace("{numberOfFlights}", numberOfFlights);

            }
            return emailBodyTripText;
        }
        public static void AddPromoDetailsInSegments(ProdDetail prodDetail)
        {
            if (prodDetail?.Segments != null)
            {
                double promoValue;
                prodDetail?.Segments.ForEach(p =>
                {
                    p.SubSegmentDetails.ForEach(subSegment =>
                    {
                        if (!string.IsNullOrEmpty(subSegment.OrginalPrice) && !string.IsNullOrEmpty(subSegment.Price))
                        {
                            promoValue = Convert.ToDouble(subSegment.OrginalPrice) - Convert.ToDouble(subSegment.Price);
                            subSegment.Price = subSegment.OrginalPrice;
                            subSegment.DisplayPrice = Decimal.Parse(subSegment.Price).ToString("c");
                            if (promoValue > 0)
                            {
                                subSegment.PromoDetails = new MOBPromoCode
                                {
                                    PriceTypeDescription = _configuration.GetValue<string>("PromoCodeAppliedText"),
                                    PromoValue = Math.Round(promoValue, 2, MidpointRounding.AwayFromZero),
                                    FormattedPromoDisplayValue = "-" + promoValue.ToString("C2", CultureInfo.CurrentCulture)
                                };
                            }
                        }
                    });

                });
            }
        }
        public static string BuildTripSharePrice(string priceWithCurrency, string currencyCode, string redirectUrl)
        {
            string emailBodyBodyPriceText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyPriceText");
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{serverCurrentDateTime}", DateTime.Now.ToString("MMM d 'at' h:mm tt"));
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{priceWithCurrency}", priceWithCurrency);
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{currencyCode}", currencyCode);
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{redirectUrl}", redirectUrl);
            return emailBodyBodyPriceText;
        }
        public static string BuildTripShareSegmentText(MOBSHOPTrip trip)
        {
            string bodyEmailSegmentCompleteText = string.Empty;

            string emailBodySegmentText = string.Empty;
            string emailBodySegmentConnectionText = string.Empty;

            string segmentDuration = string.Empty;
            string departureTime = string.Empty;
            string arrivalTime = string.Empty;

            string departureAirportWithCountryCode = string.Empty;
            string arrivalAirportWithCountryCode = string.Empty;

            //example 1h 7m connection in Chicago, IL, US (ORD - O'Hare)
            string connectionWithStateAirportCodeAndName = string.Empty;
            string emailBodySegmentOperatedByText = string.Empty;

            //string connectionDuration = string.Empty;
            string operatedByText = string.Empty;

            foreach (var flattenedFlight in trip.FlattenedFlights)
            {
                if (flattenedFlight != null)
                {
                    foreach (var flight in flattenedFlight.Flights)
                    {
                        if (!flight.IsConnection && !string.IsNullOrEmpty(flight.TotalTravelTime))
                        {
                            segmentDuration = flight.TotalTravelTime;
                        }

                        if (flight.IsConnection)
                        {
                            //connectionWithStateAirportCodeAndName = $"{flight.ConnectTimeMinutes} connection in {flight.OriginDescription}";
                            connectionWithStateAirportCodeAndName = $"{flight.ConnectTimeMinutes} connection in {flight.OriginDecodedWithCountry}";
                            emailBodySegmentConnectionText += _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodySegmentConnectionText");

                            emailBodySegmentConnectionText = emailBodySegmentConnectionText.Replace("{connectionDurationAndWithStateAirportCode}", connectionWithStateAirportCodeAndName);
                        }
                        else if (flight.IsStopOver)
                        {
                            //connectionWithStateAirportCodeAndName = $"{flight.GroundTime} connection in {flight.OriginDescription}";
                            connectionWithStateAirportCodeAndName = $"{flight.GroundTime} connection in {flight.OriginDecodedWithCountry}";
                            emailBodySegmentConnectionText += _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodySegmentConnectionText");

                            emailBodySegmentConnectionText = emailBodySegmentConnectionText.Replace("{connectionDurationAndWithStateAirportCode}", connectionWithStateAirportCodeAndName);
                        }

                        //if (string.IsNullOrEmpty(operatedByText) &&  !string.IsNullOrEmpty(flight.OperatingCarrierDescription))
                        if (!string.IsNullOrEmpty(flight.OperatingCarrierDescription))
                        {
                            if (string.IsNullOrEmpty(operatedByText))
                            {
                                operatedByText = flight.OperatingCarrierDescription;
                            }
                            else
                            {
                                operatedByText = $"{operatedByText}, {flight.OperatingCarrierDescription}";
                            }
                        }
                    }
                    if (flattenedFlight.Flights != null && flattenedFlight.Flights.Count > 0)
                    {
                        departureTime = flattenedFlight.Flights[0].DepartureDateTime;
                        arrivalTime = flattenedFlight.Flights[flattenedFlight.Flights.Count - 1].ArrivalDateTime;
                        departureAirportWithCountryCode = flattenedFlight.Flights.FirstOrDefault().OriginDecodedWithCountry;
                        arrivalAirportWithCountryCode = flattenedFlight.Flights.Last().DestinationDecodedWithCountry;
                    }
                }
            }

            emailBodySegmentText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodySegmentText");
            emailBodySegmentText = emailBodySegmentText.Replace("{segmentDuration}", segmentDuration);
            //emailBodySegmentText = emailBodySegmentText.Replace("{originWithStateAirportCode}", trip.OriginDecodedWithCountry);
            //emailBodySegmentText = emailBodySegmentText.Replace("{destinationWithStateAirportCode}", trip.DestinationDecodedWithCountry);
            emailBodySegmentText = emailBodySegmentText.Replace("{originWithStateAirportCode}", departureAirportWithCountryCode);
            emailBodySegmentText = emailBodySegmentText.Replace("{destinationWithStateAirportCode}", arrivalAirportWithCountryCode);

            emailBodySegmentText = emailBodySegmentText.Replace("{departureTime}", DateTime.Parse(departureTime).ToString("ddd, MMM dd, yyyy, h:mm tt"));
            emailBodySegmentText = emailBodySegmentText.Replace("{arrivalTime}", DateTime.Parse(arrivalTime).ToString("ddd, MMM dd, yyyy, h:mm tt"));

            if (!string.IsNullOrEmpty(operatedByText))
            {
                emailBodySegmentOperatedByText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodySegmentOperatedByText");
                emailBodySegmentOperatedByText = emailBodySegmentOperatedByText.Replace("{OperatingCarrierName}", operatedByText);
            }

            bodyEmailSegmentCompleteText = $"{emailBodySegmentText}{emailBodySegmentConnectionText}{emailBodySegmentOperatedByText}";

            return bodyEmailSegmentCompleteText;
        }

        public static void AddCouponDetails(List<ProdDetail> prodDetails, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse cslFlightReservationResponse, bool isPost, string flow, MOBApplication application)
        {
            United.Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart = new United.Service.Presentation.InteractionModel.ShoppingCart();
            flightReservationResponseShoppingCart = isPost ? cslFlightReservationResponse.CheckoutResponse.ShoppingCart : cslFlightReservationResponse.ShoppingCart;
            foreach (var prodDetail in prodDetails)
            {
                var product = flightReservationResponseShoppingCart.Items.SelectMany(I => I.Product).Where(p => p.Code == prodDetail.Code).FirstOrDefault();
                if (product != null && product.CouponDetails != null && product.CouponDetails.Any(c => c != null) && product.CouponDetails.Count() > 0)
                {
                    prodDetail.CouponDetails = new List<CouponDetails>();
                    foreach (var coupon in product.CouponDetails)
                    {
                        if (coupon != null)
                        {
                            prodDetail.CouponDetails.Add(new CouponDetails
                            {
                                PromoCode = coupon.PromoCode,
                                Product = coupon.Product,
                                IsCouponEligible = coupon.IsCouponEligible,
                                Description = coupon.Description,
                                DiscountType = coupon.DiscountType
                            });
                        }
                    }
                }
                if (flow == FlowType.POSTBOOKING.ToString() && prodDetail.CouponDetails != null && prodDetail.CouponDetails.Count > 0
                     || (flow == FlowType.BOOKING.ToString() && prodDetail.CouponDetails != null && IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major, true)))
                {
                    AddPromoDetailsInSegments(prodDetail);
                }
            }
        }

        public static bool IsOriginalPriceExists(ProdDetail prodDetail)
        {
            return !_configuration.GetValue<bool>("DisableFreeCouponFix")
                   && !string.IsNullOrEmpty(prodDetail.ProdOriginalPrice)
                   && Decimal.TryParse(prodDetail.ProdOriginalPrice, out decimal originalPrice)
                   && originalPrice > 0;
        }

        public static string BuildProductDescription(Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> travelOptions, IGrouping<string, SubItem> t, string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return string.Empty;

            productCode = productCode.ToUpper().Trim();

            if (productCode == "AAC")
                return "Award Accelerator®";

            if (productCode == "PAC")
                return "Premier Accelerator℠";

            if (productCode == "TPI" && _configuration.GetValue<bool>("GetTPIProductName_HardCode"))
                return "Travel insurance";
            if (productCode == "FARELOCK")
                return "FareLock";

            if (_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes") && productCode == "BEB")
                return !_configuration.GetValue<bool>("EnableNewBEBContentChange") ? "Switch to Economy" : _configuration.GetValue<string>("BEBuyOutPaymentInformationMessage");

            if (productCode == "PCU")
                return GetFormattedCabinName(t.Select(u => u.Description).FirstOrDefault().ToString());


            return travelOptions.Where(d => d.Key == productCode).Select(d => d.Type).FirstOrDefault().ToString();
        }

        public static string GetFormattedCabinName(string cabinName)
        {
            if (!_configuration.GetValue<bool>("EnablePcuMultipleUpgradeOptions"))
            {
                return cabinName;
            }

            if (string.IsNullOrWhiteSpace(cabinName))
                return string.Empty;

            switch (cabinName.ToUpper().Trim())
            {
                case "UNITED FIRST":
                    return "United First®";
                case "UNITED BUSINESS":
                    return "United Business®";
                case "UNITED POLARIS FIRST":
                    return "United Polaris℠ first";
                case "UNITED POLARIS BUSINESS":
                    return "United Polaris℠ business";
                case "UNITED PREMIUM PLUS":
                    return "United® Premium Plus";
                default:
                    return string.Empty;
            }
        }

        public static string BuildSegmentInfo(string productCode, Collection<ReservationFlightSegment> flightSegments, IGrouping<string, SubItem> x)
        {
            if (productCode == "AAC" || productCode == "PAC")
                return string.Empty;

            if (_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes") && productCode == "BEB")
            {
                var tripNumber = flightSegments?.Where(y => y.FlightSegment.SegmentNumber == Convert.ToInt32(x.Select(u => u.SegmentNumber).FirstOrDefault())).FirstOrDefault().TripNumber;
                var tripFlightSegments = flightSegments?.Where(c => c != null && !string.IsNullOrEmpty(c.TripNumber) && c.TripNumber.Equals(tripNumber)).ToCollection();
                if (tripFlightSegments != null && tripFlightSegments.Count > 1)
                {
                    return tripFlightSegments?.FirstOrDefault()?.FlightSegment?.DepartureAirport?.IATACode + " - " + tripFlightSegments?.LastOrDefault()?.FlightSegment?.ArrivalAirport?.IATACode;
                }
                else
                {
                    return flightSegments.Where(y => y.FlightSegment.SegmentNumber == Convert.ToInt32(x.Select(u => u.SegmentNumber).FirstOrDefault())).Select(y => y.FlightSegment.DepartureAirport.IATACode + " - " + y.FlightSegment.ArrivalAirport.IATACode).FirstOrDefault().ToString();
                }
            }

            return flightSegments.Where(y => y.FlightSegment.SegmentNumber == Convert.ToInt32(x.Select(u => u.SegmentNumber).FirstOrDefault())).Select(y => y.FlightSegment.DepartureAirport.IATACode + " - " + y.FlightSegment.ArrivalAirport.IATACode).FirstOrDefault().ToString();
        }

        public static List<ProductSegmentDetail> GetProductSegmentForInFlightMeals(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse,
            List<MOBInFlightMealsRefreshmentsResponse> savedResponse, Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> travelOptions, United.Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart)
        {
            List<ProductSegmentDetail> response = new List<ProductSegmentDetail>();
            ProductSegmentDetail segmentDetail = new ProductSegmentDetail();
            List<ProductSubSegmentDetail> subSegmentDetails = new List<ProductSubSegmentDetail>();
            var traveler = flightReservationResponse?.Reservation?.Travelers;
            string productCode = _configuration.GetValue<string>("InflightMealProductCode");

            var subProducts = flightReservationResponseShoppingCart.Items
           ?.Where(a => a.Product != null)
           ?.SelectMany(b => b.Product)
           ?.Where(c => c.SubProducts != null && c.SubProducts.Any(d => d.Code == _configuration.GetValue<string>("InflightMealProductCode")))
           ?.SelectMany(d => d.SubProducts);

            var characterStics = flightReservationResponseShoppingCart.Items
           ?.Where(a => a.Product != null)
           ?.SelectMany(b => b.Product)
           ?.Where(c => c.Code == productCode)
           ?.SelectMany(d => d.Characteristics)
           ?.Where(e => e.Code == "SegTravProdSubGroupIDQtyPrice")
           ?.FirstOrDefault();

            string[] items = characterStics.Value.Split(',');
            List<Tuple<string, string, int, string>> tupleList = new List<Tuple<string, string, int, string>>();

            if (items != null && items.Length > 0)
            {
                string[] selectedItems = null;
                foreach (var item in items)
                {
                    //segmentID - TravelerID - ProductID - SubGroupID - Quantity - Price
                    if (item != "")
                        selectedItems = item.Split('|');
                    if (selectedItems != null && selectedItems.Length > 0)
                    {
                        //TravelerID - ProductID - SubGroupID - Quantity - Price
                        tupleList.Add(Tuple.Create(selectedItems[2], selectedItems[3], Convert.ToInt32(selectedItems[4]), selectedItems[5]));
                    }
                }
            }
            for (int i = 0; i < flightReservationResponse.Reservation.Travelers.Count; i++)
            {
                if (response.Count == 0)
                    segmentDetail.SegmentInfo = UtilityHelper.GetSegmentDescription(travelOptions);
                List<ProductSubSegmentDetail> snackDetails = new List<ProductSubSegmentDetail>();
                int travelerCouter = 0;
                int prodCounter = 0;
                foreach (var subProduct in subProducts)
                {
                    ProductSubSegmentDetail segDetail = new ProductSubSegmentDetail();
                    if (subProduct.Prices.Where(a => a.Association.TravelerRefIDs[0] == (i + 1).ToString()).Any())
                    {
                        if (subProduct != null && subProduct.Extension != null)
                        {
                            var priceInfo = subProduct.Prices.Where(a => a.Association.TravelerRefIDs[0] == (i + 1).ToString()).FirstOrDefault();
                            double price = priceInfo.PaymentOptions.FirstOrDefault().PriceComponents.FirstOrDefault().Price.Totals.FirstOrDefault().Amount;
                            var tupleSelectedItem = tupleList.Where(a => a.Item2 == subProduct.SubGroupCode && a.Item1 == priceInfo.ID).FirstOrDefault();

                            if (tupleSelectedItem != null)
                            {
                                if (_configuration.GetValue<bool>("EnableisEditablePOMFeature"))
                                {
                                    if (price > 0 && subProduct.Extension.MealCatalog?.MealShortDescription != null)
                                    {
                                        if (prodCounter == 0 && travelerCouter == 0)
                                        {
                                            segDetail.Passenger = traveler[i].Person.GivenName.ToLower() + " " + traveler[i].Person.Surname.ToLower();
                                            segDetail.Price = "0";
                                            snackDetails.Add(segDetail);
                                            segDetail = new ProductSubSegmentDetail();

                                            segDetail.SegmentDescription = subProduct.Extension.MealCatalog?.MealShortDescription + " x " + tupleSelectedItem.Item3;
                                            segDetail.DisplayPrice = "$" + String.Format("{0:0.00}", price * tupleSelectedItem.Item3);
                                            segDetail.Price = price.ToString();
                                        }
                                        else
                                        {
                                            segDetail.SegmentDescription = subProduct.Extension.MealCatalog?.MealShortDescription + " x " + tupleSelectedItem.Item3;
                                            segDetail.DisplayPrice = "$" + String.Format("{0:0.00}", price * tupleSelectedItem.Item3);
                                            segDetail.Price = price.ToString();
                                        }
                                        prodCounter++;
                                        snackDetails.Add(segDetail);
                                    }
                                }
                                else
                                {
                                    //  int quantity = GetQuantity(travelOptions, subProduct.SubGroupCode, subProduct.Prices.Where(a=>a.ID == (i+1).ToString()).Select(b=>b.ID).ToString());
                                    if (prodCounter == 0 && travelerCouter == 0)
                                    {
                                        //TODO
                                        //segDetail.Passenger = traveler[i].Person.GivenName.ToLower().ToPascalCase() + " " + traveler[i].Person.Surname.ToLower().ToPascalCase();
                                        segDetail.Passenger = traveler[i].Person.GivenName.ToLower() + " " + traveler[i].Person.Surname.ToLower();
                                        segDetail.Price = "0";
                                        snackDetails.Add(segDetail);
                                        segDetail = new ProductSubSegmentDetail();

                                        segDetail.SegmentDescription = subProduct.Extension.MealCatalog?.MealShortDescription + " x " + tupleSelectedItem.Item3;
                                        segDetail.DisplayPrice = "$" + String.Format("{0:0.00}", price * tupleSelectedItem.Item3);
                                        segDetail.Price = price.ToString();
                                    }
                                    else
                                    {
                                        segDetail.SegmentDescription = subProduct.Extension.MealCatalog?.MealShortDescription + " x " + tupleSelectedItem.Item3;
                                        segDetail.DisplayPrice = "$" + String.Format("{0:0.00}", price * tupleSelectedItem.Item3);
                                        segDetail.Price = price.ToString();
                                    }
                                    prodCounter++;
                                    snackDetails.Add(segDetail);
                                }
                            }

                        }
                    }

                }
                if (segmentDetail.SubSegmentDetails == null) segmentDetail.SubSegmentDetails = new List<ProductSubSegmentDetail>();
                if (snackDetails != null)
                    segmentDetail.SubSegmentDetails.AddRange(snackDetails);
                travelerCouter++;

            }
            if (segmentDetail != null && segmentDetail.SubSegmentDetails != null && !response.Contains(segmentDetail))
                response.Add(segmentDetail);
            return response;
        }

        public static ProdDetail BuildProdDetailsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, SeatChangeState state, bool isPost)
        {
            if (flightReservationResponse.DisplayCart.DisplaySeats == null || !flightReservationResponse.DisplayCart.DisplaySeats.Any())
            {
                return null;
            }
            //check here.
            var fliterSeats = flightReservationResponse.DisplayCart.DisplaySeats.Where(d => d.PCUSeat || (CheckSeatAssignMessage(d.SeatAssignMessage, isPost) && d.Seat != "---")).ToList();
            if (_configuration.GetValue<bool>("EnablePCUFromSeatMapErrorCheckViewRes"))
            {
                fliterSeats = HandleCSLDefect(flightReservationResponse, fliterSeats, isPost);
            }
            if (!fliterSeats.Any())
            {
                return null;
            }

            var totalPrice = fliterSeats.Select(s => s.SeatPrice).ToList().Sum();
            var prod = new ProdDetail
            {
                Code = "SEATASSIGNMENTS",
                ProdDescription = string.Empty,
                ProdTotalPrice = String.Format("{0:0.00}", totalPrice),
                ProdDisplayTotalPrice = totalPrice > 0 ? Decimal.Parse(totalPrice.ToString()).ToString("c") : string.Empty,
                Segments = BuildProductSegmentsForSeats(flightReservationResponse, state.Seats, state.BookingTravelerInfo, isPost)
            };
            if (prod.Segments != null && prod.Segments.Any())
            {
                if (IsMilesFOPEnabled())
                {
                    if (prod.Segments.SelectMany(s => s.SubSegmentDetails).ToList().Select(ss => ss.Miles == 0).ToList().Count == 0 && IsMilesFOPEnabled())
                    {
                        prod.ProdTotalMiles = _configuration.GetValue<int>("milesFOP");
                        prod.ProdDisplayTotalMiles = UtilityHelper.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false);
                    }
                    else
                    {
                        prod.ProdTotalMiles = 0;
                        prod.ProdDisplayTotalMiles = string.Empty;
                    }
                }
                prod.Segments.Select(x => x.SubSegmentDetails).ToList().ForEach(item => item.RemoveAll(k => Decimal.Parse(k.Price) == 0 && (k.StrikeOffPrice == string.Empty || Decimal.Parse(k.StrikeOffPrice) == 0)));
                prod.Segments.RemoveAll(k => k.SubSegmentDetails.Count == 0);
            }
            UtilityHelper.UpdateRefundTotal(prod);
            return prod;
        }

        public static List<ProductSegmentDetail> BuildProductSegmentsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, List<Seat> seats, List<MOBBKTraveler> BookingTravelerInfo, bool isPost)
        {
            if (flightReservationResponse.DisplayCart.DisplaySeats == null || !flightReservationResponse.DisplayCart.DisplaySeats.Any())
                return null;

            var displaySeats = flightReservationResponse.DisplayCart.DisplaySeats.Clone();
            List<string> refundedSegmentNums = null;
            if (flightReservationResponse.Errors != null && flightReservationResponse.Errors.Any(e => e != null && e.MinorCode == "90506"))
            {
                var isRefundSuccess = UtilityHelper.IsRefundSuccess(flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items, out refundedSegmentNums);
                //Remove pcu seats if refund Failed
                if (!isRefundSuccess)
                {
                    displaySeats.RemoveAll(ds => ds.PCUSeat);
                }
                if (!displaySeats.Any())
                    return null;
            }

            //Remove all failed seats other than pcu seats.
            displaySeats.RemoveAll(ds => !ds.PCUSeat && !CheckSeatAssignMessage(ds.SeatAssignMessage, isPost)); // string.IsNullOrEmpty(ds.SeatAssignMessage)
            if (_configuration.GetValue<bool>("EnablePCUFromSeatMapErrorCheckViewRes"))
            {
                displaySeats = HandleCSLDefect(flightReservationResponse, displaySeats, isPost);
            }
            if (!displaySeats.Any())
                return null;

            return displaySeats.OrderBy(d => d.OriginalSegmentIndex)
                                .GroupBy(d => new { d.OriginalSegmentIndex, d.LegIndex })
                                .Select(d => new ProductSegmentDetail
                                {
                                    SegmentInfo = UtilityHelper.GetSegmentInfo(flightReservationResponse, d.Key.OriginalSegmentIndex, Convert.ToInt32(d.Key.LegIndex)),
                                    SubSegmentDetails = d.GroupBy(s => UtilityHelper.GetSeatTypeForDisplay(s, flightReservationResponse.DisplayCart.TravelOptions))
                                                        .Select(seatGroup => new ProductSubSegmentDetail
                                                        {
                                                            Price = String.Format("{0:0.00}", seatGroup.Select(s => s.SeatPrice).ToList().Sum()),
                                                            DisplayPrice = Decimal.Parse(seatGroup.Select(s => s.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                                                            StrikeOffPrice = UtilityHelper.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats, BookingTravelerInfo),
                                                            DisplayStrikeOffPrice = UtilityHelper.GetFormatedDisplayPriceForSeats(UtilityHelper.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats, BookingTravelerInfo)),
                                                            Passenger = seatGroup.Count().ToString() + (seatGroup.Count() > 1 ? " Travelers" : " Traveler"),
                                                            SeatCode = seatGroup.Key,
                                                            FlightNumber = seatGroup.Select(x => x.FlightNumber).FirstOrDefault(),
                                                            SegmentDescription = GetSeatTypeBasedonCode(seatGroup.Key, seatGroup.Count()),
                                                            IsPurchaseFailure = UtilityHelper.IsPurchaseFailed(seatGroup.Any(s => s.PCUSeat), d.Key.OriginalSegmentIndex.ToString(), refundedSegmentNums),
                                                            Miles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? 0 : _configuration.GetValue<int>("milesFOP") : 0,
                                                            DisplayMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? string.Empty : UtilityHelper.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false) : string.Empty,
                                                            StrikeOffMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? 0 : Convert.ToInt32(_configuration.GetValue<string>("milesFOP")) : 0,
                                                            DisplayStrikeOffMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? string.Empty : UtilityHelper.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false) : string.Empty
                                                        }).ToList().OrderBy(p => UtilityHelper.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
                                }).ToList();
        }

        public static string GetSeatTypeBasedonCode(string seatCode, int travelerCount)
        {
            string seatType = string.Empty;

            switch (seatCode.ToUpper().Trim())
            {
                case "SXZ": //StandardPreferredExitPlus
                case "SZX": //StandardPreferredExit
                case "SBZ": //StandardPreferredBlukheadPlus
                case "SZB": //StandardPreferredBlukhead
                case "SPZ": //StandardPreferredZone
                case "PZA":
                    seatType = (travelerCount > 1) ? "Preferred seats" : "Preferred seat";
                    break;
                case "SXP": //StandardPrimeExitPlus
                case "SPX": //StandardPrimeExit
                case "SBP": //StandardPrimeBlukheadPlus
                case "SPB": //StandardPrimeBlukhead
                case "SPP": //StandardPrimePlus
                case "PPE": //StandardPrime
                case "BSA":
                case "ASA":
                    seatType = (travelerCount > 1) ? "Advance seat assignments" : "Advance seat assignment";
                    break;
                case "EPL": //EplusPrime
                case "EPU": //EplusPrimePlus
                case "BHS": //BulkheadPrime
                case "BHP": //BulkheadPrimePlus  
                case "PSF": //PrimePlus  
                    seatType = (travelerCount > 1) ? "Economy Plus Seats" : "Economy Plus Seat";
                    break;
                case "PSL": //Prime                            
                    seatType = (travelerCount > 1) ? "Economy Plus Seats (limited recline)" : "Economy Plus Seat (limited recline)";
                    break;
                default:
                    var pcuCabinName = GetFormattedCabinName(seatCode);
                    if (!string.IsNullOrEmpty(pcuCabinName))
                    {
                        return pcuCabinName + ((travelerCount > 1) ? " Seats" : " Seat");
                    }
                    return string.Empty;
            }
            return seatType;
        }

        public static List<SeatAssignment> HandleCSLDefect(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, List<SeatAssignment> fliterSeats, bool isPost)
        {
            if (fliterSeats == null || !fliterSeats.Any())
                return fliterSeats;

            fliterSeats = fliterSeats.Where(s => s != null && s.OriginalSegmentIndex != 0 && !string.IsNullOrEmpty(s.DepartureAirportCode) && !string.IsNullOrEmpty(s.ArrivalAirportCode)).ToList();

            if (fliterSeats == null || !fliterSeats.Any())
                return fliterSeats;

            if (flightReservationResponse.Errors != null &&
                flightReservationResponse.Errors.Any(e => e != null && e.MinorCode == "90584") &&
                flightReservationResponse.DisplayCart.DisplaySeats != null &&
                flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s != null && s.PCUSeat) &&
                flightReservationResponse.DisplayCart.DisplaySeats.Any(s => s != null && !s.PCUSeat &&
                 CheckSeatAssignMessage(s.SeatAssignMessage, isPost)))
            {
                //take this from errors
                var item = flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Where(t => t.Item.Category == "Reservation.Reservation.SEATASSIGNMENTS").FirstOrDefault();
                if (item != null && item.Item != null && item.Item.Product != null && item.Item.Product.Any())
                {
                    var description = JsonConvert.DeserializeObject<Service.Presentation.FlightResponseModel.AssignTravelerSeat>(item.Item.Product.FirstOrDefault().Status.Description);
                    var unAssignedSeats = description.Travelers.SelectMany(t => t.Seats.Where(s => !string.IsNullOrEmpty(s.AssignMessage))).ToList();
                    if (unAssignedSeats != null && unAssignedSeats.Any())
                    {
                        return fliterSeats.Where(s => !UtilityHelper.IsFailedSeat(s, unAssignedSeats)).ToList();
                    }
                }
            }
            return fliterSeats;
        }

        public static bool CheckSeatAssignMessage(string seatAssignMessage, bool isPost)
        {
            if (isPost)
            {
                return !string.IsNullOrEmpty(seatAssignMessage) && seatAssignMessage.Equals("SEATS ASSIGNED", StringComparison.OrdinalIgnoreCase);
            }
            else
            {
                return string.IsNullOrEmpty(seatAssignMessage);
            }
        }

        public static ProdDetail BuildProdDetailsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, SeatChangeState state, string flow, MOBApplication application)
        {
            if (UtilityHelper.IsCheckinFlow(flow) && flightReservationResponse.DisplayCart.TravelOptions != null && flightReservationResponse.DisplayCart.TravelOptions.Any(x => x.Type == "SEATASSIGNMENTS"))
            {
                ProdDetail prod = new ProdDetail();
                prod.Code = "SEATASSIGNMENTS";
                prod.ProdDescription = string.Empty;
                decimal totalPrice = flightReservationResponse.DisplayCart.TravelOptions.Sum(x => x.Type == "SEATASSIGNMENTS" ? x.Amount : 0);
                prod.ProdTotalPrice = String.Format("{0:0.00}", totalPrice);
                prod.ProdDisplayTotalPrice = $"${prod.ProdTotalPrice}";
                if (totalPrice > 0)
                {
                    var displaySeats = flightReservationResponse.DisplayCart.DisplaySeats.Where(x => x.OriginalPrice > 0).Select(x => { x.SeatType = UtilityHelper.GetCommonSeatCode(x.SeatType); return x; }).GroupBy(x => $"{x.DepartureAirportCode} - {x.ArrivalAirportCode}");
                    prod.Segments = BuildCheckinSegmentDetail(displaySeats);
                }
                return prod;
            }

            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes")) && !UtilityHelper.IsCheckinFlow(flow))
            {
                var prod = new ProdDetail
                {
                    Code = "SEATASSIGNMENTS",
                    ProdDescription = string.Empty,
                    ProdTotalPrice = String.Format("{0:0.00}", flightReservationResponse.DisplayCart.DisplaySeats.Select(d => d.SeatPrice).ToList().Sum()),
                    ProdDisplayTotalPrice = Decimal.Parse(flightReservationResponse.DisplayCart.DisplaySeats.Select(d => d.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                    Segments = BuildProductSegmentsForSeats(flightReservationResponse, state?.Seats, application)
                };
                prod.Segments.Select(x => x.SubSegmentDetails).ToList().ForEach(item => item.RemoveAll(k => Decimal.Parse(k.Price) == 0 && (k.StrikeOffPrice == string.Empty || Decimal.Parse(k.StrikeOffPrice) == 0)));
                prod.Segments.RemoveAll(k => k.SubSegmentDetails.Count == 0);
                return prod;
            }

            var prodDetail = new ProdDetail()
            {
                Code = "SEATASSIGNMENTS",
                ProdDescription = string.Empty,
                ProdTotalPrice = String.Format("{0:0.00}", flightReservationResponse.DisplayCart.DisplaySeats.Select(d => d.SeatPrice).ToList().Sum()),
                ProdDisplayTotalPrice = Decimal.Parse(flightReservationResponse.DisplayCart.DisplaySeats.Select(d => d.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                //Mobile-1524: Include all the seats even if seat price is null when user has E+/Preferred subscriptions
                //                                                           |----Ignore the DisplaySeats ---------|------Updating the SeatPromotionCode to a common code for easy grouping ---------------|                                                                                                                            |--Ordering the DisplaySeats based---|--Group the resulted DisplaySeats objects based on OriginalSegmentIndex and LegIndex value and return the List of DisplaySeats -|   
                //       LEVEL 1                                             |----if SeatPromotionCode = null------|------and return the object using GetCommonCode method---------------------------------|----------Ignoring DisplaySeats child object if either SeatPrice = 0  ------------------------------------------------      |--on OriginalSegmentIndex-----------|--This is been done for COG, THRU flights. These flights even though are one segment at high level but have multiple segments---|
                Segments = flightReservationResponse.DisplayCart.DisplaySeats.Where(x => x.SeatPromotionCode != null).Select(x => { x.SeatPromotionCode = UtilityHelper.GetCommonSeatCode(x.SeatPromotionCode); return x; }).Where(d => (state != null ? (state.TotalEplusEligible > 0 ? true : d.SeatPrice != 0) : d.SeatPrice != 0)).OrderBy(d => d.OriginalSegmentIndex).GroupBy(d => new { d.OriginalSegmentIndex, d.LegIndex }).Select(d => new ProductSegmentDetail
                //Segments = flightReservationResponse.DisplayCart.DisplaySeats.Where(x => x.SeatPromotionCode != null).Select(x => { x.SeatPromotionCode = Utility.GetCommonSeatCode(x.SeatPromotionCode); return x; }).Where(d => (true? d.SeatPrice != 0 : d.SeatPrice >= 0)).OrderBy(d =>d.Orih]]]]]]]]]]]ginalSegmentIndex).GroupBy(d => new { d.OriginalSegmentIndex, d.LegIndex }).Select(d => new MOBProductSegmentDetail
                {
                    //                |--------------Get the individual Segment Origin and Destination detail based on OriginalSegmentIndex and LegIndex for the list of DisplaySeats from LEVEL 1---| 
                    SegmentInfo = UtilityHelper.GetSegmentInfo(flightReservationResponse, d.Select(u => u.OriginalSegmentIndex).FirstOrDefault(), Convert.ToInt32(d.Select(u => u.LegIndex).FirstOrDefault())),
                    ProductId = null,
                    //     LEVEL 2         |---- Further group the LEVEL 1 list of DisplaySeats based on OriginalSegmentIndex and SeatPromotionCode to get SubSegmentDetails--|
                    SubSegmentDetails = d.GroupBy(t => new { t.OriginalSegmentIndex, t.SeatPromotionCode }).Select(t => new ProductSubSegmentDetail
                    {
                        //              |--Getting the sum of SeatPrice from the list of DisplaySeats at LEVEL 2 --|
                        Price = String.Format("{0:0.00}", t.Select(s => s.SeatPrice).ToList().Sum()),
                        DisplayPrice = Decimal.Parse(t.Select(s => s.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                        //                  | --Getting the count of list of DisplaySeats at LEVEL 2-- |
                        Passenger = t.Count().ToString() + (t.Count() > 1 ? " Travelers" : " Traveler"), // t.GroupBy(u => u.TravelerIndex).Count().ToString() + (t.GroupBy(u => u.TravelerIndex).Count() > 1 ? " Travelers" : " Traveler"),
                        SeatCode = t.Select(u => u.SeatPromotionCode).FirstOrDefault(),
                        FlightNumber = t.Select(x => x.FlightNumber).FirstOrDefault(),
                        // DepartureTime = flightReservationResponse.Reservation.FlightSegments.Where(s => s.FlightSegment == ),
                        //                          | --Getting the SeatDescription based on SeatPromotionCode from the list of DisplaySeats at LEVEL 2, TravelerIndex count for pluralizing the text. -- |
                        SegmentDescription = GetSeatTypeBasedonCode(t.Select(u => u.SeatPromotionCode).FirstOrDefault(), t.GroupBy(u => u.TravelerIndex).Count())
                        //             | -- Once Get the final list ordering them based on the order defined in GetSEatPriceOrder method comparing with list SegmentDescription -- |
                    }).ToList().OrderBy(p => UtilityHelper.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
                }).ToList(),
            };

            if (state == null ? false : (state.TotalEplusEligible > 0))
            {
                foreach (var segmnt in prodDetail.Segments)
                {
                    var subSegments = segmnt.SubSegmentDetails;
                    foreach (var subSegment in subSegments)
                    {
                        if (state.Seats != null)
                        {
                            var sbSegments = state.Seats.Where(x => x.Origin == segmnt.SegmentInfo.Substring(0, 3) && x.Destination == segmnt.SegmentInfo.Substring(6, 3) && x.FlightNumber == subSegment.FlightNumber && (UtilityHelper.GetCommonSeatCode(x.ProgramCode) == subSegment.SeatCode)).ToList();
                            decimal totalPrice = sbSegments.Select(u => u.Price).ToList().Sum();
                            decimal discountPrice = sbSegments.Select(u => u.PriceAfterTravelerCompanionRules).ToList().Sum();
                            if (discountPrice < totalPrice)
                            {
                                subSegment.StrikeOffPrice = String.Format("{0:0.00}", sbSegments.Select(u => u.Price).ToList().Sum().ToString());
                                subSegment.DisplayStrikeOffPrice = Decimal.Parse(sbSegments.Select(u => u.Price).ToList().Sum().ToString()).ToString("c");
                            }
                            subSegment.Passenger = sbSegments.Count() + (sbSegments.Count() > 1 ? " Travelers" : " Traveler");
                            subSegment.Price = String.Format("{0:0.00}", sbSegments.Select(u => u.PriceAfterTravelerCompanionRules).ToList().Sum().ToString());
                            subSegment.DisplayPrice = Decimal.Parse(sbSegments.Select(u => u.PriceAfterTravelerCompanionRules).ToList().Sum().ToString()).ToString("c");
                        }
                    }
                }
            }
            //Mobile-1855: Remove segments with no seats to purchase
            prodDetail.Segments.Select(x => x.SubSegmentDetails).ToList().ForEach(item => item.RemoveAll(k => k.Price == "0" && (k.StrikeOffPrice == "0" || k.StrikeOffPrice == string.Empty)));
            prodDetail.Segments.RemoveAll(k => k.SubSegmentDetails.Count == 0);
            return prodDetail;
        }

        public static List<ProductSegmentDetail> BuildProductSegmentsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, List<Seat> seats, MOBApplication application)
        {
            return flightReservationResponse.DisplayCart.DisplaySeats.OrderBy(d => d.OriginalSegmentIndex)
                                                        .GroupBy(d => new { d.OriginalSegmentIndex, d.LegIndex })
                                                        .Select(d => new ProductSegmentDetail
                                                        {
                                                            SegmentInfo = UtilityHelper.GetSegmentInfo(flightReservationResponse, d.Key.OriginalSegmentIndex, Convert.ToInt32(d.Key.LegIndex)),
                                                            SubSegmentDetails = d.GroupBy(s => UtilityHelper.GetSeatTypeForDisplay(s, flightReservationResponse.DisplayCart.TravelOptions))
                                                                                .Select(seatGroup => new ProductSubSegmentDetail
                                                                                {
                                                                                    SegmentInfo = IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major,true) ? UtilityHelper.GetSegmentInfo(flightReservationResponse, d.Key.OriginalSegmentIndex, Convert.ToInt32(d.Key.LegIndex)) : string.Empty,
                                                                                    OrginalPrice = IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major,true) ? String.Format("{0:0.00}", seatGroup.Select(s => s.OriginalPrice).ToList().Sum(),true) : string.Empty,
                                                                                    Price = String.Format("{0:0.00}", seatGroup.Select(s => s.SeatPrice).ToList().Sum()),
                                                                                    DisplayPrice = Decimal.Parse(seatGroup.Select(s => s.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                                                                                    DisplayOriginalPrice = IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major,true) ? Decimal.Parse(seatGroup.Select(s => s.OriginalPrice).ToList().Sum().ToString()).ToString("c") : string.Empty,
                                                                                    StrikeOffPrice = UtilityHelper.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats),
                                                                                    DisplayStrikeOffPrice = UtilityHelper.GetFormatedDisplayPriceForSeats(UtilityHelper.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats)),
                                                                                    Passenger = seatGroup.Count().ToString() + (seatGroup.Count() > 1 ? " Travelers" : " Traveler"),
                                                                                    SeatCode = seatGroup.Key,
                                                                                    FlightNumber = seatGroup.Select(x => x.FlightNumber).FirstOrDefault(),
                                                                                    SegmentDescription = GetSeatTypeBasedonCode(seatGroup.Key, seatGroup.Count()),
                                                                                    PaxDetails = IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major, true) ? GetPaxDetails(seatGroup, flightReservationResponse) : null,
                                                                                    ProductDescription = IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major,true) ? UtilityHelper.GetSeatDescription(seatGroup.Key) : string.Empty
                                                                                }).ToList().OrderBy(p => UtilityHelper.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
                                                        }).ToList();
        }
        public static List<MOBPaxDetails> GetPaxDetails(IGrouping<string, SeatAssignment> t, FlightReservationResponse response)
        {
            List<MOBPaxDetails> paxDetails = new List<MOBPaxDetails>();
            if (response?.Reservation?.Travelers != null)
            {
                t.ForEach(seat =>
                {
                    var traveler = response.Reservation.Travelers.Where(passenger => passenger.Person != null && passenger.Person.Key == seat.PersonIndex).FirstOrDefault();
                    if (traveler != null && (seat.SeatPrice > 0 || seat.OriginalPrice > 0)) // Added OriginalPrice check as well to handle coupon applied sceanrios where seat price can be 0 but we have original price
                    {
                        paxDetails.Add(new MOBPaxDetails
                        {
                            FullName = traveler.Person.GivenName + " " + traveler.Person.Surname,
                            Key = seat.PersonIndex,
                            Seat = seat.Seat

                        });
                    }

                });
            }
            return paxDetails;
        }

        public static List<ProductSegmentDetail> BuildCheckinSegmentDetail(IEnumerable<IGrouping<string, SeatAssignment>> seatAssignmentGroup)
        {
            List<ProductSegmentDetail> segmentDetails = new List<ProductSegmentDetail>();
            seatAssignmentGroup.ForEach(seatSegment => segmentDetails.Add(new ProductSegmentDetail()
            {
                SegmentInfo = seatSegment.Key,
                SubSegmentDetails = BuildSubsegmentDetails(seatSegment.ToList()).OrderBy(p => UtilityHelper.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
            }));
            return segmentDetails;
        }

        public static List<ProductSubSegmentDetail> BuildSubsegmentDetails(List<SeatAssignment> seatAssignments)
        {
            List<ProductSubSegmentDetail> subSegmentDetails = new List<ProductSubSegmentDetail>();
            var groupedByTypeAndPrice = seatAssignments.GroupBy(s => s.SeatType, (key, grpSeats) => new { SeatType = key, OriginalPrice = grpSeats.Sum(x => x.OriginalPrice), SeatPrice = grpSeats.Sum(x => x.SeatPrice), Count = grpSeats.Count() });

            groupedByTypeAndPrice.ForEach(grpSeats =>
            {
                subSegmentDetails.Add(PopulateSubsegmentDetails(grpSeats.SeatType, grpSeats.OriginalPrice, grpSeats.SeatPrice, grpSeats.Count));
            });
            return subSegmentDetails;
        }

        public static ProductSubSegmentDetail PopulateSubsegmentDetails(string seatType, decimal originalPrice, decimal seatPrice, int count)
        {
            ProductSubSegmentDetail subsegmentDetail = new ProductSubSegmentDetail();
            subsegmentDetail.Price = String.Format("{0:0.00}", seatPrice);
            subsegmentDetail.DisplayPrice = $"${subsegmentDetail.Price}";
            if (originalPrice > seatPrice)
            {
                subsegmentDetail.StrikeOffPrice = String.Format("{0:0.00}", originalPrice);
                subsegmentDetail.DisplayStrikeOffPrice = $"${subsegmentDetail.StrikeOffPrice}";
            }
            subsegmentDetail.Passenger = $"{count} Traveler{(count > 1 ? "s" : String.Empty)}";
            subsegmentDetail.SegmentDescription = GetSeatTypeBasedonCode(seatType, count, true);
            return subsegmentDetail;
        }
        public static string GetSeatTypeBasedonCode(string seatCode, int travelerCount, bool isCheckinPath = false)
        {
            string seatType = string.Empty;

            switch (seatCode.ToUpper().Trim())
            {
                case "SXZ": //StandardPreferredExitPlus
                case "SZX": //StandardPreferredExit
                case "SBZ": //StandardPreferredBlukheadPlus
                case "SZB": //StandardPreferredBlukhead
                case "SPZ": //StandardPreferredZone
                case "PZA":
                    seatType = (travelerCount > 1) ? "Preferred seats" : "Preferred seat";
                    break;
                case "SXP": //StandardPrimeExitPlus
                case "SPX": //StandardPrimeExit
                case "SBP": //StandardPrimeBlukheadPlus
                case "SPB": //StandardPrimeBlukhead
                case "SPP": //StandardPrimePlus
                case "PPE": //StandardPrime
                case "BSA":
                case "ASA":
                    if (isCheckinPath)
                        seatType = (travelerCount > 1) ? "Seat assignments" : "Seat assignment";
                    else
                        seatType = (travelerCount > 1) ? "Advance seat assignments" : "Advance seat assignment";
                    break;
                case "EPL": //EplusPrime
                case "EPU": //EplusPrimePlus
                case "BHS": //BulkheadPrime
                case "BHP": //BulkheadPrimePlus  
                case "PSF": //PrimePlus  
                    seatType = (travelerCount > 1) ? "Economy Plus Seats" : "Economy Plus Seat";
                    break;
                case "PSL": //Prime                            
                    seatType = (travelerCount > 1) ? "Economy Plus Seats (limited recline)" : "Economy Plus Seat (limited recline)";
                    break;
                default:
                    var pcuCabinName = GetFormattedCabinName(seatCode);
                    if (!string.IsNullOrEmpty(pcuCabinName))
                    {
                        return pcuCabinName + ((travelerCount > 1) ? " Seats" : " Seat");
                    }
                    return string.Empty;
            }
            return seatType;
        }

        public static double GetGrandTotalPriceForShoppingCart(bool isCompleteFarelockPurchase, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isPost, string flow = "VIEWRES")
        {
            //Added Null check for price.Since,CSL is not sending the price when we select seat of price zero.
            //Added condition to check whether Total in the price exists(This is for scenario when we register the bundle after registering the seat).
            //return isCompleteFarelockPurchase ? Convert.ToDouble(flightReservationResponse.DisplayCart.DisplayPrices.FirstOrDefault(o => (o.Description != null && o.Description.Equals("GrandTotal", StringComparison.OrdinalIgnoreCase))).Amount)
            //                                  : (Utility.IsCheckinFlow(flow) || flow == FlowType.VIEWRES.ToString()) ? flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum() : flightReservationResponse.ShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Price != null).SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() : true)).Select(x => x.Amount).ToList().Sum();
            double shoppingCartTotalPrice = 0.0;
            double closeBookingFee = 0.0;
            if (isCompleteFarelockPurchase)
                shoppingCartTotalPrice = Convert.ToDouble(flightReservationResponse.DisplayCart.DisplayPrices.FirstOrDefault(o => (o.Description != null && o.Description.Equals("GrandTotal", StringComparison.OrdinalIgnoreCase))).Amount);
            else
            {
                if (isPost ? flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Select(x => x.Item).Any(x => x.Product.FirstOrDefault().Code == "FLK")
                        : flightReservationResponse.ShoppingCart.Items.Any(x => x.Product.FirstOrDefault().Code == "FLK"))
                    flow = FlowType.FARELOCK.ToString();
                //[MB-6519]:Getting Sorry Something went wrong for Award Booking for with Reward booking fee and reservation price is zero
                if (_configuration.GetValue<bool>("CFOP19HBugFixToggle") && (isPost ? flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items.Select(x => x.Item).Any(x => x.Product.FirstOrDefault().Code == "RBF")
                                                                            : flightReservationResponse.ShoppingCart.Items.Any(x => x.Product.FirstOrDefault().Code == "RBF")))

                {
                    United.Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart = new United.Service.Presentation.InteractionModel.ShoppingCart();
                    flightReservationResponseShoppingCart = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart : flightReservationResponse.ShoppingCart;
                    closeBookingFee = UtilityHelper.GetCloseBookingFee(isPost, flightReservationResponseShoppingCart, flow);

                }

                switch (flow)
                {
                    case "BOOKING":
                    case "RESHOP":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !UtilityHelper.CheckFailedShoppingCartItem(flightReservationResponse, x)).SelectMany(x => x.Product).Where(x => x.Price != null).SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() /*|| x.Name.ToUpper() == "Close-In Booking Fee".ToUpper()*/ : true)).Select(x => x.Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Price != null).SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() /*|| x.Name.ToUpper() == "Close-In Booking Fee".ToUpper()*/ : true)).Select(x => x.Amount).ToList().Sum();
                        break;

                    case "POSTBOOKING":
                        //productCodes = flightReservationResponseShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Characteristics != null && (x.Characteristics.Any(y => y.Description.ToUpper() == "POSTPURCHASE" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Code).ToList();
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !UtilityHelper.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false) && x.Product.FirstOrDefault().Characteristics != null && (x.Product.FirstOrDefault().Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.Where(x => (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false) && x.Product.FirstOrDefault().Characteristics != null && (x.Product.FirstOrDefault().Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum(); ;
                        break;

                    case "FARELOCK":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !UtilityHelper.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => x.Product.FirstOrDefault().Code == "FLK" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code == "FLK" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum();
                        break;

                    case "VIEWRES":
                    case "CHECKIN":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !UtilityHelper.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => x.Product.FirstOrDefault().Code != "RES" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code != "RES" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum();
                        break;
                }
            }
            if (_configuration.GetValue<bool>("CFOP19HBugFixToggle"))
            {
                shoppingCartTotalPrice = shoppingCartTotalPrice + closeBookingFee;
            }
            return shoppingCartTotalPrice;
        }

        #region SeatMap

        public static bool EnableUMNRInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidUMNRInformationVersion", "iPhoneUMNRInformationVersion", "", "", true, _configuration);
        }

        public static bool EnableNewChangeSeatCheckinWindowMsg(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidNewChangeSeatCheckinWindowMsg", "iPhoneNewChangeSeatCheckinWindowMsg", "", "", true, _configuration);
        }

        public static bool EnableLufthansaForHigherVersion(string operatingCarrierCode, int applicationId, string appVersion)
        {
            return EnableLufthansa(operatingCarrierCode) &&
                                    GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "Android_EnableInterlineLHRedirectLinkManageRes_AppVersion", "iPhone_EnableInterlineLHRedirectLinkManageRes_AppVersion", "", "", true, _configuration);

        }
        public static bool EnableLufthansa(string operatingCarrierCode)
        {

            return _configuration.GetValue<bool>("EnableInterlineLHRedirectLinkManageRes")
                                    && _configuration.GetValue<string>("InterlineLHAndParternerCode").Contains(operatingCarrierCode?.ToUpper());
        }

        public static string BuildInterlineRedirectLink(MOBRequest mobRequest, string recordLocator, string lastname, string pointOfSale, string operatingCarrierCode, bool isEnableRedesignForDeepLink)
        {
            string interlineLhRedirectUrl = string.Empty;

            //this condition for LH only 
            if (_configuration.GetValue<string>("InterlineLHAndParternerCode").Contains(operatingCarrierCode))
            {
                if (GeneralHelper.IsApplicationVersionGreater(mobRequest.Application.Id, mobRequest.Application.Version.Major, "Android_EnableInterlineLHRedirectLinkManageRes_AppVersion", "iPhone_EnableInterlineLHRedirectLinkManageRes_AppVersion", "", "", true, _configuration))
                {
                    //validate the LH and CL 
                    string lufthansaLink = CreateLufthansaDeeplink(recordLocator, lastname, pointOfSale, mobRequest.LanguageCode);

                    interlineLhRedirectUrl = lufthansaLink;
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                }
            }
            return interlineLhRedirectUrl;
        }
        public static string CreateLufthansaDeeplink(string recordLocator, string lastName, string countryCode, string languageCode)
        {
            string langCode = _configuration.GetValue<string>("LanguageCodeForLHDeepLinkRedirect");
            var stringToEncrypt = string.Format("Mode=S&Filekey={0}&Lastname={1}&Page=BKGD", recordLocator, lastName); ;
            var _cryptoKey = "528B47E01C257B43DB88ABCE62CC7F5A";
            var aesEncryption = new System.Security.Cryptography.RijndaelManaged
            {
                KeySize = 128,
                BlockSize = 128,
                Mode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                Key = UtilityHelper.StringToByteArray(_cryptoKey)
            };

            System.Security.Cryptography.ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(stringToEncrypt);

            // The result of the encryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, stringToEncrypt.Length);
            var encrypted = UtilityHelper.ByteArrayToString(cipherText);
            return string.Format(_configuration.GetValue<string>("InterlineLHRedirectLink"), countryCode, !string.IsNullOrEmpty(langCode) ? langCode : languageCode, encrypted);
        }


        public static bool EnablePcuDeepLinkInSeatMap(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnablePcuDeepLinkInSeatMap")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPcuDeepLinkInSeatMapVersion", "iPhonePcuDeepLinkInSeatMapVersion", "", "", true, _configuration);
            }
            return false;
        }

        public static bool IsTokenMiddleOfFlowDPDeployment()
        {
            return (_configuration.GetValue<bool>("ShuffleVIPSBasedOnCSS_r_DPTOken") && _configuration.GetValue<bool>("EnableDpToken")) ? true : false;

        }
        public static string ModifyVIPMiddleOfFlowDPDeployment(string token, string url)
        {
            url = token.Length < 50 ? url.Replace(_configuration.GetValue<string>("DPVIPforDeployment"), _configuration.GetValue<string>("CSSVIPforDeployment")) : url;
            return url;
        }

        private static string GetIdForKey(string key, int appId)
        {
            if (string.IsNullOrWhiteSpace(key) || appId == -1)
            {
                return string.Empty;
            }

            var catalogIds = _configuration.GetValue<string>(key);
            if (!string.IsNullOrEmpty(catalogIds))
            {
                var items = catalogIds.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var keyValue = item.Split('~');
                        if (keyValue.FirstOrDefault() == appId.ToString() && keyValue.Length == 2)
                        {
                            return keyValue[1];
                        }
                    }
                }
            }

            return string.Empty;
        }

        #endregion

        public static bool CheckEPlusSeatCode(string program)
        {
            if (_configuration.GetValue<string>("EPlusSeatProgramCodes") != null)
            {
                string[] codes = _configuration.GetValue<string>("EPlusSeatProgramCodes").Split('|');
                foreach (string code in codes)
                {
                    if (code.Equals(program))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static string SpecialcharacterFilterInPNRLastname(string stringTofilter)
        {
            try
            {
                if (!string.IsNullOrEmpty(stringTofilter))
                {
                    Regex regex = new Regex(_configuration.GetValue<string>("SpecialcharactersFilterInPNRLastname"));
                    return regex.Replace(stringTofilter, string.Empty);
                }
                else
                    return stringTofilter;
            }
            catch (Exception ex) { return stringTofilter; }
        }

        public static bool EnableActiveFutureFlightCreditPNR(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneActiveFutureFlightCreditPNRVersion", "AndroidActiveFutureFlightCreditPNRVersion", "", "", true, _configuration);
        }

        public static string GetCurrencyCode(string code)
        {
            if (string.IsNullOrEmpty(code))
                return "";

            string currencyCodeMappings = _configuration.GetValue<string>("CurrencyCodeMappings");

            var ccMap = currencyCodeMappings.Split('|');
            string currencyCode = string.Empty;

            foreach (var item in ccMap)
            {
                if (item.Split('=')[0].Trim() == code.Trim())
                {
                    currencyCode = item.Split('=')[1].Trim();
                    break;
                }
            }
            if (string.IsNullOrEmpty(currencyCode))
                return code;
            else
                return currencyCode;
        }
        public static bool EnableFareLockPurchaseViewRes(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableFareLockPurchaseViewRes")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidFareLockPurchaseViewResVersion", "iPhoneFareLockPurchaseViewResVersion", "", "", true, _configuration);
            }
            return false;
        }
        public static void GetCheckInEligibilityStatusFromCSLPnrReservation(System.Collections.ObjectModel.Collection<United.Service.Presentation.CommonEnumModel.CheckinStatus> checkinEligibilityList, ref MOBPNR pnr)
        {
            bool SegmentFlownCheckToggle = Convert.ToBoolean(_configuration.GetValue<string>("SegmentFlownCheckToggle") ?? "false");

            pnr.CheckInStatus = "0";
            pnr.IrrOps = false;
            pnr.IrrOpsViewed = false;

            if (checkinEligibilityList != null && (pnr.Segments != null && pnr.Segments.Count > 0))
            {
                int hours = Convert.ToInt32(_configuration.GetValue<string>("PNRStatusLeadHours")) * -1;
                bool isNotFlownSegmentExist = IsNotFlownSegmentExist(pnr.Segments, hours, SegmentFlownCheckToggle);

                if ((!SegmentFlownCheckToggle && Convert.ToDateTime((pnr.Segments[0].ScheduledDepartureDateTimeGMT)).AddHours(hours) < DateTime.UtcNow) ||
                    (SegmentFlownCheckToggle && isNotFlownSegmentExist))
                {
                    foreach (var checkinEligibility in checkinEligibilityList)
                    {
                        if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.AlreadyCheckedin)
                        {
                            pnr.CheckInStatus = "2"; //"AlreadyCheckedin";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.CheckinEligible)
                        {
                            pnr.CheckInStatus = "1"; //"CheckInEligible";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.IRROPS)
                        {
                            pnr.IrrOps = true; //"IRROPS";
                        }
                        else if (checkinEligibility == Service.Presentation.CommonEnumModel.CheckinStatus.IRROPS_VIEWED)
                        {
                            pnr.IrrOpsViewed = true; //"IRROPS_VIEWED";
                        }
                    }
                }
            }
        }
        private static bool IsNotFlownSegmentExist(List<MOBPNRSegment> segments, int hours, bool SegmentFlownCheckToggle)
        {
            bool isNotFlownSegmentExist = false;
            try
            {
                if (SegmentFlownCheckToggle)
                {
                    string segmentTicketCouponStatusCodes = (_configuration.GetValue<string>("SegmentTicketCouponStatusCodes") ?? "");
                    isNotFlownSegmentExist = segments.Exists(segment => (string.IsNullOrEmpty(segment.TicketCouponStatus) ||
                                                                            (segmentTicketCouponStatusCodes != string.Empty &&
                                                                             !string.IsNullOrEmpty(segment.TicketCouponStatus) &&
                                                                             !segmentTicketCouponStatusCodes.Contains(segment.TicketCouponStatus)
                                                                            )
                                                                        ) &&
                                                                        Convert.ToDateTime((segment.ScheduledDepartureDateTimeGMT)).AddHours(hours) < DateTime.UtcNow);
                }
            }
            catch
            {
                isNotFlownSegmentExist = false;
            }
            return isNotFlownSegmentExist;
        }

        public static bool IsELFFare(string productCode)
        {
            return EnableIBEFull() && !string.IsNullOrWhiteSpace(productCode) &&
                   "ELF" == productCode.Trim().ToUpper();
        }
        public static string[] SplitConcatenatedConfigValue(string configkey, string splitchar)
        {
            try
            {
                string[] splitSymbol = { splitchar };
                string[] splitString = _configuration.GetValue<string>(configkey)
                    .Split(splitSymbol, StringSplitOptions.None);
                return splitString;
            }
            catch { return null; }
        }

        public static bool EnablePetInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhonePetInformationVersion", "AndroidPetInformationVersion", "", "", true, _configuration);
        }
        public static MOBPNRAdvisory PopulateTRCAdvisoryContent(string displaycontent)
        {
            try
            {
                string[] stringarray
                    = SplitConcatenatedConfigValue("ManageResTRCContent", "||");

                if (stringarray == null || !stringarray.Any()) return null;

                MOBPNRAdvisory content = new MOBPNRAdvisory();

                stringarray.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] lineitem = UtilityHelper.SplitConcatenatedString(item, "|");

                        if (lineitem?.Length > 1)
                        {
                            switch (lineitem[0])
                            {
                                case "Header":
                                    content.Header = lineitem[1];
                                    break;
                                case "Body":
                                    content.Body = lineitem[1];
                                    break;
                                case "ButtonText":
                                    content.Buttontext = lineitem[1];
                                    break;
                            }
                        }
                    }
                });
                return content;
            }
            catch { return null; }
        }

        public static bool IncludeTRCAdvisory(MOBPNR pnr, int appId, string appVersion)
        {
            try
            {
                if (!GeneralHelper.IsApplicationVersionGreaterorEqual
                    (appId, appVersion, _configuration.GetValue<string>("AndroidTRCAdvisoryVersion"), _configuration.GetValue<string>("iPhoneTRCAdvisoryVersion"))) return false;

                if (!string.IsNullOrEmpty(pnr.FareLockMessage) || pnr.isgroup || !pnr.IsETicketed
                    || pnr.IsPetAvailable || pnr.HasScheduleChanged || pnr.IsCanceledWithFutureFlightCredit) return false;

                return true;
            }
            catch { return false; }
        }
        public static bool CheckMax737WaiverFlight
            (United.Service.Presentation.ReservationResponseModel.PNRChangeEligibilityResponse changeEligibilityResponse)
        {
            if (changeEligibilityResponse == null
                || changeEligibilityResponse.Policies == null
                || !changeEligibilityResponse.Policies.Any()) return false;

            foreach (var policies in changeEligibilityResponse.Policies)
            {
                var max737flightnames = GetListFrmPipelineSeptdConfigString("max737flightnames");
                string flightname = (!string.IsNullOrEmpty(policies.Name)) ? policies.Name.ToUpper() : string.Empty;
                if (max737flightnames != null && max737flightnames.Any() && !string.IsNullOrEmpty(flightname))
                {
                    foreach (string name in max737flightnames)
                    {
                        if (flightname.Contains(name))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }
        public static List<string> GetListFrmPipelineSeptdConfigString(string configkey)
        {
            try
            {
                var retstrarray = new List<string>();
                var configstring = _configuration.GetValue<string>(configkey);
                if (!string.IsNullOrEmpty(configstring))
                {
                    string[] strarray = configstring.Split('|');
                    if (strarray.Any())
                    {
                        strarray.ToList().ForEach(str =>
                        {
                            if (!string.IsNullOrEmpty(str) && !string.IsNullOrWhiteSpace(str))
                                retstrarray.Add(str.Trim());
                        });
                    }
                }
                return (retstrarray.Any()) ? retstrarray : null;
            }
            catch { return null; }
        }

        public static void OneTimeSCChangeCancelAlert(MOBPNR pnr)
        {
            try
            {
                if (pnr.IsSCChangeEligible)
                {
                    string[] onetimecontent;

                    if (pnr.IsSCRefundEligible)
                        onetimecontent = SplitConcatenatedConfigValue("ResDtlSCOneTimeChangeCancel", "||");
                    else
                        onetimecontent = SplitConcatenatedConfigValue("ResDtlSCOneTimeChange", "||");

                    if (onetimecontent?.Length == 2)
                    {
                        MOBPNRAdvisory sconetimechangecanceladvisory = new MOBPNRAdvisory
                        {
                            ContentType = Mobile.Model.Common.ContentType.SCHEDULECHANGE,
                            AdvisoryType = Mobile.Model.Common.AdvisoryType.INFORMATION,
                            Header = onetimecontent[0],
                            Body = onetimecontent[1],
                            IsBodyAsHtml = true,
                            IsDefaultOpen = false,
                        };
                        pnr.AdvisoryInfo = (pnr.AdvisoryInfo == null) ? new List<MOBPNRAdvisory>() : pnr.AdvisoryInfo;
                        pnr.AdvisoryInfo.Add(sconetimechangecanceladvisory);
                    }
                }
            }
            catch { }
        }

        public static string GetCurrencyAmount(double value = 0, string code = "USD", int decimalPlace = 2, string languageCode = "")
        {

            string isNegative = value < 0 ? "- " : "";
            double amount = Math.Abs(value);

            if (string.IsNullOrEmpty(code))
                code = "USD";

            string currencyCode = GetCurrencyCode(code);

            //Handle the currency code which is not in the app setting key - CurrencyCodeMappings
            if (string.IsNullOrEmpty(currencyCode))
                currencyCode = code;

            double.TryParse(amount.ToString(), out double total);
            string currencyAmount = "";

            if (string.Equals(currencyCode, "Miles", StringComparison.OrdinalIgnoreCase))
            {
                currencyAmount = string.Format("{0} {1}", total.ToString("#,##0"), currencyCode);
            }
            else if (languageCode == "")
            {
                currencyAmount = string.Format("{0}{1}{2}", isNegative, currencyCode, total.ToString("N" + decimalPlace));
            }
            else
            {
                CultureInfo locCutlure = new CultureInfo(languageCode);
                Thread.CurrentThread.CurrentCulture = locCutlure;
                NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();
                LocalFormat.CurrencySymbol = currencyCode;
                LocalFormat.CurrencyDecimalDigits = 2;

                currencyAmount = string.Format("{0}{1}", isNegative, amount.ToString("c", LocalFormat));

            }

            return currencyAmount;
        }
        public static bool EnableConsolidatedAdvisoryMessage(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneConsolidatedAdvisoryMessageVersion", "AndroidConsolidatedAdvisoryMessageVersion", "", "", true, _configuration);
        }

        public static bool CheckIfTicketedByUA(ReservationDetail response)
        {
            if (response?.Detail?.Characteristic == null) return false;
            string configbookingsource = _configuration.GetValue<string>("PNRUABookingSource");
            var charbookingsource = UtilityHelper.GetCharactersticDescription_New(response.Detail.Characteristic, "Booking Source");
            if (string.IsNullOrEmpty(configbookingsource) || string.IsNullOrEmpty(charbookingsource)) return false;
            return (configbookingsource.IndexOf(charbookingsource, StringComparison.OrdinalIgnoreCase) > -1);
        }

        public static bool VersionCheck_NullSession_AfterAppUpgradation(MOBRequest request)
        {
            bool isVersionGreaterorEqual = GeneralHelper.IsApplicationVersionGreater2(request.Application.Id, request.Application.Version.Major, "Android_NullSession_AfterUpgradation_AppVersion", "iPhone_NullSession_AfterUpgradation_AppVersion", null, null, _configuration);
            return isVersionGreaterorEqual;
        }

        public static bool IsDeepLinkSupportedAirline(string operatingCarrier, int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("EnableRedesignForInterlineDeepLink") && !string.IsNullOrEmpty(operatingCarrier) && _configuration.GetValue<string>("InterlineAirlineCodesSupportedForDeepLink").Contains(operatingCarrier) && ConfigUtility.CheckClientCatalogForEnablingFeature("InterlineDeepLinkRedesignClientCatalog", catalog);
        }

        public static bool CheckClientCatalogForEnablingFeature(string catalogFeature, List<MOBItem> clientCatalog)
        {
            if (!string.IsNullOrEmpty(catalogFeature) && clientCatalog != null && clientCatalog.Count > 0)
            {
                string catalogId = _configuration.GetValue<string>(catalogFeature);
                var Id = catalogId?.Split('|');
                foreach (var item in clientCatalog)
                {
                    if (Id?.Contains(item?.Id) ?? false)
                        return !string.IsNullOrEmpty(item?.CurrentValue) && item?.CurrentValue == "1";
                }
            }
            return false;
        }

        public static string CreateDeepLinkURLForOtherAirlinesManageRes(string recordLocator, string lastname, string pointOfSale, string languageCode, string operatingCarrierCode, out string carrierAdivisoryMessage)
        {
            carrierAdivisoryMessage = string.Empty;
            if (_configuration.GetValue<string>("InterlineLHAndParternerCode").Contains(operatingCarrierCode))
            {
                carrierAdivisoryMessage = string.Format(HttpUtility.HtmlDecode(_configuration.GetValue<string>("InterlineDeepLinkRedesignBodyMessage")), "Lufthansa.com");
                return CreateLufthansaDeeplink(recordLocator, lastname, pointOfSale, languageCode);
            }
            else if (_configuration.GetValue<string>("InterlineACAndParternerCode").Contains(operatingCarrierCode))
            {
                carrierAdivisoryMessage = string.Format(HttpUtility.HtmlDecode(_configuration.GetValue<string>("InterlineDeepLinkRedesignBodyMessage")), "Aircanada.com");
                return CreateAirCanadaDeeplink(recordLocator, lastname, pointOfSale);
            }
            return string.Empty;
        }


        public static string CreateAirCanadaDeeplink(string recordLocator, string lastName, string countryCode)
        {
            var stringToEncrypt = string.Format("REC_LOC={0}&DIRECT_RETRIEVE_LASTNAME={1}&EMBEDDED_TRANSACTION=RetrievePNRServlet", recordLocator, lastName);
            var cryptoKey = "33e2ce0ef2a458c8e3cb0fa9e6c09a7f";
            var aesEncryption = new RijndaelManaged
            {
                KeySize = 128,
                BlockSize = 128,
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                Key = StringToByteArray(cryptoKey),
            };

            // generate a random IV
            aesEncryption.GenerateIV();
            ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(stringToEncrypt);
            // The result of the encryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, stringToEncrypt.Length);
            var encrypted = $"{ByteArrayToString(aesEncryption.IV)}{ByteArrayToString(cipherText)}";
            return string.Format("https://book.aircanada.com/pl/AConline/en/OverrideServlet?ACTION=MODIFY&BOOKING_FLOW=REBOOK&DIRECT_RETRIEVE=true&SO_SITE_SEND_MAIL=TRUE&COUNTRY=US&LANGUAGE=US&SITE=SAADSAAD&EXTERNAL_ID=GUEST&AIRLINE=UA&ENC={0}", encrypted);
        }

        private static byte[] StringToByteArray(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }

        private static string ByteArrayToString(byte[] ba)
        {
            var hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString();
        }


        public static bool IsEligibleCarrierAndAPPVersion(string operatingCarrier, int appId, string appVersion, List<MOBItem> catalog)
        {
            bool isEligibleCarrier = false;
            if (!string.IsNullOrEmpty(operatingCarrier) && catalog != null && catalog.Count > 0)
            {
                switch (operatingCarrier)
                {
                    case "LH": // For Lufthansa
                    case "CL":
                        isEligibleCarrier = EnableRedesignForLufthansaDeepLink(operatingCarrier, appId, appVersion, catalog);
                        break;
                    case "AC": // For AirCanada
                    case "QK":
                    case "RV":
                        isEligibleCarrier = EnableRedesignForAirCanadaDeepLink(operatingCarrier, appId, appVersion, catalog);
                        break;
                    default:
                        isEligibleCarrier = false;
                        break;
                }
            }

            return isEligibleCarrier;
        }

        public static bool EnableRedesignForLufthansaDeepLink(string operatingCarrierCode, int applicationId, string appVersion, List<MOBItem> clientCatalog)
        {
            return _configuration.GetValue<bool>("EnableRedesignForLufthansaDeepLink") &&
                                    GeneralHelper.isApplicationVersionGreater(applicationId, appVersion, "AndroidInterlineDeepLinkRedesignVersion", "iPhoneInterlineDeepLinkRedesignVersion", "", "", true, _configuration) && CheckClientCatalogForEnablingFeature("InterlineDeepLinkRedesignClientCatalog", clientCatalog);

        }

        public static bool EnableRedesignForAirCanadaDeepLink(string operatingCarrierCode, int applicationId, string appVersion, List<MOBItem> clientCatalog)
        {
            return _configuration.GetValue<bool>("EnableRedesignForAirCanadaDeepLink") &&
                                   GeneralHelper.isApplicationVersionGreater(applicationId, appVersion, "AndroidAirCanadaInterlineDeepLinkRedesignVersion", "iPhoneAirCanadaInterlineDeepLinkRedesignVersion", "", "", true, _configuration) && CheckClientCatalogForEnablingFeature("InterlineDeepLinkRedesignClientCatalog", clientCatalog);

        }

        public static bool IsDeepLinkSupportedAirline(string operatingCarrier)
        {
            return !string.IsNullOrEmpty(operatingCarrier) && _configuration.GetValue<string>("InterlineAirlineCodesSupportedForDeepLink").Contains(operatingCarrier);
        }

        public static bool EnableEPlusAncillary(int appID, string appVersion, bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableEPlusAncillaryChanges") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("EplusAncillaryAndroidversion"), _configuration.GetValue<string>("EplusAncillaryiOSversion"));
        }

        public static bool IsInternationalBillingAddress_CheckinFlowEnabled(MOBApplication application)
        {
            if (_configuration.GetValue<bool>("EnableInternationalBillingAddress_CheckinFlow"))
            {
                if (application != null && GeneralHelper.IsApplicationVersionGreater(application.Id, application.Version.Major, "IntBillingCheckinFlowAndroidversion", "IntBillingCheckinFlowiOSversion", "", "", true, _configuration))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IncludeTravelBankFOP(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelBankFOP")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidTravelBankFOPVersion", "iPhoneTravelBankFOPVersion", "", "", true,_configuration);
        }

        public static bool IncludeTravelCredit(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelCredit") &&
                   GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTravelCreditVersion", "iPhoneTravelCreditVersion", "", "", true,_configuration);
        }

        public static bool EnableOAMessageUpdate(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOAMsgUpdate") &&
                   GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "Android_EnableOAMessageUpdate_AppVersion", "IPhone_EnableOAMessageUpdate_AppVersion", "", "", true, _configuration);
        }

        public static bool IsOAReadOnlySeatMap(string operatingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier)) return false;
            var interLineAirlines = _configuration.GetValue<string>("ReadOnlySeatMapCarriers");
            if (string.IsNullOrEmpty(interLineAirlines)) return false;

            if (interLineAirlines.Contains(operatingCarrier.Trim().ToUpper()))
                return true;

            return false;
        }
        public static bool EnableShoppingcartPhase2ChangesWithVersionCheck(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableShoppingCartPhase2Changes")
                 && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "Android_EnableShoppingCartPhase2Changes_AppVersion", "iPhone_EnableShoppingCartPhase2Changes_AppVersion", "", "", true, _configuration);
        }

        private static string BuildPaxTypeDescription(string paxTypeCode, string paxDescription, int paxCount)
        {
            string description = paxDescription;
            if (!string.IsNullOrEmpty(paxTypeCode))
            {
                switch (paxTypeCode.ToUpper())
                {
                    case "ADT":
                        description = $"{((paxCount == 1) ? "adult (18-64)" : "adults (18-64)")} ";
                        break;
                    case "SNR":
                        description = $"{((paxCount == 1) ? "senior (65+)" : "seniors (65+)")} ";
                        break;
                    case "C17":
                        description = $"{((paxCount == 1) ? "child (15-17)" : "children (15-17)")} ";
                        break;
                    case "C14":
                        description = $"{((paxCount == 1) ? "child (12-14)" : "children (12-14)")} ";
                        break;
                    case "C11":
                        description = $"{((paxCount == 1) ? "child (5-11)" : "children (5-11)")} ";
                        break;
                    case "C04":
                        description = $"{((paxCount == 1) ? "child (2-4)" : "children (2-4)")} ";
                        break;
                    case "INS":
                        description = $"{((paxCount == 1) ? "infant(under 2) - seat" : "infants(under 2) - seat")} ";
                        break;
                    case "INF":
                        description = $"{((paxCount == 1) ? "infant (under 2) - lap" : "infants (under 2) - lap")} ";
                        break;
                    default:
                        description = paxDescription;
                        break;
                }
            }
            return description;
        }
        public static bool IsIncludeWithThisToggle(int appId, string appVersion, string configToggleKey, string androidVersion, string iosVersion)
        {
            return _configuration.GetValue<bool>(configToggleKey) &&
                   GeneralHelper.IsApplicationVersionGreater(appId, appVersion, androidVersion, iosVersion, "", "", true, _configuration);
        }
    }
}
