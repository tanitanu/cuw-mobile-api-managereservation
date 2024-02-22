using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
//using United.Mobile.Model.Shopping.Internal;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using United.Definition;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Shopping;
using United.Mobile.Model;
using United.Mobile.Model.Catalog;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Bundles;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.PriceBreakDown;
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
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
//using Characteristic = United.Mobile.Model.Common.Characteristic;
using ContentType = United.Mobile.Model.Common.ContentType;
using FlowType = United.Utility.Enum.FlowType;
using MOBBKTraveler = United.Mobile.Model.Shopping.Booking.MOBBKTraveler;
using MOBFOPCertificateTraveler = United.Mobile.Model.Shopping.FormofPayment.MOBFOPCertificateTraveler;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using ShopResponse = United.Services.FlightShopping.Common.ShopResponse;
using Trip = United.Services.FlightShopping.Common.Trip;

namespace United.Common.Helper.Shopping
{
    public class ShoppingUtility : IShoppingUtility
    {
        private readonly ICacheLog<ShoppingUtility> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IDPService _dPService;
        private readonly IHeaders _headers;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly ICachingService _cachingService;
        //private static Optimizely optimizely = null;
        private readonly IShoppingBuyMiles _shoppingBuyMiles;
        private readonly IValidateHashPinService _validateHashPinService;
        private readonly IOptimizelyPersistService _optimizelyPersistService;
        private readonly IFFCShoppingcs _ffcShoppingcs;
        //  private readonly IOmniCart _omniCart;
        private readonly IMPSignInCommonService _mPSignInCommonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureSettings _featureSettings;

        public ShoppingUtility(ICacheLog<ShoppingUtility> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IDPService dPService
            , IHeaders headers
            , IDynamoDBService dynamoDBService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , ICachingService cachingService
            , IValidateHashPinService validateHashPinService
            , IOptimizelyPersistService optimizelyPersistService
            , IShoppingBuyMiles shoppingBuyMiles
            , IFFCShoppingcs ffcShoppingcs
            //,IOmniCart omniCart
            , IMPSignInCommonService mPSignInCommonService
            , IHttpContextAccessor httpContextAccessor,
              IFeatureSettings featureSettings
            )
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _dPService = dPService;
            _headers = headers;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _cachingService = cachingService;
            _validateHashPinService = validateHashPinService;
            _optimizelyPersistService = optimizelyPersistService;
            _shoppingBuyMiles = shoppingBuyMiles;
            _ffcShoppingcs = ffcShoppingcs;
            //  _omniCart = omniCart;
            _mPSignInCommonService = mPSignInCommonService;
            _httpContextAccessor = httpContextAccessor;
            _featureSettings = featureSettings;
            new ConfigUtility(_configuration);
        }



        public bool IsSeatMapSupportedOa(string operatingCarrier, string MarketingCarrier)
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

