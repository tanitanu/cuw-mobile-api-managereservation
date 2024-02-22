using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Profile;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.UpgradeCabin;
using United.Service.Presentation.FlightRequestModel;
using United.Service.Presentation.LoyaltyModel;
using United.Service.Presentation.LoyaltyRequestModel;
using United.Service.Presentation.LoyaltyResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using United.Utility.Http;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using MOBSHOPTax = United.Mobile.Model.SeatMapEngine.MOBSHOPTax;
//using Reservation = United.Mobile.Model.Shopping.Reservation;
using Session = United.Mobile.Model.Internal.Common.Session;
using Task = System.Threading.Tasks.Task;

namespace United.Mobile.UpgradeCabin.Domain
{
    public class UpgradeCabinBusiness : IUpgradeCabinBusiness
    {
        private readonly ICacheLog<UpgradeCabinBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDPService _tokenService;
        private readonly IMileagePlus _mileagePlus;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IProductOffers _shopping;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ManageResUtility _manageResUtility;
        private readonly IPKDispenserService _pKDispenserService;
        private static readonly string _websharetoken = "WebShareToken";
        private static readonly string _websessionshareurl = "WebSessionShareUrl";
        //PAX type
        private static List<string> _paxTypeCode = new List<string>() { "ADT", "SNR" };
        private static string _strChannel = "MOBILE";
        private static string _strUGC = "UGC";
        private static string _strCUG = "CUG";
        private static string _strMUA = "MUA";
        private static string _strPCU = "PCU";
        private static string _strMILES = "MILES";
        private static string _strMUAUPGRADE = "MUAUPGRADE";
        private static string _strPOINTS = "POINTS";
        private static string _strUSD = "USD";
        private static string _strAPDTaxCode = "APD";
        private static string _strAPDTaxDesc = "Airport Passenger Duty Fee";
        private static string _strADULTPaxType = "ADULT";
        private static string _strCHILDPaxType = "CHILD";
        private static List<string> _availableCabinTypes;
        private static List<string> _availablePriceTypes;
        //Plus Points variables
        private static string _strUCStatusAvailable = "AVAILABLE";
        private static string _strUCStatusWaitlist = "WAITLIST";
        private static string _strUCTypeUPPUpgrade = "UPPUPGRADE";
        private static string _strUCTypeFrontCabinUpgrade = "FRONTCABINUPGRADE";
        private static string _strUCTypeDoubleUpgrade = "DOUBLEUPGRADE";
        private static List<string> _CUtypeorder = new List<string>() { "UPPUPGRADE", "FRONTCABINUPGRADE", "DOUBLEUPGRADE" };
        private static List<string> _CUpointstypeorder = new List<string>() { "UGC", "CUG" };
        private static readonly string _UPGRADEMALL = "UPGRADEMALL";
        private static string _strDUELATERPOINTS = "DUELATERPOINTS";
        private static string _strDUENOWPOINTS = "DUENOWPOINTS";
        private static string _strTOTALPOINTS = "TOTALPOINTS";
        private static string _strTOTALMILES = "TOTALMILES";
        private static string _strTOTALPRICE = "TOTAL";
        private static string _strDUELATERPRICE = "DUELATERPRICE";
        private static string _strQTCHECKOUT = "CHECKOUT";
        private static string _strQTPOSTCONFIRMATION = "POST CONFIRMATION";
        private static bool _plusPointHideEvergreenMsg = false;
        private static bool _plusPointDoubleUpgradeAvailable;
        //Cabin Type Code
        private static string _strUPP2 = "UPP2";
        //private static string _strUP2 = "UP2";
        //Upgrade Cabin Status
        private static string _strUGCInsufficientPoints = "INSUFFICIENT POINTS";
        private static string _strSUCCESS_STATUS = "SUCCESS";
        private static string _strFAILURE_STATUS = "FAILURE";
        private static string _strINELIGIBLE_STATUS = "INELIGIBLE";
        private static Boolean _isUpgradeOptionAvailable = false;
        private readonly IHeaders _headers;
        private readonly IValidateHashPinService _validateHashPinService;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly IUpgradeEligibilityService _upgradeEligibilityService;
        private readonly IMPSignInCommonService _mPSignInCommonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureSettings _featureSettings;

        public UpgradeCabinBusiness(ICacheLog<UpgradeCabinBusiness> logger
            , IConfiguration configuration
            , IDPService tokenService
            , IMileagePlus mileagePlus, IShoppingUtility shoppingUtility
            , IDynamoDBService dynamoDBService, ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IProductOffers shopping, ISessionHelperService sessionHelperService
            , IPKDispenserService pKDispenserService
            , IHeaders headers,
            IValidateHashPinService validateHashPinService, IShoppingSessionHelper shoppingSessionHelper,
            IUpgradeEligibilityService upgradeEligibilityService
            , IMPSignInCommonService mPSignInCommonService
            , IHttpContextAccessor httpContextAccessor
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _tokenService = tokenService;
            _mileagePlus = mileagePlus;
            _shoppingUtility = shoppingUtility;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _shopping = shopping;
            _sessionHelperService = sessionHelperService;
            _pKDispenserService = pKDispenserService;
            _headers = headers;
            _validateHashPinService = validateHashPinService;
            _shoppingSessionHelper = shoppingSessionHelper;
            _upgradeEligibilityService = upgradeEligibilityService;
            _mPSignInCommonService = mPSignInCommonService;
            _httpContextAccessor = httpContextAccessor;
            _manageResUtility = new ManageResUtility(_configuration, _legalDocumentsForTitlesService, _dynamoDBService, _headers, _logger);
            _featureSettings = featureSettings;

        }
        private async Task<MOBUpgradeCabinEligibilityResponse> UpgradeCabinEligibleCheck(MOBUpgradeCabinEligibilityRequest request, bool isPrivate)
        {
            MOBUpgradeCabinEligibilityResponse response = new MOBUpgradeCabinEligibilityResponse();
            var cslRequest = CreateUpgradeCabinEligibleRequest(request);
            string actionName = "UpgradeCabinEligibleCheck";
            string transactionId = request.TransactionId;
            int applicationId = request.Application.Id;
            string appVersion = request.Application.Version.Major;
            string deviceId = request.DeviceId;
            _plusPointDoubleUpgradeAvailable = false;
            _plusPointHideEvergreenMsg = false;
            _isUpgradeOptionAvailable = false;
            _availableCabinTypes = new List<string>();

            //Get token 
            var token = (string.IsNullOrEmpty(request.Token)) ? await _tokenService.GetAnonymousToken(request.Application.Id, request.DeviceId, _configuration)
                : request.Token;

            string url = _configuration.GetValue<string>("upgradecabincslurl");
            var jsonRequest = JsonConvert.SerializeObject(cslRequest);
            var jsonResponse = await _upgradeEligibilityService.GetUpgradeCabinEligibleCheck(token, jsonRequest, request.SessionId, url).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var cslResponse = JsonConvert.DeserializeObject<United.Service.Presentation.LoyaltyResponseModel.UpgradeEligibilityResponse>(jsonResponse);

                if (cslResponse != null && cslResponse.ServiceStatus != null)
                {
                    if (string.Equals(cslResponse.ServiceStatus.StatusType, _strSUCCESS_STATUS, StringComparison.OrdinalIgnoreCase))
                    {
                        response.IsEligible = true;
                        response = await Task.Run(() => CreateCSLUpgradeCabinEligibilityResponse(response, cslResponse, request));
                        if (!_isUpgradeOptionAvailable) response.IsEligible = false;
                    }

                    if (response.IsEligible)
                    {
                        if (!string.IsNullOrEmpty(request.MileagePlusNumber))
                        {
                            //TODO : to be removed
                            //request.HashPinCode = "";
                            if (!string.IsNullOrEmpty(request.HashPinCode))
                            {
                                response.PlusPoints = await Task.Run(() => GetPlusPointsDetails(request));
                            }

                            cslResponse.Reservation.Sponsor = new Service.Presentation.PersonModel.LoyaltyPerson
                            { LoyaltyProgramMemberID = request.MileagePlusNumber, LoyaltyProgramCarrierCode = "UA" };
                        }
                        await _sessionHelperService.SaveSession(cslResponse, request.SessionId, new List<string> { request.SessionId, cslResponse.GetType().FullName }, cslResponse.GetType().FullName).ConfigureAwait(false);

                        CheckPlusPointsExpiryStatus(response.PlusPoints, cslResponse.Reservation.FlightSegments, response);
                        await SetEligibilityLearnAboutContentsAsync(response);
                        await SetEligibilityMessageContentsAsync(response);

                    }
                }
            }
            return response;
        }

        public async Task<MOBUpgradeCabinEligibilityResponse> UpgradeCabinEligibleCheck(MOBUpgradeCabinEligibilityRequest request)
        {
            var response = new MOBUpgradeCabinEligibilityResponse();

            var session = new Model.Common.Session();
            string loggingId = (string.IsNullOrEmpty(request.SessionId)) ? request.TransactionId : request.SessionId;
            if (string.IsNullOrEmpty(request.SessionId))
                session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, string.Empty);

