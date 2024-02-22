using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.SeatEngine;
using United.Mobile.DataAccess.ShopSeats;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Helper;
using Reservation = United.Mobile.Model.Shopping.Reservation;
using MOBSeatMap = United.Mobile.Model.Shopping.MOBSeatMap;
using PcuUpgradeOption = United.Mobile.Model.Shopping.Pcu.PcuUpgradeOption;
using Seat = United.Definition.SeatCSL30.Seat;
using SeatMapRequest = United.Definition.SeatCSL30.SeatMapRequest;
using Session = United.Mobile.Model.Common.Session;
using United.Mobile.Model.ShopSeats;
using United.Mobile.Model;
using United.Definition.SeatCSL30;
using United.Service.Presentation.ReferenceDataResponseModel;
using United.Service.Presentation.ReferenceDataRequestModel;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.Shopping.Booking;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Web;
using United.Utility.Enum;
using United.Service.Presentation.ReferenceDataModel;

namespace United.Common.HelperSeatEngine
{
    public class SeatMapEngine: ISeatMapEngine
    {
        private readonly ICacheLog<SeatMapEngine> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IHeaders _headers;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ISeatMapCSL30Service _seatMapCSL30service;
        private readonly IComplimentaryUpgradeService _complimentaryUpgradeService;
        private readonly IFeatureSettings _featureSettings;
        public SeatMapEngine(ICacheLog<SeatMapEngine> logger,IConfiguration configuration
            , IDynamoDBService dynamoDBService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IHeaders headers
            , IShoppingUtility shoppingUtility
            , ISessionHelperService sessionHelperService
            , ISeatMapCSL30Service seatMapCSL30service
            , IComplimentaryUpgradeService complimentaryUpgradeService,
            IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
            _shoppingUtility = shoppingUtility;
            _sessionHelperService = sessionHelperService;
            _seatMapCSL30service = seatMapCSL30service;
            _complimentaryUpgradeService = complimentaryUpgradeService;
            _featureSettings = featureSettings;
        }

