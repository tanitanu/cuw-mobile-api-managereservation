using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Common.HelperSeatEngine;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.SeatEngine;
using United.Mobile.DataAccess.ShopTrips;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.Shopping;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.Model.SeatMapEngine;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Booking;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Mobile.Model.Shopping.Misc;
using United.Mobile.Model.Shopping.Pcu;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Helper;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using FlowType = United.Utility.Enum.FlowType;
using MOBBKTrip = United.Mobile.Model.Shopping.Booking.MOBBKTrip;
using MOBErrorCodes = United.Utility.Enum.MOBErrorCodes;
using MOBPromoCodeDetails = United.Mobile.Model.Shopping.FormofPayment.MOBPromoCodeDetails;
using RegisterOfferRequest = United.Services.FlightShopping.Common.FlightReservation.RegisterOfferRequest;
using Session = United.Mobile.Model.Common.Session;
using TripSegment = United.Mobile.Model.Shopping.TripSegment;

namespace United.Mobile.ViewResSeatMap.Domain
{
    public class ViewResSeatMapBusiness : IViewResSeatMapBusiness
    {
        private readonly ICacheLog<ViewResSeatMapBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly ISeatEngine _seatEngine;
        private readonly ISeatMapCSL30 _seatMapCSL30;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IManageReservation _manageReservation;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IFeatureSettings _featureSettings;

        public ViewResSeatMapBusiness(ICacheLog<ViewResSeatMapBusiness> logger
            , IConfiguration configuration
            , IHeaders headers
            , ISessionHelperService sessionHelperService
            , IShoppingSessionHelper shoppingSessionHelper
            , ISeatEngine seatEngine
            , ISeatMapCSL30 seatMapCSL30
            , IShoppingUtility shoppingUtility
            , IManageReservation manageReservation
            , IShoppingCartService shoppingCartService
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _sessionHelperService = sessionHelperService;
            _shoppingSessionHelper = shoppingSessionHelper;
            _seatEngine = seatEngine;
            _seatMapCSL30 = seatMapCSL30;
            ConfigUtility.UtilityInitialize(_configuration);
            _shoppingUtility = shoppingUtility;
            _manageReservation = manageReservation;
            _shoppingCartService = shoppingCartService;
            _featureSettings = featureSettings;
        }