            else
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);


            request.SessionId = session.SessionId;
            request.Token = session.Token;
            request.FlowType = _UPGRADEMALL;


            if (string.IsNullOrEmpty(request.SessionId) || string.IsNullOrEmpty(request.RecordLocator) || string.IsNullOrEmpty(request.LastName))

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("upgradecabinineligiblesvcerror"));

            response = await Task.Run(() => UpgradeCabinEligibleCheck(request, true));
            if (response.IsEligible)
            {

                response.RedirectUrl = string.Format(Convert.ToString(_configuration.GetValue<string>("upgradecabinredirectdotcomurl"))
                    + "/{0}/{1}", HttpUtility.UrlEncode(EncryptString(request.RecordLocator).Replace("/", "~~")),
                    HttpUtility.UrlEncode(EncryptString(request.LastName).Replace("/", "~~")));


                if (!string.IsNullOrEmpty(request.MileagePlusNumber) && !string.IsNullOrEmpty(request.HashPinCode))
                {

                    if (_configuration.GetValue<bool>("EnableUpgradeCabinWebSSORedirect"))
                    {
                        var ssoinfo = await Task.Run(() => CreateSSOInformation(request, request.MileagePlusNumber, request.HashPinCode, request.SessionId));

                        if (ssoinfo != null && ssoinfo.Any())
                        {
                            response.WebShareToken =
                                ssoinfo.FirstOrDefault(x => string.Equals(x.Id, _websharetoken, StringComparison.OrdinalIgnoreCase))?.CurrentValue;
                            response.WebSessionShareUrl =
                                ssoinfo.FirstOrDefault(x => string.Equals(x.Id, _websessionshareurl, StringComparison.OrdinalIgnoreCase))?.CurrentValue;

                            if (await _featureSettings.GetFeatureSettingValue("EnableRedirectURLUpdate").ConfigureAwait(false))
                            {
                                response.RedirectUrl = $"{_configuration.GetValue<string>("NewDotcomSSOUrl")}?type=sso&token={response.WebShareToken}&landingUrl={response.RedirectUrl}";
                                response.WebSessionShareUrl = response.WebShareToken = string.Empty;
                            }
                        }
                                              
                    }
                }
            }
            else
            {
                response.Exception = new MOBException("9999", Convert.ToString(_configuration.GetValue<string>("upgradecabinineligiblemsg")));
            }

            response.TransactionId = request.TransactionId;
            response.LanguageCode = request.LanguageCode;
            response.SessionId = request.SessionId;
            response.TransactionId = request.TransactionId;

            await _sessionHelperService.SaveSession(response, request.SessionId, new List<string> { request.SessionId, response.ObjectName }, response.ObjectName).ConfigureAwait(false);

            return response;
        }

        private static UpgradeEligibilityRequest CreateUpgradeCabinEligibleRequest(MOBUpgradeCabinEligibilityRequest request)
        {
            string channelID = "1201";
            string channelName = "MYRES";
            string currencyCode = "USD";
            string productCode = "UPG";
            string carrierCode = "UA";
            string doubleupgradeKey = "DoubleUpgrade";
            string confirmablecombinations = "ConfirmableCombinations";

            UpgradeEligibilityRequest upgradeeligibilityrequest = new UpgradeEligibilityRequest();
            upgradeeligibilityrequest.ServiceClient
                = new Service.Presentation.CommonModel.ServiceClient
                {
                    Requestor = new Service.Presentation.CommonModel.Requestor
                    {
                        ChannelID = channelID,
                        ChannelName = channelName,
                        LanguageCode = request.LanguageCode
                    }
                };
            upgradeeligibilityrequest.Characteristics = new Collection<Characteristic> {
                new Characteristic{ Code = doubleupgradeKey, Value = bool.TrueString },
                new Characteristic{ Code = confirmablecombinations, Value = bool.TrueString },
            };
            upgradeeligibilityrequest.CurrencyCode = currencyCode;
            upgradeeligibilityrequest.Filters
                = new System.Collections.ObjectModel.Collection<Service.Presentation.ProductRequestModel.ProductFilter>
                {
                    new Service.Presentation.ProductRequestModel.ProductFilter { ProductCode = productCode }
                };
            upgradeeligibilityrequest.ReservationReferences
                = new System.Collections.ObjectModel.Collection<Service.Presentation.ProductModel.ReservationReference>
                {
                    new Service.Presentation.ProductModel.ReservationReference { ID = request.RecordLocator,
                    Travelers = new Collection<Service.Presentation.ProductModel.ProductTraveler>{
                        new Service.Presentation.ProductModel.ProductTraveler{ Surname = request.LastName }  } }
                };

            if (string.IsNullOrEmpty(request.MileagePlusNumber) == false)
            {
                upgradeeligibilityrequest.SponsorProfile
                    = new Service.Presentation.CommonModel.LoyaltyProgramProfile
                    {
                        LoyaltyProgramCarrierCode = carrierCode,
                        LoyaltyProgramMemberID = request.MileagePlusNumber
                    };
            }

            return upgradeeligibilityrequest;
        }

        private async Task<MOBUpgradeCabinEligibilityResponse> CreateCSLUpgradeCabinEligibilityResponse(MOBUpgradeCabinEligibilityResponse response, UpgradeEligibilityResponse cslResponse, MOBUpgradeCabinEligibilityRequest request)
        {
            var session = new Session { SessionId = request.SessionId, Token = request.Token };
            var shopping = new Shopping();

            response.Trips = await Task.Run(() => GetUpgradeCabinTrips(cslResponse.Reservation.FlightSegments));

            response.Segments = await Task.Run(() => GetUpgradeCabinSegments(request, cslResponse, response));

            response.MilesUpgradeOption = await Task.Run(() => GetMilesUpgradeOption(cslResponse, response));

            response.Passengers = await Task.Run(() => GetUpgradeCabinPassangers(cslResponse.Reservation.Travelers));

            return response;
        }

        public async Task<MOBPlusPoints> GetPlusPointsDetails(MOBUpgradeCabinEligibilityRequest request)
        {
            try
            {

                var pluspointsrequest = new MPAccountValidationRequest();
                pluspointsrequest.SessionId = request.SessionId;
                pluspointsrequest.HashValue = request.HashPinCode;
                pluspointsrequest.MileagePlusNumber = request.MileagePlusNumber;
                pluspointsrequest.Application = request.Application;
                pluspointsrequest.DeviceId = request.DeviceId;
                pluspointsrequest.TransactionId = request.TransactionId;
                return await _mileagePlus.GetPlusPointsFromLoyaltyBalanceService(pluspointsrequest, request.Token);
            }
            catch { return null; }
        }

        public List<MOBUpgradeCabinSegment> GetUpgradeCabinSegments(MOBUpgradeCabinEligibilityRequest request, UpgradeEligibilityResponse cslresponse, MOBUpgradeCabinEligibilityResponse response)
        {
            //TODO null check
            List<MOBUpgradeCabinSegment> segments = new List<MOBUpgradeCabinSegment>();

            var flightsegments = cslresponse.Reservation.FlightSegments;

            var travelers = cslresponse.Reservation.Travelers;

            if (flightsegments == null || !flightsegments.Any()) return null;

            var confirmedActionCodes = _configuration.GetValue<string>("flightSegmentTypeCode");

            flightsegments.ToList().ForEach(cslseg =>
            {
                if (confirmedActionCodes.IndexOf
                        (cslseg.FlightSegment.FlightSegmentType.Substring(0, 2), StringComparison.Ordinal) != -1)
                {
                    MOBUpgradeCabinSegment segment = new MOBUpgradeCabinSegment
                    {
                        SegmentNumber = Convert.ToString(cslseg.SegmentNumber),
                        TripNumber = cslseg.TripNumber,

                        ScheduledArrivalDateTime = cslseg.EstimatedArrivalTime,
                        ScheduledDepartureDateTime = cslseg.EstimatedDepartureTime
                    };

                    if (cslseg.FlightSegment != null)
                    {

                        segment.Waitlisted = GetWaitlistUpgradeType(cslseg.FlightSegment.UpgradeVisibilityType, cslseg.FlightSegment.UpgradeEligibilityStatus);

                        if (cslseg.FlightSegment.ArrivalAirport != null)
                        {
                            segment.Arrival = new MOBAirport
                            {
                                City = cslseg.FlightSegment.ArrivalAirport.IATACode,
                                Code = cslseg.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode,
                                Name = cslseg.FlightSegment.ArrivalAirport.Name
                            };
                        }

                        if (cslseg.FlightSegment.DepartureAirport != null)
                        {
                            segment.Departure = new MOBAirport
                            {
                                City = cslseg.FlightSegment.DepartureAirport.IATACode,
                                Code = cslseg.FlightSegment.DepartureAirport.IATACountryCode.CountryCode,
                                Name = cslseg.FlightSegment.DepartureAirport.Name
                            };
                        }

                        segment.Aircraft = new MOBAircraft
                        {
                            Code = cslseg.FlightSegment.Equipment != null
                            ? cslseg.FlightSegment.Equipment.Model.Fleet : string.Empty,

                            LongName = cslseg.FlightSegment.Equipment != null
                            ? cslseg.FlightSegment.Equipment.Model.Description : string.Empty,

                            ShortName = cslseg.FlightSegment.Equipment != null
                            ? cslseg.FlightSegment.Equipment.Model.STIAircraftType : string.Empty,
                        };

                        segment.FlightTime = GetTravelTime(request.Application.Version.Major, cslseg.FlightSegment.JourneyDuration);

                        segment.OperatingAirlineCode = cslseg.FlightSegment.OperatingAirlineCode;
                        segment.OperatingAirlineFlightNumber = cslseg.FlightSegment.OperatingAirlineFlightNumber;
                        segment.OperatingAirlineName = cslseg.FlightSegment.OperatingAirlineFlightNumber;

                        segment.FlightNumber = cslseg.FlightSegment.FlightNumber;
                        segment.OperatingAirlineName = cslseg.FlightSegment.OperatingAirlineName;
                    }

                    DateTime arrivaldate;
                    DateTime.TryParse(cslseg.EstimatedArrivalTime, out arrivaldate);
                    segment.FormattedScheduledArrivalDateTime = cslseg.EstimatedArrivalTime;
                    segment.FormattedScheduledArrivalDate = arrivaldate.ToShortDateString();

                    DateTime departuredate;
                    DateTime.TryParse(cslseg.EstimatedDepartureTime, out departuredate);
                    segment.FormattedScheduledDepartureDateTime = cslseg.EstimatedDepartureTime;
                    segment.FormattedScheduledDepartureDate = departuredate.ToShortDateString();

                    segment.ClassOfService = cslseg?.BookingClass?.Code;
                    segment.ClassType = cslseg?.BookingClass?.Cabin?.Name;
                    segment.ClassOfServiceDescription = cslseg?.BookingClass?.Cabin?.Description;

                    //PRICE OPTION
                    segment.Prices = GetSegmentUpgradeOptions(cslresponse, segment.TripNumber, segment.SegmentNumber, response, new List<string> { "PCU" });
                    //Order By Cabin Type
                    if (segment.Prices != null && segment.Prices.Any())
                    {
                        segment.Prices = segment.Prices.OrderBy(d => _CUtypeorder.IndexOf(d.CabinUpgradeTypeDesc)).ToList();
                    }

                    //MILES OPTION
                    segment.Miles = GetSegmentUpgradeOptions(cslresponse, segment.TripNumber, segment.SegmentNumber, response, new List<string> { "MUA" });
                    //Order By Cabin Type
                    if (segment.Miles != null && segment.Miles.Any())
                    {
                        segment.Miles = segment.Miles.OrderBy(d => _CUtypeorder.IndexOf(d.CabinUpgradeTypeDesc)).ToList();
                    }

                    //POINTS OPTION
                    segment.Points = GetSegmentUpgradeOptions(cslresponse, segment.TripNumber, segment.SegmentNumber, response, new List<string> { "UGC", "CUG" });
                    //Order By Cabin Type
                    if (segment.Points != null && segment.Points.Any())
                    {
                        segment.Points = segment.Points.OrderBy(d => _CUtypeorder.IndexOf(d.CabinUpgradeTypeDesc)).ToList();
                        segment.Points = segment.Points.OrderBy(d => _CUpointstypeorder.IndexOf(d.UpgradeType)).ToList();
                    }
                    segments.Add(segment);
                }
            });

            ShowOrHidePlusPointsEvergreenMsg(segments);

            return segments;
        }

        public static string GetTravelTime(string appVersion, TimeSpan journeyduration)
        {
            string flighttime = string.Empty;

            if (journeyduration.Hours > 0)
            {
                if (!string.IsNullOrEmpty(appVersion) && appVersion.ToUpper().Equals("2.1.8I"))
                {
                    flighttime = "0 HR " + journeyduration.Hours;
                }
                else if (journeyduration.Hours > 0)
                {
                    flighttime = journeyduration.Hours + " HR";
                }
            }
            if (journeyduration.Minutes > 0)
            {
                if (!string.IsNullOrEmpty(appVersion) && appVersion.ToUpper().Equals("2.1.8I"))
                {
                    flighttime = flighttime + " " + journeyduration.Minutes + " 0 MN";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(flighttime))
                    {
                        flighttime = journeyduration.Minutes + " MN";
                    }
                    else
                    {
                        flighttime = flighttime + " " + journeyduration.Minutes + " MN";
                    }
                }
            }
            return flighttime;
        }

        private static string GetWaitlistUpgradeType(Service.Presentation.CommonEnumModel.UpgradeVisibilityType upgradevisibility, Service.Presentation.CommonEnumModel.UpgradeEligibilityStatus eligibilitystatus)
        {
            if (eligibilitystatus == Service.Presentation.CommonEnumModel.UpgradeEligibilityStatus.Requested)
            {
                if (upgradevisibility == Service.Presentation.CommonEnumModel.UpgradeVisibilityType.MileagePlusUpgradeAwards)
                    return _strMILES;
                else if (upgradevisibility == Service.Presentation.CommonEnumModel.UpgradeVisibilityType.PlusPointsUpgrade)
                    return _strPOINTS;
            }
            return string.Empty;
        }

        public static void ShowOrHidePlusPointsEvergreenMsg(List<MOBUpgradeCabinSegment> segments)
        {
            int count = 0;
            if (segments != null && segments.Any())
            {
                segments.ForEach(x =>
                {
                    if (x.Prices != null && x.Prices.Any() && x.Prices.Count > 0) count++;
                });
                if (count == 1 && !_plusPointDoubleUpgradeAvailable) _plusPointHideEvergreenMsg = true;
            }
        }

        public static List<MOBUpgradeOption> GetSegmentUpgradeOptions(UpgradeEligibilityResponse cslresponse, string tripnumber, string segmentnumber, MOBUpgradeCabinEligibilityResponse response, List<string> upgradetypes)
        {
            //TODO null check
            List<MOBUpgradeOption> upgradeoptions = new List<MOBUpgradeOption>();
            List<MOBUpgradePriceOption> upgradepriceoptions;

            var selectedtripODOOption = GetSelectedTripODOption(cslresponse, tripnumber);

            if (selectedtripODOOption == null) return null;

            var selectedSegmentODOOption = selectedtripODOOption.FlightSegments.FirstOrDefault
                (x => string.Equals(Convert.ToString(x.SegmentNumber), segmentnumber, StringComparison.OrdinalIgnoreCase));

            if (selectedSegmentODOOption == null
                || selectedSegmentODOOption.UpgradeOptions == null
                || !selectedSegmentODOOption.UpgradeOptions.Any()) return null;

            var availableUpgrade
                = selectedSegmentODOOption.UpgradeOptions.Where(x => upgradetypes.Contains(x.UpgradeType));

            if (upgradetypes.Contains("MUX"))
            {
                availableUpgrade = selectedtripODOOption.UpgradeOptions;
            }

            if (availableUpgrade == null && !availableUpgrade.Any())
            { _isUpgradeOptionAvailable = _isUpgradeOptionAvailable || false; return null; }

            availableUpgrade.ToList().ForEach(option =>
            {
                _isUpgradeOptionAvailable = true;

                MOBUpgradeOption upgradeoption = new MOBUpgradeOption();

                upgradeoption.UpgradeType = option.UpgradeType;
                upgradeoption.AvailableSeatCount = option.AvailableSeatCount;

                upgradeoption.CabinUpgradeTypeDesc
                = (!string.IsNullOrEmpty(option.CabinUpgradeType)) ? option.CabinUpgradeType.ToUpper() : string.Empty;

                upgradeoption.UpgradeStatus = option.UpgradeStatus;

                if (string.Equals(option.UpgradeType, _strUGC, StringComparison.OrdinalIgnoreCase))
                    CheckIfDoubleUpgradeCabinAvailable(option.UpgradeStatus);

                upgradeoption.UpgradeCabinTypes = GetUpgradeCabinTypes(option, selectedSegmentODOOption, segmentnumber);

                if (upgradeoption.UpgradeCabinTypes != null && upgradeoption.UpgradeCabinTypes.Any())
                {
                    if (string.Equals(_strUCTypeDoubleUpgrade,
                        option.CabinUpgradeType, StringComparison.OrdinalIgnoreCase))
                    {
                        upgradeoption = CreateDoubleUpgradeTooltip(option, upgradeoption, tripnumber, response,
                        selectedtripODOOption.UpgradeOptions, selectedSegmentODOOption.UpgradeOptions);
                    }
                }

                upgradeoption.Id = option.Id;
                upgradeoption.SegmentRefId = segmentnumber;
                upgradeoption.TripRefId = tripnumber;

                option.PriceOptions.ToList().ForEach(priceoption =>
                {

                    MOBUpgradePriceOption upgradepriceoption;
                    upgradepriceoptions = new List<MOBUpgradePriceOption>();

                    if (priceoption.Copay != null)
                    {
                        upgradepriceoption = new MOBUpgradePriceOption
                        {
                            Type = priceoption.Copay.Currency.Code,
                            Value = Convert.ToString(priceoption.Copay.Amount),
                            EDDCode = priceoption.EDDCode
                        };
                        upgradepriceoptions.Add(upgradepriceoption);
                    }
                    if (priceoption.Instrument != null)
                    {
                        upgradepriceoption = new MOBUpgradePriceOption
                        {
                            RewardCode = priceoption.RewardCode,
                            Type = priceoption.Instrument.FirstOrDefault().Type,
                            Value = Convert.ToString(priceoption.Instrument.FirstOrDefault().Value)
                        };
                        upgradepriceoptions.Add(upgradepriceoption);
                    }

                    upgradeoption.PriceOption = upgradepriceoptions;
                });

                if (option.Taxes != null && option.Taxes.Any())
                {
                    upgradeoption.Taxes = GetUpgradeCabinAPDTaxdata(cslresponse, option);
                }

                upgradeoptions.Add(upgradeoption);

            });

            return upgradeoptions;
        }


        private static UpgradeOriginDestinationOption GetSelectedTripODOption(UpgradeEligibilityResponse cslresponse, string tripnumber)
        {
            var upgradeSolutions = cslresponse.Solutions;

            if (upgradeSolutions == null || !upgradeSolutions.Any()) return null;

            var selectedODOptions = upgradeSolutions.FirstOrDefault().ODOptions;

            if (selectedODOptions == null && !selectedODOptions.Any()) return null;

            var selectedtripODOOption = selectedODOptions.FirstOrDefault
                (x => string.Equals(x.ID, tripnumber, StringComparison.OrdinalIgnoreCase));

            return selectedtripODOOption;
        }

        public static void CheckIfDoubleUpgradeCabinAvailable(string cabintype)
        {
            if (!_plusPointDoubleUpgradeAvailable)
                _plusPointDoubleUpgradeAvailable = (string.Equals
                    (_strUCTypeDoubleUpgrade, cabintype, StringComparison.OrdinalIgnoreCase));
        }

        private static MOBUpgradeOption CreateDoubleUpgradeTooltip(UpgradeOption option, MOBUpgradeOption upgradeoption, string tripnumber,
          MOBUpgradeCabinEligibilityResponse response, Collection<UpgradeOption> tripupgradeoptions,
          Collection<UpgradeOption> segupgradeoptions)
        {
            StringBuilder sb = new StringBuilder();
            Boolean isDoubleUpgradeMixed = false;
            string strcabinstr1 = string.Empty;
            string strcabinstr2 = string.Empty;

            var doubleupgradeitems
                = upgradeoption.UpgradeCabinTypes.GroupBy(x => x.UpgradeStatus).Select(x => x.First());

            isDoubleUpgradeMixed = (doubleupgradeitems.Count() == 2);

            if (option != null)
            {
                if (string.Equals(_strUGC, option.UpgradeType, StringComparison.OrdinalIgnoreCase))
                {
                    var pointsobj = option.PriceOptions?.FirstOrDefault()?.Instrument?.FirstOrDefault();
                    var copayobj = option.PriceOptions?.FirstOrDefault()?.Copay;

                    var doubleupgradeitem
                            = upgradeoption.UpgradeCabinTypes.Select(x => x.UpgradeTypeDesc).ToList<string>();

                    if (isDoubleUpgradeMixed)
                    {
                        strcabinstr2 = string.Format("today to waitlist for {0}. If {0} doesn't clear", doubleupgradeitem[0]);
                        strcabinstr1 = Convert.ToString(pointsobj.Value);
                    }
                    else
                    {
                        if (doubleupgradeitem.Count >= 2)
                        {
                            strcabinstr2 = string.Format("when we confirm either upgrade for {0}® or {1}℠. If only {1}℠ clears",
                                doubleupgradeitem[0], doubleupgradeitem[1]);
                        }
                        strcabinstr1 = Convert.ToString(pointsobj.Value);
                    }
                    sb.Append(string.Format("&lt;p&gt; We'll charge you &lt;b&gt;{0} PlusPoints per person&lt;/b&gt; {1}, " +
                    "you'll receive a refund after your flight for the difference, if applicable.&lt;/p&gt;", strcabinstr1, strcabinstr2));
                } //_strUGC

                if (string.Equals(_strMUA, option.UpgradeType, StringComparison.OrdinalIgnoreCase))
                {
                    string strUPBCabin = string.Empty;
                    string strUPBPrice = string.Empty;
                    string strUPPCabin = string.Empty;
                    string strUPPPrice = string.Empty;

                    upgradeoption.UpgradeCabinTypes?.ForEach(cabinref =>
                    {
                        var priceobj = tripupgradeoptions?.Where
                                (x => string.Equals(cabinref.Id, x.Id, StringComparison.OrdinalIgnoreCase)
                                && string.Equals(cabinref.UpgradeStatus, x.UpgradeStatus, StringComparison.OrdinalIgnoreCase))
                                .Select(x => x.PriceOptions)?.FirstOrDefault();

                        priceobj = (priceobj == null) ? segupgradeoptions?.Where
                                (x => string.Equals(cabinref.Id, x.Id, StringComparison.OrdinalIgnoreCase)
                                && string.Equals(cabinref.UpgradeStatus, x.UpgradeStatus, StringComparison.OrdinalIgnoreCase))
                                .Select(x => x.PriceOptions)?.FirstOrDefault() : priceobj;

                        if (priceobj != null)
                        {
                            var milesobj = priceobj?.FirstOrDefault()?.Instrument?.FirstOrDefault();
                            var copayobj = priceobj?.FirstOrDefault()?.Copay;
                            string formattedmiles = FormatMiles(Convert.ToInt32(milesobj.Value));

                            if (string.Equals(cabinref.UpgradeType, _strUCTypeFrontCabinUpgrade,
                                StringComparison.OrdinalIgnoreCase))
                            {
                                strUPBCabin = cabinref.UpgradeTypeDesc;
                                strUPBPrice = string.Format("{0} miles + ${1}", formattedmiles, copayobj.Amount);
                            }
                            else if (string.Equals(cabinref.UpgradeType, _strUCTypeUPPUpgrade,
                                StringComparison.OrdinalIgnoreCase))
                            {
                                strUPPCabin = cabinref.UpgradeTypeDesc;
                                strUPPPrice = string.Format("{0} miles + ${1}", formattedmiles, copayobj.Amount);
                            }
                        }
                    });

                    if (isDoubleUpgradeMixed)
                    {
                        strcabinstr1 = string.Format
                            ("{0}®. If {0}® doesn't clear,", strUPBCabin);
                    }
                    else
                    {
                        strcabinstr1 = string.Format
                            ("both {0}® and {1}℠. If neither upgrade clears, " +
                            "we'll refund the full amount of {2} per person. If only {1}℠ clears,",
                            strUPBCabin, strUPPCabin, strUPBPrice);
                    }

                    sb.Append(string.Format("&lt;p&gt; We'll charge you &lt;b&gt; {0} per person &lt;/b&gt; today to waitlist for " +
                        "{1} you'll receive a refund after your flight for the full amount we charged today, and then we'll charge &lt;b&gt; " +
                        "{2} per person &lt;/b&gt; for the {3} upgrade.&lt;/p&gt;"
                        , strUPBPrice, strcabinstr1, strUPPPrice, strUPPCabin));

                    string triporigindestination = string.Empty;
                    var selectedtrip = response.Trips?.Where
                        (x => x.Index == Convert.ToInt32(tripnumber))?.FirstOrDefault();

                    if (selectedtrip != null)
                    {
                        triporigindestination =
                            string.Format("{0} - {1}", selectedtrip.Origin, selectedtrip.Destination);
                    }

                    var milesdoubleupgrade = new MOBUpgradeCabinAdvisory
                    {
                        AdvisoryType = UpgradeCabinAdvisoryType.INFORMATION,
                        ContentType = UpgradeCabinContentType.NONE,
                        ShouldExpand = true,
                        Header = string.Format("{0} | {1}, {2}", triporigindestination, strUPBCabin, strUPPCabin),
                        Body = string.Format("&lt;p&gt; You've chosen to be eligible for {0} and {1} for the flight(s) shown. " +
                        "If only {1} clears, you'll receive a refund after your flight for the full amount we charged today, " +
                        "and then we'll charge &lt;b&gt;{2} per person &lt;/b&gt; for the {1} upgrade.&lt;/p&gt;",
                        strUPBCabin, strUPPCabin, strUPPPrice),
                    };
                    upgradeoption.Messages = (upgradeoption.Messages == null) ?
                        new List<MOBUpgradeCabinAdvisory>() : upgradeoption.Messages;
                    upgradeoption.Messages.Add(milesdoubleupgrade);

                } //_strMUA 
            }
            upgradeoption.DoubleUpgradeTooltip = sb.IsNullOrEmpty() ? string.Empty : sb.ToString();
            return upgradeoption;
        }

        public static string FormatMiles(Int32 num)
        {
            if (num >= 1000)
                return (num / 1000) + "k";
            return Convert.ToString(num);
        }


        public static List<MOBUpgradeCabinTypeDesc> GetUpgradeCabinTypes(UpgradeOption option, UpgradeEligibilitySegment eligibilitySegment, string segmentid)
        {
            if (option == null || option.SegmentMapping == null
                || option.SegmentMapping.SegmentReferences == null
                || !option.SegmentMapping.SegmentReferences.Any()) return null;

            var equipment = eligibilitySegment.Equipment;

            var cabintypes = new List<MOBUpgradeCabinTypeDesc>();
            if (string.Equals(option.UpgradeType, _strPCU, StringComparison.OrdinalIgnoreCase))
            {
                var cabintype = new MOBUpgradeCabinTypeDesc
                {
                    UpgradeType = option.CabinUpgradeType.ToUpper(),
                    UpgradeTypeDesc = GetCabinUpgradeTypeDesc(equipment, option.CabinUpgradeType),
                    UpgradeStatus = option.UpgradeStatus,
                    Id = option.Id,
                    AvailableSeatCount = option.AvailableSeatCount,
                    AvailableSeatMsg = GetAvailableSeatBannerMsg(option),
                    SegmentNumber = segmentid
                };
                cabintypes.Add(cabintype);
            }
            else
            {
                option.SegmentMapping.SegmentReferences.ForEach(type =>
                {
                    var cabintype = new MOBUpgradeCabinTypeDesc
                    {
                        UpgradeType = type.CabinUpgradeType.ToUpper(),
                        UpgradeTypeDesc = GetCabinUpgradeTypeDesc(equipment, type.CabinUpgradeType),
                        UpgradeStatus = type.UpgradeStatus,
                        Id = type.UpgradeOptionId,
                        AvailableSeatCount = option.AvailableSeatCount,
                        AvailableSeatMsg = GetAvailableSeatBannerMsg(option),
                        SegmentNumber = type.SegmentRefID
                    };
                    cabintypes.Add(cabintype);
                });
            }
            return (cabintypes != null && cabintypes.Any())
                ? cabintypes : null;
        }

        public static string GetCabinUpgradeTypeDesc(United.Service.Presentation.CommonModel.AircraftModel.Aircraft equipment, string cabinupgradetype)
        {
            if (equipment == null || equipment.Cabins == null
                || !equipment.Cabins.Any() || string.IsNullOrEmpty(cabinupgradetype)) return string.Empty;
            try
            {
                var cabin = equipment.Cabins.Where
                    (x => string.Equals(x.Status, cabinupgradetype, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
                if (cabin != null) SetAvailableCabinTypes(cabin.Key);
                return (cabin != null) ? cabin.Description : string.Empty;
            }
            catch { return string.Empty; }
        }

        public static void SetAvailableCabinTypes(string cabinkey)
        {
            try
            {
                if (!string.IsNullOrEmpty(cabinkey) && !_availableCabinTypes.Contains(cabinkey))
                    _availableCabinTypes.Add(cabinkey);
            }
            catch { }
        }

        public static string GetAvailableSeatBannerMsg(UpgradeOption option)
        {
            try
            {
                if (string.Equals(_strUCTypeDoubleUpgrade, option.CabinUpgradeType, StringComparison.OrdinalIgnoreCase)) return string.Empty;
                if (string.Equals(_strUCStatusAvailable, option.UpgradeStatus, StringComparison.OrdinalIgnoreCase))
                {
                    int.TryParse(option.AvailableSeatCount, out int seatcount);
                    if (seatcount == 0) return string.Empty;
                    if (seatcount < 7)
                    {
                        return Convert.ToString(seatcount); //string.Format("Only {0} seat left", seatcount);
                    }
                }
                return string.Empty;
            }
            catch { return string.Empty; }
        }

        private List<MOBTrip> GetUpgradeCabinTrips(Collection<ReservationFlightSegment> flightsegments)
        {
            List<MOBTrip> trips = new List<MOBTrip>();

            if (flightsegments != null && flightsegments.Any())
            {
                flightsegments = flightsegments.OrderBy(x => x.TripNumber).ToCollection();
                int mintripnumber = Convert.ToInt32(flightsegments.Select(o => o.TripNumber).First());
                int maxtripnumber = Convert.ToInt32(flightsegments.Select(o => o.TripNumber).Last());

                for (int i = mintripnumber; i <= maxtripnumber; i++)
                {
                    MOBTrip pnrTrip = new MOBTrip();

                    var totalTripSegments = flightsegments.Where(o => o.TripNumber == i.ToString());

                    pnrTrip.Index = i;

                    foreach (United.Service.Presentation.SegmentModel.ReservationFlightSegment segment in flightsegments)
                    {
                        if (!string.IsNullOrEmpty(segment.TripNumber) && Convert.ToInt32(segment.TripNumber) == i)
                        {
                            string airportName = string.Empty;
                            string cityName = string.Empty;

                            if (segment.SegmentNumber == totalTripSegments.Min(x => x.SegmentNumber))
                            {
                                pnrTrip.Origin = segment.FlightSegment.DepartureAirport.IATACode;
                                // Utility.GetAirportCityName(pnrTrip.Origin, ref airportName, ref cityName);
                                _shoppingUtility.GetAirportCityName(pnrTrip.Origin, ref airportName, ref cityName);

                                pnrTrip.OriginName = airportName;
                                DateTime departureTime;
                                if (DateTime.TryParse(segment.FlightSegment.DepartureDateTime, out departureTime))
                                {
                                    pnrTrip.DepartureTime = departureTime.ToString("MM/dd/yyyy hh:mm tt");
                                }
                            }
                            if (segment.SegmentNumber == totalTripSegments.Max(x => x.SegmentNumber))
                            {
                                pnrTrip.Destination = segment.FlightSegment.ArrivalAirport.IATACode;
                                _shoppingUtility.GetAirportCityName(pnrTrip.Destination, ref airportName, ref cityName);
                                pnrTrip.DestinationName = airportName;
                                DateTime arrivalTime;
                                if (DateTime.TryParse(segment.FlightSegment.ArrivalDateTime, out arrivalTime))
                                {
                                    pnrTrip.ArrivalTime = arrivalTime.ToString("MM/dd/yyyy hh:mm tt");
                                }
                            }

                        }
                    }
                    trips.Add(pnrTrip);
                }
            }
            return trips;
        }
        public static List<MOBUpgradeCabinSegment> GetMilesUpgradeOption(UpgradeEligibilityResponse cslresponse, MOBUpgradeCabinEligibilityResponse response)
        {
            List<MOBUpgradeCabinSegment> tripsupgradeoptions = new List<MOBUpgradeCabinSegment>();
            List<MOBUpgradePriceOption> upgradepriceoptions;
            MOBUpgradeCabinSegment tripupgradeoption;
            MOBUpgradeOption upgradeOption;
            MOBUpgradeCabinTypeDesc cabintypes;

            var trips = response?.Trips;
            if (trips == null || !trips.Any()) return tripsupgradeoptions;

            trips.ForEach(trip =>
            {

                string tripnumber = Convert.ToString(trip.Index);

                var selectedtripODOOption = GetSelectedTripODOption(cslresponse, tripnumber);

                if (selectedtripODOOption != null)
                {
                    var availableUpgrade = selectedtripODOOption.UpgradeOptions.Where
                    (x => string.Equals(x.UpgradeType, _strMUA, StringComparison.OrdinalIgnoreCase));

                    if (availableUpgrade != null && availableUpgrade.Any())
                    {
                        tripupgradeoption = new MOBUpgradeCabinSegment { TripNumber = tripnumber };
                        availableUpgrade.ForEach(option =>
                        {
                            upgradeOption = new MOBUpgradeOption
                            {
                                UpgradeType = option.UpgradeType,
                                UpgradeStatus = option.UpgradeStatus,
                                Id = option.Id,
                                TripRefId = tripnumber,
                            };

                            upgradeOption.CabinUpgradeTypeDesc =
                            (!string.IsNullOrEmpty(option.CabinUpgradeType))
                            ? option.CabinUpgradeType.ToUpper() : string.Empty;

                            //upgradeOption.UpgradeCabinTypes
                            if (option.SegmentMapping != null
                            && option.SegmentMapping.SegmentReferences != null
                            && option.SegmentMapping.SegmentReferences.Any())
                            {
                                option.SegmentMapping.SegmentReferences.ForEach(item =>
                                {
                                    cabintypes = new MOBUpgradeCabinTypeDesc
                                    {
                                        UpgradeStatus = item.UpgradeStatus,
                                        UpgradeTypeDesc = item.CabinUpgradeTypeDesc,
                                        SegmentNumber = item.SegmentRefID,
                                    };
                                    var eligiblesegment = selectedtripODOOption.FlightSegments.FirstOrDefault
                                            (x => string.Equals(Convert.ToString(x.SegmentNumber), cabintypes.SegmentNumber, StringComparison.OrdinalIgnoreCase));

                                    cabintypes.UpgradeTypeDesc = GetCabinUpgradeTypeDesc(eligiblesegment.Equipment, item.CabinUpgradeType);
                                    upgradeOption.UpgradeCabinTypes
                                    = (upgradeOption.UpgradeCabinTypes == null)
                                    ? new List<MOBUpgradeCabinTypeDesc>() : upgradeOption.UpgradeCabinTypes;
                                    upgradeOption.UpgradeCabinTypes.Add(cabintypes);
                                });
                            }

                            option.PriceOptions.ToList().ForEach(priceoption =>
                            {

                                MOBUpgradePriceOption upgradepriceoption;
                                upgradepriceoptions = new List<MOBUpgradePriceOption>();

                                if (priceoption.Copay != null)
                                {
                                    upgradepriceoption = new MOBUpgradePriceOption
                                    {
                                        Type = priceoption.Copay.Currency.Code,
                                        Value = Convert.ToString(priceoption.Copay.Amount),
                                        EDDCode = priceoption.EDDCode
                                    };
                                    upgradepriceoptions.Add(upgradepriceoption);
                                }
                                if (priceoption.Instrument != null)
                                {
                                    upgradepriceoption = new MOBUpgradePriceOption
                                    {
                                        RewardCode = priceoption.RewardCode,
                                        Type = priceoption.Instrument.FirstOrDefault().Type,
                                        Value = Convert.ToString(priceoption.Instrument.FirstOrDefault().Value)
                                    };
                                    upgradepriceoptions.Add(upgradepriceoption);
                                }

                                upgradeOption.PriceOption = upgradepriceoptions;
                            });
                            //Order By segment
                            if (upgradeOption.UpgradeCabinTypes != null && upgradeOption.UpgradeCabinTypes.Any())
                            {
                                upgradeOption.UpgradeCabinTypes
                                = upgradeOption.UpgradeCabinTypes.OrderBy(x => x.SegmentNumber).ToList();
                            }

                            tripupgradeoption.Miles = (tripupgradeoption.Miles == null)
                                    ? new List<MOBUpgradeOption>() : tripupgradeoption.Miles;

                            if (option.Taxes != null && option.Taxes.Any())
                            {
                                upgradeOption.Taxes = GetUpgradeCabinAPDTaxdata(cslresponse, option);
                            }
                            tripupgradeoption.Miles.Add(upgradeOption);

                            //Order By upgrade type
                            if (tripupgradeoption.Miles != null && tripupgradeoption.Miles.Any())
                            {
                                tripupgradeoption.Miles = tripupgradeoption.Miles.OrderBy
                                (d => _CUtypeorder.IndexOf(d.CabinUpgradeTypeDesc)).ToList();
                            }


                        });
                        tripsupgradeoptions.Add(tripupgradeoption);
                    };
                }
            });

            return tripsupgradeoptions;
        }
        public static List<Model.Common.MOBPNRPassenger> GetUpgradeCabinPassangers(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers)
        {
            //TODO null check
            List<Model.Common.MOBPNRPassenger> passengers = new List<Model.Common.MOBPNRPassenger>();

            if (travelers == null || !travelers.Any()) return null;

            travelers.ToList().ForEach(pax =>
            {

                if (pax != null && pax.Person != null)
                {
                    Model.Common.MOBPNRPassenger passenger = new Model.Common.MOBPNRPassenger
                    {
                        SHARESPosition = pax.Person.Key,
                        TravelerTypeCode = pax.Person.Type,
                        SSRDisplaySequence = pax.Person.Key
                    };

                    passenger.PNRCustomerID = pax.Person.CustomerID;
                    passenger.BirthDate = pax.Person.DateOfBirth;
                    passenger.PassengerName = new Model.Common.MOBName
                    {
                        First = pax.Person.GivenName,
                        Last = pax.Person.Surname,
                    };
                    passengers.Add(passenger);
                }
            });

            return passengers;
        }
        private static List<MOBSHOPTax> GetUpgradeCabinAPDTaxdata(UpgradeEligibilityResponse cslresponse, UpgradeOption option)
        {
            var adtpax = new List<string>();
            var childpax = new List<string>();
            var allpax = new List<string>();

            double totalamount = 0.00;
            List<MOBSHOPTax> mobshoptax = new List<MOBSHOPTax>();

            //check if all tax amount are same
            //request.UpgradeProducts.GroupBy(x => x.Id).Select(x => x.First());
            var taxcomponents = option.Taxes.Select
                (x => x.Taxes.FirstOrDefault(y => string.Equals(y.Code, _strAPDTaxCode, StringComparison.OrdinalIgnoreCase)));
            var checktaxdifference = taxcomponents.GroupBy(x => x.Amount).Select(x => x.First());

            bool isSameTax = (checktaxdifference.Count() == 1);

            cslresponse.Travelers.ForEach(traveler =>
            {
                allpax.Add(traveler.TravelerNameIndex);

                if (_paxTypeCode.Contains(traveler.PricingPaxType))
                    adtpax.Add(traveler.TravelerNameIndex);
                else
                    childpax.Add(traveler.TravelerNameIndex);
            });
            if (isSameTax)
            {
                MOBSHOPTax travelertaxes = SetAPDTax(option, allpax, paxtype: string.Empty);
                totalamount = Convert.ToDouble(travelertaxes.Amount * allpax.Count);
                mobshoptax.Add(travelertaxes);
            }
            else
            {
                if (adtpax != null && adtpax.Any())
                {
                    MOBSHOPTax adulttaxes = SetAPDTax(option, adtpax, paxtype: "ADULT");
                    totalamount = Convert.ToDouble(adulttaxes.Amount * adtpax.Count);
                    mobshoptax.Add(adulttaxes);
                }
                if (childpax != null && childpax.Any())
                {
                    MOBSHOPTax childtaxes = SetAPDTax(option, childpax, paxtype: "CHILD");
                    totalamount = totalamount + Convert.ToDouble(childtaxes.Amount * childpax.Count);
                    mobshoptax.Add(childtaxes);
                }
            }

            if (mobshoptax != null && mobshoptax.Any())
            {
                var mobtax = new MOBSHOPTax
                {
                    DisplayAmount = Convert.ToString(totalamount),
                    DisplayNewAmount = string.Format("${0:F2}", totalamount),
                    TaxCode = "TOTALAPD",
                    TaxCodeDescription = _strAPDTaxDesc,
                    CurrencyCode = _strUSD,
                };
                mobshoptax.Add(mobtax);
            }

            return (mobshoptax != null && mobshoptax.Any()) ? mobshoptax : null;
        }

        public static MOBSHOPTax SetAPDTax(UpgradeOption option, List<string> paxlist, string paxtype = "")
        {
            var paxtaxinfo = option.Taxes.Where(x => x.Association.TravelerRefIDs.Any
                                    (y => paxlist.IndexOf(y) > -1)).FirstOrDefault().Taxes.FirstOrDefault
                                    (x => string.Equals(x.Code, _strAPDTaxCode, StringComparison.OrdinalIgnoreCase));

            var paxsingular = (string.Equals(paxtype, _strADULTPaxType, StringComparison.OrdinalIgnoreCase))
                ? (paxlist.Count == 1) ? "adult" : "adults"
                : (string.Equals(paxtype, _strCHILDPaxType, StringComparison.OrdinalIgnoreCase))
                ? (paxlist.Count == 1) ? "child" : "children" : (paxlist.Count == 1) ? "traveler" : "travelers";

            var mobtax = new MOBSHOPTax
            {
                Amount = Convert.ToDecimal(paxtaxinfo.Amount),
                CurrencyCode = paxtaxinfo.Currency.Code,
                TaxCodeDescription = paxtaxinfo.Description,
                TaxCode = paxtaxinfo.Code,
                DisplayAmount = string.Format("{0} {1} x ${2:F2}", paxlist.Count, paxsingular, paxtaxinfo.Amount),
            };

            return mobtax;
        }


        public static void CheckPlusPointsExpiryStatus(MOBPlusPoints pluspoints, Collection<ReservationFlightSegment> flightsegments, MOBUpgradeCabinEligibilityResponse response)
        {
            try
            {
                if (flightsegments != null && flightsegments.Any())
                {
                    if (pluspoints != null
                        && pluspoints.ExpirationPointsAndDatesKVP != null && pluspoints.ExpirationPointsAndDatesKVP.Any())
                    {
                        flightsegments.ForEach(segment =>
                        {
                            bool isPlusPointsPartiallyExpiredBeforeTravel = false;
                            bool isPlusPointsFullyExpiredBeforeTravel = true;

                            pluspoints.ExpirationPointsAndDatesKVP.ForEach(expiry =>
                            {
                                if (DateTime.TryParse(expiry.Value, out DateTime expirydate)
                                    && DateTime.TryParse(segment.EstimatedDepartureTime, out DateTime segmentdepartdate))
                                {
                                    isPlusPointsPartiallyExpiredBeforeTravel
                                    = isPlusPointsPartiallyExpiredBeforeTravel || (DateTime.Compare(expirydate, segmentdepartdate) < 0);

                                    isPlusPointsFullyExpiredBeforeTravel
                                    = isPlusPointsFullyExpiredBeforeTravel && (DateTime.Compare(expirydate, segmentdepartdate) < 0);
                                }
                            });

                            response.Segments.FirstOrDefault(x => string.Equals
                               (x.TripNumber, segment.TripNumber, StringComparison.OrdinalIgnoreCase)
                               && (string.Equals(x.SegmentNumber, Convert.ToString
                               (segment.SegmentNumber), StringComparison.OrdinalIgnoreCase))).IsPlusPointsExpiredBeforeTravel = isPlusPointsFullyExpiredBeforeTravel;

                            response.Segments.FirstOrDefault(x => string.Equals
                               (x.TripNumber, segment.TripNumber, StringComparison.OrdinalIgnoreCase)
                               && (string.Equals(x.SegmentNumber, Convert.ToString
                               (segment.SegmentNumber), StringComparison.OrdinalIgnoreCase))).ShowPlusPointsExpiryMessage = isPlusPointsPartiallyExpiredBeforeTravel;

                        });
                    }
                }
            }
            catch { }
        }

        private async Task SetEligibilityLearnAboutContentsAsync(MOBUpgradeCabinEligibilityResponse response)
        {
            try
            {
                List<MOBUpgradeCabinOptionContent> cabinoptioncontents = await GetUpgradeCabinPageContentsAsync(dbcontext: "UpgradeCabin_OptionsPageContent");

                if (cabinoptioncontents != null && cabinoptioncontents.Any())
                {
                    var filteredCabinoptioncontents = cabinoptioncontents.Where(x => (_availableCabinTypes.IndexOf(x.Product) > -1));

                    response.CabinOptionContents = (filteredCabinoptioncontents != null && filteredCabinoptioncontents.Any())
                        ? filteredCabinoptioncontents.ToList() : cabinoptioncontents;
                }
            }
            catch { }
        }

        private async Task<List<MOBUpgradeCabinOptionContent>> GetUpgradeCabinPageContentsAsync(string dbcontext)
        {
            var cabinoptions = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(dbcontext, _headers.ContextValues.TransactionId, true);

            if (cabinoptions == null || !cabinoptions.Any())
                return null;

            var upgradecabinoptions = new List<MOBUpgradeCabinOptionContent>();
            MOBUpgradeCabinOptionContent upgradecabinoption;
            List<string> subitemlist;

            try
            {
                cabinoptions.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.Document))
                    {
                        string[] items = item.Document.Split('|');
                        if (items.Any())
                        {
                            upgradecabinoption = new MOBUpgradeCabinOptionContent
                            {
                                Header = (string.IsNullOrEmpty(items[0])) ? string.Empty : items[0],
                                ImageUrl = (string.IsNullOrEmpty(items[1])) ? string.Empty : items[1],
                                Product = (string.IsNullOrEmpty(items[2])) ? string.Empty : items[2],
                            };

                            if (string.Equals(items[2], _strUPP2, StringComparison.OrdinalIgnoreCase))
                            {
                                upgradecabinoption.Header = AppendServiceMark(items[0]);
                            }

                            string[] subitems = items[3].Split('~');
                            if (subitems.Any())
                            {
                                if (subitems.Count() == 1)
                                    upgradecabinoption.Body = (string.IsNullOrEmpty(items[3])) ? string.Empty : items[3];
                                else
                                {
                                    subitemlist = new List<string>();
                                    subitems.ForEach(subitem =>
                                    {
                                        if (string.IsNullOrEmpty(subitem) == false)
                                        {
                                            if (subitem.Contains('#'))
                                            { subitem = AppendServiceMark(subitem); }
                                            else if (subitem.Contains('%'))
                                            { subitem = AppendTradeMark(subitem); }

                                            subitemlist.Add(subitem);
                                        }
                                    });
                                    upgradecabinoption.BodyItems = subitemlist;
                                }
                            }
                            upgradecabinoptions.Add(upgradecabinoption);
                        }
                    }
                });
                return upgradecabinoptions;
            }
            catch { return null; }
        }
        private static string AppendServiceMark(string inputstr)
        {
            try { return inputstr.Replace('#', '℠'); } catch { return string.Empty; }
        }

        private static string AppendTradeMark(string inputstr)
        {
            try { return inputstr.Replace('%', '™'); } catch { return string.Empty; }
        }

        private async Task SetEligibilityMessageContentsAsync(MOBUpgradeCabinEligibilityResponse response)
        {
            try
            {
                var cabinoptionmessages = await GetUpgradeCabinMessageContents(dbcontext: "UpgradeCabin_EligibilityMessageContent");

                //remove PPOINTSEVERGREEN
                //if (_plusPointHideEvergreenMsg)
                //{
                //    var evergreenmsg = cabinoptionmessages.FirstOrDefault(x => x.ContentType == UpgradeCabinContentType.PPOINTSEVERGREEN);
                //    if (evergreenmsg != null) cabinoptionmessages.Remove(evergreenmsg);
                //}
                response.CabinOptionMessages = cabinoptionmessages;
            }
            catch (Exception ex)
            {
            }
        }

        private async Task<List<MOBUpgradeCabinAdvisory>> GetUpgradeCabinMessageContents(string dbcontext)
        {
            var cabinoptions = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(dbcontext, _headers.ContextValues.TransactionId, true);

            if (cabinoptions == null || !cabinoptions.Any())
                return null;
            
            var upgradecabinmessages = new List<MOBUpgradeCabinAdvisory>();
            MOBUpgradeCabinAdvisory upgradecabinmessage;

            try
            {
                cabinoptions.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.Document))
                    {
                        string[] items = item.Document.Split('|');
                        if (items.Any())
                        {
                            upgradecabinmessage = new MOBUpgradeCabinAdvisory
                            {
                                Header = (string.IsNullOrEmpty(items[2])) ? string.Empty : items[2],
                                Body = (string.IsNullOrEmpty(items[3])) ? string.Empty : items[3]
                            };

                            if (!string.IsNullOrEmpty(items[0]))
                                upgradecabinmessage.ContentType =
                                (UpgradeCabinContentType)Enum.Parse(typeof(UpgradeCabinContentType), items[0]);

                            if (!string.IsNullOrEmpty(items[1]))
                                upgradecabinmessage.AdvisoryType =
                                (UpgradeCabinAdvisoryType)Enum.Parse(typeof(UpgradeCabinAdvisoryType), items[1]);

                            upgradecabinmessages.Add(upgradecabinmessage);
                        }
                    }
                });
                return upgradecabinmessages;
            }
            catch { return null; }
        }
        public async Task<MOBUpgradePlusPointWebMyTripResponse> UpgradePlusPointWebMyTrip(MOBUpgradePlusPointWebMyTripRequest request)
        {
            var response = new MOBUpgradePlusPointWebMyTripResponse();
            Model.Common.Session session = new Model.Common.Session();
            string loggingId = (string.IsNullOrEmpty(request.SessionId)) ? request.TransactionId : request.SessionId;

            //Get session from persisted file
            if (string.IsNullOrEmpty(request.SessionId))
                session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, string.Empty);

            else
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);


            request.SessionId = session.SessionId;
            request.Token = session.Token;

            if (string.IsNullOrEmpty(request.MileagePlusNumber) ||
                string.IsNullOrEmpty(request.HashPinCode) ||
                string.IsNullOrEmpty(session.SessionId))

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("upgradecabinineligiblesvcerror"));


            var ssoinfo = await Task.Run(() => CreateSSOInformation(request, request.MileagePlusNumber, request.HashPinCode, request.SessionId));

            if (ssoinfo != null && ssoinfo.Any())
                response.RedirectUrl = string.Format(Convert.ToString(_configuration.GetValue<string>("upgradecabinmytripsdotcomurl")));

            response.WebShareToken = ssoinfo.FirstOrDefault
                (x => string.Equals(x.Id, _websharetoken, StringComparison.OrdinalIgnoreCase))?.CurrentValue;
            response.WebSessionShareUrl = ssoinfo.FirstOrDefault
                (x => string.Equals(x.Id, _websessionshareurl, StringComparison.OrdinalIgnoreCase))?.CurrentValue;

            if (await _featureSettings.GetFeatureSettingValue("EnableRedirectURLUpdate").ConfigureAwait(false))
            {
                response.RedirectUrl = $"{_configuration.GetValue<string>("NewDotcomSSOUrl")}?type=sso&token={response.WebShareToken}&landingUrl={response.RedirectUrl}";
                response.WebSessionShareUrl = response.WebShareToken = string.Empty;
            }

            return response;
        }

        public async Task<List<MOBItem>> CreateSSOInformation(MOBRequest request,
  string mileageplusnumber, string hashpincode, string sessionid)
        {
            bool validSSORequest = false;
            string authToken = string.Empty;
            var response = new List<MOBItem>();

            HashPin hashPin = new HashPin(_logger, _configuration, _validateHashPinService, _dynamoDBService, _mPSignInCommonService, _headers, _httpContextAccessor, _featureSettings);
            var tupleRes = await hashPin.ValidateHashPinAndGetAuthToken(mileageplusnumber, hashpincode, request.Application.Id, request.DeviceId, request.Application.Version.Major, authToken, sessionid);
            validSSORequest = tupleRes.returnValue;
            authToken = tupleRes.validAuthToken;

            if (!validSSORequest)
                throw new MOBUnitedException(_configuration.GetValue<string>("bugBountySessionExpiredMsg"));

            if (validSSORequest)
            {
                var WebShareToken = _tokenService.GetSSOTokenString(request.Application.Id, mileageplusnumber, _configuration)?.ToString();

                response.Add(new MOBItem
                {
                    Id = "WebShareToken",
                    CurrentValue = WebShareToken
                });

                if (!string.IsNullOrEmpty(WebShareToken))
                {
                    response.Add(new MOBItem
                    {
                        Id = "WebSessionShareUrl",
                        CurrentValue = _configuration.GetValue<string>("DotcomSSOUrl")
                    });

                }
            }
            return response;
        }
        public async Task<MOBUpgradeCabinRegisterOfferResponse> UpgradeCabinRegisterOfferAsync(MOBUpgradeCabinRegisterOfferRequest request, United.Mobile.Model.Common.Session session)
        {
            _availablePriceTypes = new List<string>();
            var response = new MOBUpgradeCabinRegisterOfferResponse();

            // var upgradeeligibility = await Task.Run(() => _sessionHelperService.SaveSessiondata<UpgradeEligibilityResponse>(request.SessionId, new MOBUpgradeCabinEligibilityRequest().GetType().FullName));

            var upgradeeligibility = await Task.Run(() => _sessionHelperService.GetSession<UpgradeEligibilityResponse>(request.SessionId, typeof(UpgradeEligibilityResponse).FullName).Result);

            var reservation = upgradeeligibility.Reservation;
            var solutions = upgradeeligibility.Solutions;

            var shopregisterofferrequest = new United.Mobile.Model.Payment.MOBRegisterOfferRequest();
            request.CartId = await _shopping.CreateCart(request, session);

            shopregisterofferrequest.CartId = request.CartId;
            shopregisterofferrequest.SessionId = request.SessionId;
            shopregisterofferrequest.Application = request.Application;
            shopregisterofferrequest.DeviceId = request.DeviceId;
            shopregisterofferrequest.Flow = request.FlowType;

            _availablePriceTypes = request.UpgradeProducts.Select(x => x.UpgradeType).Distinct().ToList();

            var registerOfferRequests = BuildUpgradeCabinRegisterOffersRequest(request, upgradeeligibility);
            var shopregisterofferresponse = await _shopping.RegisterOffers(shopregisterofferrequest, null, null, null, null, session, upgradeCabinRegisterOfferRequest: registerOfferRequests);

            if (shopregisterofferresponse != null && shopregisterofferresponse.Status.Equals(United.Services.FlightShopping.Common.StatusType.Success)
            && shopregisterofferresponse.DisplayCart != null)
            {
                if (shopregisterofferresponse.Errors != null && shopregisterofferresponse.Errors.Any())
                {
                    var error = shopregisterofferresponse.Errors.FirstOrDefault
                        (x => (x?.MinorDescription?.IndexOf(_strUGCInsufficientPoints, StringComparison.OrdinalIgnoreCase) > -1));
                    if (error != null)
                    {
                        var errordata = _manageResUtility.GetListFrmPipelineSeptdConfigString("UpgradeCabinInSufficientError");
                        if (errordata != null && errordata.Any())
                            response.Exception = new MOBException { Code = errordata[0], Message = errordata[1] };
                    }
                    else
                    {
                        response.Exception = new MOBException
                        { Code = "9999", Message = _configuration.GetValue<string>("Booking2OGenericExceptionMessage") };
                    }
                }
                else
                {
                    response.UpgradeProducts = request.UpgradeProducts;
                    // await Task.Run(() => _sessionHelperService.SaveSession<MOBUpgradeOption>(request.UpgradeProducts.ToString(), new MOBUpgradeOption().GetType().FullName));


                    response.CartId = (!string.IsNullOrEmpty(request.CartId)) ? request.CartId : string.Empty;

                    response.PriceDetails = SetUpgradePriceDetails(response.UpgradeProducts, upgradeeligibility, shopregisterofferresponse, request.MileagePlusNumber);

                    await SetRegisterOfferContentMessageAsync(response);

                    response.ShoppingCart = await CreateShoppingCartAsync(shopregisterofferresponse, request);
                }
            }

            response.CartId = shopregisterofferresponse.CartId;


            //response.PKDispenserPublicKey =  _pKDispenserService.GetPkDispenserPublicKey<MOBUpgradeCabinEligibilityResponse>(request.Token,request.SessionId.ToString(),string.Empty)

            //response.UpgradeEligibility = United.Persist.FilePersist.Load<MOBUpgradeCabinEligibilityResponse>
            //    (request.SessionId, (new MOBUpgradeCabinEligibilityResponse()).GetType().FullName);

            response.TransactionId = request.TransactionId;
            response.SessionId = request.SessionId;

            return response;
        }



        private Collection<Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest> BuildUpgradeCabinRegisterOffersRequest(MOBUpgradeCabinRegisterOfferRequest request, UpgradeEligibilityResponse upgradeeligibility)
        {
            var registerOfferRequests = new Collection<Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest>();

            var requesttripcount = request.UpgradeProducts.GroupBy(x => x.TripRefId).Select(x => x.First());

            if (requesttripcount != null && !requesttripcount.Any()) return null;

            var selectedODOption = upgradeeligibility.Solutions.FirstOrDefault().ODOptions;

            requesttripcount.ForEach(reqtrip =>
            {

                Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest registerOfferRequest;

                var tripproducts = request.UpgradeProducts.Where
                        (x => string.Equals
                        (x.TripRefId, reqtrip.TripRefId, StringComparison.OrdinalIgnoreCase));

                if (tripproducts != null && tripproducts.Any())
                {
                    registerOfferRequest =
                        new Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest
                        {
                            CountryCode = "US",
                            LangCode = "en-US",
                            CartId = request.CartId,
                            CartKey = request.CartId,
                            Channel = _strChannel,
                            WorkFlowType = Services.FlightShopping.Common.FlightReservation.WorkFlowType.UpgradesPurchase,

                            Offer = new Service.Presentation.ProductResponseModel.ProductOffer
                            { Travelers = AssignSelectedTraveler(upgradeeligibility.Reservation) },

                            Reservation = upgradeeligibility.Reservation,

                            Characteristics = new Collection<Characteristic> {
                                new Characteristic{ Code = "BALANCE_CHECK", Value = "True" } }
                        };

                    //TODO - Assuming - one product type per trip.
                    var firstproducttype = tripproducts.FirstOrDefault();

                    registerOfferRequest.ProductCode = firstproducttype.UpgradeType;

                    registerOfferRequest.LoyaltyUpgradeOffer
                            = selectedODOption.Where(x => x.ID == firstproducttype.TripRefId).FirstOrDefault();

                    //Start PCU
                    if (string.Equals(registerOfferRequest.ProductCode, _strPCU, StringComparison.OrdinalIgnoreCase))
                    {
                        var ids = new List<string>();
                        var subproducts = new Collection<Service.Presentation.ProductModel.SubProduct>();

                        tripproducts.ForEach(product =>
                        {
                            var pcuproducts = GetPriceProducts(product.Id, upgradeeligibility.ProductOffers);

                            if (pcuproducts != null && pcuproducts.Any())
                            {
                                pcuproducts.ForEach(pcuproduct =>
                                {
                                    subproducts.Add(pcuproduct);
                                    pcuproduct.Prices.ForEach(item => { ids.Add(item.ID); });
                                });
                            }

                            if (ids != null && ids.Any())
                            {
                                var selectedproduct = new Services.FlightShopping.Common.FlightReservation.ProductRequest
                                {
                                    Code = _strPCU,
                                    Ids = ids
                                };

                                registerOfferRequest.Products
                                = (registerOfferRequest.Products == null)
                                ? new List<ProductRequest>() : registerOfferRequest.Products;

                                registerOfferRequest.Products.Add(selectedproduct);
                            }
                        });

                        registerOfferRequest.Offer.Offers
                             = new Collection<Service.Presentation.ProductResponseModel.Offer> {
                                     new Service.Presentation.ProductResponseModel.Offer
                                     {
                                         ProductInformation
                                         = new Service.Presentation.ProductResponseModel.ProductInformation{
                                             ProductDetails
                                             = new Collection<Service.Presentation.ProductResponseModel.ProductDetail>{
                                                 new Service.Presentation.ProductResponseModel.ProductDetail
                                                 {
                                                     Product = new Service.Presentation.ProductModel.Product
                                                     {
                                                         Code = _strPCU,
                                                         SubProducts = subproducts,
                                                     }
                                                 }
                                             }
                                         }
                                     }
                             };
                    }//End of PCU 

                    else if (string.Equals(registerOfferRequest.ProductCode, _strMUA, StringComparison.OrdinalIgnoreCase))
                    {
                        registerOfferRequest.Products = RegisterOfferMUXSelectedProducts(tripproducts, registerOfferRequest.LoyaltyUpgradeOffer);
                    }
                    else if (string.Equals(registerOfferRequest.ProductCode, _strCUG, StringComparison.OrdinalIgnoreCase)
                    || string.Equals(registerOfferRequest.ProductCode, _strUGC, StringComparison.OrdinalIgnoreCase))
                    {
                        registerOfferRequest.Products = RegisterOfferUGCSelectedProducts(tripproducts, registerOfferRequest.LoyaltyUpgradeOffer);
                    }

                    if (registerOfferRequest.Products != null && registerOfferRequest.Products.Any())
                    {
                        registerOfferRequests.Add(registerOfferRequest);
                    }
                }
            });

            return registerOfferRequests;
        }

        private static Collection<Service.Presentation.ProductModel.SubProduct> GetPriceProducts(string productid, Collection<Service.Presentation.ProductModel.SubProduct> priceproducts)
        {
            var selectedpriceproducts = priceproducts.Where(x => string.Equals(x.ID, productid, StringComparison.OrdinalIgnoreCase));
            return selectedpriceproducts.ToCollection();
        }

        public static List<Services.FlightShopping.Common.FlightReservation.ProductRequest> RegisterOfferMUXSelectedProducts(IEnumerable<MOBUpgradeOption> products, UpgradeOriginDestinationOption ODOptions)
        {
            var selectedproducts = new List<Services.FlightShopping.Common.FlightReservation.ProductRequest>();

            if (products == null || !products.Any()) return null;

            products.ToList().ForEach(product =>
            {
                if (product != null)
                {
                    UpgradeOption availableUpgrade = null;

                    availableUpgrade = ODOptions.UpgradeOptions.Where(x => product.Id == x.Id).FirstOrDefault();

                    if (availableUpgrade != null)
                    {
                        var selectedproduct = new Services.FlightShopping.Common.FlightReservation.ProductRequest
                        {
                            Code = availableUpgrade.UpgradeType,
                            Ids = new List<string> { availableUpgrade.Id }
                        };
                        selectedproducts.Add(selectedproduct);
                    }
                }
            });
            return (selectedproducts != null && selectedproducts.Any()) ? selectedproducts : null;
        }

        public static List<Services.FlightShopping.Common.FlightReservation.ProductRequest> RegisterOfferUGCSelectedProducts(IEnumerable<MOBUpgradeOption> products, UpgradeOriginDestinationOption ODOptions)
        {
            var selectedproducts = new List<Services.FlightShopping.Common.FlightReservation.ProductRequest>();

            if (products == null || !products.Any()) return null;

            var availableproducts = products.OrderBy(x => x.SegmentRefId)?.Select(x => x.Id);

            //var isExceptitem = ODOptions.UpgradeOptions.Select(x => x).Where
            //    (x => x.SegmentMapping?.SegmentRefIDs.Except(availableproducts);

            ODOptions.UpgradeOptions.ForEach(odo =>
            {
                if (_CUpointstypeorder.Contains(odo.UpgradeType, StringComparer.OrdinalIgnoreCase))
                {
                    if (odo != null && odo.SegmentMapping != null
                    && odo.SegmentMapping.SegmentRefIDs != null
                    && odo.SegmentMapping.SegmentRefIDs.Any())
                    {
                        if (!odo.SegmentMapping.SegmentRefIDs.Except(availableproducts).Any()
                        && !availableproducts.Except(odo.SegmentMapping.SegmentRefIDs).Any())
                        {
                            var selectedproduct = new Services.FlightShopping.Common.FlightReservation.ProductRequest
                            {
                                Code = odo.UpgradeType,
                                Ids = new List<string> { odo.Id }
                            };
                            selectedproducts.Add(selectedproduct);
                        }
                    }
                }
            });

            if (selectedproducts == null || !selectedproducts.Any())
            {
                products.ToList().ForEach(product =>
                {
                    if (product != null)
                    {
                        UpgradeOption availableUpgrade
                        = ODOptions.UpgradeOptions.Where(x => product.Id == x.Id).FirstOrDefault();

                        if (availableUpgrade != null)
                        {
                            var selectedproduct = new Services.FlightShopping.Common.FlightReservation.ProductRequest
                            {
                                Code = availableUpgrade.UpgradeType,
                                Ids = new List<string> { availableUpgrade.Id }
                            };
                            selectedproducts.Add(selectedproduct);
                        }
                    }
                });
            }

            return (selectedproducts != null && selectedproducts.Any()) ? selectedproducts : null;
        }

        private static Collection<Service.Presentation.ProductModel.ProductTraveler> AssignSelectedTraveler(United.Service.Presentation.ReservationModel.Reservation reservation)
        {

            var travelers = new Collection<Service.Presentation.ProductModel.ProductTraveler>();
            reservation.Travelers.ToList().ForEach(pax =>
            {

                var traveler = new Service.Presentation.ProductModel.ProductTraveler
                {
                    Characteristics = pax.Characteristics,
                    LoyaltyProgramProfile = pax.LoyaltyProgramProfile,
                };

                if (pax.Person != null)
                {
                    string sharesindex = pax.Person.Key;

                    traveler.ID = sharesindex;
                    traveler.Key = sharesindex;
                    traveler.GivenName = pax.Person.GivenName;
                    traveler.ReservationIndex = sharesindex;
                    traveler.TravelerNameIndex = sharesindex;
                    traveler.PassengerTypeCode = pax.Person.Type;
                    traveler.PricingPaxType = pax.Person.PricingPaxType;
                }
                travelers.Add(traveler);
            });
            return travelers;
        }

        private async Task<Model.Shopping.MOBShoppingCart> CreateShoppingCartAsync(FlightReservationResponse cslResponse, MOBUpgradeCabinRegisterOfferRequest request)
        {
            decimal totalamount = 0;
            Int32 totalpoints = 0;
            Int32 totalmiles = 0;
            decimal totalprice = 0;

            Model.Shopping.MOBShoppingCart shoppingCart = new Model.Shopping.MOBShoppingCart { CartId = cslResponse.CartId, Flow = _UPGRADEMALL };
            var products = new List<ProdDetail>();

            if (cslResponse.DisplayCart.DisplayFees != null
                        && cslResponse.DisplayCart.DisplayFees.Any())
            {
                cslResponse.DisplayCart.DisplayFees.ForEach(feesitem =>
                {
                    //POINTS or MILES
                    if (string.Equals(feesitem.Currency, _strUGC, StringComparison.OrdinalIgnoreCase) ||
                    (string.Equals(feesitem.Currency, _strMILES, StringComparison.OrdinalIgnoreCase)))
                    {
                        var pointsmilesproduct = new ProdDetail();
                        if (string.Equals(feesitem.Currency, _strUGC, StringComparison.OrdinalIgnoreCase))
                        {
                            pointsmilesproduct.Code = _strPOINTS;
                            totalpoints = totalpoints + decimal.ToInt32(feesitem.Amount);
                        }
                        else
                        {
                            pointsmilesproduct.Code = _strMUAUPGRADE;
                            totalmiles = totalmiles + decimal.ToInt32(feesitem.Amount);
                        }

                        if (feesitem.SubItems != null && feesitem.SubItems.Any())
                        {
                            feesitem.SubItems.ForEach(item =>
                            {
                                var tripproduct = new ProductSegmentDetail
                                {
                                    TripId = item.TripIndex,
                                    ProductId = item.Key,
                                };

                                pointsmilesproduct.Segments = (pointsmilesproduct.Segments == null)
                                ? new List<United.Mobile.Model.Shopping.ProductSegmentDetail>() : pointsmilesproduct.Segments;

                                pointsmilesproduct.Segments.Add(tripproduct);
                            });
                        }

                        if (string.Equals(feesitem.Currency, _strUGC, StringComparison.OrdinalIgnoreCase))
                        {
                            pointsmilesproduct.ProdTotalPoints = totalpoints;
                        }
                        else
                        {
                            pointsmilesproduct.ProdTotalMiles = totalmiles;
                        }

                        shoppingCart.Products = (shoppingCart.Products == null)
                            ? new List<ProdDetail>() : shoppingCart.Products;

                        shoppingCart.Products.Add(pointsmilesproduct);
                    }
                    //USD
                    if (string.Equals(feesitem.Currency, _strUSD, StringComparison.OrdinalIgnoreCase))
                    {
                        totalamount = totalamount + feesitem.Amount;
                    }
                });
            }

            if (cslResponse.DisplayCart.TravelOptions != null
                && cslResponse.DisplayCart.TravelOptions.Any())
            {
                cslResponse.DisplayCart.TravelOptions.ForEach(pcuitem =>
                {

                    var pcuproduct = new ProdDetail();

                    pcuproduct.Code = _strPCU;

                    if (string.Equals(pcuitem.Currency, _strUSD, StringComparison.OrdinalIgnoreCase))
                    {
                        totalprice = totalprice + pcuitem.Amount;
                    }

                    if (pcuitem.SubItems != null && pcuitem.SubItems.Any())
                    {
                        var pcusegments = pcuitem.SubItems.Where(x => !string.IsNullOrEmpty(x.Key)).GroupBy
                                            (x => x.SegmentNumber).Select(x => x.First());

                        if (pcusegments != null && pcusegments.Any())
                        {

                            pcusegments.ForEach(segment =>
                            {
                                var segmentproducts
                                = pcuitem.SubItems.Where(x => x.SegmentNumber == segment.SegmentNumber)
                                .GroupBy(x => x.Key).Select(x => x.First());

                                var segproduct = new ProductSegmentDetail();

                                if (segmentproducts != null && segmentproducts.Any())
                                {
                                    segproduct.SegmentInfo = segment.SegmentNumber;

                                    segmentproducts.ForEach(item =>
                                    {
                                        segproduct.ProductIds
                                        = (segproduct.ProductIds == null) ? new List<string>() : segproduct.ProductIds;

                                        segproduct.ProductIds.Add(item.Key);
                                    });
                                }

                                pcuproduct.Segments = (pcuproduct.Segments == null)
                                ? new List<ProductSegmentDetail>() : pcuproduct.Segments;

                                pcuproduct.Segments.Add(segproduct);
                            });

                        }
                    }

                    pcuproduct.ProdTotalPrice = Convert.ToString(totalprice);

                    shoppingCart.Products = (shoppingCart.Products == null)
                        ? new List<ProdDetail>() : shoppingCart.Products;

                    shoppingCart.Products.Add(pcuproduct);
                });

                if (shoppingCart.Products != null && shoppingCart.Products.Any())
                {
                    var producttypes = shoppingCart.Products.Where
                        (x => !string.IsNullOrEmpty(x.Code)).Select(x => x.Code);

                    if (producttypes != null && producttypes.Any())
                        shoppingCart.PaymentTarget = string.Join(",", producttypes);
                }

                shoppingCart.TotalMiles = Convert.ToString(totalmiles);
                shoppingCart.TotalPrice = Convert.ToString(decimal.Add(totalprice, totalamount));
                shoppingCart.TotalPoints = Convert.ToString(totalpoints);
            }
            // await Task.Run(() => _sessionHelperService.SaveSessiondata<United.Mobile.Model.Shopping.MOBShoppingCart>(request.SessionId, shoppingCart.GetType().ToString()));

            return shoppingCart;
        }

        public async Task SetRegisterOfferContentMessageAsync(MOBUpgradeCabinRegisterOfferResponse response)
        {
            try
            {
                var tncsitem = await GetUpgradeCabinPageContents(dbcontext: "UpgradeCabin_TnCOptionsContent");
                if (tncsitem != null && tncsitem.Any())
                {
                    response.TnCs = tncsitem.Where(x => _availablePriceTypes.Contains(x.Product)).ToList();
                }

                response.CabinOptionMessages = await GetUpgradeCabinMessageContents(dbcontext: "UpgradeCabin_RegisterMessageContent");
            }
            catch { }
        }

        private async Task<List<MOBUpgradeCabinOptionContent>> GetUpgradeCabinPageContents(string dbcontext)
        {

           // var cabinoptions = await _documentLibraryDynamoDB.GetNewLegalDocumentsForTitlesData(new List<string> { dbcontext }, true);
            var cabinoptions = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(dbcontext, _headers.ContextValues.SessionId, true).ConfigureAwait(false);
            var upgradecabinoptions = new List<MOBUpgradeCabinOptionContent>();
            MOBUpgradeCabinOptionContent upgradecabinoption;
            List<string> subitemlist;

            try
            {
                cabinoptions.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item.Document))
                    {
                        string[] items = item.Document.Split('|');
                        if (items.Any())
                        {
                            upgradecabinoption = new MOBUpgradeCabinOptionContent
                            {
                                Header = (string.IsNullOrEmpty(items[0])) ? string.Empty : items[0],
                                ImageUrl = (string.IsNullOrEmpty(items[1])) ? string.Empty : items[1],
                                Product = (string.IsNullOrEmpty(items[2])) ? string.Empty : items[2],
                            };

                            if (string.Equals(items[2], _strUPP2, StringComparison.OrdinalIgnoreCase))
                            {
                                upgradecabinoption.Header = AppendServiceMark(items[0]);
                            }

                            string[] subitems = items[3].Split('~');
                            if (subitems.Any())
                            {
                                if (subitems.Count() == 1)
                                    upgradecabinoption.Body = (string.IsNullOrEmpty(items[3])) ? string.Empty : items[3];
                                else
                                {
                                    subitemlist = new List<string>();
                                    subitems.ForEach(subitem =>
                                    {
                                        if (string.IsNullOrEmpty(subitem) == false)
                                        {
                                            if (subitem.Contains('#'))
                                            { subitem = AppendServiceMark(subitem); }
                                            else if (subitem.Contains('%'))
                                            { subitem = AppendTradeMark(subitem); }

                                            subitemlist.Add(subitem);
                                        }
                                    });
                                    upgradecabinoption.BodyItems = subitemlist;
                                }
                            }
                            upgradecabinoptions.Add(upgradecabinoption);
                        }
                    }
                });
                return upgradecabinoptions;
            }
            catch { return null; }
        }

        private MOBUpgradeCabinPriceDetails SetUpgradePriceDetails(List<MOBUpgradeOption> upgradeproducts, UpgradeEligibilityResponse upgradeeligibilityresponse, FlightReservationResponse flightreservationresponse, string mpaccountnumber)
        {
            MOBUpgradeCabinPriceDetails priceDetails = new MOBUpgradeCabinPriceDetails();

            priceDetails.Prices = (priceDetails.Prices == null) ? new List<Model.SeatMap.MOBSHOPPrice>() : priceDetails.Prices;

            int numberoftravelers = upgradeeligibilityresponse.Travelers.Count;

            decimal totalamount = 0;
            decimal totalduelateramount = 0;
            Int32 totalpoints = 0;
            Int32 totalmiles = 0;
            Int32 totalduelaterpoints = 0;
            Int32 totalduenowpoints = 0;

            if (flightreservationresponse.DisplayCart.DisplayFees != null
                && flightreservationresponse.DisplayCart.DisplayFees.Any())
            {
                var miles = flightreservationresponse.DisplayCart.DisplayFees.Where
                    (x => string.Equals(x.Currency, _strMILES, StringComparison.OrdinalIgnoreCase));

                if (miles != null && miles.Any())
                {
                    miles.ForEach(item =>
                    { totalmiles = totalmiles + Convert.ToInt32(item.Amount); });

                    priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strMILES, _strMUA, _strTOTALMILES, Convert.ToString(totalmiles)));
                }

                var usdamount = flightreservationresponse.DisplayCart.DisplayFees.Where
                    (x => string.Equals(x.Currency, _strUSD, StringComparison.OrdinalIgnoreCase));
                if (usdamount != null && usdamount.Any())
                {
                    var usdamounttaxnowobj = usdamount.Where(x => (x.SubItems != null && x.SubItems.Any
                        (y => string.Equals(y.QuoteType, _strQTCHECKOUT, StringComparison.OrdinalIgnoreCase)))).Select(x => x.SubItems);
                    if (usdamounttaxnowobj != null && usdamounttaxnowobj.Any())
                    {
                        usdamounttaxnowobj.ForEach(item =>
                        {
                            if (item != null && item.Any())
                            {
                                item.ForEach(sub =>
                                {
                                    totalamount = totalamount + sub.Amount;
                                });
                            }
                        });
                        if (totalamount > 0)
                        {
                            priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strUSD, _strUSD, _strTOTALPRICE, Convert.ToString(totalamount)));
                        }
                    }

                    var usdamounttaxlaterobj = usdamount.Where(x => (x.SubItems != null && x.SubItems.Any
                        (y => string.Equals(y.QuoteType, _strQTPOSTCONFIRMATION, StringComparison.OrdinalIgnoreCase)))).Select(x => x.SubItems);
                    if (usdamounttaxlaterobj != null && usdamounttaxlaterobj.Any())
                    {
                        usdamounttaxlaterobj.ForEach(item =>
                        {
                            if (item != null && item.Any())
                            {
                                item.ForEach(sub =>
                                {
                                    totalduelateramount = totalduelateramount + sub.Amount;
                                });
                            }
                        });
                        if (totalduelateramount > 0)
                        {
                            priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strUSD, _strUSD, _strDUELATERPRICE, Convert.ToString(totalduelateramount)));
                        }
                    }
                }

                var ugcpoints = flightreservationresponse.DisplayCart.DisplayFees.Where
                    (x => string.Equals(x.Currency, _strUGC, StringComparison.OrdinalIgnoreCase));
                if (ugcpoints != null && ugcpoints.Any())
                {
                    //Due now Points   
                    var duenowpointsobj = ugcpoints.Where(x => (x.SubItems != null && x.SubItems.Any
                        (y => string.Equals(y.QuoteType, _strQTCHECKOUT, StringComparison.OrdinalIgnoreCase)))).Select(x => x.SubItems);
                    if (duenowpointsobj != null && duenowpointsobj.Any())
                    {
                        //duenowpointsobj.ForEach(item => { totalduenowpoints = totalduenowpoints + item..Amount; });
                        duenowpointsobj.ForEach(item =>
                        {
                            if (item != null && item.Any())
                            {
                                item.ForEach(sub =>
                                {
                                    totalduenowpoints = totalduenowpoints + Convert.ToInt32(sub.Amount);
                                });
                            }
                        });
                        if (totalduenowpoints > 0)
                        {
                            totalduenowpoints = totalduenowpoints * numberoftravelers;
                            priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strUGC, _strUGC, _strDUENOWPOINTS, Convert.ToString(totalduenowpoints)));
                            priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strUGC, _strUGC, _strTOTALPOINTS, Convert.ToString(totalduenowpoints)));
                        }
                    }
                    //Due later Points
                    var duelaterpointsobj = ugcpoints.Where(x => (x.SubItems != null && x.SubItems.Any
                        (y => string.Equals(y.QuoteType, _strQTPOSTCONFIRMATION, StringComparison.OrdinalIgnoreCase)))).Select(x => x.SubItems);
                    if (duelaterpointsobj != null && duelaterpointsobj.Any())
                    {
                        duelaterpointsobj.ForEach(item =>
                        {
                            item.ForEach(subitem =>
                            {
                                if (Int32.TryParse(Convert.ToString(subitem.Amount), out Int32 number))
                                {
                                    totalduelaterpoints = totalduelaterpoints + number;
                                }
                            });
                        });

                        if (totalduelaterpoints > 0)
                        {
                            totalduelaterpoints = totalduelaterpoints * numberoftravelers;
                            priceDetails.Prices.Add(AddMOBSHOPPriceItem
                                (_strUGC, _strUGC, _strDUELATERPOINTS, Convert.ToString(totalduelaterpoints)));
                        }
                    }
                }
            }

            if (flightreservationresponse.DisplayCart.TravelOptions != null
                && flightreservationresponse.DisplayCart.TravelOptions.Any())
            {
                var prices = flightreservationresponse.DisplayCart.TravelOptions.Where
                    (x => string.Equals(x.Currency, _strUSD, StringComparison.OrdinalIgnoreCase));
                if (prices != null && prices.Any())
                {
                    decimal newtotalamount = 0;
                    prices.ForEach(x => { newtotalamount = newtotalamount + x.Amount; });

                    if (priceDetails.Prices != null
                        && priceDetails.Prices.Any(x => string.Equals(x.DisplayType, _strTOTALPRICE, StringComparison.OrdinalIgnoreCase)))
                    {
                        priceDetails.Prices.Where
                            (x => string.Equals(x.DisplayType, _strTOTALPRICE, StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault().DisplayValue = Convert.ToString(newtotalamount + totalamount);
                    }
                    else
                    {
                        priceDetails.Prices.Add(AddMOBSHOPPriceItem
                            (_strUSD, _strUSD, _strTOTALPRICE, Convert.ToString(newtotalamount)));
                    }
                }
            }

            priceDetails.PaymentDeductionsMsg = PaymentDeductionsMsg(totalduenowpoints, totalmiles, totalduelaterpoints, mpaccountnumber);

            return priceDetails;
        }

        private static Model.SeatMap.MOBSHOPPrice AddMOBSHOPPriceItem(string currencycode, string pricetype, string displaytype, string displayvalue)
        {
            var mobshoppriceitem = new Model.SeatMap.MOBSHOPPrice
            {
                CurrencyCode = currencycode,
                PriceType = pricetype,
                DisplayType = displaytype,
                DisplayValue = displayvalue
            };
            return mobshoppriceitem;
        }

        public string PaymentDeductionsMsg(decimal totalduenowpoints, Int32 totalmiles, decimal totalduelaterpoints, string mpaccountnumber)
        {
            string pointsmilesmsg = string.Empty;
            string duelatermsg = string.Empty;
            string paymentdeductionsmsg = string.Empty;
            string UpgradeCabinmilespointsdeductionmsg = GetConfigEntries("UpgradeCabinmilespointsdeductionmsg");
            string UpgradeCabinpointslaterdeductionmsg = GetConfigEntries("UpgradeCabinpointslaterdeductionmsg");

            mpaccountnumber = MaskData(3, mpaccountnumber);

            if (totalduenowpoints > 0 || totalmiles > 0)
            {
                pointsmilesmsg = string.Format
                    (UpgradeCabinmilespointsdeductionmsg,
                    (totalduenowpoints > 0) ? totalduenowpoints + " PlusPoints" : string.Empty,
                    (totalduenowpoints > 0 && totalmiles > 0) ? " and " + FormatMiles(totalmiles) + "miles" :
                    (totalmiles > 0) ? FormatMiles(totalmiles) + " miles" : string.Empty, mpaccountnumber + ". ");
            }
            if (totalduelaterpoints > 0)
            {
                duelatermsg = string.Format
                    (UpgradeCabinpointslaterdeductionmsg,
                    totalduelaterpoints, (string.IsNullOrEmpty(pointsmilesmsg)) ? mpaccountnumber + ". " : string.Empty);
            }

            paymentdeductionsmsg = string.Format("{0} {1}",
                (!string.IsNullOrEmpty(pointsmilesmsg)) ? pointsmilesmsg : string.Empty,
                (!string.IsNullOrEmpty(duelatermsg) && !string.IsNullOrEmpty(pointsmilesmsg))
                ? "today. " + duelatermsg : (!string.IsNullOrEmpty(duelatermsg)) ? duelatermsg : string.Empty);

            return (string.IsNullOrEmpty(paymentdeductionsmsg)) ? string.Empty : paymentdeductionsmsg;
        }

        public string GetConfigEntries(string configKey)
        {
            try
            {
                var configString = _configuration.GetValue<string>(configKey) ?? string.Empty;
                return configString = (configString.IsNullOrEmpty()) ? string.Empty : configString;
            }
            catch { return string.Empty; }
        }

        #region "UTILITY"

        public static string MaskData(int digitscount, string data)
        {
            try
            {
                string unmaskeddata = String.Concat(Enumerable.Take(data, data.Length).Skip(data.Length - digitscount));
                string msakeddata = String.Concat(Enumerable.Repeat("*", data.Length - digitscount));
                return string.Concat(msakeddata, unmaskeddata);
            }
            catch { return string.Empty; }
        }

        #endregion

        private string EncryptString(string data)
        {
            return United.ECommerce.Framework.Utilities.SecureData.EncryptString(data);
        }

    }
}