        #region - Duplicate code
        /*
        public bool ShowSeatMapForCarriers(string operatingCarrier)
        {
            if (_configuration.GetValue<string>("ShowSeatMapAirlines") != null)
            {
                string[] carriers = _configuration.GetValue<string>("ShowSeatMapAirlines").Split(',');
                foreach (string carrier in carriers)
                {
                    if (operatingCarrier != null && carrier.ToUpper().Trim().Equals(operatingCarrier.ToUpper().Trim()))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public string GetFareBasicCodeFromBundles(List<TravelOption> travelOptions, int tripId, string defaultFareBasisCode, string destination, string origin)
        {
            // string strSegmentname = origin + " - " + destination;
            if (travelOptions == null || travelOptions.Count <= 0)
                return defaultFareBasisCode;

            foreach (var travelOption in travelOptions)
            {
                foreach (var bundleCode in travelOption.BundleCode)
                {
                    if (bundleCode.AssociatedTripIndex == tripId && !string.IsNullOrEmpty(bundleCode.ProductKey))
                    {
                        return bundleCode.ProductKey;
                    }
                }
            }

            return defaultFareBasisCode;
        }
        public string GetOperatedByText(string marketingCarrier, string flightNumber, string operatingCarrierDescription)
        {
            if (string.IsNullOrEmpty(marketingCarrier) ||
                string.IsNullOrEmpty(flightNumber) ||
                string.IsNullOrEmpty(operatingCarrierDescription))
                return string.Empty;
            operatingCarrierDescription = ShopStaticUtility.RemoveString(operatingCarrierDescription, "Limited");
            return marketingCarrier + flightNumber + " operated by " + operatingCarrierDescription;
        }
       
        public async Task<string> GetDocumentTextFromDataBase(string title)
        {
            var messagesFromDb = await GetMPPINPWDTitleMessages(title).ConfigureAwait(false);
            return messagesFromDb != null && messagesFromDb.Any() ? messagesFromDb[0].CurrentValue : string.Empty;
        }
        public async Task<string> ShowOaSeatMapAvailabilityDisclaimerText()
        {
            return await GetDocumentTextFromDataBase("OA_SEATMAP_DISCLAIMER_TEXT");
        }

        public void EconomySeatsForBUSService(Mobile.Model.Shopping.MOBSeatMap seats, bool operated = false)
        {
            if (_configuration.GetValue<bool>("EnableProjectTOM") && seats != null && seats.FleetType.Length > 1 && seats.FleetType.Substring(0, 2).ToUpper().Equals("BU"))
            //if (seats != null && seats.FleetType.Substring(0, 2).ToUpper().Equals("BU"))
            {
                string seatMapLegendEntry = _configuration.GetValue<string>("seatMapLegendEntry");
                string seatMapLegendKey = _configuration.GetValue<string>("seatMapLegendKey");
                seats.SeatLegendId = seatMapLegendKey + "|" + seatMapLegendEntry;

                //seats.SeatLegendId = "seatmap_legendTOM|Available|Unavailable";
                seats.IsOaSeatMap = true;
                seats.Cabins[0].COS = string.Empty;
                seats.IsReadOnlySeatMap = !operated;
                seats.OperatedByText = operated ? _configuration.GetValue<string>("ProjectTOMOperatedByText") : "";

                foreach (var cabin in seats.Cabins)
                {
                    foreach (var row in cabin.Rows)
                    {
                        row.Number = row.Number.PadLeft(2, '0');
                        row.Wing = false;
                    }
                }
            }
        }
       
        private async Task<List<MOBItem>> GetMPPINPWDTitleMessages(string titleList)
        {
            List<MOBItem> messages = new List<MOBItem>();
            List<United.Definition.MOBLegalDocument> docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(titleList, _headers.ContextValues.TransactionId, (!_configuration.GetValue<bool>("DisableManageRes23C_P3")) ? true : false);
            if (docs != null && docs.Count > 0)
            {
                foreach (United.Definition.MOBLegalDocument doc in docs)
                {
                    MOBItem item = new MOBItem();
                    item.Id = doc.Title;
                    item.CurrentValue = doc.LegalDocument;
                    messages.Add(item);
                }
            }
            return messages;
        }
       
        public bool SupressLMX(int appId)
        {
            bool supressLMX = false;
            bool.TryParse(_configuration.GetValue<string>("SupressLMX"), out supressLMX); // ["SupressLMX"] = true to make all Apps Turn off. ["SupressLMX"] = false then will check for each app as below.
            if (!supressLMX && _configuration.GetValue<string>("AppIDSToSupressLMX") != null && _configuration.GetValue<string>("AppIDSToSupressLMX").Trim() != "")
            {
                string appIDS = _configuration.GetValue<string>("AppIDSToSupressLMX"); // AppIDSToSupressLMX = ~1~2~3~ or ~1~ or empty to allow lmx to all apps
                supressLMX = appIDS.Contains("~" + appId.ToString() + "~");
            }
            return supressLMX;
        }
        public void CountNoOfFreeSeatsAndPricedSeats(MOBSeatB seat, ref int countNoOfFreeSeats, ref int countNoOfPricedSeats)
        {
            if (seat.IsNullOrEmpty() || seat.seatvalue.IsNullOrEmpty() ||
                 seat.seatvalue == "-" || seat.seatvalue.ToUpper() == "X" || seat.IsPcuOfferEligible)
                return;

            if (seat.ServicesAndFees.IsNullOrEmpty())
            {
                countNoOfFreeSeats++;
            }
            else if (seat.ServicesAndFees[0].Available && seat.ServicesAndFees[0].TotalFee <= 0)
            {
                countNoOfFreeSeats++;
            }
            else if (seat.ServicesAndFees[0].Available)
            {
                countNoOfPricedSeats++;
            }
        }

        public async Task<List<MOBItem>> GetPcuCaptions(string travelerNames, string recordLocator)
        {
            var pcuCaptions = await GetCaptions("PCU_IN_SEATMAP_PRODUCTPAGE");
            if (pcuCaptions == null || !pcuCaptions.Any() || string.IsNullOrEmpty(travelerNames))
                return null;

            pcuCaptions.Add(new MOBItem { Id = "PremiumSeatTravelerNames", CurrentValue = travelerNames });
            pcuCaptions.Add(new MOBItem { Id = "RecordLocator", CurrentValue = recordLocator });
            return pcuCaptions;
        }
        private async Task<List<MOBItem>> GetCaptions(string key)
        {
            return !string.IsNullOrEmpty(key) ? await GetCaptions(key, true) : null;
        }
        private async Task<List<MOBItem>> GetCaptions(string keyList, bool isTnC)
        {
            var docs = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(keyList, _headers.ContextValues.TransactionId, isTnC).ConfigureAwait(false);
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

        public bool IsCabinMatchedCSL(string pcuCabin, string seatmapCabin)
        {
            if (string.IsNullOrEmpty(pcuCabin) || string.IsNullOrEmpty(seatmapCabin))
                return false;

            pcuCabin = pcuCabin.Trim().Replace("®", "").Replace("℠", "").ToUpper();
            seatmapCabin = seatmapCabin.ToUpper().Trim();

            if (pcuCabin.Equals(seatmapCabin, StringComparison.OrdinalIgnoreCase))
                return true;

            var possiblefirstCabins = new List<string> { "FIRST", "UNITED FIRST", "UNITED GLOBAL FIRST", "UNITED POLARIS FIRST" };
            if (possiblefirstCabins.Contains(seatmapCabin) && possiblefirstCabins.Contains(pcuCabin))
                return true;

            var possibleBusinessCabins = new List<string> { "UNITED BUSINESS", "UNITED BUSINESS", "UNITED POLARIS BUSINESS", "BUSINESSFIRST", "UNITED BUSINESSFIRST" };
            if (possibleBusinessCabins.Contains(seatmapCabin) && possibleBusinessCabins.Contains(pcuCabin))
                return true;

            var possibleUppCabins = new List<string> { "UNITED PREMIUM PLUS", "UNITED PREMIUMPLUS" };
            if (possibleUppCabins.Contains(seatmapCabin) && possibleUppCabins.Contains(pcuCabin))
                return true;

            return false;
        }

        public async Task<List<MOBSeatMap>> GetCSL30SeatMapForRecordLocatorWithLastName(string sessionId, string recordLocator, int segmentIndex, string languageCode, string bookingCabin, string lastName, bool cogStop, string origin, string destination, int applicationId, string appVersion, bool isELF, bool isIBE, int noOfTravelersWithNoSeat1, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, List<TripSegment> tripSegments, string operatingCarrierCode, string deviceId, List<MOBBKTraveler> BookingTravelerInfo, string flow, bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false, bool isSeatFocusEnabled = false, List<MOBItem> catalog = null)
        {
            if (!ConfigUtility.EnableAirCanada(applicationId, appVersion) && operatingCarrierCode != null
                    && operatingCarrierCode.Trim().ToUpper() == "AC")
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines") != null ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines").ToString() : string.Empty);
            }
            if (ConfigUtility.EnableLufthansa(operatingCarrierCode))
            {
                if (!ConfigUtility.EnableLufthansaForHigherVersion(operatingCarrierCode, applicationId, appVersion))
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                }
            }

            bool isDeepLink = false;
            if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && !string.IsNullOrEmpty(operatingCarrierCode))
            {
                isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog);
            }

            string[] channelInfo = _configuration.GetValue<string>("CSL30MMRChannelInfo").Split('|');
            var request = BuildSeatMapRequest(flow, languageCode, channelInfo[0], channelInfo[1], recordLocator, false, isIBE); //BuildSeatMapRequest(flow, languageCode, tripSegments, segmentIndex, channelInfo[0], channelInfo[1], recordLocator);

            bool isSeatMapTravelerDetailsEnabled = await _featureSettings.GetFeatureSettingValue("EnableTravelerInfoinSeatmapRequest").ConfigureAwait(false);

            if (isSeatMapTravelerDetailsEnabled)
            {
                request.Travelers = BuildTravelersDetails(BookingTravelerInfo, applicationId, appVersion);
            }

            request.FlightSegments = BuildFlightSegmentsForManageRes(tripSegments, segmentIndex, origin, destination, isSeatFocusEnabled);
            request.BundleCode = tripSegments?.FirstOrDefault(t => t?.OriginalSegmentNumber == segmentIndex)?.BundleProductCode;
            if (_configuration.GetValue<bool>("EnablePBE"))
            {
                string productCode = isIBE ? tripSegments?.FirstOrDefault()?.ProductCode : string.Empty;
                if (isIBE && !string.IsNullOrEmpty(productCode))
                {
                    request.ProductCode = productCode;
                }
            }
            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            string url = string.Empty;
            string cslRequest = DataContextJsonSerializer.Serialize<United.Definition.SeatCSL30.SeatMapRequest>(request);


            var cslstrResponse = await GetSelectSeatMapResponse(url, sessionId, cslRequest, session.Token, channelInfo[0], channelInfo[1], string.Empty);

            _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {ApplicationId} {ApplicationVersion} {DeviceId} {CSLResponse} and {SessionId}", applicationId, appVersion, deviceId, cslstrResponse, sessionId);

            List<MOBSeatMap> seatMaps = null;
            United.Definition.SeatCSL30.SeatMap response = new United.Definition.SeatCSL30.SeatMap();
            if (!string.IsNullOrEmpty(cslstrResponse))
            {
                response = JsonConvert.DeserializeObject<United.Definition.SeatCSL30.SeatMap>(cslstrResponse);
            }

            if (response != null && response.FlightInfo != null && response.Cabins != null && response.Cabins.Count > 0 && response.Errors.IsNullOrEmpty())
            {
                seatMaps = new List<MOBSeatMap>();

                MOBSeatMap aSeatMap = await BuildSeatMapCSL30(response, response.Travelers.Count, bookingCabin, cogStop, sessionId, isELF, isIBE, noOfTravelersWithNoSeat1, noOfFreeEplusEligibleRemaining, isOaSeatMapSegment, segmentIndex, flow, "", applicationId, appVersion, isOneofTheSegmentSeatMapShownForMultiTripPNRMRes, operatingCarrierCode, catalog);
                seatMaps.Add(aSeatMap);
            }
            else if (isOaSeatMapSegment || (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && isDeepLink))
            {
                // Not throwing error for OA. Sent message in object instead. 
                if (!_shoppingUtility.EnableOAMsgUpdateFixViewRes(applicationId, appVersion))
                {
                    if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion))
                    {
                        if (!isDeepLink)
                        {
                            throw new MOBUnitedException(HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage")));
                        }
                    }
                    else
                    {
                        if (ConfigUtility.EnableAirCanada(applicationId, appVersion) && operatingCarrierCode != null && operatingCarrierCode.Trim().ToUpper() == "AC")
                        {
                            if (!ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog))
                            {
                                if (_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation") != null)
                                {
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableAC_Managereservation").ToString());
                                }
                                string seatMapError = _configuration.GetValue<string>("AirCanadaSeatmapError");
                                if (response != null && response.Errors != null)
                                {
                                    if (response.Errors.Any(x => !x.IsNullOrEmpty() && !x.Message.IsNullOrEmpty() && x.Message.Equals(seatMapError, StringComparison.OrdinalIgnoreCase)))
                                    {
                                        throw new MOBUnitedException(_configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation") != null ? _configuration.GetValue<string>("AirCanadaSeatMapNonTicketed_Managereservation").ToString() : string.Empty);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode, applicationId, appVersion, catalog))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation") != null ? _configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation").ToString() : string.Empty);
                            }
                        }
                    }
                }
            }
            else
            {
                string errorMessage = string.Empty;
                if (response != null && response.Errors != null && response.Errors.Any())
                {
                    foreach (var error in response.Errors)
                    {
                        errorMessage = errorMessage + " " + error.Message;
                    }
                }
                if (!string.IsNullOrEmpty(errorMessage))
                {
                    _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {MOBUnitedException} and {SessionId}", applicationId, appVersion, string.Empty, errorMessage, sessionId);

                    if (errorMessage.Contains("BUS"))
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SelectSeats_BusServiceError"));
                    }
                }
            }

            _logger.LogInformation("GetCSL30SeatMapForRecordLocatorWithLastName {Client-SeatMapResponse} and {SessionId}", seatMaps, sessionId);
            return seatMaps;
        }

        private SeatMapRequest BuildSeatMapRequest
        (string flow, string languageCode, string channelId, string channelName,
        string recordLocator, bool isAwardReservation, bool isIBE)
        {
            SeatMapRequest request = new SeatMapRequest();

            if (!string.IsNullOrEmpty(recordLocator))
            {
                request.RecordLocator = recordLocator;
            }

            request.ChannelId = channelId;
            request.ChannelName = channelName;
            request.IsAwardReservation = isAwardReservation;
            request.ProductCode = isIBE
                ? _configuration.GetValue<string>("IBEProductDescription") : string.Empty;

            request.IsFrontCabin = true;
            request.IsUppCabin = true;
            request.Travelers = null;
            return request;
        }

        private ICollection<Traveler2> BuildTravelersDetails(List<MOBBKTraveler> MOBBKtravelers, int applicationId, string appVersion)
        {
            List<Traveler2> travelers = new List<Traveler2>();

            int i = 1;

            foreach (MOBBKTraveler MOBBKTraveler in MOBBKtravelers)
            {
                Traveler2 traveler = new Traveler2();
                traveler.Id = i;
                traveler.TravelerIndex = MOBBKTraveler.SHARESPosition;
                traveler.FirstName = MOBBKTraveler.Person.GivenName;
                traveler.LastName = MOBBKTraveler.Person.Surname;
                traveler.DateOfBirth = MOBBKTraveler.Person.DateOfBirth;
                traveler.Type = MOBBKTraveler.TravelerTypeCode;
                traveler.PassengerTypeCode = MOBBKTraveler.TravelerTypeCode;
                traveler.LoyaltyProfiles = new List<LoyaltyProfile>();

                if (MOBBKTraveler.LoyaltyProgramProfile != null)
                {
                    LoyaltyProfile loyaltyProfile = new LoyaltyProfile();
                    loyaltyProfile.ProgramId = MOBBKTraveler.LoyaltyProgramProfile.CarrierCode;
                    loyaltyProfile.MemberShipId = MOBBKTraveler.LoyaltyProgramProfile.MemberId;

                    traveler.LoyaltyProfiles.Add(loyaltyProfile);
                }

                travelers.Add(traveler);
                i++;
            }

            return travelers;
        }


        private Collection<United.Definition.SeatCSL30.FlightSegments> BuildFlightSegmentsForManageRes(List<TripSegment> tripSegments, int segmentIndex, string origin, string destination, bool isSeatFocusEnabled)
        {
            var flightSegments = new Collection<United.Definition.SeatCSL30.FlightSegments>();
            var segmentsCopy = tripSegments.Clone();
            if (segmentIndex > 0)
            {
                if (isSeatFocusEnabled)
                {
                    segmentsCopy = tripSegments.FindAll(t => !t.IsNullOrEmpty() && t.OriginalSegmentNumber == segmentIndex);
                }
                else
                {
                    segmentsCopy = tripSegments.FindAll(t => !t.IsNullOrEmpty() && t.SegmentIndex == segmentIndex);
                }
            }

            if (!segmentsCopy.IsNullOrEmpty())
            {
                int id = 1;
                foreach (var segment in segmentsCopy)
                {
                    if (!segment.IsNullOrEmpty() && !segment.Arrival.IsNullOrEmpty() && !segment.Departure.IsNullOrEmpty())
                    {
                        if (segment.COGStop)
                        {
                            if (segment.Departure.Code == origin && segment.Arrival.Code == destination)
                            {
                                var flightSegment = BuildFlightSegmentsForSeatMap(segment.Departure.Code, segment.Arrival.Code, segment.IsCheckInWindow, segment.ServiceClass, segment.ScheduledDepartureDate, string.Empty, segment.FareBasisCode, Convert.ToInt32(segment.FlightNumber), segment.MarketingCarrier, segment.OperatingCarrier, segment.SegmentIndex, "true", id);
                                id++;
                                flightSegments.Add(flightSegment);
                            }
                        }
                        else
                        {
                            var flightSegment = BuildFlightSegmentsForSeatMap(segment.Departure.Code, segment.Arrival.Code, segment.IsCheckInWindow, segment.ServiceClass, segment.ScheduledDepartureDate, string.Empty, segment.FareBasisCode, Convert.ToInt32(segment.FlightNumber), segment.MarketingCarrier, segment.OperatingCarrier, segment.SegmentIndex, "true", id);
                            id++;
                            flightSegments.Add(flightSegment);
                        }
                    }
                }
            }

            return flightSegments;
        }
        private async Task<string> GetSelectSeatMapResponse(string url, string sessionId, string cslRequest, string token, string channelId, string channelName, string path = "", bool isOperatedByUA = true, int appID = -1, string appVersion = "")
        {
            try
            {
                return await _seatMapCSL30service.GetSeatMapDeatils(token, sessionId, cslRequest, channelId, channelName, path).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                string exReader = ExceptionMessages(ex);

                string seatMapUnavailable = string.Empty;
                if (!string.IsNullOrEmpty(_configuration.GetValue<string>("SeatMapUnavailable-MinorDescription")))
                {
                    seatMapUnavailable = _configuration.GetValue<string>("SeatMapUnavailable-MinorDescription");
                    string[] seatMapUnavailableMinorDescription = seatMapUnavailable.Split('|');

                    if (!string.IsNullOrEmpty(exReader))
                    {
                        foreach (string minorDescription in seatMapUnavailableMinorDescription)
                        {
                            if (_shoppingUtility.EnableOAMessageUpdate(appID, appVersion) && !isOperatedByUA)
                            {
                                if (exReader.Contains(minorDescription))
                                {
                                    throw new MOBUnitedException(HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage")));
                                }
                                else
                                {
                                    throw new Exception(HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage")));
                                }
                            }
                            else if (exReader.Contains(minorDescription))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("OASeatMapUnavailableMessage"));
                            }
                        }
                    }
                }
                throw new Exception(exReader);
            }
        }
        private United.Definition.SeatCSL30.FlightSegments BuildFlightSegmentsForSeatMap(string origin, string destination, bool isCheckinEligible, string classOfService, string depatureDate, string arrivalDate, string fareBasisCode, int flightNumber, string marketingCarrier, string operatingCarrier, int segmentNumber, string pricing, int id)
        {
            var flightSegment = new United.Definition.SeatCSL30.FlightSegments();
            flightSegment.ArrivalAirport = new Definition.SeatCSL30.Airport { IataCode = destination }; ;
            flightSegment.CheckInSegment = isCheckinEligible;
            flightSegment.ClassOfService = classOfService;
            flightSegment.DepartureAirport = new Definition.SeatCSL30.Airport { IataCode = origin };
            flightSegment.DepartureDateTime = depatureDate;
            if (!string.IsNullOrEmpty(arrivalDate))
            {
                flightSegment.ArrivalDateTime = arrivalDate;
            }
            flightSegment.FarebasisCode = fareBasisCode;
            flightSegment.FlightNumber = flightNumber;
            flightSegment.Id = id;
            flightSegment.OperatingFlightNumber = flightNumber;
            flightSegment.OperatingAirlineCode = operatingCarrier;
            flightSegment.MarketingAirlineCode = marketingCarrier;
            flightSegment.SegmentNumber = segmentNumber;
            flightSegment.Pricing = pricing;
            return flightSegment;
        }
        private string ExceptionMessages(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return ex.Message + " | " + ExceptionMessages(ex.InnerException);
        }
        public async Task<MOBSeatMap> BuildSeatMapCSL30(United.Definition.SeatCSL30.SeatMap seatMapResponse, int numberOfTravelers, string bookingCabin, bool cogStop, string sessionId, bool isELF, bool isIBE, int noOfTravelersWithNoSeat, int noOfFreeEplusEligibleRemaining, bool isOaSeatMapSegment, int segmentIndex, string flow, string token = "", int appId = -1, string appVersion = "", bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false, string operatingCarrierCode = "", List<MOBItem> catalog = null)
        {
            int countNoOfFreeSeats = 0;
            int countNoOfPricedSeats = 0;
            MOBSeatMap objMOBSeatMap = new MOBSeatMap();


            bool isEnablePcuDeepLinkInSeatMap = ConfigUtility.EnablePcuDeepLinkInSeatMap(appId, appVersion);
            bool isEnablePCUSeatMapPurchaseManageRes = await IsEnablePCUSeatMapPurchaseManageRes(appId, appVersion, numberOfTravelers, catalog);
            var tupleResponse = await GetSeatMapCSL(seatMapResponse, sessionId, isELF, isIBE, isOaSeatMapSegment, segmentIndex, flow, appId, appVersion, isEnablePcuDeepLinkInSeatMap, isEnablePCUSeatMapPurchaseManageRes, countNoOfFreeSeats, countNoOfPricedSeats, bookingCabin, cogStop);

            objMOBSeatMap = tupleResponse.Item1;
            countNoOfFreeSeats = tupleResponse.countNoOfFreeSeats;
            countNoOfPricedSeats = tupleResponse.countNoOfPricedSeats;

            bool isDeepLinkSupportedAirline = _configuration.GetValue<bool>("EnableRedesignForInterlineDeepLink") && ConfigUtility.IsDeepLinkSupportedAirline(operatingCarrierCode) && ConfigUtility.CheckClientCatalogForEnablingFeature("InterlineDeepLinkRedesignClientCatalog", catalog);
            if (_shoppingUtility.EnableOAMessageUpdate(appId, appVersion))
            {
                if ((_shoppingUtility.EnableOAMsgUpdateFixViewRes(appId, appVersion) && objMOBSeatMap.IsOaSeatMap && (countNoOfFreeSeats == 0 || countNoOfFreeSeats <= numberOfTravelers)))
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = true;
                }
                // Not throwing error for OA. Sent message in object instead. 
                if (!_shoppingUtility.EnableOAMsgUpdateFixViewRes(appId, appVersion) && objMOBSeatMap.IsOaSeatMap && (countNoOfFreeSeats == 0 || countNoOfFreeSeats <= numberOfTravelers))
                {
                    throw new MOBUnitedException(HttpUtility.HtmlDecode(_configuration.GetValue<string>("OANoSeatMapAvailableMessage")));
                }
            }
            else
            {
                if (objMOBSeatMap.IsOaSeatMap && countNoOfFreeSeats == 0)
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = isDeepLinkSupportedAirline ? true : false;
                    if (!isDeepLinkSupportedAirline)
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
                    }
                }

                if (Convert.ToBoolean(_configuration.GetValue<string>("checkForPAXCount")) && objMOBSeatMap.IsOaSeatMap)
                {
                    objMOBSeatMap.HasNoComplimentarySeatsAvailableForOA = isDeepLinkSupportedAirline ? true : false;
                    if (countNoOfFreeSeats <= numberOfTravelers)
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines_Managereservation"));
                    }
                }
            }
            if (_configuration.GetValue<bool>("EnableSocialDistanceMessagingForSeatMap") && !isOneofTheSegmentSeatMapShownForMultiTripPNRMRes)
            {
                objMOBSeatMap.ShowInfoMessageOnSeatMap = _configuration.GetValue<string>("SocialDistanceSeatDisplayMessageDetailBody") + _configuration.GetValue<string>("SocialDistanceSeatMapMessagePopup");
            }
            else
            {
                objMOBSeatMap.ShowInfoMessageOnSeatMap = objMOBSeatMap.IsOaSeatMap ?
                    _shoppingUtility.EnableOAMessageUpdate(appId, appVersion) ?
                        _configuration.GetValue<string>("SeatMapMessageForEligibleOA") :
                                                _configuration.GetValue<string>("ShowFreeSeatsMessageForOtherAilines") :
                                               await ShowNoFreeSeatsAvailableMessage(noOfTravelersWithNoSeat, noOfFreeEplusEligibleRemaining, countNoOfFreeSeats, countNoOfPricedSeats, (isELF || isIBE));
                if (_configuration.GetValue<bool>("DisableFreeSeatMessageChanges") == false && objMOBSeatMap.IsOaSeatMap == false
                         && string.IsNullOrEmpty(objMOBSeatMap.ShowInfoMessageOnSeatMap) == false)
                    objMOBSeatMap.ShowInfoTitleForOA = _configuration.GetValue<string>("NoFreeSeatAvailableMessageHeader");
            }

            EconomySeatsForBUSService(objMOBSeatMap);

            return objMOBSeatMap;
        }
        public List<string> GetPolarisCabinBranding(string authenticationToken, string flightNumber, string departureAirportCode, string flightDate, string arrivalAirportCode, string cabinCount, string languageCode, string sessionId, string operatingCarrier = "UA", string marketingCarrier = "UA")
        {
            List<string> Cabins = null;
            CabinRequest cabinRequest = new CabinRequest();
            CabinResponse cabinResponse = new CabinResponse();

            //Buiding the cabinRequest
            //cabinRequest.CabinCount = cabinCount;
            cabinRequest.CabinCount = cabinCount;
            cabinRequest.DestinationAirportCode = arrivalAirportCode;
            cabinRequest.FlightDate = flightDate;//DateTime.Parse(request.FlightDate).ToString("yyyy-mm-dd");
            cabinRequest.FlightNumber = flightNumber;
            cabinRequest.LanguageCode = languageCode;
            cabinRequest.MarketingCarrier = marketingCarrier;
            cabinRequest.OperatingCarrier = operatingCarrier;
            cabinRequest.OriginAirportCode = departureAirportCode;
            //cabinRequest.ServiceClass = request.

            //try
            //{
            //    string jsonRequest = JsonConvert.SerializeObject(cabinRequest);
            //    if (this.levelSwitch.TraceError)
            //    {
            //        LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetPolarisCabinBranding", "CSLRequest", jsonRequest));
            //    }
            //    string url = ConfigurationManager.AppSettings["CabinBrandingService - URL"];
            //    if (this.levelSwitch.TraceError)
            //    {
            //        LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetPolarisCabinBranding", "CSL URL", url));
            //    }

            //    #region//****Get Call Duration Code - *******
            //    Stopwatch cslStopWatch;
            //    cslStopWatch = new Stopwatch();
            //    cslStopWatch.Reset();
            //    cslStopWatch.Start();
            //    #endregion//****Get Call Duration Code - *******
            //    string jsonResponse = HttpHelper.Post(url, "application/json; charset=utf-8", authenticationToken, jsonRequest);
            //    #region// 2 = cslStopWatch//****Get Call Duration Code - Venkat 03/17/2015*******
            //    if (cslStopWatch.IsRunning)
            //    {
            //        cslStopWatch.Stop();
            //    }
            //    string cslCallTime = (cslStopWatch.ElapsedMilliseconds / (double)1000).ToString();
            //    if (this.levelSwitch.TraceError)
            //    {
            //        LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "Polaris Cabin Branding Service - CSL Call Duration", "CSS/CSL-CallDuration", "CSLSeatMapDetail=" + cslCallTime));
            //    }
            //    #endregion//****Get Call Duration Code - Venkat 03/17/2015*******   
            //    //if (this.levelSwitch.TraceError)
            //    //{
            //    //    LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetPolarisCabinBranding", "CSLResponse", jsonResponse));
            //    //}


            //    if (!string.IsNullOrEmpty(jsonResponse))
            //    {
            //        CabinResponse response = JsonSerializer.NewtonSoftDeserialize<CabinResponse>(jsonResponse);
            //        if (this.levelSwitch.TraceError)
            //        {
            //            LogEntries.Add(United.Logger.LogEntry.GetLogEntry<CabinResponse>(sessionId, "GetPolarisCabinBranding", "DeserializedResponse", response));
            //        }
            //        if (response.Errors != null && response.Errors.Count > 0)
            //        {
            //            throw new MOBUnitedException("Errors in the CabinBranding Response");
            //        }
            //        if (response != null && response.Cabins != null && response.Cabins.Count > 0)
            //        {
            //            Cabins = new List<string>();
            //            foreach (var cabin in response.Cabins)
            //            {
            //                string aCabin = cabin.Description;
            //                Cabins.Add(aCabin);
            //            }
            //        }
            //        else
            //        {
            //            throw new MOBUnitedException("United Data Services not available.");
            //        }
            //    }
            //    else
            //    {
            //        throw new MOBUnitedException("United Data Services not available.");
            //    }

            //}
            //// Added as part of SeatMap- Cabin Branding Service Logging
            //catch (WebException exx)
            //{
            //    if (this.levelSwitch.TraceError)
            //    {

            //        var exReader = new StreamReader(exx.Response.GetResponseStream()).ReadToEnd().Trim();

            //        // Added as part of Task - 283491 GetPolarisCabinBranding Exceptions
            //        if (Utility.GetBooleanConfigValue("BugFixToggleForExceptionAnalysis") && !string.IsNullOrEmpty(exReader) &&
            //            (exReader.StartsWith("{") && exReader.EndsWith("}")))
            //        {
            //            var exceptionDetails = Newtonsoft.Json.JsonConvert.DeserializeObject<MOBFlightStatusError>(exReader);
            //            if (exceptionDetails != null && exceptionDetails.Errors != null)
            //            {
            //                foreach (var error in exceptionDetails.Errors)
            //                {
            //                    if (!string.IsNullOrEmpty(error.MinorCode) && error.MinorCode.Trim().Equals("90830"))
            //                    {
            //                        throw new MOBUnitedException(ConfigurationManager.AppSettings["Booking2OGenericExceptionMessage"]);
            //                    }
            //                }

            //            }

            //        }

            //        LogEntry objLog = United.Logger.LogEntry.GetLogEntry<string>(sessionId, "GetPolarisCabinBranding", "Exception", exReader.ToString().Trim());
            //        objLog.Message = Xml.Serialization.XmlSerializer.Deserialize<string>(objLog.Message);
            //        LogEntries.Add(objLog);
            //    }

            //    throw exx;

            //}
            //catch (System.Exception ex)
            //{   // Added as part of SeatMap- Cabin Branding Service Logging
            //    //if (levelSwitch.TraceInfo)
            //    //{
            //    //    ExceptionWrapper exceptionWrapper = new ExceptionWrapper(ex);
            //    //    LogEntries.Add(United.Logger.LogEntry.GetLogEntry<ExceptionWrapper>(sessionId, "GetPolarisCabinBranding", "Exception", exceptionWrapper));
            //    //}

            //    throw ex;
            //}

            return Cabins;
        }

        private async Task<(MOBSeatMap, int countNoOfFreeSeats, int countNoOfPricedSeats)> GetSeatMapCSL(United.Definition.SeatCSL30.SeatMap seatMapResponse, string sessionId, bool isELF, bool isIBE, bool isOaSeatMapSegment, int segmentIndex, string flow, int appId, string appVersion, bool isEnablePcuDeepLinkInSeatMap, bool isEnablePCUSeatMapPurchaseManageRes, int countNoOfFreeSeats, int countNoOfPricedSeats, string bookingCabin, bool cogStop)
        {
            MOBSeatMap objMOBSeatMap = new MOBSeatMap();
            List<string> cabinBrandingDescriptions = new List<string>();
            United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 objMOBSeatMapCSL30 = new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30();

            objMOBSeatMap.SeatMapAvailabe = true;
            objMOBSeatMap.FlightNumber = objMOBSeatMapCSL30.FlightNumber = seatMapResponse.FlightInfo.MarketingFlightNumber;
            objMOBSeatMap.FlightDateTime = objMOBSeatMapCSL30.FlightDateTime = seatMapResponse.FlightInfo.DepartureDate.ToString("MM/dd/yyyy hh:mm tt");
            objMOBSeatMap.Arrival = new MOBAirport { Code = seatMapResponse.FlightInfo.ArrivalAirport };
            objMOBSeatMap.Departure = new MOBAirport { Code = seatMapResponse.FlightInfo.DepartureAirport };
            objMOBSeatMap.IsOaSeatMap = objMOBSeatMapCSL30.IsOaSeatMap = isOaSeatMapSegment;
            objMOBSeatMap.FleetType = !string.IsNullOrEmpty(seatMapResponse.AircraftInfo.Icr) ? seatMapResponse.AircraftInfo.Icr : string.Empty;
            // SupressLMX only for booking path
            bool supressLMX = SupressLMX(appId);

            // New model to save in persist
            objMOBSeatMapCSL30.ArrivalCode = seatMapResponse.FlightInfo.ArrivalAirport;
            objMOBSeatMapCSL30.DepartureCode = seatMapResponse.FlightInfo.DepartureAirport;
            objMOBSeatMapCSL30.MarketingCarrierCode = seatMapResponse.FlightInfo.MarketingCarrierCode;
            objMOBSeatMapCSL30.OperatingCarrierCode = seatMapResponse.FlightInfo.OperatingCarrierCode;
            objMOBSeatMapCSL30.Flow = flow;
            objMOBSeatMapCSL30.SegmentNumber = segmentIndex;

            List<Mobile.Model.ShopSeats.MOBSeatCSL30> listMOBSeatCSL30 = new List<Mobile.Model.ShopSeats.MOBSeatCSL30>();

            objMOBSeatMap.LegId = string.Empty;
            int cabinCount = 0;

            /// Only in ManageRes -- code for PCU
            /// This code will not execute for booking as isEnablePcuDeepLinkInSeatMap returned as false in booking path
            List<PcuUpgradeOption> upgradeOffers = null;
            if (isEnablePcuDeepLinkInSeatMap)
            {
                var pcu = await new Helper.SeatEngine.PremiumCabinUpgrade(_sessionHelperService, sessionId, objMOBSeatMap.FlightNumber.ToString(), seatMapResponse.FlightInfo.DepartureAirport, seatMapResponse.FlightInfo.ArrivalAirport).LoadOfferStateforSeatMap();
                upgradeOffers = pcu.GetUpgradeOptionsForSeatMap();
                objMOBSeatMap.Captions = await GetPcuCaptions(pcu.GetTravelerNames(), pcu.RecordLocator);
            }

            int numberOfCabins = seatMapResponse.Cabins.Count;
            foreach (United.Definition.SeatCSL30.Cabin cabin in seatMapResponse.Cabins)
            {
                ++cabinCount;
                bool firstCabin = (cabinCount == 1);
                Mobile.Model.Shopping.MOBCabin tmpCabin = new Mobile.Model.Shopping.MOBCabin();

                bool disableSeats = true;
                if (cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)
                    || cabin.CabinBrand.Equals("Business", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("United Premium Plus", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase)
                    || cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Economy", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("Coach", StringComparison.OrdinalIgnoreCase) || cabin.CabinBrand.Equals("First", StringComparison.OrdinalIgnoreCase))
                {
                    if (cabin.CabinBrand.Equals(bookingCabin, StringComparison.OrdinalIgnoreCase))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("Economy", StringComparison.OrdinalIgnoreCase))
                    && (bookingCabin.Equals("Coach", StringComparison.OrdinalIgnoreCase)
                    || bookingCabin.Equals("Economy", StringComparison.OrdinalIgnoreCase)
                    || bookingCabin.Equals("United Economy", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("Business", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase))
                        && (bookingCabin.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("Business", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                    else if ((cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase)
                        || cabin.CabinBrand.Equals("First", StringComparison.OrdinalIgnoreCase))
                        && (bookingCabin.Equals("United First", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("First", StringComparison.OrdinalIgnoreCase)
                        || bookingCabin.Equals("United Global First", StringComparison.OrdinalIgnoreCase)))
                    {
                        disableSeats = false;
                    }
                }

                /// Only in MR Path -- Code for PCU
                /// For MR isEnablePCUSeatMapPurchaseManageRes will be true
                double pcuOfferPriceForthisCabin = 0;
                string pcuOfferAmountForthisCabin = string.Empty;
                string cabinName = string.Empty;
                string pcuOfferOptionId = string.Empty;
                var upgradeOffer = isEnablePcuDeepLinkInSeatMap && upgradeOffers != null && upgradeOffers.Any() && !cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) ? upgradeOffers.FirstOrDefault(u => IsCabinMatchedCSL(u.UpgradeOptionDescription, cabin.CabinBrand)) : null;
                if (!upgradeOffer.IsNullOrEmpty())
                {
                    pcuOfferAmountForthisCabin = string.Format("{0}.00", upgradeOffer.FormattedPrice);
                    pcuOfferPriceForthisCabin = isEnablePCUSeatMapPurchaseManageRes ? upgradeOffer.Price : 0;
                    cabinName = upgradeOffer.UpgradeOptionDescription;
                    pcuOfferOptionId = _configuration.GetValue<bool>("TurnOff_DefaultSelectionForUpgradeOptions") ? string.Empty : upgradeOffer.OptionId;
                    if (isEnablePCUSeatMapPurchaseManageRes)
                    {
                        tmpCabin.PcuOptionId = upgradeOffer.OptionId;
                        tmpCabin.HasEnoughPcuSeats = !upgradeOffer.OptionId.IsNullOrEmpty() && cabin.AvailableSeats >= seatMapResponse.Travelers.Count;
                    }               
                }
                ///  End
                tmpCabin.COS = isOaSeatMapSegment && cabin.IsUpperDeck ? "Upper Deck " + cabin.CabinBrand : cabin.CabinBrand;
                tmpCabin.Configuration = cabin.Layout;
                /// Checking with azhar as this is for other airlines and checking with cabin name.
                var isOaPremiumEconomyCabin = cabin.CabinBrand.Equals("Premium Economy", StringComparison.OrdinalIgnoreCase);
                cabinBrandingDescriptions.Add(cabin.CabinBrand);

                foreach (United.Definition.SeatCSL30.Row row in cabin.Rows)
                {
                    if (!row.IsNullOrEmpty() && row.Number < 1000)
                    {
                        Mobile.Model.Shopping.MOBRow tmpRow = new Mobile.Model.Shopping.MOBRow();
                        tmpRow.Number = row.Number.ToString();
                        tmpRow.Wing = !isOaSeatMapSegment && row.Wing;

                        var monumentrow = cabin.MonumentRows.FirstOrDefault(x => x.VerticalGridNumber == row.VerticalGridNumber);
                        var cabinColumnCount = cabin.ColumnCount == 0 ? cabin.Layout.Length : cabin.ColumnCount;
                        for (int i = 1; i <= cabinColumnCount; i++)
                        {
                            MOBSeatB tmpSeat = null;
                            MOBSeatCSL30 objMOBSeatCSL30 = new MOBSeatCSL30();
                            var seat = row.Seats.FirstOrDefault(x => x.HorizontalGridNumber == i);
                            var monumentseat = (!monumentrow.IsNullOrEmpty()) ? monumentrow.Monuments.FirstOrDefault(x => x.HorizontalGridNumber == i) : null;
                            if (!seat.IsNullOrEmpty())
                            {
                                // Build seatmap response for client
                                tmpSeat = new MOBSeatB();
                                tmpSeat.Exit = seat.IsExit;
                                tmpSeat.Fee = string.Empty; // Need to find and assign
                                tmpSeat.Number = seat.Number;
                                //tmpSeat.Location = seat.Location;
                                if (_configuration.GetValue<bool>("EnableLimitedReclineAllProducts"))
                                {
                                    tmpSeat.LimitedRecline = !isOaSeatMapSegment && IsLimitedRecline(seat);
                                }
                                else
                                {
                                    tmpSeat.LimitedRecline = !isOaSeatMapSegment && !string.IsNullOrEmpty(seat.DisplaySeatCategory)
                                    && isLimitedRecline(seat.DisplaySeatCategory);
                                }

                                // Need to revisit this code// checking only for united economy might includ UPP
                                if (!string.IsNullOrEmpty(seat.SeatType) && !isOaSeatMapSegment
                                    && (cabin.CabinBrand.Equals("United Business", StringComparison.OrdinalIgnoreCase)
                                    || cabin.CabinBrand.Equals("United First", StringComparison.OrdinalIgnoreCase)
                                    || cabin.CabinBrand.Equals("United Polaris Business", StringComparison.OrdinalIgnoreCase)))
                                {
                                    tmpSeat.Program = GetSeatPositionAccessFromCSL30SeatMap(seat.SeatType);
                                }
                                tmpSeat.IsEPlus = !string.IsNullOrEmpty(seat.SeatType)
                                                                && seat.SeatType.Equals(SeatType.BLUE.ToString(), StringComparison.OrdinalIgnoreCase);
                                bool isBasicEconomy = isELF || isIBE;
                                bool disableEplusSeats = false; bool isEconomyCabinWithAdvanceSeats = false;
                                if (_configuration.GetValue<bool>("EnableEPlusSeatsForBasicEconomy"))
                                {
                                    isEconomyCabinWithAdvanceSeats = cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) && !tmpSeat.IsEPlus && isBasicEconomy;
                                }
                                else
                                {
                                    disableEplusSeats = tmpSeat.IsEPlus && isBasicEconomy;
                                    isEconomyCabinWithAdvanceSeats = cabin.CabinBrand.Equals("United Economy", StringComparison.OrdinalIgnoreCase) && disableEplusSeats;
                                }

                                tmpSeat.seatvalue = GetSeatValueFromCSL30SeatMap(seat, disableEplusSeats, disableSeats, null, isOaSeatMapSegment, isOaPremiumEconomyCabin, pcuOfferAmountForthisCabin, cogStop);

                                var tier = seatMapResponse.Tiers.FirstOrDefault(x => !x.IsNullOrEmpty() && x.Id == Convert.ToInt32(seat.Tier));
                                var isSeatMFOPEnabled = await _featureSettings.GetFeatureSettingValue("EnableMilesFOP").ConfigureAwait(false);
                                tmpSeat.ServicesAndFees = GetServicesAndFees(seat, pcuOfferAmountForthisCabin, pcuOfferPriceForthisCabin, tmpSeat.Program, tier, isSeatMFOPEnabled);

                                bool isAdvanceSearchCouponApplied = EnableAdvanceSearchCouponBooking(appId, appVersion);
                                bool isCouponApplied = isAdvanceSearchCouponApplied ? tier?.Pricing != null && tier.Pricing.Any(x => x != null && !string.IsNullOrEmpty(x.CouponCode)) : false;

                                bool disablePCUOfferPriceForBundles = _shoppingUtility.IsEnableTravelOptionsInViewRes(appId, appVersion) && flow == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString();
                                if (!disablePCUOfferPriceForBundles)
                                {
                                    tmpSeat.PcuOfferPrice = tmpSeat.seatvalue == "O" ? pcuOfferAmountForthisCabin : null;
                                    tmpSeat.IsPcuOfferEligible = tmpSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin == 0;
                                    tmpSeat.PcuOfferOptionId = tmpSeat.seatvalue == "O" && !string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) ? pcuOfferOptionId : null;
                                }
                                tmpSeat.DisplaySeatFeature = GetDisplaySeatFeature(isOaSeatMapSegment, tmpSeat.seatvalue, pcuOfferAmountForthisCabin, cabinName, isEconomyCabinWithAdvanceSeats, cabin.CabinBrand, tmpSeat.IsEPlus);
                                tmpSeat.IsNoUnderSeatStorage = (_shoppingUtility.IsEnableBulkheadNoUnderSeatStorage(appId, appVersion) && seat.HasNoUnderSeatStorage);
                                tmpSeat.SeatFeatureList = GetSeatFeatureList(tmpSeat.seatvalue, supressLMX, tmpSeat.LimitedRecline, isEconomyCabinWithAdvanceSeats, cabin.CabinBrand, isCouponApplied, tmpSeat.Exit, hasNoUnderSeatStorageAndBulkHead: tmpSeat.IsNoUnderSeatStorage);
                                CountNoOfFreeSeatsAndPricedSeats(tmpSeat, ref countNoOfFreeSeats, ref countNoOfPricedSeats);

                                // Seatmap response with traveler pricing to save in persisit
                                // This is backend model, Client is not using it.
                                // This code needs to be modified when client is changing.
                                objMOBSeatCSL30.Number = seat.Number;
                                objMOBSeatCSL30.Tier = seat.Tier;
                                objMOBSeatCSL30.TotalFee = tmpSeat.ServicesAndFees?.Count != 0 ? tmpSeat.ServicesAndFees[0].TotalFee : 0;
                                objMOBSeatCSL30.EDoc = seat.EDoc;
                                objMOBSeatCSL30.SeatType = seat.SeatType.ToUpper();
                                objMOBSeatCSL30.DisplaySeatCategory = seat.DisplaySeatCategory;
                                objMOBSeatCSL30.IsAvailable = seat.IsAvailable;
                                objMOBSeatCSL30.Pricing = new List<United.Mobile.Model.ShopSeats.TierPricingCSL30>();
                                if (!tier.IsNullOrEmpty() && tier.Pricing != null)
                                {
                                    foreach (var pricing in tier.Pricing)
                                    {
                                        var travelerPricing = new United.Mobile.Model.ShopSeats.TierPricingCSL30()
                                        {
                                            TravelerId = pricing.TravelerId,
                                            TotalPrice = pricing.TotalPrice,
                                            TravelerIndex = seatMapResponse.Travelers.FirstOrDefault(x => !x.IsNullOrEmpty() && x.Id == pricing.TravelerId).TravelerIndex,
                                            CouponCode = isAdvanceSearchCouponApplied ? pricing.CouponCode : string.Empty,
                                            OriginalPrice = isAdvanceSearchCouponApplied ? pricing.OriginalPrice : string.Empty
                                        };
                                        objMOBSeatCSL30.Pricing.Add(travelerPricing);
                                    }
                                }
                                listMOBSeatCSL30.Add(objMOBSeatCSL30);
                            }
                            else
                            {
                                //get monumemt seat and loop based on span - build this empty seat.
                                //monumentseat.HorizontalSpan
                                tmpSeat = new MOBSeatB
                                {
                                    Number = string.Empty,
                                    Fee = string.Empty,
                                    LimitedRecline = false,
                                    seatvalue = "-",
                                    Exit = (!monumentseat.IsNullOrEmpty()) ? monumentseat.IsExit : false,
                                };
                            }
                            tmpRow.Seats.Add(tmpSeat);
                        }

                        if (tmpRow != null)
                        {
                            if (tmpRow.Seats == null || tmpRow.Seats.Count != cabin.Layout.Length)
                            {
                                if (isOaSeatMapSegment)
                                    throw new MOBUnitedException(_configuration.GetValue<string>("SeatMapUnavailableOtherAirlines"));
                                throw new MOBUnitedException(_configuration.GetValue<string>("GenericExceptionMessage"));
                            }
                        }

                        if (row.Number < 1000)
                            tmpCabin.Rows.Add(tmpRow);
                    }
                }
                tmpCabin.Configuration = tmpCabin.Configuration.Replace(" ", "-");
                objMOBSeatMap.Cabins.Add(tmpCabin);
            }

            objMOBSeatMap.SeatLegendId = objMOBSeatMap.IsOaSeatMap ? _configuration.GetValue<string>("SeatMapLegendForOtherAirlines") :
                                                                     await GetPolarisSeatMapLegendId(seatMapResponse.FlightInfo.DepartureAirport, seatMapResponse.FlightInfo.ArrivalAirport, numberOfCabins, cabinBrandingDescriptions, appId, appVersion);

            objMOBSeatMapCSL30.Seat = listMOBSeatCSL30 != null ? listMOBSeatCSL30 : null;
            await SaveCSL30SeatMapPersist(sessionId, objMOBSeatMapCSL30);

            return (objMOBSeatMap, countNoOfFreeSeats, countNoOfPricedSeats);
        }
        private async Task SaveCSL30SeatMapPersist(string sessionId, United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 seatMap)
        {
            List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30> csl30SeatMaps = new List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>();
            csl30SeatMaps.Add(seatMap);
            var persistedCSL30SeatMaps = new List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>();
            persistedCSL30SeatMaps = await _sessionHelperService.GetSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(sessionId, new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName, new List<string> { sessionId, new United.Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }).ConfigureAwait(false); //change session

            // to Append current CSL30 seatmap & maintain all segments seatmaps without duplicity
            if (!persistedCSL30SeatMaps.IsNullOrEmpty())
            {
                foreach (United.Mobile.Model.ShopSeats.MOBSeatMapCSL30 pCSL30SeatMap in persistedCSL30SeatMaps)
                {
                    if (!(pCSL30SeatMap.DepartureCode.Equals(seatMap.DepartureCode, StringComparison.OrdinalIgnoreCase) && pCSL30SeatMap.ArrivalCode.Equals(seatMap.ArrivalCode, StringComparison.OrdinalIgnoreCase) && pCSL30SeatMap.FlightNumber == seatMap.FlightNumber && pCSL30SeatMap.FlightDateTime == seatMap.FlightDateTime))
                    {
                        csl30SeatMaps.Add(pCSL30SeatMap);
                    }
                }
                await _sessionHelperService.SaveSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(csl30SeatMaps, sessionId, new List<string> { sessionId, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName,5400,false).ConfigureAwait(false);//(new United.Mobile.Model.ShopSeats.SeatMapCSL30()).GetType().FullName,
            }
            else
            {
                await _sessionHelperService.SaveSession<List<United.Mobile.Model.ShopSeats.MOBSeatMapCSL30>>(csl30SeatMaps, sessionId, new List<string> { sessionId, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName }, new Mobile.Model.ShopSeats.MOBSeatMapCSL30().ObjectName,5400,false).ConfigureAwait(false);
            }
        }
        private bool IsLimitedRecline(Seat seat)
        {
            return new string[] { "LIMITED", "NO" }.Contains(seat?.ReclineType?.ToUpper());
        }
        private bool isLimitedRecline(string displaySeatCategory)
        {
            if (!string.IsNullOrEmpty(displaySeatCategory))
            {
                var limitedReclineCategory = _configuration.GetValue<string>("SelectSeatsLimitedReclineForCSL30").Split('|');
                if (!limitedReclineCategory.IsNullOrEmpty() && limitedReclineCategory.Any())
                {
                    return limitedReclineCategory.Any(x => !x.IsNullOrEmpty() && x.Trim().Equals(displaySeatCategory, StringComparison.OrdinalIgnoreCase));
                }
            }
            return false;
        }
        public string GetSeatValueFromCSL30SeatMap(Definition.SeatCSL30.Seat seat, bool disableEplus, bool disableSeats, MOBApplication application, bool isOaSeatMapSegment, bool isOaPremiumEconomyCabin, string pcuOfferAmountForthisCabin, bool cogStop)
        {
            string seatValue = string.Empty;
            if (seat != null && !string.IsNullOrEmpty(seat.SeatType))
            {
                if (seat.IsInoperative || seat.IsPermanentBlocked || seat.IsBlocked)
                {
                    seatValue = "X";
                }
                else if (seat.SeatType.Equals(SeatType.BLUE.ToString(), StringComparison.OrdinalIgnoreCase) || isOaPremiumEconomyCabin)
                {
                    seatValue = seat.IsAvailable && (disableEplus || cogStop) ? "X" : seat.IsAvailable ? "P" : "X";
                }
                else if (seat.SeatType.Equals(SeatType.PREF.ToString(), StringComparison.OrdinalIgnoreCase))//TODO version check
                {
                    seatValue = seat.IsAvailable ? "PZ" : "X";
                }
                else if (seat.SeatType.Equals(SeatType.STANDARD.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    seatValue = seat.IsAvailable ? "O" : "X";
                }
                else
                {
                    seatValue = seat.IsAvailable ? "O" : "X";
                }
            }
            return string.IsNullOrEmpty(seatValue) || (!string.IsNullOrEmpty(seatValue) && disableSeats && string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin)) ? "X" : seatValue;
        }

        

        private List<ServicesAndFees> GetServicesAndFees(Definition.SeatCSL30.Seat seat, string pcuOfferAmountForthisCabin, double pcuOfferPriceForthisCabin, string program, Tier tier, bool isSeatMFOPEnabled)
        {
            List<ServicesAndFees> servicesAndFees = new List<ServicesAndFees>();

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && pcuOfferPriceForthisCabin > 0) // Amount will be there only if the no of travelers are eligible and Appversion and AppId matches
            {
                ServicesAndFees serviceAndFee = new ServicesAndFees();
                serviceAndFee.Available = seat.IsAvailable;
                serviceAndFee.SeatFeature = seat.DisplaySeatCategory;
                serviceAndFee.SeatNumber = seat.Number;
                serviceAndFee.Program = program; // aSeat.Program will be empty and to assign program code for higher cabins to upgrade seat
                serviceAndFee.TotalFee = Convert.ToDecimal(pcuOfferPriceForthisCabin);
                serviceAndFee.Currency = "USD";
                if (isSeatMFOPEnabled)
                {
                    serviceAndFee.DisplayFeesWithMiles = serviceAndFee.TotalFee != 0 ?
                                                         string.Concat("$", serviceAndFee.TotalFee.ToString())
                                                         : string.Empty;
                }
                servicesAndFees.Add(serviceAndFee);
            }
            else if (seat.ServicesAndFees != null && seat.ServicesAndFees.Any())
            {
                foreach (SeatService seatService in seat.ServicesAndFees)
                {
                    ServicesAndFees serviceAndFee = new ServicesAndFees();
                    serviceAndFee.AgentDutyCode = seatService.AgentDutyCode ?? string.Empty;
                    serviceAndFee.AgentId = seatService.AgentId ?? string.Empty;
                    serviceAndFee.AgentTripleA = seatService.AgentTripleA ?? string.Empty;
                    serviceAndFee.Available = seat.IsAvailable;
                    serviceAndFee.BaseFee = seatService.BaseFee;
                    serviceAndFee.Currency = seatService.Currency;
                    serviceAndFee.EliteStatus = seatService.EliteStatus.ToString();
                    serviceAndFee.FeeWaiveType = seatService.FeeWaiveType ?? string.Empty;
                    serviceAndFee.IsDefault = seatService.IsDefault;
                    serviceAndFee.OverrideReason = seatService.OverrideReason ?? string.Empty;
                    serviceAndFee.Program = seat.EDoc;
                    serviceAndFee.SeatFeature = seat.DisplaySeatCategory.ToUpper();
                    serviceAndFee.SeatNumber = seat.Number;
                    serviceAndFee.Tax = seatService.Tax;
                    serviceAndFee.TotalFee = seatService.TotalFee;
                    serviceAndFee.OriginalPrice = serviceAndFee.OriginalPrice;
                    serviceAndFee.CouponCode = serviceAndFee.CouponCode;
                    servicesAndFees.Add(serviceAndFee);
                }
            }
            if (isSeatMFOPEnabled)
            {
                if (servicesAndFees != null && servicesAndFees.Count > 0)
                {
                    string displayFeesWithMiles = servicesAndFees[0].TotalFee != 0 ?
                                                                string.Concat("$", servicesAndFees[0].TotalFee.ToString())
                                                                : string.Empty;
                    servicesAndFees[0].DisplayFeesWithMiles = displayFeesWithMiles;
                }
            }
            return servicesAndFees;
        }
        public bool EnableAdvanceSearchCouponBooking(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableAdvanceSearchCouponBooking") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("AndroidAdvanceSearchCouponBookingVersion"), _configuration.GetValue<string>("iPhoneAdvanceSearchCouponBookingVersion"));
        }
        public string GetSeatPositionAccessFromCSL30SeatMap(string seatType)
        {
            string seatPositionProgram = string.Empty;
            if (seatType.Equals(SeatType.FBLEFT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBL);
            }
            else if (seatType.Equals(SeatType.FBRIGHT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBR);
            }
            else if (seatType.Equals(SeatType.FBFRONT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBF);
            }
            else if (seatType.Equals(SeatType.FBBACK.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.FBB);
            }
            else if (seatType.Equals(SeatType.DAAFRONTL.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFL);
            }
            else if (seatType.Equals(SeatType.DAAFRONTR.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFR);
            }
            else if (seatType.Equals(SeatType.DAALEFT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAL);
            }
            else if (seatType.Equals(SeatType.DAARIGHT.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAR);
            }
            else if (seatType.Equals(SeatType.DAAFRONTRM.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                seatPositionProgram = Convert.ToString(SeatPosition.DAFRM);
            }
            return seatPositionProgram;
        }
        private string GetDisplaySeatFeature(bool isOaSeatMapSegment, string seatValue, string pcuOfferAmountForthisCabin, string pcuCabinName, bool isEconomyCabinWithAdvanceSeats, string cabinName, bool isEplus)
        {
            if (isOaSeatMapSegment)
                return string.Empty;

            if (!string.IsNullOrWhiteSpace(pcuOfferAmountForthisCabin) && seatValue.Equals("O") && !string.IsNullOrEmpty(pcuCabinName))
                return pcuCabinName;

            if (cabinName.Equals("United Premium Plus", StringComparison.OrdinalIgnoreCase))
                return "United Premium Plus";

            if (seatValue.Equals("P") || isEplus)
                return "Economy Plus";

            if (seatValue.Equals("PZ"))
                return "Preferred Seat";

            if (isEconomyCabinWithAdvanceSeats)
                return "Economy";

            return string.Empty;
        }
        private List<string> GetSeatFeatureList(string seatValue, bool supressLMX, bool limitedRecline, bool isEconomyCabinWithAdvanceSeats, string cabinName, bool isCouponApplied, bool isExit, bool hasNoUnderSeatStorageAndBulkHead = false)
        {
            List<string> seatFeatures = new List<string>();
            bool enableLimitReclineAllProducts = _configuration.GetValue<bool>("EnableLimitedReclineAllProducts");

            if (seatValue.Equals("P"))
            {
                seatFeatures.Add("Extra legroom");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add("Limited recline");
                }
                if (!supressLMX)
                    seatFeatures.Add("Eligible for PQD");
            }
            else if (seatValue.Equals("PZ"))
            {
                seatFeatures.Add("Standard legroom");
                seatFeatures.Add("Favorable location in Economy");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add("Limited recline");
                }
            }
            else if (isEconomyCabinWithAdvanceSeats)
            {
                seatFeatures.Add("Advance seat assignment");
                if (!enableLimitReclineAllProducts)
                {
                    if (limitedRecline)
                        seatFeatures.Add("Limited recline");
                }
            }
            if (enableLimitReclineAllProducts)
            {
                if (limitedRecline)
                {
                    if (isExit)
                        seatFeatures.Add(_configuration.GetValue<string>("ExitNoOrLimitedReclineMessage"));
                    else
                        seatFeatures.Add(_configuration.GetValue<string>("NoOrLimitedReclineMessage"));
                }
            }
            if (hasNoUnderSeatStorageAndBulkHead)
            {
                seatFeatures.Add(_configuration.GetValue<string>("BulkSeatNoUnderSeatStorageText"));
            }
            if (isCouponApplied)
            {
                seatFeatures.Add("Discounted price");
            }
            return seatFeatures;
        }
        public void GetOANoSeatAvailableMessage(List<TripSegment> segments)
        {
            if (segments != null && segments.Count > 0)
            {
                foreach (var segment in segments)
                {
                    segment.ShowInterlineAdvisoryMessage = true;
                    segment.InterlineAdvisoryDeepLinkURL = string.Empty;
                    segment.InterlineAdvisoryMessage = _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody");
                    string depTimeFormatted = Convert.ToDateTime(segment.ScheduledDepartureDate).ToString("ddd, MMM dd");
                    segment.InterlineAdvisoryTitle = $"{depTimeFormatted} {segment.Departure.Code} - {segment.Arrival.Code}";
                    segment.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("OANoSeatMapAvailableMessageTitle");
                }
            }
        }
        public void GetInterlineRedirectLink(List<TripSegment> segments, string pointOfSale, MOBRequest mobRequest, string recordLocator, string lastname, List<MOBItem> catalog)
        {
            foreach (var segment in segments)
            {
                if (ConfigUtility.IsEligibleCarrierAndAPPVersion(segment.OperatingCarrier, mobRequest.Application.Id, mobRequest?.Application?.Version?.Major, catalog))
                {
                    string carrierAdvisoryMessage = string.Empty;
                    string deepLinkURL = ConfigUtility.CreateDeepLinkURLForOtherAirlinesManageRes(recordLocator, lastname, pointOfSale, mobRequest.LanguageCode, segment.OperatingCarrier, out carrierAdvisoryMessage);

                    segment.ShowInterlineAdvisoryMessage = !string.IsNullOrEmpty(deepLinkURL) ? true : false;

                    segment.InterlineAdvisoryMessage = carrierAdvisoryMessage;
                    segment.InterlineAdvisoryDeepLinkURL = deepLinkURL;
                    string depTimeFormatted = Convert.ToDateTime(segment.ScheduledDepartureDate).ToString("ddd, MMM dd");
                    segment.InterlineAdvisoryTitle = $"{depTimeFormatted} {segment.Departure.Code} - {segment.Arrival.Code}";
                    segment.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("InterlineDeepLinkRedesignMessageTitle");
                }
            }
        }
        public async Task<bool> IsEnablePCUSeatMapPurchaseManageRes(int appId, string appVersion, int numberOfTravelers, List<MOBItem> catalogs)
        {
            if (ConfigUtility.CheckClientCatalogForEnablingFeature("PCUSeatsForMultiPaxClientCatalog", catalogs)) return true;
            try
            {
                if (await new CatalogHelper(_configuration, _sessionHelperService, _dynamoDBService, _headers).GetBooleanValueFromCatalogCache("PCUOnSeatMapUpgradeAndAssignSeatCatalogSwitch", appId))
                {
                    if (!string.IsNullOrEmpty(appVersion) && appId != -1 && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidEnablePCUSelectedSeatPurchaseViewResVersion", "iPhoneEnablePCUSelectedSeatPurchaseViewResVersion", "", "", true, _configuration))
                    {
                        string noOfPCUTravelers = _configuration.GetValue<string>("EnablePCUSelectedSeatPurchaseViewRes");
                        return noOfPCUTravelers.IsNullOrEmpty() ? false : numberOfTravelers <= Convert.ToInt32(noOfPCUTravelers);
                    }
                }
            }
            catch (Exception ex)
            {
                string enterlog = string.IsNullOrEmpty(ex.StackTrace) ? ex.Message : ex.StackTrace;
                _logger.LogError("IsEnablePCUSeatMapPurchaseManageRes {@Exception}", enterlog);
            }
            return false;
        }
        public async Task<string> ShowNoFreeSeatsAvailableMessage(int noOfTravelersWithoutSeat, int noOfFreeEplusEligibleRemaining, int noOfFreeSeats,
          int noOfPricedSeats, bool isBasicEconomy)
        {
            if (!_configuration.GetValue<bool>("EnableSSA")) return string.Empty;

            return _shoppingUtility.EnableIBEFull() && isBasicEconomy
                    ? await SeatmapMessageForBasicEconomy(noOfTravelersWithoutSeat, noOfPricedSeats)
                    : await SeatmapMessageForSeatAvailability(noOfTravelersWithoutSeat, noOfFreeEplusEligibleRemaining, noOfFreeSeats, noOfPricedSeats);
        }
        private async Task<string> SeatmapMessageForBasicEconomy(int noOfTravelersWithoutSeat, int noOfPricedSeats)
        {
            if (HaveEnoughSeatsToChange(noOfTravelersWithoutSeat, noOfPricedSeats))
            {
                return await GetDocumentTextFromDataBase("MR_ASA_SEATS_AVAILABLE_FOR_PURCHASE");
            }

            return noOfTravelersWithoutSeat == 0 ? string.Empty : await GetDocumentTextFromDataBase("MR_ASA_SEATS_NOT_AVAILABLE");
        }

        private async Task<string> SeatmapMessageForSeatAvailability(int noOfTravelersWithoutSeat, int noOfFreeEplusEligibleRemaining, int noOfFreeSeats, int noOfPricedSeats)
        {
            return EnoughFreeSeats(noOfTravelersWithoutSeat, noOfFreeEplusEligibleRemaining, noOfFreeSeats, noOfPricedSeats)
                    ? string.Empty
                    : await GetDocumentTextFromDataBase("SSA_NO_FREE_SEATS_MESSAGE");
        }
        private bool HaveEnoughSeatsToChange(int noOfTravelersWithoutSeat, int noOfPricedSeats)
        {
            return noOfTravelersWithoutSeat == 0 && noOfPricedSeats > 0 || noOfTravelersWithoutSeat != 0 && noOfPricedSeats - noOfTravelersWithoutSeat >= 0;
        }
        private bool EnoughFreeSeats(int travelerCount, int noOfFreeEplusEligible, int countNoOfFreeSeats, int noOfPricedSeats)
        {
            if (countNoOfFreeSeats >= travelerCount)
                return true;

            var noOfTravelersAfterPickingFreeSeats = travelerCount - countNoOfFreeSeats;
            if ((noOfPricedSeats >= noOfTravelersAfterPickingFreeSeats) && (noOfFreeEplusEligible >= noOfTravelersAfterPickingFreeSeats))
                return true;

            return false;
        }
        public async Task<string> GetPolarisSeatMapLegendId(string from, string to, int numberOfCabins, List<string> polarisCabinBrandingDescriptions, int applicationId = -1, string appVersion = "")
        {
            string seatMapLegendId = string.Empty;

            //POLARIS Cabin Branding SeatMapLegend Booking Path
            string seatMapLegendEntry1 = (_configuration.GetValue<string>("seatMapLegendEntry1") != null) ? _configuration.GetValue<string>("seatMapLegendEntry1") : "";
            string seatMapLegendEntry2 = (_configuration.GetValue<string>("seatMapLegendEntry2") != null) ? _configuration.GetValue<string>("seatMapLegendEntry2") : "";
            string seatMapLegendEntry3 = (_configuration.GetValue<string>("seatMapLegendEntry3") != null) ? _configuration.GetValue<string>("seatMapLegendEntry3") : "";
            string seatMapLegendEntry4 = (_configuration.GetValue<string>("seatMapLegendEntry4") != null) ? _configuration.GetValue<string>("seatMapLegendEntry4") : "";

            string seatMapLegendEntry5 = (_configuration.GetValue<string>("seatMapLegendEntry5") != null) ? _configuration.GetValue<string>("seatMapLegendEntry5") : "";
            string seatMapLegendEntry6 = (_configuration.GetValue<string>("seatMapLegendEntry6") != null) ? _configuration.GetValue<string>("seatMapLegendEntry6") : "";
            string seatMapLegendEntry7 = (_configuration.GetValue<string>("seatMapLegendEntry7") != null) ? _configuration.GetValue<string>("seatMapLegendEntry7") : "";
            string seatMapLegendEntry8 = (_configuration.GetValue<string>("seatMapLegendEntry8") != null) ? _configuration.GetValue<string>("seatMapLegendEntry8") : "";
            string seatMapLegendEntry9 = (_configuration.GetValue<string>("seatMapLegendEntry9") != null) ? _configuration.GetValue<string>("seatMapLegendEntry9") : "";
            string seatMapLegendEntry14 = string.Empty;
            string legendForPZA = string.Empty;
            string legendForUPP = string.Empty;
            // Preferred Seat //AB-223
            bool isPreferredZoneEnabled = ConfigUtility.EnablePreferredZone(applicationId, appVersion); // Check if preferred seat
            if (isPreferredZoneEnabled)
            {
                seatMapLegendEntry14 = _configuration.GetValue<string>("seatMapLegendEntry14");
                legendForPZA = "_PZA";
            }
            if (ConfigUtility.IsUPPSeatMapSupportedVersion(applicationId, appVersion) && numberOfCabins == 3 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Any(p => p.ToUpper() == "UNITED PREMIUM PLUS"))
            {
                legendForUPP = "_UPP";
                seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris Business|United Premium Plus|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
            }
            else
            {
                if (_configuration.GetValue<bool>("IsEnableUPPForTwoCabinFlight")
                    && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, "AndroidUPPForTwoCabinFlightVersion", "iOSUPPForTwoCabinFlightVersion") && numberOfCabins == 2 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Any(p => p.ToUpper() == "UNITED PREMIUM PLUS"))
                {
                    legendForUPP = "_UPP";
                }
                if (_configuration.GetValue<bool>("DisableComplimentaryUpgradeOnpremSqlService"))
                {

                    if (numberOfCabins == 1)
                    {
                        seatMapLegendId = "seatmap_legend6" + legendForPZA + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                    else if (numberOfCabins == 3)
                    {
                        seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                    else//If number of cabin==2 or by default assiging legend5
                    {
                        seatMapLegendId = "seatmap_legend5" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                    }
                }
                else
                {
                    var listOfCabinIds = await _complimentaryUpgradeService.GetComplimentaryUpgradeOfferedFlagByCabinCount(from, to, numberOfCabins, _headers.ContextValues.SessionId, _headers.ContextValues.TransactionId).ConfigureAwait(false);

                    if (listOfCabinIds != null)
                    {
                        foreach (var Ids in listOfCabinIds)
                        {
                            //verification needed
                            int secondCabinBrandingId = Ids.SecondCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(Ids.SecondCabinBrandingId);
                            int thirdCabinBrandingId = Ids.ThirdCabinBrandingId.Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(Ids.ThirdCabinBrandingId);

                            //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                            //Added the code to check the flag for Preferred Zone and app version > 2.1.60  
                            if (thirdCabinBrandingId == 0)
                            {
                                if (secondCabinBrandingId == 1)
                                {
                                    seatMapLegendId = "seatmap_legend5" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9; // Sample Value Could be Ex: seatmap_legend5|First|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                                }
                                else if (secondCabinBrandingId == 2)
                                {
                                    seatMapLegendId = "seatmap_legend4" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend4|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                                }
                                else if (secondCabinBrandingId == 3)
                                {
                                    seatMapLegendId = "seatmap_legend3" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend3|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend4|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                                }
                            }
                            else if (thirdCabinBrandingId == 1)
                            {
                                seatMapLegendId = "seatmap_legend2" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Economy|Occupied Seat|Exit Row 
                            }
                            else if (thirdCabinBrandingId == 4)
                            {
                                seatMapLegendId = "seatmap_legend1" + legendForPZA + legendForUPP + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                            }
                        }
                    }

                    if (string.IsNullOrEmpty(seatMapLegendId))
                    {
                        #region Adding Preferred Seat Legend ID
                        //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                        //Added the code to check the flag for Preferred Zone and app version > 2.1.60
                        //Changes added on 09/24/2018                
                        //Bug 213002 mAPP: Seat Map- Blank Legend is displayed for One Cabin Flights
                        //Bug 102152
                        if (!string.IsNullOrEmpty(appVersion) &&
                        GeneralHelper.isApplicationVersionGreater(applicationId, appVersion, "AndroidFirstCabinVersion", "iPhoneFirstCabinVersion", "", "", true, _configuration)
                        && numberOfCabins == 1 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0 &&
                        !string.IsNullOrEmpty(polarisCabinBrandingDescriptions[0].ToString().Trim()))
                        {
                            seatMapLegendId = "seatmap_legend6" + legendForPZA + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                        else
                        {
                            seatMapLegendId = "seatmap_legend5" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                        }
                    }
                }
                #region
                //Database database = DatabaseFactory.CreateDatabase("ConnectionString - DB_Flightrequest");
                //DbCommand dbCommand = (DbCommand)database.GetStoredProcCommand("usp_GetComplimentary_Upgrade_Offered_flag_By_Cabin_Count");
                //database.AddInParameter(dbCommand, "@Origin", DbType.String, from);
                //database.AddInParameter(dbCommand, "@destination", DbType.String, to);
                //database.AddInParameter(dbCommand, "@numberOfCabins", DbType.Int32, numberOfCabins);

                //try
                //{
                //    using (IDataReader dataReader = database.ExecuteReader(dbCommand))
                //    {
                //        while (dataReader.Read())
                //        {
                //            #region
                //            int secondCabinBrandingId = dataReader["SecondCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["SecondCabinBrandingId"]);
                //            int thirdCabinBrandingId = dataReader["ThirdCabinBrandingId"].Equals(System.DBNull.Value) ? 0 : Convert.ToInt32(dataReader["ThirdCabinBrandingId"]);

                //            //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                //            //Added the code to check the flag for Preferred Zone and app version > 2.1.60                                           
                //            if (thirdCabinBrandingId == 0)
                //            {
                //                if (secondCabinBrandingId == 1)
                //                {
                //                    seatMapLegendId = "seatmap_legend5" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9; // Sample Value Could be Ex: seatmap_legend5|First|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                //                }
                //                else if (secondCabinBrandingId == 2)
                //                {
                //                    seatMapLegendId = "seatmap_legend4" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend4|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row
                //                }
                //                else if (secondCabinBrandingId == 3)
                //                {
                //                    seatMapLegendId = "seatmap_legend3" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend3|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend4|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                //                }
                //            }
                //            else if (thirdCabinBrandingId == 1)
                //            {
                //                seatMapLegendId = "seatmap_legend2" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Economy|Occupied Seat|Exit Row 
                //            }
                //            else if (thirdCabinBrandingId == 4)
                //            {
                //                seatMapLegendId = "seatmap_legend1" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + "|" + polarisCabinBrandingDescriptions[1].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;// Sample Value Could be Ex: seatmap_legend1|United Polaris First|United Polaris Business|Economy Plus|Preferred Seat|Economy|Occupied Seat|Exit Row or Sample Value Could be Ex: seatmap_legend2|First|Business|Economy Plus|Economy|Occupied Seat|Exit Row
                //            }
                //            #endregion
                //        }
                //    }
                //}
                //catch (System.Exception ex)
                //{
                //    Console.Write(ex.Message);
                //}
                //if (string.IsNullOrEmpty(seatMapLegendId))
                //{
                //    #region Adding Preferred Seat Legend ID
                //    //AB-223,AB-224 Adding Preferred Seat To SeatLegendID
                //    //Added the code to check the flag for Preferred Zone and app version > 2.1.60
                //    //Changes added on 09/24/2018                
                //    //Bug 213002 mAPP: Seat Map- Blank Legend is displayed for One Cabin Flights
                //    //Bug 102152
                //    if (!string.IsNullOrEmpty(appVersion) &&
                //        GeneralHelper.IsApplicationVersionGreater(applicationId, appVersion, "AndroidFirstCabinVersion", "iPhoneFirstCabinVersion", "", "", true)
                //        && numberOfCabins == 1 && polarisCabinBrandingDescriptions != null && polarisCabinBrandingDescriptions.Count > 0 &&
                //        !string.IsNullOrEmpty(polarisCabinBrandingDescriptions[0].ToString().Trim()))
                //    {
                //        seatMapLegendId = "seatmap_legend6" + legendForPZA + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                //    }
                //    else
                //    {
                //        seatMapLegendId = "seatmap_legend5" + legendForPZA + "|" + polarisCabinBrandingDescriptions[0].ToString() + seatMapLegendEntry6 + seatMapLegendEntry14 + seatMapLegendEntry7 + seatMapLegendEntry8 + seatMapLegendEntry9;
                //    }
                //    #endregion
                //}
                #endregion
                #endregion
            }
            return seatMapLegendId;
        }

        */
        #endregion
    }

}