        public bool EnablePreferredZone(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("isEnablePreferredZone")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPreferredSeatVersion", "iOSPreferredSeatVersion", "", "", true, _configuration);
            }
            return false;
        }

        public bool IsUPPSeatMapSupportedVersion(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableUPPSeatmap")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidUPPSeatmapVersion", "iPhoneUPPSeatmapVersion", "", "", true, _configuration);
            }

            return false;
        }

        public bool IsMixedCabinFilerEnabled(int id, string version)
        {
            if (!_configuration.GetValue<bool>("EnableAwardMixedCabinFiter")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("Android_AwardMixedCabinFiterFeatureSupported_AppVersion"), _configuration.GetValue<string>("iPhone_AwardMixedCabinFiterFeatureSupported_AppVersion"));
        }

        public bool OaSeatMapExceptionVersion(int applicationId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "AndroidOaSeatMapExceptionVersion", "iPhoneOaSeatMapExceptionVersion", "", "", true, _configuration);
        }

        public bool IsIBE(Reservation persistedReservation)
        {
            if (_configuration.GetValue<bool>("EnablePBE") && (persistedReservation.ShopReservationInfo2 != null))
            {
                return persistedReservation.ShopReservationInfo2.IsIBE;
            }
            return false;
        }

        public bool IsEMinusSeat(string programCode)
        {
            if (!_configuration.GetValue<bool>("EnableSSA") || programCode.IsNullOrEmpty())
                return false;
            programCode = programCode.ToUpper().Trim();
            return programCode.Equals("ASA") || programCode.Equals("BSA");
        }


        public bool OaSeatMapSupportedVersion(int applicationId, string appVersion, string carrierCode, string MarketingCarrier = "")
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

        public bool EnableAirCanada(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAirCanada")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidAirCanadaVersion", "iPhoneAirCanadaVersion", "", "", true, _configuration);
        }

        public bool EnableTravelerTypes(int appId, string appVersion, bool reshop = false)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableTravelerTypes") && !reshop
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTravelerTypesVersion", "iPhoneTravelerTypesVersion", "", "", true, _configuration);
            }
            return false;
        }

        public bool ShopTimeOutCheckforAppVersion(int appID, string appVersion)
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

        public async Task<bool> ValidateHashPinAndGetAuthToken(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string sessionId)
        {
            var list = await new HashPin(_logger, _configuration, _validateHashPinService, _dynamoDBService, _mPSignInCommonService, _headers, _httpContextAccessor, _featureSettings).ValidateHashPinAndGetAuthTokenDynamoDB(accountNumber, hashPinCode, applicationId, deviceId, appVersion, sessionId).ConfigureAwait(false);

            var ok = (list != null && !string.IsNullOrEmpty(list.HashPincode)) ? true : false;

            return ok;
        }

        public bool EnableRoundTripPricing(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("Shopping - bPricingBySlice")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableRoundTripPricingVersion", "iPhoneEnableRoundTripPricingVersion", "", "", true, _configuration);
        }

        public bool EnableIBEFull()
        {
            return _configuration.GetValue<bool>("EnableIBE");
        }

        public bool EnableIBELite()
        {
            return _configuration.GetValue<bool>("EnableIBELite");
        }

        public bool EnableRtiMandateContentsToDisplayByMarket(int appID, string appVersion, bool isReshop)
        {
            return _configuration.GetValue<bool>("EnableRtiMandateContentsToDisplayByMarket") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("CovidTestAndroidversion"), _configuration.GetValue<string>("CovidTestiOSversion"));
        }


        public void GetAirportCityName(string airportCode, ref string airportName, ref string cityName)
        {
            #region
            try
            {
                AirportDynamoDB airportDynamoDB = new AirportDynamoDB(_configuration, _dynamoDBService);
                airportDynamoDB.GetAirportCityName(airportCode, ref airportName, ref cityName, _headers.ContextValues.SessionId);

            }
            catch (System.Exception) { }
            #endregion
        }

        public bool IsEnableOmniCartMVP2Changes(int applicationId, string appVersion, bool isDisplayCart)
        {
            if (_configuration.GetValue<bool>("EnableOmniCartMVP2Changes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableOmniCartMVP2Changes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableOmniCartMVP2Changes_AppVersion")) && isDisplayCart)
            {
                return true;
            }
            return false;
        }

        public string GetFeeWaiverMessageSoftRTI()
        {
            return _configuration.GetValue<string>("ChangeFeeWaiver_Message_SoftRTI");
        }

        public async Task<MOBSHOPReservation> GetReservationFromPersist(MOBSHOPReservation reservation, string sessionID)
        {
            #region
            Session session = await GetShoppingSession(sessionID);
            Reservation bookingPathReservation = new Reservation();
            bookingPathReservation = await _sessionHelperService.GetSession<Reservation>(sessionID, bookingPathReservation.ObjectName, new List<string> { sessionID, bookingPathReservation.ObjectName });
            return MakeReservationFromPersistReservation(reservation, bookingPathReservation, session);

            #endregion
        }

        private async Task<Session> GetShoppingSession(string sessionId)
        {
            return await GetShoppingSession(sessionId, true);
        }


        private async Task<Session> GetShoppingSession(string sessionId, bool saveToPersist)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }

            Session session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            if (session == null)
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            if (session.TokenExpireDateTime <= DateTime.Now)
            {
                session.IsTokenExpired = true;
            }

            session.LastSavedTime = DateTime.Now;

            await _sessionHelperService.SaveSession<Session>(session, sessionId, new List<string> { sessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);
            return session;
        }

        public MOBSHOPReservation MakeReservationFromPersistReservation(MOBSHOPReservation reservation, Reservation bookingPathReservation,
           Session session)
        {
            if (reservation == null)
            {
                reservation = new MOBSHOPReservation(_configuration, _cachingService);
            }
            reservation.CartId = bookingPathReservation.CartId;
            reservation.PointOfSale = bookingPathReservation.PointOfSale;
            reservation.ClubPassPurchaseRequest = bookingPathReservation.ClubPassPurchaseRequest;
            reservation.CreditCards = bookingPathReservation.CreditCards;
            reservation.CreditCardsAddress = bookingPathReservation.CreditCardsAddress;
            reservation.FareLock = bookingPathReservation.FareLock;
            reservation.FareRules = bookingPathReservation.FareRules;
            reservation.IsSignedInWithMP = bookingPathReservation.IsSignedInWithMP;
            reservation.NumberOfTravelers = bookingPathReservation.NumberOfTravelers;
            reservation.Prices = bookingPathReservation.Prices;
            reservation.SearchType = bookingPathReservation.SearchType;
            reservation.SeatPrices = bookingPathReservation.SeatPrices;
            reservation.SessionId = session.SessionId;
            reservation.Taxes = bookingPathReservation.Taxes;
            reservation.UnregisterFareLock = bookingPathReservation.UnregisterFareLock;
            reservation.AwardTravel = bookingPathReservation.AwardTravel;
            reservation.LMXFlights = bookingPathReservation.LMXFlights;
            reservation.IneligibleToEarnCreditMessage = bookingPathReservation.IneligibleToEarnCreditMessage;
            reservation.OaIneligibleToEarnCreditMessage = bookingPathReservation.OaIneligibleToEarnCreditMessage;
            reservation.SeatPrices = bookingPathReservation.SeatPrices;
            reservation.IsCubaTravel = bookingPathReservation.IsCubaTravel;

            if (bookingPathReservation.TravelersCSL != null && bookingPathReservation.TravelerKeys != null)
            {
                List<MOBCPTraveler> lstTravelers = new List<MOBCPTraveler>();
                foreach (string travelerKey in bookingPathReservation.TravelerKeys)
                {
                    lstTravelers.Add(bookingPathReservation.TravelersCSL[travelerKey]);
                }
                reservation.TravelersCSL = lstTravelers;

                if (session.IsReshopChange)
                {
                    if (reservation.IsCubaTravel)
                    {
                        reservation.TravelersCSL.ForEach(x => { x.PaxID = x.PaxIndex + 1; x.IsPaxSelected = true; });
                    }
                    else
                    {
                        reservation.TravelersCSL.ForEach(x => { x.Message = string.Empty; x.CubaTravelReason = null; });
                    }
                    bookingPathReservation.ShopReservationInfo2.AllEligibleTravelersCSL = reservation.TravelersCSL;
                }
            }

            reservation.TravelersRegistered = bookingPathReservation.TravelersRegistered;
            reservation.TravelOptions = bookingPathReservation.TravelOptions;
            reservation.Trips = bookingPathReservation.Trips;
            reservation.ReservationPhone = bookingPathReservation.ReservationPhone;
            reservation.ReservationEmail = bookingPathReservation.ReservationEmail;
            reservation.FlightShareMessage = bookingPathReservation.FlightShareMessage;
            reservation.PKDispenserPublicKey = bookingPathReservation.PKDispenserPublicKey;
            reservation.IsRefundable = bookingPathReservation.IsRefundable;
            reservation.ISInternational = bookingPathReservation.ISInternational;
            reservation.ISFlexibleSegmentExist = bookingPathReservation.ISFlexibleSegmentExist;
            reservation.ClubPassPurchaseRequest = bookingPathReservation.ClubPassPurchaseRequest;
            reservation.GetALLSavedTravelers = bookingPathReservation.GetALLSavedTravelers;
            reservation.IsELF = bookingPathReservation.IsELF;
            reservation.IsSSA = bookingPathReservation.IsSSA;
            reservation.IsMetaSearch = bookingPathReservation.IsMetaSearch;
            reservation.MetaSessionId = bookingPathReservation.MetaSessionId;
            reservation.IsUpgradedFromEntryLevelFare = bookingPathReservation.IsUpgradedFromEntryLevelFare;
            reservation.SeatAssignmentMessage = bookingPathReservation.SeatAssignmentMessage;
            reservation.IsReshopCommonFOPEnabled = bookingPathReservation.IsReshopCommonFOPEnabled;


            if (bookingPathReservation.TCDAdvisoryMessages != null && bookingPathReservation.TCDAdvisoryMessages.Count > 0)
            {
                reservation.TCDAdvisoryMessages = bookingPathReservation.TCDAdvisoryMessages;
            }
            //##Price Break Down - Kirti
            if (_configuration.GetValue<string>("EnableShopPriceBreakDown") != null &&
                Convert.ToBoolean(_configuration.GetValue<string>("EnableShopPriceBreakDown")))
            {
                reservation.ShopPriceBreakDown = GetPriceBreakDown(bookingPathReservation);
            }

            if (session != null && !string.IsNullOrEmpty(session.EmployeeId) && reservation != null)
            {
                reservation.IsEmp20 = true;
            }
            if (bookingPathReservation.IsCubaTravel)
            {
                reservation.CubaTravelInfo = bookingPathReservation.CubaTravelInfo;
            }
            reservation.FormOfPaymentType = bookingPathReservation.FormOfPaymentType;
            if (bookingPathReservation.FormOfPaymentType == MOBFormofPayment.PayPal || bookingPathReservation.FormOfPaymentType == MOBFormofPayment.PayPalCredit)
            {
                reservation.PayPal = bookingPathReservation.PayPal;
                reservation.PayPalPayor = bookingPathReservation.PayPalPayor;
            }
            if (bookingPathReservation.FormOfPaymentType == MOBFormofPayment.Masterpass)
            {
                if (bookingPathReservation.MasterpassSessionDetails != null)
                    reservation.MasterpassSessionDetails = bookingPathReservation.MasterpassSessionDetails;
                if (bookingPathReservation.Masterpass != null)
                    reservation.Masterpass = bookingPathReservation.Masterpass;
            }
            if (bookingPathReservation.FOPOptions != null && bookingPathReservation.FOPOptions.Count > 0) //FOP Options Fix Venkat 12/08
            {
                reservation.FOPOptions = bookingPathReservation.FOPOptions;
            }

            if (bookingPathReservation.IsReshopChange)
            {
                reservation.ReshopTrips = bookingPathReservation.ReshopTrips;
                reservation.Reshop = bookingPathReservation.Reshop;
                reservation.IsReshopChange = true;
            }
            reservation.ELFMessagesForRTI = bookingPathReservation.ELFMessagesForRTI;
            if (bookingPathReservation.ShopReservationInfo != null)
            {
                reservation.ShopReservationInfo = bookingPathReservation.ShopReservationInfo;
            }
            if (bookingPathReservation.ShopReservationInfo2 != null)
            {
                reservation.ShopReservationInfo2 = bookingPathReservation.ShopReservationInfo2;
            }

            if (bookingPathReservation.ReservationEmail != null)
            {
                reservation.ReservationEmail = bookingPathReservation.ReservationEmail;
            }

            if (bookingPathReservation.TripInsuranceFile != null && bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo != null)
            {
                reservation.TripInsuranceInfoBookingPath = bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo;
            }
            else
            {
                reservation.TripInsuranceInfoBookingPath = null;
            }
            reservation.AlertMessages = bookingPathReservation.AlertMessages;
            reservation.IsRedirectToSecondaryPayment = bookingPathReservation.IsRedirectToSecondaryPayment;
            reservation.RecordLocator = bookingPathReservation.RecordLocator;
            reservation.Messages = bookingPathReservation.Messages;
            reservation.CheckedbagChargebutton = bookingPathReservation.CheckedbagChargebutton;
            return reservation;
        }

        public TripPriceBreakDown GetPriceBreakDown(Reservation reservation)
        {
            //##Price Break Down - Kirti
            var priceBreakDownObj = new TripPriceBreakDown();
            bool hasAward = false;
            string awardPrice = string.Empty;
            string basePrice = string.Empty;
            string totalPrice = string.Empty;
            bool hasOneTimePass = false;
            string oneTimePassCost = string.Empty;
            bool hasFareLock = false;
            double awardPriceValue = 0;
            double basePriceValue = 0;

            if (reservation != null)
            {
                priceBreakDownObj.PriceBreakDownDetails = new PriceBreakDownDetails();
                priceBreakDownObj.PriceBreakDownSummary = new PriceBreakDownSummary();

                foreach (var travelOption in reservation.TravelOptions)
                {
                    if (travelOption.Key.Equals("FareLock"))
                    {
                        hasFareLock = true;

                        priceBreakDownObj.PriceBreakDownDetails.FareLock = new List<PriceBreakDown2Items>();
                        priceBreakDownObj.PriceBreakDownSummary.FareLock = new PriceBreakDown2Items();
                        var fareLockAmount = new PriceBreakDown2Items();
                        foreach (var subItem in travelOption.SubItems)
                        {
                            if (subItem.Key.Equals("FareLockHoldDays"))
                            {
                                fareLockAmount.Text1 = string.Format("{0} {1}", subItem.Amount, "Day FareLock");
                            }
                        }
                        //Row 0 Column 0
                        fareLockAmount.Price1 = travelOption.DisplayAmount;
                        priceBreakDownObj.PriceBreakDownDetails.FareLock.Add(fareLockAmount);
                        priceBreakDownObj.PriceBreakDownSummary.FareLock = fareLockAmount;

                        priceBreakDownObj.PriceBreakDownDetails.FareLock.Add(new PriceBreakDown2Items() { Text1 = "Total due now" });
                        //Row 1 Column 0
                    }
                }

                StringBuilder tripType = new StringBuilder();
                if (reservation.SearchType.Equals("OW"))
                {
                    tripType.Append("Oneway");
                }
                else if (reservation.SearchType.Equals("RT"))
                {
                    tripType.Append("Roundtrip");
                }
                else
                {
                    tripType.Append("MultipleTrip");
                }
                tripType.Append(" (");
                tripType.Append(reservation.NumberOfTravelers);
                tripType.Append(reservation.NumberOfTravelers > 1 ? " travelers)" : " traveler)");
                //row 2 coulum 0

                foreach (var price in reservation.Prices)
                {
                    switch (price.DisplayType)
                    {
                        case "MILES":
                            hasAward = true;
                            awardPrice = price.FormattedDisplayValue;
                            awardPriceValue = price.Value;
                            break;

                        case "TRAVELERPRICE":
                            basePrice = price.FormattedDisplayValue;
                            basePriceValue = price.Value;
                            break;

                        case "TOTAL":
                            totalPrice = price.FormattedDisplayValue;
                            break;

                        case "ONE-TIME PASS":
                            hasOneTimePass = true;
                            oneTimePassCost = price.FormattedDisplayValue;
                            break;

                        case "GRAND TOTAL":
                            if (!hasFareLock)
                                totalPrice = price.FormattedDisplayValue;
                            break;
                    }
                }

                string travelPrice = string.Empty;
                double travelPriceValue = 0;
                //row 2 column 1
                if (hasAward)
                {
                    travelPrice = awardPrice;
                    travelPriceValue = awardPriceValue;
                }
                else
                {
                    travelPrice = basePrice;
                    travelPriceValue = basePriceValue;
                }

                priceBreakDownObj.PriceBreakDownDetails.Trip = new PriceBreakDown2Items() { Text1 = tripType.ToString(), Price1 = travelPrice };

                priceBreakDownObj.PriceBreakDownSummary.TravelOptions = new List<PriceBreakDown2Items>();

                decimal taxNfeesTotal = 0;
                ShopStaticUtility.BuildTaxesAndFees(reservation, priceBreakDownObj, out taxNfeesTotal);

                if (((reservation.SeatPrices != null && reservation.SeatPrices.Count > 0) ||
                    reservation.TravelOptions != null && reservation.TravelOptions.Count > 0 || hasOneTimePass) && !hasFareLock)
                {
                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices = new PriceBreakDownAddServices();

                    // Row n+ 5 column 0
                    // Row n+ 5 column 1

                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats = new List<PriceBreakDown4Items>();
                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.Seats.Add(new PriceBreakDown4Items() { Text1 = "Additional services:" });

                    ShopStaticUtility.BuildSeatPrices(reservation, priceBreakDownObj);

                    ShopStaticUtility.BuildTravelOptions(reservation, priceBreakDownObj);
                }

                if (hasOneTimePass)
                {
                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.OneTimePass = new List<PriceBreakDown2Items>();
                    priceBreakDownObj.PriceBreakDownDetails.AdditionalServices.OneTimePass.Add(new PriceBreakDown2Items() { Text1 = "One-Time Pass", Price1 = oneTimePassCost });

                    priceBreakDownObj.PriceBreakDownSummary.TravelOptions.Add(new PriceBreakDown2Items() { Text1 = "One-Time Pass", Price1 = oneTimePassCost });
                }

                var finalPriceSummary = new PriceBreakDown2Items();

                priceBreakDownObj.PriceBreakDownDetails.Total = new List<PriceBreakDown2Items>();
                priceBreakDownObj.PriceBreakDownSummary.Total = new List<PriceBreakDown2Items>();
                if (hasFareLock)
                {
                    //column 0
                    finalPriceSummary.Text1 = "Total price (held)";
                }
                else
                {
                    //  buildDottedLine(); column 1
                    finalPriceSummary.Text1 = "Total price";
                }
                if (hasAward)
                {
                    //colum 1
                    finalPriceSummary.Price1 = awardPrice;
                    priceBreakDownObj.PriceBreakDownDetails.Total.Add(finalPriceSummary);

                    priceBreakDownObj.PriceBreakDownSummary.Total.Add(new PriceBreakDown2Items() { Price1 = string.Format("+{0}", totalPrice) });

                    priceBreakDownObj.PriceBreakDownSummary.Trip = new List<PriceBreakDown2Items>()
                             {
                                 new PriceBreakDown2Items()
                                 {
                                    Text1 = tripType.ToString(), Price1 = string.Format("${0}", taxNfeesTotal.ToString("F"))
                                 }
                             };
                }
                else
                {
                    //column 1
                    finalPriceSummary.Price1 = totalPrice;
                    priceBreakDownObj.PriceBreakDownDetails.Total.Add(new PriceBreakDown2Items() { Text1 = totalPrice });

                    priceBreakDownObj.PriceBreakDownSummary.Trip = new List<PriceBreakDown2Items>()
                             {
                                new PriceBreakDown2Items()
                                {
                                  Text1 = tripType.ToString(), Price1 = string.Format("${0}", (travelPriceValue + Convert.ToDouble(taxNfeesTotal)).ToString("F"))
                                }
                             };
                }

                priceBreakDownObj.PriceBreakDownSummary.Total.Add(finalPriceSummary);
            }

            return priceBreakDownObj;
        }


        public bool EnableForceEPlus(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableForceEPlus")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidForceEPlusVersion", "iPhoneForceEPlusVersion", "", "", true, _configuration);
        }

        public bool IsEnabledNationalityAndResidence(bool isReShop, int appid, string appversion)
        {
            if (!isReShop && EnableNationalityResidence(appid, appversion))
            {
                return true;
            }

            return false;
        }

        public bool EnableNationalityResidence(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableNationalityAndCountryOfResidence")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidiPhonePriceChangeVersion", "AndroidiPhonePriceChangeVersion", "", "", true, _configuration);
        }

        public bool EnableSpecialNeeds(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSpecialNeeds")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableSpecialNeedsVersion", "iPhoneEnableSpecialNeedsVersion", "", "", true, _configuration);
        }

        public bool EnableInflightContactlessPayment(int appID, string appVersion, bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableInflightContactlessPayment") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("InflightContactlessPaymentAndroidVersion"), _configuration.GetValue<string>("InflightContactlessPaymentiOSVersion"));
        }

        public bool AllowElfMetaSearchUpsell(int appId, string version)
        {
            var isSupportedAppVersion = GeneralHelper.IsApplicationVersionGreater(appId, version, "AndroidELFMetaSearchUpsellVersion", "iPhoneELFMetaSearchUpsellVersion", "", "", true, _configuration);
            if (isSupportedAppVersion)
            {
                return _configuration.GetValue<bool>("AllowELFMetaSearchUpsell");
            }
            return false;
        }

        public bool EnableUnfinishedBookings(MOBRequest request)
        {
            return _configuration.GetValue<bool>("EnableUnfinishedBookings")
                    && GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidEnableUnfinishedBookingsVersion", "iPhoneEnableUnfinishedBookingsVersion", "", "", true, _configuration);
        }

        public MOBSHOPUnfinishedBookingTrip MapToMOBSHOPUnfinishedBookingTrip(United.Mobile.Model.ShopTrips.Trip csTrip)
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

        public MOBSHOPUnfinishedBookingFlight MapToMOBSHOPUnfinishedBookingFlight(United.Mobile.Model.ShopTrips.Flight cslFlight)
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
            if (cslFlight.Price != 0)
            {
                ubMOBFlight.Products = new List<MOBSHOPUnfinishedBookingFlightProduct>();
                var ubproduct = new MOBSHOPUnfinishedBookingFlightProduct { Prices = new List<MOBSHOPUnfinishedBookingProductPrice>() };
                ubproduct.Prices.Add(new MOBSHOPUnfinishedBookingProductPrice { Amount = cslFlight.Price ?? 0 });
                ubMOBFlight.Products.Add(ubproduct);
            }

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

        public bool EnableSavedTripShowChannelTypes(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableUnfinishedBookings") // feature toggle
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableUnfinishedBookingsVersion", "iPhoneEnableUnfinishedBookingsVersion", "", "", true, _configuration)

                    && _configuration.GetValue<bool>("EnableSavedTripShowChannelTypes") // story toggle
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableSavedTripShowChannelTypesVersion", "iPhoneEnableSavedTripShowChannelTypesVersion", "", "", true, _configuration);
        }

        public List<MOBTypeOption> GetFopOptions(int applicationID, string appVersion)
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

        public List<MOBTypeOption> GetAppsFOPOptions(string appVersion, string[] fopTypesByLatestVersion)
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

        public MOBTypeOption GetAvailableFopOptions(string fopType)
        {
            MOBTypeOption fopOption = new MOBTypeOption();  // <add key="FOPOption1" value="ApplePay|Apple Pay" />
            fopOption.Key = string.IsNullOrEmpty(_configuration.GetValue<string>(fopType)) ? "" : _configuration.GetValue<string>(fopType).Split('|')[0];
            fopOption.Value = string.IsNullOrEmpty(_configuration.GetValue<string>(fopType)) ? "" : _configuration.GetValue<string>(fopType).Split('|')[1];
            return fopOption;
        }

        public bool IsDisplayCart(Session session, string travelTypeConfigKey = "DisplayCartTravelTypes")
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

        public string GetCSSPublicKeyPersistSessionStaticGUID(int applicationId)
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

        public List<List<MOBSHOPTax>> GetTaxAndFeesAfterPriceChange(List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> prices, bool isReshopChange = false, int appId = 0, string appVersion = "", string travelType = null)
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

                if (_configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && !string.IsNullOrEmpty(price?.Type) && price.Type.ToUpper() == "NONDISCOUNTPRICE-TRAVELERPRICE")
                    continue;

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

        public bool EnableYADesc(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableYoungAdultBooking") && _configuration.GetValue<bool>("EnableYADesc") && !isReshop;
        }

        public bool IsEnableTaxForAgeDiversification(bool isReShop, int appid, string appversion)
        {
            if (!isReShop && EnableTaxForAgeDiversification(appid, appversion))
            {
                return true;
            }
            return false;
        }

        public bool EnableTaxForAgeDiversification(int appId, string appVersion)
        {
            // return GetBooleanConfigValue("EnableForceEPlus");
            return _configuration.GetValue<bool>("EnableTaxForAgeDiversification")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidiPhoneTaxForAgeDiversificationVersion", "AndroidiPhoneTaxForAgeDiversificationVersion", "", "", true, _configuration);
        }

        public async Task SetELFUpgradeMsg(MOBSHOPAvailability availability, string productCode, MOBRequest request, Session session)
        {
            if (_configuration.GetValue<bool>("ByPassSetUpUpgradedFromELFMessages"))
            {
                if (availability?.Reservation?.IsUpgradedFromEntryLevelFare ?? false)
                {
                    if (availability.Reservation.ShopReservationInfo2.InfoWarningMessages == null)
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages = new List<InfoWarningMessages>();

                    if (IsNonRefundableNonChangable(productCode))
                    {
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages.Add(await BuildUpgradeFromNonRefuNonChanInfoMessage(request, session));
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages = availability.Reservation.ShopReservationInfo2.InfoWarningMessages.OrderBy(c => (int)((MOBINFOWARNINGMESSAGEORDER)Enum.Parse(typeof(MOBINFOWARNINGMESSAGEORDER), c.Order))).ToList();
                    }
                    else
                    {
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages.Add(BuildUpgradeFromELFInfoMessage(request.Application.Id));
                        availability.Reservation.ShopReservationInfo2.InfoWarningMessages = availability.Reservation.ShopReservationInfo2.InfoWarningMessages.OrderBy(c => (int)(MOBINFOWARNINGMESSAGEORDER)Enum.Parse(typeof(MOBINFOWARNINGMESSAGEORDER), c.Order)).ToList();
                    }
                }
            }
        }

        public InfoWarningMessages BuildUpgradeFromELFInfoMessage(int ID)
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

        public InfoWarningMessages GetBEMessage()
        {
            var message = _configuration.GetValue<string>("BEFareInversionMessage") as string ?? string.Empty;
            return ShopStaticUtility.BuildInfoWarningMessages(message);
        }

        public InfoWarningMessages GetBoeingDisclaimer()
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

        public bool IsBoeingDisclaimer(List<DisplayTrip> trips)
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

        public bool IsMaxBoeing(string boeingType)
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

        public bool IsConBoeingDisclaimer(Flight flight)
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

        public bool EnableBoeingDisclaimer(bool isReshop = false)
        {
            return _configuration.GetValue<bool>("ENABLEBOEINGDISCLOUSER") && !isReshop;
        }

        public InfoWarningMessages GetInhibitMessage(string bookingCutOffMinutes)
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

        public bool IsIBEFullFare(DisplayCart displayCart)
        {
            return EnableIBEFull() &&
                    displayCart != null &&
                    IsIBEFullFare(displayCart.ProductCode);
        }

        public bool IsIBEFullFare(string productCode)
        {
            var iBEFullProductCodes = _configuration.GetValue<string>("IBEFullShoppingProductCodes");
            return EnableIBEFull() && !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBEFullProductCodes) &&
                   iBEFullProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        public bool IsIBELiteFare(DisplayCart displayCart)
        {
            return EnableIBELite() &&
                    displayCart != null &&
                    IsIBELiteFare(displayCart.ProductCode);
        }

        public bool IsIBELiteFare(string productCode)
        {
            var iBELiteProductCodes = _configuration.GetValue<string>("IBELiteShoppingProductCodes");
            return !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(iBELiteProductCodes) &&
                   iBELiteProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        public bool EnablePBE()
        {
            return _configuration.GetValue<bool>("EnablePBE");
        }

        public List<MOBSHOPPrice> GetPrices(List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> prices,
            bool isAwardBooking, string sessionId, bool isReshopChange = false, string searchType = null,
            bool isFareLockViewRes = false, bool isCorporateFare = false, DisplayCart displayCart = null,
            int appId = 0, string appVersion = "", List<MOBItem> catalogItems = null, bool isNotSelectTripCall = false,
             FlightReservationResponse shopBookingDetailsResponse = null
             , List<United.Services.FlightShopping.Common.DisplayCart.DisplayPrice> displayFees = null, bool isRegisterOffersFlow = false,
             Session session = null)
        {
            List<MOBSHOPPrice> bookingPrices = new List<MOBSHOPPrice>();
            CultureInfo ci = null;
            var isEnableOmniCartMVP2Changes = _configuration.GetValue<bool>("EnableOmniCartMVP2Changes");
            foreach (var price in prices)
            {
                if (ci == null)
                {
                    ci = TopHelper.GetCultureInfo(price.Currency);
                }

                MOBSHOPPrice bookingPrice = new MOBSHOPPrice();
                decimal NonDiscountTravelPrice = 0;
                double promoValue = 0;
                if (_configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking"))
                {
                    if (price.Type.Equals("NONDISCOUNTPRICE-TRAVELERPRICE", StringComparison.OrdinalIgnoreCase) || price.Type.Equals("NonDiscountPrice-Total", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }
                    if (price.Type.Equals("TRAVELERPRICE", StringComparison.OrdinalIgnoreCase) || (price.Type.Equals("TOTAL", StringComparison.OrdinalIgnoreCase)))
                    {
                        if (price.Type.Equals("TRAVELERPRICE", StringComparison.OrdinalIgnoreCase))
                        {
                            var nonDiscountedPrice = prices.Find(dp => dp.PaxTypeCode == price.PaxTypeCode && dp.Type.ToUpper().Equals("NONDISCOUNTPRICE-TRAVELERPRICE"));
                            var discountedPrice = prices.Find(dp => dp.PaxTypeCode == price.PaxTypeCode && dp.Type.ToUpper().Equals("TRAVELERPRICE"));
                            if (discountedPrice != null && nonDiscountedPrice != null)
                            {
                                promoValue = Math.Round(Convert.ToDouble(nonDiscountedPrice.Amount)
                                             - Convert.ToDouble(discountedPrice.Amount), 2, MidpointRounding.AwayFromZero);
                                NonDiscountTravelPrice = nonDiscountedPrice.Amount;
                            }
                            else
                            {
                                promoValue = 0;
                            }
                        }
                        if (price.Type.Equals("TOTAL", StringComparison.OrdinalIgnoreCase))
                        {
                            var nonDiscountedTotalPrice = prices.Find(dp => dp.PaxTypeCode == price.PaxTypeCode && dp.Type.ToUpper().Equals("NONDISCOUNTPRICE-TOTAL"));
                            var discountedTotalPrice = prices.Find(dp => dp.PaxTypeCode == price.PaxTypeCode && dp.Type.ToUpper().Equals("TOTAL"));
                            if (discountedTotalPrice != null && nonDiscountedTotalPrice != null)
                            {
                                promoValue = Math.Round(Convert.ToDouble(nonDiscountedTotalPrice.Amount)
                                            - Convert.ToDouble(discountedTotalPrice.Amount), 2, MidpointRounding.AwayFromZero);
                            }
                            else
                            {
                                promoValue = 0;
                            }
                        }
                        bookingPrice.PromoDetails = promoValue > 0 ? new MOBPromoCode
                        {
                            PriceTypeDescription = price.Type.Equals("TOTAL", StringComparison.OrdinalIgnoreCase) ? _configuration.GetValue<string>("PromoSavedText") : _configuration.GetValue<string>("PromoCodeAppliedText"),
                            PromoValue = Math.Round(promoValue, 2, MidpointRounding.AwayFromZero),
                            FormattedPromoDisplayValue = "-" + promoValue.ToString("C2", CultureInfo.CurrentCulture)
                        } : null;
                    }
                }

                bookingPrice.CurrencyCode = price.Currency;
                bookingPrice.DisplayType = price.Type;
                bookingPrice.Status = price.Status;
                bookingPrice.Waived = price.Waived;
                bookingPrice.DisplayValue = NonDiscountTravelPrice > 0 ? string.Format("{0:#,0.00}", NonDiscountTravelPrice) : string.Format("{0:#,0.00}", price.Amount);
                if (_configuration.GetValue<bool>("EnableCouponsforBooking") && !string.IsNullOrEmpty(price.PaxTypeCode))
                {
                    bookingPrice.PaxTypeCode = price.PaxTypeCode;
                }
                if (!string.IsNullOrEmpty(searchType))
                {
                    string desc = string.Empty;
                    if (price.Description != null && price.Description.Length > 0)
                    {
                        if (!EnableYADesc(isReshopChange) || price.PricingPaxType == null || !price.PricingPaxType.Equals("UAY"))
                        {
                            desc = ShopStaticUtility.GetFareDescription(price);
                        }
                    }
                    if (EnableYADesc(isReshopChange) && !string.IsNullOrEmpty(price.PricingPaxType) && price.PricingPaxType.ToUpper().Equals("UAY"))
                    {
                        bookingPrice.PriceTypeDescription = ShopStaticUtility.BuildYAPriceTypeDescription(searchType);
                        bookingPrice.PaxTypeDescription = $"{price?.Count} {"young adult (18-23)"}".ToLower();
                    }
                    else
                    if (price.Description.ToUpper().Contains("TOTAL"))
                    {
                        bookingPrice.PriceTypeDescription = price?.Description.ToLower();
                        bookingPrice.PaxTypeDescription = $"{price?.Count} {price.Description}".ToLower();
                    }
                    else
                    {
                        if (_configuration.GetValue<bool>("EnableAwardStrikeThroughPricing") && session.IsAward && session.CatalogItems != null && session.CatalogItems.Count > 0 &&
                            session.CatalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.AwardStrikeThroughPricing).ToString() || a.Id == ((int)AndroidCatalogEnum.AwardStrikeThroughPricing).ToString())?.CurrentValue == "1"
                                && price.StrikeThroughPricing > 0 && (int)price.StrikeThroughPricing != (int)price.Amount
                            )
                        {
                            if (_configuration.GetValue<bool>("EnableStrikeThroughTotalMilesFix"))
                            {
                                bookingPrice.StrikeThroughDisplayValue = ShopStaticUtility.FormatAwardAmountForDisplay((price.StrikeThroughPricing * price.Count).ToString(), false);
                            }
                            else
                            {
                                bookingPrice.StrikeThroughDisplayValue = ShopStaticUtility.FormatAwardAmountForDisplay(price.StrikeThroughPricing.ToString(), false);
                            }
                            bookingPrice.StrikeThroughDescription = BuildStrikeThroughDescription();
                        }
                        bookingPrice.PriceTypeDescription = ShopStaticUtility.BuildPriceTypeDescription(searchType, price.Description, price.Count, desc, isFareLockViewRes, isCorporateFare);

                        if (isEnableOmniCartMVP2Changes)
                        {
                            string description = price?.Description;
                            if (EnableShoppingcartPhase2ChangesWithVersionCheck(appId, appVersion) && !isReshopChange && !string.IsNullOrEmpty(session?.TravelType) && (session.TravelType == TravelType.CB.ToString() || session.TravelType == TravelType.CLB.ToString()))
                            {
                                description = BuildPaxTypeDescription(price?.PaxTypeCode, price?.Description, price.Count);
                            }
                            bookingPrice.PaxTypeDescription = $"{price.Count} {description}".ToLower();
                        }
                    }
                }

                if (!isReshopChange)
                {
                    if (!string.IsNullOrEmpty(bookingPrice.DisplayType) && bookingPrice.DisplayType.Equals("MILES") && isAwardBooking && !string.IsNullOrEmpty(sessionId))
                    {
                        if (IsBuyMilesFeatureEnabled(appId, appVersion, catalogItems, isNotSelectTripCall: true) == true
                               && shopBookingDetailsResponse?.DisplayCart?.IsPurchaseIneligible == true && isRegisterOffersFlow == true)
                        {
                            throw new MOBUnitedException(_configuration.GetValue<string>("BuyMilesPriceChangeError"));
                        }
                        else if (IsBuyMilesFeatureEnabled(appId, appVersion, catalogItems, isNotSelectTripCall) == false)
                        {
                            ValidateAwardMileageBalance(sessionId, price.Amount);
                        }
                    }
                }

                double tempDouble = 0;
                double.TryParse(NonDiscountTravelPrice > 0 ? NonDiscountTravelPrice.ToString() : price.Amount.ToString(), out tempDouble);
                bookingPrice.Value = Math.Round(tempDouble, 2, MidpointRounding.AwayFromZero);

                if (price.Currency.ToUpper() == "MIL")
                {
                    bookingPrice.FormattedDisplayValue = ShopStaticUtility.FormatAwardAmountForDisplay(price.Amount.ToString(), false);
                }
                else
                {
                    bookingPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(NonDiscountTravelPrice > 0 ? NonDiscountTravelPrice : price.Amount, ci, false);
                }
                bookingPrices.Add(bookingPrice);
            }
            if (_configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking"))
            {
                AddGrandTotalIfNotExistInPrices(bookingPrices);
                AddFreeBagDetailsInPrices(displayCart, bookingPrices);
            }
            if (IsBuyMilesFeatureEnabled(appId, appVersion, isNotSelectTripCall: true))
            {
                _shoppingBuyMiles.UpdatePricesForBuyMiles(bookingPrices, shopBookingDetailsResponse, displayFees);
            }
            return bookingPrices;
        }

        public void AddGrandTotalIfNotExistInPrices(List<MOBSHOPPrice> prices)
        {
            var grandTotalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("GRAND TOTAL"));
            if (grandTotalPrice == null)
            {
                var totalPrice = prices.FirstOrDefault(p => p.DisplayType.ToUpper().Equals("TOTAL"));
                grandTotalPrice = ShopStaticUtility.BuildGrandTotalPriceForReservation(totalPrice.Value);
                if (_configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking"))
                {
                    grandTotalPrice.PromoDetails = totalPrice.PromoDetails;
                }
                prices.Add(grandTotalPrice);
            }
        }
        public void AddFreeBagDetailsInPrices(DisplayCart displayCart, List<MOBSHOPPrice> prices)
        {
            if (isAFSCouponApplied(displayCart))
            {
                if (displayCart.SpecialPricingInfo.MerchOfferCoupon.Product.ToUpper().Equals("BAG"))
                {
                    prices.Add(new MOBSHOPPrice
                    {
                        PriceTypeDescription = _configuration.GetValue<string>("FreeBagCouponDescription"),
                        DisplayType = "TRAVELERPRICE",
                        FormattedDisplayValue = "",
                        DisplayValue = "",
                        Value = 0
                    });
                }
            }
        }
        public bool isAFSCouponApplied(DisplayCart displayCart)
        {
            if (displayCart != null && displayCart.SpecialPricingInfo != null && displayCart.SpecialPricingInfo.MerchOfferCoupon != null && !string.IsNullOrEmpty(displayCart.SpecialPricingInfo.MerchOfferCoupon.PromoCode) && displayCart.SpecialPricingInfo.MerchOfferCoupon.IsCouponEligible.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
            return false;
        }
        public async Task ValidateAwardMileageBalance(string sessionId, decimal milesNeeded)
        {
            CSLShopRequest shopRequest = new CSLShopRequest();
            shopRequest = await _sessionHelperService.GetSession<CSLShopRequest>(sessionId, shopRequest.ObjectName, new List<string> { sessionId, shopRequest.ObjectName }).ConfigureAwait(false);
            if (shopRequest != null && shopRequest.ShopRequest != null && shopRequest.ShopRequest.AwardTravel && shopRequest.ShopRequest.LoyaltyPerson != null && shopRequest.ShopRequest.LoyaltyPerson.AccountBalances != null)
            {
                if (shopRequest.ShopRequest.LoyaltyPerson.AccountBalances[0] != null && shopRequest.ShopRequest.LoyaltyPerson.AccountBalances[0].Balance < milesNeeded)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("NoEnoughMilesForAwardBooking"));
                }
            }
        }

        //added-Kriti
        public void GetFlattedFlightsForCOGorThruFlights(Trip trip)
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

        #region Sathwika

        public TripShare IsShareTripValid(SelectTripResponse selectTripResponse)
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

        public bool EnableReshopCubaTravelReasonVersion(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneEnableReshopCubaTravelReasonVersion", "AndroidEnableReshopCubaTravelReasonVersion", "", "", true, _configuration);
        }

        public bool IsETCCombinabilityEnabled(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("CombinebilityETCToggle") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCCombinability_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCCombinability_AppVersion")))
            {
                return true;
            }

            return false;
        }

        public async Task LoadandAddTravelCertificate(MOBShoppingCart shoppingCart, string sessionId, List<MOBSHOPPrice> prices, bool isETCCertificatesExistInShoppingCartPersist, MOBApplication application)
        {
            var persistedTravelCertifcateResponse = new FOPTravelerCertificateResponse();
            if (_configuration.GetValue<bool>("MTETCToggle") && (prices.Exists(price => price.DisplayType.ToUpper().Trim() == "CERTIFICATE") || isETCCertificatesExistInShoppingCartPersist))
            {
                persistedTravelCertifcateResponse = await _sessionHelperService.GetSession<FOPTravelerCertificateResponse>(sessionId, persistedTravelCertifcateResponse.ObjectName, new List<string> { sessionId, persistedTravelCertifcateResponse.ObjectName }).ConfigureAwait(false);
            }
            else
            {
                persistedTravelCertifcateResponse = await _sessionHelperService.GetSession<FOPTravelerCertificateResponse>(sessionId, persistedTravelCertifcateResponse.ObjectName, new List<string> { sessionId, persistedTravelCertifcateResponse.ObjectName }).ConfigureAwait(false);
            }
            if (_configuration.GetValue<bool>("MTETCToggle") && shoppingCart.IsMultipleTravelerEtcFeatureClientToggleEnabled && shoppingCart?.SCTravelers != null && shoppingCart.SCTravelers.Exists(st => !string.IsNullOrEmpty(st.TravelerNameIndex)))
            {
                if (persistedTravelCertifcateResponse?.ShoppingCart?.CertificateTravelers?.Count > 0)
                {
                    shoppingCart.CertificateTravelers = persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers;
                }
                else if (shoppingCart.CertificateTravelers != null)
                {
                    AssignCertificateTravelers(shoppingCart, persistedTravelCertifcateResponse, prices, application);
                }
            }
            if (persistedTravelCertifcateResponse?.ShoppingCart?.FormofPaymentDetails?.TravelCertificate != null)
            {
                var formOfPayment = shoppingCart.FormofPaymentDetails;

                MOBFormofPaymentDetails persistedFOPDetail = persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails;
                formOfPayment.TravelCertificate = persistedFOPDetail.TravelCertificate;
                formOfPayment.BillingAddress = formOfPayment.BillingAddress == null ? persistedFOPDetail.BillingAddress : formOfPayment.BillingAddress;
                formOfPayment.Email = formOfPayment.Email == null ? persistedFOPDetail.Email : formOfPayment.Email;
                formOfPayment.Phone = formOfPayment.Phone == null ? persistedFOPDetail.Phone : formOfPayment.Phone;
                formOfPayment.EmailAddress = formOfPayment.EmailAddress == null ? persistedFOPDetail.EmailAddress : formOfPayment.EmailAddress;
                Reservation bookingPathReservation = await _sessionHelperService.GetSession<Reservation>(sessionId, (new Reservation()).ObjectName, new List<string> { sessionId, (new Reservation()).ObjectName }).ConfigureAwait(false);
                var requestSCRES = shoppingCart.Products.Find(p => p.Code == "RES");
                var persistSCRES = persistedTravelCertifcateResponse.ShoppingCart.Products.Find(p => p.Code == "RES");
                bool isSCRESProductGotRefreshed = true;
                if (requestSCRES != null && persistSCRES != null)
                {
                    isSCRESProductGotRefreshed = (requestSCRES.ProdTotalPrice != persistSCRES.ProdTotalPrice);
                }
                ShopStaticUtility.AddGrandTotalIfNotExistInPricesAndUpdateCertificateValue(bookingPathReservation.Prices, formOfPayment);
                UpdateCertificateAmountInTotalPrices(bookingPathReservation.Prices, shoppingCart.Products, formOfPayment.TravelCertificate.TotalRedeemAmount, isSCRESProductGotRefreshed);
                AssignIsOtherFOPRequired(formOfPayment, bookingPathReservation.Prices, shoppingCart.FormofPaymentDetails?.SecondaryCreditCard != null);
                await _sessionHelperService.SaveSession<Reservation>(bookingPathReservation, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, bookingPathReservation.ObjectName }, bookingPathReservation.ObjectName).ConfigureAwait(false);
                persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails = formOfPayment;
                persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers = shoppingCart.CertificateTravelers;
                if (_configuration.GetValue<bool>("SavedETCToggle"))
                {
                    UpdateSavedCertificate(shoppingCart);
                    persistedTravelCertifcateResponse.ShoppingCart.ProfileTravelerCertificates = shoppingCart.ProfileTravelerCertificates;
                }
                await _sessionHelperService.SaveSession<FOPTravelerCertificateResponse>(persistedTravelCertifcateResponse, sessionId, new List<string> { sessionId, persistedTravelCertifcateResponse.ObjectName }, persistedTravelCertifcateResponse.ObjectName).ConfigureAwait(false);
                await _sessionHelperService.SaveSession<MOBShoppingCart>(shoppingCart, sessionId, new List<string> { sessionId, shoppingCart.ObjectName }, shoppingCart.ObjectName).ConfigureAwait(false);
            }
        }

        public void AssignIsOtherFOPRequired(MOBFormofPaymentDetails formofPaymentDetails, List<MOBSHOPPrice> prices, bool IsSecondaryFOP = false, bool isRemoveAll = false)
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

        public void UpdateCertificateAmountInTotalPrices(List<MOBSHOPPrice> prices, List<ProdDetail> scProducts, double certificateTotalAmount, bool isShoppingCartProductsGotRefresh = false)
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
                    ShopStaticUtility.UpdateCertificateRedeemAmountInSCProductPrices(scRESProduct, certificatePrice.Value, false);
                }
                ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificatePrice.Value, false);
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
            ShopStaticUtility.UpdateCertificateRedeemAmountInSCProductPrices(scRESProduct, certificateTotalAmount);
            ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificateTotalAmount);
        }

        public MOBSHOPPrice UpdateCertificatePrice(MOBSHOPPrice certificatePrice, double totalAmount)
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

        public bool IsMilesFOPEnabled()
        {
            Boolean isMilesFOP;
            Boolean.TryParse(_configuration.GetValue<string>("EnableMilesAsPayment"), out isMilesFOP);
            return isMilesFOP;
        }

        public void AssignCertificateTravelers(MOBShoppingCart shoppingCart, FOPTravelerCertificateResponse persistedTravelCertifcateResponse, List<MOBSHOPPrice> prices, MOBApplication application)
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
                    ShopStaticUtility.AddAllTravelersOptionInCertificateTravelerList(shoppingCart);
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
                    ShopStaticUtility.ClearUnmatchedCertificatesAfterEditTravelers(shoppingCart, persistedTravelCertifcateResponse, prices);
                }
            }
        }

        public InfoWarningMessages GetIBELiteNonCombinableMessage()
        {
            var message = _configuration.GetValue<string>("IBELiteNonCombinableMessage");
            return ShopStaticUtility.BuildInfoWarningMessages(message);
        }

        public bool IncludeReshopFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableReshopFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }

        public WorkFlowType GetWorkFlowType(string flow, string productCode = "")
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

        public bool IsPOMOffer(string productCode)
        {
            if (!_configuration.GetValue<bool>("EnableInflightMealsRefreshment")) return false;
            if (string.IsNullOrEmpty(productCode)) return false;
            return (productCode == _configuration.GetValue<string>("InflightMealProductCode"));
        }

        public bool EnableReshopMixedPTC(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidVersion_ReshopEnableMixedPTC", "iphoneVersion_ReshopEnableMixedPTC", "", "", true, _configuration);
        }

        public bool IncludeFFCResidual(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableFFCResidual")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidFFCResidualVersion", "iPhoneFFCResidualVersion", "", "", true, _configuration);
        }

        private void AssignCertificateTravelers(MOBShoppingCart shoppingCart)
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

        public bool IncludeMoneyPlusMiles(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableMilesPlusMoney")
                && GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidMilesPlusMoneyVersion", "iPhoneMilesPlusMoneyVersion", "", "", true, _configuration);
        }

        public async Task LoadandAddTravelCertificate(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation, bool isETCCertificatesExistInShoppingCartPersist)
        {
            var persistedTravelCertifcateResponse = new FOPTravelerCertificateResponse();

            if (_configuration.GetValue<bool>("CombinebilityETCToggle") && (reservation.Prices.Exists(price => price.DisplayType.ToUpper().Trim() == "CERTIFICATE") || isETCCertificatesExistInShoppingCartPersist))
            {
                persistedTravelCertifcateResponse = await _sessionHelperService.GetSession<FOPTravelerCertificateResponse>(reservation.SessionId, persistedTravelCertifcateResponse.ObjectName, new List<string> { reservation.SessionId, persistedTravelCertifcateResponse.ObjectName }).ConfigureAwait(false);
            }

            if (persistedTravelCertifcateResponse?.ShoppingCart?.CertificateTravelers?.Count > 0)
            {
                shoppingCart.CertificateTravelers = persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers;
            }
            else if (shoppingCart.CertificateTravelers != null)
            {
                AssignCertificateTravelers(shoppingCart);
            }

            await AssignTravelerCertificateToFOP(persistedTravelCertifcateResponse, shoppingCart.Products, shoppingCart.Flow);
            var formOfPayment = shoppingCart.FormofPaymentDetails;

            MOBFormofPaymentDetails persistedFOPDetail = persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails;
            formOfPayment.TravelCertificate = persistedFOPDetail.TravelCertificate;
            formOfPayment.BillingAddress = formOfPayment.BillingAddress == null ? persistedFOPDetail.BillingAddress : formOfPayment.BillingAddress;
            formOfPayment.Email = formOfPayment.Email == null ? persistedFOPDetail.Email : formOfPayment.Email;
            formOfPayment.Phone = formOfPayment.Phone == null ? persistedFOPDetail.Phone : formOfPayment.Phone;
            formOfPayment.EmailAddress = formOfPayment.EmailAddress == null ? persistedFOPDetail.EmailAddress : formOfPayment.EmailAddress;

            //Add requested certificaates to TravelerCertificate object in FOP
            formOfPayment.TravelCertificate.AllowedETCAmount = GetAlowedETCAmount(shoppingCart.Products, shoppingCart.Flow);
            formOfPayment?.TravelCertificate?.Certificates?.ForEach(c => c.RedeemAmount = 0);
            ShopStaticUtility.AddRequestedCertificatesToFOPTravelerCertificates(formOfPayment.TravelCertificate.Certificates, shoppingCart.ProfileTravelerCertificates, formOfPayment.TravelCertificate);
            Reservation bookingPathReservation = await _sessionHelperService.GetSession<Reservation>(reservation.SessionId, new Reservation().ObjectName, new List<string> { reservation.SessionId, new Reservation().ObjectName }).ConfigureAwait(false);
            ShopStaticUtility.AddGrandTotalIfNotExistInPricesAndUpdateCertificateValue(bookingPathReservation.Prices, formOfPayment);
            UpdateCertificateAmountInTotalPrices(bookingPathReservation.Prices, formOfPayment.TravelCertificate.TotalRedeemAmount);
            AssignIsOtherFOPRequired(formOfPayment, bookingPathReservation.Prices, shoppingCart.FormofPaymentDetails?.SecondaryCreditCard != null);
            await _sessionHelperService.SaveSession<Reservation>(bookingPathReservation, reservation.SessionId, new List<string> { reservation.SessionId, new Reservation().ObjectName }, new Reservation().ObjectName).ConfigureAwait(false);
            reservation.Prices = bookingPathReservation.Prices;
            persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails = formOfPayment;
            persistedTravelCertifcateResponse.ShoppingCart.CertificateTravelers = shoppingCart.CertificateTravelers;
            persistedTravelCertifcateResponse.ShoppingCart.FormofPaymentDetails.TravelCertificate.ReviewETCMessages = await UpdateReviewETCAlertmessages(shoppingCart);
            UpdateSavedCertificate(shoppingCart);
            persistedTravelCertifcateResponse.ShoppingCart.ProfileTravelerCertificates = shoppingCart.ProfileTravelerCertificates;
            await _sessionHelperService.SaveSession<FOPTravelerCertificateResponse>(persistedTravelCertifcateResponse, reservation.SessionId, new List<string> { reservation.SessionId, persistedTravelCertifcateResponse.ObjectName }, persistedTravelCertifcateResponse.ObjectName).ConfigureAwait(false);
            await _sessionHelperService.SaveSession<MOBShoppingCart>(shoppingCart, reservation.SessionId, new List<string> { reservation.SessionId, shoppingCart.ObjectName }, shoppingCart.ObjectName).ConfigureAwait(false);
        }

        public double GetAlowedETCAmount(List<ProdDetail> products, string flow)
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


        public bool IsCorporateLeisureFareSelected(List<MOBSHOPTrip> trips)
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

        public void UpdateCertificateAmountInTotalPrices(List<MOBSHOPPrice> prices, double certificateTotalAmount)
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
                ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificatePrice.Value, false);
                UpdateCertificatePrice(certificatePrice, certificateTotalAmount);
            }

            if (certificateTotalAmount == 0 && certificatePrice != null)
            {
                prices.Remove(certificatePrice);
            }

            ShopStaticUtility.UpdateCertificateRedeemAmountFromTotalInReserationPrices(grandtotal, certificateTotalAmount);
        }

        private async Task<List<MOBMobileCMSContentMessages>> UpdateReviewETCAlertmessages(MOBShoppingCart shoppingCart)
        {
            List<MOBMobileCMSContentMessages> alertMessages = new List<MOBMobileCMSContentMessages>();
            alertMessages = await AssignAlertMessages("TravelCertificate_Combinability_ReviewETCAlertMsg");
            //Show other fop required message only when isOtherFop is required
            if (shoppingCart?.FormofPaymentDetails?.IsOtherFOPRequired == false)
            {
                alertMessages.Remove(alertMessages.Find(x => x.HeadLine == "TravelCertificate_Combinability_ReviewETCAlertMsgs_OtherFopRequiredMessage"));
            }
            //Update the total price
            if (shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates != null &&
                shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates.Count > 0
                )
            {
                double balanceETCAmount = shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Sum(x => x.NewValueAfterRedeem);
                if (balanceETCAmount > 0 && shoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count > 1)
                {
                    alertMessages.Find(x => x.HeadLine == "TravelCertificate_Combinability_ReviewETCAlertMsgs_ETCBalanceAttentionmessage").ContentFull = string.Format(alertMessages.Find(x => x.HeadLine == "TravelCertificate_Combinability_ReviewETCAlertMsgs_ETCBalanceAttentionmessage").ContentFull, String.Format("{0:0.00}", balanceETCAmount));
                }
                else
                {
                    alertMessages.Remove(alertMessages.Find(x => x.HeadLine == "TravelCertificate_Combinability_ReviewETCAlertMsgs_ETCBalanceAttentionmessage"));
                }
            }
            return alertMessages;
        }

        private async Task AssignTravelerCertificateToFOP(FOPTravelerCertificateResponse persistedTravelCertifcateResponse, List<ProdDetail> products, string flow)
        {
            if (persistedTravelCertifcateResponse == null)
            {
                persistedTravelCertifcateResponse = new FOPTravelerCertificateResponse();
            }
            persistedTravelCertifcateResponse.ShoppingCart = await InitialiseShoppingCartAndDevfaultValuesForETC(persistedTravelCertifcateResponse.ShoppingCart, products, flow);
        }

        public async Task<MOBShoppingCart> InitialiseShoppingCartAndDevfaultValuesForETC(MOBShoppingCart shoppingcart, List<ProdDetail> products, string flow)
        {
            if (shoppingcart == null)
            {
                shoppingcart = new MOBShoppingCart();
            }
            if (shoppingcart.FormofPaymentDetails == null)
            {
                shoppingcart.FormofPaymentDetails = new MOBFormofPaymentDetails();
            }
            if (shoppingcart.FormofPaymentDetails.TravelCertificate == null)
            {
                shoppingcart.FormofPaymentDetails.TravelCertificate = new MOBFOPTravelCertificate();
                shoppingcart.FormofPaymentDetails.TravelCertificate.AllowedETCAmount = GetAlowedETCAmount(shoppingcart.Products ?? products, (string.IsNullOrEmpty(shoppingcart.Flow) ? flow : shoppingcart.Flow));
                shoppingcart.FormofPaymentDetails.TravelCertificate.NotAllowedETCAmount = GetNotAlowedETCAmount(products, (string.IsNullOrEmpty(shoppingcart.Flow) ? flow : shoppingcart.Flow));
                shoppingcart.FormofPaymentDetails.TravelCertificate.MaxAmountOfETCsAllowed = Convert.ToDouble(_configuration.GetValue<string>("CombinebilityMaxAmountOfETCsAllowed"));
                shoppingcart.FormofPaymentDetails.TravelCertificate.MaxNumberOfETCsAllowed = Convert.ToInt32(_configuration.GetValue<string>("CombinebilityMaxNumberOfETCsAllowed"));
                shoppingcart.FormofPaymentDetails.TravelCertificate.ReviewETCMessages = await AssignAlertMessages("TravelCertificate_Combinability_ReviewETCAlertMsg");
                shoppingcart.FormofPaymentDetails.TravelCertificate.SavedETCMessages = await AssignAlertMessages("TravelCertificate_Combinability_SavedETCListAlertMsg");
                string removeAllCertificatesAlertMessage = _configuration.GetValue<string>("RemoveAllTravelCertificatesAlertMessage");
                shoppingcart.FormofPaymentDetails.TravelCertificate.RemoveAllCertificateAlertMessage = new Section { Text1 = removeAllCertificatesAlertMessage, Text2 = "Cancel", Text3 = "Continue" };
            }
            return shoppingcart;
        }

        private double GetNotAlowedETCAmount(List<ProdDetail> products, string flow)
        {
            return products.Sum(a => Convert.ToDouble(a.ProdTotalPrice)) - GetAlowedETCAmount(products, flow);
        }

        public double GetBundlesAmount(List<ProdDetail> products, string flow)
        {
            string nonBundleProductCode = _configuration.GetValue<string>("NonBundleProductCode");
            double bundleAmount = products == null ? 0 : products.Where(p => (nonBundleProductCode.IndexOf(p.Code) == -1) && !string.IsNullOrEmpty(p.ProdTotalPrice)).Sum(a => Convert.ToDouble(a.ProdTotalPrice));
            return bundleAmount;
        }

        private async Task<List<MOBMobileCMSContentMessages>> AssignAlertMessages(string captionKey)
        {
            List<MOBMobileCMSContentMessages> tncs = null;
            var docs = await GetCaptions(captionKey, true);
            if (docs != null && docs.Any())
            {
                tncs = new List<MOBMobileCMSContentMessages>();
                foreach (var doc in docs)
                {
                    var tnc = new MOBMobileCMSContentMessages
                    {
                        ContentFull = doc.CurrentValue,
                        HeadLine = doc.Id
                    };
                    tncs.Add(tnc);
                }
            }
            return tncs;
        }

        private async Task<List<MOBItem>> GetCaptions(string key)
        {
            return !string.IsNullOrEmpty(key) ? await GetCaptions(key, true) : null;
        }

        private async Task<List<MOBItem>> GetCaptions(string keyList, bool isTnC)
        {

            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(keyList, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
            if (docs == null || !docs.Any()) return null;

            var captions = new List<MOBItem>();

            captions.AddRange(
                docs.Select(doc => new MOBItem
                {
                    Id = doc.Title,
                    CurrentValue = doc.LegalDocument
                }));
            return captions;
        }

        public List<FormofPaymentOption> BuildEligibleFormofPaymentsResponse(List<FormofPaymentOption> response, MOBShoppingCart shoppingCart, MOBRequest request)
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

        public List<FormofPaymentOption> BuildEligibleFormofPaymentsResponse(List<FormofPaymentOption> response, MOBShoppingCart shoppingCart, Session session, MOBRequest request, bool isMetaSearch = false)
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
                            if (ShopStaticUtility.IsCertificatesAppliedforAllIndividualTravelers(shoppingCart))
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

        public bool IsETCEnabledforMultiTraveler(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("MTETCToggle") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCForMultiTraveler_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCForMultiTraveler_AppVersion")))
            {
                return true;
            }
            return false;
        }

        public bool IsETCEligibleTravelType(Session session, string travelTypeConfigKey = "ETCEligibleTravelTypes")
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

        public async Task AssignBalanceAttentionInfoWarningMessage(ReservationInfo2 shopReservationInfo2, MOBFOPTravelCertificate travelCertificate)
        {
            if (shopReservationInfo2 == null)
            {
                shopReservationInfo2 = new ReservationInfo2();
            }
            //To show balance attention message on RTI when Combinability is ON from Shoppingcart service  and OFF from MRest
            if (shopReservationInfo2.InfoWarningMessages == null)
            {
                shopReservationInfo2.InfoWarningMessages = new List<InfoWarningMessages>();
            }
            InfoWarningMessages balanceAttentionMessage = new InfoWarningMessages();
            balanceAttentionMessage = await GetETCBalanceAttentionInfoWarningMessage(travelCertificate);
            if (shopReservationInfo2.InfoWarningMessages.Exists(x => x.Order == MOBINFOWARNINGMESSAGEORDER.RTIETCBALANCEATTENTION.ToString()))
            {
                shopReservationInfo2.InfoWarningMessages.Remove(shopReservationInfo2.InfoWarningMessages.Find(x => x.Order == MOBINFOWARNINGMESSAGEORDER.RTIETCBALANCEATTENTION.ToString()));
            }
            if (balanceAttentionMessage != null)
            {
                shopReservationInfo2.InfoWarningMessages.Add(balanceAttentionMessage);
                shopReservationInfo2.InfoWarningMessages = shopReservationInfo2.InfoWarningMessages.OrderBy(c => (int)((MOBINFOWARNINGMESSAGEORDER)Enum.Parse(typeof(MOBINFOWARNINGMESSAGEORDER), c.Order))).ToList();
            }
        }

        private async Task<InfoWarningMessages> GetETCBalanceAttentionInfoWarningMessage(MOBFOPTravelCertificate travelCertificate)
        {
            InfoWarningMessages infoMessage = null;
            double? etcBalanceAttentionAmount = travelCertificate?.Certificates?.Sum(c => c.NewValueAfterRedeem);
            if (etcBalanceAttentionAmount > 0 && travelCertificate?.Certificates?.Count > 1)
            {
                List<MOBMobileCMSContentMessages> alertMessages = new List<MOBMobileCMSContentMessages>();
                alertMessages = await AssignAlertMessages("TravelCertificate_Combinability_ReviewETCAlertMsg");
                infoMessage = new InfoWarningMessages();
                infoMessage.Order = MOBINFOWARNINGMESSAGEORDER.RTIETCBALANCEATTENTION.ToString();
                infoMessage.IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString();
                infoMessage.Messages = new List<string>();
                infoMessage.Messages.Add(string.Format(alertMessages.Find(x => x.HeadLine == "TravelCertificate_Combinability_ReviewETCAlertMsgs_ETCBalanceAttentionmessage").ContentFull, String.Format("{0:0.00}", etcBalanceAttentionAmount)));
            }

            return infoMessage;
        }

        public Collection<FOPProduct> GetProductsForEligibleFopRequest(MOBShoppingCart shoppingCart, SeatChangeState state = null)
        {
            if (shoppingCart == null || shoppingCart.Products == null || !shoppingCart.Products.Any())
                return null;

            var products = shoppingCart.Products.GroupBy(k => new { k.Code, k.ProdDescription }).Select(x => new FOPProduct { Code = x.Key.Code, ProductDescription = x.Key.ProdDescription }).ToCollection();
            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes")))
            {
                if (!_configuration.GetValue<bool>("ByPassAddingPCUProductToEligibleFopRequest"))
                {
                    ShopStaticUtility.AddPCUToRequestWhenPCUSeatIsSelected(state, ref products);
                }
            }

            return products;
        }


        public bool IsEligibileForUplift(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart)
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

        public bool HasEligibleProductsForUplift(string totalPrice, List<ProdDetail> products)
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

        public int MinimumPriceForUplift
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

        public int MaxmimumPriceForUplift
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


        public bool IncludeMOBILE12570ResidualFix(int appId, string appVersion)
        {
            bool isApplicationGreater = GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidMOBILE12570ResidualVersion", "iPhoneMOBILE12570ResidualVersion", "", "", true, _configuration);
            return (_configuration.GetValue<bool>("eableMOBILE12570Toggle") && isApplicationGreater);
        }


        public bool IsManageResETCEnabled(int applicationId, string appVersion)
        {
            if (_configuration.GetValue<bool>("EnableEtcforSeats_PCU_Viewres") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableETCManageRes_AppVersion"), _configuration.GetValue<string>("iPhone_EnableETCManageRes_AppVersion")))
            {
                return true;
            }
            return false;
        }

        public void UpdateSavedCertificate(MOBShoppingCart shoppingcart)
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
        //public  List<MOBItem> GetCaptions(string key)
        //{
        //    //return !string.IsNullOrEmpty(key) ? GetCaptions(new List<string> { key }, true) : null;
        //}

        //public  List<MOBItem> GetCaptions(List<string> keyList, bool isTnC)
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

        #endregion
        public string BuilTripShareEmailBodyTripText(string tripType, List<MOBSHOPTrip> trips, bool isHtml)
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
        public void AddPromoDetailsInSegments(ProdDetail prodDetail)
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
        public string BuildTripSharePrice(string priceWithCurrency, string currencyCode, string redirectUrl)
        {
            string emailBodyBodyPriceText = _configuration.GetValue<string>("ShareTripInSoftRTIEmailBodyPriceText");
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{serverCurrentDateTime}", DateTime.Now.ToString("MMM d 'at' h:mm tt"));
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{priceWithCurrency}", priceWithCurrency);
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{currencyCode}", currencyCode);
            emailBodyBodyPriceText = emailBodyBodyPriceText.Replace("{redirectUrl}", redirectUrl);
            return emailBodyBodyPriceText;
        }
        public string BuildTripShareSegmentText(MOBSHOPTrip trip)
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


        public async Task<List<string>> GetProductDetailDescrption(IGrouping<String, SubItem> subItem, string productCode, String sessionId, bool isBundleProduct)
        {
            List<string> prodDetailDescription = new List<string>();
            if (string.Equals(productCode, "EFS", StringComparison.OrdinalIgnoreCase))
            {
                prodDetailDescription.Add("Included with your fare");
            }

            if (isBundleProduct && !string.IsNullOrEmpty(sessionId))
            {
                var bundleResponse = new MOBBookingBundlesResponse(_configuration);
                bundleResponse = await _sessionHelperService.GetSession<MOBBookingBundlesResponse>(sessionId, bundleResponse.ObjectName, new List<string> { sessionId, bundleResponse.ObjectName }).ConfigureAwait(false);
                if (bundleResponse != null)
                {
                    var selectedBundleResponse = bundleResponse.Products?.FirstOrDefault(p => string.Equals(p.ProductCode, productCode, StringComparison.OrdinalIgnoreCase));
                    if (selectedBundleResponse != null)
                    {
                        prodDetailDescription.AddRange(selectedBundleResponse.Tile.OfferDescription);
                    }
                }
            }
            return prodDetailDescription;
        }


        public void AddCouponDetails(List<ProdDetail> prodDetails, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse cslFlightReservationResponse, bool isPost, string flow, MOBApplication application)
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
                     || (flow == FlowType.BOOKING.ToString() && prodDetail.CouponDetails != null && prodDetail.CouponDetails.Count > 0 && IsEnableOmniCartMVP2Changes(application.Id, application.Version.Major, true)) || (_configuration.GetValue<bool>("IsEnableManageResCoupon") && (flow == FlowType.VIEWRES.ToString() || flow == FlowType.VIEWRES_SEATMAP.ToString()) && prodDetail.CouponDetails != null))
                {
                    AddPromoDetailsInSegments(prodDetail);
                }
            }
        }

        public bool IsOriginalPriceExists(ProdDetail prodDetail)
        {
            return !_configuration.GetValue<bool>("DisableFreeCouponFix")
                   && !string.IsNullOrEmpty(prodDetail.ProdOriginalPrice)
                   && Decimal.TryParse(prodDetail.ProdOriginalPrice, out decimal originalPrice)
                   && originalPrice > 0;
        }

        public string BuildProductDescription(Collection<Services.FlightShopping.Common.DisplayCart.TravelOption> travelOptions, IGrouping<string, SubItem> t, string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return string.Empty;

            productCode = productCode.ToUpper().Trim();

            if (productCode == "AAC")
                return "Award Accelerator®";

            if (productCode == "PAC")
                return "Premier Accelerator℠";

            if (productCode == "TPI" && _configuration.GetValue<bool>("GetTPIProductName_HardCode"))
                return "Trip insurance";
            if (productCode == "FARELOCK")
                return "FareLock";

            if (_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes") && productCode == "BEB")
                return !_configuration.GetValue<bool>("EnableNewBEBContentChange") ? "Switch to Economy" : _configuration.GetValue<string>("BEBuyOutPaymentInformationMessage");

            if (productCode == "PCU")
                return GetFormattedCabinName(t.Select(u => u.Description).FirstOrDefault().ToString());


            return travelOptions.Where(d => d.Key == productCode).Select(d => d.Type).FirstOrDefault().ToString();
        }

        public string GetFormattedCabinName(string cabinName)
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

        public string BuildSegmentInfo(string productCode, Collection<ReservationFlightSegment> flightSegments, IGrouping<string, SubItem> x)
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

        public async Task<List<ProdDetail>> BuildProductDetailsForInflightMeals(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, string productCode, string sessionId, bool isPost)
        {
            List<MOBInFlightMealsRefreshmentsResponse> savedResponse =
           await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(sessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { sessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false);
            United.Service.Presentation.InteractionModel.ShoppingCart flightReservationResponseShoppingCart;
            if (isPost)
                flightReservationResponseShoppingCart = flightReservationResponse.CheckoutResponse.ShoppingCart;
            else
                flightReservationResponseShoppingCart = flightReservationResponse.ShoppingCart;


            var displayTotalPrice = flightReservationResponse.DisplayCart.DisplayPrices.FirstOrDefault(o => (o.Description != null && o.Description.Equals("Total", StringComparison.OrdinalIgnoreCase))).Amount;
            var grandTotal = flightReservationResponseShoppingCart?.Items.SelectMany(p => p.Product).Where(d => d.Code == "POM")?.Select(p => p.Price?.Totals?.FirstOrDefault().Amount).FirstOrDefault();

            var travelOptions = ShopStaticUtility.GetTravelOptionItems(flightReservationResponse, productCode);
            // For RegisterOffer uppercabin when there is no price no need to build the product
            List<ProdDetail> response = new List<ProdDetail>();
            if (grandTotal > 0 && productCode == _configuration.GetValue<string>("InflightMealProductCode"))
            {
                var productDetail = new ProdDetail()
                {
                    Code = travelOptions.Where(d => d.Key == productCode).Select(d => d.Key).FirstOrDefault().ToString(),
                    ProdDescription = travelOptions.Where(d => d.Key == productCode).Select(d => d.Type).FirstOrDefault().ToString(),
                    ProdTotalPrice = String.Format("{0:0.00}", grandTotal),
                    ProdDisplayTotalPrice = grandTotal?.ToString("c"),
                    Segments = GetProductSegmentForInFlightMeals(flightReservationResponse, savedResponse, travelOptions, flightReservationResponseShoppingCart),
                };
                response.Add(productDetail);
                return response;
            }
            else return response;

        }

        public List<ProductSegmentDetail> GetProductSegmentForInFlightMeals(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse,
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
                    segmentDetail.SegmentInfo = ShopStaticUtility.GetSegmentDescription(travelOptions);
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
                            bool editExtraDataInCheckoutScreenFix = tupleSelectedItem != null && _configuration.GetValue<bool>("EnableisEditablePOMFeature") && (subProduct.Extension.MealCatalog?.MealShortDescription != null);

                            if (tupleSelectedItem != null)
                            {
                                if (_configuration.GetValue<bool>("EnableisEditablePOMFeature"))
                                {
                                    if (price > 0 && subProduct.Extension.MealCatalog?.MealShortDescription != null)
                                    {
                                        if (prodCounter == 0 && travelerCouter == 0)
                                        {
                                            segDetail.Passenger = traveler[i].Person.GivenName.ToLower().ToPascalCase() + " " + traveler[i].Person.Surname.ToLower().ToPascalCase();
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
                                        segDetail.Passenger = traveler[i].Person.GivenName.ToLower().ToPascalCase() + " " + traveler[i].Person.Surname.ToLower().ToPascalCase();
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

        public ProdDetail BuildProdDetailsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, SeatChangeState state, bool isPost)
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
                        prod.ProdDisplayTotalMiles = ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false);
                    }
                    else
                    {
                        prod.ProdTotalMiles = 0;
                        prod.ProdDisplayTotalMiles = string.Empty;
                    }
                }
                if (_configuration.GetValue<bool>("IsEnableManageResCoupon") && isAFSCouponApplied(flightReservationResponse.DisplayCart))
                    prod.Segments.Select(x => x.SubSegmentDetails).ToList().ForEach(item => item.RemoveAll(k => Decimal.Parse(k.Price) == 0 && (string.IsNullOrEmpty(k.OrginalPrice) || Decimal.Parse(k.OrginalPrice) == 0)));
                else
                    prod.Segments.Select(x => x.SubSegmentDetails).ToList().ForEach(item => item.RemoveAll(k => Decimal.Parse(k.Price) == 0 && (k.StrikeOffPrice == string.Empty || Decimal.Parse(k.StrikeOffPrice) == 0)));

                prod.Segments.RemoveAll(k => k.SubSegmentDetails.Count == 0);
            }
            ShopStaticUtility.UpdateRefundTotal(prod);
            return prod;
        }

        public List<ProductSegmentDetail> BuildProductSegmentsForSeats(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, List<Seat> seats, List<MOBBKTraveler> BookingTravelerInfo, bool isPost)
        {
            if (flightReservationResponse.DisplayCart.DisplaySeats == null || !flightReservationResponse.DisplayCart.DisplaySeats.Any())
                return null;

            var displaySeats = flightReservationResponse.DisplayCart.DisplaySeats.Clone();
            List<string> refundedSegmentNums = null;
            if (flightReservationResponse.Errors != null && flightReservationResponse.Errors.Any(e => e != null && e.MinorCode == "90506"))
            {
                bool DisableFixForPCUPurchaseFailMsg_MOBILE15837 = _configuration.GetValue<bool>("DisableFixForPCUPurchaseFailMsg_MOBILE15837");
                var isRefundSuccess = ShopStaticUtility.IsRefundSuccess(flightReservationResponse.CheckoutResponse.ShoppingCartResponse.Items, out refundedSegmentNums, DisableFixForPCUPurchaseFailMsg_MOBILE15837);
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
                                    SegmentInfo = ShopStaticUtility.GetSegmentInfo(flightReservationResponse, d.Key.OriginalSegmentIndex, Convert.ToInt32(d.Key.LegIndex)),
                                    SubSegmentDetails = d.GroupBy(s => ShopStaticUtility.GetSeatTypeForDisplay(s, flightReservationResponse.DisplayCart.TravelOptions))
                                                        .Select(seatGroup => new ProductSubSegmentDetail
                                                        {
                                                            Price = String.Format("{0:0.00}", seatGroup.Select(s => s.SeatPrice).ToList().Sum()),
                                                            OrginalPrice = _configuration.GetValue<bool>("IsEnableManageResCoupon") ? String.Format("{0:0.00}", seatGroup.Select(s => s.OriginalPrice).ToList().Sum()) : string.Empty,
                                                            DisplayPrice = Decimal.Parse(seatGroup.Select(s => s.SeatPrice).ToList().Sum().ToString()).ToString("c"),
                                                            DisplayOriginalPrice = _configuration.GetValue<bool>("IsEnableManageResCoupon") ? Decimal.Parse(seatGroup.Select(s => s.OriginalPrice).ToList().Sum().ToString()).ToString("c") : string.Empty,
                                                            StrikeOffPrice = ShopStaticUtility.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats, BookingTravelerInfo),
                                                            DisplayStrikeOffPrice = ShopStaticUtility.GetFormatedDisplayPriceForSeats(ShopStaticUtility.GetOriginalTotalSeatPriceForStrikeOff(seatGroup.ToList(), seats, BookingTravelerInfo)),
                                                            Passenger = seatGroup.Count().ToString() + (seatGroup.Count() > 1 ? " Travelers" : " Traveler"),
                                                            SeatCode = seatGroup.Key,
                                                            FlightNumber = seatGroup.Select(x => x.FlightNumber).FirstOrDefault(),
                                                            SegmentDescription = GetSeatTypeBasedonCode(seatGroup.Key, seatGroup.Count()),
                                                            IsPurchaseFailure = ShopStaticUtility.IsPurchaseFailed(seatGroup.Any(s => s.PCUSeat), d.Key.OriginalSegmentIndex.ToString(), refundedSegmentNums),
                                                            Miles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? 0 : _configuration.GetValue<int>("milesFOP") : 0,
                                                            DisplayMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? string.Empty : ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false) : string.Empty,
                                                            StrikeOffMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? 0 : Convert.ToInt32(_configuration.GetValue<string>("milesFOP")) : 0,
                                                            DisplayStrikeOffMiles = IsMilesFOPEnabled() ? seatGroup.Any(s => s.PCUSeat == true) ? string.Empty : ShopStaticUtility.FormatAwardAmountForDisplay(_configuration.GetValue<string>("milesFOP"), false) : string.Empty
                                                        }).ToList().OrderBy(p => ShopStaticUtility.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
                                }).ToList();
        }

        public string GetSeatTypeBasedonCode(string seatCode, int travelerCount)
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


        public List<SeatAssignment> HandleCSLDefect(Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, List<SeatAssignment> fliterSeats, bool isPost)
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
                        return fliterSeats.Where(s => !ShopStaticUtility.IsFailedSeat(s, unAssignedSeats)).ToList();
                    }
                }
            }
            return fliterSeats;
        }

        public bool CheckSeatAssignMessage(string seatAssignMessage, bool isPost)
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

        public List<ProductSegmentDetail> BuildCheckinSegmentDetail(IEnumerable<IGrouping<string, SeatAssignment>> seatAssignmentGroup)
        {
            List<ProductSegmentDetail> segmentDetails = new List<ProductSegmentDetail>();
            seatAssignmentGroup.ForEach(seatSegment => segmentDetails.Add(new ProductSegmentDetail()
            {
                SegmentInfo = seatSegment.Key,
                SubSegmentDetails = BuildSubsegmentDetails(seatSegment.ToList()).OrderBy(p => ShopStaticUtility.GetSeatPriceOrder()[p.SegmentDescription]).ToList()
            }));
            return segmentDetails;
        }

        public List<ProductSubSegmentDetail> BuildSubsegmentDetails(List<SeatAssignment> seatAssignments)
        {
            List<ProductSubSegmentDetail> subSegmentDetails = new List<ProductSubSegmentDetail>();
            var groupedByTypeAndPrice = seatAssignments.GroupBy(s => s.SeatType, (key, grpSeats) => new { SeatType = key, OriginalPrice = grpSeats.Sum(x => x.OriginalPrice), SeatPrice = grpSeats.Sum(x => x.SeatPrice), Count = grpSeats.Count() });

            groupedByTypeAndPrice.ForEach(grpSeats =>
            {
                subSegmentDetails.Add(PopulateSubsegmentDetails(grpSeats.SeatType, grpSeats.OriginalPrice, grpSeats.SeatPrice, grpSeats.Count));
            });
            return subSegmentDetails;
        }

        public ProductSubSegmentDetail PopulateSubsegmentDetails(string seatType, decimal originalPrice, decimal seatPrice, int count)
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
        public string GetSeatTypeBasedonCode(string seatCode, int travelerCount, bool isCheckinPath = false)
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


        public double GetGrandTotalPriceForShoppingCart(bool isCompleteFarelockPurchase, Services.FlightShopping.Common.FlightReservation.FlightReservationResponse flightReservationResponse, bool isPost, string flow = "VIEWRES")
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
                    closeBookingFee = ShopStaticUtility.GetCloseBookingFee(isPost, flightReservationResponseShoppingCart, flow);

                }

                switch (flow)
                {
                    case "BOOKING":
                    case "RESHOP":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !ShopStaticUtility.CheckFailedShoppingCartItem(flightReservationResponse, x)).SelectMany(x => x.Product).Where(x => x.Price != null).SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() /*|| x.Name.ToUpper() == "Close-In Booking Fee".ToUpper()*/ : true)).Select(x => x.Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Price != null).SelectMany(x => x.Price.Totals).Where(x => (x.Name != null ? x.Name.ToUpper() == "GrandTotalForCurrency".ToUpper() /*|| x.Name.ToUpper() == "Close-In Booking Fee".ToUpper()*/ : true)).Select(x => x.Amount).ToList().Sum();
                        break;

                    case "POSTBOOKING":
                        //productCodes = flightReservationResponseShoppingCart.Items.SelectMany(x => x.Product).Where(x => x.Characteristics != null && (x.Characteristics.Any(y => y.Description.ToUpper() == "POSTPURCHASE" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Code).ToList();
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !ShopStaticUtility.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false) && x.Product.FirstOrDefault().Characteristics != null && (x.Product.FirstOrDefault().Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.Where(x => (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false) && x.Product.FirstOrDefault().Characteristics != null && (x.Product.FirstOrDefault().Characteristics.Any(y => y.Description == "PostPurchase" && Convert.ToBoolean(y.Value) == true))).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum(); ;
                        break;

                    case "FARELOCK":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !ShopStaticUtility.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => x.Product.FirstOrDefault().Code == "FLK" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
                            : flightReservationResponse.ShoppingCart.Items.Where(x => x.Product.FirstOrDefault().Code == "FLK" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum();
                        break;

                    case "VIEWRES":
                    case "CHECKIN":
                        shoppingCartTotalPrice = isPost ? flightReservationResponse.CheckoutResponse.ShoppingCart.Items.Where(x => !ShopStaticUtility.CheckFailedShoppingCartItem(flightReservationResponse, x)).Where(x => x.Product.FirstOrDefault().Code != "RES" && (x.Product.FirstOrDefault().Price != null ? (x.Product.FirstOrDefault().Price.Totals.Any()) : false)).Select(x => x.Product.FirstOrDefault().Price.Totals.FirstOrDefault().Amount).ToList().Sum()
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
        public bool EnableEPlusAncillary(int appID, string appVersion, bool isReshop = false)
        {
            return _configuration.GetValue<bool>("EnableEPlusAncillaryChanges") && !isReshop && GeneralHelper.IsApplicationVersionGreaterorEqual(appID, appVersion, _configuration.GetValue<string>("EplusAncillaryAndroidversion"), _configuration.GetValue<string>("EplusAncillaryiOSversion"));
        }

        #region SeatMap
        public bool VersionCheck_NullSession_AfterAppUpgradation(MOBRequest request)
        {

            bool isVersionGreaterorEqual = GeneralHelper.IsApplicationVersionGreater2(request.Application.Id, request.Application.Version.Major, "Android_NullSession_AfterUpgradation_AppVersion", "iPhone_NullSession_AfterUpgradation_AppVersion", null, null, _configuration);
            return isVersionGreaterorEqual;
        }

        public bool EnableUMNRInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneUMNRInformationVersion", "AndroidUMNRInformationVersion", "", "", true, _configuration);
        }

        public bool EnableNewChangeSeatCheckinWindowMsg(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidNewChangeSeatCheckinWindowMsg", "iPhoneNewChangeSeatCheckinWindowMsg", "", "", true, _configuration);
        }

        public bool EnableLufthansaForHigherVersion(string operatingCarrierCode, int applicationId, string appVersion)
        {
            return EnableLufthansa(operatingCarrierCode) &&
                                    GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "Android_EnableInterlineLHRedirectLinkManageRes_AppVersion", "iPhone_EnableInterlineLHRedirectLinkManageRes_AppVersion", "", "", true, _configuration);

        }
        public bool EnableLufthansa(string operatingCarrierCode)
        {

            return _configuration.GetValue<bool>("EnableInterlineLHRedirectLinkManageRes")
                                    && _configuration.GetValue<string>("InterlineLHAndParternerCode").Contains(operatingCarrierCode?.ToUpper());
        }

        public string BuildInterlineRedirectLink(MOBRequest mobRequest, string recordLocator, string lastname, string pointOfSale, string operatingCarrierCode)
        {
            string interlineLhRedirectUrl = string.Empty;

            //this condition for LH only 
            if (_configuration.GetValue<string>("InterlineLHAndParternerCode").Contains(operatingCarrierCode))
            {
                if (GeneralHelper.IsApplicationVersionGreater(mobRequest.Application.Id, mobRequest.Application.Version.Major, "Android_EnableInterlineLHRedirectLinkManageRes_AppVersion", "iPhone_EnableInterlineLHRedirectLinkManageRes_AppVersion", "", "", true, _configuration))
                {
                    //validate the LH and CL 
                    string lufthansaLink = CreateLufthansaDeeplink(recordLocator, lastname, pointOfSale, mobRequest.LanguageCode);

                    interlineLhRedirectUrl = HttpUtility.HtmlDecode(_configuration.GetValue<string>("InterlinLHHtmlText")).Replace("{lufthansaLink}", lufthansaLink);
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                }
            }
            return interlineLhRedirectUrl;
        }
        public string CreateLufthansaDeeplink(string recordLocator, string lastName, string countryCode, string languageCode)
        {
            var stringToEncrypt = string.Format("Mode=S&Filekey={0}&Lastname={1}&Page=BKGD", recordLocator, lastName); ;
            var _cryptoKey = "528B47E01C257B43DB88ABCE62CC7F5A";
            var aesEncryption = new System.Security.Cryptography.RijndaelManaged
            {
                KeySize = 128,
                BlockSize = 128,
                Mode = System.Security.Cryptography.CipherMode.CBC,
                Padding = System.Security.Cryptography.PaddingMode.PKCS7,
                Key = ShopStaticUtility.StringToByteArray(_cryptoKey)
            };

            System.Security.Cryptography.ICryptoTransform crypto = aesEncryption.CreateEncryptor();
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(stringToEncrypt);

            // The result of the encryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, stringToEncrypt.Length);
            var encrypted = ShopStaticUtility.ByteArrayToString(cipherText);
            return string.Format(_configuration.GetValue<string>("InterlineLHRedirectLink"), countryCode, languageCode, encrypted);
        }

        public bool IsTokenMiddleOfFlowDPDeployment()
        {
            return (_configuration.GetValue<bool>("ShuffleVIPSBasedOnCSS_r_DPTOken") && _configuration.GetValue<bool>("EnableDpToken")) ? true : false;

        }
        public string ModifyVIPMiddleOfFlowDPDeployment(string token, string url)
        {
            url = token.Length < 50 ? url.Replace(_configuration.GetValue<string>("DPVIPforDeployment"), _configuration.GetValue<string>("CSSVIPforDeployment")) : url;
            return url;
        }
        #endregion

        public string SpecialcharacterFilterInPNRLastname(string stringTofilter)
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

        public bool EnableActiveFutureFlightCreditPNR(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneActiveFutureFlightCreditPNRVersion", "AndroidActiveFutureFlightCreditPNRVersion", "", "", true, _configuration);
        }

        public string GetCurrencyCode(string code)
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
        public bool EnableFareLockPurchaseViewRes(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnableFareLockPurchaseViewRes")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidFareLockPurchaseViewResVersion", "iPhoneFareLockPurchaseViewResVersion", "", "", true, _configuration);
            }
            return false;
        }
        public void GetCheckInEligibilityStatusFromCSLPnrReservation(System.Collections.ObjectModel.Collection<United.Service.Presentation.CommonEnumModel.CheckinStatus> checkinEligibilityList, ref MOBPNR pnr)
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
        private bool IsNotFlownSegmentExist(List<MOBPNRSegment> segments, int hours, bool SegmentFlownCheckToggle)
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

        public bool IsELFFare(string productCode)
        {
            return EnableIBEFull() && !string.IsNullOrWhiteSpace(productCode) &&
                   "ELF" == productCode.Trim().ToUpper();
        }
        public string[] SplitConcatenatedConfigValue(string configkey, string splitchar)
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

        public bool EnablePetInformation(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhonePetInformationVersion", "AndroidPetInformationVersion", "", "", true, _configuration);
        }
        public MOBPNRAdvisory PopulateTRCAdvisoryContent(string displaycontent)
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
                        string[] lineitem = ShopStaticUtility.SplitConcatenatedString(item, "|");

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

        public bool IncludeTRCAdvisory(MOBPNR pnr, int appId, string appVersion)
        {
            try
            {
                if (!GeneralHelper.IsApplicationVersionGreaterorEqual
                    (appId, appVersion, _configuration.GetValue<string>("AndroidTRCAdvisoryVersion"), _configuration.GetValue<string>("iPhoneTRCAdvisoryVersion"))) return false;

                if (!string.IsNullOrEmpty(pnr.FareLockMessage) || pnr.isgroup || !pnr.IsETicketed
                    || pnr.IsPetAvailable || pnr.HasScheduleChanged || pnr.IsCanceledWithFutureFlightCredit || pnr.MarketType.Equals("Domestic")) return false;

                return true;
            }
            catch { return false; }
        }
        public bool CheckMax737WaiverFlight
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
        public List<string> GetListFrmPipelineSeptdConfigString(string configkey)
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

        public void OneTimeSCChangeCancelAlert(MOBPNR pnr)
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
                            ContentType = ContentType.SCHEDULECHANGE,
                            AdvisoryType = AdvisoryType.INFORMATION,
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

        public string GetCurrencyAmount(double value = 0, string code = "USD", int decimalPlace = 2, string languageCode = "")
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
        public bool EnableConsolidatedAdvisoryMessage(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "iPhoneConsolidatedAdvisoryMessageVersion", "AndroidConsolidatedAdvisoryMessageVersion", "", "", true, _configuration);
        }

        public bool CheckIfTicketedByUA(ReservationDetail response)
        {
            if (response?.Detail?.Characteristic == null) return false;
            string configbookingsource = _configuration.GetValue<string>("PNRUABookingSource");
            var charbookingsource = ShopStaticUtility.GetCharactersticDescription_New(response.Detail.Characteristic, "Booking Source");
            if (string.IsNullOrEmpty(configbookingsource) || string.IsNullOrEmpty(charbookingsource)) return false;
            return (configbookingsource.IndexOf(charbookingsource, StringComparison.OrdinalIgnoreCase) > -1);
        }
        public bool DisableFSRAlertMessageTripPlan(int appId, string appVersion, string travelType)
        {
            return _configuration.GetValue<bool>("EnableAllAirportsOrNearByAirportsAlertOff") && IsTripPlanSearch(travelType) && !GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAllAirportsOrNearByAirportsAlertOffVersion"), _configuration.GetValue<string>("iOSAllAirportsOrNearByAirportsAlertOffVersion"));
        }
        private bool IsTripPlanSearch(string travelType)
        {
            return _configuration.GetValue<bool>("EnableTripPlannerView") && (travelType == MOBTripPlannerType.TPSearch.ToString() || travelType == MOBTripPlannerType.TPEdit.ToString()
                || travelType == MOBTripPlannerType.TPBooking.ToString());
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
        public bool IsAwardFSRRedesignEnabled(int appId, string appVersion)
        {
            if (!_configuration.GetValue<bool>("EnableAwardFSRChanges")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAwardFSRChangesVersion"), _configuration.GetValue<string>("iOSAwardFSRChangesVersion"));
        }
        public bool IsSortDisclaimerForNewFSR(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreater
                (appId, appVersion, "AndroidNewSortDisclaimerVersion", "iPhoneNewSortDisclaimerVersion", "", "", true, _configuration);
        }
        public bool EnableAdvanceSearchCouponBooking(MOBSHOPShopRequest request)
        {
            return (request != null && _configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && !(request.IsReshop || request.IsReshopChange)
              && !request.IsCorporateBooking && string.IsNullOrEmpty(request.EmployeeDiscountId)
              && request.Experiments != null && request.Experiments.Any() && request.Experiments.Contains(ShoppingExperiments.FSRRedesignA.ToString())
              && GeneralHelper.IsApplicationVersionGreaterorEqual(request.Application.Id, request.Application.Version.Major,
                _configuration.GetValue<string>("AndroidAdvanceSearchCouponBookingVersion"),
                _configuration.GetValue<string>("iPhoneAdvanceSearchCouponBookingVersion"))
              && !request.AwardTravel) && !request.IsYoungAdultBooking;
        }

        public async Task<bool> Authorize(string sessionId, int applicationId, string applicationVersion, string deviceId, string mileagePlusNumber, string hash)
        {
            bool validateMPHashpinAuthorize = false;
            string validAuthToken = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(mileagePlusNumber) && !string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(hash))
                {

                    if (await ValidateMileagePlusRecordInCouchbase(sessionId, mileagePlusNumber, hash, deviceId, applicationId, applicationVersion))
                    {
                        validateMPHashpinAuthorize = true;
                    }
                    else if (await ValidateHashPinAndGetAuthToken(mileagePlusNumber, hash, applicationId, deviceId, applicationId.ToString(), sessionId))
                    {
                        validateMPHashpinAuthorize = true;

                    }
                }

                return validateMPHashpinAuthorize;
            }
            catch (Exception ex)
            {
                _logger.LogInformation("AuthorizingCustomer {@Exception}", JsonConvert.SerializeObject(ex));
                return validateMPHashpinAuthorize;
            }

        }
        private async Task<bool> ValidateMileagePlusRecordInCouchbase(string sessionId, string mileagePlusNumber, string hash, string deviceId, int applicationId, string applicationVersion)
        {
            bool validateMPHashpin = false;

            try
            {
                if (!string.IsNullOrEmpty(mileagePlusNumber) && !string.IsNullOrEmpty(deviceId))
                {
                    string mileagePlusNumberKey = GetMileagePlusAuthorizationPredictableKey(mileagePlusNumber, deviceId, applicationId);

                    var customerAuthorizationRecord = await _cachingService.GetCache<CustomerAuthorization>(mileagePlusNumberKey, _headers.ContextValues.TransactionId).ConfigureAwait(false);
                    if (customerAuthorizationRecord != null)
                    {
                        var mileagePlusAuthorizationRecord = JsonConvert.DeserializeObject<CustomerAuthorization>(customerAuthorizationRecord);

                        if (mileagePlusAuthorizationRecord != null && !string.IsNullOrEmpty(mileagePlusAuthorizationRecord.Hash) && mileagePlusAuthorizationRecord.Hash.Equals(hash))
                        {
                            validateMPHashpin = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("ValidateMPHashDeviceIDCheckInCouchbase {@Exception}", JsonConvert.SerializeObject(ex));
            }

            return validateMPHashpin;
        }
        private string GetMileagePlusAuthorizationPredictableKey(string mileagePlus, string deviceId, int applicationId)
        {
            return string.Format("MileagePlusAuthorization::{0}@{1}::{2}", mileagePlus, deviceId, applicationId);
        }
        public bool EnableReShopAirfareCreditDisplay(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableReShopAirfareCreditDisplay")
           && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnableReShopAirfareCreditDisplayVersion", "iPhoneEnableReShopAirfareCreditDisplayVersion", "", "", true, _configuration);
        }
        public InfoWarningMessages GetPriceMismatchMessage()
        {
            InfoWarningMessages infoPriceMessage = new InfoWarningMessages();

            infoPriceMessage.Order = MOBINFOWARNINGMESSAGEORDER.PRICECHANGE.ToString();
            infoPriceMessage.IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString();

            infoPriceMessage.Messages = new List<string>();
            infoPriceMessage.Messages.Add((_configuration.GetValue<string>("PriceMismatchMessage") as string) ?? string.Empty);

            return infoPriceMessage;
        }

        public List<MOBSHOPPrice> UpdatePricesForEFS(MOBSHOPReservation reservation, int appID, string appVersion, bool isReshop)
        {
            if (EnableEPlusAncillary(appID, appVersion, false) &&
                         reservation.TravelOptions != null &&
                         reservation.TravelOptions.Exists(t => t?.Key?.Trim()?.ToUpper() == "EFS"))
            {
                return UpdatePricesForBundles(reservation, null, appID, appVersion, isReshop, "EFS");
            }
            return reservation.Prices;
        }
        private List<MOBSHOPPrice> UpdatePricesForBundles(MOBSHOPReservation reservation, Mobile.Model.Shopping.RegisterOfferRequest request, int appID, string appVersion, bool isReshop, string productId = "")
        {
            List<MOBSHOPPrice> prices = reservation.Prices.Clone();

            if (reservation.TravelOptions != null && reservation.TravelOptions.Count > 0)
            {
                foreach (var travelOption in reservation.TravelOptions)
                {
                    //below if condition modified by prasad for bundle checking
                    //MOB-4676-Added condition to ignore the trip insurance which is added as traveloption - sandeep/saikiran
                    if (!travelOption.Key.Equals("PAS") && (!travelOption.Type.IsNullOrEmpty() && !travelOption.Type.ToUpper().Equals("TRIP INSURANCE"))
                        && (EnableEPlusAncillary(appID, appVersion, isReshop) ? !travelOption.Key.Trim().ToUpper().Equals("FARELOCK") : true)
                        && !(_configuration.GetValue<bool>("EnableEplusCodeRefactor") && !string.IsNullOrEmpty(productId) && productId.Trim().ToUpper() != travelOption.Key.Trim().ToUpper()))
                    {
                        List<MOBSHOPPrice> totalPrices = new List<MOBSHOPPrice>();
                        bool totalExist = false;
                        double flightTotal = 0;

                        CultureInfo ci = null;

                        for (int i = 0; i < prices.Count; ++i)
                        {
                            if (ci == null)
                                ci = TopHelper.GetCultureInfo(prices[i].CurrencyCode);

                            if (prices[i].DisplayType.ToUpper() == "GRAND TOTAL")
                            {
                                totalExist = true;
                                prices[i].DisplayValue = string.Format("{0:#,0.00}", (Convert.ToDouble(prices[i].DisplayValue) + travelOption.Amount));
                                prices[i].FormattedDisplayValue = TopHelper.FormatAmountForDisplay(prices[i].DisplayValue, ci, false); // string.Format("{0:c}", prices[i].DisplayValue);
                                double tempDouble1 = 0;
                                double.TryParse(prices[i].DisplayValue.ToString(), out tempDouble1);
                                prices[i].Value = Math.Round(tempDouble1, 2, MidpointRounding.AwayFromZero);
                            }
                            if (prices[i].DisplayType.ToUpper() == "TOTAL")
                            {
                                flightTotal = Convert.ToDouble(prices[i].DisplayValue);
                            }
                        }
                        MOBSHOPPrice travelOptionPrice = new MOBSHOPPrice();
                        travelOptionPrice.CurrencyCode = travelOption.CurrencyCode;
                        travelOptionPrice.DisplayType = "Travel Options";
                        travelOptionPrice.DisplayValue = string.Format("{0:#,0.00}", travelOption.Amount.ToString());
                        travelOptionPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(travelOptionPrice.DisplayValue, ci, false); //Convert.ToDouble(travelOptionPrice.DisplayValue).ToString("C2", CultureInfo.CurrentCulture);

                        if (_configuration.GetValue<bool>("EnableEplusCodeRefactor") && travelOption.Key?.Trim().ToUpper() == "EFS")
                        {
                            travelOptionPrice.PriceType = "EFS";
                        }
                        else
                        {
                            travelOptionPrice.PriceType = "Travel Options";
                        }

                        double tmpDouble1 = 0;
                        double.TryParse(travelOptionPrice.DisplayValue.ToString(), out tmpDouble1);
                        travelOptionPrice.Value = Math.Round(tmpDouble1, 2, MidpointRounding.AwayFromZero);

                        prices.Add(travelOptionPrice);

                        if (!totalExist)
                        {
                            MOBSHOPPrice totalPrice = new MOBSHOPPrice();
                            totalPrice.CurrencyCode = travelOption.CurrencyCode;
                            totalPrice.DisplayType = "Grand Total";
                            totalPrice.DisplayValue = (flightTotal + travelOption.Amount).ToString("N2", CultureInfo.InvariantCulture);
                            totalPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(totalPrice.DisplayValue, ci, false); //string.Format("${0:c}", totalPrice.DisplayValue);
                            double tempDouble1 = 0;
                            double.TryParse(totalPrice.DisplayValue.ToString(), out tempDouble1);
                            totalPrice.Value = Math.Round(tempDouble1, 2, MidpointRounding.AwayFromZero);
                            totalPrice.PriceType = "Grand Total";
                            prices.Add(totalPrice);
                        }
                    }
                }
            }
            if (request != null && request.ClubPassPurchaseRequest != null)
            {
                List<MOBSHOPPrice> totalPrices = new List<MOBSHOPPrice>();
                bool totalExist = false;
                double flightTotal = 0;

                CultureInfo ci = null;

                for (int i = 0; i < prices.Count; ++i)
                {
                    if (ci == null)
                        ci = TopHelper.GetCultureInfo(prices[i].CurrencyCode);

                    if (prices[i].DisplayType.ToUpper() == "GRAND TOTAL")
                    {
                        totalExist = true;
                        prices[i].DisplayValue = string.Format("{0:#,0.00}", Convert.ToDouble(prices[i].DisplayValue) + request.ClubPassPurchaseRequest.AmountPaid);
                        prices[i].FormattedDisplayValue = Convert.ToDouble(prices[i].DisplayValue).ToString("C2", CultureInfo.CurrentCulture);
                        double tempDouble1 = 0;
                        double.TryParse(prices[i].DisplayValue.ToString(), out tempDouble1);
                        prices[i].Value = Math.Round(tempDouble1, 2, MidpointRounding.AwayFromZero);
                    }
                    if (prices[i].DisplayType.ToUpper() == "TOTAL")
                    {
                        flightTotal = Convert.ToDouble(prices[i].DisplayValue);
                    }
                }
                MOBSHOPPrice otpPrice = new MOBSHOPPrice();
                otpPrice.CurrencyCode = prices[prices.Count - 1].CurrencyCode;
                otpPrice.DisplayType = "One-time Pass";
                otpPrice.DisplayValue = string.Format("{0:#,0.00}", request.ClubPassPurchaseRequest.AmountPaid);
                double tempDouble = 0;
                double.TryParse(otpPrice.DisplayValue.ToString(), out tempDouble);
                otpPrice.Value = Math.Round(tempDouble, 2, MidpointRounding.AwayFromZero);
                otpPrice.FormattedDisplayValue = request.ClubPassPurchaseRequest.AmountPaid.ToString("C2", CultureInfo.CurrentCulture);
                otpPrice.PriceType = "One-time Pass";
                if (totalExist)
                {
                    prices.Insert(prices.Count - 2, otpPrice);
                }
                else
                {
                    prices.Add(otpPrice);
                }

                if (!totalExist)
                {
                    MOBSHOPPrice totalPrice = new MOBSHOPPrice();
                    totalPrice.CurrencyCode = prices[prices.Count - 1].CurrencyCode;
                    totalPrice.DisplayType = "Grand Total";
                    totalPrice.DisplayValue = (flightTotal + request.ClubPassPurchaseRequest.AmountPaid).ToString("N2", CultureInfo.InvariantCulture);
                    //totalPrice.DisplayValue = string.Format("{0:#,0.00}", (flightTotal + request.ClubPassPurchaseRequest.AmountPaid).ToString("{0:#,0.00}", CultureInfo.InvariantCulture);
                    totalPrice.FormattedDisplayValue = TopHelper.FormatAmountForDisplay(totalPrice.DisplayValue, ci, false); //string.Format("${0:c}", totalPrice.DisplayValue);
                    double tempDouble1 = 0;
                    double.TryParse(totalPrice.DisplayValue.ToString(), out tempDouble1);
                    totalPrice.Value = Math.Round(tempDouble1, 2, MidpointRounding.AwayFromZero);
                    totalPrice.PriceType = "Grand Total";
                    prices.Add(totalPrice);
                }
            }
            return prices;
        }

        private List<string> OrderPCUTnC(List<string> productCodes)
        {
            if (productCodes == null || !productCodes.Any())
                return productCodes;

            return productCodes.OrderBy(p => GetProductOrderTnC()[GetProductTnCtoOrder(p)]).ToList();
        }
        private string GetCommonSeatCode(string seatCode)
        {
            if (string.IsNullOrEmpty(seatCode))
                return string.Empty;

            string commonSeatCode = string.Empty;

            switch (seatCode.ToUpper().Trim())
            {
                case "SXZ": //StandardPreferredExitPlus
                case "SZX": //StandardPreferredExit
                case "SBZ": //StandardPreferredBlukheadPlus
                case "SZB": //StandardPreferredBlukhead
                case "SPZ": //StandardPreferredZone
                case "PZA":
                    commonSeatCode = "PZA";
                    break;
                case "SXP": //StandardPrimeExitPlus
                case "SPX": //StandardPrimeExit
                case "SBP": //StandardPrimeBlukheadPlus
                case "SPB": //StandardPrimeBlukhead
                case "SPP": //StandardPrimePlus
                case "PPE": //StandardPrime
                case "BSA":
                case "ASA":
                    commonSeatCode = "ASA";
                    break;
                case "EPL": //EplusPrime
                case "EPU": //EplusPrimePlus
                case "BHS": //BulkheadPrime
                case "BHP": //BulkheadPrimePlus  
                case "PSF": //PrimePlus    
                    commonSeatCode = "EPU";
                    break;
                case "PSL": //Prime                           
                    commonSeatCode = "PSL";
                    break;
                default:
                    return seatCode;
            }
            return commonSeatCode;
        }
        private Dictionary<string, int> GetProductOrderTnC()
        {
            return new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) {
                    { "SEATASSIGNMENTS", 0 },
                    { "PCU", 1 },
                    { string.Empty, 2 } };
        }
        private string GetProductTnCtoOrder(string productCode)
        {
            productCode = string.IsNullOrEmpty(productCode) ? string.Empty : productCode.ToUpper().Trim();

            if (productCode == "SEATASSIGNMENTS" || productCode == "PCU")
                return productCode;

            return string.Empty;
        }
        private bool IsBundleProductSelected(FlightReservationResponse flightReservationResponse)
        {
            if (!_configuration.GetValue<bool>("EnableTravelOptionsBundleInViewRes"))
                return false;

            return flightReservationResponse?.ShoppingCart?.Items?.Where(x => x.Product?.FirstOrDefault()?.Code != "RES")?.Any(x => x.Product?.Any(p => p?.SubProducts?.Any(sp => sp?.GroupCode == "BE") ?? false) ?? false) ?? false;
        }
        private async Task<List<MOBMobileCMSContentMessages>> GetTermsAndConditions(bool hasPremierAccelerator)
        {

            var dbKey = _configuration.GetValue<bool>("EnablePPRChangesForAAPA") ? hasPremierAccelerator ? "PPR_AAPA_TERMS_AND_CONDITIONS_AA_PA_MP"
                                              : "PPR_AAPA_TERMS_AND_CONDITIONS_AA_MP" : hasPremierAccelerator ? "AAPA_TERMS_AND_CONDITIONS_AA_PA_MP"
                                              : "AAPA_TERMS_AND_CONDITIONS_AA_MP";

            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(dbKey, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
            if (docs == null || !docs.Any())
                return null;

            var tncs = new List<MOBMobileCMSContentMessages>();
            foreach (var doc in docs)
            {
                var tnc = new MOBMobileCMSContentMessages
                {
                    Title = "Terms and conditions",
                    ContentFull = doc.LegalDocument,
                    ContentShort = _configuration.GetValue<string>("PaymentTnCMessage"),
                    HeadLine = doc.Title
                };
                tncs.Add(tnc);
            }

            return tncs;
        }
        private List<MOBTypeOption> GetPBContentList(string configValue)
        {
            List<MOBTypeOption> contentList = new List<MOBTypeOption>();
            if (_configuration.GetValue<string>(configValue) != null)
            {
                string pBContentList = HttpUtility.HtmlDecode(_configuration.GetValue<string>(configValue));
                foreach (string eachItem in pBContentList.Split('~'))
                {
                    contentList.Add(new MOBTypeOption(eachItem.Split('|')[0].ToString(), eachItem.Split('|')[1].ToString()));
                }
            }
            return contentList;
        }
        private List<MOBTypeOption> GetPATermsAndConditionsList()
        {
            List<MOBTypeOption> tAndCList = new List<MOBTypeOption>();
            if (_configuration.GetValue<string>("PremierAccessTermsAndConditionsList") != null)
            {
                string premierAccessTermsAndConditionsList = _configuration.GetValue<string>("PremierAccessTermsAndConditionsList");
                foreach (string eachItem in premierAccessTermsAndConditionsList.Split('~'))
                {
                    tAndCList.Add(new MOBTypeOption(eachItem.Split('|')[0].ToString(), eachItem.Split('|')[1].ToString()));
                }
            }
            else
            {
                #region
                tAndCList.Add(new MOBTypeOption("paTandC1", "This Premier Access offer is nonrefundable and non-transferable"));
                tAndCList.Add(new MOBTypeOption("paTandC2", "Voluntary changes to your itinerary may forfeit your Premier Access purchase and \n any associated fees."));
                tAndCList.Add(new MOBTypeOption("paTandC3", "In the event of a flight cancellation or involuntary schedule change, we will refund \n the fees paid for the unused Premier Access product upon request."));
                tAndCList.Add(new MOBTypeOption("paTandC4", "Premier Access is offered only on flights operated by United and United Express."));
                tAndCList.Add(new MOBTypeOption("paTandC5", "This Premier Access offer is processed based on availability at time of purchase."));
                tAndCList.Add(new MOBTypeOption("paTandC6", "Premier Access does not guarantee wait time in airport check-in, boarding, or security lines. Premier Access does not exempt passengers from check-in time limits."));
                tAndCList.Add(new MOBTypeOption("paTandC7", "Premier Access benefits apply only to the customer who purchased Premier Access \n unless purchased for all customers on a reservation. Each travel companion must purchase Premier Access in order to receive benefits."));
                tAndCList.Add(new MOBTypeOption("paTandC8", "“Premier Access” must be printed or displayed on your boarding pass in order to \n receive benefits."));
                tAndCList.Add(new MOBTypeOption("paTandC9", "This offer is made at United's discretion and is subject to change or termination \n at any time with or without notice to the customer."));
                tAndCList.Add(new MOBTypeOption("paTandC10", "By clicking “I agree - Continue to purchase” you agree to all terms and conditions."));
                #endregion
            }
            return tAndCList;
        }
        private async Task<List<MOBMobileCMSContentMessages>> GetTermsAndConditions()
        {
            var cmsContentMessages = new List<MOBMobileCMSContentMessages>();
            var docKeys = "PCU_TnC";
            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(docKeys, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);

            if (docs != null && docs.Any())
            {
                foreach (var doc in docs)
                {
                    var cmsContentMessage = new MOBMobileCMSContentMessages();
                    cmsContentMessage.ContentFull = doc.LegalDocument;
                    cmsContentMessage.Title = doc.Title;
                    cmsContentMessages.Add(cmsContentMessage);
                }
            }

            return cmsContentMessages;
        }
        public string GetPaymentTargetForRegisterFop(TravelOptionsCollection travelOptions, bool isCompleteFarelockPurchase = false)
        {
            if (string.IsNullOrEmpty(_configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes")))
                return string.Empty;

            if (isCompleteFarelockPurchase)
                return "RES";

            if (travelOptions == null || !travelOptions.Any())
                return string.Empty;

            return string.Join(",", travelOptions.Select(x => x.Type == "SEATASSIGNMENTS" ? x.Type : x.Key).Distinct());
        }
        public string GetBookingPaymentTargetForRegisterFop(FlightReservationResponse flightReservationResponse)
        {
            if (flightReservationResponse.ShoppingCart == null || !flightReservationResponse.ShoppingCart.Items.Any())
                return string.Empty;

            return string.Join(",", flightReservationResponse.ShoppingCart.Items.SelectMany(x => x.Product).Select(x => x.Code).Distinct());
        }

        public async Task PopulateMissingValues(MOBSHOPSelectUnfinishedBookingRequest request)
        {
            try
            {
                if (!_configuration.GetValue<bool>("DisableUnfinishedBookingIosBugFix") && GeneralHelper.IsApplicationVersionGreaterorEqual(request.Application.Id, request.Application.Version.Major, "", _configuration.GetValue<string>("Iphone_UnfinishedBookingsBugFix_AppVersion")))
                {
                    if (string.IsNullOrEmpty(request.DeviceId))
                    {
                        request.DeviceId = !string.IsNullOrEmpty(request.TransactionId) ? request.TransactionId.Split('|')[0] : request.DeviceId;
                        string guid = request.DeviceId + "_" + request.Application.Name + "_" + request.Application.Version.Major;
                        var getUnfinished = await _cachingService.GetCache<MOBSHOPGetUnfinishedBookingsRequest>(guid + "MOBSHOPGetUnfinishedBookingsRequest", request.TransactionId).ConfigureAwait(false);
                        var getUnfinishedBookingRequest = JsonConvert.DeserializeObject<MOBSHOPGetUnfinishedBookingsRequest>(getUnfinished);
                        if (getUnfinishedBookingRequest != null && getUnfinishedBookingRequest.AccessCode.Contains(request.SelectedUnfinishBooking.Id))
                        {
                            request.DeviceId = getUnfinishedBookingRequest.DeviceId;
                            request.MileagePlusAccountNumber = getUnfinishedBookingRequest.MileagePlusAccountNumber;
                            request.PasswordHash = getUnfinishedBookingRequest.PasswordHash;
                            request.CustomerId = getUnfinishedBookingRequest.CustomerId;
                            request.PremierStatusLevel = getUnfinishedBookingRequest.PremierStatusLevel;
                        }

                    }
                }
            }
            catch { }
        }
        public async Task<MOBShoppingCart> PopulateShoppingCart(MOBShoppingCart shoppingCart, string flow, string sessionId, string CartId, MOBRequest request = null, Mobile.Model.Shopping.MOBSHOPReservation reservation = null)
        {
            shoppingCart = shoppingCart ?? new MOBShoppingCart();
            shoppingCart = await _sessionHelperService.GetSession<MOBShoppingCart>(sessionId, shoppingCart.ObjectName, new List<string> { sessionId, shoppingCart.ObjectName }).ConfigureAwait(false);
            if (shoppingCart == null)
                shoppingCart = new MOBShoppingCart();
            shoppingCart.CartId = CartId;
            shoppingCart.Flow = flow;
            if (flow == FlowType.BOOKING.ToString() && request?.Application != null
               && IsEnableOmniCartMVP2Changes(request.Application.Id, request.Application.Version.Major, reservation?.ShopReservationInfo2?.IsDisplayCart == true)
               )
            {
                BuildOmniCart(shoppingCart, reservation);
            }
            await _sessionHelperService.SaveSession<MOBShoppingCart>(shoppingCart, sessionId, new List<string> { sessionId, shoppingCart.ObjectName }, shoppingCart.ObjectName).ConfigureAwait(false);
            return shoppingCart;
        }
        public void BuildOmniCart(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation)
        {
            if (shoppingCart.OmniCart == null)
            {
                shoppingCart.OmniCart = new Cart();
            }
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

        private static string GetCostBreakdownFareHeader(string travelType)
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

        private List<MOBSection> GetFOPDetails(MOBSHOPReservation reservation)
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


        private void AssignUpliftText(MOBShoppingCart shoppingCart, MOBSHOPReservation reservation)
        {

            //if (_shoppingUtility.IsEligibileForUplift(reservation, shoppingCart) && Shoppingcart?.Form)                //Check if eligible for Uplift
            if (IsEligibileForUplift(reservation, shoppingCart) && shoppingCart?.FormofPaymentDetails?.MoneyPlusMilesCredit?.SelectedMoneyPlusMiles == null) //Check if eligible for Uplift
            {
                shoppingCart.OmniCart.IsUpliftEligible = true;      //Set property to true, if Uplift is eligible                
            }
            else //Set Uplift properties to false / empty as Uplift isn't eligible
            {
                shoppingCart.OmniCart.IsUpliftEligible = false;
            }
        }

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
        public string GetFareLockPrice(List<ProdDetail> products)
        {
            return products.Where(p => p.Code.ToUpper() == "FARELOCK" || p.Code.ToUpper() == "FLK").First().ProdDisplayTotalPrice;
        }

        public bool IsAllFareLockOptionEnabled(int id, string version, List<MOBItem> catalgItems = null)
        {
            if (!_configuration.GetValue<bool>("EnableContinueFareLockDynamicOption")) return false;
            if ((catalgItems != null && catalgItems.Count > 0 &&
                  catalgItems.FirstOrDefault(a => a.Id == _configuration.GetValue<string>("Android_EnableAllFareLockOptionFeatureCatalogID") || a.Id == _configuration.GetValue<string>("iOS_EnableAllFareLockOptionFeatureCatalogID"))?.CurrentValue == "1"))
                return GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("Android_EnableContinueFareLockDynamicOption_AppVersion"), _configuration.GetValue<string>("IPhone_EnableContinueFareLockDynamicOption_AppVersion"));
            else
                return false;
        }

        public bool? GetBooleanFromCharacteristics(Collection<Characteristic> characteristic, string key)
        {
            if (characteristic != null && characteristic.Any())
            {
                string stringvalue = GetCharactersticValue(characteristic, key);
                if (!string.IsNullOrEmpty(stringvalue))
                {
                    Boolean.TryParse(stringvalue, out bool boolvalue);
                    return boolvalue;
                }
            }
            return null;
        }
        public string GetCharactersticValue(Collection<Characteristic> characteristics, string code)
        {
            if (characteristics == null || characteristics.Count <= 0) return string.Empty;
            var characteristic = characteristics.FirstOrDefault(c => c != null && c.Code != null
            && !string.IsNullOrEmpty(c.Code) && c.Code.Trim().Equals(code, StringComparison.InvariantCultureIgnoreCase));
            return characteristic == null ? string.Empty : characteristic.Value;
        }
        public string GetSDLStringMessageFromList(List<CMSContentMessage> list, string title)
        {
            return list?.Where(x => x.Title.Equals(title))?.FirstOrDefault()?.ContentFull?.Trim();
        }
        public bool IsEnableMostPopularBundle(int appId, string version)
        {
            if (!_configuration.GetValue<bool>("EnableMostPopularBundleFeature")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_EnableMostPopularBundleFeature_AppVersion"), _configuration.GetValue<string>("IPhone_EnableMostPopularBundleFeature_AppVersion"));
        }
        public bool IsCheckedIn(ReservationFlightSegment cslSegment)
        {
            var characteristic = cslSegment?.Characteristic;
            try
            {
                if (characteristic != null && characteristic.Any())
                {
                    var selectDesc = characteristic.FirstOrDefault
                        (x => (string.Equals(x.Code, "CheckedIn", StringComparison.OrdinalIgnoreCase)));

                    bool.TryParse(selectDesc?.Value, out bool retValue);

                    return retValue;
                }
            }
            catch { }
            return false;
        }

        public bool IsCheckInEligible(ReservationFlightSegment cslSegment)
        {
            var characteristic = cslSegment?.Characteristic;
            try
            {
                if (characteristic != null && characteristic.Any())
                {
                    var selectDesc = characteristic.FirstOrDefault
                        (x => (string.Equals(x.Code, "CheckinTriggered", StringComparison.OrdinalIgnoreCase)));

                    bool.TryParse(selectDesc?.Value, out bool retValue);

                    return retValue;
                }
            }
            catch { }
            return false;
        }

        public bool IsAllPaxCheckedIn(ReservationFlightSegment cslSegment)
        {
            var characteristic = cslSegment?.Characteristic;
            try
            {
                if (characteristic != null && characteristic.Any())
                {
                    var selectDesc = characteristic.FirstOrDefault
                        (x => (string.Equals(x.Code, "CheckinTriggered", StringComparison.OrdinalIgnoreCase)));

                    bool.TryParse(selectDesc?.Value, out bool retValue);

                    return retValue;
                }
            }
            catch { }
            return false;
        }

        public bool IsAllPaxCheckedIn(ReservationDetail reservation, ReservationFlightSegment cslSegment, bool isCheckedIn)
        {
            //NO-CHECK-CONDITIONS 
            var reservationDetails = reservation?.Detail;

            if (!isCheckedIn && reservationDetails == null && cslSegment == null) return false;

            var travelers = reservationDetails?.Travelers;

            if (reservationDetails.Travelers == null || !reservationDetails.Travelers.Any()) return false;

            var paxCount = reservationDetails.Travelers?.Count();
            var segmentNumber = cslSegment.SegmentNumber;

            bool isAllPaxCheckedIn = true;
            try
            {
                reservation.Detail.Travelers.ForEach(pax =>
                {

                    var firstTicketCoupons = pax.Tickets?.FirstOrDefault()?.FlightCoupons;

                    if (firstTicketCoupons != null && firstTicketCoupons.Any())
                    {
                        firstTicketCoupons.ForEach(coupons =>
                        {
                            if (coupons?.FlightSegment?.SegmentNumber == segmentNumber)
                            {
                                if (!string.Equals
                                (coupons?.Status?.Code, "CHECKED-IN", StringComparison.OrdinalIgnoreCase))
                                {
                                    isAllPaxCheckedIn = isAllPaxCheckedIn && false;
                                }
                            }
                        });
                    }
                });
            }
            catch { }
            return isAllPaxCheckedIn;
        }
        public bool EnableAdvanceSearchCouponBooking(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAdvanceSearchCouponBookingVersion"), _configuration.GetValue<string>("iPhoneAdvanceSearchCouponBookingVersion"));
        }

        public bool IsNonRefundableNonChangable(string productCode)
        {
            var supportedProductCodes = _configuration.GetValue<string>("NonRefundableNonChangableProductCodes");
            return EnableNonRefundableNonChangable() && !string.IsNullOrWhiteSpace(productCode) &&
                   !string.IsNullOrWhiteSpace(supportedProductCodes) &&
                   supportedProductCodes.IndexOf(productCode.Trim().ToUpper()) > -1;
        }

        private bool EnableNonRefundableNonChangable()
        {
            return _configuration.GetValue<bool>("EnableNonRefundableNonChangable");
        }

        public async Task<United.Mobile.Model.Shopping.InfoWarningMessages> GetNonRefundableNonChangableInversionMessage(MOBRequest request, Session session)
        {
            List<CMSContentMessage> lstMessages = await _ffcShoppingcs.GetSDLContentByGroupName(request, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_BookingRTI_Messages"), _configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID"));
            var message = GetSDLStringMessageFromList(lstMessages, _configuration.GetValue<string>("NonRefundableNonChangableFareInversionMessage"));
            return BuildInfoWarningMessages(message);
        }
        private static United.Mobile.Model.Shopping.InfoWarningMessages BuildInfoWarningMessages(string message)
        {
            var infoWarningMessages = new United.Mobile.Model.Shopping.InfoWarningMessages
            {
                Order = MOBINFOWARNINGMESSAGEORDER.BEFAREINVERSION.ToString(),
                IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString(),
                Messages = new List<string>
                {
                    message
                }
            };
            return infoWarningMessages;
        }

        private async Task<United.Mobile.Model.Shopping.InfoWarningMessages> BuildUpgradeFromNonRefuNonChanInfoMessage(MOBRequest request, Session session)
        {
            List<CMSContentMessage> lstMessages = await _ffcShoppingcs.GetSDLContentByGroupName(request, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_BookingRTI_Messages"), _configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID"));
            var message = GetSDLMessageFromList(lstMessages, _configuration.GetValue<string>("UpgradedFromNonRefuNonChanTextWithHtml")).FirstOrDefault();
            var infoWarningMessages = new United.Mobile.Model.Shopping.InfoWarningMessages
            {
                Order = MOBINFOWARNINGMESSAGEORDER.INHIBITBOOKING.ToString(), // Using existing order for sorting. 
                IconType = MOBINFOWARNINGMESSAGEICON.INFORMATION.ToString(),
                HeaderMessage = (request.Application.Id == 1) ? message.HeadLine : string.Empty,
                Messages = new List<string>
                {
                   (request.Application.Id==1)? message.ContentShort : message.ContentFull
                }
            };
            return infoWarningMessages;
        }

        public bool IsNonRefundableNonChangable(DisplayCart displayCart)
        {
            return EnableNonRefundableNonChangable() &&
                    displayCart != null &&
                    IsNonRefundableNonChangable(displayCart.ProductCode);
        }

        public bool IsFSRNearByAirportAlertEnabled(int id, string version)
        {
            if (!_configuration.GetValue<bool>("EnableFSRNearByAirportAlertFeature")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("Android_EnableFSRNearByAirportAlertFeature_AppVersion"), _configuration.GetValue<string>("IPhone_EnableFSRNearByAirportAlertFeature_AppVersion"));
        }

        public bool IsServiceAnimalEnhancementEnabled(int id, string version, List<MOBItem> catalogItems)
        {
            if (!_configuration.GetValue<bool>("EnableServiceAnimalEnhancements")) return false;
            if (catalogItems != null && catalogItems.Count > 0 &&
                              catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableTaskTrainedServiceAnimalFeature).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableTaskTrainedServiceAnimalFeature).ToString())?.CurrentValue == "1")
                return GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("Android_EnableServiceAnimalEnhancements_AppVersion"), _configuration.GetValue<string>("IPhone_EnableServiceAnimalEnhancements_AppVersion"));
            else return false;
        }

        public bool EnableBagCalcSelfRedirect(int id, string version)
        {
            return _configuration.GetValue<bool>("EnableBagCalcSelfRedirect") && GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("AndroidBagCalcSelfRedirectVersion"), _configuration.GetValue<string>("iOSBagCalcSelfRedirectVersion"));
        }
        public List<MOBMobileCMSContentMessages> GetSDLMessageFromList(List<CMSContentMessage> list, string title)
        {
            List<MOBMobileCMSContentMessages> listOfMessages = new List<MOBMobileCMSContentMessages>();
            list?.Where(l => l.Title.ToUpper().Equals(title.ToUpper()))?.ForEach(i => listOfMessages.Add(new MOBMobileCMSContentMessages()
            {
                Title = i.Title,
                ContentFull = i.ContentFull,
                HeadLine = i.Headline,
                ContentShort = i.ContentShort,
                LocationCode = i.LocationCode
            }));

            return listOfMessages;
        }

        public string BuildStrikeThroughDescription()
        {
            return _configuration.GetValue<string>("StrikeThroughPriceTypeDescription");
        }
        public bool IsEnableBulkheadNoUnderSeatStorage(int appId, string version)
        {
            if (!_configuration.GetValue<bool>("EnableBulkSeatNoUnderSeatCoverageFeature")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_EnableBulkSeatNoUnderSeatCoverageFeature_AppVersion"), _configuration.GetValue<string>("IPhone_EnableBulkSeatNoUnderSeatCoverageFeature_AppVersion"));
        }

        public bool EnablePOMDeepLinkRedirect(int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("POMDeepLinkRedirect") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_POMDeepLinkRedirect_AppVersion"), _configuration.GetValue<string>("IPhone_POMDeepLinkRedirect_AppVersion")) && CheckClientCatalogForEnablingFeature("POMDeepLinkRedirectClientCatalogValues", catalog);
        }
        public bool EnablePOMDeepLinkInActivePNR(int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("EnablePOMDeepLinkInActivePNR") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_EnablePOMDeepLinkInActivePNR_AppVersion"), _configuration.GetValue<string>("IPhone_EnablePOMDeepLinkInActivePNR_AppVersion"))
                && (catalog != null && catalog.Count > 0
                && catalog.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnablePOMDeepLinkInActivePNR).ToString() || a.Id == ((int)AndroidCatalogEnum.EnablePOMDeepLinkInActivePNR).ToString())?.CurrentValue == "1");
        }

        public bool EnableEditForAllCabinPOM(int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("EnableisEditablePOMFeature") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_isEditablePOMFeatureSupported_AppVersion"), _configuration.GetValue<string>("IPhone_isEditablePOMFeatureSupported_AppVersion")) && CheckClientCatalogForEnablingFeature("POMClientCatalogValues", catalog);
        }

        public bool EnableEditForAllCabinPOM(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableisEditablePOMFeature") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_isEditablePOMFeatureSupported_AppVersion"), _configuration.GetValue<string>("IPhone_isEditablePOMFeatureSupported_AppVersion"));
        }

        public bool EnablePOMPreArrival(int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("EnablePOMPreArrival") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_EnablePOMPreArrival_AppVersion"), _configuration.GetValue<string>("IPhone_EnablePOMPreArrival_AppVersion")) && CheckClientCatalogForEnablingFeature("POMPreArrivalClientCatalogValues", catalog);
        }

        public bool EnablePOMPreArrival(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnablePOMPreArrival") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_EnablePOMPreArrival_AppVersion"), _configuration.GetValue<string>("IPhone_EnablePOMPreArrival_AppVersion"));
        }

        public bool EnablePOMMealOutOfStock(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableMealOutOfStockFix") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_EnablePOMOutOfStock_AppVersion"), _configuration.GetValue<string>("IPhone_EnablePOMOutOfStock_AppVersion"));
        }

        public bool EnablePOMFlightEligibilityCheck(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableFlightEligibilityCheck") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_EnableFlightEligibilityCheck_AppVersion"), _configuration.GetValue<string>("IPhone_EnableFlightEligibilityCheck_AppVersion"));
        }

        public bool HasNearByAirport(ShopResponse _cslShopResponse)
        {
            var flights = _cslShopResponse.Trips.FirstOrDefault()?.Flights;
            return (flights?.SelectMany(a => a.Warnings).Where(b => b.Key == _configuration.GetValue<string>("WARNING_NEARBYAIRPORT"))?.Any() == true ? true : false);
        }

        public bool CheckClientCatalogForEnablingFeature(string catalogFeature, List<MOBItem> clientCatalog)
        {
            if (!string.IsNullOrEmpty(catalogFeature) && clientCatalog != null && clientCatalog.Count > 0)
            {
                string catalogId = _configuration.GetValue<string>(catalogFeature);
                var Id = catalogId.Split('|');
                foreach (var item in clientCatalog)
                {
                    if (Id.Contains(item?.Id))
                        return !string.IsNullOrEmpty(item?.CurrentValue) && item?.CurrentValue == "1";
                }
            }
            return false;
        }

        public async Task<(bool returnValue, string validAuthToken)> ValidateHashPinAndGetAuthToken(string accountNumber, string hashPinCode, int applicationId, string deviceId, string appVersion, string validAuthToken, string sessionId)
        {
            bool ok = false;
            bool iSDPAuthentication = _configuration.GetValue<bool>("EnableDPToken");
            string SPname = string.Empty;

            /// CSS Token length is 36 and Data Power Access Token length is more than 1500 to 1700 chars
            if (iSDPAuthentication)
            {
                SPname = "uasp_select_MileagePlusAndPin_DP";
            }
            else
            {
                SPname = "uasp_select_MileagePlusAndPin_CSS";
            }

            //Database database = DatabaseFactory.CreateDatabase("ConnectionString - iPhone");
            //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand(SPname);
            //database.AddInParameter(dbCommand, "@MileagePlusNumber", DbType.String, accountNumber);
            //database.AddInParameter(dbCommand, "@HashPincode", DbType.String, hashPinCode);
            //database.AddInParameter(dbCommand, "@ApplicationID", DbType.Int32, applicationId);
            //database.AddInParameter(dbCommand, "@AppVersion", DbType.String, appVersion);
            //database.AddInParameter(dbCommand, "@DeviceID", DbType.String, deviceId);
            //try
            //{
            //    using (IDataReader dataReader = database.ExecuteReader(dbCommand))
            //    {
            //        while (dataReader.Read())
            //        {
            //            if (Convert.ToInt32(dataReader["AccountFound"]) == 1)
            //            {
            //                ok = true;
            //                validAuthToken = dataReader["AuthenticatedToken"].ToString();
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex) { string msg = ex.Message; }

            if (ok == false && _configuration.GetValue<String>("ByPassMPByPassCheckForDpMPSignCall2_1_41") != null &&
                _configuration.GetValue<String>("ByPassMPByPassCheckForDpMPSignCall2_1_41").ToString().ToUpper().Trim() == appVersion.ToUpper().Trim())
            {
                var deviceDynamodb = new DeviceDynamDB(_configuration, _dynamoDBService);
                ok = await deviceDynamodb.ValidateDeviceIDAPPID(deviceId, applicationId, accountNumber, appVersion).ConfigureAwait(false);

            }

            return (ok, validAuthToken);
        }

        public async Task<string> ValidateAndGetSingleSignOnWebShareToken(MOBRequest request, string mileagePlusAccountNumber, string passwordHash, string sessionId)
        {
            bool validSSORequest = false;
            string webShareToken = string.Empty;
            try
            {
                string authToken = string.Empty;

                if (!string.IsNullOrEmpty(mileagePlusAccountNumber) && !string.IsNullOrEmpty(mileagePlusAccountNumber.Trim()) && !string.IsNullOrEmpty(passwordHash))
                {
                    var tupleRes = await ValidateHashPinAndGetAuthToken
                        (mileagePlusAccountNumber, passwordHash, request.Application.Id, request.DeviceId, request.Application.Version.Major, authToken, sessionId);
                    validSSORequest = tupleRes.returnValue;
                    authToken = tupleRes.validAuthToken;

                }
                if (validSSORequest)
                {
                    webShareToken = _dPService.GetSSOTokenString(request.Application.Id, mileagePlusAccountNumber, _configuration);
                }
            }
            catch (Exception ex)
            {
                string[] messages = ex.Message.Split('#');

                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                exceptionWrapper.Message = messages[0];
                //logEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBExceptionWrapper>(sessionId, "SingleSignOn", "Exception", request.Application.Id, request.Application.Version.Major, request.DeviceId, exceptionWrapper, true, false));
                //logEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBExceptionWrapper>(sessionId, "SingleSignOnIPOS", "DotComSSOBuildError", request.Application.Id, request.Application.Version.Major, request.DeviceId, exceptionWrapper, true, false));

            }
            return webShareToken;
        }

        public bool IsInterLine(string operatingCarrier, string MarketingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier) && string.IsNullOrEmpty(MarketingCarrier)) return false;
            var interLineAirlines = _configuration.GetValue<string>("InterLineAirlines");
            if (string.IsNullOrEmpty(interLineAirlines)) return false;

            if (interLineAirlines.Contains(operatingCarrier.Trim().ToUpper()) || interLineAirlines.Contains(MarketingCarrier.Trim().ToUpper()))
                return true;

            return false;
        }

        public bool IsOperatedBySupportedAirlines(string operatingCarrier, string MarketingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier) && string.IsNullOrEmpty(MarketingCarrier)) return false;
            var operatedBySupportedAirlines = _configuration.GetValue<string>("SupportedCarriers");
            if (string.IsNullOrEmpty(operatedBySupportedAirlines)) return false;

            if (!string.IsNullOrEmpty(MarketingCarrier) && MarketingCarrier.Trim().ToUpper() == "UA"
                && operatedBySupportedAirlines.Contains(operatingCarrier.Trim().ToUpper()))
                return true;

            return false;
        }

        public bool IsOperatedByUA(string operatingCarrier, string MarketingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier) && string.IsNullOrEmpty(MarketingCarrier)) return false;
            var operatedByUA = _configuration.GetValue<string>("UnitedCarriers");
            if (string.IsNullOrEmpty(operatedByUA)) return false;

            if (!string.IsNullOrEmpty(MarketingCarrier) && MarketingCarrier.Trim().ToUpper() == "UA"
                && operatedByUA.Contains(operatingCarrier.Trim().ToUpper()))
                return true;

            return false;
        }

        public bool IsLandTransport(string equipmentType)
        {
            if (!string.IsNullOrEmpty(equipmentType) && (equipmentType.Contains("BUS") || equipmentType.Contains("TRN")))
            {
                return true;
            }
            return false;
        }

        public bool EnableOAMessageUpdate(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOAMsgUpdate") &&
            GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "Android_EnableOAMessageUpdate_AppVersion", "IPhone_EnableOAMessageUpdate_AppVersion", "", "", true, _configuration);
        }

        public bool EnableOAMsgUpdateFixViewRes(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableOAMsgUpdateFix") &&
            GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "Android_EnableOAMsgUpdateFixViewRes_AppVersion", "IPhone_EnableOAMsgUpdateFixViewRes_AppVersion", "", "", true, _configuration);
        }

        public bool IsOAReadOnlySeatMap(string operatingCarrier)
        {
            if (string.IsNullOrEmpty(operatingCarrier)) return false;
            var interLineAirlines = _configuration.GetValue<string>("ReadOnlySeatMapCarriers");
            if (string.IsNullOrEmpty(interLineAirlines)) return false;

            if (interLineAirlines.Contains(operatingCarrier.Trim().ToUpper()))
                return true;

            return false;
        }

        public bool IsOperatedByOtherAirlines(string operatingCarrier, string MarketingCarrier, string equipmentType)
        {
            if (!IsInterLine(operatingCarrier, MarketingCarrier) && !IsOperatedByUA(operatingCarrier, MarketingCarrier)
                && !IsOperatedBySupportedAirlines(operatingCarrier, MarketingCarrier) && !IsLandTransport(equipmentType))
            {
                return true;
            }
            return false;
        }

        public bool EnableShoppingcartPhase2ChangesWithVersionCheck(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableShoppingCartPhase2Changes")
                 && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "Android_EnableShoppingCartPhase2Changes_AppVersion", "iPhone_EnableShoppingCartPhase2Changes_AppVersion", "", "", true, _configuration);
        }

        public string BuildPaxTypeDescription(string paxTypeCode, string paxDescription, int paxCount)
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
        #region ReservationToShoppingCart_DataMigration
        public async Task<MOBShoppingCart> ReservationToShoppingCart_DataMigration(MOBSHOPReservation reservation, MOBShoppingCart shoppingCart, MOBRequest request)
        {
            try
            {
                bool isETCCertificatesExistInShoppingCartPersist = (_configuration.GetValue<bool>("MTETCToggle") &&
                                                                    shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates != null &&
                                                                    shoppingCart?.FormofPaymentDetails?.TravelCertificate?.Certificates.Count > 0);
                if (shoppingCart == null)
                    shoppingCart = new MOBShoppingCart();
                var formOfPaymentDetails = new MOBFormofPaymentDetails();
                shoppingCart.CartId = reservation.CartId;
                shoppingCart.PointofSale = reservation.PointOfSale;
                if (_configuration.GetValue<bool>("MTETCToggle"))
                    shoppingCart.IsMultipleTravelerEtcFeatureClientToggleEnabled = reservation.ShopReservationInfo2 != null ? reservation.ShopReservationInfo2.IsMultipleTravelerEtcFeatureClientToggleEnabled : false;
                formOfPaymentDetails.FormOfPaymentType = reservation.FormOfPaymentType.ToString();
                formOfPaymentDetails.PayPal = reservation.PayPal;
                formOfPaymentDetails.PayPalPayor = reservation.PayPalPayor;
                formOfPaymentDetails.MasterPassSessionDetails = reservation.MasterpassSessionDetails;
                formOfPaymentDetails.masterPass = reservation.Masterpass;
                formOfPaymentDetails.Uplift = reservation.ShopReservationInfo2?.Uplift;
                shoppingCart.SCTravelers = (reservation.TravelersCSL != null && reservation.TravelersCSL.Count() > 0) ? reservation.TravelersCSL : null;
                if (shoppingCart.SCTravelers != null && shoppingCart.SCTravelers.Any())
                {
                    shoppingCart.SCTravelers[0].SelectedSpecialNeeds = (reservation.TravelersCSL != null && reservation.TravelersCSL.Count() > 0) ? reservation.TravelersCSL[0].SelectedSpecialNeeds : null;
                    shoppingCart.SCTravelers[0].SelectedSpecialNeedMessages = (reservation.TravelersCSL != null && reservation.TravelersCSL.Count() > 0) ? reservation.TravelersCSL[0].SelectedSpecialNeedMessages : null;
                }
                if (shoppingCart.FormofPaymentDetails != null && shoppingCart.FormofPaymentDetails.SecondaryCreditCard != null)
                {
                    formOfPaymentDetails.CreditCard = shoppingCart.FormofPaymentDetails.CreditCard;
                    formOfPaymentDetails.SecondaryCreditCard = shoppingCart.FormofPaymentDetails.SecondaryCreditCard;
                }
                else
                {
                    formOfPaymentDetails.CreditCard = reservation.CreditCards?.Count() > 0 ? reservation.CreditCards[0] : null;
                }
                if (IncludeFFCResidual(request.Application.Id, request.Application.Version.Major) && shoppingCart.FormofPaymentDetails != null)
                {
                    formOfPaymentDetails.TravelFutureFlightCredit = shoppingCart.FormofPaymentDetails?.TravelFutureFlightCredit;
                    formOfPaymentDetails.FormOfPaymentType = shoppingCart.FormofPaymentDetails.FormOfPaymentType;
                }
                if (IncludeMoneyPlusMiles(request.Application.Id, request.Application.Version.Major) && shoppingCart.FormofPaymentDetails?.MoneyPlusMilesCredit != null)
                {
                    formOfPaymentDetails.MoneyPlusMilesCredit = shoppingCart.FormofPaymentDetails.MoneyPlusMilesCredit;
                }
                bool isTravelCredit = ConfigUtility.IncludeTravelCredit(request.Application.Id, request.Application.Version.Major);
                if (isTravelCredit)
                {
                    formOfPaymentDetails.TravelCreditDetails = shoppingCart.FormofPaymentDetails?.TravelCreditDetails;
                }

                if (ConfigUtility.IncludeTravelBankFOP(request.Application.Id, request.Application.Version.Major))
                {
                    formOfPaymentDetails.TravelBankDetails = shoppingCart.FormofPaymentDetails?.TravelBankDetails;
                    if (formOfPaymentDetails.TravelBankDetails?.TBApplied > 0)
                    {
                        _ffcShoppingcs.AssignIsOtherFOPRequired(formOfPaymentDetails, reservation.Prices);
                        shoppingCart.FormofPaymentDetails.IsOtherFOPRequired = formOfPaymentDetails.IsOtherFOPRequired;
                        shoppingCart.FormofPaymentDetails.FormOfPaymentType = formOfPaymentDetails.FormOfPaymentType;
                    }
                }
                await _ffcShoppingcs.AssignFFCValues(reservation.SessionId, shoppingCart, request, formOfPaymentDetails, reservation);
                shoppingCart.FormofPaymentDetails = formOfPaymentDetails;
                shoppingCart.FormofPaymentDetails.Phone = reservation.ReservationPhone;
                shoppingCart.FormofPaymentDetails.Email = reservation.ReservationEmail;
                shoppingCart.FormofPaymentDetails.EmailAddress = reservation.ReservationEmail != null ? reservation.ReservationEmail.EmailAddress : null;
                shoppingCart.FormofPaymentDetails.BillingAddress = reservation.CreditCardsAddress?.Count() > 0 ? reservation.CreditCardsAddress[0] : null;
                if (reservation.IsReshopChange)
                {

                    double changeFee = 0.0;
                    double grandTotal = 0.0;
                    if (reservation.Prices.Exists(price => price.DisplayType.ToUpper().Trim() == "CHANGEFEE"))
                        changeFee = reservation.Prices.First(price => price.DisplayType.ToUpper().Trim() == "CHANGEFEE").Value;

                    if (reservation.Prices.Exists(price => price.DisplayType.ToUpper().Trim() == "GRAND TOTAL"))
                        grandTotal = reservation.Prices.First(price => price.DisplayType.ToUpper().Trim() == "GRAND TOTAL").Value;

                    if (!reservation.AwardTravel)
                    {
                        if (grandTotal == 0.0 && reservation.Prices.Any())
                        {
                            grandTotal = (reservation.Prices != null && reservation.Prices.Count > 0) ? reservation.Prices.First(price => price.DisplayType.ToUpper().Trim() == "TOTAL").Value : grandTotal;
                        }
                    }
                    string totalDue = (grandTotal > changeFee ? (grandTotal - changeFee) : 0).ToString();
                    shoppingCart.TotalPrice = String.Format("{0:0.00}", totalDue);
                    shoppingCart.DisplayTotalPrice = totalDue;  //string.Format("${0:c}", totalDue); 
                }
                if (IsETCCombinabilityEnabled(request.Application.Id, request.Application.Version.Major) && shoppingCart.Flow == FlowType.BOOKING.ToString())
                {
                    await LoadandAddTravelCertificate(shoppingCart, reservation, isETCCertificatesExistInShoppingCartPersist);
                }
                else if (_configuration.GetValue<bool>("ETCToggle") && shoppingCart.Flow == FlowType.BOOKING.ToString())
                {
                    await LoadandAddTravelCertificate(shoppingCart, reservation.SessionId, reservation.Prices, isETCCertificatesExistInShoppingCartPersist, request.Application);
                }
                if (_configuration.GetValue<bool>("EnableETCBalanceAttentionMessageOnRTI") && !IsETCCombinabilityEnabled(request.Application.Id, request.Application.Version.Major))
                {
                    await AssignBalanceAttentionInfoWarningMessage(reservation.ShopReservationInfo2, shoppingCart.FormofPaymentDetails?.TravelCertificate);
                }
                if (isTravelCredit)
                {
                    await UpdateTCPriceAndFOPType(reservation.Prices, shoppingCart.FormofPaymentDetails, request.Application, shoppingCart.Products, shoppingCart.SCTravelers);
                }
                if (_configuration.GetValue<bool>("EnableCouponsforBooking") && shoppingCart.Flow == FlowType.BOOKING.ToString())
                {
                    await LoadandAddPromoCode(shoppingCart, reservation.SessionId, request.Application);
                }
                reservation.CartId = null;
                reservation.PointOfSale = null;
                reservation.PayPal = null;
                reservation.PayPalPayor = null;
                reservation.MasterpassSessionDetails = null;
                reservation.Masterpass = null;
                reservation.TravelersCSL = null;
                reservation.CreditCards2 = null;
                reservation.ReservationPhone2 = null;
                reservation.ReservationEmail2 = null;
                reservation.CreditCardsAddress = null;
                reservation.FOPOptions = null;

                if (_configuration.GetValue<bool>("EnableSelectDifferentFOPAtRTI"))
                {
                    if (!reservation.IsReshopChange)
                    {
                        //If ETC, ghost card, no saved cc presents and no due in reshop disable this button.
                        if (reservation.ShopReservationInfo2 != null && shoppingCart.FormofPaymentDetails != null)
                        {
                            if (((shoppingCart.FormofPaymentDetails.CreditCard != null && (reservation.ShopReservationInfo == null || !reservation.ShopReservationInfo.CanHideSelectFOPOptionsAndAddCreditCard)) ||
                                                        shoppingCart.FormofPaymentDetails.masterPass != null || shoppingCart.FormofPaymentDetails.PayPal != null || shoppingCart.FormofPaymentDetails.Uplift != null ||
                                                      (!string.IsNullOrEmpty(shoppingCart.FormofPaymentDetails.FormOfPaymentType) && shoppingCart.FormofPaymentDetails.FormOfPaymentType.ToUpper().Equals("APPLEPAY"))) && (shoppingCart.FormofPaymentDetails.TravelCertificate == null
                                                      || (shoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates == null || shoppingCart.FormofPaymentDetails?.TravelCertificate?.Certificates?.Count == 0)
                                                      ))
                            {
                                reservation.ShopReservationInfo2.ShowSelectDifferentFOPAtRTI = true;
                            }
                            else
                            {
                                reservation.ShopReservationInfo2.ShowSelectDifferentFOPAtRTI = false;
                            }
                        }
                    }
                }
                _ffcShoppingcs.AssignNullToETCAndFFCCertificates(shoppingCart.FormofPaymentDetails, request);
                if (IsEnableOmniCartMVP2Changes(request.Application.Id, request.Application.Version.Major, true) && !reservation.IsReshopChange)
                {
                    BuildOmniCart(shoppingCart, reservation);
                }

                if (_shoppingBuyMiles.IsBuyMilesFeatureEnabled(request.Application.Id, request.Application.Version.Major, null, isNotSelectTripCall: true))
                    _shoppingBuyMiles.UpdateGrandTotal(reservation, true);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }

            return shoppingCart;
        }
        private async Task LoadandAddPromoCode(MOBShoppingCart shoppingCart, string sessionId, MOBApplication application)
        {
            var persistedApplyPromoCodeResponse = new ApplyPromoCodeResponse();
            persistedApplyPromoCodeResponse = await _sessionHelperService.GetSession<ApplyPromoCodeResponse>(sessionId, persistedApplyPromoCodeResponse.ObjectName, new List<string> { sessionId, persistedApplyPromoCodeResponse.ObjectName }).ConfigureAwait(false);
            if (shoppingCart.PromoCodeDetails == null)
            {
                shoppingCart.PromoCodeDetails = new MOBPromoCodeDetails();
            }
            if (persistedApplyPromoCodeResponse != null)
            {
                UpdateShoppinCartWithCouponDetails(shoppingCart);
                persistedApplyPromoCodeResponse.ShoppingCart.PromoCodeDetails = shoppingCart.PromoCodeDetails;
                await _sessionHelperService.SaveSession<MOBShoppingCart>(shoppingCart, sessionId, new List<string> { sessionId, shoppingCart.ObjectName }, shoppingCart.ObjectName).ConfigureAwait(false);
                await _sessionHelperService.SaveSession<ApplyPromoCodeResponse>(persistedApplyPromoCodeResponse, sessionId, new List<string> { sessionId, persistedApplyPromoCodeResponse.ObjectName }, persistedApplyPromoCodeResponse.ObjectName).ConfigureAwait(false);
            }
            // DisablePromoOption(shoppingCart);
            IsHidePromoOption(shoppingCart);
        }
        private async Task UpdateTCPriceAndFOPType(List<MOBSHOPPrice> prices, MOBFormofPaymentDetails formofPaymentDetails, MOBApplication application, List<ProdDetail> products, List<MOBCPTraveler> travelers)
        {
            if (ConfigUtility.IncludeTravelCredit(application.Id, application.Version.Major))
            {
                _ffcShoppingcs.ApplyFFCToAncillary(products, application, formofPaymentDetails, prices);
                var price = prices.FirstOrDefault(p => p.DisplayType.ToUpper() == "CERTIFICATE" || p.DisplayType.ToUpper() == "FFC");
                if (price != null)
                {
                    formofPaymentDetails.TravelCreditDetails.AlertMessages = (formofPaymentDetails.TravelFutureFlightCredit?.FutureFlightCredits?.Count > 0 ?
                                                                              formofPaymentDetails.TravelFutureFlightCredit.ReviewFFCMessages :
                                                                              formofPaymentDetails.TravelCertificate.ReviewETCMessages.Where(m => m.HeadLine != "TravelCertificate_Combinability_ReviewETCAlertMsgs_OtherFopRequiredMessage").ToList());
                }
                else if (formofPaymentDetails.TravelCreditDetails != null)
                {
                    formofPaymentDetails.TravelCreditDetails.AlertMessages = null;
                }

                _ffcShoppingcs.UpdateTravelCreditAmountWithSelectedETCOrFFC(formofPaymentDetails, prices, travelers);
                try
                {
                    CSLContentMessagesResponse lstMessages = null;
                    string s = await _cachingService.GetCache<string>(_configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID") + "MOBCSLContentMessagesResponse", _headers.ContextValues.TransactionId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(s) && formofPaymentDetails.TravelCreditDetails != null)
                    {
                        lstMessages = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(s);
                        formofPaymentDetails.TravelCreditDetails.AlertMessages = _ffcShoppingcs.BuildReviewFFCHeaderMessage(formofPaymentDetails?.TravelFutureFlightCredit, travelers, lstMessages.Messages);
                    }
                }
                catch { }

                if (formofPaymentDetails?.FormOfPaymentType == "ETC" ||
                   formofPaymentDetails?.FormOfPaymentType == "FFC")
                    formofPaymentDetails.FormOfPaymentType = "TC";

            }
        }
        public void UpdateShoppinCartWithCouponDetails(MOBShoppingCart persistShoppingCart)
        {
            if (persistShoppingCart != null && persistShoppingCart.Products.Any())
            {
                persistShoppingCart.PromoCodeDetails = new MOBPromoCodeDetails();
                persistShoppingCart.PromoCodeDetails.PromoCodes = new List<MOBPromoCode>();
                persistShoppingCart.Products.ForEach(product =>
                {
                    if (product.CouponDetails != null && product.CouponDetails.Any())
                    {
                        product.CouponDetails.ForEach(CouponDetail =>
                        {
                            if (_configuration.GetValue<bool>("EnableFareandAncillaryPromoCodeChanges") ? !IsDuplicatePromoCode(persistShoppingCart.PromoCodeDetails.PromoCodes, CouponDetail.PromoCode) : true)
                            {
                                persistShoppingCart.PromoCodeDetails.PromoCodes
                                .Add(new MOBPromoCode
                                {
                                    PromoCode = CouponDetail.PromoCode,
                                    AlertMessage = CouponDetail.Description,
                                    IsSuccess = true,
                                    TermsandConditions = new MOBMobileCMSContentMessages
                                    {
                                        Title = _configuration.GetValue<string>("PromoCodeTermsandConditionsTitle"),
                                        HeadLine = _configuration.GetValue<string>("PromoCodeTermsandConditionsTitle")
                                    }
                                });
                            }
                        });
                    }
                });
            }
        }
        public void IsHidePromoOption(MOBShoppingCart shoppingCart)
        {
            bool isTravelCertificateAdded = shoppingCart?.FormofPaymentDetails?.TravelCertificate != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates != null && shoppingCart.FormofPaymentDetails.TravelCertificate.Certificates.Count > 0;
            bool isCouponAdded = (shoppingCart?.PromoCodeDetails?.PromoCodes != null && shoppingCart.PromoCodeDetails.PromoCodes.Count > 0);
            if (shoppingCart?.Products != null && shoppingCart.Products.Any(p => p?.Code?.ToUpper() == "FARELOCK" || p?.Code?.ToUpper() == "FLK"))
            {
                shoppingCart.PromoCodeDetails.IsHidePromoOption = true;
                return;
            }

            if (!isCouponAdded && (_configuration.GetValue<string>("Fops_HidePromoOption").Contains(shoppingCart?.FormofPaymentDetails?.FormOfPaymentType)
                || (_configuration.GetValue<string>("Fops_HidePromoOption").Contains("ETC") && isTravelCertificateAdded)))
            {
                shoppingCart.PromoCodeDetails.IsHidePromoOption = true;
            }
            else
            {
                shoppingCart.PromoCodeDetails.IsHidePromoOption = false;
            }
        }
        private bool IsDuplicatePromoCode(List<MOBPromoCode> promoCodes, string promoCode)
        {

            if (promoCodes != null && promoCodes.Any() && promoCodes.Count > 0)
            {
                if (promoCodes.Exists(c => c.PromoCode.Equals(promoCode)))
                {
                    return true;
                }
            }
            return false;
        }
        #endregion

        public bool IsEnableEditSearchOnFSRHeaderBooking(int applicationId, string appVersion, List<MOBItem> catalogItems = null)
        {
            return _configuration.GetValue<bool>("EnableEditSearchHeaderOnFSRBooking") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("Android_EnableEditSearchHeaderOnFSRBooking_AppVersion"), _configuration.GetValue<string>("iPhone_EnableEditSearchHeaderOnFSRBooking_AppVersion")) && (catalogItems != null && catalogItems.Count > 0 && catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableEditSearchOnFSRHeader).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableEditSearchOnFSRHeader).ToString())?.CurrentValue == "1");
        }

        public bool EnableEditSearchOnFSRHeaderBooking(MOBSHOPShopRequest request)
        {
            return request != null && !(request.IsReshop || request.IsReshopChange)
              && !request.IsCorporateBooking && string.IsNullOrEmpty(request.EmployeeDiscountId)
             && request.SearchType != "MD";
        }

        public bool EnableEditSearchOnFSRHeaderBooking(SelectTripRequest request, Session session)
        {
            return request != null && !session.IsReshopChange
              && !session.IsCorporateBooking && string.IsNullOrEmpty(session.EmployeeId);
        }

        public bool IsEnableTravelOptionsInViewRes(int applicationId, string appVersion, List<MOBItem> catalogItems)
        {
            return _configuration.GetValue<bool>("EnableTravelOptionsInViewRes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Android"), _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Iphone")) && catalogItems != null &&
                              catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableBundlesInManageRes).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableBundlesInManageRes).ToString())?.CurrentValue == "1";
        }
        public bool IsEnableTravelOptionsInViewRes(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableTravelOptionsInViewRes") && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Android"), _configuration.GetValue<string>("EnableTravelOptionsInViewRes_AppVersion_Iphone"));
        }

        public async Task<bool> IsEnableNewFilterRequestForMROffers()
        {
            if (_configuration.GetValue<bool>("EnableFeatureSettingsChanges"))
                return await _featureSettings.GetFeatureSettingValue("IsEnableNewFilterRequestForMROffers").ConfigureAwait(false);

            return false;
        }

        public async Task<bool> IsEnableIBEBuyOutViewRes(int applicationId, string appVersion, List<MOBItem> catalogItems = null)
        {

            return await IsEnableIBEBuyOutViewRes().ConfigureAwait(false) && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("IsEnableIBEBuyOutViewRes_AppVersion_Android"), _configuration.GetValue<string>("IsEnableIBEBuyOutViewRes_AppVersion_Iphone")) && (catalogItems != null && catalogItems.Count > 0 && catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableIBEBuyOutInViewRes).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableIBEBuyOutInViewRes).ToString())?.CurrentValue == "1");

        }
        public async Task<bool> IsEnableIBEBuyOutViewRes()
        {
            return await _featureSettings.GetFeatureSettingValue("IsEnableIBEBuyOutViewRes").ConfigureAwait(false);
        }
        public async Task<bool> IsEnableGenericMessageFeature(int applicationId, string appVersion, List<MOBItem> catalogItems = null)
        {
            return await IsEnableGenericMessageFeature().ConfigureAwait(false) && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("EnableGenericMessageFeature_AppVersion_Android"), _configuration.GetValue<string>("EnableGenericMessageFeature_AppVersion_Iphone")) && (catalogItems != null && catalogItems.Count > 0 && catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnablePOMGenericMessage).ToString() || a.Id == ((int)AndroidCatalogEnum.EnablePOMGenericMessage).ToString())?.CurrentValue == "1");
        }
        public async Task<bool> IsEnableGenericMessageFeature()
        {
            return await _featureSettings.GetFeatureSettingValue("EnableGenericMessageFeature").ConfigureAwait(false);
        }

        public async Task<bool> IsEnableCCEFeedBackIntrestedEventForE01OfferTile(United.Mobile.Model.MPRewards.MOBSeatChangeInitializeRequest request)
        {
            return await _featureSettings.GetFeatureSettingValue("IsEnableCCEFeedBackIntrestedEventForE01OfferTile").ConfigureAwait(false) && !string.IsNullOrEmpty(request.RequestType) && request.RequestType.Equals(SeatMapRequestType.E01.ToString(), StringComparison.OrdinalIgnoreCase) && GeneralHelper.IsApplicationVersionGreaterorEqual(request.Application.Id, request.Application.Version.Major, _configuration.GetValue<string>("IsEnableCCEFeedBackIntrestedEventForE01OfferTile_AppVersion_Android"), _configuration.GetValue<string>("IsEnableCCEFeedBackIntrestedEventForE01OfferTile_AppVersion_Iphone"));
        }

        public async Task<bool> IsEnableUpselltoUpsellInManageRes(int applicationId, string appVersion)
        {
            return await _featureSettings.GetFeatureSettingValue("IsEnableUpselltoUpsellInManageRes").ConfigureAwait(false) && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("IsEnableUpselltoUpsellInManageRes_AppVersion_Android"), _configuration.GetValue<string>("IsEnableUpselltoUpsellInManageRes_AppVersion_Iphone"));
        }

        /* DeadCode Removed
        public bool IsEnableXmlToCslSeatMapMigration(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("SwithToCSLSeatMapChangeSeats")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidXmlToCslSMapVersion", "iPhoneXmlToCslSMapVersion", "", "", true, _configuration);
            }
            return false;
        }

        public bool CheckEPlusSeatCode(string program)
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

        public bool EnableSSA(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableSSA") && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidSSAVersion", "iPhoneSSAVersion", "", "", true, _configuration);
        }

        public bool EnablePcuDeepLinkInSeatMap(int appId, string appVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && appId != -1)
            {
                return _configuration.GetValue<bool>("EnablePcuDeepLinkInSeatMap")
               && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPcuDeepLinkInSeatMapVersion", "iPhonePcuDeepLinkInSeatMapVersion", "", "", true, _configuration);
            }
            return false;
        }

        */
    }

    public static class Extension
    {
        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()?
                            .GetMember(enumValue.ToString())?
                            .First()?
                            .GetCustomAttribute<DisplayAttribute>()?
                            .Name;
        }

        // Convert the string to Pascal case.
        public static string ToPascalCase(this string the_string)
        {
            // If there are 0 or 1 characters, just return the string.
            if (the_string == null) return the_string;
            if (the_string.Length < 2) return the_string.ToUpper();

            // Split the string into words.
            string[] words = the_string.Split(
                new char[] { },
                StringSplitOptions.RemoveEmptyEntries);

            // Combine the words.
            string result = "";
            foreach (string word in words)
            {
                result +=
                    word.Substring(0, 1).ToUpper() +
                    word.Substring(1);
            }

            return result;
        }

    }

}