        public async Task<MOBSeatChangeSelectResponse> SelectSeats(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination, string catalogValues = null)
        {           
            MOBSeatChangeSelectRequest request = new MOBSeatChangeSelectRequest();
            request.AccessCode = accessCode;
            request.TransactionId = transactionId;
            request.LanguageCode = languageCode;
            request.Application = new MOBApplication();
            request.Application.Id = applicationId;
            request.Application.Version = new MOBVersion();
            request.Application.Version.DisplayText = appVersion;
            request.Application.Version.Major = appVersion;
            request.SessionId = sessionId;
            request.Origin = origin;
            request.Destination = destination;
            request.FlightNumber = flightNumber;
            request.FlightDate = flightDate;
            request.PaxIndex = paxIndex;
            if (seatAssignment == null)
                seatAssignment = string.Empty;
            request.SeatAssignment = seatAssignment;
            request.NextOrigin = nextOrigin;
            request.NextDestination = nextDestination;            
            request.CatalogValues = catalogValues;

            bool isIOSDecodingFixEnabled = await _featureSettings.GetFeatureSettingValue("EnableIOSDecodingFix");

            if (isIOSDecodingFixEnabled) //To handle double encoded parameters
            {
                request.TransactionId = HttpUtility.UrlDecode(transactionId);
                request.FlightDate = HttpUtility.UrlDecode(flightDate);
                request.CatalogValues = HttpUtility.UrlDecode(catalogValues);
                request.SeatAssignment = HttpUtility.UrlDecode(seatAssignment);
            }

            _logger.LogInformation("SeatChangeSelectSeatsRequest {Request} and {SessionId}", JsonConvert.SerializeObject(request), sessionId);


            MOBSeatChangeSelectResponse response = new MOBSeatChangeSelectResponse();
            response.SessionId = request.SessionId;
            response.Request = request;
            bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false;

            Session session = null;

            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
            }
            else
            {
                _logger.LogInformation("SelectSeats : Session Null {SessionId} {ApplicationId} {ApplicationVersion} {DeviceId} and {Request}", request.SessionId, request.Application.Id, request.Application.Version.Major, request.DeviceId, request);
               
                if (ConfigUtility.VersionCheck_NullSession_AfterAppUpgradation(request))
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOP_NullSession_AfterAppUpgradation).ToString(), _configuration.GetValue<string>("CFOPViewRes_ReloadAppWhileUpdateMessage").ToString());
                else
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOPSessionExpire).ToString(), _configuration.GetValue<string>("CFOPViewRes_ReloadAppWhileUpdateMessage").ToString());
            }

            if (GeneralHelper.ValidateAccessCode(request.AccessCode))
            {
                SeatChangeState state = await _sessionHelperService.GetSession<SeatChangeState>(request.SessionId, new SeatChangeState().ObjectName, new List<string> { request.SessionId, new SeatChangeState().ObjectName }).ConfigureAwait(false);

                if (state == null)
                {
                    throw new MOBUnitedException("Unable to retrieve information needed for seat change.");
                }

                var InitializeSeatResponse = await _sessionHelperService.GetSession<MOBSeatChangeInitializeResponse>(request.SessionId, typeof(MOBSeatChangeInitializeResponse).Name, new List<string> { request.SessionId, typeof(MOBSeatChangeInitializeResponse).Name }).ConfigureAwait(false);
                CheckElfSegementsAndEnableSSAToRaiseException(applicationId, appVersion, flightNumber, nextOrigin, nextDestination, InitializeSeatResponse.Segments);

                response.SelectedTrips = state.Trips;

                foreach (MOBBKTrip selectedTrip in state.Trips)
                {
                    foreach (Model.Shopping.Booking.MOBBKFlattenedFlight ff in selectedTrip.FlattenedFlights)
                    {
                        foreach (Model.Shopping.Booking.MOBBKFlight segment in ff.Flights)
                        {
                            if (segment.Origin.Equals(nextOrigin) && segment.Destination.Equals(nextDestination) && segment.FlightNumber.Equals(flightNumber))
                            {
                                if (!string.IsNullOrEmpty(segment.CheckInWindowText) && segment.IsCheckInWindow)
                                {
                                    throw new MOBUnitedException(segment.CheckInWindowText);
                                }
                            }
                        }
                    }
                }

                AddSeats(ref state, origin, destination, flightNumber, request.FlightDate, paxIndex, request.SeatAssignment);
                List<MOBItem> clientCatalog = request.CatalogValues != null && request.CatalogValues.Any() ? JsonConvert.DeserializeObject<List<MOBItem>>(request.CatalogValues) : null;

                string iOSVersionWithNewSeatMapLegend = _configuration.GetValue<string>("iOSAppVersionWithNewSeatMapLegendForPolaris").ToString();
                string andriodVersionWithNewSeatMapLegend = _configuration.GetValue<string>("AndriodAppVersionWithNewSeatMapLegendForPolaris").ToString();

                bool returnPolarisLegendforSeatMap = GeneralHelper.IsVersion1Greater(appVersion, (applicationId == 1 ? iOSVersionWithNewSeatMapLegend : andriodVersionWithNewSeatMapLegend), true);
                try
                {
                    #region ALM - 26058 - Price Manipulation validation - Srini - 01/04/2016
                    List<MOBSeatMap> MOBSeatMapList = await _sessionHelperService.GetSession<List<MOBSeatMap>>(request.SessionId, ObjectNames.MOBSeatMapListFullName, new List<string> { request.SessionId, ObjectNames.MOBSeatMapListFullName }).ConfigureAwait(false);

                    if (MOBSeatMapList != null && MOBSeatMapList.Count > 0 && state != null && state.Seats != null)
                    {
                        _logger.LogInformation("SelectSeats : SeatMPCache2ValidatPriceManplate {SessionId} {ApplicationId} {ApplicationVersion} and {TransactionId}", MOBSeatMapList, sessionId, request.Application.Id, request.Application.Version.DisplayText, request.TransactionId);

                        isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = true;
                        List<Seat> unavailableSeats = new List<Seat>();
                        bool seatChangeToggle = _configuration.GetValue<bool>("SeatUpgradeForUnavailableSeatCheck");
                        foreach (Seat seat in state.Seats)
                        {
                            if (_configuration.GetValue<bool>("EnableBacklogIssueFixes"))
                            {
                                if (!string.IsNullOrEmpty(seat.SeatAssignment) && seat.Origin == origin && seat.Destination == destination)
                                {
                                    List<MOBSeatB> mobSeatB = (from list in MOBSeatMapList
                                                               from cabin in list.Cabins
                                                               from row in cabin.Rows
                                                               from se in row.Seats
                                                               where se.Number.ToUpper().Trim() == seat.SeatAssignment.ToUpper().Trim()
                                                               select se).ToList();

                                    if ((!mobSeatB.IsNullOrEmpty() && mobSeatB.Count > 0) && mobSeatB[0].ServicesAndFees != null && mobSeatB[0].ServicesAndFees.Count > 0)
                                    {
                                        seat.Price = Convert.ToDecimal(mobSeatB[0].ServicesAndFees[0].TotalFee);
                                        seat.PcuOfferOptionId = mobSeatB[0].PcuOfferOptionId;

                                        if (ConfigUtility.IsMFOPCatalogEnabled(clientCatalog))
                                        {
                                            seat.Miles = mobSeatB[0].ServicesAndFees[0].MilesFee;
                                            seat.DisplayMiles = mobSeatB[0].ServicesAndFees[0].DisplayMilesFee;
                                            seat.MilesAfterTravelerCompanionRules = mobSeatB[0].ServicesAndFees[0].MilesFee;
                                        }
                                    }
                                    if (seatChangeToggle && mobSeatB.Count > 0 && mobSeatB[0].seatvalue == "X")
                                    {
                                        unavailableSeats.Add(seat);
                                    }
                                }
                            }
                            else
                            {
                                if (seat.SeatAssignment != string.Empty && seat.Origin == origin && seat.Destination == destination)
                                {

                                    List<MOBSeatB> mobSeatB = (from list in MOBSeatMapList
                                                               from cabin in list.Cabins
                                                               from row in cabin.Rows
                                                               from se in row.Seats
                                                               where se.Number.ToUpper().Trim() == seat.SeatAssignment.ToUpper().Trim()
                                                               select se).ToList();

                                    if ((!mobSeatB.IsNullOrEmpty() && mobSeatB.Count > 0) && mobSeatB[0].ServicesAndFees != null && mobSeatB[0].ServicesAndFees.Count > 0)
                                    {
                                        seat.Price = Convert.ToDecimal(mobSeatB[0].ServicesAndFees[0].TotalFee);
                                        seat.PcuOfferOptionId = mobSeatB[0].PcuOfferOptionId;

                                        if (ConfigUtility.IsMFOPCatalogEnabled(clientCatalog))
                                        {
                                            seat.Miles = mobSeatB[0].ServicesAndFees[0].MilesFee;
                                            seat.DisplayMiles = mobSeatB[0].ServicesAndFees[0].DisplayMilesFee;
                                            seat.MilesAfterTravelerCompanionRules = mobSeatB[0].ServicesAndFees[0].MilesFee;
                                        }
                                    }

                                    if (seatChangeToggle && mobSeatB[0].seatvalue == "X")
                                    {
                                        unavailableSeats.Add(seat);
                                    }
                                }
                            }
                        }
                        if (seatChangeToggle && unavailableSeats.Count > 0)
                        {
                            foreach (var seat in unavailableSeats)
                            {
                                state.Seats.Remove(seat);
                            }
                        }
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    if (_configuration.GetValue<string>("SeatNotFoundAtCompleteSeatsSelection") != null)
                    {
                        string exMessage = ex.Message != null ? ex.Message : "";
                        throw new Exception(_configuration.GetValue<string>("SeatNotFoundAtCompleteSeatsSelection").ToString() + " - " + exMessage);
                    }
                }

                response.BookingTravelerInfo = state.BookingTravelerInfo;
                IEnumerable<Seat> returnSeatsList = from s in state.Seats
                                                    where s.Origin == nextOrigin.Trim().ToUpper()
                                                    && s.Destination == nextDestination.Trim().ToUpper()
                                                    select s;
                if (returnSeatsList.Count() > 0)
                {
                    response.Seats = returnSeatsList.ToList();
                }

                bool isVerticalSeatMapEnabled = InitializeSeatResponse.IsVerticalSeatMapEnabled;
                response.IsVerticalSeatMapEnabled = isVerticalSeatMapEnabled;
                bool isLandTransport = false;

                bool isOANoSeatMapAvailableNewMessageEnabled = await _featureSettings.GetFeatureSettingValue("EnableNewOASeatMapUnavailableMessage");

                int segmentIndex = 0;
                List<Mobile.Model.Shopping.MOBSeatMap> seatMap = new List<Mobile.Model.Shopping.MOBSeatMap>();
                foreach (MOBBKTrip selectedTrip in state.Trips)
                {
                    foreach (Model.Shopping.Booking.MOBBKFlattenedFlight ff in selectedTrip.FlattenedFlights)
                    {
                        foreach (Model.Shopping.Booking.MOBBKFlight segment in ff.Flights)
                        {
                            ++segmentIndex;
                            if (segment.Origin.Equals(nextOrigin) && segment.Destination.Equals(nextDestination))
                            {
                                int sIndex = 0;
                                Int32.TryParse(segment.FlightId, out sIndex);
                                bool isElf = Common.Helper.UtilityHelper.IsElfSegment(segment.MarketingCarrier, segment.ServiceClass);
                                isLandTransport = _shoppingUtility.IsLandTransport(segment.EquipmentDisclosures.EquipmentType);

                                int noOfTravelersWithNoSeat;
                                int noOfFreeEplusEligibleRemaining = _seatEngine.GetNoOfFreeEplusEligibleRemaining(response.BookingTravelerInfo, nextOrigin, nextDestination, state.TotalEplusEligible, isElf, out noOfTravelersWithNoSeat);
                                bool isOaSeatMapSegment = false;
                                bool hideSeatMap = false;
                                bool isDeepLink = false;
                                if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major))
                                {
                                    bool isInterLine = _shoppingUtility.IsInterLine(segment.OperatingCarrier, segment.MarketingCarrier);
                                    bool isOperatedByOtherAirlines = _shoppingUtility.IsOperatedByOtherAirlines(segment.OperatingCarrier, segment.MarketingCarrier, segment.EquipmentDisclosures != null ? segment.EquipmentDisclosures.EquipmentType : null);
                                    isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(segment.OperatingCarrier);
                                    hideSeatMap = (isOperatedByOtherAirlines && !isDeepLink);
                                    isOaSeatMapSegment = ((isInterLine || isOperatedByOtherAirlines));
                                }
                                else if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion))
                                {
                                    isOaSeatMapSegment = _shoppingUtility.IsInterLine(segment.OperatingCarrier, segment.MarketingCarrier);
                                    bool isOperatedByOtherAirlines = _shoppingUtility.IsOperatedByOtherAirlines(segment.OperatingCarrier, segment.MarketingCarrier, segment.EquipmentDisclosures != null ? segment.EquipmentDisclosures.EquipmentType : null);
                                    isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(segment.OperatingCarrier);
                                    if (isOperatedByOtherAirlines && !isDeepLink)
                                    {
                                        string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await _seatMapCSL30.GetOANoSeatMapAvailableNewMessage(session) : _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody");
                                        throw new MOBUnitedException(HttpUtility.HtmlDecode(OANoSeatMapAvailableMessage));
                                    }
                                }
                                else
                                {
                                    isOaSeatMapSegment = _shoppingUtility.IsSeatMapSupportedOa(segment.OperatingCarrier, segment.MarketingCarrier);
                                }

                                string flow = FlowType.VIEWRES.ToString();
                                // Sending true to isSeatFocusEnabled
                                // We are taking flightId for segment index and flight id is segment number from PNR.
                                // We are assigning same value for originalSegmentNumber and need to filter flight segment with originalsegment number with new implementation.
                                // XML is passing the flightid(Segmentnumber) to seat engine and seats team will send response.
                                if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(applicationId, appVersion) && hideSeatMap)
                                {
                                    seatMap = null;
                                }
                                else
                                {
                                    bool isSeatMapSignInUserDataChangeEnabled = await _featureSettings.GetFeatureSettingValue("EnableSeatMapSignInUserDataChange");

                                    seatMap = await _seatMapCSL30.GetCSL30SeatMapForRecordLocatorWithLastName(request.SessionId, state.RecordLocator,
                                            sIndex, request.LanguageCode, segment.ServiceClassDescription, state.LastName, segment.ChangeOfGauge,
                                            nextOrigin, nextDestination, applicationId, appVersion, isElf, segment.IsIBE, noOfTravelersWithNoSeat,
                                            noOfFreeEplusEligibleRemaining, isOaSeatMapSegment, state.Segments, segment.OperatingCarrier, request.DeviceId,
                                            state.BookingTravelerInfo, flow, isVerticalSeatMapEnabled, isOANoSeatMapAvailableNewMessageEnabled, state.TotalEplusEligible, state.TravelerSignInData, isSeatMapSignInUserDataChangeEnabled,
                                            isOneofTheSegmentSeatMapShownForMultiTripPNRMRes, true, clientCatalog, cartId: state.CartId);
                                }

                                if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && ConfigUtility.IsDeepLinkSupportedAirline(segment.OperatingCarrier, applicationId, appVersion, clientCatalog) && (seatMap == null || (seatMap != null && seatMap.Count > 0 && seatMap.FirstOrDefault().HasNoComplimentarySeatsAvailableForOA)))
                                {
                                    response.InterLineDeepLink = _seatMapCSL30.GetInterlineRedirectLink(response?.BookingTravelerInfo, "US", request, state.RecordLocator, state.LastName, clientCatalog, segment.OperatingCarrier, nextOrigin, nextDestination, segment.DepartDate);
                                }
                                else if (!_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion) && isOaSeatMapSegment && ConfigUtility.IsDeepLinkSupportedAirline(segment.OperatingCarrier, applicationId, appVersion, clientCatalog) && (seatMap == null || (seatMap != null && seatMap.Count > 0 && seatMap.FirstOrDefault().HasNoComplimentarySeatsAvailableForOA)))
                                {
                                    response.InterLineDeepLink = _seatMapCSL30.GetInterlineRedirectLink(response?.BookingTravelerInfo, "US", request, state.RecordLocator, state.LastName, clientCatalog, segment.OperatingCarrier, nextOrigin, nextDestination, segment.DepartDate);
                                }

                                if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major) && isOaSeatMapSegment
                                        && (seatMap == null || (seatMap != null && seatMap.Count == 1 && seatMap[0] != null && seatMap[0].HasNoComplimentarySeatsAvailableForOA)))
                                {
                                    if (isDeepLink)
                                        response.InterLineDeepLink = _seatMapCSL30.GetInterlineRedirectLink(response?.BookingTravelerInfo, "US", request, state.RecordLocator, state.LastName, clientCatalog, segment.OperatingCarrier, nextOrigin, nextDestination, segment.DepartDate);
                                    else
                                        response.InterLineDeepLink = await GetOANoSeatAvailableMessage(nextOrigin, nextDestination, segment.DepartDate, isOANoSeatMapAvailableNewMessageEnabled, session);
                                }
                                
                                bool tomToggle = _configuration.GetValue<bool>("EnableProjectTOM");
                                bool isBus = false;

                                if (_configuration.GetValue<bool>("HandleNullExceptionDueToSeatEngineWSError") && seatMap != null)
                                    isBus = (seatMap[0].FleetType.Length >= 2 && seatMap[0].FleetType.Substring(0, 2).ToUpper().Equals("BU"));

                                else if (!_configuration.GetValue<bool>("HandleNullExceptionDueToSeatEngineWSError"))
                                    isBus = (seatMap[0].FleetType.Length >= 2 && seatMap[0].FleetType.Substring(0, 2).ToUpper().Equals("BU"));

                                if (seatMap != null && seatMap.Count == 1 && seatMap[0] != null && seatMap[0].IsOaSeatMap && (!tomToggle || (tomToggle && !isBus)))
                                {
                                    seatMap[0].OperatedByText = _seatEngine.GetOperatedByText(segment.MarketingCarrier, segment.FlightNumber, segment.OperatingCarrierDescription);
                                    if (_shoppingUtility.EnableOAMessageUpdate(applicationId, appVersion))
                                    {
                                        var operatingCarrierDescription = UtilityHelper.RemoveString(segment?.OperatingCarrierDescription, "Limited");
                                        if (!string.IsNullOrEmpty(operatingCarrierDescription) && !string.IsNullOrEmpty(segment?.MarketingCarrier))
                                        {
                                            seatMap[0].ShowInfoTitleForOA = String.Format(_configuration.GetValue<string>("SeatMapMessageForEligibleOATitle"), segment?.MarketingCarrier, segment.FlightNumber, operatingCarrierDescription);
                                        }
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

                if (isVerticalSeatMapEnabled && isLandTransport && (seatMap == null || (seatMap.Count == 1 && seatMap[0] == null)))
                {
                    response.InterLineDeepLink = GetLandTransportMessage();
                }

                bool isPreferredZoneEnabled = ConfigUtility.EnablePreferredZone(applicationId, appVersion);
                
                response.SeatMap = _seatEngine.GetSeatMapWithPreAssignedSeats(seatMap, response.Seats, isPreferredZoneEnabled);

                bool bookingTravelerOldSeatLocationEnabled = await _featureSettings.GetFeatureSettingValue("EnableBookingTravelerOldSeatLocation");

                if (response.IsVerticalSeatMapEnabled && bookingTravelerOldSeatLocationEnabled)
                {
                    response.BookingTravelerInfo = _seatEngine.GetBookingTravelerInfoWithSeatLocation(seatMap, response.BookingTravelerInfo);
                }

                #region check version For DAA
                ChangeDAAMapForUperVersion(request, response);
                #endregion

                if (isVerticalSeatMapEnabled && String.IsNullOrEmpty(request.SeatAssignment))
                {
                    List<MOBSeatMap> MOBSeatMapList = _sessionHelperService.GetSession<List<MOBSeatMap>>(request.SessionId, ObjectNames.MOBSeatMapListFullName, new List<string> { request.SessionId, ObjectNames.MOBSeatMapListFullName }).Result;

                    if (MOBSeatMapList == null)
                    {
                        MOBSeatMapList = new List<MOBSeatMap>();
                        if (response.SeatMap != null)
                            MOBSeatMapList.AddRange(response.SeatMap);
                    }
                    else if (response.SeatMap != null && response.SeatMap.Count > 0)
                    {
                        var existingSeatMap = MOBSeatMapList.Where(s => s.Departure.Code == response.SeatMap[0].Departure.Code
                                                                    && s.Arrival.Code == response.SeatMap[0].Arrival.Code
                                                                    && string.Equals(s.FlightDateTime, response.SeatMap[0].FlightDateTime)
                                                                    && s.FlightNumber == response.SeatMap[0].FlightNumber).FirstOrDefault();

                        if (existingSeatMap != null)
                        {
                            MOBSeatMapList.Remove(existingSeatMap);
                        }

                        MOBSeatMapList.AddRange(response.SeatMap);
                    }

                    await _sessionHelperService.SaveSession<List<Model.Shopping.MOBSeatMap>>(MOBSeatMapList, sessionId, new List<string> { sessionId, ObjectNames.MOBSeatMapListFullName }, ObjectNames.MOBSeatMapListFullName);
                }
                else
                {
                    await _sessionHelperService.SaveSession<List<Model.Shopping.MOBSeatMap>>(response.SeatMap, sessionId, new List<string> { sessionId, ObjectNames.MOBSeatMapListFullName }, ObjectNames.MOBSeatMapListFullName);
                }

                response.ExitAdvisory = UtilityHelper.GetExitAdvisory();
                await _sessionHelperService.SaveSession<SeatChangeState>(state, sessionId, new List<string> { request.SessionId, state.ObjectName }, state.ObjectName);
            }
            else
            {
                throw new MOBUnitedException("The access code you provided is not valid.");
            }
            return response;
        }

        private async Task<InterLineDeepLink> GetOANoSeatAvailableMessage(string origin, string destination, string departDate, bool isOANoSeatMapAvailableNewMessageEnabled, Session session)
        {
            string depTimeFormatted = Convert.ToDateTime(departDate).ToString("ddd, MMM dd");
            return new InterLineDeepLink()
            {
                ShowInterlineAdvisoryMessage = true,
                InterlineAdvisoryDeepLinkURL = string.Empty,
                InterlineAdvisoryMessage = isOANoSeatMapAvailableNewMessageEnabled ? await _seatMapCSL30.GetOANoSeatMapAvailableNewMessageBodyFromSDL(session) : _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody"),
                InterlineAdvisoryTitle = $"{depTimeFormatted} {origin} - {destination}",
                InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("OANoSeatMapAvailableMessageTitle")
            };
        }

        private InterLineDeepLink GetLandTransportMessage()
        {
            return new InterLineDeepLink()
            {
                ShowInterlineAdvisoryMessage = true,
                InterlineAdvisoryDeepLinkURL = string.Empty,
                InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("SelectSeats_BusServiceErrorTitle"),
                InterlineAdvisoryMessage = _configuration.GetValue<string>("SelectSeats_BusServiceError"),                
            };
        }

        public async Task<MOBSeatChangeInitializeResponse> SeatChangeInitialize(MOBSeatChangeInitializeRequest request)
        { 
            return await _manageReservation.SeatChangeInitialize(request);
        }

        private bool ShouldCreateNewCart(string cartId)
        {
            if (!string.IsNullOrEmpty(cartId))
                return false;

            return string.IsNullOrEmpty(cartId);
        }
        
        private void AddSeats(ref SeatChangeState state, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment)
        {
            string[] paxIndexArray = paxIndex.Split(',');
            if (_headers.ContextValues.Application.Id == 2 && !_configuration.GetValue<bool>("DisableFixForAddSeatsACM2753"))
            {
                seatAssignment = seatAssignment.Replace("0,00", "0.00");
            }
            string[] seatAssignmentArray = seatAssignment.Split(',');
            if (state.Seats != null)
            {
                for (int i = 0; i < seatAssignmentArray.Length; i++)
                {
                    IEnumerable<Seat> seatList = from s in state.Seats
                                                 where s.Origin == origin.Trim().ToUpper()
                                                 && s.Destination == destination.Trim().ToUpper()
                                                 && s.TravelerSharesIndex == paxIndexArray[i]
                                                 && Convert.ToDateTime(s.DepartureDate).ToString("MMddyyy") == Convert.ToDateTime(flightDate).ToString("MMddyyy")
                                                 && s.FlightNumber == flightNumber
                                                 select s;
                    if (seatList.Count() > 0)
                    {
                        List<Seat> seats = new List<Seat>();
                        seats = seatList.ToList();
                        if (seats.Count() == 1)
                        {
                            Seat aSeat = new Seat();
                            aSeat.Destination = destination;
                            aSeat.Origin = origin;
                            aSeat.FlightNumber = flightNumber;
                            aSeat.DepartureDate = flightDate;
                            aSeat.TravelerSharesIndex = seats[0].TravelerSharesIndex;
                            aSeat.Key = seats[0].Key;
                            aSeat.OldSeatAssignment = seats[0].OldSeatAssignment;
                            aSeat.OldSeatCurrency = seats[0].OldSeatCurrency;
                            aSeat.OldSeatPrice = seats[0].OldSeatPrice;
                            aSeat.OldSeatType = seats[0].OldSeatType;
                            aSeat.OldSeatProgramCode = seats[0].OldSeatProgramCode;
                            aSeat.OldSeatMiles = seats[0].OldSeatMiles;

                            string[] assignments = seatAssignmentArray[i].Split('|');
                            if (assignments.Length == 5)
                            {
                                aSeat.SeatAssignment = assignments[0];
                                aSeat.Price = Convert.ToDecimal(assignments[1]);
                                aSeat.PriceAfterTravelerCompanionRules = aSeat.Price;
                                aSeat.Currency = assignments[2];
                                aSeat.ProgramCode = assignments[3];
                                aSeat.SeatType = assignments[4];
                                aSeat.LimitedRecline = (aSeat.ProgramCode == "PSL");
                            }
                            else
                            {
                                aSeat.SeatAssignment = seatAssignmentArray[i];
                            }

                            if (_configuration.GetValue<bool>("FixArgumentOutOfRangeExceptionInRegisterSeatsAction"))
                            {
                                state.Seats[state.Seats.IndexOf(seats[0])] = aSeat;
                            }
                            else
                            {
                                state.Seats[seats[0].Key] = aSeat;
                            }
                        }
                    }
                    else
                    {
                        Seat aSeat = new Seat();
                        aSeat.Destination = destination;
                        aSeat.Origin = origin;
                        aSeat.FlightNumber = flightNumber;
                        aSeat.DepartureDate = flightDate;

                        string[] assignments = seatAssignmentArray[i].Split('|');
                        if (assignments.Length == 5)
                        {
                            aSeat.SeatAssignment = assignments[0];
                            aSeat.Price = Convert.ToDecimal(assignments[1]);
                            aSeat.PriceAfterTravelerCompanionRules = aSeat.Price;
                            aSeat.Currency = assignments[2];
                            aSeat.ProgramCode = assignments[3];
                            aSeat.SeatType = assignments[4];
                            aSeat.LimitedRecline = (aSeat.ProgramCode == "PSL");
                        }
                        else
                        {
                            aSeat.SeatAssignment = seatAssignmentArray[i];
                        }

                        aSeat.TravelerSharesIndex = paxIndexArray[i];
                        aSeat.Key = state.Seats.Count;
                        state.Seats.Add(aSeat);
                    }
                }
            }
            else
            {
                for (int i = 0; i < seatAssignment.Split(',').Length; i++)
                {
                    Seat aSeat = new Seat();
                    aSeat.Destination = destination;
                    aSeat.Origin = origin;
                    aSeat.FlightNumber = flightNumber;
                    aSeat.DepartureDate = flightDate;

                    string[] assignments = seatAssignmentArray[i].Split('|');
                    if (assignments.Length == 5)
                    {
                        aSeat.SeatAssignment = assignments[0];
                        aSeat.Price = Convert.ToDecimal(assignments[1]);
                        aSeat.PriceAfterTravelerCompanionRules = aSeat.Price;
                        aSeat.Currency = assignments[2];
                        aSeat.ProgramCode = assignments[3];
                        aSeat.SeatType = assignments[4];
                        aSeat.LimitedRecline = (aSeat.ProgramCode == "PSL");
                    }
                    else
                    {
                        aSeat.SeatAssignment = seatAssignmentArray[i];
                    }

                    aSeat.TravelerSharesIndex = paxIndexArray[i];
                    aSeat.Key = state.Seats.Count;
                    state.Seats.Add(aSeat);
                }
            }
            if (state.Seats != null)
            {
                foreach (MOBBKTraveler traveler in state.BookingTravelerInfo)
                {
                    IEnumerable<Seat> seatList = from s in state.Seats
                                                 where s.TravelerSharesIndex == traveler.SHARESPosition
                                                 select s;
                    if (seatList.Count() > 0)
                    {
                        List<Seat> travelerSeats = new List<Seat>();
                        travelerSeats = seatList.ToList();
                        travelerSeats = travelerSeats.OrderBy(s => s.Key).ToList();
                        traveler.Seats = travelerSeats;
                    }
                }
            }
        }

        private void CheckElfSegementsAndEnableSSAToRaiseException(int applicationId, string appVersion, string flightNumber,
            string nextOrigin, string nextDestination, List<TripSegment> segments)
        {
            if (!ConfigUtility.EnableSSA(applicationId, appVersion) && MatchElfSegment(segments, nextOrigin, nextDestination, flightNumber))
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("ELFManageResAdvisoryMsg"));
            }
        }

        private bool MatchElfSegment(List<TripSegment> segments, string origin, string destination, string flightNumber)
        {
            return (segments != null && segments.Count > 0 && segments.Exists(p => p.Arrival.Code == destination.ToUpper() &&
                                                                                   p.Departure.Code == origin.ToUpper() &&
                                                                                   p.IsELF));
        }

        private void ChangeDAAMapForUperVersion(MOBSeatChangeSelectRequest request, MOBSeatChangeSelectResponse response)
        {
            bool replaceDAFRMtoDAFL = _configuration.GetValue<bool>("ReplaceDAFRMtoDAFR");
            if (replaceDAFRMtoDAFL)
            {
                string iOSVersionWithNewDAASeatMap = _configuration.GetValue<string>("iOSVersionWithNewDAASeatMap").ToString();
                string andriodVersionWithNewDAASeatMap = _configuration.GetValue<string>("andriodVersionWithNewDAASeatMap").ToString();
                var versionWithNewDAASeatMap = iOSVersionWithNewDAASeatMap;
                if (request.Application.Id == 2)
                {
                    versionWithNewDAASeatMap = andriodVersionWithNewDAASeatMap;
                }
                bool returnNewDAASeatMap = GeneralHelper.IsVersion1Greater(request.Application.Version.Major, versionWithNewDAASeatMap, true);
                if (!returnNewDAASeatMap && response.SeatMap != null)
                {
                    response.SeatMap.All(d => d.Cabins.All(a => a.Rows.All(b => b.Seats.All(c => { c.Program = (c.Program.Contains("DAFRM")) ? c.Program.Replace("DAFRM", "DAFL") : c.Program; return true; }))));
                }
            }

        }
    }
}
