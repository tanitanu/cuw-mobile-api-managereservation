using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.PreOrderMeals;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Payment;
using United.Mobile.Model.PreOrderMeals;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.FormofPayment;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.CommonModel.AircraftModel;
using United.Service.Presentation.PersonalizationRequestModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ProductRequestModel;
using United.Service.Presentation.ReservationModel;
using United.Service.Presentation.ReservationRequestModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.FlightReservation;
using United.Utility.Enum;
using United.Utility.Helper;
using Aircraft = United.Service.Presentation.CommonModel.AircraftModel.Aircraft;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using FlightSegment = United.Mobile.Model.PreOrderMeals.FlightSegment;
using IMerchandizingServices = United.Common.Helper.Merchandize.IMerchandizingServices;
using MerchandizingOfferDetails = United.Mobile.Model.Shopping.FormofPayment.MerchandizingOfferDetails;
using MOBPNRByRecordLocatorRequest = United.Mobile.Model.PreOrderMeals.MOBPNRByRecordLocatorRequest;
using Passenger = United.Mobile.Model.PreOrderMeals.Passenger;
using RegisterOfferRequest = United.Mobile.Model.Shopping.FormofPayment.RegisterOfferRequest;
using Traveler = United.Service.Presentation.ReservationModel.Traveler;

namespace United.Mobile.Services.PreOrderMeals.Domain
{
    public class PreOrderMealsBusiness : IPreOrderMealsBusiness
    {
        private readonly ICacheLog<PreOrderMealsBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IFFCShoppingcs _fFCShoppingcs;
        private readonly IMerchandizingServices _merchandizingServices;
        private List<CMSContentMessage> cmsContents;
        private readonly IRegisterCFOP _registerCFOP;
        private readonly ISSOTokenKeyHelper _sSOTokenKeyHelper;
        private readonly IFlightReservation _flightReservation;
        private readonly IUnfinishedBooking _unfinishedBooking;
        private readonly IDPService _dPService;
        private readonly IPreOrderMealRegisterService _preOrderMealRegisterService;
        private readonly IGetMealOfferDetailsFromCslService _getMealOfferDetailsFromCslService;
        private readonly IGetPNRByRecordLocatorService _getPNRByRecordLocatorService;
        private readonly IRegisterOffersService _registerOffersService;
        private const string cryptoKey = "8a6b7c5d3e0f9";
        private const string IV = "8AMtHQBMrTs=";
        private const string MEAL_PRODUCT_CODE = "FOOD";
        private const string MEAL_PAST_SELECTED_CODE = "PAST_SELECTION";
        private const string MEAL_QUANTITY_CODE = "Quantity";
        private const string MEAL_SERVICE_CODE = "MealServiceCode";
        private const string MEAL_TYPE_CODE = "Category";
        private const string MEAL_ERROR_CODE = "POMM00001";
        private const string MEAL_ADD_TO_CART_ERROR_CODE = "POMM00002";
        private const string MEAL_INELIGIBLE_ERROR_CODE = "POMM00003";
        private const string MEAL_ADD_TO_CART_CODE = "CreateCart";
        private const string MEAL_ADD_TO_CART_DMS_ERROR_CODE = "100998";
        private const string SPECIAL_MEAL_NAME = "Request a special meal";
        private const string SPECIAL_MEAL_CODE = "SPML";
        private const string SPECIAL_MEAL_TITLE = "Select one special meal";
        private const string POLARIS_CODE = "EAUPL";
        private const string MEAL_ADD_TO_CART_SUCCESS_CODE = "1";
        private const string OTHER_MEAL_TITLE = "Or if you have a different plan";
        private const string NON_MEAL_GROUP_CODE = "NonMeal";
        private const string DONT_WANT_MEAL_CODE = "111111";
        private const string DECIDE_LATER_CODE = "222222";
        private const string POLARISE_CODE = "333333";
        private const string OFFER_TYPE = "OfferProvisionType";
        private const string DYNAMIC_OFFER = "Dynamic";
        private const string STATIC_OFFER = "Static";
        private const string REQUESTTYPE_VALUE = "POMPhase1";
        private const string REQUESTTYPE_CODE = "RequestType";
        private const string SEQUENCENUMBER_ONE = "1";
        private const string ORDER_ID = "OrderID";
        private const string OFFER_QUANTITY = "OfferQty";
        private const string IMAGE_URL = "ImageURL";
        private const string SEQUENCE_NUMBER = "ServiceSequenceNumber";
        private const string AVAILABLEMEAL_QUANTITY_CODE = "AvailableQuantity";
        private readonly List<string> segmentIneligibleErrorCodes = new List<string>() { "E001", "E004" };
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IFeatureSettings _featureSettings;

        public PreOrderMealsBusiness(ICacheLog<PreOrderMealsBusiness> logger
            , IConfiguration configuration
            , IHeaders headers
            , IShoppingSessionHelper shoppingSessionHelper
            , ISessionHelperService sessionHelperService
            , IFFCShoppingcs fFCShoppingcs
            , IMerchandizingServices merchandizingServices
            , IRegisterCFOP registerCFOP
            , ISSOTokenKeyHelper sSOTokenKeyHelper
            , IFlightReservation flightReservation
            , IUnfinishedBooking unfinishedBooking
            , IDPService dPService
            , IPreOrderMealRegisterService preOrderMealRegisterService
            , IGetMealOfferDetailsFromCslService getMealOfferDetailsFromCslService
            , IGetPNRByRecordLocatorService getPNRByRecordLocatorService
            , IRegisterOffersService registerOffersService
            , IShoppingUtility shoppingUtility
            , IFeatureSettings featureSettings)

        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _shoppingSessionHelper = shoppingSessionHelper;
            _sessionHelperService = sessionHelperService;
            _fFCShoppingcs = fFCShoppingcs;
            _merchandizingServices = merchandizingServices;
            _registerCFOP = registerCFOP;
            _sSOTokenKeyHelper = sSOTokenKeyHelper;
            _flightReservation = flightReservation;
            _unfinishedBooking = unfinishedBooking;
            _dPService = dPService;
            _preOrderMealRegisterService = preOrderMealRegisterService;
            _getMealOfferDetailsFromCslService = getMealOfferDetailsFromCslService;
            _getPNRByRecordLocatorService = getPNRByRecordLocatorService;
            _registerOffersService = registerOffersService;
            _shoppingUtility = shoppingUtility;
            _featureSettings = featureSettings;
        }
        public async Task<MOBInFlightMealsOfferResponse> GetInflightMealOffers(MOBInFlightMealsOfferRequest request)
        {
            _logger.LogInformation("GetInflightMealOffers request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            MOBInFlightMealsOfferResponse response = new MOBInFlightMealsOfferResponse();

            var session = await GetSession(request, request.SessionId);
            await LoadCMSContents(request, session).ConfigureAwait(false);
            response = await GetInflightMealOffers(request, session);
            response.SessionId = session.SessionId;
            response.ProductCode = request.ProductCode;
            response.TransactionId = request.TransactionId;

            return await System.Threading.Tasks.Task.FromResult(response);
        }

        public async Task<MOBInFlightMealsOfferResponse> GetInflightMealOffersForDeeplink(MOBInFlightMealsOfferRequest request)
        {
            _logger.LogInformation("GetInflightMealOffersForDeeplink request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            MOBInFlightMealsOfferResponse response = new MOBInFlightMealsOfferResponse();
            request.InflightMealOffersActionType = InflightMealOffersFlowType.TapOnDeeplinkClick;
            string pnr = DecryptString(request.EncryptedQueryString);
            var decryptedData = new
            {
                Pnr = string.Empty,
                LastName = string.Empty
            };

            decryptedData = JsonConvert.DeserializeAnonymousType(pnr, decryptedData);
            var mobPnrRequest = new MOBPNRByRecordLocatorRequest();

            //GetPNRByRecordLocator - Request Mapping
            mobPnrRequest.Application = new MOBApplication();
            mobPnrRequest.Application = request.Application;
            mobPnrRequest.DeviceId = request.DeviceId;
            mobPnrRequest.TransactionId = request.TransactionId;
            mobPnrRequest.RecordLocator = (string.IsNullOrEmpty(request.EncryptedQueryString)) ? request.RecordLocator : decryptedData?.Pnr;
            mobPnrRequest.LastName = (string.IsNullOrEmpty(request.EncryptedQueryString)) ? request.LastName : decryptedData?.LastName;
            mobPnrRequest.Flow = Convert.ToString(FlowType.VIEWRES);
            var jsonRequest = JsonConvert.SerializeObject(mobPnrRequest);
            var jsonResponse = await _getPNRByRecordLocatorService.GetPNRByRecordLocator(jsonRequest, request.TransactionId, "/ManageReservation/GetPNRByRecordLocator").ConfigureAwait(false);
            var pnrResponse = JsonConvert.DeserializeObject<MOBPNRByRecordLocatorResponse>(jsonResponse.ToString());
            request.SessionId = pnrResponse.SessionId;
            request.ProductCode = _configuration.GetValue<string>("InflightMealProductCode");
            var session = await GetSession(request, request.SessionId);
            await LoadCMSContents(request, session);
            response = await GetInflightMealOffers(request, session);
            response.RecordLocator = (string.IsNullOrEmpty(request.EncryptedQueryString)) ? request.RecordLocator : decryptedData?.Pnr;
            response.LastName = (string.IsNullOrEmpty(request.EncryptedQueryString)) ? request.LastName : decryptedData?.LastName;
            response.SessionId = pnrResponse.SessionId;
            response.EnableBackButton = false;
            response.EnableReservationDetailButton = true;
            response.TransactionId = request.TransactionId;
            return await System.Threading.Tasks.Task.FromResult(response);
        }

        public async Task<PreOrderMealTripDetailResponseContext> GetPreOrderMealsTripDetailsV2(
   MOBPNRByRecordLocatorRequest mobilePnrRequest, Session session)
        {
            var pnrResponseContext = new PreOrderMealTripDetailResponseContext();
            mobilePnrRequest.LanguageCode = string.IsNullOrWhiteSpace(mobilePnrRequest.LanguageCode)
                ? "en-US"
                : mobilePnrRequest.LanguageCode;
            try
            {
                //Decrypt PNR using CSL Partner Single SignOn API
                await DecryptConfirmationNumberAndLastName(mobilePnrRequest, session.Token);

                //Get PNR information from PNR details CLS   
                var mobilePnrResponse =
                    await GetPNRFlightSegmentsFromCSL(mobilePnrRequest, session, true);


                if (mobilePnrResponse != null)
                {
                    // Return the exception message, from PNR API call
                    if (mobilePnrResponse.Exception != null)
                    {
                        throw new MOBUnitedException(mobilePnrResponse.Exception.Code,
                            mobilePnrResponse.Exception.Message);
                    }

                    if (mobilePnrResponse.PNR?.Segments != null)
                    {
                        //Get only active segments which is under 24 hour window time.
                        mobilePnrResponse.PNR.Segments = mobilePnrResponse.PNR.Segments
                            .Where(s => !IsSegmentMoreThan24Hours(s.ScheduledDepartureDateTime))
                            .OrderBy(s => Convert.ToDateTime(s.ScheduledDepartureDateTime)).ToList();
                        if (mobilePnrResponse.PNR.Segments.Count == 0)
                        {
                            throw new MOBUnitedException(
                                _configuration.GetValue<String>("NoSegmentsFoundErrorMessage"));
                        }

                        //Get Meal Service offer response from CSL
                        var mealServiceResponse = GetMealServiceOfferDetailsV2(mobilePnrResponse, mobilePnrRequest,
                            session.Token, "GetPreOrderMealsDetails", null);

                        pnrResponseContext = null;
                        pnrResponseContext.ConfirmationNumber = mobilePnrResponse.PNR.RecordLocator;
                        pnrResponseContext.LastName = mobilePnrRequest.LastName;
                    }

                    pnrResponseContext.SessionId = session.SessionId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPreOrderMealsTripDetailsV2 exception:{ex}", ex.Message);

                throw;
            }

            return await System.Threading.Tasks.Task.FromResult(pnrResponseContext);
        }


        public async Task<MOBInFlightMealsRefreshmentsResponse> GetInflightMealRefreshments(MOBInFlightMealsRefreshmentsRequest request)
        {
            _logger.LogInformation("GetInflightMealRefreshments request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            MOBInFlightMealsRefreshmentsResponse response = new MOBInFlightMealsRefreshmentsResponse();

            var session = await GetSession(request, request.SessionId);
            await LoadCMSContents(request, session);
            DynamicOfferDetailResponse persistDynamicOfferDetailResponse = await GetCCEOffersFromPersist(request.SessionId);
            if (persistDynamicOfferDetailResponse != null)
            {
                if (IsDynamicPOMOffer(request.Application.Id, request.Application.Version.Major))
                {
                    response = await GetRefreshmentsV2(request, persistDynamicOfferDetailResponse);
                }
                else
                {
                    response = await GetRefreshments(request, persistDynamicOfferDetailResponse);
                }
            }
            if (request.InflightMealRefreshmentsActionType == InflightMealRefreshmentsActionType.TapOnSaveNContinue && response != null && response.Exception == null
                && (response.AlertMessages == null || response.AlertMessages.Count == 0))
            {
                //RegisterOfferForUpper cabin
                await RegisterUpperCabinMeals(request, response);
            }
            if (!string.IsNullOrEmpty(request.PremiumCabinMealEmailAddress))
                response.PremiumCabinMealEmailAddress = request.PremiumCabinMealEmailAddress;
            response.SessionId = session.SessionId;
            response.ProductCode = request.ProductCode;
            if (response == null || ((response.Snacks == null || response.Snacks.Count == 0) && (response.FreeMeals == null || response.FreeMeals.Count == 0) && (response.Beverages == null || response.Beverages.Count == 0) && (response.DifferentPlanOptions == null || response.DifferentPlanOptions.Count == 0)))
            {
                response.Passenger = null;
                response.DifferentPlanOptions = null;
                throw new MOBUnitedException("10000",
                _configuration.GetValue<string>("GetUnitedClubDetailsByAirportGenericExceptionMessage"));
            }
            response.TransactionId = request.TransactionId;
            return await System.Threading.Tasks.Task.FromResult(response);
        }

        public async Task<PreOrderMealCartResponse> AddToCart(PreOrderMealCartRequest request)
        {
            _logger.LogInformation("AddToCart request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            var preOrderMealResponse = await AddToCartServiceCall(request, false);
            return await System.Threading.Tasks.Task.FromResult(preOrderMealResponse);
        }
        public async Task<PreOrderMealCartResponse> AddToCartV2(PreOrderMealCartRequest request)
        {
            _logger.LogInformation("AddToCartV2 request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            var preOrderMealResponse = await AddToCartServiceCall(request, false);
            return await System.Threading.Tasks.Task.FromResult(preOrderMealResponse);
        }

        public async Task<PreOrderMealResponseContext> GetTripsForPerOrderMeal(MOBPNRByRecordLocatorRequest request)
        {
            _logger.LogInformation("GetTripsForPerOrderMeal request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            var myTripPreOrderMealsResponse = new PreOrderMealResponseContext();
            var session = await GetSession(request, request.SessionId);
            await _sessionHelperService.SaveSession<Session>(session, session.SessionId,
                     new List<string> {session.SessionId,
                    session.ObjectName }, session.ObjectName);
            session.Flow = request.Flow;
            request.SessionId = session.SessionId;

            myTripPreOrderMealsResponse = await GetPreOrderMealsTripDetails(request, session);
            myTripPreOrderMealsResponse.TransactionId = request.TransactionId;
            return await System.Threading.Tasks.Task.FromResult(myTripPreOrderMealsResponse);
        }

        public async Task<MealsDetailResponse> GetAvailableMeals(PreOrderMealListRequest request)
        {
            _logger.LogInformation("GetAvailableMeals request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            var mealsDetailResponse = await LoadAvailableMeals(request, false);
            return await System.Threading.Tasks.Task.FromResult(mealsDetailResponse);
        }
        public async Task<MealsDetailResponse> GetAvailableMealsV2(PreOrderMealListRequest request)
        {
            _logger.LogInformation("GetAvailableMealsV2 request:{request} with sessionId:{Id}", JsonConvert.SerializeObject(request), request.SessionId);
            var mealsDetailResponse = await LoadAvailableMeals(request, true);
            return await System.Threading.Tasks.Task.FromResult(mealsDetailResponse);
        }



        private async Task<MealsDetailResponse> LoadAvailableMeals(PreOrderMealListRequest request, bool isVersion2)
        {
            var mealsDetailResponse = new MealsDetailResponse();
            try
            {
                request.Application = new MOBApplication
                {
                    Id = Convert.ToInt32(request.ApplicationId),
                    Version = new MOBVersion { Major = request.AppVersion }
                };

                var session = await GetSession(request, request.SessionId);
                request.SessionId = session.SessionId;
                if (isVersion2)
                {
                    mealsDetailResponse = await GetAvailableMealsV2(request, session);
                }
                else
                {
                    mealsDetailResponse = await GetAvailableMeals(request, session);
                }
            }
            catch (MOBUnitedException mobEx)
            {

                _logger.LogWarning("LoadAvailableMeals MOBUnitedException:{ex}", mobEx.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("LoadAvailableMeals Exception:{ex}", ex.Message);

                mealsDetailResponse.Exception = new MOBException("10000",
                     _configuration.GetValue<String>("PreOrderMealMealAvailableUnhandledErrorMessage"));

            }
            mealsDetailResponse.TransactionId = request.TransactionId;
            return mealsDetailResponse;
        }

        private PreOrderMealTripDetailResponseContext BuildPreOrderMealTripResponseV2(
            MOBPNRByRecordLocatorRequest mobilePnrRequest,
            MOBPNRByRecordLocatorResponse mobilePnrResponse, MealOfferResponse mealServiceResponse)
        {
            var pnrResponseContext = new PreOrderMealTripDetailResponseContext();
            //Iterate each flight segment response from PNR and map then to pre order meal response.
            foreach (var flightSegment in mobilePnrResponse.PNR.Segments)
            {
                if (flightSegment != null)
                {
                    var passengers = new List<PassengerV2>();
                    if (mobilePnrResponse.PNR.Passengers != null)
                    {
                        var passengerCounter = 1;
                        foreach (var passenger in mobilePnrResponse.PNR.Passengers.OrderBy(p =>
                            Convert.ToDecimal(p.SHARESPosition)))
                        {
                            var firstName = string.Empty;
                            var middleName = string.Empty;
                            var lastName = string.Empty;
                            var fullName = string.Empty;
                            if (passenger.PassengerName != null)
                            {
                                firstName = passenger.PassengerName.First ?? string.Empty;
                                middleName = passenger.PassengerName.Middle ?? string.Empty;
                                lastName = passenger.PassengerName.Last ?? string.Empty;
                                fullName = Regex.Replace(
                                    string.Format("{0} {1} {2}", firstName.ToLower(),
                                        middleName.ToLower(), lastName.ToLower()),
                                    @"\s+", " ");
                            }

                            passengers.Add(new PassengerV2
                            {
                                SharesPosition = passenger.SHARESPosition,
                                PassengerId = passenger.PNRCustomerID,
                                FirstName = firstName,
                                MiddleName = middleName,
                                LastName = lastName,
                                PassengerName = ConvertToTitleCase(fullName),
                                PassengerTypeCode = passenger.TravelerTypeCode,
                                GivenName = firstName
                            });
                            passengerCounter++;
                        }

                    }

                    var flightDeparture = default(FlightTransit);
                    var flightArrival = default(FlightTransit);
                    if (flightSegment.Departure != null)
                    {
                        flightDeparture = new FlightTransit
                        {
                            Code = flightSegment.Departure.Code,
                            Name = flightSegment.Departure.Name,
                            TransitTime = flightSegment.ScheduledDepartureDateTime,
                            City = flightSegment.Departure.City
                        };
                    }

                    if (flightSegment.Arrival != null)
                    {
                        flightArrival = new FlightTransit
                        {
                            Code = flightSegment.Arrival.Code,
                            Name = flightSegment.Arrival.Name,
                            City = flightSegment.Arrival.City,
                            TransitTime = flightSegment.ScheduledArrivalDateTime
                        };
                    }

                    var mobFlightSegment = new FlightSegmentV2
                    {
                        FlightNumber = flightSegment.FlightNumber,
                        TripNumber = flightSegment.TripNumber,
                        SegmentNumber = flightSegment.SegmentNumber,
                        MealType = flightSegment.Meal,
                        Header = GenerateFlightSegmentHeader(flightSegment.ScheduledDepartureDateTime,
                            flightSegment.Departure, flightSegment.Arrival),
                        HeaderDate = Convert.ToDateTime(flightSegment.ScheduledDepartureDateTime).ToString("ddd, MMM. d, yyyy"),
                        HeaderSegment = GenerateFlightSegmentHeaderV2(flightSegment.Departure, flightSegment.Arrival),
                        Passengers = passengers,
                        Departure = flightDeparture,
                        Arrival = flightArrival,
                        AircraftModelCode = flightSegment.Aircraft?.ModelCode,
                        OperatingAirlineCode = flightSegment.OperationoperatingCarrier?.Code
                    };
                    //Update the flight segment meal eligibility and pre selected meal if already selected.
                    UpdatePreOderAndMealEligibleDataFromMealServiceResponseV2(mobFlightSegment,
                        mealServiceResponse, mobilePnrRequest.TransactionId, mobilePnrRequest.RecordLocator);
                    pnrResponseContext.FlightSegments.Add(mobFlightSegment);
                }
            }

            pnrResponseContext.NumberOfPassengers =
                Convert.ToString(mobilePnrResponse.PNR?.Passengers?.Count);
            pnrResponseContext.FooterDescription =
                _configuration.GetValue<string>("PreOrderMealFooterDescription");
            pnrResponseContext.IsAnySegmentEligibleForPreOrderMeal =
                pnrResponseContext.FlightSegments.Any(a => a.IsEligibleForPreOrderMeals);
            pnrResponseContext.FooterDescriptionHtmlContent =
                _configuration.GetValue<string>("PreOrderMealFooterDescription_Html");
            return pnrResponseContext;
        }

        public async Task<MealsDetailResponse> GetAvailableMeals(PreOrderMealListRequest request, Session appSession)
        {
            request.LanguageCode = string.IsNullOrWhiteSpace(request.LanguageCode) ? "en-US" : request.LanguageCode;
            var mealDetailResponse = new MealsDetailResponse();
            var mobilePnrRequest = new MOBPNRByRecordLocatorRequest
            {
                RecordLocator = request.ConfirmationNumber,
                LastName = request.LastName,
                SessionId = request.SessionId,
                Application = request.Application,
                DeviceId = request.DeviceId,
            };
            //Decrypt PNR using CSL Partner Single SignOn API
            await DecryptConfirmationNumberAndLastName(mobilePnrRequest, appSession.Token);

            //Call Light weight PNR to get the segments
            var lightWeightPnrResponse =
                await GetPNRFlightSegmentsFromCSL(mobilePnrRequest, appSession);

            var selectedPnrSegment = lightWeightPnrResponse.PNR?.Segments
                ?.FirstOrDefault(s => s.SegmentNumber == request.SegmentNumber);
            //Get the available meal offers from CSL for the selected segment
            var mealServiceResponse = await GetMealServiceOfferDetails(lightWeightPnrResponse, mobilePnrRequest,
                appSession.Token, "GetPreOrderMealsDetails", selectedPnrSegment);
            var mealAvailabilityErrorMessage = _configuration.GetValue<string>("PreOrderMealAvailabilityErrorMessage");
            if (mealServiceResponse != null)
            {
                if (mealServiceResponse.ErrorResponse?.Error?.Count > 0)
                {
                    var errorResponse = mealServiceResponse.ErrorResponse.Error.FirstOrDefault();
                    throw new MOBUnitedException(errorResponse?.Code ?? MEAL_ERROR_CODE, errorResponse?.Description);
                }

                if (mealServiceResponse.AvailableMeals?.Count > 0)
                {
                    //Check meal count is less than passenger count then meal is not eligible to select. 
                    mealDetailResponse.AvailableMeals = mealServiceResponse.AvailableMeals
                        .Where(m => m.SegmentNumbers.Contains(request.SegmentNumber)).Select(m => new AvailableMeal
                        {
                            MealName = m.MealName,
                            MealCode = m.MealCode,
                            MealDescription = m.MealDescription,
                            MealCount = m.MealCount,
                            MealServiceCode = m.MealServiceCode,
                            MealSourceType = (int)MealSourceType.Regular,
                            IsMealAvailable = m.IsMealAvailable,
                            ErrorMessage =
                                !m.IsMealAvailable
                                    ? mealAvailabilityErrorMessage
                                    : string.Empty,
                            ErrorCode = m.MealCount <= 0 ? MEAL_ERROR_CODE : string.Empty
                        }).OrderBy(m => m.MealName).ThenBy(m => m.MealDescription).ToList();
                }
            }

            if (mealDetailResponse.AvailableMeals.Count == 0)
            {
                throw new MOBUnitedException(MEAL_ERROR_CODE, mealAvailabilityErrorMessage);
            }

            mealDetailResponse.FooterDescription =
                _configuration.GetValue<string>("PreOrderMealSelectionFooterDescription");
            //HotFix - 09/13/2019 for UI Issues - TODO: This is a temp fix .. This needs to be removed once UI Fix go live
            mealDetailResponse.OtherMealOptionTitle = string.Empty;
            mealDetailResponse.OtherMealOptions = new List<BaseMeal>();
            return mealDetailResponse;
        }

        private void UpdatePreOderAndMealEligibleDataFromMealServiceResponseV2(FlightSegmentV2 flightSegment,
            MealOfferResponse mealServiceResponse, string transactionId, string confirmationNumber)
        {
            if (flightSegment != null && mealServiceResponse != null)
            {
                //Get ineligible segment from meal service API response and update isEligible
                var inEligibleSegments =
                    mealServiceResponse.InEligibleSegments?.Where(a =>
                        a.SegmentNumber == flightSegment.SegmentNumber).ToList();
                var hasSegmentIneligible =
                    inEligibleSegments?.Any(a => segmentIneligibleErrorCodes.Contains(a.ReasonCode)) ?? false;
                flightSegment.IsEligibleForPreOrderMeals = !hasSegmentIneligible;
                if (mealServiceResponse.ErrorResponse?.Error?.Count > 0)
                {
                    flightSegment.IsEligibleForPreOrderMeals = false;
                    flightSegment.ErrorMessage = _configuration.GetValue<string>("PreOrderMealInEligibleErrorMessage");
                    flightSegment.ErrorCode = MEAL_INELIGIBLE_ERROR_CODE;
                    _logger.LogError("UpdatePreOderAndMealEligibleDataFromMealServiceResponseV2 mealServiceResponse description:{desc}", mealServiceResponse.ErrorResponse?.Error?.FirstOrDefault()?.Description);

                }
                else if (inEligibleSegments?.Count > 0 && !flightSegment.IsEligibleForPreOrderMeals)
                {
                    flightSegment.ErrorMessage =
                        _configuration.GetValue<string>("PreOrderMealInEligibleErrorMessage");
                    flightSegment.ErrorCode = inEligibleSegments
                        .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ReasonCode))?.ReasonCode;
                }

                var passengerSharePositions = flightSegment.Passengers.Select(p => p.SharesPosition).ToList();
                var preSelectedMeals =
                    mealServiceResponse.PreSelectedMeals.Where(pm =>
                        pm.SegmentNumber == flightSegment.SegmentNumber &&
                        passengerSharePositions.Contains(pm.PassengerSharePosition)).OrderBy(c => c.SequenceNumber).ToList();
                var nonMealOptions = mealServiceResponse.AvailableMeals.Where(m => string.IsNullOrWhiteSpace(m.MealType)).ToList();

                var mealsByMealType = mealServiceResponse.AvailableMeals
                     .Where(m => m.SegmentNumbers.Contains(flightSegment.SegmentNumber) && !string.IsNullOrWhiteSpace(m.MealType))
                            .OrderBy(c => c.SequenceNumber).GroupBy(m => m.MealType,
                                   (key, meals) => new { MealType = key, Meals = meals.ToList() }).ToList();
                foreach (var item in mealsByMealType)
                {
                    item.Meals.AddRange(nonMealOptions);
                }

                //Add all availble meals in each passenger as meal list with meal type.
                foreach (var flightSegmentPassenger in flightSegment.Passengers)
                {
                    var currentPassengerSelectedMeals = preSelectedMeals.Where(p =>
                        p.PassengerSharePosition == flightSegmentPassenger.SharesPosition);

                    foreach (var mealByMealType in mealsByMealType)
                    {
                        var currentPassengerSelectedMeal = currentPassengerSelectedMeals.FirstOrDefault(p =>
                        p.MealType == mealByMealType.MealType);

                        if (currentPassengerSelectedMeal != null)
                        {
                            flightSegmentPassenger.Meals.Add(new PassengerMeal
                            {
                                MealType = currentPassengerSelectedMeal.MealType,
                                IsMealSelected = currentPassengerSelectedMeal != null,
                                MealCode = currentPassengerSelectedMeal?.MealCode,
                                MealName = currentPassengerSelectedMeal?.MealName,
                                OrderId = currentPassengerSelectedMeal.OrderId,
                                SequenceNumber = currentPassengerSelectedMeal.SequenceNumber,
                                MealSourceType = currentPassengerSelectedMeal.MealSourceType
                            });
                        }
                        else
                        {
                            flightSegmentPassenger.Meals.Add(new PassengerMeal
                            {
                                MealType = mealByMealType.MealType,
                                IsMealSelected = currentPassengerSelectedMeal != null,
                            });
                        }
                    }

                    //Check any passenger on the flight segment already selected meal.
                    if (preSelectedMeals.Count > 0 && !flightSegment.IsEligibleForPreOrderMeals)
                    {
                        //Set static meal name "No meal selected" when few passengers selected meal and few not.
                        flightSegmentPassenger.Meals.Where(m => !m.IsMealSelected)
                            //&& otherPassengerSelectedMeals.Any(p => p.MealType.Equals(m.MealType)))
                            .ForEach(p =>
                            {
                                p.IsMealSelected = true;
                                p.MealName =
                                    _configuration.GetValue<string>("PreOrderMealInEligiblePartialPassengerSelected");
                                p.MealDescription = string.Empty;
                            });
                    }
                }

                //IsPartiallyMealSelected should be true when one or more passenger selected meal and segment outside the meal selection window.
                flightSegment.IsPartiallyMealSelected =
                    preSelectedMeals.Count > 0 && !flightSegment.IsEligibleForPreOrderMeals;

                var departureDateUtc =
                    Convert.ToDateTime(flightSegment.Departure?.TransitTime).ToUniversalTime();
                if (DateTime.UtcNow > departureDateUtc && DateTime.UtcNow <= departureDateUtc.AddHours(24) &&
                    (!hasSegmentIneligible || flightSegment.SegmentNumber == 1))
                //TODO:Pon Need to this hardcode condition untill upstream system send proper error code.
                //((!hasSegmentIneligible || flightSegment.SegmentNumber == 1))
                {
                    flightSegment.FlyThankingMessage =
                        _configuration.GetValue<string>("PreOrderMealFlyThankingMessage");
                }
            }
        }

        private string GenerateFlightSegmentHeaderV2(MOBAirport origin, MOBAirport destination)
        {
            string departureAirport = string.Format("{0} ({1})", origin.City, origin.Code);
            string arrivalAirport = string.Format("{0} ({1})", destination.City, destination.Code);
            return string.Format("{0} to {1}", departureAirport, arrivalAirport);
        }

        public async Task<MealsDetailResponse> GetAvailableMealsV2(PreOrderMealListRequest request, Session appSession)
        {
            request.LanguageCode = string.IsNullOrWhiteSpace(request.LanguageCode) ? "en-US" : request.LanguageCode;
            var mealDetailResponse = new MealsDetailResponse();
            var mobilePnrRequest = new MOBPNRByRecordLocatorRequest
            {
                RecordLocator = request.ConfirmationNumber,
                LastName = request.LastName,
                SessionId = request.SessionId,
                Application = request.Application,
                DeviceId = request.DeviceId,
            };
            //Decrypt PNR using CSL Partner Single SignOn API
            await DecryptConfirmationNumberAndLastName(mobilePnrRequest, appSession.Token);
            var pnrCslResponse = new PreOrderMealPnrResponse();
            var pnrResponseFromSession = await _sessionHelperService.GetSession<ReservationDetail>(request.SessionId, typeof(ReservationDetail).FullName, new List<string> { request.SessionId, typeof(ReservationDetail).FullName }).ConfigureAwait(false);
            if (pnrResponseFromSession != null && pnrResponseFromSession.Detail != null &&
                pnrResponseFromSession.Detail.ConfirmationID.Equals(request.ConfirmationNumber,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                pnrCslResponse.FlightSegmentSpecialMeals = new List<FlightSegmentSpecialMeal>();
            }

            //Call Light weight PNR to get the segments
            var lightWeightPnrResponse =
                await GetPNRFlightSegmentsFromCSL(mobilePnrRequest, appSession, true);

            var selectedPnrSegment = lightWeightPnrResponse.PNR?.Segments
              ?.FirstOrDefault(s => s.SegmentNumber == request.SegmentNumber);
            //Get the available meal offers from CSL for the selected segment
            var mealServiceResponse = await GetMealServiceOfferDetailsV2(lightWeightPnrResponse, mobilePnrRequest,
                  appSession.Token, "GetPreOrderMealsDetails", selectedPnrSegment);
            var mealAvailabilityErrorMessage = _configuration.GetValue<String>("PreOrderMealAvailabilityErrorMessage");
            if (mealServiceResponse != null)
            {
                if (mealServiceResponse.ErrorResponse?.Error?.Count > 0)
                {
                    var errorResponse = mealServiceResponse.ErrorResponse.Error.FirstOrDefault();
                    throw new MOBUnitedException(errorResponse?.Code ?? MEAL_ERROR_CODE, errorResponse?.Description);
                }

                if (mealServiceResponse.AvailableMeals?.Count > 0)
                {
                    mealDetailResponse.AvailableMeals = mealServiceResponse.AvailableMeals
                        .Where(m => m.SegmentNumbers.Contains(request.SegmentNumber) &&
                                    m.MealType.Equals(request.MealType, StringComparison.InvariantCultureIgnoreCase))
                        .Select(m => new AvailableMeal
                        {
                            SequenceNumber = m.SequenceNumber,
                            MealName = m.MealName,
                            MealCode = m.MealCode,
                            MealDescription = m.MealDescription,
                            MealCount = m.MealCount,
                            MealServiceCode = m.MealServiceCode,
                            IsMealAvailable = m.IsMealAvailable,
                            OfferType = m.OfferType,
                            OfferQuantity = m.OfferQuantity,
                            ImageUrl = m.ImageUrl,
                            MealSourceType = m.MealSourceType,
                            MealType = m.MealType,
                            ErrorMessage = !m.IsMealAvailable ? mealAvailabilityErrorMessage : string.Empty,
                            ErrorCode = m.MealCount <= 0 ? MEAL_ERROR_CODE : string.Empty,
                        }).OrderBy(m => m.MealName).ThenBy(m => m.MealDescription).ToList();
                    mealDetailResponse.OtherMealOptions = mealServiceResponse.AvailableMeals
                        .Where(m => m.SegmentNumbers.Contains(request.SegmentNumber) &&
                                    string.IsNullOrWhiteSpace(m.MealType))
                         .Select(m => new BaseMeal
                         {
                             MealName = m.MealName,
                             MealCode = m.MealCode,
                             MealDescription = _configuration.GetValue<String>(m.MealCode),
                             MealServiceCode = m.MealServiceCode,
                             MealSourceType = m.MealSourceType
                         }).OrderBy(m => m.MealCode).ToList();
                    //Not showing Polaris option for mealtype which don't have sequence number equal to one
                    var mealTypeSequenceNumber = mealDetailResponse.AvailableMeals.FirstOrDefault(m => m.MealType.
                    Equals(request.MealType, StringComparison.InvariantCultureIgnoreCase))?.SequenceNumber.ToString();
                    mealDetailResponse.OtherMealOptions.RemoveAll(a => a.MealCode.Equals(POLARISE_CODE) &&
                                    !string.IsNullOrWhiteSpace(mealTypeSequenceNumber) && !(mealTypeSequenceNumber.Equals(SEQUENCENUMBER_ONE)));
                }

                //Get Special meals from PNR Management Retrival API response.
                if (lightWeightPnrResponse?.FlightSegmentSpecialMeals?.Count > 0 &&
                    lightWeightPnrResponse.FlightSegmentSpecialMeals.Any(
                        sm => sm.SegmentNumber == request.SegmentNumber))
                {
                    var specialMeal = new AvailableMeal
                    {
                        MealCode = SPECIAL_MEAL_CODE,
                        MealName = SPECIAL_MEAL_NAME,
                        MealDescription = _configuration.GetValue<String>("PreOrderMealSpecialMealDescription"),
                        MealSourceType = Convert.ToInt32(MealSourceType.Special),
                        IsMealAvailable = true,
                        SpecialMealTitle = SPECIAL_MEAL_TITLE,
                        SpecialMeals = lightWeightPnrResponse.FlightSegmentSpecialMeals
                            .FirstOrDefault(s => s.SegmentNumber == request.SegmentNumber)?.SpecialMeals.Select(s =>
                                new BaseMeal
                                {
                                    OfferType = DYNAMIC_OFFER,
                                    MealCode = s.Code,
                                    MealSourceType = Convert.ToInt32(MealSourceType.Special),
                                    MealName = s.DisplayDescription,
                                    MealDescription = s.DisplayDescription,
                                    IsMealAvailable = true
                                }).ToList()
                    };
                    mealDetailResponse.AvailableMeals.Add(specialMeal);
                }
                if (mealDetailResponse.AvailableMeals.Any(a => a.MealCode == SPECIAL_MEAL_CODE))
                {
                    mealDetailResponse.LearnMoreAboutSpeciaMeal = _configuration.GetValue<String>("LearnMoreAboutSpecialMeals");
                    mealDetailResponse.SpecialMealNote = _configuration.GetValue<String>("SpecialMealNote");
                }
                if (mealDetailResponse.OtherMealOptions.Any(a => a.MealCode == POLARISE_CODE))
                {
                    mealDetailResponse.LearnAboutPolarisLounge = _configuration.GetValue<String>("LearnMoreAboutUnitedPolarisLounge");
                }
            }

            if (mealDetailResponse.AvailableMeals.Count == 0)
            {
                throw new MOBUnitedException(MEAL_ERROR_CODE, mealAvailabilityErrorMessage);
            }

            return mealDetailResponse;
        }

        private async Task<MealOfferResponse> GetMealServiceOfferDetailsV2(PreOrderMealPnrResponse mobilePnrResponse,
        MOBRequest mobRequest, string token, string actionName, MOBPNRSegment selectedSegment = null)
        {
            var clsMealOfferRequest = new DynamicOfferDetailRequest
            {
                Requester = new ServiceClient
                {
                    Requestor = new Requestor
                    {
                        Characteristic = new Collection<Characteristic>
                        {
                            new Characteristic
                            {
                                Code = REQUESTTYPE_CODE,
                                Value = REQUESTTYPE_VALUE
                            }
                        },
                        ChannelID = _configuration.GetValue<String>("MealServiceChannelId"),
                        LanguageCode = "en-US",
                        ChannelName = _configuration.GetValue<String>("MealServiceChannelName"),
                        CountryCode = "US",
                        CurrencyCode = "USD"
                    }
                },
                Filters = new Collection<ProductFilter>() { new ProductFilter { ProductCode = MEAL_PRODUCT_CODE, IsIncluded = true.ToString() } },
                ODOptions = new Collection<ProductOriginDestinationOption>(),
                Travelers = new Collection<ProductTraveler>()
            };

            if (mobilePnrResponse?.PNR != null)
            {
                clsMealOfferRequest.ReservationReferences = new Collection<ReservationReference>()
                {
                    new ReservationReference
                    {
                        ID = mobilePnrResponse.PNR.RecordLocator,
                        ReservationCreateDate = mobilePnrResponse.PNR.DateCreated
                    }
                };
                if (mobilePnrResponse.PNR.Passengers?.Count > 0)
                {
                    var passengerCounter = 1;
                    foreach (var passenger in mobilePnrResponse.PNR.Passengers)
                    {
                        var currentTraveler = new ProductTraveler
                        {
                            ID = passengerCounter.ToString(),
                            TravelerNameIndex = passenger.SHARESPosition,
                            GivenName = passenger.SharesGivenName,
                            PassengerTypeCode = passenger.TravelerTypeCode,
                            Surname = passenger.PassengerName?.Last,
                            ProductLoyaltyProgramProfile = new Collection<ProductTravelerLoyaltyProfile>(),
                        };

                        if (passenger.LoyaltyProgramProfile != null)
                        {
                            var travelerLoyaltyProfile = new ProductTravelerLoyaltyProfile
                            {
                                LoyaltyProgramMemberID =
                                    passenger.LoyaltyProgramProfile.LoyaltyProgramMemberID ?? string.Empty
                            };
                            currentTraveler.ProductLoyaltyProgramProfile.Add(travelerLoyaltyProfile);
                        }

                        clsMealOfferRequest.Travelers.Add(currentTraveler);

                        passengerCounter++;
                    }
                }

                if (mobilePnrResponse.PNR.Segments?.Count > 0)
                {
                    if (selectedSegment != null)
                    {
                        mobilePnrResponse.PNR.Segments = mobilePnrResponse.PNR.Segments
                            .Where(s => s.SegmentNumber == selectedSegment.SegmentNumber).ToList();
                    }

                    var segmentCounter = 1;
                    clsMealOfferRequest.ODOptions.Add(new ProductOriginDestinationOption
                    { ID = "OD1", FlightSegments = new Collection<ProductFlightSegment>() });
                    foreach (var segment in mobilePnrResponse.PNR.Segments)
                    {
                        clsMealOfferRequest.ODOptions[0]?.FlightSegments.Add(new ProductFlightSegment
                        {
                            ID = segmentCounter.ToString(),
                            FlightNumber = segment.FlightNumber,
                            SegmentNumber = segment.SegmentNumber,
                            ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            { IATACode = segment.Arrival?.Code },
                            DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            { IATACode = segment.Departure?.Code },
                            ArrivalDateTime = Convert.ToDateTime(segment.ScheduledArrivalDateTime).ToString("M/dd/yyyy h:mm:ss tt"),
                            DepartureDateTime = Convert.ToDateTime(segment.ScheduledDepartureDateTime).ToString("M/dd/yyyy h:mm:ss tt"),
                            ClassOfService = segment.ClassOfService,
                            OperatingAirlineCode = segment.OperationoperatingCarrier?.Code,
                            Equipment = new Aircraft { Model = new AircraftModel { Fleet = segment.Aircraft.Code, Key = segment.Aircraft?.ModelCode } },
                            BookingClasses = new Collection<BookingClass>()
                                {new BookingClass {Code = segment.ClassOfService}}
                        });
                        segmentCounter++;
                    }
                }
            }

            var confirmationNumber = clsMealOfferRequest.ReservationReferences?[0]?.ID ?? string.Empty;

            var mealOfferResponse = new MealOfferResponse();
            //Get meal offer details from Meal services as json string
            var responseJsonString =
           await GetMealOfferDetailsFromCsl(clsMealOfferRequest, mobRequest, token, actionName);
            //Deserialize the meal service json response to presentation models
            var cslMealOfferResponse =
               JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(responseJsonString);

            if (cslMealOfferResponse != null)
            {
                if (cslMealOfferResponse.ODOptions == null || cslMealOfferResponse.Offers == null ||
                    cslMealOfferResponse.Travelers == null)
                {
                    _logger.LogError("GetMealServiceOfferDetailsV2 There is no meal offer data return from meal orchestration service transactionId:{id}", mobRequest.TransactionId);

                    throw new MOBUnitedException("10000",
                        _configuration.GetValue<String>("PreOrderMealTripUnhandledErrorMessage"));
                }

                var odOptionData = cslMealOfferResponse.ODOptions;
                var offersData = cslMealOfferResponse.Offers;
                var odOptions = odOptionData?.SelectMany(o => o.FlightSegments).ToList();
                var travelerData = cslMealOfferResponse.Travelers;
                var availableProducts =
                    offersData?[0]?.ProductInformation?.ProductDetails?.Where(a => a.Product != null).ToList();
                mealOfferResponse.PreSelectedMeals = availableProducts?.Where(a =>
                        a.Product.Code.Equals(MEAL_PAST_SELECTED_CODE, StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(a => a.Product.SubProducts).Select(a => new PreSelectedMeal
                    {
                        ImageUrl = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(IMAGE_URL, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty,
                        OrderId = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(ORDER_ID, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty,
                        MealCode = a?.Code,
                        MealName = a?.Name,
                        MealDescription = a?.Descriptions?.First(),
                        SegmentNumber =
                            odOptions?.FirstOrDefault(s => s.ID == a.Association?.SegmentRefIDs.FirstOrDefault())
                                ?.SegmentNumber ?? 0,
                        PassengerSharePosition = travelerData?.FirstOrDefault(t =>
                            t.ID.Equals(a.Association?.TravelerRefIDs.FirstOrDefault(),
                                StringComparison.InvariantCultureIgnoreCase))?.TravelerNameIndex,
                        SequenceNumber = Convert.ToInt32(a.Extension?.AdditionalExtensions
                             ?.SelectMany(e => e.Characteristics)
                             .FirstOrDefault(c =>
                                 c.Code.Equals(SEQUENCE_NUMBER,
                                 StringComparison.InvariantCultureIgnoreCase))?.Value ?? "0"),
                        MealType = a?.GroupCode,
                        MealSourceType = !string.IsNullOrWhiteSpace(a?.GroupCode) ? a.GroupCode.Equals(NON_MEAL_GROUP_CODE, StringComparison.InvariantCultureIgnoreCase)
                                                      ? (int)MealSourceType.NonMeal : (int)MealSourceType.Regular : 0
                    }).ToList();

                var allFoodOffers = availableProducts?.Where(a => a.Product.Code.Equals(MEAL_PRODUCT_CODE))
                    .SelectMany(a => a.Product.SubProducts).ToList();
                mealOfferResponse.InEligibleSegments = allFoodOffers?.Where(a => a.InEligibleReason != null)
                    .Select(a => new InEligibleSegment
                    {
                        SegmentNumber =
                            odOptions.FirstOrDefault(s =>
                                a.Association != null && a.Association.SegmentRefIDs.Contains(s.ID))?.SegmentNumber ??
                            0,
                        ReasonCode = a.InEligibleReason?.MajorCode,
                        ReasonDescription = a.InEligibleReason?.Description
                    }).ToList();
                mealOfferResponse.AvailableMeals = allFoodOffers?.Where(a => !string.IsNullOrWhiteSpace(a.Code))
                    .Select(a => new AvailableMeal
                    {
                        ImageUrl = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(IMAGE_URL, StringComparison.InvariantCultureIgnoreCase))?.Value ?? string.Empty,
                        MealCode = a?.Code,
                        MealName = a?.Name,
                        MealDescription = a?.Descriptions?.First(),
                        SegmentNumbers = odOptions?
                            .Where(s => a.Association != null && a.Association.SegmentRefIDs.Contains(s.ID))
                            .Select(s => s.SegmentNumber).ToList(),
                        MealCount = Convert.ToInt32(a.Extension?.AdditionalExtensions
                                                        ?.SelectMany(e => e.Characteristics)
                                                        .FirstOrDefault(c =>
                                                            c.Code.Equals(AVAILABLEMEAL_QUANTITY_CODE,
                                                                StringComparison.InvariantCultureIgnoreCase))?.Value ??
                                                    "0"),
                        MealServiceCode = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(MEAL_SERVICE_CODE, StringComparison.InvariantCultureIgnoreCase))?.Value,
                        IsMealAvailable = a.GroupCode.Equals(NON_MEAL_GROUP_CODE, StringComparison.InvariantCultureIgnoreCase) ? false :
                                              a.InEligibleReason == null && (a.Extension.AdditionalExtensions?.SelectMany(e => e.Characteristics)
                                                .FirstOrDefault(c => c.Code.Equals(OFFER_TYPE, StringComparison.InvariantCultureIgnoreCase))
                                                         ?.Value ?? STATIC_OFFER).Equals(DYNAMIC_OFFER, StringComparison.InvariantCultureIgnoreCase) || Convert.ToInt32
                                                            (a.Extension?.AdditionalExtensions
                                                                 ?.SelectMany(e => e.Characteristics)
                                                                     .FirstOrDefault(c =>
                                                                        c.Code.Equals(AVAILABLEMEAL_QUANTITY_CODE,
                                                                               StringComparison.InvariantCultureIgnoreCase))?.Value ?? "0") > 0,
                        OfferType = a.Extension?.AdditionalExtensions
                                             ?.SelectMany(e => e.Characteristics)
                                                 .FirstOrDefault(c =>
                                                     c.Code.Equals(OFFER_TYPE, StringComparison.InvariantCultureIgnoreCase))?.Value,
                        OfferQuantity = Convert.ToInt32(a.Extension?.AdditionalExtensions
                                                        ?.SelectMany(e => e.Characteristics)
                                                        .FirstOrDefault(c =>
                                                            c.Code.Equals(OFFER_QUANTITY,
                                                                StringComparison.InvariantCultureIgnoreCase))?.Value ??
                                                    "0"
                        ),
                        SequenceNumber = Convert.ToInt32(a.Extension?.AdditionalExtensions
                             ?.SelectMany(e => e.Characteristics)
                             .FirstOrDefault(c =>
                                 c.Code.Equals(SEQUENCE_NUMBER,
                                 StringComparison.InvariantCultureIgnoreCase))?.Value ?? "0"),
                        MealType = a.GroupCode.Equals(NON_MEAL_GROUP_CODE) ? string.Empty : a.GroupCode,
                        MealSourceType = a.GroupCode.Equals(NON_MEAL_GROUP_CODE, StringComparison.InvariantCultureIgnoreCase) ? (int)MealSourceType.NonMeal :
                                        (int)MealSourceType.Regular
                    }).ToList();


                if (mealOfferResponse.PreSelectedMeals.Count > 0)
                {
                    var preSelectedSpecialMeals = new List<PreSelectedMeal>();
                    var allSpecialMealCodes = mobilePnrResponse.AllSpecialMeals.Where(s => s.Type != null).Select(s => s.Type?.Key).ToList();
                    var selectedSpecialMeals = mealOfferResponse.PreSelectedMeals.Where(s => allSpecialMealCodes.Contains(s.MealCode)).ToList();
                    foreach (var preSelectedSpecialMeal in selectedSpecialMeals)
                    {
                        var availableMealTypes = mealOfferResponse.AvailableMeals
                                .Where(m => m.SegmentNumbers.Contains(preSelectedSpecialMeal.SegmentNumber) && !string.IsNullOrWhiteSpace(m.MealType))
                                     .GroupBy(m => m.MealType, (key, meals) => new { MealType = key, Meals = meals.ToList() }).ToList();
                        var specialMealName = mobilePnrResponse.AllSpecialMeals.FirstOrDefault(s => s.Type?.Key == preSelectedSpecialMeal.MealCode).Description;
                        foreach (var availableMealType in availableMealTypes)
                        {
                            preSelectedSpecialMeals.Add(new PreSelectedMeal
                            {
                                OrderId = preSelectedSpecialMeal?.OrderId,
                                MealType = availableMealType?.MealType,
                                SegmentNumber = preSelectedSpecialMeal.SegmentNumber,
                                MealSourceType = (int)MealSourceType.Special,
                                MealCode = preSelectedSpecialMeal?.MealCode,
                                MealName = specialMealName,
                                MealDescription = preSelectedSpecialMeal?.MealDescription,
                                IsMealAvailable = true,
                                PassengerSharePosition = preSelectedSpecialMeal?.PassengerSharePosition,
                                MealServiceCode = preSelectedSpecialMeal?.MealServiceCode,
                                SequenceNumber = availableMealType.Meals[0].SequenceNumber
                            });
                        }

                    }
                    mealOfferResponse.PreSelectedMeals.AddRange(preSelectedSpecialMeals);
                }

                mealOfferResponse.ErrorResponse = cslMealOfferResponse.Response;
            }
            else
            {
                _logger.LogError("GetMealServiceOfferDetailsV2 There is no meal offer data return from meal orchestration service transactionId:{id}", mobRequest.TransactionId);
                throw new MOBUnitedException("10000",
                    _configuration.GetValue<String>("PreOrderMealTripUnhandledErrorMessage"));
            }

            return mealOfferResponse;
        }

        public async Task<PreOrderMealResponseContext> GetPreOrderMealsTripDetails(MOBPNRByRecordLocatorRequest mobilePnrRequest,
            Session session)
        {
            var pnrResponseContext = new PreOrderMealResponseContext();
            mobilePnrRequest.LanguageCode = string.IsNullOrWhiteSpace(mobilePnrRequest.LanguageCode)
                ? "en-US"
                : mobilePnrRequest.LanguageCode;
            try
            {
                //Decrypt PNR using CSL Partner Single SignOn API
                await DecryptConfirmationNumberAndLastName(mobilePnrRequest, session.Token);

                //Get PNR information from PNR details CLS 
                var mobilePnrResponse = await GetPNRFlightSegmentsFromCSL(mobilePnrRequest, session);

                if (mobilePnrResponse != null)
                {
                    // Return the exception message, from PNR API call
                    if (mobilePnrResponse.Exception != null)
                    {
                        throw new MOBUnitedException(mobilePnrResponse.Exception.Code,
                            mobilePnrResponse.Exception.Message);
                    }

                    if (mobilePnrResponse.PNR?.Segments != null)
                    {
                        //Get only active segments which is under 24 hour window time.
                        mobilePnrResponse.PNR.Segments = mobilePnrResponse.PNR.Segments
                            .Where(s => !IsSegmentMoreThan24Hours(s.ScheduledDepartureDateTime))
                            .OrderBy(s => Convert.ToDateTime(s.ScheduledDepartureDateTime)).ToList();
                        if (mobilePnrResponse.PNR.Segments.Count == 0)
                        {
                            throw new MOBUnitedException(
                                 _configuration.GetValue<string>("NoSegmentsFoundErrorMessage"));
                        }

                        //Get Meal Service offer response from CSL
                        var mealServiceResponse = await GetMealServiceOfferDetails(mobilePnrResponse, mobilePnrRequest,
                            session.Token, "GetPreOrderMealsDetails", null);
                        //todo
                        //pnrResponseContext = BuildPreOrderMealTripResponse(mobilePnrRequest, mobilePnrResponse,
                        // mealServiceResponse);
                        pnrResponseContext.ConfirmationNumber = mobilePnrResponse.PNR.RecordLocator;
                        pnrResponseContext.LastName = mobilePnrRequest.LastName;
                    }

                    pnrResponseContext.SessionId = session.SessionId;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPreOrderMealsTripDetails Exception:{ex}", ex.Message);

                throw;
            }

            return pnrResponseContext;
        }

        private PreOrderMealResponseContext BuildPreOrderMealTripResponse(MOBPNRByRecordLocatorRequest mobilePnrRequest,
            MOBPNRByRecordLocatorResponse mobilePnrResponse, MealOfferResponse mealServiceResponse)
        {
            var pnrResponseContext = new PreOrderMealResponseContext();
            //Iterate each flight segment response from PNR and map then to pre order meal response.
            foreach (var flightSegment in mobilePnrResponse.PNR.Segments)
            {
                if (flightSegment != null)
                {
                    var passengers = new List<Passenger>();
                    if (mobilePnrResponse.PNR.Passengers != null)
                    {
                        var passengerCounter = 1;
                        foreach (var passenger in mobilePnrResponse.PNR.Passengers.OrderBy(p =>
                            Convert.ToDecimal(p.SHARESPosition)))
                        {
                            var firstName = string.Empty;
                            var middleName = string.Empty;
                            var lastName = string.Empty;
                            var fullName = string.Empty;
                            if (passenger.PassengerName != null)
                            {
                                firstName = passenger.PassengerName.First ?? string.Empty;
                                middleName = passenger.PassengerName.Middle ?? string.Empty;
                                lastName = passenger.PassengerName.Last ?? string.Empty;
                                fullName = Regex.Replace(
                                    string.Format("{0} {1} {2}", firstName.ToLower(),
                                        middleName.ToLower(), lastName.ToLower()),
                                    @"\s+", " ");
                            }

                            passengers.Add(new Passenger
                            {
                                SharesPosition = passenger.SHARESPosition,
                                PassengerId = passenger.PNRCustomerID,
                                FirstName = firstName,
                                MiddleName = middleName,
                                LastName = lastName,
                                PassengerName = ConvertToTitleCase(fullName),
                                PassengerTypeCode = passenger.TravelerTypeCode,
                                GivenName = firstName
                            });
                            passengerCounter++;
                        }

                    }

                    var flightDeparture = default(FlightTransit);
                    var flightArrival = default(FlightTransit);
                    if (flightSegment.Departure != null)
                    {
                        flightDeparture = new FlightTransit
                        {
                            Code = flightSegment.Departure.Code,
                            Name = flightSegment.Departure.Name,
                            TransitTime = flightSegment.ScheduledDepartureDateTime,
                            City = flightSegment.Departure.City
                        };
                    }

                    if (flightSegment.Arrival != null)
                    {
                        flightArrival = new FlightTransit
                        {
                            Code = flightSegment.Arrival.Code,
                            Name = flightSegment.Arrival.Name,
                            City = flightSegment.Arrival.City,
                            TransitTime = flightSegment.ScheduledArrivalDateTime
                        };
                    }

                    var mobFlightSegment = new FlightSegment
                    {
                        FlightNumber = flightSegment.FlightNumber,
                        TripNumber = flightSegment.TripNumber,
                        SegmentNumber = flightSegment.SegmentNumber,
                        MealType = flightSegment.Meal,
                        Header = GenerateFlightSegmentHeader(flightSegment.ScheduledDepartureDateTime,
                            flightSegment.Departure, flightSegment.Arrival),
                        Passengers = passengers,
                        Departure = flightDeparture,
                        Arrival = flightArrival,
                        AircraftModelCode = flightSegment.Aircraft?.Code,
                        OperatingAirlineCode = flightSegment.OperationoperatingCarrier?.Code
                    };
                    //Update the flight segment meal eligibility and pre selected meal if already selected.
                    UpdatePreOderAndMealEligibleDataFromMealServiceResponse(mobFlightSegment,
                        mealServiceResponse, mobilePnrRequest.TransactionId,
                        mobilePnrRequest.RecordLocator);
                    pnrResponseContext.FlightSegments.Add(mobFlightSegment);
                }
            }

            pnrResponseContext.NumberOfPassengers =
                Convert.ToString(mobilePnrResponse.PNR?.Passengers?.Count);
            pnrResponseContext.FooterDescription =
                _configuration.GetValue<string>("PreOrderMealFooterDescription");
            pnrResponseContext.IsAnySegmentEligibleForPreOrderMeal =
                pnrResponseContext.FlightSegments.Any(a => a.IsEligibleForPreOrderMeals);
            pnrResponseContext.FooterDescriptionHtmlContent =
                _configuration.GetValue<string>("PreOrderMealFooterDescription_Html");
            return pnrResponseContext;
        }

        private string ConvertToTitleCase(string valueToConvert)
        {
            return System.Threading.Thread.CurrentThread.CurrentCulture
                .TextInfo
                .ToTitleCase((!string.IsNullOrEmpty(valueToConvert))
                    ? valueToConvert.ToLower()
                    : string.Empty);
        }

        private void UpdatePreOderAndMealEligibleDataFromMealServiceResponse(FlightSegment flightSegment,
            MealOfferResponse mealServiceResponse, string transactionId, string confirmationNumber)
        {
            if (flightSegment != null && mealServiceResponse != null)
            {
                //Get ineligible segment from meal service API response and update isEligible
                var inEligibleSegments =
                    mealServiceResponse.InEligibleSegments?.Where(a =>
                        a.SegmentNumber == flightSegment.SegmentNumber).ToList();
                var hasSegmentIneligible =
                    inEligibleSegments?.Any(a => segmentIneligibleErrorCodes.Contains(a.ReasonCode)) ?? false;
                flightSegment.IsEligibleForPreOrderMeals = !hasSegmentIneligible;
                if (mealServiceResponse.ErrorResponse?.Error?.Count > 0)
                {
                    flightSegment.IsEligibleForPreOrderMeals = false;
                    flightSegment.ErrorMessage = _configuration.GetValue<string>("PreOrderMealInEligibleErrorMessage");
                    flightSegment.ErrorCode = MEAL_INELIGIBLE_ERROR_CODE;
                    _logger.LogError("UpdatePreOderAndMealEligibleDataFromMealServiceResponse exception description:{desc}", mealServiceResponse.ErrorResponse?.Error?.FirstOrDefault()?.Description);
                }
                else if (inEligibleSegments?.Count > 0 && !flightSegment.IsEligibleForPreOrderMeals)
                {
                    flightSegment.ErrorMessage =
                        _configuration.GetValue<string>("PreOrderMealInEligibleErrorMessage");
                    flightSegment.ErrorCode = inEligibleSegments
                        .FirstOrDefault(s => !string.IsNullOrWhiteSpace(s.ReasonCode))?.ReasonCode;
                }
                //load available meals into each flight segments
                var availableMeals = mealServiceResponse.AvailableMeals?
                    .Where(ma => ma.SegmentNumbers.Contains(flightSegment.SegmentNumber)).Select(ma =>
                        new BaseMeal
                        {
                            MealCode = ma.MealCode,
                            MealName = ma.MealName,
                            MealDescription = ma.MealDescription,
                            MealServiceCode = ma.MealServiceCode,
                            MealCount = ma.MealCount
                        })
                    .ToList();
                flightSegment.AvailableMeals = availableMeals;
                //Check any passenger on the flight segment already selected meal.
                if (mealServiceResponse.PreSelectedMeals?.Count > 0)
                {
                    var passengerSharePositions = flightSegment.Passengers.Select(p => p.SharesPosition).ToList();
                    var preSelectedMeals =
                        mealServiceResponse.PreSelectedMeals.Where(pm =>
                            pm.SegmentNumber == flightSegment.SegmentNumber &&
                            passengerSharePositions.Contains(pm.PassengerSharePosition)).ToList();
                    if (preSelectedMeals.Count > 0)
                    {
                        foreach (var flightSegmentPassenger in flightSegment.Passengers)
                        {
                            var currentPassengerSelectedMeal = preSelectedMeals.FirstOrDefault(p =>
                                p.PassengerSharePosition == flightSegmentPassenger.SharesPosition);
                            //Set selected meal code and description for the passenger
                            if (currentPassengerSelectedMeal != null)
                            {
                                flightSegmentPassenger.MealCode = currentPassengerSelectedMeal.MealCode;
                                flightSegmentPassenger.MealName = currentPassengerSelectedMeal.MealName;
                                flightSegmentPassenger.MealDescription = currentPassengerSelectedMeal.MealDescription;
                                flightSegmentPassenger.MealSourceType = currentPassengerSelectedMeal.MealSourceType == 0
                                    ? (int)MealSourceType.Regular
                                    : currentPassengerSelectedMeal.MealSourceType;
                                flightSegmentPassenger.IsMealSelected = true;
                            }
                            else if (!flightSegment.IsEligibleForPreOrderMeals)
                            {
                                //Set static meal name "No meal selected" when few passengers selected meal and few not.
                                flightSegmentPassenger.IsMealSelected = true;
                                flightSegmentPassenger.MealName =
                                    _configuration.GetValue<string>(
                                        "PreOrderMealInEligiblePartialPassengerSelected");
                                flightSegmentPassenger.MealSourceType = (int)MealSourceType.Regular;
                            }
                        }

                        flightSegment.IsPartiallyMealSelected = !flightSegment.IsEligibleForPreOrderMeals;
                    }
                }
            }
        }

        private string GenerateFlightSegmentHeader(string departureDateTime, MOBAirport origin, MOBAirport destination)
        {
            string departureAirport = string.Format("{0} ({1})", origin.City, origin.Code);
            string arrivalAirport = string.Format("{0} ({1})", destination.City, destination.Code);
            return string.Format("{0} | {1} to {2}", Convert.ToDateTime(departureDateTime).ToString("ddd, MMM d"),
                departureAirport, arrivalAirport);
        }

        public async Task<string> GetMealOfferDetailsFromCsl(DynamicOfferDetailRequest request, MOBRequest mobRequest, string token,
            string actionName)
        {
            var jsonRequest = JsonConvert.SerializeObject(request);
            var url = string.Empty;//_configuration.GetValue<string>("CSLEligibilityCheckServiceURL").ToString();
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _dPService.GetAnonymousToken(mobRequest.Application.Id, mobRequest.DeviceId, _configuration).ConfigureAwait(false);
            }

            string jsonResponse;
            try
            {
                jsonResponse = await _getMealOfferDetailsFromCslService.GetMealOfferDetailsFromCsl(token, jsonRequest, _headers.ContextValues.SessionId, url).ConfigureAwait(false);
            }
            catch (System.Net.WebException webException)
            {
                _logger.LogError("GetMealOfferDetailsFromCsl WebException:{ex}", webException.Message);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMealOfferDetailsFromCsl Exception:{ex}", ex.Message);
                throw;
            }

            return jsonResponse;
        }

        private async Task<MealOfferResponse> GetMealServiceOfferDetails(PreOrderMealPnrResponse mobilePnrResponse,
        MOBRequest mobRequest, string token, string actionName, MOBPNRSegment selectedSegment = null)
        {
            var clsMealOfferRequest = new DynamicOfferDetailRequest
            {
                Requester = new ServiceClient
                {
                    Requestor = new Requestor
                    {
                        ChannelID = _configuration.GetValue<string>("MealServiceChannelId"),
                        LanguageCode = "en-US",
                        ChannelName = _configuration.GetValue<string>("MealServiceChannelName")
                    }
                },
                Filters = new Collection<ProductFilter>() { new ProductFilter { ProductCode = MEAL_PRODUCT_CODE } },
                ODOptions = new Collection<ProductOriginDestinationOption>(),
                Travelers = new Collection<ProductTraveler>()
            };

            if (mobilePnrResponse?.PNR != null)
            {
                clsMealOfferRequest.ReservationReferences = new Collection<ReservationReference>()
                {
                    new ReservationReference
                    {
                        ID = mobilePnrResponse.PNR.RecordLocator,
                        ReservationCreateDate = mobilePnrResponse.PNR.DateCreated
                    }
                };
                if (mobilePnrResponse.PNR.Passengers?.Count > 0)
                {
                    var passengerCounter = 1;
                    foreach (var passenger in mobilePnrResponse.PNR.Passengers)
                    {
                        clsMealOfferRequest.Travelers.Add(new ProductTraveler
                        {
                            ID = passengerCounter.ToString(),
                            TravelerNameIndex = passenger.SHARESPosition,
                            GivenName = passenger.SharesGivenName,
                            PassengerTypeCode = passenger.TravelerTypeCode,
                            Surname = passenger.PassengerName?.Last
                        });
                        passengerCounter++;
                    }
                }

                if (mobilePnrResponse.PNR.Segments?.Count > 0)
                {
                    if (selectedSegment != null)
                    {
                        mobilePnrResponse.PNR.Segments = mobilePnrResponse.PNR.Segments
                            .Where(s => s.SegmentNumber == selectedSegment.SegmentNumber).ToList();
                    }

                    var segmentCounter = 1;
                    clsMealOfferRequest.ODOptions.Add(new ProductOriginDestinationOption
                    { ID = "OD1", FlightSegments = new Collection<ProductFlightSegment>() });
                    foreach (var segment in mobilePnrResponse.PNR.Segments)
                    {
                        clsMealOfferRequest.ODOptions[0].FlightSegments.Add(new ProductFlightSegment
                        {
                            ID = segmentCounter.ToString(),
                            FlightNumber = segment.FlightNumber,
                            SegmentNumber = segment.SegmentNumber,
                            ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            { IATACode = segment.Arrival?.Code },
                            DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            { IATACode = segment.Departure?.Code },
                            ArrivalDateTime = segment.ScheduledArrivalDateTime,
                            DepartureDateTime = segment.ScheduledDepartureDateTime,
                            ClassOfService = segment.ClassOfService,
                            OperatingAirlineCode = segment.OperationoperatingCarrier?.Code,
                            BookingClasses = new Collection<BookingClass>()
                                {new BookingClass {Code = segment.ClassOfService}}

                        });
                        segmentCounter++;
                    }
                }
            }

            var confirmationNumber = clsMealOfferRequest.ReservationReferences?[0]?.ID ?? string.Empty;
            _logger.LogInformation("GetMealServiceOfferDetails TransactionID:{id}, Device:{deviceId}, MealOfferRequest:{req}", mobRequest.TransactionId, mobRequest.DeviceId, clsMealOfferRequest);

            var mealOfferResponse = new MealOfferResponse();
            //Get meal offer details from Meal services as json string
            //todo
            var responseJsonString = await
               GetMealOfferDetailsFromCsl(clsMealOfferRequest, mobRequest, token, actionName);
            //var responseJsonString = "";
            //Deserialize the meal service json response to presentation models
            //var cslMealOfferResponse =
            //    UnitedSerialization.JsonSerializer.Deserialize<DynamicOfferDetailResponse>(responseJsonString);
            var cslMealOfferResponse =
                JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(responseJsonString);

            if (cslMealOfferResponse != null)
            {
                if (cslMealOfferResponse.ODOptions == null || cslMealOfferResponse.Offers == null ||
                    cslMealOfferResponse.Travelers == null)
                {
                    _logger.LogError("GetMealServiceOfferDetails There is no meal offer data return from meal orchestration service TransactionId:{Id}", mobRequest.TransactionId);

                    throw new MOBUnitedException("10000",
                        _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage"));
                }

                var odOptionData = cslMealOfferResponse.ODOptions;
                var offersData = cslMealOfferResponse.Offers;
                var odOptions = odOptionData?.SelectMany(o => o.FlightSegments).ToList();
                var travelerData = cslMealOfferResponse.Travelers;
                var availableProducts =
                    offersData?[0]?.ProductInformation?.ProductDetails?.Where(a => a.Product != null).ToList();
                mealOfferResponse.PreSelectedMeals = availableProducts?.Where(a =>
                        a.Product.Code.Equals(MEAL_PAST_SELECTED_CODE, StringComparison.InvariantCultureIgnoreCase))
                    .SelectMany(a => a.Product.SubProducts).Select(a => new PreSelectedMeal
                    {
                        MealCode = a.Code,
                        MealName = a.Name,
                        MealDescription = a.Descriptions?.First(),
                        SegmentNumber =
                            odOptions?.FirstOrDefault(s => s.ID == a.Association?.SegmentRefIDs.FirstOrDefault())
                                ?.SegmentNumber ?? 0,
                        PassengerSharePosition = travelerData?.FirstOrDefault(t =>
                            t.ID.Equals(a.Association?.TravelerRefIDs.FirstOrDefault(),
                                StringComparison.InvariantCultureIgnoreCase))?.TravelerNameIndex,
                        MealType = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(MEAL_TYPE_CODE, StringComparison.InvariantCultureIgnoreCase))?.Value,
                        MealSourceType = (int)MealSourceType.Regular
                    }).ToList();
                var allFoodOffers = availableProducts?.Where(a => a.Product.Code.Equals(MEAL_PRODUCT_CODE))
                    .SelectMany(a => a.Product.SubProducts).ToList();
                mealOfferResponse.InEligibleSegments = allFoodOffers?.Where(a => a.InEligibleReason != null)
                    .Select(a => new InEligibleSegment
                    {
                        SegmentNumber =
                            odOptions.FirstOrDefault(s =>
                                a.Association != null && a.Association.SegmentRefIDs.Contains(s.ID))?.SegmentNumber ??
                            0,
                        ReasonCode = a.InEligibleReason?.MajorCode,
                        ReasonDescription = a.InEligibleReason?.Description
                    }).ToList();
                mealOfferResponse.AvailableMeals = allFoodOffers?.Where(a => !string.IsNullOrWhiteSpace(a.Code))
                    .Select(a => new AvailableMeal
                    {
                        MealCode = a.Code,
                        MealName = a.Name,
                        MealDescription = a.Descriptions?.First(),
                        SegmentNumbers = odOptions?
                            .Where(s => a.Association != null && a.Association.SegmentRefIDs.Contains(s.ID))
                            .Select(s => s.SegmentNumber).ToList(),
                        MealCount = Convert.ToInt32(a.Extension?.AdditionalExtensions
                                                        ?.SelectMany(e => e.Characteristics)
                                                        .FirstOrDefault(c =>
                                                            c.Code.Equals(MEAL_QUANTITY_CODE,
                                                                StringComparison.InvariantCultureIgnoreCase))?.Value ??
                                                    "0"
                        ),
                        MealServiceCode = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(MEAL_SERVICE_CODE, StringComparison.InvariantCultureIgnoreCase))?.Value,
                        IsMealAvailable = a.InEligibleReason == null && Convert.ToInt32(
                                              a.Extension?.AdditionalExtensions
                                                  ?.SelectMany(e => e.Characteristics)
                                                  .FirstOrDefault(c =>
                                                      c.Code.Equals(MEAL_QUANTITY_CODE,
                                                          StringComparison.InvariantCultureIgnoreCase))?.Value ?? "0") >
                                          0,
                        MealType = a.Extension?.AdditionalExtensions
                            ?.SelectMany(e => e.Characteristics)
                            .FirstOrDefault(c =>
                                c.Code.Equals(MEAL_TYPE_CODE, StringComparison.InvariantCultureIgnoreCase))?.Value,
                        MealSourceType = (int)MealSourceType.Regular
                    }).ToList();



                mealOfferResponse.ErrorResponse = cslMealOfferResponse.Response;
            }
            else
            {
                _logger.LogError("GetMealServiceOfferDetails There is no meal offer data return from meal orchestration service TransactionId:{Id}", mobRequest.TransactionId);

                throw new MOBUnitedException("10000",
                    _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage"));
            }

            return mealOfferResponse;
        }

        private bool IsSegmentMoreThan24Hours(string departureDate)
        {
            var isMoreThan24Hours = default(bool);
            var departureDateTime = Convert.ToDateTime(departureDate).ToUniversalTime().AddHours(24);
            if (DateTime.UtcNow > departureDateTime)
            {
                isMoreThan24Hours = true;
            }

            return isMoreThan24Hours;
        }

        public async Task<PreOrderMealPnrResponse> GetPNRFlightSegmentsFromCSL(MOBPNRByRecordLocatorRequest mobilePnrRequest,
            Session session, bool enableManageRes = false)
        {
            var mobilePnrResponse = new PreOrderMealPnrResponse();
            var cslPnrRequest = new RetrievePNRSummaryRequest
            {
                Channel = _configuration.GetValue<string>("ChannelName"),
                IsIncludeETicketSDS = "False",
                IsIncludeFlightRange = "False",
                IsIncludeFlightStatus = "True",
                IncludeManageResDetails = enableManageRes.ToString(),
                IsIncludeLMX = "False",
                IsIncludePNRDB = "False",
                IsIncludeSegmentDuration = "False",
                ConfirmationID = mobilePnrRequest.RecordLocator.ToUpper(),
                LastName = mobilePnrRequest.LastName,
                PNRType = string.Empty,
                FilterHours = _configuration.GetValue<string>("FilterHours")
            };
            //var flightReservation = new FlightReservation();
            var applicationId = default(int);
            var appVersion = default(string);
            if (mobilePnrRequest.Application != null)
            {
                applicationId = mobilePnrRequest.Application?.Id ?? 0;
                if (mobilePnrRequest.Application.Version != null)
                {
                    appVersion = mobilePnrRequest.Application.Version.Major;
                }
            }

            var token = session.Token;
            var pnrResponseFromCsl = default(ReservationDetail);
            if (!string.IsNullOrWhiteSpace(session?.SessionId))
            {
                //pnrResponseFromCsl = Persist.FilePersist.Load<ReservationDetail>(session.SessionId,
                //    typeof(ReservationDetail).FullName);
                pnrResponseFromCsl = await _sessionHelperService.GetSession<ReservationDetail>(session.SessionId, typeof(ReservationDetail).FullName, new List<string> { session.SessionId, typeof(ReservationDetail).FullName }).ConfigureAwait(false);
                //Check retrived PNR confirmation number from cache and requested PNR confirmation number are same.
                //if not then need to get the requested PNR need to get from CSL service
                if (pnrResponseFromCsl != null && pnrResponseFromCsl.Detail != null &&
                    !pnrResponseFromCsl.Detail.ConfirmationID.Equals(mobilePnrRequest.RecordLocator,
                        StringComparison.InvariantCultureIgnoreCase))
                {
                    pnrResponseFromCsl = null;
                }
            }

            if (pnrResponseFromCsl == null)
            {
                //todo confirm
                //var pnJsonResponseFromCsl = _flightReservation.RetrievePnrDetailsFromCsl(mobilePnrRequest.TransactionId,
                // applicationId, appVersion, "GetPNRFlightSegments", cslPnrRequest, ref token);
                var pnJsonResponseFromCsl = await _flightReservation.RetrievePnrDetailsFromCsl(applicationId, mobilePnrRequest.TransactionId,
                     cslPnrRequest).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(pnJsonResponseFromCsl))
                {
                    //pnrResponseFromCsl =
                    //    Json.Serialization.JsonSerializer.DeserializeUseContract<ReservationDetail>(
                    //        pnJsonResponseFromCsl);
                    pnrResponseFromCsl =
                        JsonConvert.DeserializeObject<ReservationDetail>(
                            pnJsonResponseFromCsl);
                    if (session != null)
                    {
                        //todo
                        await _sessionHelperService.SaveSession<ReservationDetail>(pnrResponseFromCsl, _headers.ContextValues.SessionId, new List<String> { _headers.ContextValues.SessionId, new ReservationDetail().GetType().FullName }, new ReservationDetail().GetType().FullName).ConfigureAwait(false);
                    }
                }
            }

            if (pnrResponseFromCsl != null)
            {
                if (pnrResponseFromCsl.Error == null || pnrResponseFromCsl.Error.Count == 0)
                {
                    if (pnrResponseFromCsl.Detail?.FlightSegments != null)
                    {
                        //CSMC service will return always the inactive or cancelled flight segment for Mobile channel.
                        //Hence remove the inactive or cancelled flight segment which indicate FlightSegmentType as "HX"
                        pnrResponseFromCsl.Detail.FlightSegments = pnrResponseFromCsl.Detail
                            ?.FlightSegments.Where(fs =>
                                string.IsNullOrWhiteSpace(fs.FlightSegment.FlightSegmentType) ||
                                !fs.FlightSegment.FlightSegmentType.Equals("HX",
                                    StringComparison.InvariantCultureIgnoreCase)).ToCollection();
                        mobilePnrResponse.PNR = new MOBPNR { SessionId = session.SessionId };
                        var isActive =
                            _flightReservation.GetCharactersticValue(pnrResponseFromCsl.Detail.Characteristic,
                                "ActiveTicketsExist");
                        mobilePnrResponse.PNR.IsActive =
                            Convert.ToBoolean(string.IsNullOrEmpty(isActive) ? "false" : isActive);
                        mobilePnrResponse.PNR.DateCreated = GeneralHelper.FormatDatetime(
                            Convert.ToDateTime(pnrResponseFromCsl.Detail.CreateDate)
                                .ToString("yyyyMMdd hh:mm tt"), mobilePnrRequest.LanguageCode);
                        var isSpaceAvailablePassRaider = default(bool);
                        var isPositiveSpace = default(bool);
                        mobilePnrResponse.PNR.RecordLocator = pnrResponseFromCsl.Detail.ConfirmationID;
                        mobilePnrResponse.PNR.Segments = new List<MOBPNRSegment>();
                        //Iterate each flight segment and map the CSL PNR Response to mobile PNR response
                        foreach (var flightSegment in pnrResponseFromCsl.Detail.FlightSegments)
                        {
                            //Fill flight segment details
                            mobilePnrResponse.PNR.Segments.Add(await _flightReservation.GetPnrSegment(

                                mobilePnrRequest.LanguageCode, appVersion, applicationId,
                                flightSegment, 0));
                        }

                        //Fill passenger details
                        _flightReservation.GetPassengerDetails(mobilePnrResponse.PNR, pnrResponseFromCsl,
                            ref isSpaceAvailablePassRaider, ref isPositiveSpace);
                        //Throw Exception when there is no segment present
                        if (pnrResponseFromCsl.Detail.FlightSegments.Count == 0)
                        {
                            throw new MOBUnitedException(
                                _configuration.GetValue<string>("NoSegmentsFoundErrorMessage"));
                        }
                    }
                    else
                    {
                        throw new MOBUnitedException(
                             _configuration.GetValue<string>("NoSegmentsFoundErrorMessage"));
                    }
                }
                else
                {
                    throw new MOBUnitedException(pnrResponseFromCsl.Error[0].Code,
                        pnrResponseFromCsl.Error[0].Description);
                }
            }

            //Get availble slecial meals from session if PNR Retrival service not providing the same and
            //update the data retrived from session to PNR flight segment response.
            await UpdateSegmentAvailableSpecialMealFromSession(session, pnrResponseFromCsl);

            //Get all reference data for special meal description.
            var specialMealReferenceData = await _unfinishedBooking.GetSpecialNeedsReferenceDataFromFlightShopping(session,
                mobilePnrRequest.Application?.Id ?? 0, mobilePnrRequest.Application?.Version?.Major,
                mobilePnrRequest.DeviceId, mobilePnrRequest.LanguageCode);
            var allSpecialMeals = specialMealReferenceData?.SpecialMealResponses?[0]?.SpecialMeals;
            mobilePnrResponse.AllSpecialMeals = allSpecialMeals.ToList();
            //Get flight segment eligible special meals along with special meal description.
            mobilePnrResponse.FlightSegmentSpecialMeals = pnrResponseFromCsl.Detail?.FlightSegments.Where(f =>
                f.FlightSegment != null && f.FlightSegment.Characteristic != null &&
                f.FlightSegment.Characteristic.Any(c => !string.IsNullOrWhiteSpace(c.Code) &&
                                                        c.Code.Equals(SPECIAL_MEAL_CODE,
                                                            StringComparison.InvariantCultureIgnoreCase) &&
                                                        !string.IsNullOrWhiteSpace(c.Value))).Select(s =>
                new FlightSegmentSpecialMeal
                {
                    SegmentNumber = s.SegmentNumber,
                    TripNumber = s.TripNumber,
                    FlightNumber = s.FlightSegment.FlightNumber,
                    SpecialMeals = s.FlightSegment.Characteristic
                        .FirstOrDefault(fc => !string.IsNullOrWhiteSpace(fc.Code) &&
                                              fc.Code.Equals(SPECIAL_MEAL_CODE,
                                                  StringComparison.InvariantCultureIgnoreCase))?.Value
                        .Split('|').Select(v => v.Trim()).Select(v => new MOBTravelSpecialNeed
                        {
                            Code = v,
                            DisplayDescription = allSpecialMeals?.FirstOrDefault(sm => sm.Type != null &&
                                                                                       sm.Type.Key.Equals(v,
                                                                                           StringComparison
                                                                                               .InvariantCultureIgnoreCase))
                                ?.Description,
                            Value = allSpecialMeals?.FirstOrDefault(sm => sm.Type != null &&
                                                                          sm.Type.Key.Equals(v,
                                                                              StringComparison
                                                                                  .InvariantCultureIgnoreCase))
                                ?.Value?[0]
                        }).OrderBy(spm => Convert.ToInt32(spm.Value)).ToList()
                }).ToList();

            return mobilePnrResponse;
        }

        private async System.Threading.Tasks.Task UpdateSegmentAvailableSpecialMealFromSession(Session session, ReservationDetail pnrResponseFromCsl)
        {
            if (pnrResponseFromCsl.Detail != null && !pnrResponseFromCsl.Detail.FlightSegments.Any(f =>
                    f.FlightSegment.Characteristic.Any(c => !string.IsNullOrWhiteSpace(c.Code) &&
                                                            c.Code.Equals(SPECIAL_MEAL_CODE,
                                                                StringComparison.InvariantCultureIgnoreCase))))
            {
                var specialMealFromSession = await _sessionHelperService.GetSession<List<ReservationFlightSegment>>(_headers.ContextValues.SessionId, typeof(ReservationFlightSegment).FullName, new List<string> { _headers.ContextValues.SessionId, typeof(ReservationFlightSegment).FullName }).ConfigureAwait(false);
                if (specialMealFromSession != null)
                {
                    var specialMealSegments = specialMealFromSession.Where(s =>
                            s.FlightSegment != null && s.FlightSegment.Characteristic != null &&
                            s.FlightSegment.Characteristic.Any(c => !string.IsNullOrWhiteSpace(c.Code) &&
                                                                    c.Code.Equals(SPECIAL_MEAL_CODE,
                                                                        StringComparison.InvariantCultureIgnoreCase)))
                        .ToList();
                    if (specialMealSegments.Count > 0)
                    {
                        foreach (var flightSegment in pnrResponseFromCsl.Detail?.FlightSegments)
                        {
                            var segmentSpecialMealCharacteristic = specialMealSegments.FirstOrDefault(s =>
                                    s.FlightSegment != null &&
                                    s.FlightSegment.FlightNumber == flightSegment.FlightSegment.FlightNumber &&
                                    s.FlightSegment.DepartureAirport?.IATACode ==
                                    flightSegment.FlightSegment.DepartureAirport.IATACode &&
                                    s.FlightSegment.DepartureDateTime ==
                                    flightSegment.FlightSegment.DepartureDateTime)?.FlightSegment
                                ?.Characteristic.FirstOrDefault(c => !string.IsNullOrWhiteSpace(c.Code) &&
                                                                     c.Code.Equals(SPECIAL_MEAL_CODE,
                                                                         StringComparison.InvariantCultureIgnoreCase));
                            if (segmentSpecialMealCharacteristic != null)
                            {
                                flightSegment.FlightSegment.Characteristic.Add(segmentSpecialMealCharacteristic);
                            }
                        }
                    }
                }
            }
        }

        private async Task<PreOrderMealCartResponse> AddToCartServiceCall(PreOrderMealCartRequest request, bool isVersion2)
        {
            var preOrderMealResponse = new PreOrderMealCartResponse();
            try
            {
                _logger.LogInformation("AddToCartServiceCall request:{request}, sessionId:{id}", JsonConvert.SerializeObject(request), request.SessionId);

                var session = await GetSession(request, request.SessionId);
                if (isVersion2)
                {
                    preOrderMealResponse = await AddToCartV2(request, session);
                }
                else
                {
                    preOrderMealResponse = await AddToCart(request, session);
                }
            }
            catch (MOBUnitedException appException)
            {
                _logger.LogWarning("AddToCartServiceCall MOBUnitedException:{excpetion}", appException.Message);

                preOrderMealResponse.Exception = new MOBException
                {
                    Message = appException.Message
                };
            }
            catch (Exception ex)
            {

                _logger.LogError("AddToCartServiceCall Exception:{excpetion}", ex.Message);

                preOrderMealResponse.Exception = new MOBException("10000",
                    _configuration.GetValue<string>("PreOrderMealAddToCartErrorMessage"));
            }

            preOrderMealResponse.TransactionId = request.TransactionId;
            return preOrderMealResponse;
        }

        public async Task<PreOrderMealCartResponse> AddToCartV2(PreOrderMealCartRequest request, Session appSession)
        {
            request.LanguageCode = string.IsNullOrWhiteSpace(request.LanguageCode) ? "en-US" : request.LanguageCode;
            var preOrderMealsCartResponse = new PreOrderMealCartResponse();
            var mobilePnrRequest = new MOBPNRByRecordLocatorRequest
            {
                RecordLocator = request.ConfirmationNumber,
                LastName = request.LastName,
                SessionId = request.SessionId,
                Application = request.Application,
                DeviceId = request.DeviceId,
            };

            //Decrypt PNR using CSL Partner Single SignOn API and assign decrypted confirmation number and lastname back to request.
            await DecryptConfirmationNumberAndLastName(mobilePnrRequest, appSession.Token);
            request.ConfirmationNumber = mobilePnrRequest.RecordLocator;
            request.LastName = mobilePnrRequest.LastName;
            var addCartRequest = CreateAddCartRequestV2(request);
            _logger.LogInformation("AddToCartV2 request:{req}, sessionId:{id}", JsonConvert.SerializeObject(request), request.SessionId);

            var mealRegisterJsonResponse =
                await PreOrderMealRegister(addCartRequest, request, appSession.Token, true) ??
                string.Empty;
            var registerMealResponse =
            //UnitedSerialization.JsonSerializer.Deserialize<FlightReservationResponse>(mealRegisterJsonResponse);
            JsonConvert.DeserializeObject<FlightReservationResponse>(mealRegisterJsonResponse);

            if (registerMealResponse != null)
            {
                preOrderMealsCartResponse.CartId = registerMealResponse.CartId;
                preOrderMealsCartResponse.ConfirmationNumber = request.ConfirmationNumber;
                preOrderMealsCartResponse.OrderId = registerMealResponse.DisplayCart?.TravelOptions
                    ?.FirstOrDefault()
                    ?.SubItems?.FirstOrDefault()?.Status;
                preOrderMealsCartResponse.IsSuccess = registerMealResponse.Status == United.Services.FlightShopping.Common.StatusType.Success ||
                                                      !string.IsNullOrWhiteSpace(preOrderMealsCartResponse.OrderId);
                preOrderMealsCartResponse.TransactionId = request.TransactionId;
            }

            if (!preOrderMealsCartResponse.IsSuccess)
            {
                var errorCode = string.Empty;
                var errorMessage = string.Empty;
                if (registerMealResponse?.Errors?.Count > 0)
                {
                    errorCode = registerMealResponse.Errors[0]?.MajorCode;
                    errorMessage = registerMealResponse.Errors[0]?.Message;
                    _logger.LogError("AddToCartV2 errorcode:{code}, errorMessage:{msg}", errorCode, errorMessage);
                }

                if (errorCode != MEAL_ADD_TO_CART_DMS_ERROR_CODE)
                {
                    errorCode = MEAL_ADD_TO_CART_ERROR_CODE;
                    errorMessage = _configuration.GetValue<string>("PreOrderMealAddToCartErrorMessage");
                }

                preOrderMealsCartResponse.Exception = new MOBException
                {
                    Code = errorCode,
                    Message = errorMessage
                };
            }

            return preOrderMealsCartResponse;
        }

        private United.Service.Presentation.ReservationModel.Reservation CreateAddCartRequestV2(PreOrderMealCartRequest request)
        {
            var mealSourceType = (MealSourceType)request.MealSourceType;
            var addCartRequest = new United.Service.Presentation.ReservationModel.Reservation
            {
                CartId = Guid.NewGuid().ToString(),
                Association = new Collection<ReservationAssociation>()
                {
                    new ReservationAssociation
                    {
                        Requestor = new Requestor
                        {
                            CountryCode = "US", LanguageCode = request.LanguageCode,
                            ChannelID = _configuration.GetValue<string>("MealServiceChannelId"),
                            ChannelName = _configuration.GetValue<string>("MealServiceChannelName")
                        }
                    }
                },
                Characteristic = new Collection<Characteristic>
                {
                    new Characteristic { Code = MEAL_ADD_TO_CART_CODE, Value = Convert.ToString(false) },
                    new Characteristic { Code = OFFER_TYPE, Value = request.OfferProvisionType }
                },
                ConfirmationID = request.ConfirmationNumber,
                Channel = _configuration.GetValue<string>("ChannelName"),
                MealSegments = new Collection<MealSegment>()
                {
                    new MealSegment
                    {
                        DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            {IATACode = request.Departure},
                        ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            {IATACode = request.Arrival},
                        DepartureDateTime = request.DepartureDate,
                        FlightNumber = request.FlightNumber,
                        Equipment = new United.Service.Presentation.CommonModel.AircraftModel.Aircraft {Model = new AircraftModel {Key = request.AircraftModelCode}},
                        OperatingAirlineCode = request.OperatingAirlineCode,
                        SegmentNumber = request.SegmentNumber,
                        Meals = new Collection<TravelerMeal>
                        {
                            new TravelerMeal
                            {
                                OfferType = request.OfferProvisionType,
                                ServiceSequenceNumber = request.ServiceSequenceNumber,
                                OrderId= request.OrderId,
                                OfferCount = Convert.ToString(request.OfferQuantity),
                                MealCategory = mealSourceType.GetDisplayName(),
                                EntreeIndicator = request.MealServiceCode, MealItemCode = request.MealCode,
                                MealItemDescription = request.MealDescription, TravelerIndex = request.SharesPosition
                            }
                        }
                    }
                },
                Travelers = new Collection<Traveler>()
                {
                    new Traveler
                    {
                        Person = new United.Service.Presentation.PersonModel.Person
                        {
                            GivenName = request.GivenName, Surname = request.LastName, Key = request.SharesPosition,
                            CustomerID = request.PassengerId
                        }
                    }
                }
            };
            return addCartRequest;
        }

        private async Task<PreOrderMealCartResponse> AddToCart(PreOrderMealCartRequest request, Session appSession)
        {
            request.LanguageCode = string.IsNullOrWhiteSpace(request.LanguageCode) ? "en-US" : request.LanguageCode;
            var preOrderMealsCartResponse = new PreOrderMealCartResponse();
            var mobilePnrRequest = new MOBPNRByRecordLocatorRequest
            {
                RecordLocator = request.ConfirmationNumber,
                LastName = request.LastName,
                SessionId = request.SessionId,
                Application = request.Application,
                DeviceId = request.DeviceId,
            };

            //Decrypt PNR using CSL Partner Single SignOn API and assign decrypted confirmation number and lastname back to request.
            await DecryptConfirmationNumberAndLastName(mobilePnrRequest, appSession.Token);
            request.ConfirmationNumber = mobilePnrRequest.RecordLocator;
            request.LastName = mobilePnrRequest.LastName;
            var addCartRequest = CreateAddCartRequest(request);
            _logger.LogInformation("AddToCart request:{req}, sessionId:{id}", JsonConvert.SerializeObject(request), request.SessionId);

            var mealRegisterJsonResponse =
                await PreOrderMealRegister(addCartRequest, request, appSession.Token) ?? string.Empty;
            var registerMealResponse =
            //UnitedSerialization.JsonSerializer.Deserialize<FlightReservationResponse>(mealRegisterJsonResponse);
            JsonConvert.DeserializeObject<FlightReservationResponse>(mealRegisterJsonResponse);

            if (registerMealResponse != null)
            {
                preOrderMealsCartResponse.CartId = registerMealResponse.CartId;
                preOrderMealsCartResponse.ConfirmationNumber = request.ConfirmationNumber;
                preOrderMealsCartResponse.OrderId = registerMealResponse.DisplayCart?.TravelOptions?.FirstOrDefault()
                    ?.SubItems?.FirstOrDefault()?.Status;
                preOrderMealsCartResponse.IsSuccess = registerMealResponse.Status == United.Services.FlightShopping.Common.StatusType.Success &&
                                                      !string.IsNullOrWhiteSpace(preOrderMealsCartResponse.OrderId);
                preOrderMealsCartResponse.TransactionId = request.TransactionId;
            }

            if (!preOrderMealsCartResponse.IsSuccess)
            {
                var errorCode = string.Empty;
                var errorMessage = string.Empty;
                if (registerMealResponse?.Errors?.Count > 0)
                {
                    errorCode = registerMealResponse.Errors[0]?.MajorCode;
                    errorMessage = registerMealResponse.Errors[0]?.Message;
                    _logger.LogError("AddToCart ErrorCode:{code},Error Message:{message}", errorCode, errorMessage);
                }

                if (errorCode != MEAL_ADD_TO_CART_DMS_ERROR_CODE)
                {
                    errorCode = MEAL_ADD_TO_CART_ERROR_CODE;
                    errorMessage = _configuration.GetValue<string>("PreOrderMealAddToCartErrorMessage");
                }

                preOrderMealsCartResponse.Exception = new MOBException
                {
                    Code = errorCode,
                    Message = errorMessage
                };
            }

            return preOrderMealsCartResponse;
        }

        public async Task<string> PreOrderMealRegister(United.Service.Presentation.ReservationModel.Reservation request, MOBRequest mobRequest,
            string token, bool isVersion2 = false)
        {
            var jsonRequest =
               JsonConvert.SerializeObject(request);
            var url = string.Empty;
            if (isVersion2)
            {
                url = _configuration.GetValue<string>("CSLMealRegisterServiceV2URL").ToString();
            }
            else
            {
                url = _configuration.GetValue<string>("CSLMealRegisterServiceURL").ToString();
            }
            if (string.IsNullOrWhiteSpace(token))
            {
                token = await _dPService.GetAnonymousToken(mobRequest.Application.Id, mobRequest.DeviceId,
                        _configuration).ConfigureAwait(false);

            }

            _logger.LogInformation("PreOrderMealRegister request:{req}, TransactionId:{id}", JsonConvert.SerializeObject(mobRequest), mobRequest.TransactionId);

            string jsonResponse;
            try
            {
                //jsonResponse = HttpHelper.Post(url, "application/json; charset=utf-8", token, jsonRequest);
                jsonResponse = await _preOrderMealRegisterService.PreOrderMealRegister(token, jsonRequest, _headers.ContextValues.SessionId, url).ConfigureAwait(false);
            }
            catch (System.Net.WebException webException)
            {
                _logger.LogError("PreOrderMealRegister WebException:{ex}", webException.Message);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("PreOrderMealRegister Exception:{ex}", ex.Message);
                throw;
            }

            return jsonResponse;
        }

        private United.Service.Presentation.ReservationModel.Reservation CreateAddCartRequest(PreOrderMealCartRequest request)
        {
            var addCartRequest = new United.Service.Presentation.ReservationModel.Reservation
            {
                CartId = Guid.NewGuid().ToString(),
                Association = new Collection<ReservationAssociation>()
                {
                    new ReservationAssociation
                    {
                        Requestor = new Requestor
                        {
                            CountryCode = "US", LanguageCode = request.LanguageCode,
                            ChannelID = _configuration.GetValue<string>("MealServiceChannelId"),
                            ChannelName =  _configuration.GetValue<string>("MealServiceChannelName")
                        }
                    }
                },
                Characteristic = new Collection<Characteristic>
                    {new Characteristic {Code = MEAL_ADD_TO_CART_CODE, Value = Convert.ToString(false)}},
                ConfirmationID = request.ConfirmationNumber,
                Channel = _configuration.GetValue<string>("ChannelName"),
                MealSegments = new Collection<MealSegment>()
                {
                    new MealSegment
                    {
                        DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            {IATACode = request.Departure},
                        ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport
                            {IATACode = request.Arrival},
                        DepartureDateTime = request.DepartureDate,
                        FlightNumber = request.FlightNumber,
                        Equipment = new United.Service.Presentation.CommonModel.AircraftModel.Aircraft {Model = new United.Service.Presentation.CommonModel.AircraftModel.AircraftModel {Key = request.AircraftModelCode}},
                        OperatingAirlineCode = request.OperatingAirlineCode,
                        Meals = new Collection<TravelerMeal>
                        {
                            new TravelerMeal
                            {
                                EntreeIndicator = request.MealServiceCode, MealItemCode = request.MealCode,
                                MealItemDescription = request.MealDescription, TravelerIndex = request.SharesPosition
                            }
                        }
                    }
                },
                Travelers = new Collection<Traveler>()
                {
                    new United.Service.Presentation.ReservationModel.Traveler
                    {
                        Person = new United.Service.Presentation.PersonModel.Person
                        {
                            GivenName = request.GivenName, Surname = request.LastName, Key = request.SharesPosition,
                            CustomerID = request.PassengerId
                        }
                    }
                }
            };
            return addCartRequest;
        }

        private async System.Threading.Tasks.Task DecryptConfirmationNumberAndLastName(MOBPNRByRecordLocatorRequest mobilePnrRequest, string token)
        {
            // To Bypass the Decryption functionality when passed plain text for PNR and lastname for testing purpose
            //if (!Utility.GetBooleanConfigValue("ByPassPNRDecryptionAtLowerEnvironmentOnly")) 
            //{
            //if (this.levelSwitch.TraceInfo)
            //{
            //    LogEntries.Add(LogEntry.GetLogEntry<string>(mobilePnrRequest.TransactionId,
            //        "Decrypt Confirmation - Partner SSO", "Request", mobilePnrRequest.Application.Id,
            //        mobilePnrRequest.Application?.Version.Major, mobilePnrRequest.DeviceId,
            //        mobilePnrRequest.RecordLocator, true, false));
            //}

            var decryptedPnr =
               await _sSOTokenKeyHelper.DecryptData(mobilePnrRequest.RecordLocator, mobilePnrRequest, token).ConfigureAwait(false);
            _logger.LogInformation("DecryptConfirmationNumberAndLastName request:{req}, SessionId:{Id}", JsonConvert.SerializeObject(mobilePnrRequest), mobilePnrRequest.SessionId);

            if (!string.IsNullOrWhiteSpace(decryptedPnr))
            {
                mobilePnrRequest.RecordLocator = decryptedPnr;
            }

            //else
            //{
            //    throw new MOBUnitedException("10000",
            //    _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage"]);
            //}
            //if (this.levelSwitch.TraceInfo)
            //{
            //    LogEntries.Add(LogEntry.GetLogEntry<string>(mobilePnrRequest.TransactionId,
            //        "Decrypt LastName - Partner SSO", "Request", mobilePnrRequest.Application.Id,
            //        mobilePnrRequest.Application?.Version.Major, mobilePnrRequest.DeviceId, mobilePnrRequest.LastName,
            //        true, false));
            //}

            var decryptedLastName =
               await _sSOTokenKeyHelper.DecryptData(mobilePnrRequest.LastName, mobilePnrRequest, token).ConfigureAwait(false);

            if (!string.IsNullOrWhiteSpace(decryptedLastName))
            {
                mobilePnrRequest.LastName = decryptedLastName;
            }

            //else
            //{
            //    throw new MOBUnitedException("10000",
            //    _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage"]);
            //}
            //}
        }

        private async Task<MOBInFlightMealsRefreshmentsResponse> GetRefreshments(MOBInFlightMealsRefreshmentsRequest request, DynamicOfferDetailResponse persistDynamicOfferDetailResponse)
        {
            bool isAtleastOneProductSelected = false;
            var paxId = request.PassengerId;
            var segmentId = request.SegmentId;
            MOBInFlightMealsRefreshmentsResponse refreshmentsResponse = null;
            List<MOBInFlightMealsRefreshmentsResponse> saveResponse = new List<MOBInFlightMealsRefreshmentsResponse>();
            if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString()
                           || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString()
                           )
            {
                saveResponse = await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(_headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false);

                if (saveResponse != null)
                {
                    ResetSelectedRefreshments(request, saveResponse);

                    if (request.SelectedRefreshments != null && request.SelectedRefreshments.Count > 0)
                    {
                        if (saveResponse.Any(x => x.Passenger.PassengerId == request.PassengerId && x.SegmentId == request.SegmentId))
                            saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    }
                    await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);
                }

                if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString())
                    paxId = await IsNextTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);
                else if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString())
                    paxId = await IsPreviousTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);

                if (saveResponse.Any(x => x.Passenger.PassengerId == paxId && x.SegmentId == request.SegmentId))
                {
                    var resp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                    if (saveResponse != null && saveResponse.Count > 0)
                        UpdateQuantity(saveResponse, resp, request.Application);
                    return resp;
                }
            }
            //Continue checkout action
            if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnContinueCheckoutButton.ToString()
                || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnSaveNContinue.ToString())
            {
                saveResponse = await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(_headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false);
                if (saveResponse != null)
                    ResetSelectedRefreshments(request, saveResponse);
                if (request.SelectedRefreshments != null && request.SelectedRefreshments.Count > 0 && request.SelectedRefreshments.Any(x => x.Quantity > 0))
                {
                    if (saveResponse != null && saveResponse.Count > 0)
                        saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    else
                    {
                        var currentPaxResonse = await GetRefreshmentsPerPaxSegment(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
                        if (saveResponse == null) saveResponse = new List<MOBInFlightMealsRefreshmentsResponse>();
                        saveResponse.Add(currentPaxResonse);
                        saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    }
                    isAtleastOneProductSelected = true;
                }
                await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);

                if (saveResponse.Any(x => x.Passenger.PassengerId == paxId && x.SegmentId == request.SegmentId))
                {
                    var resp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                    if (saveResponse != null && saveResponse.Count > 0)
                        UpdateQuantity(saveResponse, resp, request.Application);

                    if (_configuration.GetValue<bool>("EnableInflightMealsPremiumCabinEmailAddressFeature") &&
                          request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnSaveNContinue.ToString() &&
                          string.IsNullOrEmpty(request.PremiumCabinMealEmailAddress))
                    {
                        var validationResp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                        SetValidationMessage(validationResp);
                    }

                    if (!isAtleastOneProductSelected && !CheckAtleastOneMealSelected(saveResponse))
                    {
                        SetValidationMessage(resp);
                    }
                    return resp;
                }
                if (!isAtleastOneProductSelected && !CheckAtleastOneMealSelected(saveResponse))
                {
                    var currentPaxResonse = await GetRefreshmentsPerPaxSegment(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
                    SetValidationMessage(currentPaxResonse);
                    return currentPaxResonse;
                }
            }


            var response = await GetRefreshmentsPerPaxSegment(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
            saveResponse.Add(response);
            await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);
            if (saveResponse != null && saveResponse.Count > 0)
                UpdateQuantity(saveResponse, response, request.Application);
            return response;
        }

        private async Task<MOBInFlightMealsRefreshmentsResponse> GetRefreshmentsPerPaxSegment(MOBInFlightMealsRefreshmentsRequest request, DynamicOfferDetailResponse persistDynamicOfferDetailResponse,
            string paxId, string segmentId, MOBInFlightMealsRefreshmentsResponse response)
        {
            if (response == null) response = new MOBInFlightMealsRefreshmentsResponse();

            if (persistDynamicOfferDetailResponse.Travelers == null ||
                persistDynamicOfferDetailResponse.Offers == null ||
                persistDynamicOfferDetailResponse.FlightSegments == null)
                return null;
            bool isFreeMeals = false;
            var bookingClasses = persistDynamicOfferDetailResponse.FlightSegments.Where(a => a.ID == segmentId).SelectMany(b => b.BookingClasses).ToList();
            Collection<BookingClass> collection = new ObservableCollection<BookingClass>(bookingClasses.ToList().Distinct());
            Service.Presentation.ReservationModel.Reservation cslReservation = await GetCslReservation(request.SessionId, false);
            var totalPassengers = cslReservation.Travelers.Count();
            var emailAddress = _configuration.GetValue<bool>("EnablePrePopulateEmail") ? GetPrePopulateEmailAddressFromCSLReservation(cslReservation, request) : GetEmailAddressFromCSLReservation(cslReservation.Travelers, request);

            foreach (var offer in persistDynamicOfferDetailResponse.Offers)
            {
                var productDetails = offer?.ProductInformation?.ProductDetails;

                foreach (var productDetail in productDetails)
                {
                    string softDrinksURL = productDetail?.Product?.Characteristics.Where(a => a.Code == "ImageURL_SOFTDRNK").FirstOrDefault()?.Value;
                    if (productDetail.Product.Code == request.ProductCode &&
                        productDetail.Product.SubProducts.Where(a => a.Association.SegmentRefIDs[0] == segmentId).ToList().Count > 0) // have to check segment id otherwise it will loop for other segments and affect the logic down the layer
                    {
                        var subProducts = productDetail?.Product?.SubProducts;

                        response.Passenger = GetPassenger(persistDynamicOfferDetailResponse.Travelers, paxId, subProducts, totalPassengers, softDrinksURL);
                        response.FlightDescription = GetFlightDescription(persistDynamicOfferDetailResponse.FlightSegments.Where(x => x.ID == segmentId).FirstOrDefault());
                        var maxBevQty = subProducts?.Where(c => c.InEligibleReason == null && c.Extension?.MealCatalog?.IsAlcohol == "true")?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(b => b.Code == "MaxQtyByMealType")?.FirstOrDefault()?.Value;
                        if (!string.IsNullOrEmpty(maxBevQty))
                            response.MaxQuantity = Convert.ToInt32(maxBevQty);
                        List<Service.Presentation.ProductModel.SubProduct> subProductsList = subProducts.Cast<Service.Presentation.ProductModel.SubProduct>().ToList();
                        isFreeMeals = (!isFreeMeals) ? IsFreeMeals(subProductsList) : isFreeMeals;//1 segment will always either have Free meals or Snacks n Beverages
                        response.PremiumCabinMealEmailAddress = isFreeMeals ? emailAddress : string.Empty;
                        response.Captions = GetCaptionsForSnacksNBeverages(isFreeMeals);
                        response.IsNextTravellerAvailable = await IsNextTravellerAvailable(Convert.ToInt32(paxId), request.SessionId, segmentId) != "" ? true : false;
                        response.IsPreviousTravellerAvailable = await IsPreviousTravellerAvailable(Convert.ToInt32(paxId), request.SessionId, segmentId) != "" ? true : false;
                        response.SegmentId = segmentId;
                        if (subProducts != null)
                        {

                            foreach (var subProduct in subProducts)
                            {
                                var catalog = subProduct.Extension.MealCatalog;
                                isFreeMeals = IsUpperCabinMeals(subProduct); // with dynamic POM for upper cabin there will be more free meals, there will NONMEAL, SPLML
                                MOBInFlightMealRefreshment refreshment = GetMeal(catalog, paxId, segmentId, subProduct.Prices, request, subProduct.Extension.AdditionalExtensions, subProduct, subProduct.Extension.MealCatalog.IsAlcohol, null);
                                if (subProduct.Extension.MealCatalog.IsAlcohol.ToLower() == "true")
                                {
                                    if (response.Beverages == null)
                                        response.Beverages = new List<MOBInFlightMealRefreshment>();
                                    if (refreshment != null)
                                    {
                                        response.Beverages.Add(refreshment);
                                    }
                                }
                                else if (isFreeMeals)
                                {
                                    if (response.FreeMeals == null)
                                        response.FreeMeals = new List<MOBInFlightMealRefreshment>();

                                    if (refreshment != null)
                                        response.FreeMeals.Add(refreshment);
                                }
                                else if (subProduct.Extension.MealCatalog?.MealCategory == InflightRefreshementType.Snack.ToString())
                                {
                                    if (response.Snacks == null)
                                        response.Snacks = new List<MOBInFlightMealRefreshment>();

                                    if (refreshment != null)
                                    {
                                        if (refreshment.DietIndicators == null) refreshment.DietIndicators = new Collection<string>();
                                        refreshment.DietIndicators = subProduct.Descriptions;
                                        response.Snacks.Add(refreshment);
                                    }

                                }

                            }
                            BuildDietIndicators(response, productDetail);
                        }
                    }
                }
            }
            return response;
        }

        private async Task<MOBInFlightMealsRefreshmentsResponse> GetRefreshmentsV2(MOBInFlightMealsRefreshmentsRequest request, DynamicOfferDetailResponse persistDynamicOfferDetailResponse)
        {
            var toggleCheck = _shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version?.Major, request?.CatalogValues);
            bool isAtleastOneProductSelected = false;
            var paxId = request.PassengerId;
            var segmentId = request.SegmentId;
            List<Service.Presentation.ProductModel.SubProduct> subProductsList = persistDynamicOfferDetailResponse.Offers?.FirstOrDefault(x => x.ID == segmentId)?.ProductInformation?.ProductDetails?.FirstOrDefault().Product.SubProducts.Cast<Service.Presentation.ProductModel.SubProduct>().ToList();
            var isFreeMeal = IsFreeMeals(subProductsList);
            MOBInFlightMealsRefreshmentsResponse refreshmentsResponse = null;

            List<MOBInFlightMealsRefreshmentsResponse> saveResponse = new List<MOBInFlightMealsRefreshmentsResponse>();
            if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString()
                           || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString()
                           )
            {
                saveResponse = await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(_headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false);

                if (saveResponse != null)
                {
                    ResetSelectedRefreshments(request, saveResponse);

                    if (request.SelectedRefreshments != null && request.SelectedRefreshments.Count > 0)
                    {
                        if (saveResponse.Any(x => x.Passenger.PassengerId == request.PassengerId && x.SegmentId == request.SegmentId))
                            saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    }
                    await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);
                }

                if (_shoppingUtility.EnablePOMPreArrival(request.Application.Id, request.Application.Version.Major, request.CatalogValues))
                {
                    var currentPax = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                    if (currentPax != null && !IsPreArrivalSelectionsQualified(request, subProductsList, currentPax))
                    {
                        return currentPax;
                    }
                }

                if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString())
                    paxId = await IsNextTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);
                else if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString())
                    paxId = await IsPreviousTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);

                if (saveResponse.Any(x => x.Passenger.PassengerId == paxId && x.SegmentId == request.SegmentId))
                {
                    var resp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                    if (saveResponse != null && saveResponse.Count > 0)
                        UpdateQuantity(saveResponse, resp, request.Application);
                    if (toggleCheck)
                    {
                        await SetBtnTxtChangePermission(request, saveResponse, persistDynamicOfferDetailResponse);
                    }
                    return resp;
                }
            }
            //Continue checkout action
            if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnContinueCheckoutButton.ToString()
                || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnSaveNContinue.ToString())
            {
                saveResponse = await _sessionHelperService.GetSession<List<MOBInFlightMealsRefreshmentsResponse>>(_headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }).ConfigureAwait(false);
                if (saveResponse != null)
                    ResetSelectedRefreshments(request, saveResponse);
                if (request.SelectedRefreshments != null && request.SelectedRefreshments.Count > 0 && request.SelectedRefreshments.Any(x => x.Quantity > 0))
                {
                    if (saveResponse != null && saveResponse.Count > 0)
                        saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    else
                    {
                        var currentPaxResonse = await GetRefreshmentsPerPaxSegmentV2(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
                        if (saveResponse == null) saveResponse = new List<MOBInFlightMealsRefreshmentsResponse>();
                        saveResponse.Add(currentPaxResonse);
                        saveResponse = AddUpdateSelectedRefreshments(saveResponse, request);
                    }
                    isAtleastOneProductSelected = true;
                }
                await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);

                if (saveResponse.Any(x => x.Passenger.PassengerId == paxId && x.SegmentId == request.SegmentId))
                {
                    var resp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                    if (saveResponse != null && saveResponse.Count > 0)
                        UpdateQuantity(saveResponse, resp, request.Application);

                    if (_configuration.GetValue<bool>("EnableInflightMealsPremiumCabinEmailAddressFeature") &&
                          request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnSaveNContinue.ToString() &&
                          string.IsNullOrEmpty(request.PremiumCabinMealEmailAddress))
                    {
                        var validationResp = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == paxId);
                        SetValidationMessage(validationResp);
                    }

                    if (!isAtleastOneProductSelected && !CheckAtleastOneMealSelected(saveResponse))
                    {
                        SetValidationMessage(resp);
                    }
                    if (!isFreeMeal && IsTherePastSelection(request, saveResponse, persistDynamicOfferDetailResponse))
                    {
                        resp.AlertMessages = null;
                    }
                    return resp;
                }
                if (!isAtleastOneProductSelected && !CheckAtleastOneMealSelected(saveResponse))
                {
                    var currentPaxResonse = await GetRefreshmentsPerPaxSegmentV2(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
                    SetValidationMessage(currentPaxResonse);
                    if (!isFreeMeal && IsTherePastSelection(request, saveResponse, persistDynamicOfferDetailResponse))
                    {
                        currentPaxResonse.AlertMessages = null;
                    }
                    return currentPaxResonse;
                }
            }


            var response = await GetRefreshmentsPerPaxSegmentV2(request, persistDynamicOfferDetailResponse, paxId, segmentId, refreshmentsResponse);
            saveResponse.Add(response);
            _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(saveResponse, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false).GetAwaiter().GetResult();
            if (saveResponse != null && saveResponse.Count > 0)
            {
                UpdateQuantity(saveResponse, response, request.Application);
                if (toggleCheck)
                {
                    await SetBtnTxtChangePermission(request, saveResponse, persistDynamicOfferDetailResponse);
                }
            }
            return response;
        }

        private bool IsPreArrivalSelectionsQualified(MOBInFlightMealsRefreshmentsRequest request, List<Service.Presentation.ProductModel.SubProduct> subProducts, MOBInFlightMealsRefreshmentsResponse resp)
        {
            var isOnlyOneMealSelectedInPreArrivalMarket = subProducts.Any(s => InflightMealType.Prearrival.ToString().Equals(s.Extension.MealCatalog.MealCategory, StringComparison.OrdinalIgnoreCase)) && request.SelectedRefreshments?.Count == 1 && request.SelectedRefreshments[0].SelectedMealType != InflightMealType.SPML.ToString();
            if (isOnlyOneMealSelectedInPreArrivalMarket)
            {
                if (resp.AlertMessages == null) resp.AlertMessages = new List<MOBFSRAlertMessage>();
                resp.AlertMessages.Add(new MOBFSRAlertMessage
                {
                    AlertType = MOBFSRAlertMessageType.Warning.ToString(),
                    HeaderMessage = "Please make a selection to save changes."
                });
                return false;
            }
            else
            {
                resp.AlertMessages = null;
            }
            return true;
        }

        // using dynamic offer details
        // using past selections
        private int GetEconomyEditPastSelectionQuantity(DynamicOfferDetailResponse persistDynamicOfferDetailResponse, MOBInFlightMealRefreshment refreshment)
        {
            // helper to return correct quantity
            int countHelpInside = 0;
            foreach (var offer in persistDynamicOfferDetailResponse.Offers)
            {
                var productDetails = offer?.ProductInformation?.ProductDetails;
                foreach (var productDetail in productDetails)
                {
                    var product = productDetail?.Product;
                    var subProducts = productDetail?.Product?.SubProducts;
                    if (product.Code == "PAST_SELECTION")
                    {
                        foreach (var subProduct in subProducts)
                        {
                            var quanHelps = subProduct?.Extension.AdditionalExtensions;
                            var mealHelp = subProduct?.Extension?.MealCatalog?.Characteristics;
                            var mealCodeHelp = mealHelp.Where(a => a.Code == "MealServiceCode")?.FirstOrDefault()?.Value.ToString();
                            var idHelps = subProduct?.Prices;
                            if ((idHelps.Select(a => a.ID)?.FirstOrDefault()?.ToString()) == (refreshment.ProductId))
                            {
                                foreach (var quanHelp in quanHelps)
                                {
                                    var quanHelp2 = quanHelp?.Characteristics;
                                    var quanCodeHelp = quanHelp2.Where(x => x.Code == "Quantity")?.FirstOrDefault()?.Value.ToString();
                                    if (mealCodeHelp == refreshment?.MealCode)
                                    {
                                        countHelpInside = Convert.ToInt32(quanCodeHelp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return countHelpInside;
        }

        private bool CheckAtleastOneMealSelected(List<MOBInFlightMealsRefreshmentsResponse> saveResponse)
        {

            return saveResponse.Any(X => X.Snacks != null && X.Snacks.Any(y => y.Quantity > 0))
                || saveResponse.Any(X => X.Beverages != null && X.Beverages.Any(y => y.Quantity > 0))
                || saveResponse.Any(X => X.FreeMeals != null && X.FreeMeals.Any(y => y.Quantity > 0))
                || saveResponse.Any(X => X.DifferentPlanOptions != null && X.DifferentPlanOptions.Any(y => y.Quantity > 0))
                || saveResponse.Any(X => X.SpecialMeals != null && X.SpecialMeals.Any(y => y.Quantity > 0));

        }

        // using dynamic offer details
        // using past selections
        private MOBInFlightMealsRefreshmentsResponse EconomySaveBtnPermissionV2(MOBInFlightMealsRefreshmentsRequest request, MOBInFlightMealsRefreshmentsResponse response, DynamicOfferDetailResponse persistDynamicOfferDetailResponse)
        {
            response.IsSaveBtnAllowedInEconomy = true;
            var countSubproducts = 0;
            // helps keep count of the sold quantity per traveler
            var countHelpInside = 0;
            // helps keep count of the selected refreshments per traveler 
            int countHelpSR1 = 0;
            // helps keep count for quantities that aren't 0
            int countHelpQantNot0 = 0;
            var paxId = request.PassengerId;
            var segmentId = request.SegmentId;
            var firstTravelerId = persistDynamicOfferDetailResponse.Travelers[0].ID;
            for (int i = 0; i < persistDynamicOfferDetailResponse.Travelers.Count; i++)
            {
                foreach (var offer in persistDynamicOfferDetailResponse.Offers)
                {
                    var productDetails = offer?.ProductInformation?.ProductDetails;
                    var travelerId = persistDynamicOfferDetailResponse.Travelers[i].ID;
                    foreach (var productDetail in productDetails)
                    {
                        var product = productDetail?.Product;
                        var characteristics = product?.Characteristics;
                        var subProducts = productDetail?.Product?.SubProducts;
                        countSubproducts = subProducts.Count;
                        if ((characteristics != null) && (characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true"))
                        {
                            response.Passenger.IsEditable = true;
                        }
                        // checking more than 1 traveler
                        if ((product.Code == "PAST_SELECTION") && (request.PassengerId != travelerId) && (request.InflightMealRefreshmentsActionType != InflightMealRefreshmentsActionType.TapOnEditLink))
                        {
                            foreach (var subProduct in subProducts)
                            {
                                var quanHelps = subProduct?.Extension.AdditionalExtensions;
                                var idHelps = subProduct.Association.TravelerRefIDs;
                                foreach (var idHelp in idHelps)
                                {
                                    if (idHelp != request.PassengerId)
                                    {
                                        foreach (var quanHelp in quanHelps)
                                        {
                                            var quanHelp2 = quanHelp.Characteristics;
                                            var quanValueHelp = quanHelp2.Where(x => x.Code == "Quantity")?.FirstOrDefault()?.Value.ToString();
                                            var quanCodeHelp = quanHelp2.Where(x => x.Code == "Quantity")?.FirstOrDefault()?.Code.ToString();
                                            if ((quanCodeHelp == "Quantity") && (Convert.ToInt32(quanValueHelp)) > 0)
                                            {
                                                countHelpInside = countHelpInside + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if ((product.Code == "PAST_SELECTION") && (request.InflightMealRefreshmentsActionType == InflightMealRefreshmentsActionType.TapOnEditLink) && (travelerId != firstTravelerId))
                        {
                            foreach (var subProduct in subProducts)
                            {
                                var quanHelps = subProduct?.Extension.AdditionalExtensions;
                                var idHelps = subProduct.Association.TravelerRefIDs;
                                foreach (var idHelp in idHelps)
                                {
                                    if (idHelp != request.PassengerId)
                                    {
                                        foreach (var quanHelp in quanHelps)
                                        {
                                            var quanHelp2 = quanHelp.Characteristics;
                                            var quanValueHelp = quanHelp2.Where(x => x.Code == "Quantity")?.FirstOrDefault()?.Value.ToString();
                                            var quanCodeHelp = quanHelp2.Where(x => x.Code == "Quantity")?.FirstOrDefault()?.Code.ToString();
                                            if ((quanCodeHelp == "Quantity") && (Convert.ToInt32(quanValueHelp)) > 0)
                                            {
                                                countHelpInside = countHelpInside + 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (productDetail.Product.Code == request.ProductCode &&
                                productDetail.Product.SubProducts.Where(a => a.Association.SegmentRefIDs[0] == segmentId).ToList().Count > 0) // have to check segment id otherwise it will loop for other segments and affect the logic down the layer
                        {
                            if (subProducts != null)
                            {
                                var traveler = persistDynamicOfferDetailResponse.Travelers[i];
                                if ((request.PassengerId) == travelerId && (request.InflightMealRefreshmentsActionType != InflightMealRefreshmentsActionType.TapOnEditLink))
                                {
                                    foreach (var selectedRefreshment in request.SelectedRefreshments)
                                    {
                                        if ((selectedRefreshment.Quantity == 0) && ((selectedRefreshment.SelectedMealCode) != null))
                                        {
                                            countHelpSR1 = countHelpSR1 + 1;
                                        }
                                    }
                                    if (countHelpSR1 == 0)
                                    {
                                        countHelpQantNot0 = countHelpQantNot0 + 1;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (request.InflightMealRefreshmentsActionType == InflightMealRefreshmentsActionType.TapOnEditLink)
            {
                if ((persistDynamicOfferDetailResponse.Travelers.Count == 1) && (countHelpInside > 0))
                {
                    response.IsSaveBtnAllowedInEconomy = true;
                }
                else if ((countHelpInside == 0) && (persistDynamicOfferDetailResponse.Travelers.Count > 1))
                {
                    response.IsSaveBtnAllowedInEconomy = true;
                }
                else
                {
                    response.IsSaveBtnAllowedInEconomy = false;
                }
                return response;
            }
            if ((persistDynamicOfferDetailResponse.Travelers.Count == 1) && (countHelpSR1 != request.SelectedRefreshments.Count) && (request.SelectedRefreshments.Count != 0))
            {
                response.IsSaveBtnAllowedInEconomy = true;
            }
            else if ((countHelpSR1 == request.SelectedRefreshments.Count) && (countHelpQantNot0 == 0) && (countHelpInside == 0) && (persistDynamicOfferDetailResponse.Travelers.Count > 1) && (request.InflightMealRefreshmentsActionType != InflightMealRefreshmentsActionType.TapOnEditLink))
            {
                response.IsSaveBtnAllowedInEconomy = true;
            }
            else
            {
                response.IsSaveBtnAllowedInEconomy = false;
            }
            return response;
        }


        private void SetValidationMessage(MOBInFlightMealsRefreshmentsResponse response)
        {
            if (response.AlertMessages == null) response.AlertMessages = new List<MOBFSRAlertMessage>();
            response.AlertMessages.Add(new MOBFSRAlertMessage
            {
                AlertType = MOBFSRAlertMessageType.Warning.ToString(),
                HeaderMessage = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Make_Selection_Message")
            });
        }

        private async Task<string> IsNextTravellerAvailable(int paxId, string sessionId, string segmentId)
        {

            var offerResponse = await _sessionHelperService.GetSession<MOBInFlightMealsOfferResponse>(_headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }).ConfigureAwait(false);
            var passengers = offerResponse.FlightSegments.Where(x => x.SegmentId == segmentId).FirstOrDefault()?.Passengers;
            if (passengers != null)
            {
                int passengerId = GetNextPassengerId(paxId, passengers);
                if (passengerId > 0)
                    return passengerId.ToString();
            }
            return "";
        }

        private async Task<MOBInFlightMealsRefreshmentsResponse> GetRefreshmentsPerPaxSegmentV2(MOBInFlightMealsRefreshmentsRequest request, DynamicOfferDetailResponse persistDynamicOfferDetailResponse,
           string paxId, string segmentId, MOBInFlightMealsRefreshmentsResponse response)
        {
            if (response == null) response = new MOBInFlightMealsRefreshmentsResponse();
            string selectedMealCode = string.Empty;
            if (persistDynamicOfferDetailResponse.Travelers == null ||
                persistDynamicOfferDetailResponse.Offers == null ||
                persistDynamicOfferDetailResponse.FlightSegments == null)
                return null;
            bool isFreeMeals = false;
            var bookingClasses = persistDynamicOfferDetailResponse.FlightSegments.Where(a => a.ID == segmentId).SelectMany(b => b.BookingClasses).ToList();
            Collection<BookingClass> collection = new ObservableCollection<BookingClass>(bookingClasses.ToList().Distinct());
            Service.Presentation.ReservationModel.Reservation cslReservation = await GetCslReservation(request.SessionId, false);
            var totalPassengers = cslReservation.Travelers.Count();
            var emailAddress = _configuration.GetValue<bool>("EnablePrePopulateEmail") ? GetPrePopulateEmailAddressFromCSLReservation(cslReservation, request) : GetEmailAddressFromCSLReservation(cslReservation.Travelers, request);
            bool isTapOnEditClicked = false;
            if (!_configuration.GetValue<bool>("DisableSaveChangesButtonTextFeature"))
            {
                var mealOfferResponse = await _sessionHelperService.GetSession<MOBInFlightMealsOfferResponse>(_headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }).ConfigureAwait(false);
                var isAlreadySelected = mealOfferResponse?.FlightSegments?.Where(a => a.SegmentId == segmentId)?.FirstOrDefault()?.Passengers?.Where(b => b.SummaryOfPurchases == null || b.SummaryOfPurchases.Count != 0);
                isTapOnEditClicked = (isAlreadySelected != null && isAlreadySelected.Count() != 0) ? true : false;
            }
            foreach (var offer in persistDynamicOfferDetailResponse.Offers)
            {
                var productDetails = offer?.ProductInformation?.ProductDetails;
                var isEditable = false;
                var pastSelection = productDetails.FirstOrDefault(x => x.Product.Code == "PAST_SELECTION");
                if (pastSelection != null && pastSelection.Product.SubProducts.Any(x => x.Association.SegmentRefIDs.Contains(request.SegmentId) && x.Association.TravelerRefIDs.Contains(request.PassengerId)))
                {
                    isEditable = true;
                }

                foreach (var productDetail in productDetails)
                {
                    string softDrinksURL = productDetail?.Product?.Characteristics?.Where(a => a.Code == "ImageURL_SOFTDRNK").FirstOrDefault()?.Value;
                    if (productDetail.Product.Code == request.ProductCode &&
                        productDetail.Product.SubProducts.Where(a => a.Association.SegmentRefIDs[0] == segmentId).ToList().Count > 0) // have to check segment id otherwise it will loop for other segments and affect the logic down the layer
                    {
                        var subProducts = productDetail.Product.SubProducts;

                        response.Passenger = GetPassenger(persistDynamicOfferDetailResponse.Travelers, paxId, subProducts, totalPassengers, softDrinksURL, isEditable);
                        response.FlightDescription = GetFlightDescription(persistDynamicOfferDetailResponse.FlightSegments.Where(x => x.ID == segmentId).FirstOrDefault());
                        var maxBevQty = subProducts?.Where(c => c.InEligibleReason == null && c.Extension?.MealCatalog?.IsAlcohol == "true")?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(b => b.Code == "MaxQtyByMealType")?.FirstOrDefault()?.Value;
                        if (!string.IsNullOrEmpty(maxBevQty))
                            response.MaxQuantity = Convert.ToInt32(maxBevQty);
                        List<Service.Presentation.ProductModel.SubProduct> subProductsList = subProducts.Cast<Service.Presentation.ProductModel.SubProduct>().ToList();
                        isFreeMeals = (!isFreeMeals) ? IsFreeMealsV2(subProductsList) : isFreeMeals;//1 segment will always either have Free meals or Snacks n Beverages
                        response.PremiumCabinMealEmailAddress = isFreeMeals ? emailAddress : string.Empty;
                        response.Captions = GetCaptionsForSnacksNBeverages(isFreeMeals, isTapOnEditClicked);
                        response.IsNextTravellerAvailable = await IsNextTravellerAvailable(Convert.ToInt32(paxId), request.SessionId, segmentId) != "" ? true : false;
                        response.IsPreviousTravellerAvailable = await IsPreviousTravellerAvailable(Convert.ToInt32(paxId), request.SessionId, segmentId) != "" ? true : false;
                        response.SegmentId = segmentId;
                        if (subProducts != null)
                        {
                            var selectedMealCodeV2 = new List<String>();
                            var isPreArrivalEnabled = _shoppingUtility.EnablePOMPreArrival(request.Application.Id, request.Application.Version.Major, request.CatalogValues);
                            if (IsDynamicPOMOffer(request.Application.Id, request.Application.Version.Major))
                            {
                                var offerResponse = await _sessionHelperService.GetSession<MOBInFlightMealsOfferResponse>(_headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }).ConfigureAwait(false);
                                if (isPreArrivalEnabled && offerResponse != null && offerResponse.FlightSegments != null)
                                {
                                    selectedMealCodeV2 = offerResponse.FlightSegments.Where(x => x.SegmentId == segmentId)?.FirstOrDefault()?.Passengers?.Where(b => b.PassengerId == paxId)?.FirstOrDefault()?.SummaryOfPurchases?.Where(s => s.MealCode != null)?.Select(s => s.MealCode)?.ToList();
                                }
                                else if (offerResponse != null)
                                {
                                    selectedMealCode = offerResponse?.FlightSegments?.Where(x => x.SegmentId == segmentId)?.FirstOrDefault()?.Passengers?.Where(b => b.PassengerId == paxId)?.FirstOrDefault()?.SummaryOfPurchases?.Where(s => s.MealCode != null)?.Select(m => m.MealCode)?.FirstOrDefault();
                                }
                            }
                            bool enableMealOutOfStock = isFreeMeals && _shoppingUtility.EnablePOMMealOutOfStock(request.Application.Id, request.Application.Version.Major);
                            foreach (var subProduct in subProducts)
                            {
                                var catalog = subProduct.Extension.MealCatalog;
                                MOBInFlightMealRefreshment refreshment = GetMeal(catalog, paxId, segmentId, subProduct.Prices, request, subProduct.Extension.AdditionalExtensions, subProduct, subProduct.Extension.MealCatalog.IsAlcohol, pastSelection, enableMealOutOfStock);

                                var isPreArrivalMeal = isPreArrivalEnabled && (InflightMealType.Prearrival.ToString().Equals(subProduct.Extension.MealCatalog.MealCategory, StringComparison.OrdinalIgnoreCase) || subProduct.Extension.MealCatalog.Characteristics?.Any(c => c.Code == "MealAssosiation" && InflightMealType.Prearrival.ToString().Equals(c.Value, StringComparison.OrdinalIgnoreCase)) == true);

                                if (IsDynamicPOMOffer(request.Application.Id, request.Application.Version.Major))
                                {
                                    isFreeMeals = IsUpperCabinMeals(subProduct); // with dynamic POM for upper cabin there will be more free meals, there will NONMEAL, SPLML
                                    if (isFreeMeals && refreshment != null)
                                    {
                                        if (selectedMealCodeV2 != null)
                                        {
                                            if (isPreArrivalEnabled && selectedMealCodeV2.Contains(refreshment.MealCode?.Trim()))
                                            {
                                                refreshment.Quantity = 1;
                                            }
                                        }
                                        else if (selectedMealCode == refreshment.MealCode?.Trim())
                                            refreshment.Quantity = 1; // make this one selected in the radio button
                                    }
                                    else
                                    {
                                        if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version?.Major, request?.CatalogValues) && refreshment != null)
                                        {
                                            refreshment.Quantity = GetEconomyEditPastSelectionQuantity(persistDynamicOfferDetailResponse, refreshment); // use function for using past selections
                                        }
                                    }

                                }
                                if (subProduct.Extension.MealCatalog.IsAlcohol.ToLower() == "true")
                                {
                                    if (response.Beverages == null)
                                        response.Beverages = new List<MOBInFlightMealRefreshment>();
                                    if (refreshment != null)
                                    {
                                        response.Beverages.Add(refreshment);
                                    }
                                }
                                else if (isFreeMeals && !isPreArrivalMeal)
                                {
                                    if (response.FreeMeals == null)
                                        response.FreeMeals = new List<MOBInFlightMealRefreshment>();

                                    if (refreshment != null)
                                        response.FreeMeals.Add(refreshment);
                                }
                                else if (subProduct.Extension.MealCatalog?.MealCategory == InflightRefreshementType.Snack.ToString())
                                {
                                    if (response.Snacks == null)
                                        response.Snacks = new List<MOBInFlightMealRefreshment>();

                                    if (refreshment != null)
                                    {
                                        if (refreshment.DietIndicators == null) refreshment.DietIndicators = new Collection<string>();
                                        refreshment.DietIndicators = subProduct.Descriptions;
                                        response.Snacks.Add(refreshment);
                                    }

                                }
                                if (IsDynamicPOMOffer(request.Application.Id, request.Application.Version.Major) && !isPreArrivalMeal)
                                    BuildDynamicPOMData(request, response, subProduct, refreshment);
                                if (isPreArrivalMeal)
                                {
                                    AddPreArrivalMeals(request, response, subProduct, refreshment);
                                }

                            }
                            if (IsDynamicPOMOffer(request.Application.Id, request.Application.Version.Major))
                            {
                                response.Captions.Add(new MOBItem { Id = "REF_splMealHeader", CurrentValue = productDetail?.Product?.Characteristics?.Where(a => a.Code == "SPML_DESC")?.FirstOrDefault()?.Value });
                                response.Captions.Add(new MOBItem { Id = "REF_splMealDescription", CurrentValue = productDetail?.Product?.Characteristics?.Where(a => a.Code == "SPML")?.FirstOrDefault()?.Value });
                                response.Captions.Add(new MOBItem { Id = "REF_differentPlanOptionHeader", CurrentValue = productDetail?.Product?.Characteristics?.Where(a => a.Code == "SPML_DESC_2")?.FirstOrDefault()?.Value });
                                response.Captions.Add(new MOBItem { Id = "REF_splMealPageTitle", CurrentValue = productDetail?.Product?.Characteristics?.Where(a => a.Code == "SPML_DESC_1")?.FirstOrDefault()?.Value });
                            }
                            if (isPreArrivalEnabled)
                            {
                                if (response.PreArrivalFreeMeals != null && response.PreArrivalFreeMeals.Count > 0 || response.PreArrivalDifferentPlanOptions != null && response.PreArrivalDifferentPlanOptions.Count > 0)
                                {
                                    var preArrivalMeal = subProducts.FirstOrDefault(x => x.Extension.MealCatalog.MealCategory == InflightMealType.Prearrival.ToString() && x.Extension.MealCatalog.Characteristics.Any(y => y.Code == "MealType" && y.Value == InflightMealType.Meal.ToString()));
                                    var mainMeal = subProducts.FirstOrDefault(x => x.Extension.MealCatalog.MealCategory != InflightMealType.Prearrival.ToString() && x.Extension.MealCatalog.Characteristics.Any(y => y.Code == "MealType" && y.Value == InflightMealType.Meal.ToString()));
                                    response.Captions.Add(new MOBItem { Id = "REF_MainMealTabText", CurrentValue = mainMeal.Extension.MealCatalog.MealCategory });
                                    response.Captions.Add(new MOBItem { Id = "REF_PreArrivalTabText", CurrentValue = preArrivalMeal.Extension.MealCatalog.MealCategory });
                                    response.Captions.Add(new MOBItem { Id = "REF_GoToPreArrivalButton", CurrentValue = "Go to prearrival" });
                                    response.Captions.Add(new MOBItem { Id = "REF_SpecialMealsForAll", CurrentValue = productDetail?.Product?.Characteristics?.Where(a => a.Code == "SPML_DESC_4")?.FirstOrDefault()?.Value });
                                    response.Captions.Add(new MOBItem { Id = "REF_PreArrivalSpecialMealUnavailable", CurrentValue = string.Format("{0} {1} {2}", "<b>You've requested to receive special meals for all services on this flight. You may change your preferences by going back to the</b> ", mainMeal.Extension.MealCatalog.MealCategory, " menu") });
                                    response.Captions.Add(new MOBItem { Id = "REF_PreArrivalNoSelectionAlert", CurrentValue = "Please make a selection to save changes." });
                                    response.Captions.Add(new MOBItem { Id = "REF_PreArrivalSaveChanges", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_SaveChanges_ButtonText") });
                                }
                            }
                            BuildDietIndicators(response, productDetail);
                        }
                    }
                }
            }
            return response;
        }
        private void AddPreArrivalMeals(MOBInFlightMealsRefreshmentsRequest request, MOBInFlightMealsRefreshmentsResponse response, Service.Presentation.ProductModel.SubProduct subProduct, MOBInFlightMealRefreshment refreshment)
        {
            if (refreshment != null)
            {
                var mealType = subProduct.Extension.MealCatalog.Characteristics.FirstOrDefault(x => x.Code == "MealType");

                if (InflightMealType.Meal.ToString().Equals(mealType?.Value, StringComparison.OrdinalIgnoreCase))
                {
                    if (response.PreArrivalFreeMeals == null)
                        response.PreArrivalFreeMeals = new List<MOBInFlightMealRefreshment>();
                    response.PreArrivalFreeMeals.Add(refreshment);
                }
                else if (InflightMealType.NONMEAL.ToString().Equals(mealType?.Value, StringComparison.OrdinalIgnoreCase))
                {
                    if (response.PreArrivalDifferentPlanOptions == null)
                        response.PreArrivalDifferentPlanOptions = new List<MOBInFlightMealRefreshment>();
                    response.PreArrivalDifferentPlanOptions.Add(refreshment);
                }
            }
        }
        private void BuildDietIndicators(MOBInFlightMealsRefreshmentsResponse response, Service.Presentation.ProductResponseModel.ProductDetail productDetail)
        {
            if (response.Snacks != null && response.Snacks.Count > 0)
            {
                response.SnackDietIndicators = GetDietIndicators(productDetail.Product?.Characteristics, true);
            }

            if (response.Beverages != null && response.Beverages.Count > 0)
            {
                response.BeverageDietIndicators = GetDietIndicators(productDetail.Product?.Characteristics, false);
            }

            if (response.FreeMeals != null && response.FreeMeals.Count > 0 && _configuration.GetValue<bool>("ShowFreeMealsDietIndicators"))
            {
                response.FreeMealsDietIndicators = GetDietIndicators(productDetail.Product?.Characteristics, true);
            }
        }
        private List<MOBItem> GetDietIndicators(Collection<Characteristic> characteristics, bool isAllRequired)
        {
            var dietIndicators = new List<MOBItem>();
            foreach (var charecterstic in characteristics)
            {
                switch (charecterstic.Code)
                {
                    case "ImageURL_GLUTENFREE":
                        dietIndicators.Add(new MOBItem { Id = "Gluten Free", CurrentValue = charecterstic.Value });
                        break;
                    case "ImageURL_KOSHER":
                        if (isAllRequired)
                            dietIndicators.Add(new MOBItem { Id = "Kosher", CurrentValue = charecterstic.Value });
                        break;
                    case "ImageURL_VEG":
                        if (isAllRequired)
                            dietIndicators.Add(new MOBItem { Id = "Vegeterian", CurrentValue = charecterstic.Value });
                        break;
                    case "ImageURL_VEGAN":
                        if (isAllRequired)
                            dietIndicators.Add(new MOBItem { Id = "Vegan", CurrentValue = charecterstic.Value });
                        break;
                }
            }

            return dietIndicators;
        }
        private void BuildDynamicPOMData(MOBInFlightMealsRefreshmentsRequest request, MOBInFlightMealsRefreshmentsResponse response, Service.Presentation.ProductModel.SubProduct subProduct, MOBInFlightMealRefreshment refreshment)
        {
            if (subProduct.Extension.MealCatalog?.MealCategory == InflightRefreshementType.SPML.ToString())
            {
                if (response.SpecialMeals == null) response.SpecialMeals = new List<MOBInFlightMealRefreshment>();
                if (refreshment != null && IsPOMSplMealsOffer(request.Application.Id, request.Application.Version.Major))
                    response.SpecialMeals.Add(refreshment);
            }
            if (subProduct.Extension.MealCatalog?.MealCategory == InflightRefreshementType.NONMEAL.ToString() || subProduct.Extension.MealCatalog.Characteristics.Any(x => x.Code == "MealType" && InflightMealType.NONMEAL.ToString().Equals(x.Value, StringComparison.OrdinalIgnoreCase)))
            {
                if (response.DifferentPlanOptions == null) response.DifferentPlanOptions = new List<MOBInFlightMealRefreshment>();
                if (refreshment != null)
                    response.DifferentPlanOptions.Add(refreshment);
            }

        }

        public bool IsPOMSplMealsOffer(int appId, string version)
        {
            if (!_configuration.GetValue<bool>("EnablePOMSplMeals")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_InflightMealSpecialMealsFeatureSupported_AppVersion"), _configuration.GetValue<string>("iPhone_InflightMealSpecialMealsFeatureSupported_AppVersion"));
        }

        private bool IsUpperCabinMeals(Service.Presentation.ProductModel.SubProduct subProduct, bool isAlreadySelected = false)
        {
            if (isAlreadySelected)
            {
                var mealType = subProduct?.Extension?.AdditionalExtensions?.FirstOrDefault()?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(mealType) && (mealType == InflightMealType.Meal.ToString() || mealType == InflightMealType.NONMEAL.ToString() || mealType == InflightMealType.SPML.ToString()))
                    return true;
            }
            else
            {
                var mealType = subProduct?.Extension?.MealCatalog?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (mealType != null && (mealType == InflightMealType.Meal.ToString()))
                    return true;
            }
            return false;
        }
        private int GetMealMaxQty(MealCatalogProductExtension catalog, string paxId, MOBInFlightMealsRefreshmentsRequest request, United.Service.Presentation.ProductModel.SubProduct subProduct, United.Service.Presentation.ProductResponseModel.ProductDetail pastSelection)
        {
            var result = subProduct?.Extension?.MealCatalog?.MealCategory == "Beverage" ? Convert.ToInt32(catalog?.Characteristics?.FirstOrDefault(b => b.Code != null && b.Code == ("MaxQtyByMealType"))?.Value)
                : Convert.ToInt32(catalog?.Characteristics?.FirstOrDefault(a => a.Code != null && a.Code == ("MaxAvailableQuantity"))?.Value);
            var productCodeFromPastV2 = pastSelection?.Product?.SubProducts?.Where(e =>
            (e.SubGroupCode == subProduct?.SubGroupCode))?.ToList();

            if (productCodeFromPastV2 == null || subProduct?.Prices == null || string.IsNullOrEmpty(paxId))
                return result;

            foreach (var subProductPastSelection in productCodeFromPastV2)
            {
                foreach (var subProductPrices in subProduct.Prices)
                {
                    if (subProductPrices?.ID == subProductPastSelection?.Prices?.FirstOrDefault()?.ID && subProductPastSelection.Association != null && subProductPastSelection.Association.TravelerRefIDs != null && subProductPastSelection.Association.TravelerRefIDs.Contains(paxId))
                    {
                        var mealCharacteristics = subProductPastSelection.Extension?.MealCatalog?.Characteristics;
                        if (subProduct?.Extension?.MealCatalog?.MealCategory == "Beverage")
                        {
                            break;
                        }
                        else
                        {
                            var maxQuantityFromPastSelection = mealCharacteristics?.FirstOrDefault(a => a != null && a.Code != null && a.Code == ("MaxQuantityPerTraveler"))?.Value;
                            result = maxQuantityFromPastSelection != null ? Convert.ToInt32(maxQuantityFromPastSelection) : result;
                        }
                    }
                }
            }
            return result;
        }

        private MOBInFlightMealRefreshment GetMeal(MealCatalogProductExtension catalog, string paxId, string segmentId, Collection<ProductPriceOption> prices,
            MOBInFlightMealsRefreshmentsRequest request, Collection<AdditionalExtension> additionalExtensions, United.Service.Presentation.ProductModel.SubProduct subProduct, string isAlcohol,
            United.Service.Presentation.ProductResponseModel.ProductDetail pastSelection, bool enableMealOutOfStock = false)
        {
            var toggleCheck = _shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version?.Major, request?.CatalogValues);
            var maxQty = toggleCheck ? GetMealMaxQty(catalog, paxId, request, subProduct, pastSelection) : Convert.ToInt32(catalog?.OfferQty);
            var price = GetDisplayPrice(segmentId, paxId, prices);
            string outOfStockText = string.Empty;
            if (toggleCheck)
            {
                outOfStockText = subProduct?.Prices?.FirstOrDefault(i => i.Association.TravelerRefIDs.Contains(paxId))?.PaymentOptions?.
                FirstOrDefault(j => j?.PriceComponents != null)?.PriceComponents?.FirstOrDefault(k => k?.Characteristics != null)?.Characteristics?.FirstOrDefault(l => l?.Code == "OutOfStock")?.Value;

                if (!string.IsNullOrEmpty(outOfStockText) && outOfStockText.Equals("true", StringComparison.OrdinalIgnoreCase))

                    outOfStockText = "0";
                else
                    outOfStockText = maxQty.ToString();

            }

            var paxs = subProduct.Prices.Where(a => a.Association.TravelerRefIDs[0] == paxId);
            if (paxs.Count() > 0)
            {
                outOfStockText = GetOutofStockText(paxId, segmentId, subProduct, toggleCheck ? Convert.ToInt32(outOfStockText) : Convert.ToInt32(catalog?.OfferQty));
                if (enableMealOutOfStock)
                {
                    var isMealOutOfStock = subProduct.Extension.MealCatalog?.Characteristics?.Any(x => x.Code == "MealType" && InflightMealType.Meal.ToString().Equals(x.Value, StringComparison.OrdinalIgnoreCase)) != null && paxs.FirstOrDefault()?.PaymentOptions?.FirstOrDefault()?.PriceComponents?.FirstOrDefault()?.Characteristics?.Any(c => c.Code == "OutOfStock" && c.Value.Equals("true", StringComparison.OrdinalIgnoreCase)) == true;
                    var text = GetCaptionFromCMSContent("InFlightMeal_UpperCabinMeal_OutOfStockMsg");
                    text = string.IsNullOrEmpty(text) ? _configuration.GetValue<string>("POMMealOutOfStockText") : text;
                    outOfStockText = isMealOutOfStock ? text : string.Empty;
                }

                return new MOBInFlightMealRefreshment
                {
                    ImageURL = GetImageUrl(request, additionalExtensions, catalog.IsAlcohol),
                    DisplayPrice = string.Format("{0}{1}", "$", price),
                    IsMealAvailable = "true",
                    MaxQty = toggleCheck ? maxQty : Convert.ToInt32(catalog?.OfferQty),
                    OfferQuantity = toggleCheck ? maxQty : Convert.ToInt32(catalog?.OfferQty),
                    MaxQtyTxt = string.Format("{0} {1}", GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Maximum_Quantity"), toggleCheck ? maxQty : Convert.ToInt32(catalog?.OfferQty)),
                    MealCode = catalog?.MealCode?.Trim(),
                    MealDescription = (!string.IsNullOrEmpty(isAlcohol) && isAlcohol == "true") ? string.Empty : catalog?.MealDescription?.Trim(),
                    MealName = catalog?.MealShortDescription?.Trim(),
                    MealServiceCode = catalog?.MealServiceCode?.Trim(),
                    MealType = catalog?.MealCategory?.Trim(),
                    NotAvailableMessage = "",
                    OutOfStockText = outOfStockText,
                    Price = price,
                    Quantity = 0,
                    SequenceNumber = catalog?.ServiceSeqNumber,
                    ProductId = GetProductId(segmentId, paxId, prices)
                };
            }
            else return null;
        }
        private string GetProductId(string segmentId, string paxId, Collection<ProductPriceOption> prices)
        {
            foreach (var price in prices)
            {
                if (price.Association.SegmentRefIDs[0] == segmentId && price.Association.TravelerRefIDs[0] == paxId)
                {
                    return price.ID;
                }
            }
            return "";
        }
        private string GetOutofStockText(string paxId, string segmentId, Service.Presentation.ProductModel.SubProduct subProduct, int maxQty)
        {
            // possibly remove
            if (_configuration.GetValue<bool>("EnableDynamicPOM") && maxQty == 0)
                return GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Out_Of_Stock_Message");
            return ((subProduct.InEligibleReason != null && (!string.IsNullOrEmpty(subProduct.InEligibleReason.Description))) || !(subProduct.Prices.Any(x => x.Association.SegmentRefIDs[0] == segmentId && x.Association.TravelerRefIDs[0] == paxId)))
                             ? GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Out_Of_Stock_Message") : string.Empty;
        }
        private string GetImageUrl(MOBInFlightMealsRefreshmentsRequest request, Collection<AdditionalExtension> additionalExtensions, string IsAlcohol)
        {
            if (additionalExtensions == null)
                return string.Empty;

            if (request.Application.Id == 1 || IsAlcohol.ToLower() == "true")
                return additionalExtensions.Select(x => x.Characteristics).FirstOrDefault()?.Where(y => y.Code.ToLower() == "imageurl_3x").Select(z => z.Value).FirstOrDefault();
            else if (request.Application.Id == 2)
                return additionalExtensions.Select(x => x.Characteristics).FirstOrDefault()?.Where(y => y.Code.ToLower() == "imageurl_4x").Select(z => z.Value).FirstOrDefault();
            else
                return string.Empty;
        }

        private double GetDisplayPrice(string segmentId, string paxId, Collection<ProductPriceOption> prices)
        {
            foreach (var price in prices)
            {
                if (price.Association.SegmentRefIDs[0] == segmentId && price.Association.TravelerRefIDs[0] == paxId)
                {
                    double amount;
                    double.TryParse(Convert.ToString(price.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount), out amount);
                    return amount;
                }
            }
            return 0;
        }

        private List<MOBItem> GetCaptionsForSnacksNBeverages(bool isFreeMeals, bool isTaponEditClicked = false)
        {
            var captions = new List<MOBItem>();

            captions.Add(new MOBItem { Id = "REF_NextButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_NextTraveler_ButtonText") });
            captions.Add(new MOBItem { Id = "REF_PrevButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_PreviousTraveler_ButtonText") });
            if (isFreeMeals)
            {
                captions.Add(new MOBItem { Id = "REF_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_UpperCabin_Screen_Title") });


                if (isTaponEditClicked && !_configuration.GetValue<bool>("DisableSaveChangesButtonTextFeature"))
                    captions.Add(new MOBItem { Id = "REF_ContinueButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_SaveChanges_ButtonText") });
                else
                    captions.Add(new MOBItem { Id = "REF_ContinueButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_SaveAndContinue_ButtonText") });

                if (_configuration.GetValue<bool>("EnableInflightMealsPremiumCabinEmailAddressFeature"))
                    captions.Add(new MOBItem { Id = "REF_DESC", CurrentValue = string.Format("{0}\n\n{1}", GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Header_Content"), GetCaptionFromCMSContent("InFlightMeal_UpperCabin_Preorder_Refreshments_Screen_Provide_Email")) });
                else
                    captions.Add(new MOBItem { Id = "REF_DESC", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Header_Content") });
            }
            else
            {
                captions.Add(new MOBItem { Id = "REF_DESC", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Heading_Content") });
                captions.Add(new MOBItem { Id = "REF_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Title") });
                captions.Add(new MOBItem { Id = "REF_ContinueButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_ContinueCheckout_ButtonText") });
                captions.Add(new MOBItem { Id = "REF_SaveButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_SaveAndContinue_ButtonText") });
            }
            return captions;
        }

        private MOBInFlightMealPassenger GetPassenger(Collection<ProductTraveler> travelers, string paxId, Collection<Service.Presentation.ProductModel.SubProduct> subProducts, int paxCount, string softDrinksURL, bool isEditable = false)
        {
            if (travelers == null)
                return null;
            bool isBeveragesAvailable = false;
            int beveragesCount = subProducts.Where(a => a.InEligibleReason == null && a.Extension.MealCatalog.IsAlcohol == "true").ToList().Count;
            var maxBeverageQty = subProducts?.Where(c => c.InEligibleReason == null && c.Extension?.MealCatalog?.IsAlcohol == "true")?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(b => b.Code == "MaxQtyByMealType")?.FirstOrDefault()?.Value;
            foreach (var pax in travelers)
            {
                if (pax.ID == paxId)
                {
                    isBeveragesAvailable = CheckBeverageEligibility(pax.ID, subProducts);
                    return new MOBInFlightMealPassenger
                    {
                        FullName = pax.GivenName.ToLower().ToPascalCase() + " " + pax.Surname.ToLower().ToPascalCase(),
                        PassengerId = pax.ID,
                        IsEditable = isEditable,
                        PassengerDesc = GetPassengerDescription(paxId, paxCount, pax.GivenName.ToLower().ToPascalCase(), pax.Surname.ToLower().ToPascalCase()),
                        NotEligibileDefaultText = GetBeveragesText(isBeveragesAvailable, beveragesCount, pax, maxBeverageQty),
                        IsEligibleForBeverages = (isBeveragesAvailable == true && beveragesCount > 0),
                        BeverageEligiblityDesc = (isBeveragesAvailable == true && beveragesCount > 0) ? GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Alcoholic_Beverages") : string.Empty,
                        SoftDrinksURL = (GetBeveragesText(isBeveragesAvailable, beveragesCount, pax, maxBeverageQty) == string.Empty || (isBeveragesAvailable == true && beveragesCount > 0)) ? string.Empty : softDrinksURL
                    };
                }
            }
            return null;
        }

        private string GetBeveragesText(bool isBeveragesAvailable, int beveragesCount, ProductTraveler pax, string maxBeverageCount)
        {
            string beveragesText = string.Empty;
            if ((isBeveragesAvailable && beveragesCount > 0))
                beveragesText = string.Format(GetCaptionFromCMSContent("InFlightMeal_Beverages_ScreenTitle_Prefix"), maxBeverageCount,
                                " " + GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Complimentary_Non_Alcoholic_Beverages_Message"));
            else if (beveragesCount == 0)
                beveragesText = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Complimentary_Non_Alcoholic_Beverages_Message");
            else if (!isBeveragesAvailable && beveragesCount > 0)
                beveragesText = string.Format(GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Under_21_Years_Non_Alcoholic_Beverages_Message"),
                                                                                   "<b>" + pax.GivenName.ToLower().ToPascalCase(), pax.Surname.ToLower().ToPascalCase() + "</b>");
            return beveragesText;
        }

        private string GetPassengerDescription(string paxId, int totalPassengers, string firstName, string lastName)
        {

            return string.Format("Traveler {0} of {1} | {2} {3}", paxId, totalPassengers, firstName, lastName);
        }

        private bool CheckBeverageEligibility(string paxId, Collection<Service.Presentation.ProductModel.SubProduct> subProducts)
        {
            if (subProducts.Count() > 0) //If any prices available and not available only for this PAX ID 
            {
                foreach (var subProduct in subProducts)
                {
                    if (subProduct.InEligibleReason == null && subProduct.Prices?.ToList().Count > 0 && subProduct.Extension.MealCatalog.IsAlcohol?.ToLower() == "true")
                    {
                        var prices = subProduct.Prices;
                        if (prices.Where(a => a.Association.TravelerRefIDs[0] == paxId).Any())
                            return true;
                    }
                }
            }
            return false;
        }
        private string GetEmailAddressFromCSLReservation(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers, MOBInFlightMealsRefreshmentsRequest request)
        {
            if (_configuration.GetValue<bool>("EnableInflightMealsPremiumCabinEmailAddressFeature"))
            {
                if (!string.IsNullOrEmpty(request.PremiumCabinMealEmailAddress))
                    return request.PremiumCabinMealEmailAddress.Trim();

                var emailAddress = travelers?.Select(x => x.Person)?.Where(x => x.Contact?.Emails != null && x.Contact?.Emails.Count > 0)?.FirstOrDefault()?.Contact?.Emails?.FirstOrDefault()?.Address;
                if (!string.IsNullOrEmpty(emailAddress))
                    return emailAddress.Trim();
            }
            return "";
        }
        private string GetPrePopulateEmailAddressFromCSLReservation(Service.Presentation.ReservationModel.Reservation reservation, MOBInFlightMealsRefreshmentsRequest request)
        {
            if (_configuration.GetValue<bool>("EnableInflightMealsPremiumCabinEmailAddressFeature"))
            {
                if (!string.IsNullOrEmpty(request.PremiumCabinMealEmailAddress))
                    return request.PremiumCabinMealEmailAddress.Trim();

                if (reservation?.EmailAddress != null && reservation?.EmailAddress.Count > 0)
                {
                    var emailAddress = reservation?.EmailAddress[0]?.Address;
                    return emailAddress?.Trim()?.ToLower();
                }
            }
            return "";
        }
        private int GetNextPassengerId(int paxId, List<MOBInFlightMealPassenger> passengers)
        {
            foreach (var pax in passengers)
            {
                if (Convert.ToInt32(pax.PassengerId) <= paxId)
                    continue;
                else
                {
                    return Convert.ToInt32(pax.PassengerId);

                }
            }
            return 0;
        }

        public void UpdateQuantity(List<MOBInFlightMealsRefreshmentsResponse> saveResponse, MOBInFlightMealsRefreshmentsResponse resp, MOBApplication application)
        {
            if (!GeneralHelper.IsApplicationVersionGreaterorEqual(application.Id, application.Version.Major, _configuration.GetValue<string>("Android_InflightMealSupressPassengerLevelCheckFeatureSupported_AppVersion"), _configuration.GetValue<string>("iPhone_InflightMealSupressPassengerLevelCheckFeatureSupported_AppVersion")))
            {
                if (resp.Snacks != null)
                {
                    foreach (var snack in resp.Snacks)
                    {
                        var sumOfAllSelected = saveResponse.Where(c => c.Snacks.Any(s => s.MealCode == snack.MealCode)).SelectMany(d => d.Snacks.Where(c => c.MealCode == snack.MealCode)).Sum(w => w.Quantity);
                        if (sumOfAllSelected <= snack.OfferQuantity)
                        {
                            snack.MaxQty = (snack.OfferQuantity - sumOfAllSelected) + snack.Quantity;
                            snack.MaxQtyTxt = string.Format("{0} {1}", GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Maximum_Quantity"), snack.MaxQty);
                            snack.OutOfStockText = (snack.MaxQty == 0) ? GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Out_Of_Stock_Message") : string.Empty;
                        }
                    }
                }
                if (resp.Beverages != null)
                {
                    foreach (var beverage in resp.Beverages)
                    {
                        var sumOfAllSelected = saveResponse.Where(c => c.Beverages.Any(s => s.MealCode == beverage.MealCode)).SelectMany(d => d.Beverages.Where(c => c.MealCode == beverage.MealCode)).Sum(w => w.Quantity);
                        if (sumOfAllSelected <= beverage.OfferQuantity)
                        {
                            beverage.MaxQty = (beverage.OfferQuantity - sumOfAllSelected) + beverage.Quantity;
                            beverage.MaxQtyTxt = string.Format("{0} {1}", GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Maximum_Quantity"), beverage.MaxQty);
                            beverage.OutOfStockText = (beverage.MaxQty == 0) ? GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Out_Of_Stock_Message") : string.Empty;
                        }
                    }
                }
            }
        }

        private async Task<string> IsPreviousTravellerAvailable(int paxId, string sessionId, string segmentId)
        {
            var offerResponse = await _sessionHelperService.GetSession<MOBInFlightMealsOfferResponse>(_headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }).ConfigureAwait(false);

            var passengers = offerResponse.FlightSegments.Where(x => x.SegmentId == segmentId).FirstOrDefault()?.Passengers;
            if (passengers != null)
            {
                int passengerId = GetPreviousPassengerId(paxId, passengers);
                if (passengerId > 0)
                    return passengerId.ToString();
            }
            return "";
        }
        private int GetPreviousPassengerId(int paxId, List<MOBInFlightMealPassenger> passengers)
        {
            for (int i = passengers.Count() - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(passengers[i].PassengerId) >= paxId)
                    continue;
                else
                {
                    return Convert.ToInt32(passengers[i].PassengerId);
                }
            }
            return 0;
        }

        private List<MOBInFlightMealsRefreshmentsResponse> AddUpdateSelectedRefreshments(List<MOBInFlightMealsRefreshmentsResponse> refreshmentsResponse, MOBInFlightMealsRefreshmentsRequest request)
        {
            var isPreArrivalEnabled = _shoppingUtility.EnablePOMPreArrival(request.Application.Id, request.Application.Version.Major, request.CatalogValues);

            foreach (var meal in request.SelectedRefreshments)
            {
                if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
                x.SegmentId == request.SegmentId))?.Snacks != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId)).Snacks.Any(x => x.MealCode == meal.SelectedMealCode))
                {
                    foreach (var snack in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).Snacks)
                    {
                        if (snack.MealCode == meal.SelectedMealCode && snack.ProductId == meal.SelectedProductId)
                        {
                            snack.Quantity = meal.Quantity;
                        }
                    }
                }
                else if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
                x.SegmentId == request.SegmentId))?.Beverages != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
                x.SegmentId == request.SegmentId)).Beverages.Any(x => x.MealCode == meal.SelectedMealCode))
                {
                    foreach (var beverages in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).Beverages)
                    {
                        if (beverages.MealCode == meal.SelectedMealCode && beverages.ProductId == meal.SelectedProductId)
                        {
                            beverages.Quantity = meal.Quantity;
                        }
                    }
                }
                else if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId))?.FreeMeals != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
              x.SegmentId == request.SegmentId)).FreeMeals.Any(x => x.MealCode == meal.SelectedMealCode))
                {
                    foreach (var freeMeal in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).FreeMeals)
                    {
                        if (freeMeal.MealCode == meal.SelectedMealCode && freeMeal.ProductId == meal.SelectedProductId)
                        {
                            freeMeal.Quantity = meal.Quantity;
                        }
                    }
                }
                else if (_configuration.GetValue<bool>("EnableDynamicPOM") && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
              x.SegmentId == request.SegmentId))?.DifferentPlanOptions != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
             x.SegmentId == request.SegmentId)).DifferentPlanOptions.Any(x => x.MealCode == meal.SelectedMealCode))
                {
                    foreach (var differentPlanOption in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).DifferentPlanOptions)
                    {
                        if (differentPlanOption.MealCode == meal.SelectedMealCode && differentPlanOption.ProductId == meal.SelectedProductId)
                        {
                            differentPlanOption.Quantity = meal.Quantity;
                        }
                    }
                }
                else if (_configuration.GetValue<bool>("EnableDynamicPOM") && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
              x.SegmentId == request.SegmentId))?.SpecialMeals != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
             x.SegmentId == request.SegmentId)).SpecialMeals.Any(x => x.MealCode == meal.SelectedMealCode))
                {
                    foreach (var specialMeal in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).SpecialMeals)
                    {
                        if (specialMeal.MealCode == meal.SelectedMealCode && specialMeal.ProductId == meal.SelectedProductId)
                        {
                            specialMeal.Quantity = meal.Quantity;
                        }
                    }
                }
                else if (isPreArrivalEnabled)
                {
                    var list = refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId && x.SegmentId == request.SegmentId)?.PreArrivalFreeMeals?.Where(x => x.MealCode == meal.SelectedMealCode && x.ProductId == meal.SelectedProductId);
                    foreach (var preArrivalMeal in list)
                    {
                        preArrivalMeal.Quantity = meal.Quantity;
                    }

                    list = refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId && x.SegmentId == request.SegmentId)?.PreArrivalDifferentPlanOptions?.Where(x => x.MealCode == meal.SelectedMealCode && x.ProductId == meal.SelectedProductId);
                    foreach (var diffPlan in list)
                    {
                        diffPlan.Quantity = meal.Quantity;
                    }
                }
            }

            return refreshmentsResponse;
        }

        public void ResetSelectedRefreshments(MOBInFlightMealsRefreshmentsRequest request, List<MOBInFlightMealsRefreshmentsResponse> refreshmentsResponse)
        {

            if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
            x.SegmentId == request.SegmentId))?.Snacks != null && refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
             x.SegmentId == request.SegmentId)).Snacks.Any())
            {
                foreach (var snack in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).Snacks)
                {

                    snack.Quantity = 0;
                }
            }
            if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
            x.SegmentId == request.SegmentId))?.Beverages != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
            x.SegmentId == request.SegmentId).Beverages.Any())
            {
                foreach (var beverages in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).Beverages)
                {
                    beverages.Quantity = 0;
                }
            }
            if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId))?.FreeMeals != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId).FreeMeals.Any())
            {
                foreach (var freeMeal in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).FreeMeals)
                {
                    freeMeal.Quantity = 0;
                }
            }
            if (_configuration.GetValue<bool>("EnableDynamicPOM"))
            {
                if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId))?.DifferentPlanOptions != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId).DifferentPlanOptions.Any())
                {
                    foreach (var differentOption in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).DifferentPlanOptions)
                    {
                        differentOption.Quantity = 0;
                    }
                }
                if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId))?.SpecialMeals != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId).SpecialMeals.Any())
                {
                    foreach (var specialMeal in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).SpecialMeals)
                    {
                        specialMeal.Quantity = 0;
                    }
                }
            }
            if (_shoppingUtility.EnablePOMPreArrival(request.Application.Id, request.Application.Version.Major, request.CatalogValues))
            {
                if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId))?.PreArrivalFreeMeals != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
           x.SegmentId == request.SegmentId).PreArrivalFreeMeals.Any())
                {
                    foreach (var preArrival in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).PreArrivalFreeMeals)
                    {
                        preArrival.Quantity = 0;
                    }
                }
                if (refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId))?.PreArrivalDifferentPlanOptions != null && refreshmentsResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId &&
               x.SegmentId == request.SegmentId).PreArrivalDifferentPlanOptions.Any())
                {
                    foreach (var diffPlan in refreshmentsResponse.FirstOrDefault((x => x.Passenger.PassengerId == request.PassengerId)).PreArrivalDifferentPlanOptions)
                    {
                        diffPlan.Quantity = 0;
                    }
                }
            }
        }

        private async System.Threading.Tasks.Task RegisterUpperCabinMeals(MOBInFlightMealsRefreshmentsRequest request, MOBInFlightMealsRefreshmentsResponse response)
        {
            //PaymentController paymentController = new PaymentController(logger, payment, formsOfPayment, null, null, seatEngine, null);
            Collection<MerchandizingOfferDetails> merchandizingOfferDetails = new Collection<MerchandizingOfferDetails>();

            merchandizingOfferDetails.Add(new MerchandizingOfferDetails { ProductCode = request.ProductCode });
            if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version.Major))
            {
                merchandizingOfferDetails[0].ProductIds = new List<string>() { request.SegmentId };
            }
            RegisterOfferRequest mOBRegisterOfferRequest = new RegisterOfferRequest
            {
                AccessCode = request.AccessCode,
                Application = request.Application,
                DeviceId = request.DeviceId,
                LanguageCode = request.LanguageCode,
                MerchandizingOfferDetails = merchandizingOfferDetails,
                Flow = FlowType.VIEWRES.ToString(),
                SessionId = request.SessionId
            };
            var jsonRequest = JsonConvert.SerializeObject(mOBRegisterOfferRequest);
            // TO DO: Dependency on RegisterOffers API
            var jsonResponse = await _registerOffersService.RegisterOffers(jsonRequest, request.SessionId, "/Payment/RegisterOffers").ConfigureAwait(false);
            var offerResponse = JsonConvert.DeserializeObject<MOBRegisterOfferResponse>(jsonResponse.ToString());

            if (offerResponse.Exception != null)
            {
                response.Exception = offerResponse.Exception;
                throw new MOBUnitedException(offerResponse.Exception.Message);
            }
            if (offerResponse != null && !string.IsNullOrEmpty(offerResponse.ShoppingCart.CartId) && offerResponse.Exception == null)
            {
                CheckOutRequest checkOutRequest = new CheckOutRequest()
                {
                    SessionId = request.SessionId,
                    CartId = offerResponse.ShoppingCart.CartId,
                    Application = request.Application,
                    AccessCode = request.AccessCode,
                    LanguageCode = request.LanguageCode,
                    FormofPaymentDetails = new MOBFormofPaymentDetails
                    {
                        FormOfPaymentType = null,
                        EmailAddress = request.PremiumCabinMealEmailAddress

                    },
                    Flow = offerResponse.Flow
                };
                //var shopping = new Shopping();
                CheckOutResponse checkOutResponse = new CheckOutResponse();
                checkOutResponse = await _registerCFOP.RegisterFormsOfPayments_CFOP(checkOutRequest);
                if (checkOutResponse.Exception != null)
                {
                    if (response.Exception == null)
                        response.Exception = new MOBException();
                    response.Exception = checkOutResponse.Exception;
                    throw new MOBUnitedException(checkOutResponse.Exception.Message);
                }
            }
        }

        public string DecryptString(string toDecrypt)
        {
            if (!String.IsNullOrEmpty(toDecrypt))
            {
                return MD5DecryptString(toDecrypt, cryptoKey, IV);
            }
            return String.Empty;
        }
        public string MD5DecryptString(string dataToDecrypt, string cryptoKey, string iv)
        {
            try
            {
                byte[] buffer = Convert.FromBase64String(WebUtility.UrlDecode(dataToDecrypt));

                // This is necessary as this is the style 1.0 uses for encryption
                using (TripleDESCryptoServiceProvider crypto = new TripleDESCryptoServiceProvider())
                using (MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider())
                {
                    crypto.Key = MD5.ComputeHash(Encoding.ASCII.GetBytes(cryptoKey));
                    crypto.IV = Convert.FromBase64String(iv);
                    return Encoding.UTF8.GetString(crypto.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
                }
            }
            catch
            {
                return string.Empty;
            }

        }

        public async Task<MOBInFlightMealsOfferResponse> GetInflightMealOffers(MOBInFlightMealsOfferRequest inflightMealRequest, Session session)
        {
            MOBInFlightMealsOfferResponse response = new MOBInFlightMealsOfferResponse();
            inflightMealRequest.ProductCode = _configuration.GetValue<string>("InflightMealProductCode");

            // Flow controls the back button and reservation details visibility
            if (inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnManagerReservationScreen || inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnDeeplinkClick)
            {
                response = await GetInFlightMealsOfferResponse(inflightMealRequest, session);
                response.EnableBackButton = true;
                response.EnableReservationDetailButton = false;
                await _sessionHelperService.SaveSession<MOBInFlightMealsOfferResponse>(response, session.SessionId, new List<string> { session.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }, new MOBInFlightMealsOfferResponse().ObjectName).ConfigureAwait(false);
            }
            else if (inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnSnBCancelClick
                || inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnSnBCloseButton
                || inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnAgreeNPurchaseClick
                || inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnSaveNContinue)
            {
                //Clear the already saved session
                await ClearSessionForInflightMealOffers(inflightMealRequest);
                try
                {
                    //todo
                    response = await GetInFlightMealsOfferResponse(inflightMealRequest, session);
                    response.EnableBackButton = false;
                    response.EnableReservationDetailButton = true;
                    await _sessionHelperService.SaveSession<MOBInFlightMealsOfferResponse>(response, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }, new MOBInFlightMealsOfferResponse().ObjectName).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    response.AlertMessages = new List<Section>();
                    response.AlertMessages.Add(new Section
                    {
                        Text1 = GetCaptionFromCMSContent("InFlightMeal_Payment_Screen_ErrorMessage_Header")
                                                                 ,
                        Text2 = GetCaptionFromCMSContent("InFlightMeal_Payment_Screen_ErrorMessage_Body")
                    });
                    response.Exception = new MOBException() { Message = GetCaptionFromCMSContent("InFlightMeal_Payment_Screen_ErrorMessage_Body") };
                    if (response.Captions == null) response.Captions = new List<MOBItem>();
                    response.Captions.Add(new MOBItem
                    {
                        Id = "LND_ErrorScenario_HeaderMessage",
                        CurrentValue = GetCaptionFromCMSContent("InFlightMeal_LND_ErrorScenario_HeaderMessage")
                    });
                    response.Captions.Add(new MOBItem
                    {
                        Id = "LND_ErrorScenario_BodyMessage",
                        CurrentValue = GetCaptionFromCMSContent("InFlightMeal_LND_ErrorScenario_BodyMessage")
                    });
                    response.EnableReservationDetailButton = true;
                    response.EnableBackButton = false;
                    response.Captions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_LND_ReservationDetailButtonText") });
                    if (inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnSaveNContinue)
                    {
                        response.Captions.Add(new MOBItem { Id = "LND_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_UpperCabin_Screen_Title") });
                    }
                    else
                    {
                        response.Captions.Add(new MOBItem { Id = "LND_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Title") });
                    }
                }
            }
            if (!_configuration.GetValue<bool>("DisableSaveChangesButtonTextFeature"))
                await SetEditFlow(session.SessionId);
            response.ProductCode = inflightMealRequest.ProductCode;
            return response;


        }

        private async Task<MOBInFlightMealsOfferResponse> GetInFlightMealsOfferResponse(MOBInFlightMealsOfferRequest inflightMealRequest, Session session)
        {
            MOBInFlightMealsOfferResponse response;
            Service.Presentation.ReservationModel.Reservation cslReservation = await GetCslReservation(inflightMealRequest.SessionId, false);
            var enableGenericMessageFeature = await _shoppingUtility.IsEnableGenericMessageFeature(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues);

            if (cslReservation != null && cslReservation.FlightSegments != null)
            {
                var mobPnr = await _sessionHelperService.GetSession<MOBPNR>(inflightMealRequest.SessionId, new MOBPNR().ObjectName, new List<string> { inflightMealRequest.SessionId, new MOBPNR().ObjectName }).ConfigureAwait(false);

                DynamicOfferDetailResponse cslMealOfferResponse = null;
                try
                {
                    bool isPOMDeepLinkRequest = false;
                    if (_configuration.GetValue<bool>("EnableRetryLogicForDeeplinkPOMWindowClosed"))
                        isPOMDeepLinkRequest = inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnDeeplinkClick;

                    cslMealOfferResponse = await _merchandizingServices.GetMerchOffersDetailsFromCCE(session, cslReservation, inflightMealRequest, FlowType.VIEWRES.ToString(), mobPnr, inflightMealRequest.ProductCode, isPOMDeepLinkRequest).ConfigureAwait(false);
                    if (enableGenericMessageFeature
                        && cslMealOfferResponse?.Response?.Error != null
                        && cslMealOfferResponse?.Response?.Error?.Count > 0 
                        && inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnDeeplinkClick)
                    {
                        response = GenericMessageError(enableGenericMessageFeature,true);
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    cslMealOfferResponse = null;
                    _logger.LogError("GetInFlightMealsOfferResponse Exception:{ex}", ex.Message + "\n\n Stack Trace - " + ex.StackTrace);
                }

                response = IsDynamicPOMOffer(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major) ? await MapToMOBInflighMealsResponseV2(cslMealOfferResponse, cslReservation, inflightMealRequest.ProductCode, inflightMealRequest.InflightMealOffersActionType, inflightMealRequest) :
                 MapToMOBInflighMealsResponse(cslMealOfferResponse, cslReservation, inflightMealRequest.ProductCode, inflightMealRequest.InflightMealOffersActionType, inflightMealRequest);
            }
            else if (inflightMealRequest.InflightMealOffersActionType == InflightMealOffersFlowType.TapOnDeeplinkClick
                           && _shoppingUtility.EnablePOMDeepLinkInActivePNR(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues))
            {
                response = GenericMessageError(enableGenericMessageFeature,false);
            }
            else
            {
                throw new MOBUnitedException("10000",
                       _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage"));
            }

            return response;
        }

        private MOBInFlightMealsOfferResponse GenericMessageError(bool enableGenericMessageFeature, bool isReservation)
        {
            MOBInFlightMealsOfferResponse response = new MOBInFlightMealsOfferResponse();
            response.Captions = AddDeepLinkFailureCaptions(enableGenericMessageFeature, isReservation);
            if (enableGenericMessageFeature)
                SetCCEAlertMessage(response);
            return response;
        }

        private void SetCCEAlertMessage(MOBInFlightMealsOfferResponse response)
        {
            if (response.AlertMessages == null) response.AlertMessages = new List<Section>();
            response.AlertMessages.Add(new Section
            {
                Text1 = GetCaptionFromCMSContent("PreOrderMealTripUnhandledErrorMessage") ?? _configuration.GetValue<string>("PreOrderMealTripUnhandledErrorMessage")
            });
        }
        private MOBInFlightMealsOfferResponse MapToMOBInflighMealsResponse(DynamicOfferDetailResponse dynamicOfferResponse, Service.Presentation.ReservationModel.Reservation cslReservation,
            string productCode, InflightMealOffersFlowType inflightMealOffersFlowType, MOBInFlightMealsOfferRequest inflightMealRequest)
        {
            var response = new MOBInFlightMealsOfferResponse();
            if (dynamicOfferResponse == null || dynamicOfferResponse.Offers == null)
            {
                if (inflightMealOffersFlowType != InflightMealOffersFlowType.TapOnDeeplinkClick)
                    response.Exception = new MOBException() { Code = "9999", Message = "There are no merchandising offers" };
                return response;
            }

            bool isUpperCabin = false;
            var offersData = dynamicOfferResponse.Offers;
            string landingImgurl = string.Empty;

            response.FlightSegments = new List<MOBInflightMealSegment>();


            if (cslReservation.FlightSegments != null && cslReservation.FlightSegments.Count() > 0)
            {
                #region  the all flight not eligible when the flightsegment data is not returned
                if (dynamicOfferResponse.FlightSegments == null || dynamicOfferResponse.FlightSegments.Count == 0)
                {
                    foreach (var cslSegment in cslReservation.FlightSegments)
                    {

                        var allInEligibleSegments = GetInflightMealSegment(MapToProductFlightSegment(cslSegment), null, dynamicOfferResponse, cslSegment.TripNumber);
                        allInEligibleSegments.InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");
                        allInEligibleSegments.IsMealSelctionAvailable = false;
                        allInEligibleSegments.SnacksPurchaseLinkText = "";
                        response.FlightSegments.Add(allInEligibleSegments);
                    }
                    return response;
                }
                #endregion

                foreach (var segment in dynamicOfferResponse.FlightSegments)
                {

                    bool isMealsegmentAdded = false;
                    string tripNumber = GetTripNumberFromCSLReservation(cslReservation, segment.SegmentNumber, segment.ArrivalAirport.IATACode, segment.DepartureAirport.IATACode);
                    foreach (var offer in dynamicOfferResponse?.Offers)
                    {

                        if (offer == null || offer.ProductInformation == null || offer.ProductInformation.ProductDetails == null
                            || !offer.ProductInformation.ProductDetails.Select(b => b.Product?.SubProducts).Where(c => c.Any(d => d.Association.SegmentRefIDs[0] == segment.ID)).Any())
                            continue;
                        if (response.FlightSegments == null)
                            response.FlightSegments = new List<MOBInflightMealSegment>();

                        var productDetails = offer.ProductInformation?.ProductDetails;
                        var product = productDetails?.Where(x => x.Product != null && x.Product.Code == productCode && x.Product.SubProducts != null)
                            ?.Select(y => y.Product)
                            .ToList();
                        var alreadySelectedProduct = productDetails?.Where(a => a.Product.Code == "PAST_SELECTION")
                            ?.Select(b => b.Product)
                            .ToList();
                        var alreadySelectedSubProducts = alreadySelectedProduct?.SelectMany(x => x.SubProducts)
                            ?.Where(y => y.Association.SegmentRefIDs[0] == segment.SegmentNumber.ToString())
                            .ToList();
                        var subProducts = product?.SelectMany(x => x.SubProducts)
                            ?.Where(y => y.Prices.Any(z => z.Association.SegmentRefIDs[0] == segment.SegmentNumber.ToString()))
                            .ToList();

                        #region Already purchased meals
                        if (alreadySelectedSubProducts != null && alreadySelectedSubProducts.Count > 0)
                        {
                            double paxOrderSumamry = 0.00;
                            isUpperCabin = (!isUpperCabin) ? IsFreeMeals(alreadySelectedSubProducts, true) : isUpperCabin;
                            landingImgurl = product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "ImageURL_LANDINGPAGE")?.FirstOrDefault()?.Value;
                            var footer = _configuration.GetValue<bool>("EnablePOMFooterText") ? product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "MYTRIPS.POM.POMFOOTER")?.FirstOrDefault()?.Value
                                                                                : string.Empty;

                            MOBInflightMealSegment alreadySelectedSegmet = GetInflightMealSegment(segment, alreadySelectedSubProducts, dynamicOfferResponse, tripNumber, true, segment.BookingClasses, footer);

                            var segmentPassengers = LoadPassengersForAlreadySelected(alreadySelectedSubProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), ref paxOrderSumamry, isUpperCabin);
                            var paxIds = segmentPassengers.Select(x => x.PassengerId).ToList();

                            var otherPassenger = LoadPassengers(subProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), segment.BookingClasses);
                            if (otherPassenger != null)
                            {
                                foreach (var pax in otherPassenger)
                                {
                                    if (!paxIds.Contains(pax.PassengerId))
                                    {
                                        segmentPassengers.Add(pax);
                                    }
                                }
                            }
                            alreadySelectedSegmet.Passengers = segmentPassengers.OrderBy(a => a.PassengerId).ToList();
                            DisableMultiPassengerPurchase(productDetails?.FirstOrDefault()?.InEligibleSegments, alreadySelectedSegmet.Passengers, isUpperCabin, product?.FirstOrDefault()?.Characteristics, inflightMealRequest);
                            if (paxOrderSumamry > 0)
                                alreadySelectedSegmet.OrderSummary = "$" + String.Format("{0:0.00}", paxOrderSumamry);

                            response.FlightSegments.Add(alreadySelectedSegmet);
                            isMealsegmentAdded = true;
                        }
                        #endregion
                        else
                        {
                            isUpperCabin = (!isUpperCabin) ? IsFreeMeals(subProducts) : isUpperCabin;
                            landingImgurl = product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "ImageURL_LANDINGPAGE")?.FirstOrDefault().Value;
                            var footer = _configuration.GetValue<bool>("EnablePOMFooterText") ? product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "MYTRIPS.POM.POMFOOTER")?.FirstOrDefault()?.Value
                                                                              : string.Empty;

                            MOBInflightMealSegment mealSegment = GetInflightMealSegment(segment, subProducts, dynamicOfferResponse, tripNumber, false, segment.BookingClasses, footer);

                            var segmentPassengers = LoadPassengers(subProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), segment.BookingClasses);
                            mealSegment.Passengers = segmentPassengers;
                            response.FlightSegments.Add(mealSegment);
                            isMealsegmentAdded = true;
                        }
                        if (isMealsegmentAdded)
                            break;

                    }



                    if (!isMealsegmentAdded)
                    {
                        if (response.FlightSegments == null)
                            response.FlightSegments = new List<MOBInflightMealSegment>();

                        var inEligibleSegment = GetInflightMealSegment(segment, null, dynamicOfferResponse, tripNumber, false, segment.BookingClasses);
                        inEligibleSegment.InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");
                        inEligibleSegment.IsMealSelctionAvailable = false;
                        inEligibleSegment.SnacksPurchaseLinkText = "";
                        response.FlightSegments.Add(inEligibleSegment);
                        isMealsegmentAdded = true;
                    }

                }
            }

            response.Captions = AddCaptionsForLandingScreen(isUpperCabin, landingImgurl);
            return response;
        }

        private List<MOBInFlightMealPassenger> LoadPassengers(List<Service.Presentation.ProductModel.SubProduct> subProducts, Collection<ProductTraveler> travelers, string segmentid, Collection<BookingClass> bookingClasses)
        {
            List<MOBInFlightMealPassenger> passengers = null;
            bool isFreeMeals = IsFreeMeals(subProducts);
            foreach (var traveler in travelers)
            {
                foreach (var subProduct in subProducts)
                {
                    if (subProduct != null && subProduct.Prices != null && subProduct.Prices.Any(price => price.Association.TravelerRefIDs[0] == traveler.ID && price.Association.SegmentRefIDs[0] == segmentid)
                        )
                    {
                        if (passengers == null)
                            passengers = new List<MOBInFlightMealPassenger>();
                        if (passengers.Where(a => a.PassengerId == traveler.ID).ToList()?.Count == 0)
                        {
                            passengers.Add(new MOBInFlightMealPassenger
                            {
                                AddSnacksLinkText = GetAddSnacksText(isFreeMeals),
                                BeverageEligiblityDesc = "",
                                Captions = null,
                                FullName = string.Format("{0} {1}", traveler.GivenName.ToLower().ToPascalCase(), traveler.Surname.ToLower().ToPascalCase()),
                                IsEligibleForBeverages = true,
                                NotEligibileDefaultText = "",
                                PassengerDesc = "",
                                PassengerId = traveler.ID,
                                SoftDrinksURL = "",
                                SummaryOfPurchases = null
                            });
                        }
                    }
                }
            }
            return passengers;
        }

        private bool IsFreeMeals(List<Service.Presentation.ProductModel.SubProduct> subProducts, bool isAlreadySelected = false)
        {
            if (isAlreadySelected)
            {
                var mealType = subProducts?.FirstOrDefault()?.Extension?.AdditionalExtensions?.FirstOrDefault()?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(mealType) && mealType == InflightMealType.Meal.ToString())
                    return true;
            }
            else
            {
                var mealType = subProducts?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (mealType != null && mealType == InflightMealType.Meal.ToString())
                    return true;
            }
            return false;
        }

        private List<MOBInFlightMealPassenger> LoadPassengersForAlreadySelected(List<Service.Presentation.ProductModel.SubProduct> subProducts, Collection<ProductTraveler> travelers,
            string segmentid, ref double paxOrderSummary, bool isUpperCabin = false)
        {
            List<MOBInFlightMealPassenger> passengers = null;

            double paxSummary = 0;
            List<MOBInflightMealSummary> summary = null;

            foreach (var traveler in travelers)
            {

                if (subProducts.Where(a => a.Association.SegmentRefIDs[0] == segmentid && a.Association.TravelerRefIDs[0] == traveler.ID).Any())
                {
                    summary = new List<MOBInflightMealSummary>();
                    foreach (var alreadySelectedSub in subProducts)
                    {
                        if (passengers == null)
                            passengers = new List<MOBInFlightMealPassenger>();
                        if (alreadySelectedSub.Association.TravelerRefIDs[0] == traveler.ID)
                        {
                            summary = GetSummaryOfPurchases(alreadySelectedSub, ref paxSummary, summary, isUpperCabin);
                            paxOrderSummary += paxSummary;

                        }

                    }
                    passengers.Add(new MOBInFlightMealPassenger
                    {
                        AddSnacksLinkText = null,
                        BeverageEligiblityDesc = "",
                        Captions = null,
                        FullName = string.Format("{0} {1}", traveler.GivenName.ToLower().ToPascalCase(), traveler.Surname.ToLower().ToPascalCase()),
                        IsEligibleForBeverages = true,
                        NotEligibileDefaultText = "",
                        PassengerDesc = "",
                        PassengerId = traveler.ID,
                        SoftDrinksURL = "",
                        SummaryOfPurchases = summary
                    });

                }
            }

            return passengers;
        }

        private async Task<MOBInFlightMealsOfferResponse> MapToMOBInflighMealsResponseV2(DynamicOfferDetailResponse dynamicOfferResponse, Service.Presentation.ReservationModel.Reservation cslReservation,
            string productCode, InflightMealOffersFlowType inflightMealOffersFlowType, MOBInFlightMealsOfferRequest inflightMealRequest)
        {
            var response = new MOBInFlightMealsOfferResponse();
            var notDeparted = new HashSet<string> { "ContactCarrier", "OnSchedule", "EstimatedDepartureOnTime", "EstimatedDepartureEarly", "EstimatedDepartureLate", "NDPT", "NA" };

            if (dynamicOfferResponse == null
                    || dynamicOfferResponse.Offers == null
                    || dynamicOfferResponse.FlightSegments == null
                    || dynamicOfferResponse.FlightSegments.Count == 0
                    || (await _featureSettings.GetFeatureSettingValue("EnablePOM24HourWindow").ConfigureAwait(false)
                    && AllFlightIn24HourWindow(cslReservation.FlightSegments)))
            {
                if (inflightMealOffersFlowType != InflightMealOffersFlowType.TapOnDeeplinkClick)
                    response.Exception = new MOBException() { Code = "9999", Message = "There are no merchandising offers" };
                else if (inflightMealOffersFlowType == InflightMealOffersFlowType.TapOnDeeplinkClick
                            && _shoppingUtility.EnablePOMDeepLinkRedirect(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues))
                {
                    var enableGenericMessageFeature = await _featureSettings.GetFeatureSettingValue("EnableGenericMessageFeature").ConfigureAwait(false);

                    var enablePOMInActivePNR = _shoppingUtility.EnablePOMDeepLinkInActivePNR(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues);
                    response.Captions = AddDODFailureCaptions(enableGenericMessageFeature, enablePOMInActivePNR, cslReservation.FlightSegments, notDeparted);
                }

                return response;
            }

            bool isUpperCabin = false;
            var offersData = dynamicOfferResponse.Offers;
            string landingImgurl = string.Empty;
            response.FlightSegments = new List<MOBInflightMealSegment>();


            if (cslReservation.FlightSegments != null && cslReservation.FlightSegments.Count() > 0)
            {

                #region  the all flight not eligible when the flightsegment data is not returned
                if (dynamicOfferResponse.FlightSegments == null || dynamicOfferResponse.FlightSegments.Count == 0)
                {
                    foreach (var cslSegment in cslReservation.FlightSegments)
                    {

                        var allInEligibleSegments = GetInflightMealSegment(MapToProductFlightSegment(cslSegment), null, dynamicOfferResponse, cslSegment.TripNumber);
                        allInEligibleSegments.InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");
                        allInEligibleSegments.IsMealSelctionAvailable = false;
                        allInEligibleSegments.SnacksPurchaseLinkText = "";
                        response.FlightSegments.Add(allInEligibleSegments);
                    }
                    return response;
                }
                #endregion

                foreach (var segment in dynamicOfferResponse.FlightSegments)
                {

                    bool isMealsegmentAdded = false;
                    string tripNumber = GetTripNumberFromCSLReservation(cslReservation, segment.SegmentNumber, segment.ArrivalAirport.IATACode, segment.DepartureAirport.IATACode);
                    foreach (var offer in dynamicOfferResponse?.Offers)
                    {

                        if (offer == null || offer.ProductInformation == null || offer.ProductInformation.ProductDetails == null
                            || !offer.ProductInformation.ProductDetails.Select(b => b.Product?.SubProducts).Where(c => c.Any(d => d.Association.SegmentRefIDs[0] == segment.ID)).Any())
                            continue;
                        if (response.FlightSegments == null)
                            response.FlightSegments = new List<MOBInflightMealSegment>();

                        var productDetails = offer.ProductInformation?.ProductDetails;
                        var product = productDetails?.Where(x => x.Product != null && x.Product.Code == productCode && x.Product.SubProducts != null)
                            ?.Select(y => y.Product)
                            .ToList();
                        var alreadySelectedProduct = productDetails?.Where(a => a.Product.Code == "PAST_SELECTION")
                            ?.Select(b => b.Product)
                            .ToList();
                        var alreadySelectedSubProducts = alreadySelectedProduct?.SelectMany(x => x.SubProducts)
                            ?.Where(y => y.Association.SegmentRefIDs[0] == segment.SegmentNumber.ToString())
                            .ToList();
                        var subProducts = product?.SelectMany(x => x.SubProducts)
                            ?.Where(y => y.Prices.Any(z => z.Association.SegmentRefIDs[0] == segment.SegmentNumber.ToString()))
                            .ToList();

                        #region Already purchased meals
                        if (alreadySelectedSubProducts != null && alreadySelectedSubProducts.Count > 0)
                        {

                            double paxOrderSumamry = 0.00;
                            isUpperCabin = (!isUpperCabin) ? IsFreeMealsV2(alreadySelectedSubProducts, true) : isUpperCabin;
                            landingImgurl = product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "ImageURL_LANDINGPAGE")?.FirstOrDefault()?.Value;
                            var footer = _configuration.GetValue<bool>("EnablePOMFooterText") ? product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "MYTRIPS.POM.POMFOOTER")?.FirstOrDefault()?.Value
                                                                                              : string.Empty;
                            MOBInflightMealSegment alreadySelectedSegmet = GetInflightMealSegment(segment, alreadySelectedSubProducts, dynamicOfferResponse, tripNumber, true, segment.BookingClasses, footer);

                            var isPreArrivalMarket = _shoppingUtility.EnablePOMPreArrival(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues) && subProducts.Any(x => InflightMealType.Prearrival.ToString().Equals(x.Extension.MealCatalog.MealCategory, StringComparison.OrdinalIgnoreCase) || x.Extension.MealCatalog.Characteristics?.Any(c => c.Code == "MealCategory" && InflightMealType.Prearrival.ToString().Equals(c.Value, StringComparison.OrdinalIgnoreCase)) == true);

                            var segmentPassengers = LoadPassengersForAlreadySelectedV2(alreadySelectedSubProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), ref paxOrderSumamry, isUpperCabin, product?.FirstOrDefault()?.Characteristics, inflightMealRequest, isPreArrivalMarket);

                            var paxIds = segmentPassengers.Select(x => x.PassengerId).ToList();

                            var otherPassenger = LoadPassengersV2(subProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), segment.BookingClasses);
                            if (otherPassenger != null)
                            {
                                foreach (var pax in otherPassenger)
                                {
                                    if (!paxIds.Contains(pax.PassengerId))
                                    {
                                        if (!await _featureSettings.GetFeatureSettingValue("EnablePOM24HourWindow").ConfigureAwait(false) 
                                            && product?.FirstOrDefault()?.Characteristics != null && product?.FirstOrDefault()?.Characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "false")
                                        {
                                            var summaryOfPurchases = new List<MOBInflightMealSummary>();
                                            summaryOfPurchases.Add(new MOBInflightMealSummary
                                            {
                                                Text = _configuration.GetValue<string>("POMIneligibleReasonForBackwardBuilds")
                                            });
                                            segmentPassengers.Add(new MOBInFlightMealPassenger
                                            {
                                                AddSnacksLinkText = null,
                                                BeverageEligiblityDesc = "",
                                                Captions = new List<MOBItem>() { (new MOBItem { Id = "disablePurchase", CurrentValue = "true" }) },
                                                FullName = pax?.FullName,
                                                IsEligibleForBeverages = true,
                                                NotEligibileDefaultText = "",
                                                PassengerDesc = "",
                                                PassengerId = pax.PassengerId,
                                                SoftDrinksURL = "",
                                                SummaryOfPurchases = summaryOfPurchases,
                                                IsEditable = false,
                                                EditText = null
                                            });
                                        }
                                        else if (!_shoppingUtility.EnableEditForAllCabinPOM(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version?.Major, inflightMealRequest?.CatalogValues) && (productDetails?.FirstOrDefault()?.InEligibleSegments == null || productDetails?.FirstOrDefault()?.InEligibleSegments?.Count == 0) && !isUpperCabin) //true
                                        {
                                            var summaryOfPurchases = new List<MOBInflightMealSummary>();
                                            summaryOfPurchases.Add(new MOBInflightMealSummary
                                            {
                                                Text = _configuration.GetValue<string>("POMIneligibleReasonForBackwardBuilds")
                                            });
                                            segmentPassengers.Add(new MOBInFlightMealPassenger
                                            {
                                                AddSnacksLinkText = null,
                                                BeverageEligiblityDesc = "",
                                                Captions = new List<MOBItem>() { (new MOBItem { Id = "disablePurchase", CurrentValue = "true" }) },
                                                FullName = pax?.FullName?.ToUpper(),
                                                IsEligibleForBeverages = true,
                                                NotEligibileDefaultText = "",
                                                PassengerDesc = "",
                                                PassengerId = pax.PassengerId,
                                                SoftDrinksURL = "",
                                                SummaryOfPurchases = summaryOfPurchases,
                                                IsEditable = false,
                                                EditText = null
                                            });
                                        }
                                        else
                                            segmentPassengers.Add(pax);
                                    }
                                }
                            }
                            alreadySelectedSegmet.Passengers = segmentPassengers.OrderBy(a => a.PassengerId).ToList();
                            DisableMultiPassengerPurchase(productDetails?.FirstOrDefault()?.InEligibleSegments, alreadySelectedSegmet.Passengers, isUpperCabin, product?.FirstOrDefault()?.Characteristics, inflightMealRequest);
                            if (paxOrderSumamry > 0)
                                alreadySelectedSegmet.OrderSummary = "$" + String.Format("{0:0.00}", paxOrderSumamry);

                            response.FlightSegments.Add(alreadySelectedSegmet);
                            isMealsegmentAdded = true;
                        }
                        #endregion
                        else
                        {
                            isUpperCabin = (!isUpperCabin) ? IsFreeMealsV2(subProducts) : isUpperCabin;
                            landingImgurl = product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "ImageURL_LANDINGPAGE")?.FirstOrDefault()?.Value;
                            var footer = _configuration.GetValue<bool>("EnablePOMFooterText") ? product?.FirstOrDefault().Characteristics?.Where(a => a.Code == "MYTRIPS.POM.POMFOOTER")?.FirstOrDefault()?.Value
                                                                                              : string.Empty;
                            MOBInflightMealSegment mealSegment = GetInflightMealSegment(segment, subProducts, dynamicOfferResponse, tripNumber, false, segment.BookingClasses, footer);

                            var cslFlightSegment = cslReservation.FlightSegments.FirstOrDefault(s => s.FlightSegment.FlightNumber == mealSegment.FlightNumber && s.FlightSegment.SegmentNumber.ToString() == mealSegment.SegmentId);
                            var flightStatus = cslFlightSegment.Characteristic.FirstOrDefault(c => c.Code == "uflifo-FlightStatus")?.Value;

                            if (_shoppingUtility.EnablePOMFlightEligibilityCheck(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major) && (UtilityHelper.CheckForCheckinEligible(cslFlightSegment) || flightStatus != null && !notDeparted.Contains(flightStatus)))
                            {
                                mealSegment.InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");

                                if (_shoppingUtility.EnablePOMDeepLinkRedirect(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues))
                                {
                                    mealSegment.InEligibleMealText = _configuration.GetValue<string>("POM_Preorder_Window_Closed_Title_Text");
                                }

                                mealSegment.IsMealSelctionAvailable = false;
                                // Add the toggle for enabling the meal selection link when user heven't select any meal before 24 hours window
                                mealSegment.SnacksPurchaseLinkText = await _featureSettings.GetFeatureSettingValue("EnablePOM24HourWindow").ConfigureAwait(false) ? string.Empty : GetSnackPurchaseText(isUpperCabin, false);
                            }
                            else
                            {
                                mealSegment.Passengers = LoadPassengersV2(subProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), segment.BookingClasses);
                            }

                            //var segmentPassengers = LoadPassengersV2(subProducts, dynamicOfferResponse.Travelers, segment.SegmentNumber.ToString(), segment.BookingClasses);
                            //mealSegment.Passengers = segmentPassengers;

                            response.FlightSegments.Add(mealSegment);
                            isMealsegmentAdded = true;
                        }
                        if (isMealsegmentAdded)
                            break;

                    }



                    if (!isMealsegmentAdded)
                    {
                        if (response.FlightSegments == null)
                            response.FlightSegments = new List<MOBInflightMealSegment>();

                        var inEligibleSegment = GetInflightMealSegment(segment, null, dynamicOfferResponse, tripNumber, false, segment.BookingClasses);
                        inEligibleSegment.InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");

                        var cslFlightSegment = cslReservation.FlightSegments.FirstOrDefault(s => s.FlightSegment.FlightNumber == inEligibleSegment.FlightNumber && s.FlightSegment.SegmentNumber.ToString() == inEligibleSegment.SegmentId);
                        if (UtilityHelper.CheckForCheckinEligible(cslFlightSegment)
                                && _shoppingUtility.EnablePOMDeepLinkRedirect(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major, inflightMealRequest.CatalogValues))
                        {
                            inEligibleSegment.InEligibleMealText = _configuration.GetValue<string>("POM_Preorder_Window_Closed_Title_Text");
                        }

                        inEligibleSegment.IsMealSelctionAvailable = false;
                        inEligibleSegment.SnacksPurchaseLinkText = "";
                        response.FlightSegments.Add(inEligibleSegment);
                        isMealsegmentAdded = true;
                    }

                }
            }

            response.Captions = AddCaptionsForLandingScreen(isUpperCabin, landingImgurl);
            //todo
            if (IsTermsAndCondtionsPOMOffer(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version.Major))
            {
                DynamicOfferDetailResponse persistDynamicOfferDetailResponse = await GetCCEOffersFromPersist(inflightMealRequest.SessionId);
                response.TermsNConditions = GetTermsAndConditionsFromCCEResponse(isUpperCabin, persistDynamicOfferDetailResponse);
                AddCaptionsForPremier1K(isUpperCabin, persistDynamicOfferDetailResponse, response);
            }
            return response;
        }

        private bool AllFlightIn24HourWindow(Collection<ReservationFlightSegment> flightsegments)
        {
            foreach (var flightSegment in flightsegments)
            {
                if (!UtilityHelper.CheckForCheckinEligible(flightSegment))
                {
                    return false;
                }
            }

            return true;
        }
        private bool IsFlightDeparted(Collection<ReservationFlightSegment> flightsegments, HashSet<string> notDeparted)
        {
            bool isPNRInActive = false;
            foreach (var flightSegment in flightsegments)
            {
                var flightStatus = flightSegment?.Characteristic?.FirstOrDefault(c => c?.Code != null && c.Code == "uflifo-FlightStatus")?.Value;
                if (!notDeparted.Contains(flightStatus))
                {
                    isPNRInActive = true;
                }
                else
                {
                    isPNRInActive = false;
                    return isPNRInActive;
                }
            }
            return isPNRInActive;
        }

        private void AddCaptionsForPremier1K(bool isUpperCabin, DynamicOfferDetailResponse persistDynamicOfferDetailResponse, MOBInFlightMealsOfferResponse response)
        {
            if (response.Captions == null) response.Captions = new List<MOBItem>();
            if (persistDynamicOfferDetailResponse?.ResponseData != null)
            {
                SDLContent1 gsOr1KMessage = null;
                SDLContentResponseData sdlData = persistDynamicOfferDetailResponse.ResponseData.ToObject<SDLContentResponseData>();
                gsOr1KMessage = GetSDLContentForGSOr1K(sdlData, _configuration.GetValue<string>("GS1KMemberPOMSubKey"));
                response.Captions.Add(new MOBItem { Id = "LND_1K_Header", CurrentValue = gsOr1KMessage?.subtitle });
                response.Captions.Add(new MOBItem { Id = "LND_1K_BodyHeader", CurrentValue = gsOr1KMessage?.title });
                response.Captions.Add(new MOBItem { Id = "LND_1K_BodyImage", CurrentValue = gsOr1KMessage?.image_assets?.FirstOrDefault()?.content?.image_source?.content?.src });
                response.Captions.Add(new MOBItem { Id = "LND_1K_Desc", CurrentValue = gsOr1KMessage?.body });

            }

        }

        private SDLContent1 GetSDLContentForGSOr1K(SDLContentResponseData sdlData, string subKey)
        {
            var content = sdlData?.Body?.SelectMany(a => a.content);
            foreach (var item in content)
            {
                if (item.content != null && item.name != null && item.name == subKey)
                {
                    return item.content;
                }
            }

            return sdlData?.Body?.SelectMany(a => a.content)?.FirstOrDefault(c => c?.name?.Equals(subKey, StringComparison.OrdinalIgnoreCase) ?? false)?.content;
        }

        private List<MOBMobileCMSContentMessages> GetTermsAndConditionsFromCCEResponse(bool isUpperCabin, DynamicOfferDetailResponse persistDynamicOfferDetailResponse)
        {
            var response = new List<MOBMobileCMSContentMessages>();

            if (persistDynamicOfferDetailResponse?.ResponseData == null)
                return null;
            SDLContentResponseData sdlData = persistDynamicOfferDetailResponse.ResponseData.ToObject<SDLContentResponseData>();
            if (isUpperCabin)
            {
                AddUpperCabinTermsAndConditions(response, sdlData);
            }
            else
            {
                AddEconomyTermsAndConditions(response, sdlData);
            }
            return response;
        }

        private SDLContent1 AddEconomyTermsAndConditions(List<MOBMobileCMSContentMessages> response, SDLContentResponseData sdlData)
        {
            SDLContent1 termsAndConditions = GetSDLContent(sdlData, _configuration.GetValue<string>("TermsNConditionsSnackMainKey"), _configuration.GetValue<string>("TermsNConditionsSnackKey"));
            response.Add(new MOBMobileCMSContentMessages
            {
                Title = $"{termsAndConditions?.title}",
                ContentShort = "",
                HeadLine = $"{termsAndConditions?.subtitle}",
                ContentFull = $"{termsAndConditions?.body}"
            });
            termsAndConditions = GetSDLContent(sdlData, _configuration.GetValue<string>("TermsNConditionsSnackMainKey"), _configuration.GetValue<string>("TermsNConditionsSnackFlightChangeKey"));
            response.Add(new MOBMobileCMSContentMessages
            {
                Title = $"{termsAndConditions?.title}",
                ContentShort = "",
                HeadLine = $"{termsAndConditions?.subtitle}",
                ContentFull = $"{termsAndConditions?.body}"
            });
            return termsAndConditions;
        }

        private SDLContent1 AddUpperCabinTermsAndConditions(List<MOBMobileCMSContentMessages> response, SDLContentResponseData sdlData)
        {
            SDLContent1 termsAndConditions = GetSDLContent(sdlData, _configuration.GetValue<string>("TermsNConditionsMealMainKey"), _configuration.GetValue<string>("TermsNConditionsMealKey"));
            response.Add(new MOBMobileCMSContentMessages
            {
                Title = $"{termsAndConditions?.title}",
                ContentShort = _configuration.GetValue<string>("PaymentTnCMessage"),
                HeadLine = $"{termsAndConditions?.subtitle}",
                ContentFull = $"{termsAndConditions?.body}"
            });
            termsAndConditions = GetSDLContent(sdlData, _configuration.GetValue<string>("TermsNConditionsMealMainKey"), _configuration.GetValue<string>("TermsNConditionsMealFlighChangeKey"));
            response.Add(new MOBMobileCMSContentMessages
            {
                Title = $"{termsAndConditions?.title}",
                ContentShort = _configuration.GetValue<string>("PaymentTnCMessage"),
                HeadLine = $"{termsAndConditions?.subtitle}",
                ContentFull = $"{termsAndConditions?.body}"
            });
            return termsAndConditions;
        }

        private SDLContent1 GetSDLContent(SDLContentResponseData sdlData, string mainKey, string subKey)
        {
            return sdlData?.Body
                                                        ?.FirstOrDefault(b => b?.name?.Equals(mainKey, StringComparison.OrdinalIgnoreCase) ?? false)
                                                        ?.content
                                                        ?.FirstOrDefault(c => c?.name?.Equals(subKey, StringComparison.OrdinalIgnoreCase) ?? false)
                                                        ?.content;
        }

        private async Task<DynamicOfferDetailResponse> GetCCEOffersFromPersist(string sessionId)
        {
            if (!_configuration.GetValue<bool>("EnablePOMTermsAndConditions"))
                return await _sessionHelperService.GetSession<DynamicOfferDetailResponse>(sessionId, new DynamicOfferDetailResponse().GetType().FullName, new List<string> { sessionId, new DynamicOfferDetailResponse().GetType().FullName }).ConfigureAwait(false);

            var productOfferCce = new GetOffersCce();
            var productOfferCcePOMSessionValue = productOfferCce.ObjectName + _configuration.GetValue<string>("InflightMealProductCode").ToString();
            productOfferCce = await _sessionHelperService.GetSession<GetOffersCce>(sessionId, productOfferCcePOMSessionValue, new List<string> { sessionId, productOfferCcePOMSessionValue }).ConfigureAwait(false);

            var persistDynamicOfferDetailResponse = string.IsNullOrEmpty(productOfferCce?.OfferResponseJson)
                                    ? null
                                    : Newtonsoft.Json.JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(productOfferCce?.OfferResponseJson);
            return persistDynamicOfferDetailResponse;
        }

        private bool IsTermsAndCondtionsPOMOffer(int appId, string version)
        {
            if (!_configuration.GetValue<bool>("EnablePOMTermsAndConditions")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_POMTermsAnd1KFeatureSupported_AppVersion"), _configuration.GetValue<string>("iPhone_POMTermsAnd1KFeatureSupported_AppVersion"));
        }

        private List<MOBItem> AddCaptionsForLandingScreen(bool isUpperCabin, string landingPageImageURL)
        {
            var captions = new List<MOBItem>();


            captions.Add(new MOBItem { Id = "LND_OrderSummary", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Order_Summary") });
            captions.Add(new MOBItem { Id = "LND_FooterDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Footer_Content") });
            captions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_ReservationDetail_ButtonText") });
            if (isUpperCabin)
            {
                captions.Add(new MOBItem { Id = "LND_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_UpperCabin_Screen_Title") });
                captions.Add(new MOBItem { Id = "LND_Subtitle", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Heading") });
                captions.Add(new MOBItem { Id = "LND_HeaderDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Header_Content") });
                captions.Add(new MOBItem { Id = "LND_UpperCabinImageURL", CurrentValue = landingPageImageURL });
                captions.Add(new MOBItem { Id = "LND_UpperCabinLinkText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Footer_LinkText") });
                captions.Add(new MOBItem { Id = "LND_UpperCabinLink", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_UpperCabinLND_Screen_Footer_Link_URL") });

            }
            else
            {
                captions.Add(new MOBItem { Id = "LND_Title", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Preorder_Refreshments_Screen_Title") });
                captions.Add(new MOBItem { Id = "LND_Subtitle", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Heading") });
                captions.Add(new MOBItem { Id = "LND_HeaderDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Header_Content") });
            }

            return captions;
        }

        private List<MOBItem> AddDODFailureCaptions(bool enableGenericMessageFeature, bool enablePOMInActivePNR, Collection<ReservationFlightSegment> flightsegments, HashSet<string> notDeparted)
        {
            var dodFailureCaptions = new List<MOBItem>();
            if (enablePOMInActivePNR)
            {
                if (enableGenericMessageFeature)
                    dodFailureCaptions.Add(new MOBItem { Id = "LND_POMResponseType", CurrentValue = _configuration.GetValue<string>("POMClosedError") });

                dodFailureCaptions.Add(new MOBItem { Id = "LND_Title", CurrentValue = _configuration.GetValue<string>("POMDeepLinkRedirect_Title_Text_WindowClosed") });
                dodFailureCaptions.Add(new MOBItem { Id = "LND_Subtitle", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Heading_WindowClosed") });
                dodFailureCaptions.Add(new MOBItem { Id = "LND_ErrorScenario_HeaderMessage", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Heading_WindowClosed") });
                dodFailureCaptions.Add(new MOBItem { Id = "LND_HeaderDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Header_Content_DeepLink") });
                if (IsFlightDeparted(flightsegments, notDeparted))
                {
                    dodFailureCaptions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = GetCaptionFromCMSContent("POMDeepLinkRedirect_Button_Text_GotoHome") ?? _configuration.GetValue<string>("POMDeepLinkRedirect_Button_Text_WindowClosed") });
                    dodFailureCaptions.Add(new MOBItem { Id = "LND_IsHomePage", CurrentValue = "true" });
                }
                else
                {
                    dodFailureCaptions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_LND_ReservationDetailButtonText") });
                    dodFailureCaptions.Add(new MOBItem { Id = "LND_IsHomePage", CurrentValue = "false" });
                }
                return dodFailureCaptions;
            }
            dodFailureCaptions.Add(new MOBItem { Id = "LND_Title", CurrentValue = _configuration.GetValue<string>("POMDeepLinkRedirect_Title_Text") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_Subtitle", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Heading") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_ErrorScenario_HeaderMessage", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Heading") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_HeaderDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Header_Content") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = _configuration.GetValue<string>("POMDeepLinkRedirect_Button_Text") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_FooterDesc", CurrentValue = GetCaptionFromCMSContent("InFlightMeal_Screen_Footer_Content") });

            return dodFailureCaptions;
        }

        private List<MOBItem> AddDeepLinkFailureCaptions(bool enableGenericMessageFeature = false,bool isReservation=false)
        {
            var dodFailureCaptions = new List<MOBItem>();
            if (enableGenericMessageFeature)
                dodFailureCaptions.Add(new MOBItem { Id = "LND_POMResponseType", CurrentValue = _configuration.GetValue<string>("DeepLinkError") });

            dodFailureCaptions.Add(new MOBItem { Id = "LND_Title", CurrentValue = _configuration.GetValue<string>("POMDeepLinkRedirect_Title_Text_WindowClosed") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_Subtitle", CurrentValue = enableGenericMessageFeature ? GetCaptionFromCMSContent("InFlightMeal_Screen_Heading") : GetCaptionFromCMSContent("InFlightMeal_Screen_Heading_WindowClosed") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_ErrorScenario_HeaderMessage", CurrentValue = enableGenericMessageFeature ? GetCaptionFromCMSContent("InFlightMeal_Screen_Heading") : GetCaptionFromCMSContent("InFlightMeal_Screen_Heading_WindowClosed") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_HeaderDesc", CurrentValue = enableGenericMessageFeature ? GetCaptionFromCMSContent("InFlightMeal_Screen_Header_Content") : GetCaptionFromCMSContent("InFlightMeal_Screen_Header_Content_DeepLink") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_ReservationDetailButtonText", CurrentValue = isReservation? GetCaptionFromCMSContent("InFlightMeal_LND_ReservationDetailButtonText"): GetCaptionFromCMSContent("POMDeepLinkRedirect_Button_Text_GotoHome") ?? _configuration.GetValue<string>("POMDeepLinkRedirect_Button_Text_WindowClosed") });
            dodFailureCaptions.Add(new MOBItem { Id = "LND_IsHomePage", CurrentValue = isReservation ? "false" : "true" });
            
            return dodFailureCaptions;
        }
        private void DisableMultiPassengerPurchase(Collection<Service.Presentation.ProductResponseModel.InEligibleSegment> inEligibleSegments, List<MOBInFlightMealPassenger> passengers, bool isUpperCabin = false, Collection<Characteristic> characteristics = null, MOBInFlightMealsOfferRequest inflightMealRequest = null)
        {
            if (_configuration.GetValue<bool>("DisableMultiPaxPurchase")
               && inEligibleSegments?.FirstOrDefault()?.Assocatiation?.TravelerRefIDs != null)
            {
                foreach (var passengerName in inEligibleSegments?.FirstOrDefault()?.Assocatiation?.TravelerRefIDs)
                {
                    if (passengers == null) passengers = new List<MOBInFlightMealPassenger>();
                    var summaryOfPurchases = new List<MOBInflightMealSummary>();
                    summaryOfPurchases.Add(new MOBInflightMealSummary { Text = inEligibleSegments?.FirstOrDefault()?.Reason?.Description });
                    passengers.Add(new MOBInFlightMealPassenger
                    {
                        AddSnacksLinkText = null,
                        BeverageEligiblityDesc = "",
                        Captions = new List<MOBItem>() { (new MOBItem { Id = "disablePurchase", CurrentValue = "true" }) },
                        FullName = passengerName,
                        IsEligibleForBeverages = true,
                        NotEligibileDefaultText = "",
                        PassengerDesc = "",
                        PassengerId = null,
                        SoftDrinksURL = "",
                        SummaryOfPurchases = summaryOfPurchases,
                        IsEditable = (_shoppingUtility.EnableEditForAllCabinPOM(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version?.Major, inflightMealRequest?.CatalogValues)) ? (characteristics != null && characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true") : IsRefreshmentEditable(isUpperCabin, (characteristics != null && characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true")),
                        EditText = (_shoppingUtility.EnableEditForAllCabinPOM(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version?.Major, inflightMealRequest?.CatalogValues)) && (characteristics != null && characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true") || IsRefreshmentEditable(isUpperCabin, (characteristics != null && characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true")) ? "Edit" : null
                    });
                }

            }

        }

        private List<MOBInFlightMealPassenger> LoadPassengersV2(List<Service.Presentation.ProductModel.SubProduct> subProducts, Collection<ProductTraveler> travelers, string segmentid, Collection<BookingClass> bookingClasses)
        {
            List<MOBInFlightMealPassenger> passengers = null;
            bool isFreeMeals = IsFreeMealsV2(subProducts);
            foreach (var traveler in travelers)
            {
                foreach (var subProduct in subProducts)
                {
                    if (subProduct != null && subProduct.Prices != null && subProduct.Prices.Any(price => price.Association.TravelerRefIDs[0] == traveler.ID && price.Association.SegmentRefIDs[0] == segmentid))
                    {
                        if (passengers == null)
                            passengers = new List<MOBInFlightMealPassenger>();
                        if (passengers.Where(a => a.PassengerId == traveler.ID).ToList()?.Count == 0)
                        {
                            passengers.Add(new MOBInFlightMealPassenger
                            {
                                AddSnacksLinkText = GetAddSnacksText(isFreeMeals),
                                BeverageEligiblityDesc = "",
                                Captions = null,
                                FullName = string.Format("{0} {1}", traveler.GivenName.ToLower().ToPascalCase(), traveler.Surname.ToLower().ToPascalCase()),
                                IsEligibleForBeverages = true,
                                NotEligibileDefaultText = "",
                                PassengerDesc = "",
                                PassengerId = traveler.ID,
                                SoftDrinksURL = "",
                                SummaryOfPurchases = null
                            });
                        }
                    }
                }
            }
            return passengers;
        }

        private string GetAddSnacksText(bool isFreeMeals)
        {
            if (isFreeMeals)
                return GetCaptionFromCMSContent("InFlightMeal_Screen_SelectMeal");
            else
                return System.Web.HttpUtility.HtmlDecode(GetCaptionFromCMSContent("InFlightMeal_Screen_Add_Snacks"));
        }

        private List<MOBInFlightMealPassenger> LoadPassengersForAlreadySelectedV2(List<Service.Presentation.ProductModel.SubProduct> subProducts, Collection<ProductTraveler> travelers,
          string segmentid, ref double paxOrderSummary, bool isUpperCabin = false, Collection<Characteristic> characteristics = null, MOBInFlightMealsOfferRequest inflightMealRequest = null, bool isPreArrivalMarket = false)
        {
            List<MOBInFlightMealPassenger> passengers = null;
            bool isEditable = false;
            double paxSummary = 0;
            List<MOBInflightMealSummary> summary = null;

            if (characteristics != null && characteristics.Where(a => a.Code == "IsEligibleForEdit")?.FirstOrDefault()?.Value?.ToLower() == "true")
                isEditable = true;
            foreach (var traveler in travelers)
            {

                if (subProducts.Where(a => a.Association.SegmentRefIDs[0] == segmentid && a.Association.TravelerRefIDs[0] == traveler.ID).Any())
                {
                    summary = new List<MOBInflightMealSummary>();
                    foreach (var alreadySelectedSub in subProducts)
                    {
                        if (passengers == null)
                            passengers = new List<MOBInFlightMealPassenger>();
                        if (alreadySelectedSub.Association.TravelerRefIDs[0] == traveler.ID)
                        {
                            summary = GetSummaryOfPurchases(alreadySelectedSub, ref paxSummary, summary, isUpperCabin, isPreArrivalMarket);
                            paxOrderSummary += paxSummary;

                        }

                    }
                    passengers.Add(new MOBInFlightMealPassenger
                    {
                        AddSnacksLinkText = null,
                        BeverageEligiblityDesc = "",
                        Captions = null,
                        FullName = string.Format("{0} {1}", traveler.GivenName.ToLower().ToPascalCase(), traveler.Surname.ToLower().ToPascalCase()),
                        IsEligibleForBeverages = true,
                        NotEligibileDefaultText = "",
                        PassengerDesc = "",
                        PassengerId = traveler.ID,
                        SoftDrinksURL = "",
                        SummaryOfPurchases = summary,
                        IsEditable = (_shoppingUtility.EnableEditForAllCabinPOM(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version?.Major, inflightMealRequest?.CatalogValues)) ? isEditable : IsRefreshmentEditable(isUpperCabin, isEditable),
                        EditText = (_shoppingUtility.EnableEditForAllCabinPOM(inflightMealRequest.Application.Id, inflightMealRequest.Application.Version?.Major, inflightMealRequest?.CatalogValues) && isEditable) || (IsRefreshmentEditable(isUpperCabin, isEditable)) ? "Edit" : null
                    });

                }
            }

            return passengers;
        }

        private bool IsRefreshmentEditable(bool isUpperCabin = false, bool isEditable = false)
        {
            if (isUpperCabin && isEditable)
                return true; //todo based on cce response
            else
                return false;
        }

        private bool EnableEditForAllCabinPOM(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnableisEditablePOMFeature") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("Android_isEditablePOMFeatureSupported_AppVersion"), _configuration.GetValue<string>("IPhone_isEditablePOMFeatureSupported_AppVersion"));
        }
        private bool EnableEditForAllCabinPOM(int appId, string appVersion, List<MOBItem> catalog)
        {
            return _configuration.GetValue<bool>("EnableisEditablePOMFeature") && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, "Android_isEditablePOMFeatureSupported_AppVersion", "IPhone_isEditablePOMFeatureSupported_AppVersion") && _shoppingUtility.CheckClientCatalogForEnablingFeature("POMClientCatalogValues", catalog);
        }

        private List<MOBInflightMealSummary> GetSummaryOfPurchases(Service.Presentation.ProductModel.SubProduct alreadySelectedSub, ref double paxSummary, List<MOBInflightMealSummary> summary, bool isUpperCabin, bool isPreArrivalMarket = false)
        {
            MOBInflightMealSummary inlfightMealSummary = new MOBInflightMealSummary();
            foreach (var additionalExtension in alreadySelectedSub.Extension?.AdditionalExtensions)
            {
                if (additionalExtension.Characteristics.Where(a => a.Code == "Price").FirstOrDefault().Value != "0.00")
                {
                    inlfightMealSummary.Price = additionalExtension.Characteristics.Where(a => a.Code == "Price").FirstOrDefault().Value;
                    inlfightMealSummary.MealCode = additionalExtension.Characteristics.Where(a => a.Code == "MealServiceCode").FirstOrDefault().Value;
                    inlfightMealSummary.Text = additionalExtension.Characteristics.Where(a => a.Code == "MealShortDescription")?.FirstOrDefault()?.Value + " x " + additionalExtension.Characteristics.Where(a => a.Code == "Quantity").FirstOrDefault().Value;
                }
                else
                {
                    if (summary.Count == 0 && isUpperCabin || isPreArrivalMarket)
                    {
                        //Added SPML Category 
                        inlfightMealSummary.Text = (_configuration.GetValue<bool>("EnablePOMSplMeals") && additionalExtension.Characteristics.Where(a => a.Code == "MealCategory")?.FirstOrDefault()?.Value == InflightRefreshementType.SPML.ToString()) ? additionalExtension.Characteristics.Where(a => a.Code == "SPMLCategory")?.FirstOrDefault()?.Value : additionalExtension.Characteristics.Where(a => a.Code == "MealCategory")?.FirstOrDefault()?.Value;
                        inlfightMealSummary.IsBold = isPreArrivalMarket;
                        summary.Add(inlfightMealSummary);
                        inlfightMealSummary = new MOBInflightMealSummary();
                        inlfightMealSummary.Text = additionalExtension.Characteristics.Where(a => a.Code == "MealShortDescription")?.FirstOrDefault()?.Value;
                        inlfightMealSummary.MealCode = additionalExtension.Characteristics.Where(a => a.Code == "MealServiceCode").FirstOrDefault().Value;
                    }
                    else
                    {
                        inlfightMealSummary.Text = additionalExtension.Characteristics.Where(a => a.Code == "MealShortDescription")?.FirstOrDefault()?.Value;
                        inlfightMealSummary.MealCode = additionalExtension.Characteristics.Where(a => a.Code == "MealServiceCode").FirstOrDefault().Value;
                    }
                }
                if (inlfightMealSummary.Price != null)
                {
                    var pricePerItem = Convert.ToDouble(inlfightMealSummary.Price) * Convert.ToDouble(additionalExtension.Characteristics.Where(a => a.Code == "Quantity").FirstOrDefault().Value);
                    if (pricePerItem > 0)
                    {
                        paxSummary = pricePerItem;
                        inlfightMealSummary.Price = "$" + String.Format("{0:0.00}", pricePerItem);
                    }
                }
            }
            summary.Add(inlfightMealSummary);
            return summary;
        }

        private string GetTripNumberFromCSLReservation(United.Service.Presentation.ReservationModel.Reservation cslReservation, int segmentNumber, string arrivalAirportCode, string departureAirportCode)
        {
            return cslReservation.FlightSegments.Where(a => a.SegmentNumber == segmentNumber
                                        && a.FlightSegment.ArrivalAirport.IATACode == arrivalAirportCode && a.FlightSegment.DepartureAirport.IATACode == departureAirportCode).FirstOrDefault()?.TripNumber;
        }

        private ProductFlightSegment MapToProductFlightSegment(ReservationFlightSegment cslSegment)
        {
            ProductFlightSegment segment = new ProductFlightSegment();
            segment.FlightNumber = cslSegment.FlightSegment.FlightNumber;
            segment.ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport() { IATACode = cslSegment.FlightSegment.ArrivalAirport.IATACode, Name = cslSegment.FlightSegment.ArrivalAirport.Name };
            segment.DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport() { IATACode = cslSegment.FlightSegment.DepartureAirport.IATACode, Name = cslSegment.FlightSegment.DepartureAirport.Name };
            segment.TripIndicator = cslSegment.TripNumber;
            segment.SegmentNumber = cslSegment.FlightSegment.SegmentNumber;
            segment.DepartureDateTime = cslSegment.FlightSegment.DepartureDateTime;

            return segment;

        }

        private MOBInflightMealSegment GetInflightMealSegment(ProductFlightSegment segment, List<Service.Presentation.ProductModel.SubProduct> subProducts, DynamicOfferDetailResponse dynamicOfferResponse
            , string tripNumber, bool isAlreadySelected = false, Collection<BookingClass> bookingClasses = null, string footer = "")
        {
            bool isUpperCabin = IsFreeMealsV2(subProducts, isAlreadySelected);
            bool isOfferAvailable = (subProducts == null) ? false : (subProducts.Where(a => a.Prices != null && a.Prices.Count > 0).Count() > 0);
            if (isAlreadySelected || isOfferAvailable)
            {
                return new MOBInflightMealSegment
                {
                    DepartureDate = Convert.ToDateTime(segment.DepartureDateTime).ToString("MM/dd/yyyy HH:mm tt"),
                    Destination = segment.ArrivalAirport.IATACode,
                    FlightNumber = segment.FlightNumber,
                    TripNumber = tripNumber,
                    InEligibleMealText = "",
                    IsMealSelctionAvailable = true,
                    Origin = segment.DepartureAirport.IATACode,
                    OrderSummary = "",
                    Passengers = null,
                    SegmentId = segment.SegmentNumber.ToString(),
                    SnacksPurchaseLinkText = GetSnackPurchaseText(isUpperCabin, isOfferAvailable),
                    Description = GetFlightDescription(segment),
                    Footer = footer

                };
            }
            else
            {
                return new MOBInflightMealSegment
                {
                    DepartureDate = Convert.ToDateTime(segment.DepartureDateTime).ToString("MM/dd/yyyy HH:mm tt"),
                    Destination = segment.ArrivalAirport.IATACode,
                    FlightNumber = segment.FlightNumber,
                    TripNumber = tripNumber,
                    InEligibleMealText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available"),
                    IsMealSelctionAvailable = false,
                    Origin = segment.DepartureAirport.IATACode,
                    OrderSummary = "",
                    Passengers = null,
                    SegmentId = segment.SegmentNumber.ToString(),
                    SnacksPurchaseLinkText = string.Empty,
                    Description = GetFlightDescription(segment),
                    Footer = footer

                };
            }
        }

        private string GetSnackPurchaseText(bool isUpperCabin, bool isOfferAvailable)
        {
            string snacksPurchaseText;
            //if (!isOfferAvailable)
            //    snacksPurchaseText = GetCaptionFromCMSContent("InFlightMeal_Screen_Selection_Not_Available");
            if (isUpperCabin)
                snacksPurchaseText = GetCaptionFromCMSContent("InFlightMeal_EntryScreen_SelectMeal");
            else
                snacksPurchaseText = GetCaptionFromCMSContent("InFlightMeal_Screen_Snacks_For_Purchase");
            return snacksPurchaseText;
        }

        private string GetFlightDescription(ProductFlightSegment segment)
        {
            if (segment == null)
                return "";
            return string.Format("{0} | {1} to {2}", Convert.ToDateTime(segment.DepartureDateTime).ToString("ddd, MMM dd"), segment.DepartureAirport.Name, segment.ArrivalAirport.Name);
        }

        private bool IsFreeMealsV2(List<Service.Presentation.ProductModel.SubProduct> subProducts, bool isAlreadySelected = false)
        {
            if (isAlreadySelected)
            {
                var mealType = subProducts?.FirstOrDefault()?.Extension?.AdditionalExtensions?.FirstOrDefault()?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (!string.IsNullOrEmpty(mealType) && (mealType == InflightMealType.Meal.ToString() || mealType == InflightMealType.NONMEAL.ToString() || mealType == InflightMealType.SPML.ToString()))
                    return true;
            }
            else
            {
                var mealType = subProducts?.FirstOrDefault()?.Extension?.MealCatalog?.Characteristics?.Where(a => a.Code == "MealType")?.FirstOrDefault()?.Value;
                if (mealType != null && (mealType == InflightMealType.Meal.ToString() || mealType == InflightMealType.NONMEAL.ToString() || mealType == InflightMealType.SPML.ToString()))
                    return true;
            }
            return false;
        }

        private bool IsDynamicPOMOffer(int appId, string version)
        {
            if (!_configuration.GetValue<bool>("EnableDynamicPOM")) return false;
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, version, _configuration.GetValue<string>("Android_DynamicPOMFeatureSupported_AppVersion"), _configuration.GetValue<string>("iPhone_DynamicPOMFeatureSupported_AppVersion"));
        }

        private async Task<Service.Presentation.ReservationModel.Reservation> GetCslReservation(string sessionId, bool isbooking)
        {
            if (isbooking)
            {
                var flightReservationResponse = await _sessionHelperService.GetSession<FlightReservationResponse>(sessionId, new FlightReservationResponse().GetType().FullName, new List<string> { sessionId, new FlightReservationResponse().GetType().FullName }).ConfigureAwait(false);
                if (flightReservationResponse != null)
                    return flightReservationResponse.Reservation;
            }
            else
            {
                var reservationDetail = await _sessionHelperService.GetSession<ReservationDetail>(sessionId, new ReservationDetail().GetType().FullName, new List<string> { sessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);
                if (reservationDetail != null)
                    return reservationDetail.Detail;
            }
            return null;
        }

        private async System.Threading.Tasks.Task SetEditFlow(string sessionId = "", MOBInFlightMealsOfferResponse response = null)
        {
            //todo
            var offerResponse = await _sessionHelperService.GetSession<MOBInFlightMealsOfferResponse>(sessionId, new MOBInFlightMealsOfferResponse().ObjectName, new List<string> { sessionId, new MOBInFlightMealsOfferResponse().ObjectName }).ConfigureAwait(false);
            //todo from shopping team
            offerResponse.InflightMealAddOrEditFlow = InflightMealRefreshmentsActionType.TapOnEditLink;
            await _sessionHelperService.SaveSession<MOBInFlightMealsOfferResponse>(offerResponse, sessionId, new List<string> { sessionId, new MOBInFlightMealsOfferResponse().ObjectName }, new MOBInFlightMealsOfferResponse().ObjectName).ConfigureAwait(false);
        }
        private async System.Threading.Tasks.Task ClearSessionForInflightMealOffers(MOBInFlightMealsOfferRequest inflightMealRequest)
        {
            await _sessionHelperService.SaveSession<List<MOBInFlightMealsRefreshmentsResponse>>(null, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsRefreshmentsResponse().ObjectName }, new MOBInFlightMealsRefreshmentsResponse().ObjectName).ConfigureAwait(false);
            await _sessionHelperService.SaveSession<MOBInFlightMealsOfferResponse>(null, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, new MOBInFlightMealsOfferResponse().ObjectName }, new MOBInFlightMealsOfferResponse().ObjectName).ConfigureAwait(false);

        }
        private string GetCaptionFromCMSContent(string key)
        {
            return cmsContents?.Where(x => x.Title.Equals(key))?.FirstOrDefault()?.ContentFull.Trim();
        }

        public async Task<Session> GetSession(MOBRequest mobRequest, string sessionId)
        {
            Session session;
            if (!string.IsNullOrEmpty(sessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(sessionId, false, true);
            }
            else
            {
                session = await _shoppingSessionHelper.CreateShoppingSession(mobRequest.Application?.Id ?? 0, mobRequest.DeviceId,
                    mobRequest.Application?.Version?.Major, mobRequest.TransactionId, string.Empty, string.Empty, false, true);
            }

            return session;
        }
        private async System.Threading.Tasks.Task LoadCMSContents(MOBRequest request, Session session)
        {
            cmsContents = await _fFCShoppingcs.GetSDLContentByGroupName(request, session.SessionId, session.Token, "ManageReservation:Offers", "ManageReservation_Offers_CMSContentMessagesCached_StaticGUID");

        }

        private async System.Threading.Tasks.Task SetBtnTxtChangePermission(MOBInFlightMealsRefreshmentsRequest request, List<MOBInFlightMealsRefreshmentsResponse> saveResponse, DynamicOfferDetailResponse offerDetailResponse)
        {
            var currentPax = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == request.PassengerId && x.SegmentId == request.SegmentId);
            if (offerDetailResponse.Travelers.Count == 1)
            {
                if (currentPax != null)
                {
                    currentPax.IsSaveBtnAllowedInEconomy = (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnEditLink.ToString());
                    //foreach (var restTraveler in offerDetailResponse.Travelers.Where(x => x.ID == request.PassengerId))
                    //{
                    //    GetPastSelection(request, restTraveler, saveResponse, offerDetailResponse);
                    //}
                }
            }
            else
            {
                if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnEditLink.ToString()
                    || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnAddSnacksLink.ToString())
                {
                    // this variable decides if UI is allowed to update button text when the quantity reaches to 0 in each snack page.
                    // if it's true, UI should show "save and sontinue" when quantity == 0, "continue to check out" when quantity != 0.
                    // else UI should always show "continue to checkout"
                    var isSaveBtnAllowedInEconomy = currentPax?.Passenger?.IsEditable == true;
                    // check the selections in rest travelers, excluding current traveler.
                    foreach (var otherTraveler in offerDetailResponse.Travelers.Where(x => x.ID != request.PassengerId))
                    {
                        // in edit or add scenario, when the rest traveler has selected something, UI should show "continue to checkout" no matter if current traveler has selections.
                        if (HasTravelerSelectedAnything(request, otherTraveler, saveResponse, offerDetailResponse))
                        {
                            isSaveBtnAllowedInEconomy = false;
                            break;
                        }
                    }

                    if (currentPax != null)
                    {
                        currentPax.IsSaveBtnAllowedInEconomy = isSaveBtnAllowedInEconomy;
                        //foreach (var restTraveler in offerDetailResponse.Travelers.Where(x => x.ID == request.PassengerId))
                        //{
                        //    GetPastSelection(request, restTraveler, saveResponse, offerDetailResponse);
                        //}
                    }
                }
                else if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString()
                    || request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString())
                {
                    var preOrNextPaxId = "";
                    if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnNextTravelerButton.ToString())
                        preOrNextPaxId = await IsNextTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);
                    else if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnPreviousTravelerButton.ToString())
                        preOrNextPaxId = await IsPreviousTravellerAvailable(Convert.ToInt32(request.PassengerId), request.SessionId, request.SegmentId);

                    // otherPaxResponse is what we are returning for current request
                    var otherPaxResponse = saveResponse.FirstOrDefault(x => x.Passenger.PassengerId == preOrNextPaxId && x.SegmentId == request.SegmentId);
                    var isSaveBtnAllowedInEconomy = otherPaxResponse?.Passenger?.IsEditable == true;
                    // if the request contains any selections, since we are moving to next/previous traveler, UI should always show "continue to checkout"

                    if (request.SelectedRefreshments.Any(x => x.Quantity > 0) == true)
                    {
                        isSaveBtnAllowedInEconomy = false;
                    }
                    else
                    {
                        // in next/previous scenario, we need to check the rest travelers, excluding the traveler in current request and the next/previous travler  
                        foreach (var restTraveler in offerDetailResponse.Travelers.Where(x => x.ID != request.PassengerId && x.ID != preOrNextPaxId))
                        {
                            if (HasTravelerSelectedAnything(request, restTraveler, saveResponse, offerDetailResponse))
                            {
                                isSaveBtnAllowedInEconomy = false;
                                break;
                            }
                        }
                    }

                    if (otherPaxResponse != null)
                    {
                        otherPaxResponse.IsSaveBtnAllowedInEconomy = isSaveBtnAllowedInEconomy;
                        //foreach (var restTraveler in offerDetailResponse.Travelers.Where(x => x.ID != request.PassengerId && x.ID != preOrNextPaxId))
                        //{
                        //    GetPastSelection(request, restTraveler, saveResponse, offerDetailResponse);
                        //}
                    }
                }
            }
        }

        private Boolean HasTravelerSelectedAnything(MOBInFlightMealsRefreshmentsRequest request, ProductTraveler traveler, List<MOBInFlightMealsRefreshmentsResponse> saveResponse, DynamicOfferDetailResponse offerDetailResponse)
        {
            // when the response is null, it means this traveler hasn't been visited in UI, so we need to check the original selection from CCE response,
            // return true if it has selected something;
            // otherwise false;
            var travelerRes = saveResponse.Find(res => res.Passenger.PassengerId == traveler.ID && res.SegmentId == request.SegmentId);
            if (travelerRes == null)
            {
                foreach (var offer in offerDetailResponse.Offers)
                {
                    // check if the CCE response contains valid selections according to traveler's ID and segment ID.
                    // return true when the traveler selected something else false.
                    var product = offer.ProductInformation?.ProductDetails?.FirstOrDefault(d => d.Product.Code == "PAST_SELECTION")?.Product;
                    if (product != null && product.SubProducts != null && product.SubProducts.Any(x => x.Association != null && x.Association.SegmentRefIDs != null && x.Association.SegmentRefIDs.Contains(request.SegmentId) && x.Association.TravelerRefIDs != null && x.Association.TravelerRefIDs.Contains(traveler.ID)))
                    {
                        return true;
                    }
                }
            }
            // return true when the response is not null and if any snack and beverage is selected in the traveler response 
            else if (travelerRes.Snacks.Any(x => x.Quantity > 0) || travelerRes.Beverages.Any(x => x.Quantity > 0))
            {
                return true;
            }

            // return false when nothing is selected 
            return false;
        }

        private Boolean PassengerVerification(List<Service.Presentation.ProductResponseModel.ProductDetail> productDetailList, ProductTraveler traveler, MOBInFlightMealsRefreshmentsRequest request)
        {
            foreach (var productDetail in productDetailList)
            {
                return (productDetail?.Product?.SubProducts?.Any(x => (x.Association?.SegmentRefIDs?.Contains(request.SegmentId) == true) && (x.Association.TravelerRefIDs?.Contains(traveler.ID) == true))) == true;
            }
            return false;
        }

        private int GetPastSelection(MOBInFlightMealsRefreshmentsRequest request, ProductTraveler traveler, List<MOBInFlightMealsRefreshmentsResponse> saveResponse, DynamicOfferDetailResponse offerDetailResponse)
        {
            var travelerRes = saveResponse.Find(res => res.Passenger.PassengerId == traveler.ID && res.SegmentId == request.SegmentId);
            if (travelerRes != null)
            {
                foreach (var offer in offerDetailResponse.Offers)
                {
                    // check if the CCE response contains valid selections according to traveler's ID and segment ID.
                    // return true when the traveler selected something else false.
                    var product = offer.ProductInformation?.ProductDetails?.Where(d => d.Product.Code == "PAST_SELECTION").ToList();
                    if (PassengerVerification(product, traveler, request))
                    {
                        if (travelerRes.Snacks != null)
                        {
                            foreach (var snack in travelerRes.Snacks)
                            {
                                if (snack != null)
                                {
                                    snack.MaxQty = snack.MaxQty + snack.Quantity;
                                }
                            }
                        }
                        // beverage fix for later if any changes made to selection amount
                        //if (travelerRes.Beverages != null)
                        //{
                        //    foreach (var beverage in travelerRes.Beverages)
                        //    {
                        //        if (beverage != null)
                        //        {
                        //            beverage.MaxQty = beverage.MaxQty + beverage.Quantity;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            return 0;
        }


        private bool IsTherePastSelection(MOBInFlightMealsRefreshmentsRequest request, List<MOBInFlightMealsRefreshmentsResponse> saveResponse, DynamicOfferDetailResponse offerDetailResponse)
        {
            if (_shoppingUtility.EnableEditForAllCabinPOM(request.Application.Id, request.Application.Version.Major, request?.CatalogValues))
            {
                if (request.InflightMealRefreshmentsActionType.ToString() == InflightMealRefreshmentsActionType.TapOnSaveNContinue.ToString())
                {
                    return true;
                }
                foreach (var otherTraveler in offerDetailResponse.Travelers.Where(x => x.ID != request.PassengerId))
                {
                    if (HasTravelerSelectedAnything(request, otherTraveler, saveResponse, offerDetailResponse))
                    {
                        return true;
                    }
                }
            }
            return false;
        }



    }

}
