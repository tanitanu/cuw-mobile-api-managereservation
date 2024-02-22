using EmployeeRes.Common;
using MerchandizingServices;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.MerchandizeService;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.ShopProducts;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Fitbit;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.MPSignIn;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Pcu;
using United.Service.Presentation.CommonEnumModel;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.PersonalizationModel;
using United.Service.Presentation.PersonalizationRequestModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.PriceModel;
using United.Service.Presentation.ProductModel;
using United.Service.Presentation.ProductRequestModel;
using United.Service.Presentation.ProductResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Service.Presentation.ValueDocumentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using BagFeesPerSegment = United.Mobile.Model.Common.BagFeesPerSegment;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using Constants = United.Mobile.Model.Constants;
using FlightSegment = United.Service.Presentation.SegmentModel.FlightSegment;
using ItineraryType = MerchandizingServices.ItineraryType;
using Location = MerchandizingServices.Location;
using MOBPriorityBoarding = United.Mobile.Model.MPRewards.MOBPriorityBoarding;
using Offer = United.Service.Presentation.ProductResponseModel.Offer;
using Product = United.Service.Presentation.ProductModel.Product;
using ProductOffer = United.Service.Presentation.ProductResponseModel.ProductOffer;
using Reservation = United.Service.Presentation.ReservationModel.Reservation;
using ReservationType = United.Service.Presentation.CommonEnumModel.ReservationType;
using ServiceType = MerchandizingServices.ServiceType;
using SubscriptionRequest = MerchandizingServices.SubscriptionRequest;

namespace United.Common.Helper.Merchandize
{
    public class MerchandizingServices : IMerchandizingServices
    {
        /// <summary>
        /// TODO::
        /// Once the DynamoDB wrapper service exposed scan and query endpoints we will get the legal documents by title by now by ID
        /// </summary>
        private const string DOTBaggageInfoDBTitle1 = "402";//"DOTBaggageInfoText1";
        private const string DOTBaggageInfoDBTitle1ELF = "403";//"DOTBaggageInfoText1 - ELF";
        private const string DOTBaggageInfoDBTitle1IBE = "740";//"DOTBaggageInfoText1 - IBE";
        private const string DOTBaggageInfoDBTitle2 = "404";//"DOTBaggageInfoText2";
        private const string DOTBaggageInfoDBTitle3 = "405";//"DOTBaggageInfoText3";
        private const string DOTBaggageInfoDBTitle3IBE = "741";//"DOTBaggageInfoText3IBE";
        private const string DOTBaggageInfoDBTitle4 = "406";//"DOTBaggageInfoText4";
        private static readonly List<string> Titles = new List<string>
        {
            DOTBaggageInfoDBTitle1,
            DOTBaggageInfoDBTitle1ELF,
            DOTBaggageInfoDBTitle2,
            DOTBaggageInfoDBTitle3,
            DOTBaggageInfoDBTitle4,
            DOTBaggageInfoDBTitle1IBE,
            DOTBaggageInfoDBTitle3IBE
        };
        private readonly ICacheLog<MerchandizingServices> _logger;
        private List<United.Definition.MOBLegalDocument> cachedLegalDocuments = null;
        private readonly IConfiguration _configuration;
        private readonly IMerchOffersService _merchOffersService;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IPKDispenserService _pKDispenserService;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IDPService _dPService;
        private readonly IPurchaseMerchandizingService _purchaseMerchandizingService;
        private readonly IVendorMerchandizingService _vendorMerchandizingService;
        private readonly IShoppingCcePromoService _shoppingCcePromoService;
        private readonly ICachingService _cachingService;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IHeaders _headers;
        private readonly IFeatureSettings _featureSettings;
        private readonly ICCEDynamicOfferDetailsService _cceDODService;
        private readonly ICCEDynamicOffersService _cceDOService;
        private readonly ICCEAncillaryMyTripsFeedbackService _cceAncillaryMyTripsFeedbackService;
        private List<string> nonBundlesCodes = new List<string> { "AAC", "PAC", "PAS", "PBS", "BAG" };

        public MerchandizingServices(ICacheLog<MerchandizingServices> logger, IConfiguration configuration, IMerchOffersService merchOffersService
            , ISessionHelperService sessionHelperService
            , IHeaders headers
            , IDPService dPService
            , IPurchaseMerchandizingService purchaseMerchandizingService
            , IVendorMerchandizingService vendorMerchandizingService
            , IShoppingCcePromoService shoppingCcePromoService
            , IShoppingUtility shoppingUtility,
            IDynamoDBService dynamoDBService,
            IPKDispenserService pKDispenserService
            , ICachingService cachingService
            , IProductInfoHelper productInfoHelper
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService,
            IFeatureSettings featureSettings
            ,ICCEDynamicOfferDetailsService cceDODService
            ,ICCEDynamicOffersService cceDOService,
            ICCEAncillaryMyTripsFeedbackService cceAncillaryMyTripsFeedbackService
           )
        {
            _logger = logger;
            _configuration = configuration;
            _merchOffersService = merchOffersService;
            _vendorMerchandizingService = vendorMerchandizingService;
            _sessionHelperService = sessionHelperService;
            _dPService = dPService;
            _purchaseMerchandizingService = purchaseMerchandizingService;
            _shoppingCcePromoService = shoppingCcePromoService;
            _shoppingUtility = shoppingUtility;
            _dynamoDBService = dynamoDBService;
            _pKDispenserService = pKDispenserService;
            _cachingService = cachingService;
            _productInfoHelper = productInfoHelper;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _headers = headers;
            _featureSettings = featureSettings;
            _cceDODService = cceDODService;
            _cceDOService = cceDOService;
            _cceAncillaryMyTripsFeedbackService = cceAncillaryMyTripsFeedbackService;
        }

        public async Task<(bool showEPAMsg, List<United.Mobile.Model.Shopping.Booking.MOBBKTrip> trips)> GetEPlusSubscriptionsForBookingSelectedTravelers(List<FlightSegmentType> flightSegmentTypeList, List<TravelerType> travelTypeList, List<United.Mobile.Model.Shopping.Booking.MOBBKTrip> trips)
        {
            bool showEPAMsg = false;
            MerchOffers offers = await GetMerchOffersForSelectedTravelers(flightSegmentTypeList, travelTypeList);
            int segmentId = -1;

            if (offers != null && offers.Itinerary != null && offers.Itinerary[0].MerchandisingInfo != null && offers.Itinerary[0].MerchandisingInfo.Services != null && offers.Itinerary[0].MerchandisingInfo.Services != null)
            {
                foreach (ServiceType serviceType in offers.Itinerary[0].MerchandisingInfo.Services)
                {
                    if (serviceType.Tiers != null)
                    {
                        foreach (TierType tierOBJ in serviceType.Tiers)
                        {
                            if (tierOBJ.Pricing != null)
                            {
                                #region
                                foreach (PriceBreakdownType priceOBJ in tierOBJ.Pricing)
                                {
                                    if (priceOBJ.PaymentOptions != null && priceOBJ.PaymentOptions[0].Price != null && priceOBJ.PaymentOptions[0].Price[0].TotalPrice.amount > 0)
                                    {
                                        showEPAMsg = true;
                                        if (Int32.TryParse(priceOBJ.Id.Substring(0, 1), out segmentId))
                                        {
                                            //segments[segmentId - 1].ShowEPAMessage = true;
                                            int count = 0;
                                            foreach (United.Mobile.Model.Shopping.Booking.MOBBKTrip trip in trips)
                                            {
                                                foreach (United.Mobile.Model.Shopping.Booking.MOBBKFlattenedFlight ff in trip.FlattenedFlights)
                                                {
                                                    foreach (United.Mobile.Model.Shopping.Booking.MOBBKFlight flight in ff.Flights)
                                                    {
                                                        count += 1;
                                                        if (segmentId == count)
                                                        {
                                                            flight.ShowEPAMessage = true;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                    }
                }
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("DOTBaggageGenericExceptionMessage"));
            }

            return (showEPAMsg, trips);
        }

        private async Task<MerchOffers> GetMerchOffersForSelectedTravelers(List<FlightSegmentType> flightSegmentTypeList, List<TravelerType> travelTypeList)
        {
            DOTBaggageCalculatorResponse baggageCalculatorResponse = new DOTBaggageCalculatorResponse(true, _configuration);
            GetMerchandizingOffersOutput offers = null;
            try
            {
                //MerchandizingServicesClient merchClient = new MerchandizingServicesClient();
                MerchandizingServicesClient merchClient = null;
                if (_configuration.GetValue<string>("AssignTimeOutForMerchandizeDOTBaggageServiceCall") != null && Convert.ToBoolean(_configuration.GetValue<string>("AssignTimeOutForMerchandizeDOTBaggageServiceCall")))
                {
                    #region Assigne Time Out Value for Merchantize Engine Call
                    MerchandizingServicesClient merchClient1 = new MerchandizingServicesClient();
                    int timeOutSeconds = _configuration.GetValue<string>("TimeOutSecondsForMerchandizeDOTBaggage") != null ? Convert.ToInt32(_configuration.GetValue<string>("TimeOutSecondsForMerchandizeDOTBaggage").ToString().Trim()) : 7;
                    BasicHttpBinding binding = new BasicHttpBinding();
                    TimeSpan timeout = new TimeSpan(0, 0, timeOutSeconds);
                    if (_configuration.GetValue<bool>("BasicHttpSecurityMode"))
                    {
                        binding.Security.Mode = BasicHttpSecurityMode.Transport;
                        binding.MaxReceivedMessageSize = _configuration.GetValue<long>("maxReceivedMessageSize");
                    }
                    binding.CloseTimeout = timeout;
                    binding.SendTimeout = timeout;
                    binding.ReceiveTimeout = timeout;
                    binding.OpenTimeout = timeout;
                    EndpointAddress endPoint = new EndpointAddress(merchClient1.Endpoint.Address.ToString());
                    merchClient = new MerchandizingServicesClient(binding, endPoint);
                    #endregion
                }
                else
                {
                    merchClient = new MerchandizingServicesClient();
                }
                #region Assign Segment List and Traveler list
                var offerRequest = new GetMerchandizingOffersInput();
                offerRequest.MerchOfferRequest = new MerchOfferRequest();
                RequestCriterionType requestCriterionType = new RequestCriterionType();
                requestCriterionType.Items = flightSegmentTypeList.ToArray();
                requestCriterionType.TravelerInfo = new TravelerInfoType();
                TravelerInfoType travelerInfoType = new TravelerInfoType();
                travelerInfoType.Traveler = travelTypeList.ToArray();
                requestCriterionType.TravelerInfo = travelerInfoType;
                #endregion
                #region
                ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode[] includeExcludeOffersList = new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode[1];
                ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode includeExcludeOffer = new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode();
                includeExcludeOffer.ServiceCode = ServiceFilterGroupTypeServiceCode.EPR;
                includeExcludeOffer.ResultAction = ServiceFilterGroupTypeResultAction.Include;
                //includeExcludeOffer.ServiceSubType = ServiceFilterGroupTypeServiceSubType.NONE; // As per praveen do not pass Sub Type for EPU
                includeExcludeOffer.ServiceSubTypeSpecified = true;
                includeExcludeOffersList[0] = new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode();
                includeExcludeOffersList[0] = includeExcludeOffer;
                requestCriterionType.IncludeExcludeOffers = includeExcludeOffersList;
                #endregion
                #region
                RequestorType requestorType = new RequestorType();
                string channelId = string.Empty;
                string channelName = string.Empty;
                if (_configuration.GetValue<bool>("EnabledMERCHChannels"))
                {
                    string merchChannel = "MOBMYRES";
                    SetMerchandizeChannelValues(merchChannel, ref channelId, ref channelName);
                    requestorType.ChannelId = channelId;
                    requestorType.ChannelName = channelName;
                }
                else
                {
                    requestorType.ChannelId = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID").ToString().Trim();// "401";  //Changed per Praveen Vemulapalli email
                    requestorType.ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName").ToString().Trim();//"MBE";  //Changed per Praveen Vemulapalli email
                }
                //requestorType.CountryCode = "US";
                //requestorType.TicketingCountryCode = "USD";
                //requestorType.CurrencyCode = "USD";
                #endregion
                offerRequest.MerchOfferRequest.RequestCriterion = requestCriterionType;
                offerRequest.MerchOfferRequest.Requestor = requestorType;
                //string resquestXML = Serialization.Serialize(offerRequest, typeof(MerchOfferRequest));
                offerRequest.MerchOfferRequest.TransactionIdentifier = "Mobile_Request_With_Org_Dest";

                _logger.LogInformation("GetEPlusSubscriptionsForSelectedTravelers {Request}", offerRequest.MerchOfferRequest);
                offers = await _merchOffersService.GetOffers(offerRequest);
                // offers = await merchClient.GetMerchandizingOffersAsync(offerRequest.MerchOfferRequest).ConfigureAwait(false);
            }
            catch (System.ServiceModel.FaultException ex)
            {
                throw ex;
            }
            _logger.LogInformation("GetEPlusSubscriptionsForSelectedTravelers {Response}", offers);

            return offers.MerchOffers;
        }


        public async Task<MOBUASubscriptions> GetEPlusSubscriptions(string mpAccountNumber, int applicationID, string sessionID)
        {
            string requestXML = string.Empty;
            Mobile.Model.Common.MOBUASubscriptions objUASubscriptionsList = null;
            MerchSubscriptions subscriptionsResponse = new MerchSubscriptions();
            try
            {
                try
                {
                    #region Assigne Time Out Value for Merchandize Engine Call
                    MerchandizingServicesClient merchClient = null;
                    if (_configuration.GetValue<bool>("AssignTimeOutForMerchandizeGetUASubscriptionsCall"))
                    {
                        #region Assigne Time Out Value for Merchandize Engine Call
                        MerchandizingServicesClient merchClient1 = new MerchandizingServicesClient();
                        int timeOutSeconds = _configuration.GetValue<string>("TimeOutSecondsForMerchandizeGetUASubscriptionsCall") != null ? (_configuration.GetValue<int>("TimeOutSecondsForMerchandizeGetUASubscriptionsCall")) : 60;
                        BasicHttpBinding binding = new BasicHttpBinding();
                        TimeSpan timeout = new TimeSpan(0, 0, timeOutSeconds);
                        binding.CloseTimeout = timeout;
                        binding.SendTimeout = timeout;
                        binding.ReceiveTimeout = timeout;
                        binding.OpenTimeout = timeout;
                        EndpointAddress endPoint = new EndpointAddress(merchClient1.Endpoint.Address.ToString());
                        merchClient = new MerchandizingServicesClient(binding, endPoint);
                        #endregion
                    }
                    else
                    {
                        merchClient = new MerchandizingServicesClient();
                    }
                    #endregion
                    #region Define SubscriptionRequest
                    SubscriptionRequest objSubscriptionRequest = new SubscriptionRequest();

                    SubscriptionStatusType[] objSubscriptionStatusTypes = new SubscriptionStatusType[1];
                    objSubscriptionRequest.Filters = new FiltersType();
                    objSubscriptionStatusTypes[0] = SubscriptionStatusType.ACTIVE;
                    objSubscriptionRequest.Filters.Status = objSubscriptionStatusTypes;
                    objSubscriptionRequest.MemberShipID = mpAccountNumber;
                    objSubscriptionRequest.Requestor = new RequestorType();

                    RequestorType requestorType = new RequestorType();

                    string channelId = string.Empty;
                    string channelName = string.Empty;
                    if (_configuration.GetValue<bool>("EnabledMERCHChannels"))
                    {
                        string merchChannel = "MOBBE";
                        SetMerchandizeChannelValues(merchChannel, ref channelId, ref channelName);
                        requestorType.ChannelId = channelId;
                        requestorType.ChannelName = channelName;
                    }
                    else
                    {
                        requestorType.ChannelId = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID")?.Trim();
                        requestorType.ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName")?.Trim();
                        requestorType.ChannelId = "";
                        requestorType.ChannelName = "";
                    }
                    requestorType.CountryCode = "US";
                    requestorType.CurrencyCode = "USD";
                    requestorType.TicketingCountryCode = "USD";
                    requestorType.LangCode = RequestorTypeLangCode.enUS;
                    requestorType.LangCodeSpecified = true;
                    objSubscriptionRequest.Requestor = requestorType;

                    _logger.LogInformation("GetEPlusSubscriptions Request {@Request}", JsonConvert.SerializeObject(objSubscriptionRequest));

                    #endregion
                    #region
                    MerchManagementInput merchManagementInput = new MerchManagementInput { SubscriptionRequest = objSubscriptionRequest };
                    var merchOut = await _merchOffersService.GetSubscriptions(merchManagementInput);
                    subscriptionsResponse = merchOut.MerchSubscriptions;
                    _logger.LogInformation("GetEPlusSubscriptions Response from ME {@Response}", JsonConvert.SerializeObject(merchOut));

                    string responseXML = XmlSerializerHelper.Serialize<MerchSubscriptions>(subscriptionsResponse);
                    if (subscriptionsResponse.Subscription != null)
                    {
                        objUASubscriptionsList = new MOBUASubscriptions();
                        objUASubscriptionsList.MPAccountNumber = mpAccountNumber;
                        objUASubscriptionsList.SubscriptionTypes = new List<MOBUASubscription>();
                        foreach (ManageSubscriptionType objSubscriptionType in subscriptionsResponse.Subscription)
                        {
                            if (objSubscriptionType.SubscriptionInfo?.Status != null && objSubscriptionType.SubscriptionInfo?.Status.Trim().ToUpper() == "ACTIVE" && objSubscriptionType.SubscriptionInfo?.SubscriptionType.Trim().ToUpper() == "SEP")
                            {
                                objUASubscriptionsList.SubscriptionTypes.Add(GetEPlusSubscriptionDetails(objSubscriptionType));
                                continue;
                            }
                        }
                    }
                    #endregion
                }
                catch (System.ServiceModel.FaultException ex)
                {
                    if (ex != null && ex.Message != null)
                    {
                        //string responseErrorXML = ex.Message;
                        _logger.LogError("MerchandizingService GetEPlusSubscriptions FaultException {@Exception}", JsonConvert.SerializeObject(ex.Message));
                    }
                }
            }
            catch (System.Exception ex)
            {
                if (ex != null && ex.Message != null)
                {
                    //string responseErrorXML = ex.Message;
                    _logger.LogError("MerchandizingService GetEPlusSubscriptions {@Exception}", JsonConvert.SerializeObject(ex.Message));
                }
            }

            return objUASubscriptionsList;
        }

        public void SetMerchandizeChannelValues(string merchChannel, ref string channelId, ref string channelName)
        {
            channelId = string.Empty;
            channelName = string.Empty;

            if (merchChannel != null)
            {
                switch (merchChannel)
                {
                    case "MOBBE":
                        channelId = _configuration.GetValue<string>("MerchandizeOffersServiceMOBBEChannelID").Trim();
                        channelName = _configuration.GetValue<string>("MerchandizeOffersServiceMOBBEChannelName").Trim();
                        break;
                    case "MOBMYRES":
                        channelId = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelID").Trim();
                        channelName = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelName").Trim();
                        break;
                    case "MOBWLT":
                        channelId = _configuration.GetValue<string>("MerchandizeOffersServiceMOBWLTChannelID").Trim();
                        channelName = _configuration.GetValue<string>("MerchandizeOffersServiceMOBWLTChannelName").Trim();
                        break;
                    default:
                        break;
                }
            }
        }

        private MOBUASubscription GetEPlusSubscriptionDetails(ManageSubscriptionType objSubscriptionType)
        {
            #region Get EPlus Subscription
            MOBUASubscription objUASubscription = new MOBUASubscription();
            objUASubscription.Items = new List<MOBItem>();

            var item1 = new MOBItem();
            item1.Id = "Subscription";
            item1.CurrentValue = "Economy Plus subscription";

            var item2 = new MOBItem();
            item2.Id = "Expiration";
            item2.CurrentValue = objSubscriptionType.SubscriptionInfo.ExpirationDate;

            var item3 = new MOBItem();
            item3.Id = "Auto-renew";
            item3.CurrentValue = "Off";
            if (objSubscriptionType.AutoRenewDetails != null && objSubscriptionType.AutoRenewDetails.AutoRenew != null && objSubscriptionType.AutoRenewDetails.AutoRenew.Trim().ToUpper() == "TRUE")
            {
                item3.CurrentValue = "On";
            }
            MOBItem item4 = null;
            MOBItem item5 = null;
            MOBItem item6 = null;
            if (objSubscriptionType.SubscriptionInfo.Feature != null)
            {
                #region
                foreach (FeatureType objFeature in objSubscriptionType.SubscriptionInfo.Feature)
                {
                    if (objFeature.Type == FeatureTypeType.REGION)
                    {
                        item4 = new MOBItem();
                        item4.Id = "EplusSubscribeRegion";
                        item4.CurrentValue = objFeature.DisplayName;//"Global";
                    }
                    else if (objFeature.Type == FeatureTypeType.BENEFICIARY)
                    {
                        item5 = new MOBItem();
                        item5.Id = "EplusSubscribeType";
                        item5.CurrentValue = objFeature.Name;//"Subscriber";

                        item6 = new MOBItem();
                        item6.Id = "EPlusSubscribeCompanionCount";
                        item6.CurrentValue = objFeature.Value != null ? objFeature.Value : "0";//"Subscriber";
                    }
                }
                #endregion
            }
            objUASubscription.Items.Add(item1);
            objUASubscription.Items.Add(item2);
            objUASubscription.Items.Add(item3);
            if (item4 != null)
            {
                objUASubscription.Items.Add(item4);
            }
            if (item5 != null)
            {
                objUASubscription.Items.Add(item5);
            }
            if (item6 != null)
            {
                objUASubscription.Items.Add(item6);
            }
            #endregion
            return objUASubscription;
        }

        public async Task<ProductOffer> GetMerchOffersDetails(Session session, Service.Presentation.ReservationModel.Reservation cslReservation, MOBRequest mobRequest, string flow, MOBPNR pnrResponse,string productCode="")
        {
            GetOffers response = null;
            try
            {
                string token = session.Token;
                if (productCode != "SFC")
                {
                    if (session == null || cslReservation == null || mobRequest == null || pnrResponse.IsFareLockOrNRSA == true || (!_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && IsWaitListPNRFromCharacteristics(cslReservation.FlightSegments)))
                        return null;
                    #region WI #2377 -code for schedule change check
                    var isScheduledChanged = GetHasScheduledChanged(pnrResponse.Segments);
                    if (isScheduledChanged)
                    {
                        return null;
                    }
                    #endregion
                }
                else
                {
                    if(IsNRSA(cslReservation))
                    {
                        return null;
                    }
                }
               
                var request = BuildMerchOffersRequest(cslReservation, flow,productCode);

                if (request == null)
                {
                    return response = null;
                }

                string jsonRequest = JsonConvert.SerializeObject(request);

                await _sessionHelperService.SaveSession<United.Service.Presentation.ProductRequestModel.ProductOffer>(request, _headers.ContextValues.SessionId, new List<string> { _headers.ContextValues.SessionId, request.GetType().FullName }, request.GetType().FullName).ConfigureAwait(false);

                response = await _purchaseMerchandizingService.GetMerchOfferInfo<GetOffers>(token, "getoffers", jsonRequest, _headers.ContextValues.SessionId).ConfigureAwait(false);

                if (response != null)
                {
                    if (response != null && response.Offers != null && response.Response.Error == null)
                    {
                        await _sessionHelperService.SaveSession<GetOffers>(response, session.SessionId, new List<string> { session.SessionId, response.ObjectName }, response.ObjectName).ConfigureAwait(false);
                    }
                    else
                        response = null;
                }
                else
                    response = null;

            }
            catch (MOBUnitedException coex)
            {
                response = null;
                _logger.LogInformation("GetMerchOffersDetails {ApplicationId} {ApplicationVersion} {DeviceId} {MOBUnitedException} {TransactionId}", mobRequest.Application.Id, mobRequest.Application.Version.Major, mobRequest.DeviceId, coex.Message, mobRequest.TransactionId);
            }
            catch (System.Exception ex)
            {
                response = null;
                _logger.LogInformation("GetMerchOffersDetails {ApplicationId} {ApplicationVersion} {DeviceId} {Exception} {TransactionId}", mobRequest.Application.Id, mobRequest.Application.Version.Major, mobRequest.DeviceId, ex.Message, mobRequest.TransactionId);
            }

            return response;
        }
        private bool IsNRSA(Service.Presentation.ReservationModel.Reservation reservationDetail)
        {
            foreach (var traveler in reservationDetail.Travelers)
            {
                if (traveler.Tickets != null)
                {
                    foreach (var ticket in traveler.Tickets)
                    {
                        string passengerindicator = GetCharactersticValue(ticket.Characteristic, "PassengerIndicator");

                        if (!string.IsNullOrEmpty(passengerindicator))
                        {
                            if (string.Equals(passengerindicator, "SA", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(passengerindicator, "SJ", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(passengerindicator, "SK", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(passengerindicator, "PS", StringComparison.OrdinalIgnoreCase))
                            {
                                return true;
                            }

                        }
                    }
                }
            }

            return false;
        }
        private bool GetHasScheduledChanged(List<MOBPNRSegment> segments)
        {
            if (segments != null && segments.Count > 0)
            {
                foreach (var segment in segments)
                {
                    if (_configuration.GetValue<string>("ScheduleChangeCodes") != null)
                    {
                        string[] schChangeCodes = _configuration.GetValue<string>("ScheduleChangeCodes").Split(',');
                        foreach (string schChangeCode in schChangeCodes)
                        {
                            if (segment.ActionCode.Contains(schChangeCode))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private United.Service.Presentation.ProductRequestModel.ProductOffer BuildMerchOffersRequest(Service.Presentation.ReservationModel.Reservation cslReservation, string flow,string productCode="")
        {
            if (cslReservation == null)
                return null;

            string ticketPriceRevenue = "0";
            string ticketPriceAward = "0";
            string currencyCode = string.Empty;

            #region ticket price with tax including revenue and award 
            GetTKTPrice(cslReservation.Characteristic, ref ticketPriceRevenue, ref ticketPriceAward, ref currencyCode);
            if (Convert.ToDouble(ticketPriceRevenue) <= 0)
            {
                throw new MOBUnitedException("Ticket price should be greater than zero");
            }
            #endregion
            var countryCode = TicketingCountryCode(cslReservation.PointOfSale);
            var isAwardTravel = IsAward(cslReservation.Type);

            var offer = new United.Service.Presentation.ProductRequestModel.ProductOffer
            {
                Characteristics = BuildCharacteristics(ticketPriceRevenue, isAwardTravel),
                CurrencyCode = currencyCode, //currency code should be the same as ticketing currency code
                CountryCode = countryCode,
                TicketingCountryCode = countryCode,
                Requester = Requester(),
                Filters = ProductFilters(flow,productCode),
                IsAwardReservation = isAwardTravel.ToString(),
                FlightSegments = ProductFlightSegments(cslReservation.FlightSegments, cslReservation.Prices),
                Solutions = Solutions(cslReservation.FlightSegments),
                ODOptions = ProductOriginDestinationOptions(cslReservation.FlightSegments),
                ReservationReferences = ReservationReferences(cslReservation),
                Travelers = ProductTravelers_CFOP(cslReservation.Travelers)
            };

            return offer;
        }

        private Collection<ProductTraveler> ProductTravelers_CFOP(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers, string productCode = "")
        {
            if (travelers == null || !travelers.Any())
                return null;

            var i = 0;
            return travelers.Select(t => new ProductTraveler
            {
                DateOfBirth = t.Person.DateOfBirth,
                GivenName = t.Person.GivenName,
                ID = (++i).ToString(),
                IsSelected = true.ToString(),
                LoyaltyProgramProfile = t.LoyaltyProgramProfile,
                PassengerTypeCode = t.Person.Type,
                ReservationIndex = t.Person.Key,
                Sex = t.Person.Sex,
                Surname = t.Person.Surname,
                TicketingDate = TicketingDate(t.Tickets),
                TicketNumber = TicketedNumber(t.Tickets),
                TravelerNameIndex = t.Person.Key,
                Documents = Documents(t.Person.Documents),
                ProductLoyaltyProgramProfile = ProductLoyaltyProgramProfile(t.LoyaltyProgramProfile),
                Characteristics = (IsPOMOffer(productCode)) ? new Collection<Characteristic>() { new Characteristic { Code = "IsUnaccompaniedMinor" ,
                    Value =GetIsUNMRField(t)} } : null
            }).ToCollection();
        }

        private string GetIsUNMRField(United.Service.Presentation.ReservationModel.Traveler t)
        {
            return (string.IsNullOrEmpty(t.IsUnaccompaniedMinor)) ? "false" : t.IsUnaccompaniedMinor;
        }

        private Collection<ProductTravelerLoyaltyProfile> ProductLoyaltyProgramProfile(LoyaltyProgramProfile loyaltyProgramProfile)
        {
            if (loyaltyProgramProfile == null || string.IsNullOrWhiteSpace(loyaltyProgramProfile.LoyaltyProgramCarrierCode) || string.IsNullOrWhiteSpace(loyaltyProgramProfile.LoyaltyProgramMemberID))
                return null;

            return new Collection<ProductTravelerLoyaltyProfile>
                    {
                        new ProductTravelerLoyaltyProfile
                        {
                            LoyaltyProgramCarrierCode = loyaltyProgramProfile.LoyaltyProgramCarrierCode,
                            LoyaltyProgramMemberID = loyaltyProgramProfile.LoyaltyProgramMemberID
                        }
                    };
        }

        private Collection<United.Service.Presentation.PersonModel.Document> Documents(Collection<United.Service.Presentation.PersonModel.Document> documents)
        {
            if (documents == null || !documents.Any())
                return null;

            var ktnDocument = documents.Where(h => h != null && h.KnownTravelerNumber != null).FirstOrDefault();
            if (ktnDocument == null || string.IsNullOrEmpty(ktnDocument.KnownTravelerNumber))
                return null;

            return new Collection<United.Service.Presentation.PersonModel.Document> { new United.Service.Presentation.PersonModel.Document { Type = DocumentType.NexusCard, KnownTravelerNumber = ktnDocument.KnownTravelerNumber } };
        }


        private string TicketedNumber(Collection<ValueDocument> tickets)
        {
            if (tickets.IsNullOrEmpty()) return null;

            var eTicket = tickets.FirstOrDefault(t => t.Type == "ETicketNumber");
            return eTicket == null ? null : eTicket.DocumentID;
        }

        private string TicketingDate(Collection<ValueDocument> tickets)
        {
            if (tickets.IsNullOrEmpty()) return DefaultDate;

            var eTicket = tickets.FirstOrDefault(t => t.Type == "ETicketNumber");
            return eTicket == null ? DefaultDate : eTicket.IssueDate;
        }

        private string DefaultDate
        {
            get { return new DateTime(0001, 01, 01).ToString(CultureInfo.InvariantCulture); }
        }

        private Collection<ReservationReference> ReservationReferences(Service.Presentation.ReservationModel.Reservation reservation, bool isCCE = false, bool isBulk = false, string productCode = "", bool isCCEDORequest = false)
        {
            if (string.IsNullOrEmpty(reservation.ConfirmationID) || string.IsNullOrEmpty(reservation.CreateDate))
            {
                return null;
            }
            return new Collection<ReservationReference> { new ReservationReference {
                        ID = reservation.ConfirmationID,
                        PaxFares = reservation.Prices,
                        Remarks = (reservation.Remarks != null) ? reservation.Remarks.Select(x => new Remark { Description = x.Description, DisplaySequence = x.DisplaySequence }).ToCollection() : null,
                        SpecialServiceRequests = reservation.Services.Select(x => new United.Service.Presentation.CommonModel.Service { Description = x.Description }).ToCollection(),
                        ReservationType = (IsPOMOffer(productCode) ? GetReservationType(reservation, isCCE, isBulk) : ( (reservation.Type.ToList().Exists(p => p.Description == "GROUP")) ? United.Service.Presentation.CommonEnumModel.ReservationType.GroupBooking : (isCCE & isBulk ? United.Service.Presentation.CommonEnumModel.ReservationType.Bulk : United.Service.Presentation.CommonEnumModel.ReservationType.None))),
                        ReservationCreateDate = (IsPOMOffer(productCode)) || isCCEDORequest ? reservation.CreateDate : null
                } };
        }

        private United.Service.Presentation.CommonEnumModel.ReservationType GetReservationType(Reservation reservation, bool isCCE, bool isBulk)
        {
            if ((reservation.Type.ToList().Exists(p => p.Description == "GROUP")))
                return ReservationType.GroupBooking;
            else if (isCCE & isBulk)
                return ReservationType.Bulk;
            else if (reservation.Type.Where(a => a.Description == "ITIN_TYPE")?.FirstOrDefault().Key == "NONREVENUE")
                return ReservationType.NonRevenue;
            else
                return ReservationType.None;

        }

        private Collection<ProductOriginDestinationOption> ProductOriginDestinationOptions(Collection<ReservationFlightSegment> flightSegments)
        {
            var segmentsWithOd = flightSegments.GroupBy(f => f.TripNumber);
            return segmentsWithOd
                    .Select(od => new ProductOriginDestinationOption()
                    {
                        ID = "OD" + od.Key,
                        FlightSegments = od.Select(q => new ProductFlightSegment { RefID = q.FlightSegment.SegmentNumber.ToString() }).ToCollection()
                    }).ToCollection();
        }

        private Collection<ODOption> PopulateODOptions(Collection<ReservationFlightSegment> flightSegments)
        {
            var segmentsWithOd = flightSegments.GroupBy(f => f.TripNumber);



            return segmentsWithOd
                     .Select(od => new ODOption()
                     {
                         ID = "OD" + od.Key,
                         FlightSegments = od.Select(q => new ProductFlightSegment { RefID = q.FlightSegment.SegmentNumber.ToString() }).ToCollection()
                     }).ToCollection();
        }

        public Collection<Solution> Solutions(Collection<ReservationFlightSegment> flightSegments)
        {
            var segmentsWithOd = flightSegments.GroupBy(f => f.TripNumber);
            return new Collection<Solution>
            {
                new Solution
                {
                    ID = "SOL1",
                    ODOptions = segmentsWithOd.Select(od => new ProductOriginDestinationOption() {RefID = "OD" + od.Key}).ToCollection()
                }
            };
        }

        public Collection<ProductFlightSegment> ProductFlightSegments(Collection<ReservationFlightSegment> flightSegments, Collection<Price> prices, string productCode = "")
        {
            if (_configuration.GetValue<bool>("EnableTravelOptionsInViewRes"))
            {
                return flightSegments.Select(f => BuildProductFlightSegments(prices, f, productCode)).ToCollection(); 
            }
            else
            {
                return _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") ?
                       flightSegments.Select(f => ProductFlightSegmentWLPNR(prices, f, productCode)).ToCollection() :
                       flightSegments.Select(f => ProductFlightSegment(prices, f, productCode)).ToCollection();
            }
        }

        private ProductFlightSegment ProductFlightSegment(Collection<Price> prices, ReservationFlightSegment flightSegment, string productCode = "")
        {
            return new ProductFlightSegment
            {
                ArrivalAirport = flightSegment.FlightSegment.ArrivalAirport,
                ArrivalDateTime = flightSegment.FlightSegment.ArrivalDateTime,
                BookingClasses = flightSegment.FlightSegment.BookingClasses,
                Characteristic = flightSegment.FlightSegment.Characteristic,
                ClassOfService = flightSegment.FlightSegment.BookingClasses[0].Code,
                DepartureAirport = flightSegment.FlightSegment.DepartureAirport,
                DepartureDateTime = flightSegment.FlightSegment.DepartureDateTime,
                FareBasisCode = ShopStaticUtility.GetFareBasisCode(prices, flightSegment.FlightSegment.SegmentNumber),
                FlightNumber = flightSegment.FlightSegment.FlightNumber,
                IsActive = (!ShopStaticUtility.IsUsed(prices, flightSegment.FlightSegment.SegmentNumber)).ToString(),
                ID = flightSegment.FlightSegment.SegmentNumber.ToString(),
                IsInternational = flightSegment.FlightSegment.IsInternational,
                IsConnection = flightSegment.IsConnection,
                MarketedFlightSegment = flightSegment.FlightSegment.MarketedFlightSegment,
                OperatingAirlineCode = flightSegment.FlightSegment.OperatingAirlineCode,
                SegmentNumber = flightSegment.FlightSegment.SegmentNumber,
                TripIndicator = flightSegment.TripNumber,
                Equipment = (IsPOMOffer(productCode)) ? new United.Service.Presentation.CommonModel.AircraftModel.Aircraft
                {
                    CabinCount = (flightSegment.FlightSegment.Equipment != null) ? flightSegment.FlightSegment.Equipment.CabinCount : 0,
                    Model = new United.Service.Presentation.CommonModel.AircraftModel.AircraftModel
                    {
                        Description = flightSegment.FlightSegment.Equipment.Model.Description,
                        Fleet = flightSegment.FlightSegment.Equipment.Model.Fleet,
                        Key = flightSegment.FlightSegment.Equipment.Model.Key
                    }
                } : null
            };
        }

        private ProductFlightSegment ProductFlightSegmentWLPNR(Collection<Price> prices, ReservationFlightSegment flightSegment, string productCode = "")
        {
            return new ProductFlightSegment
            {
                ArrivalAirport = flightSegment.FlightSegment.ArrivalAirport,
                ArrivalDateTime = flightSegment.FlightSegment.ArrivalDateTime,
                BookingClasses = flightSegment.FlightSegment.BookingClasses,
                Characteristic = flightSegment.FlightSegment.Characteristic,
                ClassOfService = flightSegment.FlightSegment.BookingClasses[0].Code,
                DepartureAirport = flightSegment.FlightSegment.DepartureAirport,
                DepartureDateTime = flightSegment.FlightSegment.DepartureDateTime,
                FareBasisCode = ShopStaticUtility.GetFareBasisCode(prices, flightSegment.FlightSegment.SegmentNumber),
                FlightNumber = flightSegment.FlightSegment.FlightNumber,
                FlightSegmentType = flightSegment.FlightSegment.FlightSegmentType,
                IsActive = (!ShopStaticUtility.IsUsed(prices, flightSegment.FlightSegment.SegmentNumber)).ToString(),
                ID = flightSegment.FlightSegment.SegmentNumber.ToString(),
                IsInternational = flightSegment.FlightSegment.IsInternational,
                IsConnection = flightSegment.IsConnection,
                MarketedFlightSegment = flightSegment.FlightSegment.MarketedFlightSegment,
                OperatingAirlineCode = flightSegment.FlightSegment.OperatingAirlineCode,
                SegmentNumber = flightSegment.FlightSegment.SegmentNumber,
                TripIndicator = flightSegment.TripNumber,
                Equipment = (IsPOMOffer(productCode)) ? new United.Service.Presentation.CommonModel.AircraftModel.Aircraft
                {
                    CabinCount = (flightSegment.FlightSegment.Equipment != null) ? flightSegment.FlightSegment.Equipment.CabinCount : 0,
                    Model = (flightSegment.FlightSegment.Equipment?.Model != null) ? (new United.Service.Presentation.CommonModel.AircraftModel.AircraftModel
                    {
                        Description = flightSegment.FlightSegment.Equipment.Model.Description,
                        Fleet = flightSegment.FlightSegment.Equipment.Model.Fleet,
                        Key = flightSegment.FlightSegment.Equipment.Model.Key
                    }) : null
                } : null
            };
        }

        private ProductFlightSegment BuildProductFlightSegments(Collection<Price> prices, ReservationFlightSegment flightSegment, string productCode = "")
        {
            return new ProductFlightSegment
            {
                ArrivalAirport = flightSegment.FlightSegment.ArrivalAirport,
                ArrivalDateTime = flightSegment.FlightSegment.ArrivalDateTime,
                BookingClasses = flightSegment.FlightSegment.BookingClasses,
                Characteristic = flightSegment.FlightSegment.Characteristic,
                ClassOfService = flightSegment.FlightSegment.BookingClasses[0].Code,
                DepartureAirport = flightSegment.FlightSegment.DepartureAirport,
                DepartureDateTime = flightSegment.FlightSegment.DepartureDateTime,
                FareBasisCode = ShopStaticUtility.GetFareBasisCode(prices, flightSegment.FlightSegment.SegmentNumber),
                FlightNumber = flightSegment.FlightSegment.FlightNumber,
                FlightSegmentType = flightSegment.FlightSegment.FlightSegmentType,
                IsActive = (!ShopStaticUtility.IsUsed(prices, flightSegment.FlightSegment.SegmentNumber)).ToString(),
                ID = flightSegment.FlightSegment.SegmentNumber.ToString(),
                RefID = flightSegment.FlightSegment.SegmentNumber.ToString(),
                IsInternational = flightSegment.FlightSegment.IsInternational,
                IsConnection = flightSegment.IsConnection,
                MarketedFlightSegment = flightSegment.FlightSegment.MarketedFlightSegment,
                OperatingAirlineCode = flightSegment.FlightSegment.OperatingAirlineCode,
                SegmentNumber = flightSegment.FlightSegment.SegmentNumber,
                TripIndicator = flightSegment.TripNumber,
                Equipment = (IsPOMOffer(productCode)) ? new United.Service.Presentation.CommonModel.AircraftModel.Aircraft
                {
                    CabinCount = (flightSegment.FlightSegment.Equipment != null) ? flightSegment.FlightSegment.Equipment.CabinCount : 0,
                    Model = (flightSegment.FlightSegment.Equipment?.Model != null) ? (new United.Service.Presentation.CommonModel.AircraftModel.AircraftModel
                    {
                        Description = flightSegment.FlightSegment.Equipment.Model.Description,
                        Fleet = flightSegment.FlightSegment.Equipment.Model.Fleet,
                        Key = flightSegment.FlightSegment.Equipment.Model.Key
                    }) : null
                } : null
            };
        }

        private Collection<ProductFilter> ProductFilters(string flowType,string productCode="")
        {

            if (flowType == United.Utility.Enum.FlowType.VIEWRES_SEATMAP.ToString())
            {
                return new Collection<ProductFilter> { new ProductFilter { IsIncluded = "true", ProductCode = "PCU" } };
            }

            if (flowType == United.Utility.Enum.FlowType.VIEWRES.ToString())
            {
                if(productCode=="SFC")
                {
                    Collection<ProductFilter> productFilters = new Collection<ProductFilter>();
                    productFilters.Add(new ProductFilter() { IsIncluded = "true", ProductCode = "SFC" });
                    return productFilters;
                }
                var productFilter = Constants.ViewResFlow_ProductMapping.Split(',').Select(x => new ProductFilter() { IsIncluded = "true", ProductCode = x }).ToCollection();
                if (!_configuration.GetValue<bool>("EnableAwardAccelerators"))
                {
                    productFilter.RemoveWhere(f => f.ProductCode == "APA");
                }
                return productFilter;
            }
            return null;
        }

        private ServiceClient Requester()
        {
            return new ServiceClient()
            {
                Requestor = new Requestor()
                {
                    ChannelID = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID"), //1301
                    ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName"), //"MMR",
                    LanguageCode = "en"
                }
            };
        }

        private Collection<Characteristic> BuildCharacteristics(string ticketPriceRevenue, bool isAward)
        {
            return new Collection<Service.Presentation.CommonModel.Characteristic>() {
                    // Ticket price including tax only, no ancillary
                    new Characteristic() { Code = "TKT_PRICE", Value = ticketPriceRevenue },
                    new Characteristic() { Code = "MILES_NEEDED", Value = isAward ? "Y" : "N" } };
        }

        private bool IsAward(Collection<Service.Presentation.CommonModel.Genre> types)
        {
            return types != null && types.Any(IsAward);
        }

        private bool IsAward(Service.Presentation.CommonModel.Genre type)
        {
            return type.Description != null && type.Key != null && type.Description.ToUpper().Trim().Equals("ITIN_TYPE") && type.Key.ToUpper().Trim().Equals("AWARD");
        }

        private string TicketingCountryCode(United.Service.Presentation.CommonModel.PointOfSale pointOfSale)
        {
            return pointOfSale != null && pointOfSale.Country != null ? pointOfSale.Country.CountryCode : string.Empty;
        }

        private void GetTKTPrice(Collection<Characteristic> characteristics, ref string ticketPriceRevenue, ref string ticketPriceAward, ref string currencyCode)
        {
            if (characteristics != null && characteristics.Count() > 0)
            {
                foreach (var characteristic in characteristics)
                {
                    if (characteristic != null && characteristic.Description != null && characteristic.Value != null)
                    {
                        switch (characteristic.Description.Trim().ToUpper())
                        {
                            case "ITINTOTALFORMILEAGE":
                                ticketPriceAward = characteristic.Value.Trim();
                                break;
                            case "ITINTOTALFORCURRENCY":
                                ticketPriceRevenue = characteristic.Value.Trim();
                                currencyCode = characteristic.Code != null ? characteristic.Code.Trim() : string.Empty;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private bool IsWaitListPNRFromCharacteristics(Collection<ReservationFlightSegment> FlightSegments)
        {
            var flightSegmentCharacteristics = FlightSegments.Where(t => t != null && t.Characteristic != null).SelectMany(t => t.Characteristic).ToCollection();
            return flightSegmentCharacteristics.Any(p => p != null && !p.Code.IsNullOrEmpty() && !p.Value.IsNullOrEmpty() && p.Code.Equals("Waitlisted", StringComparison.OrdinalIgnoreCase) && p.Value.Equals("True", StringComparison.OrdinalIgnoreCase));
        }

        public async Task<DynamicOfferDetailResponse> GetMerchOffersDetailsFromCCE(Session session, Reservation cslReservation, MOBRequest mobRequest, string flow, MOBPNR pnrResponse, string productCode = "", bool isPomDeepLink = false)
        {
            DynamicOfferDetailResponse response = null;
            try
            {
                string token = session.Token;

                if ((session == null || cslReservation == null || mobRequest == null || (pnrResponse.IsFareLockOrNRSA == true && !IsPOMOffer(productCode)) || (!_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && IsWaitListPNRFromCharacteristics(cslReservation.FlightSegments))))
                    return null;

                bool isEnableIBEBuyOutViewRes = await _shoppingUtility.IsEnableIBEBuyOutViewRes(mobRequest.Application.Id, mobRequest?.Application?.Version?.Major, session.CatalogItems).ConfigureAwait(false) && (pnrResponse.IsIBE || pnrResponse.IsCBE);

                if (!IsPOMOffer(productCode) && !(_configuration.GetValue<bool>("EnableTravelOptionsBundleInViewRes") || (_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes") && (pnrResponse.isELF || isEnableIBEBuyOutViewRes))))
                {
                    return null;
                }

                if (!GeneralHelper.IsApplicationVersionGreaterorEqual(mobRequest.Application.Id, mobRequest?.Application?.Version?.Major, _configuration.GetValue<string>("AndroidBasicEconomyBuyOutVersion"), _configuration.GetValue<string>("iOSBasicEconomyBuyOutVersion")))
                    return null;

                #region WI #2377 -code for schedule change check
                var isScheduledChanged = GetHasScheduledChanged(pnrResponse.Segments);
                if (isScheduledChanged)
                {
                    return null;
                }

                var request = await BuildDynamicOfferRequest(cslReservation, flow, session.Token, mobRequest.TransactionId, pnrResponse.IsBulk, productCode, "","",false, isEnableIBEBuyOutViewRes);
                string jsonRequest = JsonConvert.SerializeObject(request);
                string jsonresponse = "";
                if (!await _featureSettings.GetFeatureSettingValue("EnableCCEDynamicOfferUrls").ConfigureAwait(false))
                {
                    jsonresponse = await _shoppingCcePromoService.MerchOffersCceDetails(token, jsonRequest, session.SessionId).ConfigureAwait(false);
                }
                else
                {
                    jsonresponse = await _cceDODService.GetCCEDynamicOffersDetail(session.Token, jsonRequest);
                }
                #endregion

                if (jsonresponse != null)
                {
                    response = await DeserializeGetMerchOffersDetailsFromCCE(session, flow, productCode, jsonRequest, jsonresponse, isPomDeepLink).ConfigureAwait(false);
                }
                else
                {
                    response = null;
                }

                if (isPomDeepLink)
                {
                    var invalidResponse = string.IsNullOrEmpty(jsonresponse) 
                                            || response == null || response.Offers == null 
                                            || response.FlightSegments == null || response.FlightSegments.Count == 0;

                    if (invalidResponse && CheckIf2ndTryNeeded(cslReservation))
                    {
                        if (!await _featureSettings.GetFeatureSettingValue("EnableCCEDynamicOfferUrls").ConfigureAwait(false))
                        {
                            jsonresponse = await _shoppingCcePromoService.MerchOffersCceDetails(token, jsonRequest, session.SessionId).ConfigureAwait(false);
                        }
                        else
                        {
                            jsonresponse = await _cceDODService.GetCCEDynamicOffersDetail(session.Token, jsonRequest);
                        }
                        if (jsonresponse != null)
                        {
                            response = await DeserializeGetMerchOffersDetailsFromCCE(session, flow, productCode, jsonRequest, jsonresponse, isPomDeepLink).ConfigureAwait(false);
                        }
                        else
                        {
                            response = null;
                        }
                    }
                }

            }
            catch (MOBUnitedException coex)
            {
                response = null;

                _logger.LogWarning("GetMerchOffersDetailsFromCCE Unitedexception{unitedexception}", JsonConvert.SerializeObject(coex));

            }
            catch (System.Exception ex)
            {
                response = null;

                _logger.LogError("GetMerchOffersDetailsFromCCE Exception{exception}", JsonConvert.SerializeObject(ex));
            }

            return response;
        }

        private async Task<DynamicOfferDetailResponse> DeserializeGetMerchOffersDetailsFromCCE(Session session, string flow, string productCode, string jsonRequest, string jsonresponse, bool isPomDeepLink)
        {
            DynamicOfferDetailResponse response = JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(jsonresponse);
            if (response != null && response.Offers != null && response.Response.Error == null)
            {
                var getOffersCce = new GetOffersCce()
                {
                    OfferResponseJson = jsonresponse
                };
                if (IsPOMOffer(productCode))
                {
                    if (_configuration.GetValue<bool>("EnablePOMTermsAndConditions"))
                    {
                        var getOfferCcePOMSessionValue = getOffersCce.ObjectName + _configuration.GetValue<string>("InflightMealProductCode").ToString();
                        await _sessionHelperService.SaveSession<GetOffersCce>(getOffersCce, session.SessionId, new List<string> { session.SessionId, getOfferCcePOMSessionValue }, getOfferCcePOMSessionValue).ConfigureAwait(false);
                    }
                    else
                    {
                        await _sessionHelperService.SaveSession<DynamicOfferDetailResponse>(response, session.SessionId, new List<string> { session.SessionId, typeof(DynamicOfferDetailResponse).FullName }, typeof(DynamicOfferDetailResponse).FullName).ConfigureAwait(false);
                    }
                }
                else
                {
                    await _sessionHelperService.SaveSession<GetOffersCce>(getOffersCce, session.SessionId, new List<string> { session.SessionId, getOffersCce.ObjectName }, getOffersCce.ObjectName).ConfigureAwait(false);
                }
            }
            else
            {
                if(isPomDeepLink)
                    _logger.LogWarning("GetMerchOffersDetailsFromCCE DODResponse {flow}, {@Request} and {@Response} ", flow, jsonRequest, response);
                
                response = null;
            }

            return response;
        }

        private bool CheckIf2ndTryNeeded(Reservation cslReservation)
        {
            return cslReservation.FlightSegments.Any(segment => IsWithInEligibleWindow(Convert.ToDateTime(segment?.FlightSegment?.DepartureUTCDateTime)));
        }

        private bool IsWithInEligibleWindow(DateTime departureUTCTime)
        {
            var after24Hours = DateTime.Now.AddDays(1);
            var after5Days = DateTime.Now.AddDays(5);

            return departureUTCTime > after24Hours && after5Days > departureUTCTime;
        }

        public async Task<DynamicOfferDetailResponse> GetProductOfferAndDetails(TravelOptionsRequest request, Service.Presentation.ReservationModel.Reservation cslReservation, MOBPNR pnrResponse, string token)
        {
            DynamicOfferDetailResponse response = null;
            try
            {

                if (request == null || cslReservation == null || pnrResponse == null)
                    return null;

                bool isEnableNewFilterRequestForMROffers = await _shoppingUtility.IsEnableNewFilterRequestForMROffers().ConfigureAwait(false);
                var dodRequest = await BuildDynamicOfferRequest(cslReservation, request.Flow, token, request.TransactionId, pnrResponse.IsBulk, request.ProductCode, request.ProductName, request.CorrelationId, isEnableNewFilterRequestForMROffers).ConfigureAwait(false);
                string jsonRequest = JsonConvert.SerializeObject(dodRequest);
                string jsonresponse = "";
                if (!await _featureSettings.GetFeatureSettingValue("EnableCCEDynamicOfferUrls").ConfigureAwait(false))
                {
                    jsonresponse = await _shoppingCcePromoService.MerchOffersCceDetails(token, jsonRequest, request.SessionId).ConfigureAwait(false);
                }
                else
                {
                    jsonresponse = await _cceDODService.GetCCEDynamicOffersDetail(token, jsonRequest);
                }

                if (jsonresponse != null)
                {
                    response = JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(jsonresponse);
                    if (request.ProductName != ProductName.ANC.ToString())
                    {
                        // Save Empty PCU State when new offer call is made to CCE, This is to clear PCU state to being used in seat map upper cabin
                        var pcuState = new PcuState();
                        await _sessionHelperService.SaveSession(pcuState, request.SessionId, new List<string> { request.SessionId, pcuState.ObjectName }, pcuState.ObjectName).ConfigureAwait(false);

                        var getOffersCce = new GetOffersCce()
                        {
                            OfferResponseJson = jsonresponse
                        };
                        await _sessionHelperService.SaveSession<GetOffersCce>(getOffersCce, request.SessionId, new List<string> { request.SessionId, ObjectNames.CCEDynamicOfferDetailResponse }, ObjectNames.CCEDynamicOfferDetailResponse).ConfigureAwait(false);
                    }
                }
                else
                    response = null;

            }
            catch (MOBUnitedException coex)
            {
                response = null;

                _logger.LogWarning("GetProductOfferAndDetails Unitedexception{unitedexception}", JsonConvert.SerializeObject(coex));

            }
            catch (System.Exception ex)
            {
                response = null;

                _logger.LogError("GetProductOfferAndDetails Exception{exception}", JsonConvert.SerializeObject(ex));
            }

            return response;
        }

        public async Task<DynamicOfferResponse> GetProductOffersFromCCE(Session session, Reservation cslReservation, MOBRequest mobRequest, string flow, MOBPNR pnrResponse, string productCode = "")
        {
            DynamicOfferResponse response = null;
            try
            {
                string token = session.Token;

                if ((session == null || cslReservation == null || mobRequest == null || (pnrResponse.IsFareLockOrNRSA == true && !IsPOMOffer(productCode)) || (!_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && IsWaitListPNRFromCharacteristics(cslReservation.FlightSegments))))
                    return null;

                #region WI #2377 -code for schedule change check
                var isScheduledChanged = GetHasScheduledChanged(pnrResponse.Segments);
                if (isScheduledChanged)
                {
                    return null;
                }

                var request = await BuildCCEDOOfferRequest(cslReservation, flow, session.Token, mobRequest.TransactionId, pnrResponse.IsBulk, productCode).ConfigureAwait(false);
                string jsonRequest = JsonConvert.SerializeObject(request);

                string jsonresponse = ""; 
                if (!await _featureSettings.GetFeatureSettingValue("EnableCCEDynamicOfferUrls").ConfigureAwait(false))
                {
                    jsonresponse = await _shoppingCcePromoService.CCEDynamicOfferResponse(token, jsonRequest, session.SessionId).ConfigureAwait(false);
                }
                else
                {
                    jsonresponse = await _cceDOService.GetDynamicOffers(token, jsonRequest);
                }
                #endregion

                if (jsonresponse != null)
                {
                    DynamicOfferResponse doResponse = JsonConvert.DeserializeObject<DynamicOfferResponse>(jsonresponse);
                    if (doResponse != null && doResponse.Error == null && doResponse.ResponseData != null)
                    {
                        return doResponse;
                    }
                    else
                    {
                        response = null;
                    }
                }
                else
                    response = null;

            }

            catch (MOBUnitedException coex)
            {
                response = null;

                _logger.LogWarning("GetProductOffersFromCCE Unitedexception{unitedexception}", JsonConvert.SerializeObject(coex));

            }
            catch (System.Exception ex)
            {
                response = null;

                _logger.LogError("GetProductOffersFromCCE Exception{exception}", JsonConvert.SerializeObject(ex));
            }

            return response;
        }

        public async System.Threading.Tasks.Task SendEplusStandAloneOfferFeedbackToCCE(MOBSeatChangeInitializeRequest request, string token)
        {
            if (request != null && !string.IsNullOrEmpty(token))
            {
                try
                {
                    United.Service.Presentation.OptimizationRequestModel.FeedbackRequest feedbackRequest = new Service.Presentation.OptimizationRequestModel.FeedbackRequest();
                    var guid = await _sessionHelperService.GetSession<Collection<UniqueIdentifier>>(request.SessionId, ObjectNames.CCEDOResponseGuidSession, new List<string> { request.SessionId, ObjectNames.CCEDOResponseGuidSession }).ConfigureAwait(false);

                    bool hasAdvisory = guid != null && guid.Count > 0 && guid.Any(x => !string.IsNullOrEmpty(x.Name) && x.Name.Equals("AdvisorId", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(x.ID));

                    if (hasAdvisory)
                    {
                        feedbackRequest.Events = new Collection<Service.Presentation.OptimizationRequestModel.OptimizationEvent> {

                        new Service.Presentation.OptimizationRequestModel.OptimizationEvent
                        {
                            Products = new Collection<Service.Presentation.OptimizationModel.OptimizationProduct>
                            {
                                new Service.Presentation.OptimizationModel.OptimizationProduct
                                {
                                    Code = _configuration.GetValue<string>("FeedBackEplusOfferTileProductCode")
                                }
                            },
                            Type = _configuration.GetValue<string>("FeedBackEplusOfferTileEventType")
                        }
                    };
                        feedbackRequest.Itinerary = new Service.Presentation.OptimizationModel.OptimizationItinerary
                        {
                            ConfirmationID = request.RecordLocator
                        };
                        feedbackRequest.ServiceClient = new ServiceClient();
                        feedbackRequest.ServiceClient.Requestor = new Requestor
                        {
                            ChannelName = _configuration.GetValue<string>("FeedBackEplusOfferTileChannelName"),
                            Description = _configuration.GetValue<string>("FeedBackEplusOfferTileDescription")
                        };

                        feedbackRequest.ServiceClient.GUIDs = new Collection<UniqueIdentifier>();
                        feedbackRequest.ServiceClient.GUIDs = guid;

                        var jsonRequest = JsonConvert.SerializeObject(feedbackRequest);
                        var jsonResponse = await _cceAncillaryMyTripsFeedbackService.SendCCEAncillaryMyTripsFeedback(token, jsonRequest).ConfigureAwait(false);
                    }
                }
                catch
                { }
            }
        }

        private async Task<DynamicOfferRequest> BuildCCEDOOfferRequest(Reservation cslReservation, string flow, string token, string transactionId, bool isBulk, string productCode)
        {

            string currencyCode = string.Empty;
            string ticketPriceRevenue = "0";
            string ticketPriceAward = "0";
            GetTKTPrice(cslReservation.Characteristic, ref ticketPriceRevenue, ref ticketPriceAward, ref currencyCode);
            if (!_configuration.GetValue<bool>("IsSuppressTicketPriceAndShowOffers") && Convert.ToDouble(ticketPriceRevenue) <= 0)
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("TicketPriceMessageForViewResOffersWhenPriceOrMilesIsZeroFromPNR"));
            }
            var countryCode = TicketingCountryCode(cslReservation.PointOfSale);
            DynamicOfferRequest doRequest = new DynamicOfferRequest();
            doRequest.Requester = new Requester()
            {
                Requestor = new Requestor()
                {
                    ChannelName = "MOBMYRES",
                    LanguageCode = "en-us",
                    CurrencyCode = currencyCode,
                    CountryCode = countryCode,
                    TicketingCountryCode = countryCode
                },
                GUIDs = new Collection<UniqueIdentifier>
                {
                    new UniqueIdentifier { Name = "TransactionId", ID = transactionId},
                    new UniqueIdentifier { Name = "AuthorizationToken", ID = token}
                }
            };

            doRequest.RequestData = new RequestData();
            var isAwardTravel = IsAward(cslReservation.Type);

            var reqData = new RequestData();
            reqData.FlightSegments = ProductFlightSegments(cslReservation.FlightSegments, cslReservation.Prices, productCode);
            reqData.Travelers = ProductTravelers_CFOP(cslReservation.Travelers, productCode);
            reqData.ODOptions = PopulateODOptions(cslReservation.FlightSegments);
            reqData.Characteristics = await BuildCharacteristicsCce(ticketPriceRevenue, isAwardTravel, false, false, null);
            reqData.Solutions = Solutions(cslReservation.FlightSegments);
            reqData.ReservationReferences = ReservationReferences(cslReservation, true, isBulk, productCode, true);
            doRequest.RequestData = reqData;

            return doRequest;
        }

        public async Task<DynamicOfferDetailRequest> BuildDynamicOfferRequest(Reservation cslReservation, string flow, string token, string transactionId, bool isBulkPNR, string productCode = "", string productName = "", string correlationId = "", bool isEnableNewFilterRequestForMROffers = false, bool isEnableIBEBuyOutViewRes = false)
        {
            string ticketPriceRevenue = "0";
            string ticketPriceAward = "0";
            string currencyCode = string.Empty;
            
            #region ticket price with tax including revenue and award 
            GetTKTPrice(cslReservation.Characteristic, ref ticketPriceRevenue, ref ticketPriceAward, ref currencyCode);
            if (Convert.ToDouble(ticketPriceRevenue) <= 0)
            {
                if ((!IsPOMOffer(productCode)))
                    throw new MOBUnitedException("Ticket price should be greater than zero");
            }
            #endregion
            var countryCode = TicketingCountryCode(cslReservation.PointOfSale);
            var isAwardTravel = IsAward(cslReservation.Type);
            bool isAgencyPNR = isEnableIBEBuyOutViewRes && cslReservation.BookingAgency != null && cslReservation.BookingAgency.IsOnlineAgency;
            
            return new DynamicOfferDetailRequest()
            {
                Characteristics = await BuildCharacteristicsCce(ticketPriceRevenue, isAwardTravel, isAgencyPNR, isEnableIBEBuyOutViewRes, cslReservation.BookingAgency),
                CurrencyCode = currencyCode, //currency code should be the same as ticketing currency code
                CountryCode = countryCode,
                TicketingCountryCode = countryCode,
                Requester = !string.IsNullOrEmpty(correlationId) ? RequesterCce(token, transactionId, correlationId) : RequesterCce(token, transactionId),
                Filters = isEnableNewFilterRequestForMROffers && !string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(productCode) ? ProductFiltersCceWithProductName(flow, productCode, productName):ProductFiltersCce(flow, productCode, productName),
                IsAwardReservation = isAwardTravel.ToString(),
                FlightSegments = ProductFlightSegments(cslReservation.FlightSegments, cslReservation.Prices, productCode),
                Solutions = Solutions(cslReservation.FlightSegments),
                ODOptions = ProductOriginDestinationOptions(cslReservation.FlightSegments),
                ReservationReferences = ReservationReferences(cslReservation, true, isBulkPNR, productCode),
                Travelers = ProductTravelers_CFOP(cslReservation.Travelers, productCode)
            };
        }

        private Collection<ProductFilter> ProductFiltersCce(string flowType, string productCode = "", string productName = "")
        {
            if (flowType == FlowType.VIEWRES.ToString())
            {
                var productCodes = new List<string>();
                if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(productCode))
                {
                    if (productName.Equals(ProductName.BUNDLE.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        productCodes.Add(productCode);
                    }
                    else if (!string.IsNullOrEmpty(productCode))
                    {
                        productCodes.Add(productCode);
                    }
                }
                else
                {
                    if (IsPOMOffer(productCode))
                    {
                        productCodes.Add(_configuration.GetValue<string>("InflightMealProductCode"));
                        return productCodes.Select(x => new ProductFilter() { IsIncluded = "true", ProductCode = x }).ToCollection();
                    }
                    if (_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes"))
                    {
                        productCodes.Add("BEB");
                    }
                    if (_configuration.GetValue<bool>("EnableTravelOptionsBundleInViewRes"))
                    {
                        productCodes.Add("SBE");
                    }
                }
                var productFilter = productCodes.Select(x => new ProductFilter() { IsIncluded = "true", ProductCode = x }).ToCollection();
                return productFilter;
            }

            return null;
        }

        private Collection<ProductFilter> ProductFiltersCceWithProductName(string flowType, string productCode = "", string productName = "")
        {
           
            var productCodes = new List<string>();
            if (!string.IsNullOrEmpty(productName) && !string.IsNullOrEmpty(productCode))
            {
                if (productName.Equals(ProductName.BUNDLE.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    productCodes.Add(productCode);
                }
                else if (!string.IsNullOrEmpty(productCode))
                {
                    productCodes.Add(productCode);
                }
                var productFilter = productCodes.Select(x => new ProductFilter() { IsIncluded = "true", ProductCode = x }).ToCollection();
                return productFilter;
            }
               
            return null;
        }

        private ServiceClient RequesterCce(string token, string transactionId)
        {
            return new ServiceClient()
            {
                Requestor = new Requestor()
                {
                    ChannelID = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelID"), //"6401"
                    ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelName"), //"MOBMYRES",
                    LanguageCode = "en"
                },
                GUIDs = new Collection<UniqueIdentifier>
                {
                    new UniqueIdentifier { Name = "TransactionId", ID = transactionId},
                    new UniqueIdentifier { Name = "AuthorizationToken", ID = token},
                }
            };
        }
        private ServiceClient RequesterCce(string token, string transactionId, string correlationId)
        {
            return new ServiceClient()
            {
                Requestor = new Requestor()
                {
                    ChannelID = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelID"), //"6401"
                    ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceMOBMYRESChannelName"), //"MOBMYRES",
                    LanguageCode = "en"
                },
                GUIDs = new Collection<UniqueIdentifier>
                {
                    new UniqueIdentifier { Name = "TransactionId", ID = transactionId},
                    new UniqueIdentifier { Name = "AuthorizationToken", ID = token},
                    new UniqueIdentifier { Name = "CorrelationId", ID = correlationId},
                }
            };
        }


        private async Task< Collection<Characteristic>> BuildCharacteristicsCce(string ticketPriceRevenue, bool isAward, bool isOnlineAgency, bool isEnableIBEBuyOutViewRes, United.Service.Presentation.CommonModel.VendorModel.TravelAgency bookingAgency)
        {
            if (await _featureSettings.GetFeatureSettingValue("IsEnableAgencyChangeForIBE").ConfigureAwait(false) && bookingAgency !=null && isEnableIBEBuyOutViewRes)
            {
                return new Collection<Characteristic>() {
                    // Ticket price including tax only, no ancillary
                    new Characteristic() { Code = "TKT_PRICE", Value = ticketPriceRevenue },
                    new Characteristic() { Code = "MILES_NEEDED", Value = isAward ? "Y" : "N" },
                    new Characteristic() { Code = "IsEnabledThroughCCE", Value = "True"},
                    new Characteristic() { Code = "Context", Value = "TD"},
                    new Characteristic() { Code = "IsAwardReservation", Value = isAward ? "True" : "False"},
                    new Characteristic() { Code = "IsOnlineAgency", Value = isOnlineAgency ? "True" : "False"}
            };
            }

            return new Collection<Characteristic>() {
                    // Ticket price including tax only, no ancillary
                    new Characteristic() { Code = "TKT_PRICE", Value = ticketPriceRevenue },
                    new Characteristic() { Code = "MILES_NEEDED", Value = isAward ? "Y" : "N" },
                    new Characteristic() { Code = "IsEnabledThroughCCE", Value = "True"},
                    new Characteristic() { Code = "Context", Value = "TD"},
                    new Characteristic() { Code = "IsAwardReservation", Value = isAward ? "True" : "False"}
            };
        }


        public MOBTravelOptionsBundle TravelOptionsBundleOffer(United.Service.Presentation.PersonalizationResponseModel.DynamicOfferDetailResponse productOffers, MOBRequest request, string sessionId)
        {
            if (!_configuration.GetValue<bool>("EnableTravelOptionsBundleInViewRes")) return null;

            MOBTravelOptionsBundle travelOptionsBundle = null;
            try
            {
                var productOffer = BuildIndividualProductOffer(productOffers, "SBE");
                travelOptionsBundle = new TravelOptionsBundle(productOffer)
                                            .BuildTravelOptions()
                                            .BundleOffer;
            }
            catch (Exception ex)
            {
                _logger.LogError("TravelOptionBundleOffer {exception}", JsonConvert.SerializeObject(ex));
            }

            return travelOptionsBundle;
        }

        public async Task<MOBBasicEconomyBuyOut> BasicEconomyBuyOutOffer(DynamicOfferDetailResponse productOffers, MOBRequest request, string sessionId, MOBPNR pnrResponse, List<MOBItem> catalogItems)
        {
            if (!_configuration.GetValue<bool>("EnableBasicEconomyBuyOutInViewRes")) return null;

            MOBBasicEconomyBuyOut basicEconomyBuyOutOffer = null;
            try
            {
                bool isEnableIBEBuyOutViewRes = await _shoppingUtility.IsEnableIBEBuyOutViewRes(request.Application.Id, request?.Application?.Version?.Major, catalogItems).ConfigureAwait(false);

                var productOffer = BuildIndividualProductOffer(productOffers, "BEB");
                var response = await new BasicEconomyBuyOut(productOffer, _configuration, request, isEnableIBEBuyOutViewRes, _featureSettings)
                                .BuildBuyOutOptions();
                if (response != null && response.BasicEconomyBuyOutOffer != null) 
                {
                    basicEconomyBuyOutOffer = response.BasicEconomyBuyOutOffer;
                }
                
            }
            catch (Exception ex)
            {
                _logger.LogError("BasicEconomyBuyOutOffer {exception}", JsonConvert.SerializeObject(ex));
            }

            return basicEconomyBuyOutOffer;
        }

        public async Task<DOTBaggageInfoResponse> GetDOTBaggageInfoWithPNR(string accessCode, string transactionId, string languageCode, string messageFormat, int applicationId, string recordLocator, string ticketingDate, string channelId, string channelName, MOBSHOPReservation reservation = null, Reservation cslReservation = null)
        {
            DOTBaggageInfoRequest request = new DOTBaggageInfoRequest();
            request.TransactionId = "Mobile_Request_11";
            request.AccessCode = accessCode;
            request.TransactionId = transactionId;
            request.LanguageCode = languageCode;
            request.Application = new MOBApplication();
            request.Application.Id = applicationId;
            request.RecordLocator = recordLocator;
            request.TraverlerINfo = new List<DOTBaggageTravelerInfo>();
            DOTBaggageTravelerInfo travelInfo = new DOTBaggageTravelerInfo();
            travelInfo.Id = "1";
            if (ticketingDate.Trim() == "" || ticketingDate.Trim() == "01/01/0001")
            {
                ticketingDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            else if (DateTime.ParseExact(ticketingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) < DateTime.Now.AddYears(-2) || DateTime.ParseExact(ticketingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture) < DateTime.Now.AddDays(1)) // If passed any default date which is not accurate
            {
                ticketingDate = DateTime.Now.ToString("MM/dd/yyyy");
            }
            travelInfo.TicketingDate = DateTime.ParseExact(ticketingDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            travelInfo.TicketingDateString = ticketingDate;
            request.TraverlerINfo.Add(travelInfo);

            DOTBaggageInfoResponse response = new DOTBaggageInfoResponse();
            try
            {
                response = await GetDOTBaggageInfoWithPNR(request, channelId, channelName, reservation, cslReservation);
                response.Request = request;
            }
            catch (MOBUnitedException uex)
            {
                response.DotBaggageInfo = null;
                response.DotBaggageInfo = new DOTBaggageInfo();
                response.DotBaggageInfo.ErrorMessage = _configuration.GetValue<string>("DOTBaggageGenericExceptionMessage");
                response.Exception = new MOBException();
                response.Exception.Message = uex.Message;
            }
            catch (System.Exception ex)
            {
                response.DotBaggageInfo = null;
                response.DotBaggageInfo = new DOTBaggageInfo();
                response.DotBaggageInfo.ErrorMessage = _configuration.GetValue<string>("DOTBaggageGenericExceptionMessage");

                response.Exception = new MOBException("99999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }


            return response;
        }

        public async Task<DOTBaggageInfoResponse> GetDOTBaggageInfoWithPNR(DOTBaggageInfoRequest request, string channelId, string channelName, MOBSHOPReservation reservation, Reservation cslReservation)
        {
            DOTBaggageInfoResponse response = new DOTBaggageInfoResponse();
            try
            {
                MerchandizingServicesClient merchClient = null;
                if (_configuration.GetValue<bool>("AssignTimeOutForMerchandizeDOTBaggageServiceCall"))
                {
                    #region Assigne Time Out Value for Merchantize Engine Call
                    MerchandizingServicesClient merchClient1 = new MerchandizingServicesClient();
                    int timeOutSeconds = _configuration.GetValue<string>("TimeOutSecondsForMerchandizeDOTBaggage") != null ? Convert.ToInt32(_configuration.GetValue<string>("TimeOutSecondsForMerchandizeDOTBaggage").Trim()) : 7;
                    BasicHttpBinding binding = new BasicHttpBinding();
                    TimeSpan timeout = new TimeSpan(0, 0, timeOutSeconds);
                    binding.CloseTimeout = timeout;
                    binding.SendTimeout = timeout;
                    binding.ReceiveTimeout = timeout;
                    binding.OpenTimeout = timeout;
                    var merchEndpoint = merchClient1.Endpoint.Address.ToString();
                    EndpointAddress endPoint = new EndpointAddress(merchEndpoint);
                    merchClient = new MerchandizingServicesClient(binding, endPoint);
                    #endregion
                }
                else
                {
                    merchClient = new MerchandizingServicesClient();
                }

                var offerRequest = EnableIBEFull() && cslReservation != null
                                   ? BuildBagRequestFromCslreservationDetail(cslReservation, channelId, channelName)
                                   : MerchOfferRequestWithPNR(request, channelId, channelName, reservation);

                var offerRequests = new GetMerchandizingOffersInput();
                offerRequests.MerchOfferRequest = offerRequest;

                _logger.LogInformation("GetDOTBaggageInfoWithPNR Request {GetDOTBaggageInfoWithPNR_REQUEST}", JsonConvert.SerializeObject(offerRequest));

                try
                {
                    GetMerchandizingOffersOutput offers = await _merchOffersService.GetOffers(offerRequests).ConfigureAwait(false);

                    _logger.LogInformation("GetDOTBaggageInfoWithPNR Response from backend {GetDOTBaggageInfoWithPNR_Response} from backend and {TransactionId}", JsonConvert.SerializeObject(offers), request.TransactionId);

                    response.DotBaggageInfo = await MapDOTBagInfoFromMerchandizingResponse(offers.MerchOffers, reservation);
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                //string responseXML = Serialization.Serialize(offers, typeof(MerchOffers));
            }
            catch (System.Exception ex) { throw ex; }

            return response;
        }
        private async Task<DOTBaggageInfo> GetBaggageInfo(MOBSHOPReservation reservation)
        {
            var isElf = reservation != null && reservation.IsELF;

            var isIBE = EnableIBEFull() && reservation != null && (reservation.ShopReservationInfo2 != null) && reservation.ShopReservationInfo2.IsIBE;

            return await GetBaggageInfo(isElf, isIBE);
        }
        private async Task<DOTBaggageInfo> GetBaggageInfo(bool isElf, bool isIBE)
        {
            if (cachedLegalDocuments.IsNull())
            {
               var cachedLegalDocuments = new List<Definition.MOBLegalDocument>();

                foreach (var title in Titles)
                {
                    var cachedLegalDocument = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(title, _headers.ContextValues.TransactionId, true).ConfigureAwait(false);
                    cachedLegalDocuments.AddRange(cachedLegalDocument);
                }
            }
            var legalDocuments = cachedLegalDocuments.Clone();

            if (isElf || isIBE)
            {
                legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText1");
                if (isIBE)
                {
                    legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText1 - ELF");
                    legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText3");
                }
                else
                {
                    legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText1 - IBE");
                    legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText3IBE");
                }
            }
            else
            {
                legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText1 - ELF");
                legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText1 - IBE");
                legalDocuments.RemoveAll(l => l.Title == "DOTBaggageInfoText3IBE");
            }

            var document1TitleAndDescription = legalDocuments.First(l => l.Title.Contains("DOTBaggageInfoText1")).LegalDocument.Split('|');
            var document2TitleAndDescription = legalDocuments.First(l => l.Title.Contains("DOTBaggageInfoText2")).LegalDocument.Split('|');
            var document3TitleAndDescription = legalDocuments.First(l => l.Title.Contains("DOTBaggageInfoText3")).LegalDocument.Split('|');
            var document4 = legalDocuments.First(l => l.Title.Contains("DOTBaggageInfoText4")).LegalDocument;

            return new DOTBaggageInfo
            {
                Title1 = document1TitleAndDescription[0],
                Title2 = document2TitleAndDescription[0],
                Title3 = document3TitleAndDescription[0],
                Description1 = document1TitleAndDescription[1],
                Description2 = document2TitleAndDescription[1],
                Description3 = document3TitleAndDescription[1],
                Description4 = document4
            };
        }

            

        private async Task<DOTBaggageInfo> MapDOTBagInfoFromMerchandizingResponse(MerchOffers offers, MOBSHOPReservation reservation)
        {
            #region
            var baggageInfo = await GetBaggageInfo(reservation);
            List<BagFeesPerSegment> baggageFeesPerSegment = new List<BagFeesPerSegment>();
            BagFeesPerSegment obj1 = new BagFeesPerSegment();
            bool isFirstBagFree = true;
            decimal chaseOfferAmount = 0; // Added new variable for getting the highest baggage fee
            var chaseOfferCurrency = "USD"; // Added new variable for getting the highest baggage fee
            string chaseFOPMessageText = string.Empty;
            if (offers != null && offers.Itinerary != null)
            {
                #region
                foreach (ItineraryType itinerary in offers.Itinerary)
                {
                    #region
                    if (itinerary.Items != null && itinerary.Items.Count() > 0)
                    {
                        foreach (PricedItineraryType item in itinerary.Items)
                        {
                            if (item.AirItinerary != null && item.AirItinerary.OriginDestinationOptions != null)
                            {
                                foreach (OriginDestinationOptionType orgdestType in item.AirItinerary.OriginDestinationOptions)
                                {
                                    #region
                                    obj1 = GetFirstAndSecondBagFeePerTrip(offers, orgdestType.FlightSegment[0].Id.Trim(), ref chaseFOPMessageText, ref chaseOfferAmount, ref chaseOfferCurrency);
                                    int i = orgdestType.FlightSegment.Count();
                                    BagFeesPerSegment obj2 = new BagFeesPerSegment();
                                    obj2.FirstBagFee = obj1.FirstBagFee;
                                    obj2.SecondBagFee = obj1.SecondBagFee;
                                    obj2.WeightPerBag = obj1.WeightPerBag;
                                    obj2.IsFirstBagFree = obj1.IsFirstBagFree;
                                    obj2.RegularFirstBagFee = obj1.RegularFirstBagFee;
                                    obj2.RegularSecondBagFee = obj1.RegularSecondBagFee;
                                    obj2.FlightTravelDate = orgdestType.FlightSegment[0].DepartureDateTime.ToString("ddd., MMM dd, yyyy");
                                    obj2.OriginAirportCode = orgdestType.FlightSegment[0].DepartureAirport.LocationCode;
                                    string airportName = string.Empty;
                                    string cityName = string.Empty;
                                    _shoppingUtility.GetAirportCityName(obj2.OriginAirportCode, ref airportName, ref cityName);
                                    obj2.OriginAirportName = airportName;
                                    obj2.DestinationAirportCode = orgdestType.FlightSegment[i - 1].ArrivalAirport.LocationCode;//To get the destination
                                    airportName = string.Empty;
                                    cityName = string.Empty;
                                    _shoppingUtility.GetAirportCityName(obj2.DestinationAirportCode, ref airportName, ref cityName);
                                    obj2.DestinationAirportName = airportName;
                                    if (!obj2.IsFirstBagFree)
                                    {
                                        isFirstBagFree = false;
                                    }
                                    baggageFeesPerSegment.Add(obj2);
                                    #endregion
                                }
                            }
                        }
                    }
                    #endregion
                }
                #endregion
            }
            bool showChaseCardMessage = CheckShowChaseBagFeeMessage(offers);
            //if(isFirstBagFree && !showChaseCardMessage)
            if (isFirstBagFree)
            {
                baggageInfo.Title3 = "";
                baggageInfo.Description3 = "";
            }
            else
            {
                if (!_configuration.GetValue<bool>("SupressFixForCheckedBagsChaseOfferDynamicPrice") && !isFirstBagFree && chaseOfferAmount > 0) //Added this to get the highest first baggage price of IBE,IBELite
                {
                    var chaseFormatedOfferAmount = FormatedBagFee(chaseOfferAmount * 2, chaseOfferCurrency);
                    baggageInfo.Description3 = string.Format(baggageInfo.Description3, chaseFormatedOfferAmount);
                }
            }
            baggageInfo.Title3 = ""; // According ot curtis do not show the chase free bag header for My Flights (view reservation also comes as my flights)
            baggageInfo.BaggageFeesPerSegment = baggageFeesPerSegment;
            if (!String.IsNullOrEmpty(chaseFOPMessageText))
            {
                baggageInfo.Description3 = baggageInfo.Description3 + " " + chaseFOPMessageText;
            }
            return baggageInfo;
            #endregion
        }

        private string FormatedBagFee(decimal amount, string currencyCode)
        {
            if (currencyCode.Equals("USD"))
            {
                return "$" + amount.ToString().Replace(".00", "");
            }
            else
            {
                return amount.ToString().Replace(".00", "") + " " + currencyCode;
            }
        }

        private bool CheckShowChaseBagFeeMessage(MerchOffers offers)
        {
            #region
            foreach (ItineraryType itinerary in offers.Itinerary)
            {
                #region
                if (itinerary.TravelerInfo != null)
                {
                    if (itinerary.TravelerInfo != null && itinerary.TravelerInfo.Traveler != null)
                    {
                        foreach (TravelerType traveler in itinerary.TravelerInfo.Traveler)
                        {
                            if (traveler.Loyalty != null)
                            {
                                foreach (CustomerLoyaltyType loyalty in traveler.Loyalty)
                                {
                                    if (loyalty.CardType != null && (loyalty.CardType == CustomerLoyaltyTypeCardType.PPC || loyalty.CardType == CustomerLoyaltyTypeCardType.MEC || loyalty.CardType == CustomerLoyaltyTypeCardType.OPC || loyalty.CardType == CustomerLoyaltyTypeCardType.CCC))
                                    {
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            return true;
            #endregion
        }

        private BagFeesPerSegment GetFirstAndSecondBagFeePerTrip(MerchOffers offers, string flightSegmentIndex, ref string chaseFOPMessageText, ref decimal chaseOfferAmount, ref string chaseOfferCurrency)
        {
            BagFeesPerSegment obj1 = new BagFeesPerSegment();
            string first1BagFee = "", second2BagFee = ""; string chaseCurreny = "USD"; // Chase FOP Fix
            if (offers != null && offers.Itinerary != null)
            {
                #region
                foreach (ItineraryType itinerary in offers.Itinerary)
                {
                    #region
                    if (itinerary.MerchandisingInfo != null && itinerary.MerchandisingInfo.Services != null)
                    {
                        foreach (ServiceType service in itinerary.MerchandisingInfo.Services)
                        {
                            if (service != null && service.Tiers != null)
                            {
                                foreach (TierType tier in service.Tiers)
                                {
                                    if (tier.TierInfo != null && tier.TierInfo.Group == TierInfoTypeGroup.BG && tier.TierInfo.SubGroup == TierInfoTypeSubGroup.BG && tier.TierExtension != null && tier.TierExtension.Bags != null && tier.TierExtension.Bags.Bag != null && tier.Pricing != null && tier.Pricing[0] != null && tier.Pricing[0].Association != null)
                                    {
                                        foreach (AssociationTypeSegmentMapping item in tier.Pricing[0].Association.Items)
                                        {
                                            if (item.SegmentReference.Trim() == flightSegmentIndex)
                                            {
                                                #region

                                                string currencyCode = "USD";

                                                if (tier.TierExtension.Bags.Bag[0] != null && Convert.ToInt32(tier.TierExtension.Bags.Bag[0].MinQuantity.Trim()) <= 1 && Convert.ToInt32(tier.TierExtension.Bags.Bag[0].MaxQuantity.Trim()) >= 1)
                                                {
                                                    #region

                                                    if (!string.IsNullOrEmpty(tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode) && tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode.Trim().ToUpper().Equals("USD"))
                                                    {
                                                        currencyCode = "USD";
                                                    }
                                                    else
                                                    {
                                                        currencyCode = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode;
                                                    }

                                                    if (currencyCode.Equals("USD"))
                                                    {
                                                        obj1.FirstBagFee = "$" + tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", "");
                                                    }
                                                    else
                                                    {
                                                        obj1.FirstBagFee = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", "") + " " + currencyCode;
                                                    }

                                                    first1BagFee = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", ""); // Chase FOP Fix

                                                    obj1.WeightPerBag = tier.TierExtension.Bags.Bag[0].Weight[1].Max + " " + tier.TierExtension.Bags.Bag[0].Weight[1].Unit + " (" + tier.TierExtension.Bags.Bag[0].Weight[0].Max + " " + tier.TierExtension.Bags.Bag[0].Weight[0].Unit + ")";//."50 lbs (23 kg)";
                                                    if (tier.TierInfo.Type == TierInfoTypeType.Allowance)
                                                    {
                                                        obj1.IsFirstBagFree = true;
                                                        if (currencyCode.Equals("USD"))
                                                        {
                                                            obj1.FirstBagFee = "$0";
                                                        }
                                                        else
                                                        {
                                                            obj1.FirstBagFee = "0 " + currencyCode;
                                                        }

                                                        first1BagFee = "0"; // Chase FOP Fix

                                                        if (tier.TierExtension.Bags.Bag[0].RegularPrice != null && tier.TierExtension.Bags.Bag[0].RegularPrice[0].Price != null)
                                                        {
                                                            if (currencyCode.Equals("USD"))
                                                            {
                                                                obj1.RegularFirstBagFee = "$" + tier.TierExtension.Bags.Bag[0].RegularPrice[0].Price.BasePrice.amount.ToString().Replace(".00", "");
                                                            }
                                                            else
                                                            {
                                                                obj1.RegularFirstBagFee = tier.TierExtension.Bags.Bag[0].RegularPrice[0].Price.BasePrice.amount.ToString().Replace(".00", "") + " " + currencyCode;
                                                            }

                                                        }
                                                    }
                                                    #endregion
                                                    chaseCurreny = currencyCode;
                                                }
                                                if (tier.TierExtension.Bags.Bag[0] != null && Convert.ToInt32(tier.TierExtension.Bags.Bag[0].MinQuantity.Trim()) <= 2 && Convert.ToInt32(tier.TierExtension.Bags.Bag[0].MaxQuantity.Trim()) >= 2)
                                                {
                                                    if (!string.IsNullOrEmpty(tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode) && tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode.Trim().ToUpper().Equals("USD"))
                                                    {
                                                        currencyCode = "USD";
                                                    }
                                                    else
                                                    {
                                                        currencyCode = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.currencycode;
                                                    }

                                                    if (currencyCode.Equals("USD"))
                                                    {
                                                        obj1.SecondBagFee = "$" + tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", "");
                                                    }
                                                    else
                                                    {
                                                        obj1.SecondBagFee = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", "") + " " + currencyCode;
                                                    }

                                                    second2BagFee = tier.Pricing[0].PaymentOptions[0].Price[0].TotalPrice.amount.ToString().Replace(".00", "");// Chase FOP Fix

                                                    if (tier.TierInfo.Type == TierInfoTypeType.Allowance)
                                                    {
                                                        if (currencyCode.Equals("USD"))
                                                        {
                                                            obj1.SecondBagFee = "$0";
                                                        }
                                                        else
                                                        {
                                                            obj1.SecondBagFee = "0 " + currencyCode;
                                                        }

                                                        second2BagFee = "0"; // Chase FOP Fix

                                                        if (tier.TierExtension.Bags.Bag[0].RegularPrice != null && tier.TierExtension.Bags.Bag[0].RegularPrice.Length > 0 && tier.TierExtension.Bags.Bag[0].RegularPrice[tier.TierExtension.Bags.Bag[0].RegularPrice.Length - 1].Price != null)
                                                        {
                                                            if (currencyCode.Equals("USD"))
                                                            {
                                                                obj1.RegularSecondBagFee = "$" + tier.TierExtension.Bags.Bag[0].RegularPrice[tier.TierExtension.Bags.Bag[0].RegularPrice.Length - 1].Price.BasePrice.amount.ToString().Replace(".00", "");
                                                            }
                                                            else
                                                            {
                                                                obj1.RegularSecondBagFee = tier.TierExtension.Bags.Bag[0].RegularPrice[tier.TierExtension.Bags.Bag[0].RegularPrice.Length - 1].Price.BasePrice.amount.ToString().Replace(".00", "") + " " + currencyCode;
                                                            }

                                                        }
                                                    }
                                                    chaseCurreny = currencyCode;
                                                }
                                                #endregion                                                
                                            }
                                        }
                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("DOTBaggageGenericExceptionMessage"));
                    }
                    #endregion
                }
                #endregion
            }
            #region // Chase FOP Fix
            if (first1BagFee != "0")
            {
                obj1.IsFirstBagFree = false;
                obj1.RegularFirstBagFee = "";
                chaseFOPMessageText = (_configuration.GetValue<string>("ChaseFOPTextMessage") == null ? string.Empty : _configuration.GetValue<string>("ChaseFOPTextMessage"));
            }
            if (second2BagFee != "0")
            {
                obj1.RegularSecondBagFee = "";
                chaseFOPMessageText = (_configuration.GetValue<string>("ChaseFOPTextMessage") == null ? string.Empty : _configuration.GetValue<string>("ChaseFOPTextMessage"));
            }

            if (!_configuration.GetValue<bool>("SupressFixForCheckedBagsChaseOfferDynamicPrice") && !string.IsNullOrEmpty(first1BagFee) && (Convert.ToDecimal(first1BagFee) > chaseOfferAmount)) //Added this to get the highest first baggage price of IBE,IBELite
            {
                chaseOfferAmount = Convert.ToDecimal(first1BagFee);
                chaseOfferCurrency = chaseCurreny;
            }
            #endregion
            return obj1;
        }


        private MerchOfferRequest MerchOfferRequestWithPNR(DOTBaggageInfoRequest request, string channelId, string channelName, MOBSHOPReservation reservation)
        {
            MerchOfferRequest offerRequest = new MerchOfferRequest();
            RequestCriterionType requestCriterionType = new RequestCriterionType();
            RequestCriterionTypeBookingReferenceIds[] pnrs = new RequestCriterionTypeBookingReferenceIds[1];
            RequestCriterionTypeBookingReferenceIds pnr = new RequestCriterionTypeBookingReferenceIds();
            pnr.BookingReferenceId = request.RecordLocator;
            pnrs[0] = pnr;
            requestCriterionType.BookingReferenceIds = pnrs;

            #region

            ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode[] includeExcludeOffersList =
                new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode[1];
            ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode includeExcludeOffer =
                new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode();
            includeExcludeOffer.ServiceCode = ServiceFilterGroupTypeServiceCode.BAG;
            includeExcludeOffer.ResultAction = ServiceFilterGroupTypeResultAction.Include;
            includeExcludeOffersList[0] = new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode();
            includeExcludeOffersList[0] = includeExcludeOffer;
            requestCriterionType.IncludeExcludeOffers = includeExcludeOffersList;

            #endregion

            #region Traveler

            if (EnableIBEFull() && reservation != null)
            {
                //requestCriterionType.TravelerInfo = new TravelerInfoType();
                if (reservation.TravelersCSL != null && reservation.TravelersCSL.Count > 0)
                {
                    TravelerInfoType travelerInfoType1 = new TravelerInfoType();
                    List<TravelerType> lst = new List<TravelerType>();


                    foreach (var traveler in reservation.TravelersCSL)
                    {
                        TravelerType tType = new TravelerType();
                        tType.Id = traveler.PaxIndex + 1.ToString();

                        tType.GivenName = traveler.FirstName;
                        tType.Surname = traveler.LastName;
                        if (!string.IsNullOrEmpty(traveler.TravelerTypeCode))
                        {
                            tType.PassengerTypeCode =
                                (TravelerTypePassengerTypeCode)
                                    System.Enum.Parse(typeof(TravelerTypePassengerTypeCode), traveler.TravelerTypeCode);
                        }
                        if (!string.IsNullOrEmpty(traveler.GenderCode))
                        {
                            tType.Gender = (GenderType)System.Enum.Parse(typeof(GenderType), (GetGenderCode(traveler.GenderCode)));
                        }
                        tType.TicketingDate = request.TraverlerINfo[0].TicketingDate;
                        tType.TicketingDateSpecified = true;
                        //tType.TicketNumber = traveler;
                        if (traveler.AirRewardPrograms != null && traveler.AirRewardPrograms.Count > 0)
                        {
                            CustomerLoyaltyType[] loyaltyType = new CustomerLoyaltyType[traveler.AirRewardPrograms.Count];
                            for (int l = 0; l < loyaltyType.Count(); l++)
                            {
                                CustomerLoyaltyType clt = new CustomerLoyaltyType()
                                {
                                    ProgramId = traveler.AirRewardPrograms[l].ProgramId,
                                    MemberShipId = traveler.AirRewardPrograms[l].MemberId,
                                    //LoyalLevel = (!string.IsNullOrEmpty(traveler.AirRewardPrograms[l].MemberTierLevel))?((CustomerLoyaltyTypeLoyalLevel)Enum.Parse(typeof(CustomerLoyaltyTypeLoyalLevel), traveler.AirRewardPrograms[l].MemberTierLevel)) : CustomerLoyaltyTypeLoyalLevel.Unknown
                                    LoyalLevel =
                                        (CustomerLoyaltyTypeLoyalLevel)
                                            System.Enum.Parse(typeof(CustomerLoyaltyTypeLoyalLevel),
                                                (traveler.AirRewardPrograms[l].MPEliteLevel + 1).ToString())
                                    //Subscriptions = new CustomerLoyaltyTypeSubscriptions()
                                    //{
                                    //    IsAvailable = false
                                    //}
                                };

                                loyaltyType[l] = clt;
                            }
                            tType.Loyalty = loyaltyType;
                        }
                        lst.Add(tType);
                    }

                    travelerInfoType1.Traveler = lst.ToArray<TravelerType>();
                    requestCriterionType.TravelerInfo = travelerInfoType1;
                }

                #endregion Traveler 

                #region OriginDestinationOptions 

                List<OriginDestinationOptionType> lstOriginDestinationOptions = new List<OriginDestinationOptionType>();

                int i = 0;
                int segmentNumber = 0;
                foreach (var trip in reservation.Trips)
                {
                    OriginDestinationOptionType type = new OriginDestinationOptionType();
                    type.Id = "OD" + ++i;
                    FlightSegmentType[] lstFlightSegments = new FlightSegmentType[trip.FlattenedFlights[0].Flights.Count];
                    int j = 0;
                    foreach (var seg in trip.FlattenedFlights[0].Flights)
                    {
                        ++segmentNumber;
                        FlightSegmentType fs = new FlightSegmentType();

                        fs.DepartureAirport = new Location()
                        {
                            LocationCode = seg.Origin
                        };
                        fs.ArrivalAirport = new Location() { LocationCode = seg.Destination };
                        fs.DepartureDateTime = Convert.ToDateTime(seg.DepartureDateTime);
                        fs.DepartureDateTimeSpecified = true;
                        fs.ArrivalDateTime = Convert.ToDateTime(seg.ArrivalDateTime);
                        fs.ArrivalDateTimeSpecified = true;
                        fs.FlightNumber = seg.FlightNumber;
                        fs.OperatingAirline = seg.OperatingCarrier;
                        fs.MarketingAirline = seg.MarketingCarrier;
                        fs.SegmentNumber = segmentNumber.ToString();
                        fs.ClassOfService = seg.ServiceClass;
                        if ((reservation.IsELF ||
                             (reservation.ShopReservationInfo2 != null &&
                              (reservation.ShopReservationInfo2.IsIBE || reservation.ShopReservationInfo2.IsIBELite))))
                        {
                            ArrayOfAdditionalInfoTypeInfoInfo[] additionalInfo = new ArrayOfAdditionalInfoTypeInfoInfo[1];
                            additionalInfo[0] = new ArrayOfAdditionalInfoTypeInfoInfo()
                            {
                                Name = "BEProductCode",
                                Value = seg.ShoppingProducts[0].ProductCode
                            };
                            fs.AdditionalInfo = additionalInfo;
                        }
                        fs.ActiveInd = FlightDetailsActiveInd.Y;
                        fs.Id = segmentNumber.ToString();

                        lstFlightSegments[j] = fs;
                        j++;
                    }
                    type.FlightSegment = lstFlightSegments;

                    lstOriginDestinationOptions.Add(type);
                }
                PricedItineraryType[] piType = new PricedItineraryType[1];
                piType[0] = new PricedItineraryType();
                piType[0].AirItinerary = new AirItineraryType();
                piType[0].AirItinerary.OriginDestinationOptions = lstOriginDestinationOptions.ToArray();
                requestCriterionType.Items = piType;

                #endregion OriginDestinationOptions
            }

            #region

            RequestorType requestorType = new RequestorType();
            if (_configuration.GetValue<bool>("EnabledMERCHChannels"))
            {
                requestorType.ChannelId = channelId;
                requestorType.ChannelName = channelName;
            }
            else
            {
                requestorType.ChannelId = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID").Trim();// "401";  //Changed per Praveen Vemulapalli email
                requestorType.ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName").Trim();//"MBE";  //Changed per Praveen Vemulapalli email
            }

            #endregion

            offerRequest.RequestCriterion = requestCriterionType;
            offerRequest.Requestor = requestorType;
            offerRequest.TransactionIdentifier = "Mobile_Request_With_RecordLocator_" + request.RecordLocator;
            return offerRequest;
        }

        private string GetGenderCode(string gender)
        {
            switch (gender.ToUpper())
            {
                case "M": return "Male";
                case "F": return "Female";
                default: return "Unknown";
            }
        }

        private MerchOfferRequest BuildBagRequestFromCslreservationDetail(Reservation reservation, string channelId, string channelName)
        {
            if (!_configuration.GetValue<bool>("EnabledMERCHChannels"))
            {
                channelId = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID").Trim();// "401";  //Changed per Praveen Vemulapalli email
                channelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName").Trim();//"MBE";  //Changed per Praveen Vemulapalli email
            }
            var offerRequest = new MerchOfferRequest
            {
                RequestCriterion = new RequestCriterionType
                {
                    IncludeExcludeOffers = IncludeExcludeOffersBag(),
                    BookingReferenceIds = BookingReferenceIds(reservation.ConfirmationID),
                    TravelerInfo = TravelerInfo(reservation.Travelers),
                    Items = PricedItinerary(reservation.FlightSegments)
                },
                Requestor = new RequestorType()
                {
                    ChannelId = channelId,
                    ChannelName = channelName
                },
                TransactionIdentifier = "Mobile_Request_With_RecordLocator_" + reservation.ConfirmationID
            };

            return offerRequest;
        }

        private PricedItineraryType[] PricedItinerary(Collection<ReservationFlightSegment> flightSegments)
        {
            var segmentsWithOd = flightSegments.GroupBy(f => f.TripNumber);

            return new PricedItineraryType[]
            {
                new PricedItineraryType
                {
                    AirItinerary = new AirItineraryType
                    {
                        OriginDestinationOptions = segmentsWithOd.Select(f => new OriginDestinationOptionType
                        {
                            Id = "OD" + f.Key,
                            FlightSegment = f.Select(q => BuildFlightSegmentType(q.FlightSegment)).ToArray()
                        }).ToArray()
                    }
                }
            };
        }

        private FlightSegmentType BuildFlightSegmentType(FlightSegment segment)
        {
            return new FlightSegmentType()
            {
                ActiveInd = FlightDetailsActiveInd.Y,
                ActiveIndSpecified = true,
                Id = segment.SegmentNumber.ToString(),
                DepartureAirport = new Location { LocationCode = segment.DepartureAirport.IATACode },
                ArrivalAirport = new Location { LocationCode = segment.ArrivalAirport.IATACode },
                DepartureDateTime = Convert.ToDateTime(segment.DepartureDateTime),
                DepartureDateTimeSpecified = true,
                ArrivalDateTime = Convert.ToDateTime(segment.ArrivalDateTime),
                ArrivalDateTimeSpecified = true,
                FlightNumber = segment.FlightNumber,
                OperatingAirline = segment.OperatingAirlineCode,
                MarketingAirline = segment.MarketedFlightSegment[0].MarketingAirlineCode,
                SegmentNumber = segment.SegmentNumber.ToString(),
                ClassOfService = segment.BookingClasses[0].Code,
                SegmentStatus = segment.FlightSegmentType,
                AdditionalInfo = AdditionalInfo(segment.Characteristic)
            };
        }

        private ArrayOfAdditionalInfoTypeInfoInfo[] AdditionalInfo(Collection<Characteristic> characteristic)
        {
            var productCode = GetCharactersticValue(characteristic, "ProductCode");
            return string.IsNullOrEmpty(productCode) ? null :
                    new ArrayOfAdditionalInfoTypeInfoInfo[] { new ArrayOfAdditionalInfoTypeInfoInfo() { Name = "BEProductCode", Value = productCode } };
        }

        private string GetCharactersticValue(Collection<Characteristic> characteristics, string code)
        {
            if (characteristics == null || characteristics.Count <= 0) return string.Empty;
            var characteristic = characteristics.FirstOrDefault(c => c != null && c.Code != null
            && !string.IsNullOrEmpty(c.Code) && c.Code.Trim().Equals(code, StringComparison.InvariantCultureIgnoreCase));
            return characteristic == null ? string.Empty : characteristic.Value;
        }

        private TravelerInfoType TravelerInfo(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers)
        {
            var i = 0;
            return new TravelerInfoType
            {
                Traveler = travelers.Select(t => new TravelerType
                {
                    Id = (++i).ToString(),
                    GivenName = t.Person.GivenName,
                    Surname = t.Person.Surname,
                    PassengerTypeCode = CheckIsPassengerMilatryOnDuty(t.Person.Type),
                    PassengerTypeCodeSpecified = true,
                    Gender = Gender(t.Person.Sex),
                    GenderSpecified = true,
                    Loyalty = Loyalty(t.LoyaltyProgramProfile),
                    TicketNumber = TicketedNumber(t.Tickets),
                    TicketingDate = Convert.ToDateTime(TicketingDate(t.Tickets)),
                    TicketingDateSpecified = true
                }).ToArray()
            };
        }

        private TravelerTypePassengerTypeCode CheckIsPassengerMilatryOnDuty(string passengerType)
        {
            if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ACC.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ACC; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ADD.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ADD; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ADT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ADT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.AGT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.AGT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ANN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ANN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ASB.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ASB; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.AST.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.AST; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.BAG.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.BAG; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.BLD.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.BLD; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.BNN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.BNN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.BRV.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.BRV; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.CMA.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ANN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.CMP.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.CMP; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.CNN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.CNN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.CPN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.CPN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.DAT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.DAT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.DIS.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.DIS; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.DOD.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.DOD; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ECH.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ECH; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.EDT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.EDT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.FFP.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.FFP; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.FFY.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.FFY; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.GNN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.GNN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.GRP.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.GRP; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.INF.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.INF; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.INS.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.INS; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MBT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MBT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MCR.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MCR; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MDP.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MDP; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MIL.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MIL; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MIR.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MIR; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MNF.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MNF; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MNN.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MNN; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MNS.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MNS; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MPA.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MPA; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MRE.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MRE; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MSB.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MSB; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MUS.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MUS; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.MXS.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.MXS; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.NAT.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.NAT; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.REC.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.REC; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.REF.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.REF; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.SPA.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.SPA; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.SRC.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.SRC; }
            else if (passengerType.ToString().ToUpper().Trim() == TravelerTypePassengerTypeCode.ZED.ToString().ToUpper().Trim()) { return TravelerTypePassengerTypeCode.ZED; }

            return TravelerTypePassengerTypeCode.ADT;
        }

        private CustomerLoyaltyType[] Loyalty(LoyaltyProgramProfile profile)
        {
            return new CustomerLoyaltyType[]
            {
                new CustomerLoyaltyType()
                {
                    ProgramId = profile.LoyaltyProgramCarrierCode,
                    MemberShipId = profile.LoyaltyProgramMemberID,
                    LoyalLevel = GetLoyaltyLevel(profile.LoyaltyProgramMemberTierDescription),
                    Subscriptions = new CustomerLoyaltyTypeSubscriptions() {IsAvailable = false}
                }
            };
        }

        private CustomerLoyaltyTypeLoyalLevel GetLoyaltyLevel(LoyaltyProgramMemberTierLevel loyaltyProgramMemberTierDescription)
        {
            switch (loyaltyProgramMemberTierDescription)
            {
                case LoyaltyProgramMemberTierLevel.GeneralMember:
                    return CustomerLoyaltyTypeLoyalLevel.GeneralMember;
                case LoyaltyProgramMemberTierLevel.PremierSilver:
                    return CustomerLoyaltyTypeLoyalLevel.PremierSilver;
                case LoyaltyProgramMemberTierLevel.PremierGold:
                    return CustomerLoyaltyTypeLoyalLevel.PremierGold;
                case LoyaltyProgramMemberTierLevel.Premier1K:
                    return CustomerLoyaltyTypeLoyalLevel.Premier1K;
                case LoyaltyProgramMemberTierLevel.PremierPlatinum:
                    return CustomerLoyaltyTypeLoyalLevel.PremierPlatinum;
                case LoyaltyProgramMemberTierLevel.GlobalServices:
                    return CustomerLoyaltyTypeLoyalLevel.GlobalServices;
                case LoyaltyProgramMemberTierLevel.StarSilver:
                    return CustomerLoyaltyTypeLoyalLevel.StarAllianceSilver;
                case LoyaltyProgramMemberTierLevel.StarGold:
                    return CustomerLoyaltyTypeLoyalLevel.StarAllianceGold;
                default:
                    return CustomerLoyaltyTypeLoyalLevel.Unknown;
            }
        }

        private GenderType Gender(string sex)
        {
            sex = sex ?? string.Empty;
            switch (sex.ToUpper().Trim())
            {
                case "M":
                    return GenderType.Male;
                case "F":
                    return GenderType.Female;
                default:
                    return GenderType.Unknown;
            }
        }

        private RequestCriterionTypeBookingReferenceIds[] BookingReferenceIds(string recordLocator)
        {
            return new[]
            {
                new RequestCriterionTypeBookingReferenceIds
                {
                    BookingReferenceId = recordLocator
                }
            };
        }

        private ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode[] IncludeExcludeOffersBag()
        {
            return new[]
            {
                new ArrayOfIncludeExcludeOffersTypeServiceFilterCodeServiceFilterCode()
                {
                    ServiceCode = ServiceFilterGroupTypeServiceCode.BAG,
                    ResultAction = ServiceFilterGroupTypeResultAction.Include,
                    ServiceSubType = ServiceFilterGroupTypeServiceSubType.BAGPOLICYCALC,
                    ServiceSubTypeSpecified = true
                }
            };
        }

        private bool EnableIBEFull()
        {
            return _configuration.GetValue<bool>("EnableIBE");
        }


        public string GetPnrCreatedDate(Reservation cslReservation)
        {
            if (cslReservation == null || cslReservation.CreateDate == null)
                return null;

            return GeneralHelper.FormatDatetime(Convert.ToDateTime(cslReservation.CreateDate).ToString("yyyyMMdd hh:mm tt"), "en-US");
        }

        public async System.Threading.Tasks.Task<(MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess)> PBAndPADetailAssignment_CFOP(string sessionId, MOBApplication application, string recordLocator, string lastname, string pnrCreateDate, MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess, string jsonRequest, string jsonResponse, bool isTravelOptionsRequest = false)
        {
            Service.Presentation.ProductResponseModel.ProductOffer productOffers = null;
            DynamicOfferDetailResponse dodResponse = null;
            bool isEnableTravelOptionsInViewRes = _shoppingUtility.IsEnableTravelOptionsInViewRes(application.Id, application?.Version?.Major) && isTravelOptionsRequest;
            if (isEnableTravelOptionsInViewRes)
                dodResponse = JsonConvert.DeserializeObject<DynamicOfferDetailResponse>(jsonResponse);
            else
                productOffers = JsonConvert.DeserializeObject<Service.Presentation.ProductResponseModel.ProductOffer>(jsonResponse);

            if ((productOffers != null && (productOffers.Response.Error == null || !productOffers.Response.Error.Any())) || (dodResponse != null && (dodResponse.Response?.Error == null || !dodResponse.Response.Error.Any())))
            {
                int custCount = 0;
                if (EnablePBS(application.Id, application.Version.Major))
                {
                    var offerResponse = isEnableTravelOptionsInViewRes ? BuildIndividualProductOffers(dodResponse, "PBS") : BuildIndividualProductOffer(productOffers, "PBS");
                    priorityBoarding = GetPBDetail(offerResponse, ref custCount);
                }
                if (priorityBoarding != null && priorityBoarding.Segments.IsNotNullNorEmpty() && priorityBoarding.Segments.Any())
                {
                    await SavePBInFile(sessionId, recordLocator, lastname, pnrCreateDate, priorityBoarding, jsonResponse, custCount);
                }
                else if (_configuration.GetValue<bool>("PASXMLtoCSLMigration"))
                {
                    string selectedCustomerInSegments = string.Empty;
                    bool isSavePACall = false;
                    var offerResponse = isEnableTravelOptionsInViewRes ? BuildIndividualProductOffers(dodResponse, "PAS") : BuildIndividualProductOffer(productOffers, "PAS");
                    premierAccess = GetPADetail_CFOP(sessionId, recordLocator, lastname, offerResponse, selectedCustomerInSegments, isSavePACall);
                    await SavePAInFile(sessionId, recordLocator, lastname, pnrCreateDate, jsonRequest, jsonResponse);
                }
            }
            else if (CheckInvalidLogError(productOffers))
            {
                priorityBoarding = null;
                premierAccess = null;
            }
            else if (!CheckInvalidLogError(productOffers))
            {
                throw new MOBUnitedException("Merch service sending null or error response");
            }

            return (priorityBoarding, premierAccess);
        }

        #region PriorityBoarding
        private bool CheckInvalidLogError(Service.Presentation.ProductResponseModel.ProductOffer offerResponse)
        {
            return _configuration.GetValue<bool>("PBS_HotFix_Do_Not_Log_Invalid_PNR") &&
                offerResponse != null && offerResponse.Response != null && offerResponse.Response.Error != null && offerResponse.Response.Error.Any(e => e != null && !string.IsNullOrEmpty(e.Code) && e.Code == "80003.06");
        }
        private bool CheckInvalidLogError(DynamicOfferDetailResponse offerResponse)
        {
            return _configuration.GetValue<bool>("PBS_HotFix_Do_Not_Log_Invalid_PNR") &&
                offerResponse != null && offerResponse.Response != null && offerResponse.Response.Error != null && offerResponse.Response.Error.Any(e => e != null && !string.IsNullOrEmpty(e.Code) && e.Code == "80003.06");
        }

        private ProductDetail GetProductDetails(ProductOffer offersResponse, string productCode)
        {
            var productDetail = new ProductDetail();
            if (offersResponse != null && !offersResponse.Offers.IsListNullOrEmpty() && !offersResponse.Offers[0].IsNullOrEmpty()
                && !offersResponse.Offers[0].ProductInformation.IsNullOrEmpty() && !offersResponse.Offers[0].ProductInformation.ProductDetails.IsListNullOrEmpty() && !string.IsNullOrEmpty(productCode))
            {
                productDetail = offersResponse.Offers[0].ProductInformation.ProductDetails.FirstOrDefault(p => p != null && p.Product != null && p.Product.Code.ToUpper().Trim() == productCode);
            }
            else
            {
                productDetail = null;
            }
            return productDetail;
        }

        private MOBPriorityBoarding BuildPBDetailBaseOnSegment(ProductOffer offerResponse, ref int custCount, bool isPostBooking)
        {
            var priorityBoarding = new MOBPriorityBoarding();
            double lowestPrice = 0;
            priorityBoarding.Segments = BuildPbOfferBaseOnSegment(offerResponse, ref lowestPrice, isPostBooking);

            if (priorityBoarding.Segments.IsNullOrEmpty())
                return null;

            BuildPBPurchasedTextBaseOnSegment(priorityBoarding);

            // client need segment and customer info for building purchase confirmation message. When lowest price is Zero, send segments only.
            if (lowestPrice == 0)
            {
                priorityBoarding.PbOfferTileInfo = null;
                priorityBoarding.PbOfferDetails = null;
                priorityBoarding.TAndC = null;
                return priorityBoarding;
            }

            priorityBoarding.PbOfferTileInfo = new Mobile.Model.MPRewards.MOBOfferTile()
            {
                CurrencyCode = "$",
                Price = Convert.ToDecimal(lowestPrice),
                Text1 = "Travel made easier with",
                Text2 = "From",
                Text3 = "per person",
                Title = "Priority Boarding"
            };
            priorityBoarding.TAndC = GetPBContentList("PriorityBoardingTermsAndConditionsList");
            priorityBoarding.PbOfferDetails = GetPBContentList("PriorityBoardingOfferDetailsList");
            priorityBoarding.ProductCode = (offerResponse.Offers[0] != null) ? offerResponse.Offers[0].ProductInformation.ProductDetails[0].Product.Code : null;
            priorityBoarding.ProductName = (offerResponse.Offers[0] != null) ? offerResponse.Offers[0].ProductInformation.ProductDetails[0].Product.Description : null;
            if (!offerResponse.IsNullOrEmpty() && offerResponse.Travelers.IsNotNullNorEmpty() && offerResponse.Travelers.Any())
            {
                custCount = offerResponse.Travelers.Count();
            }
            return priorityBoarding;
        }

        private void BuildPBPurchasedTextBaseOnSegment(MOBPriorityBoarding priorityBoarding)
        {
            if (!priorityBoarding.IsNullOrEmpty() && string.IsNullOrEmpty(priorityBoarding.PbDetailsConfirmationTxt) && !priorityBoarding.Segments.IsNullOrEmpty())
            {
                if (priorityBoarding.Segments.Any(segment => segment.PbSegmentType == MOBPBSegmentType.AlreadyPurchased))
                {
                    priorityBoarding.PbDetailsConfirmationTxt = _configuration.GetValue<string>("PriorityBoardingConfirmationTxtBaseOnSegment");
                    priorityBoarding.PbAddedTravelerTxt = _configuration.GetValue<string>("PriorityBoardingAddedSegmentTxt");
                }
            }
        }

        private List<MOBPBSegment> BuildPbOfferBaseOnSegment(ProductOffer offerResponse, ref double lowestPrice, bool isPostBooking)
        {
            List<MOBPBSegment> segments = new List<MOBPBSegment>();
            var productCode = "PBS";
            var productDetail = GetProductDetails(offerResponse, productCode);
            if (productDetail == null) return null;
            if (isPostBooking && (offerResponse.FlightSegments == null || !offerResponse.FlightSegments.Any()))
                return null;
            // all segments with basic info as for date, origination and destinaton 
            segments = isPostBooking
                ? BuildSegmentsBasicInfoForPostBookingPath(offerResponse.FlightSegments)
                : BuildSegmentBasicInfoBaseOnSegment(offerResponse.Solutions[0].ODOptions);
            if (segments.IsNullOrEmpty())
                return null;

            segments = GetAllPricesInfoBaseOnSegment(productDetail, segments, ref lowestPrice);

            if (segments.IsNullOrEmpty())
                return null;
            return segments;
        }

        private List<MOBPBSegment> GetAllPricesInfoBaseOnSegment(Service.Presentation.ProductResponseModel.ProductDetail produtDetail, List<MOBPBSegment> segments, ref double lowestPrice)
        {
            if (produtDetail.IsNullOrEmpty())
                return null;

            if (produtDetail.InEligibleSegments.IsNotNullNorEmpty())
            {
                foreach (var ineligibleSegment in produtDetail.InEligibleSegments.Where(i => !i.Assocatiation.IsNullOrEmpty() && i.Assocatiation.SegmentRefIDs.IsNotNullNorEmpty()))
                {
                    foreach (var segmentID in ineligibleSegment.Assocatiation.SegmentRefIDs)
                    {
                        // if we found out PA is purchased or PA is offered, then we jump out from the loop. 
                        segments = BuildSegmentDetailInfoWithNoPBOfferedBaseOnSegment(segments, ineligibleSegment.Reason, segmentID);
                        if (segments == null)
                        {
                            return null;
                        }
                    }
                }
                // if all segments not offered with PBS or already included in Premier/ first calss, send back null segments 
                if (!segments.Any(s => s.PbSegmentType == MOBPBSegmentType.Regular || s.PbSegmentType == MOBPBSegmentType.AlreadyPurchased))
                {
                    return null;
                }
            }

            if (!produtDetail.Product.IsNullOrEmpty() && !produtDetail.Product.SubProducts.IsListNullOrEmpty())
            {
                var prices = produtDetail.Product.SubProducts.Where(s => s.InEligibleReason.IsNullOrEmpty() && s.Prices.IsNotNullNorEmpty()).SelectMany(p => p.Prices.Where(a => a.Association != null && a.PaymentOptions != null));
                BuildSegmentDetailPriceInfo(segments, prices, ref lowestPrice);
            }
            return segments;
        }

        private List<MOBPBSegment> BuildSegmentDetailInfoWithNoPBOfferedBaseOnSegment(List<MOBPBSegment> segments, InEligibleReason inEligibleReason, string segId)
        {
            if (!inEligibleReason.IsNullOrEmpty() && !segments.IsNullOrEmpty())
            {
                foreach (var segment in segments.Where(s => s.SegmentId == segId))
                {
                    BuildSegmentsNotOfferMessageWithReasonBaseOnSegment(segment, inEligibleReason);
                    if (segment.IsNullOrEmpty() || string.IsNullOrEmpty(segment.SegmentId))
                    {
                        segments = null;
                        break;
                    }
                }
            }
            return segments;
        }

        private void BuildSegmentsNotOfferMessageWithReasonBaseOnSegment(MOBPBSegment segment, InEligibleReason inEligibleReason)
        {
            if (segment.IsNullOrEmpty() || inEligibleReason.IsNullOrEmpty())
            {
                segment.SegmentId = string.Empty;
            }
            else if (segment.PbSegmentType == MOBPBSegmentType.Regular)
            {
                switch (string.Format("{0},{1}", inEligibleReason.MajorCode.Trim(), inEligibleReason.MinorCode.Trim()))
                {
                    // PBS Already purchased
                    case "010,3":
                        segment.PbSegmentType = MOBPBSegmentType.AlreadyPurchased;
                        segment.Fee = 0;
                        segment.Message = _configuration.GetValue<string>("PriorityBoardingAlreadyPurchasedMessage");
                        break;
                    // PBS  offered only for Economy
                    case "010,4":
                    case "010,7":
                        segment.PbSegmentType = MOBPBSegmentType.Included;
                        segment.Message = _configuration.GetValue<string>("PriorityBoardingIncludedMessage");
                        segment.Fee = 0;
                        break;
                    //PBS not Offered 
                    case "010,1":
                    case "010,2":
                    case "010,5":
                    case "010,10":
                    case "010,18":
                    case "010,19":
                    case "010,22":
                    case "002,3":
                        segment.SegmentId = string.Empty;
                        break;
                    default:
                        segment.PbSegmentType = MOBPBSegmentType.InEligible;
                        segment.Message = _configuration.GetValue<string>("PriorityBoardingNotAvailableMessage");
                        segment.Fee = 0;
                        break;
                }
            }
        }

        private void BuildSegmentDetailPriceInfo(List<MOBPBSegment> segmentsWithBasics, IEnumerable<ProductPriceOption> prices, ref double lowestPrice)
        {
            if (prices.IsNull()) return;
            foreach (var segment in segmentsWithBasics)
            {
                segment.Fee = 0;
                foreach (var price in prices.
                    Where(p => p.PaymentOptions.IsNotNullNorEmpty()
                               && !p.PaymentOptions[0].IsNullOrEmpty()
                               && p.PaymentOptions[0].PriceComponents.IsNotNullNorEmpty()
                               && !p.PaymentOptions[0].PriceComponents[0].IsNullOrEmpty()
                               && !p.PaymentOptions[0].PriceComponents[0].Price.IsNullOrEmpty()
                               && p.PaymentOptions[0].PriceComponents[0].Price.Totals.IsNotNullNorEmpty()
                               && !p.PaymentOptions[0].PriceComponents[0].Price.Totals[0].IsNullOrEmpty()
                               && p.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount > 0))
                {
                    if (segment.SegmentId == price.Association.SegmentRefIDs[0] && segment.PbSegmentType == MOBPBSegmentType.Regular)
                    {
                        int fee = Convert.ToInt32(price.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount);
                        segment.Message = string.Format("{0}{1}{2}", "$", fee, "/traveler");
                        lowestPrice = GetLowest(lowestPrice, fee);
                        segment.CustPrice = fee;
                        segment.Fee = segment.Fee + fee;
                        if (segment.OfferIds.IsListNullOrEmpty())
                        {
                            segment.OfferIds = new List<string>();
                        }
                        segment.OfferIds.Add(price.ID);
                    }
                }
            }
        }

        private double GetLowest(double lowestPrice, double fee)
        {
            if (lowestPrice <= 0 && fee >= 0)
            {
                lowestPrice = fee;
            }
            else if (lowestPrice > fee && fee > 0)
            {
                lowestPrice = fee;
            }

            return lowestPrice;
        }


        private List<MOBPBSegment> BuildSegmentBasicInfoBaseOnSegment(Collection<ProductOriginDestinationOption> oDOptions)
        {
            List<MOBPBSegment> segmentsWithBasicsInfo = new List<MOBPBSegment>() { };

            if (oDOptions == null || !oDOptions.Any())
                return null;

            foreach (var flightSegment in oDOptions.Where(o => o.FlightSegments != null && o.FlightSegments.Count > 0).SelectMany(f => f.FlightSegments.Where(s => s != null && !string.IsNullOrEmpty(s.IsActive) && s.IsActive.ToUpper().Trim() == "Y")))
            {
                MOBPBSegment segment = new MOBPBSegment() { };
                segment.SegmentId = flightSegment.ID;
                segment.Origin = flightSegment.DepartureAirport.IATACode.ToString();
                segment.Destination = flightSegment.ArrivalAirport.IATACode.ToString();
                segment.FlightDate = string.Empty;
                segment.Customers = null;
                segment.OfferIds = new List<string>();
                segment.PbSegmentType = MOBPBSegmentType.Regular;
                segmentsWithBasicsInfo.Add(segment);
            }
            return segmentsWithBasicsInfo;
        }

        private List<MOBPBSegment> BuildSegmentsBasicInfoForPostBookingPath(Collection<ProductFlightSegment> flightSegments)
        {
            var segmentsWithBasicInfo = new List<MOBPBSegment>();
            if (flightSegments.IsListNullOrEmpty())
            {
                return null;
            }
            foreach (var flightSegment in flightSegments.
                Where(fs => fs != null && !string.IsNullOrEmpty(fs.ID)
                && fs.DepartureAirport != null && !string.IsNullOrEmpty(fs.DepartureAirport.IATACode)
                && fs.ArrivalAirport != null && !string.IsNullOrEmpty(fs.ArrivalAirport.IATACode)))
            {
                MOBPBSegment segment = new MOBPBSegment() { };
                segment.SegmentId = flightSegment.ID;
                segment.Origin = flightSegment.DepartureAirport.IATACode.ToString();
                segment.Destination = flightSegment.ArrivalAirport.IATACode.ToString();
                segment.FlightDate = string.Empty;
                segment.Customers = null;
                segment.PbSegmentType = MOBPBSegmentType.Regular;
                segment.OfferIds = new List<string>();
                segmentsWithBasicInfo.Add(segment);
            }
            return segmentsWithBasicInfo;
        }

        private MOBPriorityBoarding BuildPBDetail(ProductOffer offerResponse)
        {
            var priorityBoarding = new MOBPriorityBoarding();
            double lowestPrice = 0;
            priorityBoarding.Segments = BuildSegmentsForPbOffer(offerResponse);

            if (priorityBoarding.Segments.IsNullOrEmpty())
                return null;

            BuildPBPurchasedText(priorityBoarding);

            lowestPrice = GetLowestPrice(priorityBoarding.Segments);

            // client need segment and customer info for building purchase confirmation message. When lowest price is Zero, send segments only.
            if (lowestPrice == 0)
            {
                priorityBoarding.PbOfferTileInfo = null;
                priorityBoarding.PbOfferDetails = null;
                priorityBoarding.TAndC = null;
                return priorityBoarding;
            }

            priorityBoarding.PbOfferTileInfo = new Mobile.Model.MPRewards.MOBOfferTile()
            {
                CurrencyCode = "$",
                Price = Convert.ToDecimal(lowestPrice),
                Text1 = "Travel made easier with",
                Text2 = "From",
                Text3 = "per person",
                Title = "Priority Boarding"
            };
            priorityBoarding.ProductCode = (offerResponse.Offers[0] != null) ? offerResponse.Offers[0].ProductInformation.ProductDetails[0].Product.Code : null;
            priorityBoarding.ProductName = (offerResponse.Offers[0] != null) ? offerResponse.Offers[0].ProductInformation.ProductDetails[0].Product.Description : null;
            priorityBoarding.TAndC = GetPBContentList("PriorityBoardingTermsAndConditionsList");
            priorityBoarding.PbOfferDetails = GetPBContentList("PriorityBoardingOfferDetailsList");
            return priorityBoarding;
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

        private double GetLowestPrice(List<MOBPBSegment> segments)
        {
            if (segments == null || !segments.Any())
                return 0;

            return segments.Where(seg => seg.Customers != null).SelectMany(s => s.Customers).Where(c => c.Fee > 0).DefaultIfEmpty(new MOBPBCustomer()).Min(m => m.Fee);
        }

        private void BuildPBPurchasedText(MOBPriorityBoarding priorityBoarding)
        {
            if (!priorityBoarding.IsNullOrEmpty() && string.IsNullOrEmpty(priorityBoarding.PbDetailsConfirmationTxt) && !priorityBoarding.Segments.IsNullOrEmpty())
            {
                if (priorityBoarding.Segments.Any(segment => segment.PbSegmentType == MOBPBSegmentType.AlreadyPurchased || (!segment.Customers.IsNullOrEmpty() && segment.Customers.Any(cust => cust.AlreadyPurchased))))
                {
                    priorityBoarding.PbDetailsConfirmationTxt = _configuration.GetValue<string>("PriorityBoardingConfirmationTxt");
                    priorityBoarding.PbAddedTravelerTxt = _configuration.GetValue<string>("PriorityBoardingAddedTravelerTxt");
                }
            }
        }

        private List<MOBPBSegment> BuildSegmentsForPbOffer(ProductOffer offerResponse)
        {
            List<MOBPBSegment> segments = new List<MOBPBSegment>();
            // All travelers with basic info as for name and custmerId 
            var travelersBasics = new List<MOBPBCustomer>();
            var productCode = "PBS";
            var productDetail = GetProductDetails(offerResponse, productCode);
            if (productDetail == null) return null;
            // all segments with basic info as for date, origination and destinaton 
            segments = BuildSegmentBasicInfo(offerResponse.Solutions[0].ODOptions);

            travelersBasics = BuildTravelerBasicInfo(offerResponse.Travelers);

            if (segments.IsNullOrEmpty() || travelersBasics.IsNullOrEmpty())
                return null;

            segments = GetAllPricesInfo(productDetail, segments, travelersBasics);

            if (segments.IsNullOrEmpty())
                return null;
            return segments;
        }

        private List<MOBPBSegment> GetAllPricesInfo(Service.Presentation.ProductResponseModel.ProductDetail produtDetail, List<MOBPBSegment> segments, List<MOBPBCustomer> travelersBasics)
        {
            if (produtDetail.IsNullOrEmpty())
                return null;

            if (produtDetail.InEligibleSegments.IsNotNullNorEmpty())
            {
                foreach (var ineligibleSegment in produtDetail.InEligibleSegments)
                {
                    // if we found out PA is purchased or PA is offered, then we jump out from the loop. 
                    segments = BuildSegmentDetailInfoWithNoPBOffered(segments, travelersBasics, ineligibleSegment.Reason, ineligibleSegment.Assocatiation.TravelerRefIDs[0], ineligibleSegment.Assocatiation.SegmentRefIDs[0]);
                    if (segments == null)
                    {
                        return null;
                    }
                }
                ChangeSegmentTypeWhenAllSegmentsPurchased(segments, travelersBasics.Count());
                // if all segments not offered with PBS or already included in Premier/ first calss, send back null segments 
                if (!segments.Any(s => s.PbSegmentType == MOBPBSegmentType.Regular || s.PbSegmentType == MOBPBSegmentType.AlreadyPurchased))
                {
                    return null;
                }
            }

            if (produtDetail.Product.SubProducts.IsNotNullNorEmpty())
            {
                var prices = produtDetail.Product.SubProducts.Where(s => s.InEligibleReason.IsNullOrEmpty() && s.Prices.IsNotNullNorEmpty()).SelectMany(p => p.Prices.Where(a => a.Association != null && a.PaymentOptions != null));
                foreach (var price in prices.Where(p => !p.IsNullOrEmpty()))
                {
                    BuildSegmentDetailInfo(segments, travelersBasics, price);
                }
            }

            return segments;
        }

        private void BuildSegmentDetailInfo(List<MOBPBSegment> segmentsWithBasics, List<MOBPBCustomer> travelersBasics, ProductPriceOption price)
        {
            if (travelersBasics.IsNotNullNorEmpty())
            {
                foreach (var segment in segmentsWithBasics)
                {
                    if (segment.SegmentId == price.Association.SegmentRefIDs[0])
                    {
                        var customer = BuildTravelerDetailInfo(price, travelersBasics, segment.SegmentId);
                        segment.Customers.Add(customer);
                        // has to be changed if not eligible for all custmers in this segment 
                        segment.PbSegmentType = MOBPBSegmentType.Regular;
                    }
                }
            }
        }

        private MOBPBCustomer BuildTravelerDetailInfo(ProductPriceOption price, List<MOBPBCustomer> allTravelers, string segmentId)
        {
            MOBPBCustomer customerDetail = new MOBPBCustomer() { };
            if (allTravelers.IsNullOrEmpty()) return customerDetail;
            foreach (var traveler in allTravelers)
            {
                if (traveler != null && traveler.CustId == price.Association.TravelerRefIDs[0])
                {
                    customerDetail.CustId = traveler.CustId;
                    customerDetail.CustName = traveler.CustName;
                    customerDetail.Fee = price.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount;
                    customerDetail.FormattedFee = string.Format("${0:0}", customerDetail.Fee);
                    customerDetail.TagId = string.Format("{0}-{1}", segmentId, customerDetail.CustId);
                }

            }
            return customerDetail;
        }

        private void ChangeSegmentTypeWhenAllSegmentsPurchased(List<MOBPBSegment> segments, int travelerNumber)
        {
            if (segments.IsNotNullNorEmpty())
            {
                segments.Where(
                    s =>
                        s.Customers.IsNotNullNorEmpty() &&
                        s.Customers.Count(t => t.AlreadyPurchased) == travelerNumber)
                    .EachFor(a =>
                    {
                        a.Message = "All travelers have Priority Boarding on this flight";
                        a.PbSegmentType = MOBPBSegmentType.AlreadyPurchased;
                    });
            }
        }

        private List<MOBPBSegment> BuildSegmentDetailInfoWithNoPBOffered(List<MOBPBSegment> segments, List<MOBPBCustomer> travelersBasics, InEligibleReason inEligibleReason, string custId, string segId)
        {
            if (!inEligibleReason.IsNullOrEmpty() && !segments.IsNullOrEmpty())
            {
                foreach (var segment in segments.Where(s => s.SegmentId == segId))
                {
                    BuildSegmentsNotOfferMessageWithReason(segment, custId, inEligibleReason, travelersBasics);
                    if (segment.IsNullOrEmpty() || string.IsNullOrEmpty(segment.SegmentId))
                    {
                        segments = null;
                        break;
                    }
                }
            }
            return segments;
        }

        private void BuildSegmentsNotOfferMessageWithReason(MOBPBSegment segment, string custId, InEligibleReason inEligibleReason, List<MOBPBCustomer> travelersBasics)
        {
            if (segment.IsNullOrEmpty() || inEligibleReason.IsNullOrEmpty())
            {
                segment.SegmentId = string.Empty;
            }
            else
            {
                switch (string.Format("{0},{1}", inEligibleReason.MajorCode.Trim(), inEligibleReason.MinorCode.Trim()))
                {
                    // PBS Already purchased
                    case "010,3":
                        if (segment.Customers.IsNullOrEmpty() || segment.Customers.All(c => c.CustId != custId))
                        {
                            MOBPBCustomer customer = BuildTravelerDetailInfoWithNoPB(travelersBasics, segment.SegmentId, custId);
                            if (!customer.IsNullOrEmpty())
                                segment.Customers.Add(customer);
                        }
                        break;
                    // PBS  offered only for Economy
                    case "010,4":
                    case "010,7":
                        segment.PbSegmentType = MOBPBSegmentType.Included;
                        segment.Message = "Priority Boarding is included for this flight";
                        break;
                    //PBS not Offered 
                    case "010,1":
                    case "010,2":
                    case "010,5":
                    case "010,10":
                    case "010,18":
                    case "010,19":
                    case "010,22":
                    case "002,3":
                        segment.SegmentId = string.Empty;
                        break;
                    default:
                        segment.PbSegmentType = MOBPBSegmentType.InEligible;
                        segment.Message = "Priority Boarding is not offered for this flight";
                        break;
                }
            }
        }

        private MOBPBCustomer BuildTravelerDetailInfoWithNoPB(List<MOBPBCustomer> travelersBasics, string segmentId, string travelerId)
        {
            MOBPBCustomer customer = new MOBPBCustomer();

            if (travelersBasics.IsNullOrEmpty())
                return customer;

            foreach (var traveler in travelersBasics.Where(t => t.CustId == travelerId))
            {
                customer.CustId = travelerId;
                customer.CustName = traveler.CustName;
                customer.Fee = 0;
                customer.Message = "Already purchased for this flight";
                customer.AlreadyPurchased = true;
                customer.TagId = string.Format("{0}-{1}", segmentId, customer.CustId);
            }
            return customer;
        }

        private List<MOBPBCustomer> BuildTravelerBasicInfo(Collection<ProductTraveler> travelers)
        {
            List<MOBPBCustomer> customers = new List<MOBPBCustomer>() { };
            foreach (var traveler in travelers)
            {
                MOBPBCustomer customer = new MOBPBCustomer() { };
                customer.CustId = traveler.CustomerID;
                customer.CustName = string.Format("{0} {1}", traveler.GivenName, traveler.Surname);
                customers.Add(customer);
            }
            return customers;
        }

        private List<MOBPBSegment> BuildSegmentBasicInfo(Collection<ProductOriginDestinationOption> oDOptions)
        {
            List<MOBPBSegment> segmentsWithBasicsInfo = new List<MOBPBSegment>() { };

            if (oDOptions == null || !oDOptions.Any())
                return null;

            foreach (var flightSegment in oDOptions.Where(o => o.FlightSegments != null && o.FlightSegments.Any()).SelectMany(f => f.FlightSegments.Where(s => !s.IsNullOrEmpty())))
            {
                MOBPBSegment segment = new MOBPBSegment() { };
                segment.SegmentId = flightSegment.ID;
                segment.Origin = flightSegment.DepartureAirport.IATACode.ToString();
                segment.Destination = flightSegment.ArrivalAirport.IATACode.ToString();
                //Wed. Jun. 09, 2010;
                segment.FlightDate =
                    Convert.ToDateTime(flightSegment.DepartureDateTime).ToString("ddd. MMM. dd, yyyy");
                segment.Customers = new List<MOBPBCustomer>() { };
                segmentsWithBasicsInfo.Add(segment);
            }
            return segmentsWithBasicsInfo;
        }
        #endregion

        private async System.Threading.Tasks.Task SavePAInFile(string sessionId, string recordLocator, string lastName, string pnrCreateDate, string jsonRequest, string jsonResponse)
        {
            PremierAccess pa = new PremierAccess();
            pa.LastName = lastName;
            pa.RecordLocator = recordLocator;
            pa.RequestXml = jsonRequest;
            pa.ResponseXml = jsonResponse;
            pa.PnrCreateDate = pnrCreateDate;
            await _sessionHelperService.SaveSession<PremierAccess>(pa, sessionId, new List<string> { sessionId, pa.ObjectName + "_GetPAOffersForPNR" }, pa.ObjectName + "_GetPAOffersForPNR").ConfigureAwait(false);
        }

        private MOBPremierAccess GetPADetail_CFOP(string sessionId, string recordLocator, string lastName, Service.Presentation.ProductResponseModel.ProductOffer offerResponse, string selectedCustomerInSegments, bool isSavePACall)
        {
            MOBPremierAccess premierAccess = ReadOfferCSLResponse_CFOP(offerResponse, selectedCustomerInSegments, isSavePACall);
            premierAccess.PAOffersSessionId = sessionId;
            premierAccess.RecordLocator = recordLocator;
            premierAccess.LastName = lastName;
            if (!CheckPAOffersStatus(premierAccess))
            {
                premierAccess.ErrorMessage = _configuration.GetValue<string>("NoPremierMerchandizeOffersAvailabeMessage").Trim();
            }

            return premierAccess;
        }

        private bool CheckPAOffersStatus(MOBPremierAccess premierAccess)
        {
            #region
            bool showPA = false;
            if (premierAccess != null && premierAccess.Segments != null)
            {
                foreach (MOBPASegment paSegment in premierAccess.Segments)
                {
                    if (paSegment.PASegmentType == MOBPASegmentType.Regular)
                    {
                        showPA = true;
                        break;
                    }
                }
            }
            return showPA;
            #endregion
        }

        private MOBPremierAccess ReadOfferCSLResponse_CFOP(Service.Presentation.ProductResponseModel.ProductOffer offersResponse, string selectedCustomerInSegments, bool isSavePACall)
        {
            MOBPremierAccess premierAccess = new MOBPremierAccess();
            var productCode = "PAS";
            var productDetail = GetProductDetails(offersResponse, productCode);
            if (productDetail == null) return premierAccess;
            premierAccess.ProductCode = productDetail.Product.Code;
            premierAccess.ProductName = productDetail.Product.DisplayName;
            GetPASegmentsBasicInfo_CFOP(offersResponse, selectedCustomerInSegments, isSavePACall, premierAccess);
            BuildSegmentsWithPriceInfo_CFOP(productDetail, premierAccess);
            decimal lowestPrice = BuildPaInfoForSelectAsBundleAndGetLowestPrice(premierAccess);
            premierAccess.PAOfferTileInfo = GetPremierAccessOffer(lowestPrice);
            premierAccess.TAndC = GetPATermsAndConditionsList();
            AllCustomersPaymentCheckInSegmentLevel(premierAccess.Segments);
            return premierAccess;
        }

        private void AllCustomersPaymentCheckInSegmentLevel(List<MOBPASegment> segments)
        {
            if (segments.IsNotNullNorEmpty())
            {
                foreach (MOBPASegment segment in segments.Where(s => !s.IsNullOrEmpty()))
                {
                    int alReadyPurchasedCustomerCount =
                        segment.Customers.Count(
                            t =>
                                t.paCustomerType == MOBPACustomerType.AlreadyPurchased ||
                                t.paCustomerType == MOBPACustomerType.AlreadyPremier);
                    if (alReadyPurchasedCustomerCount == segment.Customers.Count)
                    {
                        segment.PASegmentType = MOBPASegmentType.AlreadyPurchased;
                        // If all customers Purchased PA in one same segment for a Multi Segment PNR then segmentTYpe sould also be Already Purchaed Flag.
                    }
                }
            }
        }

        private List<MOBTypeOption> GetPATermsAndConditionsList()
        {
            List<MOBTypeOption> tAndCList = new List<MOBTypeOption>();
            if (_configuration.GetValue<string>("PremierAccessTermsAndConditionsList") != null)
            {
                string premierAccessTermsAndConditionsList = HttpUtility.HtmlDecode(_configuration.GetValue<string>("PremierAccessTermsAndConditionsList"));
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

        private MOBPremierAccessOfferTileInfo GetPremierAccessOffer(decimal lowestPrice)
        {
            MOBPremierAccessOfferTileInfo paOffer = null;

            try
            {
                if (lowestPrice > 0)
                {
                    paOffer = new MOBPremierAccessOfferTileInfo();
                    paOffer.Price = lowestPrice;
                    paOffer.CurrencyCode = "$";//currencyCode;
                    paOffer.OfferTitle = _configuration.GetValue<string>("PremierAccessOfferTitle");
                    paOffer.OfferText1 = _configuration.GetValue<string>("PremierAccessOfferText1");
                    paOffer.OfferText2 = _configuration.GetValue<string>("PremierAccessOfferText2");
                    paOffer.OfferText3 = _configuration.GetValue<string>("PremierAccessOfferText3");
                }
            }
            catch (System.Exception) { }


            return paOffer;
        }

        private decimal BuildPaInfoForSelectAsBundleAndGetLowestPrice(MOBPremierAccess premierAccess)
        {
            decimal lowestPrice = 0;
            decimal totalPriceToBuyAvailableEveryOneInPNR = 0;
            var isAllSegmentsAllPaxEligibleToPurchasePa = IsAllSegmentsAllPaxEligibleToPurchasePa(premierAccess.Segments);
            lowestPrice = BuildLowestPriceAndPriceForAll(premierAccess, ref totalPriceToBuyAvailableEveryOneInPNR);
            if (totalPriceToBuyAvailableEveryOneInPNR > 0 && isAllSegmentsAllPaxEligibleToPurchasePa)
            {
                //1750
                premierAccess.PAForCurrentTrip = lowestPrice;
                premierAccess.IndividualFlights = _configuration.GetValue<string>("PremierAccessIndividualFlightsText");
            }
            else
            {
                premierAccess.PAForCurrentTrip = 0;
                premierAccess.IndividualFlights = "";
            }
            return lowestPrice;
        }

        private decimal BuildLowestPriceAndPriceForAll(MOBPremierAccess premierAccess, ref decimal totalPriceToBuyAvailableEveryOneInPNR)
        {
            decimal lowestPrice = 0;
            foreach (MOBPASegment segment in premierAccess.Segments.Where(segment => segment.PASegmentType == MOBPASegmentType.Regular))
            {
                foreach (MOBPACustomer customer in segment.Customers.Where(customer => customer.paCustomerType == MOBPACustomerType.Regular))
                {
                    if (customer.Fee < lowestPrice || lowestPrice == 0)
                    {
                        lowestPrice = customer.Fee;
                    }
                    totalPriceToBuyAvailableEveryOneInPNR = totalPriceToBuyAvailableEveryOneInPNR + customer.Fee;
                }
            }
            return lowestPrice;
        }

        private bool IsAllSegmentsAllPaxEligibleToPurchasePa(List<MOBPASegment> segments)
        {
            bool isAllSegmentsAllPaxEligibleToPurchasePa =
                !segments.Any(
                    s =>
                        s.PASegmentType != MOBPASegmentType.Regular ||
                        s.Customers.Any(c => c.paCustomerType != MOBPACustomerType.Regular));
            return isAllSegmentsAllPaxEligibleToPurchasePa;
        }

        private void BuildSegmentsWithPriceInfo_CFOP(ProductDetail productDetail, MOBPremierAccess premierAccess)
        {
            if (productDetail != null)
            {
                BuildEligibleSegmentsInfo_CFOP(productDetail, premierAccess);
                BuildIneligibleSegmentsInfo_CFOP(productDetail, premierAccess);
            }
        }

        private void BuildIneligibleSegmentsInfo_CFOP(ProductDetail productDetail, MOBPremierAccess premierAccess)
        {
            if (productDetail.InEligibleSegments != null && productDetail.InEligibleSegments.Any())
            {
                foreach (var ineligibleSegment in productDetail.InEligibleSegments
                    .Where(ineligibleSegment => ineligibleSegment.Reason != null && !string.IsNullOrEmpty(ineligibleSegment.Reason.MajorCode)
                    && !string.IsNullOrEmpty(ineligibleSegment.Reason.MinorCode) && ineligibleSegment.Assocatiation != null
                    && ineligibleSegment.Assocatiation.SegmentRefIDs != null && ineligibleSegment.Assocatiation.SegmentRefIDs.Any()))
                {
                    foreach (var segRefID in ineligibleSegment.Assocatiation.SegmentRefIDs.Where(s => !string.IsNullOrEmpty(s)))
                    {
                        foreach (var segment in premierAccess.Segments.Where(segment => segment != null && !string.IsNullOrEmpty(segment.SegmentId) && segRefID == segment.SegmentId))
                        {
                            BuildSegmentTypeAndCustomerType_CFOP(ineligibleSegment, segment);
                        }
                    }
                }
            }
        }

        private void BuildSegmentTypeAndCustomerType_CFOP(InEligibleSegment ineligibleSegment, MOBPASegment segment)
        {
            switch (string.Format("{0},{1}", ineligibleSegment.Reason.MajorCode.Trim(), ineligibleSegment.Reason.MinorCode.Trim()))
            {
                //no iventory
                case "002,21":
                    segment.PASegmentType = MOBPASegmentType.SoldOut;
                    break;
                //"002,3" PA Already purchased)
                //"010/19" PAS is Partially/fully purchased
                case "002,3":
                case "010,19":
                    foreach (MOBPACustomer customer in segment.Customers)
                    {
                        if (ineligibleSegment.Assocatiation.TravelerRefIDs.Contains(customer.CustId.Trim()))
                        {
                            customer.alreadypurchased = true;
                            customer.paCustomerType = MOBPACustomerType.AlreadyPurchased;
                        }
                    }
                    break;
                case "002,7":
                case "002,4":
                    //"PA Already Entitled"
                    //PA  offered only for Economy
                    segment.PASegmentType = MOBPASegmentType.Regular;
                    foreach (var customer in segment.Customers)
                    {
                        customer.alreadypurchased = true;
                        customer.paCustomerType = MOBPACustomerType.AlreadyPremier;
                    }
                    break;
                default:
                    segment.PASegmentType = MOBPASegmentType.NotOffered;
                    break;
            }
        }

        private void BuildEligibleSegmentsInfo_CFOP(ProductDetail productDetail, MOBPremierAccess premierAccess)
        {
            foreach (var subProduct in productDetail.Product.SubProducts
                .Where(sp => (sp.GroupCode != null && sp.Code != null) && sp.GroupCode.ToUpper().Trim() == "PA" && (sp.Code.ToUpper().Trim() == "CSB" || sp.Code.ToUpper().Trim() == "COB")))
            {
                BuildEligibleSegmentPriceInfo(premierAccess, subProduct);
                BuildPABenifits(premierAccess, subProduct);
            }
        }

        private void BuildPABenifits(MOBPremierAccess premierAccess, SubProduct subProduct)
        {
            bool showAvailability = false;
            if (premierAccess != null && premierAccess.Segments != null && premierAccess.Segments.Count > 0 &&
                subProduct.Features != null)
            {
                List<MOBTypeOption> priorityOptions = new List<MOBTypeOption>();
                string benefitsList = _configuration.GetValue<string>("PremierAccessBenefitsList");
                string[] benefitsTextLlist = benefitsList.Split('|');
                priorityOptions.Add(new MOBTypeOption("paBInclude", benefitsTextLlist[0].ToString()));
                priorityOptions.Add(new MOBTypeOption("priorityBoarding", benefitsTextLlist[1].ToString()));
                priorityOptions.Add(new MOBTypeOption("priorityCheckin", benefitsTextLlist[2].ToString()));
                priorityOptions.Add(new MOBTypeOption("prioritySecurity", benefitsTextLlist[3].ToString()));

                if (subProduct.Features.Count() < 3)
                {
                    showAvailability = true;
                }
                if (showAvailability)
                {
                    priorityOptions.Add(new MOBTypeOption("seeAvailability", benefitsTextLlist[4].ToString()));
                }
                premierAccess.Benefits = priorityOptions;
            }
        }

        private void BuildEligibleSegmentPriceInfo(MOBPremierAccess premierAccess, SubProduct subProduct)
        {
            if (subProduct.Prices.IsNotNullNorEmpty() && subProduct.Prices.Any() &&
                premierAccess != null && premierAccess.Segments != null && premierAccess.Segments.Any())
            {
                foreach (var price in subProduct.Prices.Where(p => p != null && p.Association != null &&
                p.Association.SegmentRefIDs.IsNotNullNorEmpty() && p.Association.SegmentRefIDs.Any()))
                {
                    foreach (MOBPASegment segment in premierAccess.Segments
                        .Where(s => s != null && !string.IsNullOrEmpty(s.SegmentId) && price.Association.SegmentRefIDs[0] == s.SegmentId))
                    {
                        decimal mAmount;
                        string mEddCode, mCurrencyCode;
                        this.GetPricingCSL(price.PaymentOptions[0], out mAmount, out mCurrencyCode, out mEddCode);
                        foreach (MOBPACustomer customer in segment.Customers)
                        {
                            customer.Fee = mAmount;
                            customer.CurrencyCode = mCurrencyCode;
                        }
                        segment.PASegmentType = MOBPASegmentType.Regular;
                        GetPAFeatureValue(subProduct, segment);
                    }
                }
            }
        }

        private void GetPAFeatureValue(SubProduct subProduct, MOBPASegment segment)
        {
            bool isPriorityBoarding = false,
                isPriorityCheckin = false,
                isPrioritySecurity = false;
            foreach (var feature in subProduct.Features)
            {
                if (feature.Type == Service.Presentation.CommonEnumModel.ProductFeatureType.PriorityBoarding)
                {
                    isPriorityBoarding = true;
                }
                if (feature.Type == Service.Presentation.CommonEnumModel.ProductFeatureType.PriorityCheckIn)
                {
                    isPriorityCheckin = true;
                }
                if (feature.Type == Service.Presentation.CommonEnumModel.ProductFeatureType.PrioritySecurity)
                {
                    isPrioritySecurity = true;
                }
            }
            segment.PriorityBoarding = isPriorityBoarding;
            segment.PriorityCheckin = isPriorityCheckin;
            segment.PrioritySecurity = isPrioritySecurity;
        }

        private void GetPricingCSL(United.Service.Presentation.ProductModel.ProductPaymentOption oPaymentOption, out decimal Amount, out string CurrencyCode, out string EDDCode)
        {
            EDDCode = string.Empty;
            CurrencyCode = string.Empty;
            Amount = 0;

            if (oPaymentOption.PriceComponents.IsNotNullNorEmpty() && oPaymentOption.PriceComponents.Any() && oPaymentOption.PriceComponents[0] != null
                && oPaymentOption.PriceComponents[0].Price != null && oPaymentOption.PriceComponents[0].Price.Totals.IsNotNullNorEmpty()
                && oPaymentOption.PriceComponents[0].Price.Totals.Any() && oPaymentOption.PriceComponents[0].Price.Totals[0] != null
                && oPaymentOption.PriceComponents[0].Price.Totals[0].Currency != null && !string.IsNullOrEmpty(oPaymentOption.PriceComponents[0].Price.Totals[0].Currency.Code))
            {
                Amount = Convert.ToDecimal(oPaymentOption.PriceComponents[0].Price.Totals[0].Amount);
                CurrencyCode = oPaymentOption.PriceComponents[0].Price.Totals[0].Currency.Code;
            }
        }

        private IEnumerable<ProductFlightSegment> GetAllFlightsList(Service.Presentation.ProductResponseModel.ProductOffer offersResponse)
        {
            return from oDOption in offersResponse.Solutions[0].ODOptions.Where(
                o => o.FlightSegments.IsNotNullNorEmpty() &&
                     o.FlightSegments.Any())
                   select oDOption.FlightSegments.ToList()
                into flightSegmentsList
                   from flightSegment in flightSegmentsList
                   where !string.IsNullOrEmpty(flightSegment.IsActive) && flightSegment.IsActive == "Y" && !string.IsNullOrEmpty(flightSegment.OperatingAirlineCode) && flightSegment.OperatingAirlineCode.ToUpper().Trim() == "UA"
                   select flightSegment;
        }

        private void GetPASegmentsBasicInfo_CFOP(Service.Presentation.ProductResponseModel.ProductOffer offersResponse, string selectedCustomerInSegments,
            bool isSavePACall, MOBPremierAccess premierAccess)
        {
            #region
            if (offersResponse != null
                && offersResponse.Solutions.IsNotNullNorEmpty() && offersResponse.Solutions.Any()
                && offersResponse.Solutions[0].ODOptions.IsNotNullNorEmpty() && offersResponse.Solutions[0].ODOptions.Any())
            {
                premierAccess.Segments = new List<MOBPASegment>();
                foreach (ProductFlightSegment flightSegment in GetAllFlightsList(offersResponse))
                {
                    if (!isSavePACall)
                    {
                        AddSegmentWithBasicInfo_CFOP(offersResponse, premierAccess, flightSegment);
                    }
                    else
                    {
                        //Ex:selectedSegmentWithCustomers == [1-1,2],[2-1,2]
                        string[] selectedSegmentWithCustomers = selectedCustomerInSegments.Split(';');
                        foreach (string segmentWithCustomers in BuildSegmentWithCustomersUsingRequesList(selectedSegmentWithCustomers, flightSegment))
                        {
                            AddSelectedSegmentWithBasicinfo_CFOP(offersResponse, premierAccess, flightSegment, segmentWithCustomers);
                        }
                    }
                }
            }
            #endregion
        }

        private void AddSelectedSegmentWithBasicinfo_CFOP(Service.Presentation.ProductResponseModel.ProductOffer offersResponse, MOBPremierAccess premierAccess,
            ProductFlightSegment flightSegment, string segmentWithCustomers)
        {
            MOBPASegment pAsegment = new MOBPASegment();
            GetPASegmentbasicInfo(pAsegment, flightSegment);
            pAsegment.Customers = new List<MOBPACustomer>();
            foreach (var traveler in offersResponse.Travelers)
            {
                foreach (MOBPACustomer pAcustomer in
                    from custmr in segmentWithCustomers.Split('-')[1].ToString().Split(',')
                    where traveler.CustomerID.Trim() == custmr.Trim()
                    select new MOBPACustomer())
                {
                    pAcustomer.CustId = traveler.CustomerID;
                    pAcustomer.CustName = traveler.GivenName + " " + traveler.Surname;
                    pAcustomer.ProductIds = (offersResponse.Offers != null && offersResponse.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Where(x => x.Prices != null).FirstOrDefault() != null) ? offersResponse.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Where(x => x.Prices != null).SelectMany(x => x.Prices).Where(y => y.Association.SegmentRefIDs.FirstOrDefault().ToString() == pAsegment.SegmentId && y.Association.TravelerRefIDs.FirstOrDefault().ToString() == traveler.ID).Select(u => new List<string> { u.ID }).FirstOrDefault() : null;
                    pAsegment.Customers.Add(pAcustomer);
                }
            }
            premierAccess.Segments.Add(pAsegment);
        }

        private void GetPASegmentbasicInfo(MOBPASegment pAsegment, ProductFlightSegment flightSegment)
        {
            pAsegment.Origin = flightSegment.DepartureAirport.IATACode;
            pAsegment.Destination = flightSegment.ArrivalAirport.IATACode;
            pAsegment.FlightDate =
                Convert.ToDateTime(flightSegment.DepartureDateTime).ToString("ddd. MMM. dd, yyyy");
            //Wed. Jun. 09, 2010
            pAsegment.SegmentId = flightSegment.ID;
            pAsegment.FlightNumber = flightSegment.FlightNumber;
        }

        private IEnumerable<string> BuildSegmentWithCustomersUsingRequesList(string[] selectedSegmentWithCustomers, ProductFlightSegment flightSegment)
        {
            return from segmentWithCustomers
                   in selectedSegmentWithCustomers
                   let segmentID = segmentWithCustomers.Split('-')[0].ToString()
                   where segmentID.Trim() == flightSegment.ID
                   select segmentWithCustomers;
        }

        private void AddSegmentWithBasicInfo_CFOP(Service.Presentation.ProductResponseModel.ProductOffer offersResponse, MOBPremierAccess premierAccess,
            ProductFlightSegment flightSegment)
        {
            MOBPASegment pAsegment = new MOBPASegment();
            GetPASegmentbasicInfo(pAsegment, flightSegment);

            pAsegment.Customers = new List<MOBPACustomer>();
            foreach (var traveler in offersResponse.Travelers)
            {
                MOBPACustomer pAcustomer = new MOBPACustomer();
                pAcustomer.CustId = traveler.CustomerID.ToString();
                pAcustomer.CustName = traveler.GivenName + " " + traveler.Surname;
                pAcustomer.paCustomerType = MOBPACustomerType.Regular;
                pAcustomer.ProductIds = (offersResponse.Offers != null && offersResponse.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Where(x => x.Prices != null).FirstOrDefault() != null) ? offersResponse.Offers[0].ProductInformation.ProductDetails[0].Product.SubProducts.Where(x => x.Prices != null).SelectMany(x => x.Prices).Where(y => y.Association.SegmentRefIDs.FirstOrDefault().ToString() == pAsegment.SegmentId && y.Association.TravelerRefIDs.FirstOrDefault().ToString() == traveler.ID).Select(u => new List<string> { u.ID }).FirstOrDefault() : null;
                pAsegment.Customers.Add(pAcustomer);
            }
            premierAccess.Segments.Add(pAsegment);
        }


        private async System.Threading.Tasks.Task SavePBInFile(string sessionId, string recordLocator, string lastName, string pnrCreateDate, MOBPriorityBoarding priorityBoarding, string jsonResponse, int custCount)
        {
            var pBFile = new Mobile.Model.MPRewards.PriorityBoardingFile();
            pBFile.NumberOfTraveler = custCount;
            pBFile.PriorityBoarding = priorityBoarding;
            pBFile.OfferResponse = jsonResponse;
            pBFile.RecordLocator = recordLocator;
            pBFile.LastName = lastName;
            pBFile.PNRCreationDate = pnrCreateDate;
            await _sessionHelperService.SaveSession<Mobile.Model.MPRewards.PriorityBoardingFile>(pBFile, sessionId, new List<string> { sessionId, pBFile.ObjectName }, pBFile.ObjectName).ConfigureAwait(false);
        }

        private MOBPriorityBoarding GetPBDetail(Service.Presentation.ProductResponseModel.ProductOffer offerResponse, ref int custCount)
        {
            MOBPriorityBoarding priorityBoarding;
            if (_configuration.GetValue<bool>("PriorityBoardingOfferBasedOnSegment"))
            {
                priorityBoarding = BuildPBDetailBaseOnSegment(offerResponse, ref custCount, false);
            }
            else
            {
                priorityBoarding = BuildPBDetail(offerResponse);
            }

            return priorityBoarding;
        }

        private bool EnablePBS(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("EnablePBS")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPBSConfirmationVersion", "iPhonePBSConfirmationVersion", "", "", true, _configuration);
        }

        public void PBAndPAAssignment(string transactionId, ref MOBAncillary ancillary, MOBApplication application, string deviceId, MOBPriorityBoarding priorityBoarding, string logAction, ref MOBPremierAccess premierAccess, ref bool showPremierAccess)
        {
            if (priorityBoarding != null && priorityBoarding.Segments != null && priorityBoarding.Segments.Any())
            {
                if (ancillary == null)
                {
                    ancillary = new MOBAncillary();
                }
                ancillary.PriorityBoarding = priorityBoarding;
                premierAccess = null;
                showPremierAccess = false;

                _logger.LogInformation("PBAndPAAssignment{response}", JsonConvert.SerializeObject(ancillary.PriorityBoarding));

            }
            else if (premierAccess != null)
            {
                if (premierAccess.ErrorMessage.Trim() == "" && premierAccess.PAOfferTileInfo != null)
                {
                    showPremierAccess = true;
                }

                _logger.LogInformation("PBAndPAAssignment{response}", premierAccess);
            }
            else
            {
                if (ancillary != null)
                {
                    ancillary.PriorityBoarding = null;
                }
                premierAccess = null;
                showPremierAccess = false;
            }
        }

        public async Task<(MOBBundlesMerchandizingResponse response, MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess)> GetBundleInfoWithPNR(MOBBundlesMerchangdizingRequest request, string channelId, string channelName)
        {
            MOBBundlesMerchandizingResponse response = new MOBBundlesMerchandizingResponse();
            MOBPriorityBoarding priorityBoarding = new MOBPriorityBoarding();
            MOBPremierAccess premierAccess = new MOBPremierAccess();
            try
            {
                #region
                #region
                MerchandizingServicesClient merchClient = null;
                if (_configuration.GetValue<string>("AssignTimeOutForMerchandizeBundleServiceCall") != null && Convert.ToBoolean(_configuration.GetValue<string>("AssignTimeOutForMerchandizeBundleServiceCall")))
                {
                    #region Assigne Time Out Value for Merchantize Engine Call
                    MerchandizingServicesClient merchClient1 = new MerchandizingServicesClient();
                    int timeOutSeconds = _configuration.GetValue<string>("TimeOutSecondsForMerchandizeBundle") != null ? Convert.ToInt32(_configuration.GetValue<string>("TimeOutSecondsForMerchandizeBundle").Trim()) : 7;
                    BasicHttpBinding binding = new BasicHttpBinding();
                    TimeSpan timeout = new TimeSpan(0, 0, timeOutSeconds);
                    binding.CloseTimeout = timeout;
                    binding.SendTimeout = timeout;
                    binding.ReceiveTimeout = timeout;
                    binding.OpenTimeout = timeout;
                    EndpointAddress endPoint = new EndpointAddress(merchClient1.Endpoint.Address.ToString());
                    merchClient = new MerchandizingServicesClient(binding, endPoint);
                    #endregion
                }
                else
                {
                    merchClient = new MerchandizingServicesClient();
                }
                var offerRequest = new GetPurchasedProductsInput();
                offerRequest.PurchasedProductsRequest = new PurchasedProductsRequest();
                RequestCriterionType requestCriterionType = new RequestCriterionType();
                RequestCriterionTypeBookingReferenceIds[] pnrs = new RequestCriterionTypeBookingReferenceIds[1];
                RequestCriterionTypeBookingReferenceIds pnr = new RequestCriterionTypeBookingReferenceIds();
                pnr.BookingReferenceId = request.RecordLocator;
                pnrs[0] = pnr;
                requestCriterionType.BookingReferenceIds = pnrs;
                #endregion
                #region
                RequestorType requestorType = new RequestorType();
                if (_configuration.GetValue<bool>("PassChannelID_101_ChannelName_OBE_For_ME_GetBundleInfoWithPNR"))
                {
                    requestorType.ChannelId = "101";
                    requestorType.ChannelName = "OBE";
                }
                else
                {
                    if (_configuration.GetValue<bool>("EnabledMERCHChannels"))
                    {
                        requestorType.ChannelId = channelId;
                        requestorType.ChannelName = channelName;
                    }
                    else
                    {
                        requestorType.ChannelId = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID").Trim();
                        requestorType.ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName").Trim();
                    }
                }
                #endregion

                offerRequest.PurchasedProductsRequest.RequestCriterion = requestCriterionType;
                offerRequest.PurchasedProductsRequest.Requestor = requestorType;
                offerRequest.PurchasedProductsRequest.TransactionIdentifier = "Mobile_Request_With_RecordLocator_" + request.RecordLocator;

                try
                {
                    //SoapCall not used
                    //PurchasedProductsResponse offers = merchClient.GetPurchasedProductsAsync(offerRequest).Result;
                    GetPurchasedProductsOutput PurchasedProductsResponseOffers = await _merchOffersService.GetPurchasedProducts(offerRequest).ConfigureAwait(false);
                    PurchasedProductsResponse offers = PurchasedProductsResponseOffers.PurchasedProductsResponse;

                    response.BundleInfo = _configuration.GetValue<bool>("EnableAwardAccelerators") ? MapBundleInfoFromMerchandizingResponse2(offers, request.Application.Id, request.Application.Version.Major) : MapBundleInfoFromMerchandizingResponse(offers);

                    // Code added as part of merch migration to CCE and implementation of bundles
                    // In old behaviour PAS and PBS purchase is built from merch, as moving to cce building response based on merch purchased products.
                    if(_shoppingUtility.IsEnableTravelOptionsInViewRes(request.Application.Id, request.Application.Version.Major))
                    {
                        List<MOBPBSegment> segments = new List<MOBPBSegment>();
                        List<MOBPASegment> paSegments = new List<MOBPASegment>();
                        bool isPriorityBoarding = false;
                        bool isPremierAccess = false;
                        if (offers != null && offers.Itinerary != null)
                        {
                            #region
                            foreach (ItineraryType itinerary in offers.Itinerary)
                            {
                                #region
                                if (itinerary.Items != null && itinerary.Items.Count() > 0)
                                {
                                    foreach (FlightSegmentType item in itinerary.Items)
                                    {
                                        var thisSegmentTiers = itinerary.MerchandisingInfo.Services.SelectMany(s => s.Tiers.Where(t => t != null && t.Association != null && t.Association.Items != null && t.Association.Items.Any(it => it != null && (it as AssociationTypeSegmentMapping).SegmentReference == item.SegmentNumber)));
                                        if (thisSegmentTiers != null && thisSegmentTiers.Any())
                                        {
                                            isPriorityBoarding = HasThisProduct(thisSegmentTiers, "PBS");
                                            if(isPriorityBoarding)
                                            {
                                                MOBPBSegment segment = GetPBSegment(item);
                                                segments.Add(segment);
                                            }
                                            isPremierAccess = HasThisProduct(thisSegmentTiers, "PAS");
                                            if(isPremierAccess)
                                            {
                                                premierAccess.ProductCode = "PAS";
                                                premierAccess.ProductName = itinerary.MerchandisingInfo.Services.FirstOrDefault(x => x != null && x.ServiceDefinition !=null && x.ServiceDefinition?.Code == "PAS")?.ServiceDefinition?.Description;
                                                MOBPASegment segment = GePAPurchasedSegment(itinerary, item);
                                                paSegments.Add(segment);
                                            }
                                        }
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }

                        if(isPriorityBoarding && segments.Count > 0)
                        {
                            priorityBoarding.Segments = segments;
                            priorityBoarding.PbDetailsConfirmationTxt = _configuration.GetValue<string>("PriorityBoardingConfirmationTxtBaseOnSegment");
                            priorityBoarding.PbAddedTravelerTxt = _configuration.GetValue<string>("PriorityBoardingAddedSegmentTxt");
                        }
                        if(isPremierAccess && paSegments.Count > 0)
                        {
                            premierAccess.Segments = paSegments;
                        }
                    }

                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
                #endregion
            }
            catch (System.Exception ex) { throw ex; }

            return (response, priorityBoarding, premierAccess);
        }

        private MOBPASegment GePAPurchasedSegment(ItineraryType itinerary, FlightSegmentType item)
        {
            MOBPASegment segment = new MOBPASegment();
            if (itinerary != null && item != null)
            {
                segment.Origin = item.DepartureAirport.LocationCode;
                segment.Destination = item.ArrivalAirport.LocationCode;
                segment.FlightDate = item.DepartureDateTime.ToShortDateString();
                segment.FlightNumber = item.FlightNumber;
                segment.PASegmentType = MOBPASegmentType.AlreadyPurchased;
                segment.SegmentId = item.SegmentNumber.Trim();

                segment.Customers = new List<MOBPACustomer>();
                var serviceTypes = itinerary.MerchandisingInfo.Services.Where(s => s.Tiers.Any(t => t != null && t.Association != null && t.Association.Items != null && t.Association.Items.Any(it => it != null && (it as AssociationTypeSegmentMapping).SegmentReference == item.SegmentNumber)));
                var travelerInfo = GetTravelersPurchases(segment.SegmentId, itinerary.TravelerInfo.Traveler, serviceTypes);
                foreach (var traveler in travelerInfo)
                {
                    MOBPACustomer mobPACustomer = new MOBPACustomer();
                    mobPACustomer.alreadypurchased = true;
                    mobPACustomer.CustId = traveler.Id;
                    mobPACustomer.CustName = traveler.GivenName + " " + traveler.Surname;
                    mobPACustomer.paCustomerType = MOBPACustomerType.AlreadyPurchased;
                    segment.Customers.Add(mobPACustomer);
                }
            }
            return segment;
        }

        private MOBPBSegment GetPBSegment(FlightSegmentType item)
        {
            MOBPBSegment segment = new MOBPBSegment();
            if (item != null)
            {
                segment.Origin = item.DepartureAirport.LocationCode;
                segment.Destination = item.ArrivalAirport.LocationCode;
                segment.PbSegmentType = MOBPBSegmentType.AlreadyPurchased;
                segment.SegmentId = item.SegmentNumber.Trim();
                segment.Message = _configuration.GetValue<string>("PriorityBoardingAlreadyPurchasedMessage");
            }
            return segment;
        }

        private MOBBundleInfo MapBundleInfoFromMerchandizingResponse(PurchasedProductsResponse offers)
        {
            MOBBundleInfo bundleInfo = new MOBBundleInfo();
            List<MOBBundleFlightSegment> bundlePerSegment = new List<MOBBundleFlightSegment>();
            //MOBBundleFlightSegment obj1;
            List<MOBBundleTraveler> travelers;
            bool isBundlesSwitchOn = Convert.ToBoolean(_configuration.GetValue<string>("BundlesMerchandizingServicesUpgradeSwitchON") ?? "false");

            if (offers != null && offers.Itinerary != null)
            {
                #region
                foreach (ItineraryType itinerary in offers.Itinerary)
                {

                    #region
                    if (itinerary.Items != null && itinerary.Items.Count() > 0)
                    {
                        foreach (FlightSegmentType item in itinerary.Items)
                        {
                            MOBBundleFlightSegment obj1 = new MOBBundleFlightSegment();
                            obj1.ArrivalAirport = item.ArrivalAirport.LocationCode;
                            obj1.DepartureAirport = item.DepartureAirport.LocationCode;
                            obj1.DepartureDate = item.DepartureDateTime.ToShortDateString();
                            obj1.FlightNumber = item.FlightNumber;
                            obj1.SegmentId = item.SegmentNumber.Trim();
                            //obj1.Travelers = travelers;
                            bundlePerSegment.Add(obj1);
                        }


                    }
                    #endregion
                    if (itinerary.MerchandisingInfo != null && itinerary.MerchandisingInfo.Services != null)
                    {
                        foreach (ServiceType item in itinerary.MerchandisingInfo.Services)
                        {
                            //foreach (AssociationTypeSegmentMapping serviceTypeItem in item.S.Association.Items)
                            //{
                            //foreach (MOBBundleFlightSegment seg in bundlePerSegment)
                            //{
                            //    if (item.Tiers != null && item.Tiers.Length > 0 && item.Tiers[0].Association != null && item.Tiers[0].Association.Items != null && item.Tiers[0].Association.Items.Length > 0 && item.Tiers[0].Association.Items[AssociationTypeSegmentMapping]. == seg.SegmentId)
                            //    {
                            //        seg.Bundle = item.ServiceDefinition.Code;
                            //    }
                            //}
                            //}

                            foreach (TierType tier in item.Tiers)
                            {
                                foreach (MOBBundleFlightSegment seg in bundlePerSegment)
                                {
                                    foreach (AssociationTypeSegmentMapping serviceTypeItem in tier.Association.Items)
                                    {
                                        if (serviceTypeItem.SegmentReference.Trim() == seg.SegmentId)
                                        {
                                            if (tier.TierInfo.SubGroupType.Trim() == "EPU")
                                                seg.IsEPU = true;
                                            else if (tier.TierInfo.SubGroupType.Trim() == "PAS")
                                                seg.IsPremierAccess = true;
                                            else if (tier.TierInfo.SubGroupType.Trim() == "BMI")
                                                seg.IsBonusMiles = true;
                                            else if (tier.TierInfo.SubGroupType.Trim() == "CTP")
                                                seg.IsClubTripPass = true;
                                            else if (tier.TierInfo.SubGroupType.Trim() == "EXB")
                                                seg.IsExtraBag = true;

                                            seg.Bundle = item.ServiceDefinition.Code;


                                        }
                                    }

                                }
                            }
                        }
                    }
                    if (bundlePerSegment.Count > 0)
                    {
                        for (int i = 0; i < bundlePerSegment.Count; i++)
                        {
                            if (itinerary.TravelerInfo != null)
                            {
                                travelers = new List<MOBBundleTraveler>();
                                foreach (TravelerType passenger in itinerary.TravelerInfo.Traveler)
                                {
                                    MOBBundleTraveler traveler = new MOBBundleTraveler();
                                    traveler.GivenName = passenger.GivenName;
                                    traveler.Surname = passenger.Surname;
                                    traveler.Id = passenger.Id;

                                    GetBundleDescription(isBundlesSwitchOn, bundlePerSegment, i, traveler);


                                    travelers.Add(traveler);
                                }
                                bundlePerSegment[i].Travelers = travelers;
                            }


                        }
                    }
                }
                #endregion

            }

            bundleInfo.FlightSegments = bundlePerSegment;

            return bundleInfo;
        }

        private void GetBundleDescription(bool isBundlesSwitchOn, List<MOBBundleFlightSegment> bundlePerSegment, int i, MOBBundleTraveler traveler)
        {
            if (!isBundlesSwitchOn)
            {
                traveler.BundleDescription = (bundlePerSegment[i].Bundle.Trim() == "EPS") ? "EconomyPlus\u00ae Essentials" : "EconomyPlus\u00ae Enhanced";
            }
            else
            {
                traveler.BundleDescription = GetBundlesCommonDescription(bundlePerSegment[i].Bundle.Trim().ToUpper());
            }
        }

        private MOBBundleInfo MapBundleInfoFromMerchandizingResponse2(PurchasedProductsResponse offers, int appID, string appVersion)
        {
            MOBBundleInfo bundleInfo = new MOBBundleInfo();
            List<MOBBundleFlightSegment> bundlePerSegment = new List<MOBBundleFlightSegment>();

            if (offers != null && offers.Itinerary != null)
            {
                #region
                foreach (ItineraryType itinerary in offers.Itinerary)
                {
                    #region
                    if (itinerary.Items != null && itinerary.Items.Count() > 0)
                    {
                        foreach (FlightSegmentType item in itinerary.Items)
                        {
                            MOBBundleFlightSegment obj1 = new MOBBundleFlightSegment();
                            obj1.ArrivalAirport = item.ArrivalAirport.LocationCode;
                            obj1.DepartureAirport = item.DepartureAirport.LocationCode;
                            obj1.DepartureDate = item.DepartureDateTime.ToShortDateString();
                            obj1.FlightNumber = item.FlightNumber;
                            obj1.SegmentId = item.SegmentNumber.Trim();
                            var thisSegmentTiers = itinerary.MerchandisingInfo.Services.SelectMany(s => s.Tiers.Where(t => t != null && t.Association != null && t.Association.Items != null && t.Association.Items.Any(it => it != null && (it as AssociationTypeSegmentMapping).SegmentReference == item.SegmentNumber)));
                            if (thisSegmentTiers != null && thisSegmentTiers.Any())
                            {
                                obj1.IsEPU = HasThisProduct(thisSegmentTiers, "EPU");
                                obj1.IsPremierAccess = HasThisProduct(thisSegmentTiers, "PAS");
                                obj1.IsBonusMiles = HasThisProduct(thisSegmentTiers, "BMI");
                                obj1.IsClubTripPass = HasThisProduct(thisSegmentTiers, "CTP");
                                obj1.IsExtraBag = HasThisProduct(thisSegmentTiers, "EXB");
                                var serviceTypes = itinerary.MerchandisingInfo.Services.Where(s => s.Tiers.Any(t => t != null && t.Association != null && t.Association.Items != null && t.Association.Items.Any(it => it != null && (it as AssociationTypeSegmentMapping).SegmentReference == item.SegmentNumber)));
                                obj1.Travelers = GetTravelersPurchases(obj1.SegmentId, itinerary.TravelerInfo.Traveler, serviceTypes);
                                var serviceCode = serviceTypes.Where(s => s != null && s.ServiceDefinition != null && !string.IsNullOrEmpty(s.ServiceDefinition.Code) && !nonBundlesCodes.Contains(s.ServiceDefinition.Code)).FirstOrDefault();
                                obj1.Bundle = serviceCode != null && serviceCode.ServiceDefinition != null ? serviceCode.ServiceDefinition.Code : string.Empty;
                            }
                            bundlePerSegment.Add(obj1);
                        }
                    }
                    #endregion
                }
                #endregion
            }

            bundleInfo.FlightSegments = bundlePerSegment;

            return bundleInfo;
        }

        private List<MOBBundleTraveler> GetTravelersPurchases(string segmentId, TravelerType[] travelers, IEnumerable<ServiceType> serviceTypes)
        {
            var bundletravelers = new List<MOBBundleTraveler>();
            foreach (TravelerType traveler in travelers)
            {
                var bundletraveler = new MOBBundleTraveler
                {
                    GivenName = traveler.GivenName,
                    Surname = traveler.Surname,
                    Id = traveler.Id,
                    BundleDescription = BundleDescription(serviceTypes, traveler.Id)
                };
                bundletravelers.Add(bundletraveler);
            }
            return bundletravelers;
        }

        private string BundleDescription(IEnumerable<ServiceType> serviceTypes, string travelerId)
        {
            var thisTravelerServiceTypes = serviceTypes.Where(s => s != null && s.ServiceDefinition != null && !string.IsNullOrEmpty(s.ServiceDefinition.Code) && s.Tiers.Any(t => t != null && t.Association != null && t.Association.Items1 != null && t.Association.Items1.Any(it => it != null && (it as AssociationTypePassengerMapping).PassengerRefrence == travelerId)));
            if (thisTravelerServiceTypes == null || !thisTravelerServiceTypes.Any())
                return string.Empty;

            var codes = thisTravelerServiceTypes.Select(s => s.ServiceDefinition.Code);

            if (codes.All(c => c == "AAC"))
                return "Award Accelerator";

            if (codes.All(c => c == "AAC" || c == "PAC"))
                return "Award Accelerator and Premier Accelerator";

            return codes.Any(c => !nonBundlesCodes.Contains(c)) ? _configuration.GetValue<string>("BundlesCodeCommonDescription") : "";
        }

        private bool HasThisProduct(IEnumerable<TierType> thisSegmentTiers, string productCode)
        {
            if (string.IsNullOrEmpty(productCode))
                return false;

            productCode = productCode.ToUpper().Trim();
            return thisSegmentTiers.Any(t => t != null && t.TierInfo != null && !string.IsNullOrEmpty(t.TierInfo.SubGroupType) && t.TierInfo.SubGroupType.ToUpper().Trim() == productCode);
        }

        public async Task<MOBTPIInfo> GetTPIINfoDetails_CFOP(bool isTPIIncluded, bool isFareLockOrNRSA, MOBPNRByRecordLocatorRequest request, Session session)
        {
            if (!EnableTPI(request.Application.Id, request.Application.Version.Major, 2) || request.Flow == FlowType.VIEWRES_SEATMAP.ToString())
                return null;

            MOBTPIInfo tripInsuranceInfo = new MOBTPIInfo() { };
            if (isTPIIncluded || isFareLockOrNRSA)
            {
                tripInsuranceInfo = null;
            }
            else
            {
                try
                {
                    ProductOffer vendorProductOffers = new ProductOffer();
                    vendorProductOffers = await GetMerchVendorOffersDetails(session, request);

                    if (vendorProductOffers != null)
                    {
                        #region Trip Insurance
                        tripInsuranceInfo = await GetTPIDetails(vendorProductOffers, session.SessionId, false, false, request.Application.Id).ConfigureAwait(false);
                        #endregion
                    }
                }
                catch
                {
                    tripInsuranceInfo = null;
                }
            }
            return tripInsuranceInfo;
        }

        private async Task<ProductOffer> GetMerchVendorOffersDetails(Session session, MOBPNRByRecordLocatorRequest pNRByRecordLocatorRequest)
        {
            var appVersion = pNRByRecordLocatorRequest.Application.Version.Major;
            GetVendorOffers response = null;

            try
            {
                response = new GetVendorOffers();
                United.Service.Presentation.ReservationResponseModel.ReservationDetail reservationDetail = new United.Service.Presentation.ReservationResponseModel.ReservationDetail();
                reservationDetail = await _sessionHelperService.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(session.SessionId, new Service.Presentation.ReservationResponseModel.ReservationDetail().GetType().FullName, new List<string> { session.SessionId, new Service.Presentation.ReservationResponseModel.ReservationDetail().GetType().FullName }).ConfigureAwait(false);
                string token = session.Token;
                Service.Presentation.ProductRequestModel.ProductOffer request = BuildMerchVendorOffersRequest(reservationDetail, pNRByRecordLocatorRequest, token);
                if (request == null)
                {
                    return response = null;
                }
                string jsonRequest = JsonConvert.SerializeObject(request);

                if (await _featureSettings.GetFeatureSettingValue("EnableDefaultResilientCircuitBreaker"))
                {
                    response = await _purchaseMerchandizingService.GetVendorOfferInfo<GetVendorOffers>(token, jsonRequest, _headers.ContextValues.SessionId).ConfigureAwait(false);
                }
                else
                {
                    _logger.ILoggerInfo("GetVendorOffer by new resilient client");
                    response = await _vendorMerchandizingService.GetVendorOfferInfo<GetVendorOffers>(token, jsonRequest, _headers.ContextValues.SessionId).ConfigureAwait(false);
                }

                // for building purchase request in viewRes, we have to persist the get offer response 
                await _sessionHelperService.SaveSession<United.Service.Presentation.ProductRequestModel.ProductOffer>(request, session.SessionId, new List<string> { session.SessionId, new United.Service.Presentation.ProductRequestModel.ProductOffer().GetType().FullName }, new United.Service.Presentation.ProductRequestModel.ProductOffer().GetType().FullName).ConfigureAwait(false);

                if (response != null)
                {
                    if (response != null && response.Offers != null && response.Response.Error == null)
                    {
                        await _sessionHelperService.SaveSession<GetVendorOffers>(response, session.SessionId, new List<string> { session.SessionId, new GetVendorOffers().ObjectName }, new GetVendorOffers().ObjectName).ConfigureAwait(false);
                    }
                    else
                        response = null;
                }
                else
                    response = null;
            }
            catch (MOBUnitedException coex)
            {
                response = null;

                _logger.LogWarning("GetMerchVendorOffersDetails UnitedExecption{unitedexception}", JsonConvert.SerializeObject(coex));

            }
            catch (System.Exception ex)
            {
                response = null;
                _logger.LogError("GetMerchVendorOffersDetails Execption{exception}", JsonConvert.SerializeObject(ex));
            }

            return response;
        }

        public United.Service.Presentation.ProductRequestModel.ProductOffer BuildMerchVendorOffersRequest(United.Service.Presentation.ReservationResponseModel.ReservationDetail reservationDetail, MOBPNRByRecordLocatorRequest request, string token)
        {
            United.Service.Presentation.ProductRequestModel.ProductOffer offer = new Service.Presentation.ProductRequestModel.ProductOffer() { };
            if (reservationDetail != null && reservationDetail.Detail != null)
            {
                string ticketPriceRevenue = "0";
                string isAward = "N";
                bool isAwardTravel = false;
                string ticketPriceAward = "0";
                string currencyCode = string.Empty;
                string tripType = string.Empty;
                offer.FlightSegments = new Collection<ProductFlightSegment>();
                offer.Travelers = new Collection<Service.Presentation.ProductModel.ProductTraveler>();
                offer.SelectedProducts = new Collection<Service.Presentation.ProductModel.Product>();
                offer.SelectedProducts = SelectedProduct(reservationDetail.Detail.Travelers);

                #region awardTravel and trip type: "OW", "MC" or "RT"
                GetAwardTravelInfoAndTripTye(reservationDetail.Detail.Type, ref isAward, ref isAwardTravel, ref tripType);
                #endregion

                #region ticket price with tax including revenue and award 
                GetTKTPrice(reservationDetail.Detail.Characteristic, ref ticketPriceRevenue, ref ticketPriceAward, ref currencyCode);
                #endregion
                bool isInternational = offer.FlightSegments.Any(f => f.IsInternational.ToBoolean());
                offer.Characteristics = new System.Collections.ObjectModel.Collection<Characteristic>() {
                    // Ticket price including tax only, no ancillary
                    new Characteristic() { Code = "TKT_PRICE", Value = ticketPriceRevenue },
                    new Characteristic() { Code = "MILES_NEEDED", Value = isAward },
                    new Characteristic() { Code = "TripOfferVersionId", Value = "2"},
                    new Characteristic() { Code = "IsDomestic", Value = (!isInternational).ToString() },
                    new Characteristic() { Code = "RESERVED", Value = "False" },
                    new Characteristic() { Code = "TRIP_TYPE", Value = tripType },
                    new Characteristic() { Code = "TOTAL_MILES_COST", Value = ticketPriceAward },
                    new Characteristic() { Code = "New", Value = "true" },
                    new Characteristic() { Code = "AWARD", Value = isAwardTravel.ToString() },
                    new Characteristic() { Code = "NGRP", Value = "true" } };
                //currency code should be the same as ticketing currency code 
                offer.CurrencyCode = currencyCode;
                // point of sale 
                if (reservationDetail.Detail.PointOfSale != null &&
                    reservationDetail.Detail.PointOfSale.Country != null &&
                    reservationDetail.Detail.PointOfSale.Country.CountryCode != null &&
                    !string.IsNullOrEmpty(reservationDetail.Detail.PointOfSale.Country.CountryCode))
                {
                    offer.CountryCode = reservationDetail.Detail.PointOfSale.Country.CountryCode;
                }
                else if (Convert.ToBoolean(_configuration.GetValue<string>("DontCallMerchanIfPOSIsNotReturned") ?? "false"))
                {
                    return offer = null;
                }

                System.Collections.ObjectModel.Collection<United.Service.Presentation.ProductRequestModel.ProductFilter> productFilters
                    = new System.Collections.ObjectModel.Collection<Service.Presentation.ProductRequestModel.ProductFilter>();

                if (request.Flow == FlowType.VIEWRES.ToString())
                {
                    productFilters = Constants.ViewResFlow_VendorProductMapping.Split(',').Select(x =>
                            new ProductFilter() { IsIncluded = "true", ProductCode = x }
                            ).ToCollection();
                }


                offer.Filters = productFilters;
                offer.FlightSegments = ProductFlightSegments(reservationDetail.Detail.FlightSegments, reservationDetail.Detail.Prices);
                offer.IsAwardReservation = isAwardTravel.ToString();
                offer.ODOptions = ProductOriginDestinationOptions(reservationDetail.Detail.FlightSegments);
                offer.Requester = new ServiceClient()
                {
                    //GUIDs = new System.Collections.ObjectModel.Collection<UniqueIdentifier>() { new UniqueIdentifier() { Name = "TransactionId", ID = transactionId }, new UniqueIdentifier() { Name = "AuthorizationToken", ID = token } },
                    Requestor = new Requestor()
                    {
                        //ID = "",
                        ChannelID = _configuration.GetValue<string>("MerchandizeOffersServiceChannelID"), //1301
                        ChannelName = _configuration.GetValue<string>("MerchandizeOffersServiceChannelName"), //"MMR",
                        LanguageCode = "en"
                    }
                };
                if (!string.IsNullOrEmpty(reservationDetail.Detail.ConfirmationID) && !string.IsNullOrEmpty(reservationDetail.Detail.CreateDate))
                {
                    offer.ReservationReferences = new Collection<ReservationReference> { new ReservationReference {
                        ID = reservationDetail.Detail.ConfirmationID,
                        PaxFares = reservationDetail.Detail.Prices,
                        Remarks = (reservationDetail.Detail.Remarks != null) ? reservationDetail.Detail.Remarks.Select(x => new Remark { Description = x.Description, DisplaySequence = x.DisplaySequence }).ToCollection():null,
                        SpecialServiceRequests = reservationDetail.Detail.Services.Select(x => new United.Service.Presentation.CommonModel.Service { Description = x.Description }).ToCollection(),
                        ReservationType = (reservationDetail.Detail.Type.ToList().Exists(p => p.Description == "GROUP")) ? ReservationType.GroupBooking : ReservationType.None
                    } };
                }
                offer.Solutions = Solutions(reservationDetail.Detail.FlightSegments);
                #region ticketing country should be the same as point of sale 
                offer.TicketingCountryCode = offer.CountryCode;
                #endregion
                offer.Travelers = ProductTravelers(reservationDetail.Detail.Travelers);

                if (Convert.ToDouble(ticketPriceRevenue) <= 0)
                {
                    throw new MOBUnitedException("Ticket price should be greater than zero");
                }
            }
            return offer;
        }

        public Collection<ProductTraveler> ProductTravelers(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers)
        {
            var i = 0;
            return travelers.Select(t => new ProductTraveler
            {
                DateOfBirth = t.Person.DateOfBirth,
                GivenName = t.Person.GivenName,
                ID = (++i).ToString(),
                IsSelected = true.ToString(),
                LoyaltyProgramProfile = t.LoyaltyProgramProfile,
                PassengerTypeCode = t.Person.Type,
                ReservationIndex = t.Person.Key,
                Sex = t.Person.Sex,
                Surname = t.Person.Surname,
                TicketingDate = TicketingDate(t.Tickets),
                TicketNumber = TicketedNumber(t.Tickets),
                TravelerNameIndex = t.Person.Key
            }).ToCollection();
        }

        private void GetAwardTravelInfoAndTripTye(Collection<Service.Presentation.CommonModel.Genre> type, ref string isAward, ref bool isAwardTravel, ref string tripType)
        {
            if (type.Count > 0)
            {
                foreach (var item in type)
                {
                    if (item != null && item.Description != null)
                    {
                        switch (item.Description.ToUpper())
                        {
                            case "ITIN_TYPE":
                                if (item.Key.ToUpper() == "AWARD")
                                {
                                    isAward = "Y";
                                    isAwardTravel = true;
                                }
                                else
                                {
                                    isAward = "N";
                                    isAwardTravel = false;
                                }
                                break;
                            case "JOURNEY_TYPE":
                                if (item.Key.ToUpper() == "ONE_WAY")
                                {
                                    tripType = "OW";
                                }
                                else if (item.Key.ToUpper() == "MULTI_CITY")
                                {
                                    tripType = "MC";
                                }
                                else
                                {
                                    tripType = "RT";
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
        }

        private Collection<Product> SelectedProduct(Collection<United.Service.Presentation.ReservationModel.Traveler> travelers)
        {
            var selectedProducts = new Collection<Product>();
            foreach (var traveler in travelers)
            {
                if (traveler.Person.Charges == null || !traveler.Person.Charges.Any()) continue;
                foreach (var subProduct in traveler.Person.Charges)
                {
                    var product = new Product
                    {
                        Price = new Price
                        {
                            Totals = new Collection<Service.Presentation.CommonModel.Charge>
                            {
                                new Service.Presentation.CommonModel.Charge
                                {
                                    Currency = Currency(subProduct.Currency),
                                    Amount = subProduct.Amount
                                }
                            },
                            FareType = 0 // CSL ENum, stay 0 
                        },
                        Description = subProduct.Description,
                        Code = subProduct.Type
                    };

                    if (product.Price.Totals != null && product.Price.Totals.Count > 0 &&
                        product.Price.Totals[0].Amount > 0)
                    {
                        if (!string.IsNullOrEmpty(subProduct.Currency.Code))
                        {

                            selectedProducts.Add(product);
                        }
                        else
                        {
                            throw new MOBUnitedException("subproducts should have currency code");
                        }
                    }
                }
            }

            return selectedProducts;
        }

        private Currency Currency(Currency currency)
        {
            return new Currency
            {
                Code = currency != null && !string.IsNullOrEmpty(currency.Code)
                        ? currency.Code
                        : string.Empty,
            };
        }

        public async Task<MOBAccelerators> GetMileageAndStatusOptions(ProductOffer productOffers, MOBRequest request, string sessionId)
        {
            if (!_configuration.GetValue<bool>("EnableAwardAccelerators")) return null;

            MOBAccelerators accelerators = null;
            try
            {
                var offerResponse = BuildIndividualProductOffer(productOffers, "APA");
                var offerTile = (await (new MileageAndStatusOptions(offerResponse, sessionId, _productInfoHelper)).BuildOfferTile()).OfferTile;

                if (offerTile != null)
                {
                    accelerators = new MOBAccelerators { OfferTile = offerTile };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetMileageAndStatusOptions Exception {exception}", JsonConvert.SerializeObject(ex));
            }

            return accelerators;
        }

        private Service.Presentation.ProductResponseModel.ProductOffer BuildIndividualProductOffer(Service.Presentation.ProductResponseModel.ProductOffer productOffers, string productCode, bool isWaitListPNR = false)
        {
            if (productOffers == null)
                return productOffers;

            var offerResponse = new Service.Presentation.ProductResponseModel.ProductOffer();
            offerResponse.Offers = new Collection<Offer>();
            Offer offer = new Offer();
            offer.ProductInformation = new ProductInformation();
            offer.ProductInformation.ProductDetails = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, productCode)).ToCollection();
            offerResponse.Offers.Add(offer);
            offerResponse.Travelers = new Collection<ProductTraveler>();
            offerResponse.Travelers = productOffers.Travelers;
            offerResponse.Solutions = new Collection<Solution>();
            offerResponse.Solutions = productOffers.Solutions;
            offerResponse.Response = productOffers.Response;
            offerResponse.FlightSegments = new Collection<Service.Presentation.SegmentModel.ProductFlightSegment>();
            offerResponse.FlightSegments = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isWaitListPNR && !productOffers.FlightSegments.IsNullOrEmpty()?
                                           productOffers.FlightSegments.Where(p => p != null && !p.FlightSegmentType.IsNullOrEmpty() && p.FlightSegmentType.ToUpper().Trim().Contains("HK")).ToCollection() :
                                           productOffers.FlightSegments;
            return offerResponse;
        }

        private Service.Presentation.ProductResponseModel.ProductOffer BuildIndividualProductOffers(DynamicOfferDetailResponse productOffers, string productCode, bool isWaitListPNR = false)
        {
            if (productOffers == null || productOffers?.Offers == null || !productOffers.Offers.Any())
                return null;

            var offerResponse = new Service.Presentation.ProductResponseModel.ProductOffer();
            offerResponse.Offers = new Collection<Offer>();
            Offer offer = new Offer();
            offer.ProductInformation = new ProductInformation();
            offer.ProductInformation.ProductDetails = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, productCode)).ToCollection();
            offerResponse.Offers.Add(offer);
            offerResponse.Travelers = new Collection<ProductTraveler>();
            offerResponse.Travelers = productOffers.Travelers;
            offerResponse.Solutions = new Collection<Solution>();
            offerResponse.Solutions = productOffers.Solutions;
            offerResponse.Response = productOffers.Response;
            offerResponse.FlightSegments = new Collection<Service.Presentation.SegmentModel.ProductFlightSegment>();
            offerResponse.FlightSegments = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isWaitListPNR ?
                                           productOffers.FlightSegments.Where(p => p != null && !p.FlightSegmentType.IsNullOrEmpty() && p.FlightSegmentType.ToUpper().Trim().Contains("HK")).ToCollection() :
                                           productOffers.FlightSegments;
            return offerResponse;
        }

        private DynamicOfferDetailResponse BuildIndividualProductOffer(DynamicOfferDetailResponse productOffers, string productCode, bool isWaitListPNR = false)
        {
            if (productOffers == null || productOffers.Offers != null || !productOffers.Offers.Any())
                return productOffers;

            var offerResponse = new DynamicOfferDetailResponse();
            offerResponse.Offers = new Collection<Offer>();
            Offer offer = new Offer();
            offer.ProductInformation = new ProductInformation();
            offer.ProductInformation.ProductDetails = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, productCode)).ToCollection();
            offerResponse.Offers.Add(offer);
            offerResponse.Travelers = new Collection<ProductTraveler>();
            offerResponse.Travelers = productOffers.Travelers;
            offerResponse.Solutions = new Collection<Solution>();
            offerResponse.Solutions = productOffers.Solutions;
            offerResponse.Response = productOffers.Response;
            offerResponse.FlightSegments = new Collection<Service.Presentation.SegmentModel.ProductFlightSegment>();
            offerResponse.FlightSegments = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isWaitListPNR ?
                                           productOffers.FlightSegments.Where(p => p != null && !p.FlightSegmentType.IsNullOrEmpty() && p.FlightSegmentType.ToUpper().Trim().Contains("HK")).ToCollection() :
                                           productOffers.FlightSegments;
            return offerResponse;
        }

        private bool IsProductMatching(ProductDetail productDetail, string productCode)
        {
            if (productDetail == null || productDetail.Product == null || string.IsNullOrWhiteSpace(productDetail.Product.Code))
                return false;
            productCode = productCode ?? string.Empty;
            if (productCode.ToUpper().Trim().Equals("APA"))
            {
                return productDetail.Product.Code.ToUpper().Equals("AAC") || productDetail.Product.Code.ToUpper().Equals("PAC");
            }

            if (productCode.ToUpper().Trim().Equals("SBE"))
            {
                return productDetail.Product?.SubProducts?.Any(sp => sp.GroupCode?.ToUpper()?.Equals("BE") ?? false) ?? false;
            }

            return productDetail.Product.Code.ToUpper().Equals(productCode.ToUpper().Trim());
        }

        public async Task<MOBPremiumCabinUpgrade> GetPremiumCabinUpgrade_CFOP(Service.Presentation.ProductResponseModel.ProductOffer productOffers, MOBPNRByRecordLocatorRequest request, string sessionId, Reservation cslReservation, DynamicOfferDetailResponse dodResponse = null, TravelOptionsRequest travelOptionsRequest = null)
        {
            var pcuLogEntries = new List<LogEntry>();
            //var isBookingPath = !string.IsNullOrEmpty(cartId);
            //var logAction = isBookingPath ? "PCUPostBookingOffer" : "PremiumCabinUpgrade Offer";
            bool isEnableTravelOptionsManageRes = travelOptionsRequest != null && travelOptionsRequest.Application != null && _shoppingUtility.IsEnableTravelOptionsInViewRes(travelOptionsRequest.Application.Id, travelOptionsRequest?.Application?.Version?.Major);
            try
            {
                bool isEnablePCU = isEnableTravelOptionsManageRes ? EnablePCU(travelOptionsRequest.Application.Id, travelOptionsRequest.Application?.Version?.Major, false) : EnablePCU(request.Application.Id, request.Application.Version.Major, false);

                if (!isEnablePCU)
                    return null;
                var premiumCabinUpgrade = new PremiumCabinUpgrade(_sessionHelperService, _configuration, _productInfoHelper, _dynamoDBService, _dPService, _pKDispenserService, _cachingService, _legalDocumentsForTitlesService, _headers);

                if (isEnableTravelOptionsManageRes)
                    await premiumCabinUpgrade.Initialization(travelOptionsRequest.RecordLocator, sessionId, null, travelOptionsRequest, cslReservation);
                else
                    await premiumCabinUpgrade.Initialization(request.RecordLocator, sessionId, null, request, cslReservation);
                bool isWaitListPNR = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && IsWaitListPNRFromCharacteristics(cslReservation.FlightSegments);
                premiumCabinUpgrade.MerchProductOffer = isEnableTravelOptionsManageRes ? BuildIndividualProductOffers(dodResponse, travelOptionsRequest.ProductCode, isWaitListPNR) : BuildIndividualProductOffer(productOffers, "PCU", isWaitListPNR);
                premiumCabinUpgrade.CartId = Guid.NewGuid().ToString();

                await (await (await (await premiumCabinUpgrade.ValidateOfferResponse()
                                   .BuildPremiumCabinUpgrade())
                                   .GetTokenFromSession())
                                   .GetpkDispenserPublicKey(request.Flow))
                                   .SaveState();
                return premiumCabinUpgrade.CabinUpgradeOffer;
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetPremiumCabinUpgradeNew UnitedException {unitedexception}", JsonConvert.SerializeObject(uaex));
                if (isEnableTravelOptionsManageRes)
                {
                    throw new MOBUnitedException("10000", _configuration.GetValue<string>("GenericExceptionMessage"));
                }
            }
            catch (Exception ex)
            {

                _logger.LogError("GetPremiumCabinUpgradeNew Exception {exception}", JsonConvert.SerializeObject(ex));
                if (isEnableTravelOptionsManageRes)
                {
                    throw ex;
                }
            }
            return null;
        }

        public string GetBundlesCommonDescription(string bundleCode)
        {
            string bundlesCodeDescription = _configuration.GetValue<string>("BundlesCodeCommonDescription") ?? string.Empty;
            string bundleTypeDesc = bundleCode != null ? bundlesCodeDescription : string.Empty;    //BusinessRule : BundleCode from MerchServices in 4.5.0 is EPS/EPA; 4.5.3 has B01-B15; If not null display common Desc
            return bundleTypeDesc;
        }

        public Collection<ODOption> ProductOriginDestinationOptionsForMircoSite(Collection<ReservationFlightSegment> flightSegments)
        {
            var segmentsWithOd = flightSegments.GroupBy(f => f.TripNumber);
            return segmentsWithOd
                    .Select(od => new ODOption()
                    {
                        ID = "OD" + od.Key,
                        FlightSegments = od.Select(q => new ProductFlightSegment { ID = q.FlightSegment.SegmentNumber.ToString(), RefID = q.FlightSegment.SegmentNumber.ToString() }).ToCollection()
                    }).ToCollection();
        }
        public MOBSustainableAviationFuel GetSAFPriceDetails(Service.Presentation.ProductResponseModel.ProductOffer productOffers, MOBSustainableAviationFuel saf)
        {
            if(!string.IsNullOrEmpty(saf?.Description?.Text1) && !string.IsNullOrEmpty(saf?.Description?.Text2) && productOffers?.Offers!=null
                && productOffers.Offers[0].ProductInformation?.ProductDetails!=null)
            {
                var safSubProductDetail = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, "SFC"));
                saf.Code = safSubProductDetail.First().Product.Code;
                if (safSubProductDetail!=null && safSubProductDetail.First().Product.SubProducts.Any())
                {

                    string price = string.Empty;
                    foreach (var subProduct in safSubProductDetail.First().Product.SubProducts)
                    {
                        if(saf.PriceItems==null)
                        {
                           saf.PriceItems = new List<MOBPriceItem>();
                        }
                        price = GetAmount(subProduct.Prices[0]).ToString("c");
                        saf.PriceItems.Add(new MOBPriceItem
                        {
                            SubProductCode = subProduct.SubGroupCode,
                            ProductId = subProduct.Prices[0].ID,
                            Price = price,
                            IsDefault=(price == _configuration.GetValue<string>("SAF_PriceItem_DefaultPrice")) ?true:false
                        });
                    }
                    
                }
                return saf;
            }
            return null;
        }
        #region//utilities
        private bool IsPOMOffer(string productCode)
        {
            if (!_configuration.GetValue<bool>("EnableInflightMealsRefreshment")) return false;
            if (string.IsNullOrEmpty(productCode)) return false;
            return (productCode == _configuration.GetValue<string>("InflightMealProductCode"));
        }

        private bool EnablePCU(int appId, string appVersion, bool isBookingPath)
        {
            return _configuration.GetValue<bool>("EnablePCU") &&
                   isBookingPath ? GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPCUInBookingVersion", "iPhonePCUInBookingVersion", "", "", true, _configuration)
                                 : GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPCUVersion", "iPhonePCUVersion", "", "", true, _configuration);
        }

        private bool EnableTPI(int appId, string appVersion, int path)
        {
            // path 1 means confirmation flow, path 2 means view reservation flow, path 3 means booking flow 
            if (path == 1)
            {
                // ==>> Venkat and Elise chagne code to offer TPI postbooking when inline TPI is off for new clients 2.1.36 and above
                // App Version 2.1.36 && ShowTripInsuranceSwitch = true
                bool offerTPIAtPostBooking = _configuration.GetValue<bool>("ShowTripInsuranceSwitch") &&
                    GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTPIConfirmationVersion", "iPhoneTPIConfirmationVersion", "", "", true, _configuration);
                if (offerTPIAtPostBooking)
                {
                    offerTPIAtPostBooking = !GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTPIBookingVersion", "iPhoneTPIBookingVersion", "", "", true, _configuration);                    // When the Flag is true, we offer for old versions, when the flag is off, we offer for all versions. 
                    if (!offerTPIAtPostBooking && _configuration.GetValue<bool>("ShowTPIatPostBooking_ForAppVer_2.1.36_UpperVersions"))
                    {
                        //"ShowTripInsuranceBookingSwitch" == false
                        //ShowTPIatPostBooking_ForAppVer_2.1.36_LowerVersions = true
                        //
                        offerTPIAtPostBooking = true;
                    }
                }
                return offerTPIAtPostBooking;
            }
            else if (path == 2)
            {
                return _configuration.GetValue<bool>("ShowTripInsuranceViewResSwitch") &&
                    GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTPIViewResVersion", "iPhoneTPIViewResVersion", "", "", true,
                    _configuration);
            }
            else if (path == 3)
            {
                return _configuration.GetValue<bool>("ShowTripInsuranceBookingSwitch") &&
                    GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidTPIBookingVersion", "iPhoneTPIBookingVersion", "", "", true, _configuration);
            }
            else
            {
                return false;
            }
        }

        private async Task<MOBTPIInfo> GetTPIDetails(ProductOffer productOffer, string sessionId, bool isShoppingCall, bool isBookingPath = false, int appid = -1)
        {
            if (productOffer?.Offers == null || !(productOffer?.Offers?.Any() ?? false))
            {
                return null;
            }
            MOBTPIInfo tripInsuranceInfo = new MOBTPIInfo();
            var product = productOffer.Offers.FirstOrDefault(a => a.ProductInformation.ProductDetails.Where(b => b.Product != null && b.Product.Code.ToUpper().Trim() == "TPI").ToList().Count > 0).
                ProductInformation.ProductDetails.FirstOrDefault(c => c.Product != null && c.Product.Code.ToUpper().Trim() == "TPI").Product;
            #region // sample code If AIG Dont Offer TPI, the contents and Prices should be null. 
            if (product.SubProducts[0].Prices == null || product.Presentation.Contents == null)
            {
                tripInsuranceInfo = null;
                return tripInsuranceInfo;
            }
            #endregion
            #region mapping content
            tripInsuranceInfo = await GetTPIInfoFromContent(product.Presentation.Contents, tripInsuranceInfo, sessionId, isShoppingCall, isBookingPath, appid).ConfigureAwait(false);
            #endregion

            if (tripInsuranceInfo != null)
            {
                tripInsuranceInfo.ProductId = GetTPIQuoteId(product.Characteristics);
                // if quoteId is null, we should keep reponse null
                if (!string.IsNullOrEmpty(tripInsuranceInfo.ProductId))
                {
                    tripInsuranceInfo = GetTPIAmountAndFormattedAmount(tripInsuranceInfo, product.SubProducts);
                }
                tripInsuranceInfo.ProductCode = product.Code;
                tripInsuranceInfo.ProductName = product.DisplayName;
            }

            return tripInsuranceInfo;
        }

        private string GetTPIQuoteId(Collection<Service.Presentation.CommonModel.Characteristic> characteristics)
        {
            string productId = string.Empty;
            productId = characteristics.FirstOrDefault(a => !string.IsNullOrEmpty(a.Code) && a.Code.ToUpper().Trim() == "QUOTEPACKID").Value.Trim();
            return productId;
        }

        private MOBTPIInfo GetTPIAmountAndFormattedAmount(MOBTPIInfo tripInsuranceInfo, System.Collections.ObjectModel.Collection<Service.Presentation.ProductModel.SubProduct> subProducts)
        {
            string currencyCode = string.Empty;
            var prices = subProducts.Where(a => a.Prices != null && a.Prices.Count > 0).FirstOrDefault().Prices;
            foreach (var price in prices)
            {
                if (price != null && price.PaymentOptions != null && price.PaymentOptions.Count > 0)
                {
                    foreach (var option in price.PaymentOptions)
                    {
                        if (option != null && option.Type != null && option.Type.ToUpper().Contains("TOTALPRICE"))
                        {
                            foreach (var PriceComponent in option.PriceComponents)
                            {
                                foreach (var total in PriceComponent.Price.Totals)
                                {
                                    tripInsuranceInfo.Amount = total.Amount;
                                    currencyCode = total.Currency.Code.ToUpper().Trim();
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            // concate currency sign and round up amount //removed
            //tripInsuranceInfo.FormattedDisplayAmount = AttachCurrencySymbol(tripInsuranceInfo.Amount, currencyCode, true);
            // real amount concat with currency sign
            tripInsuranceInfo.DisplayAmount = AttachCurrencySymbol(tripInsuranceInfo.Amount, currencyCode, false);
            if (tripInsuranceInfo.Amount <= 0)
            {
                tripInsuranceInfo = null;
            }
            return tripInsuranceInfo;
        }

        private string AttachCurrencySymbol(double amount, string currencyCode, bool isRoundUp)
        {
            string formattedDisplayAmount = string.Empty;
            CultureInfo ci = TopHelper.GetCultureInfo(currencyCode);
            formattedDisplayAmount = TopHelper.FormatAmountForDisplay(string.Format("{0:#,0.00}", amount), ci, isRoundUp);
            return formattedDisplayAmount;
        }

        private async Task<MOBTPIInfo> GetTPIInfoFromContent(System.Collections.ObjectModel.Collection<United.Service.Presentation.ProductModel.PresentationContent> contents, MOBTPIInfo tripInsuranceInfo, string sessionId, bool isShoppingCall, bool isBookingPath = false, int appid = -1)
        {
            string tncPaymentText1 = string.Empty;
            string tncPaymentText2 = string.Empty;
            string tncPaymentText3 = string.Empty;
            string tncPaymentLinkMessage = string.Empty;
            string tncProductPageText1 = string.Empty;
            string tncProductPageText2 = string.Empty;
            string tncProductPageLinkMessage = string.Empty;
            string confirmationResponseDetailMessage1 = string.Empty;
            string confirmationResponseDetailMessage2 = string.Empty;
            string contentInBooking1 = string.Empty;
            string contentInBooking2 = string.Empty;
            string contentInBooking3 = string.Empty;
            string header1 = string.Empty;
            string header2 = string.Empty;
            string legalInfo = string.Empty;
            string legalInfoText = string.Empty;
            string bookingImg = string.Empty;
            string bookingTncContentMsg = string.Empty;
            string bookingTncLinkMsg = string.Empty;
            string bookingLegalInfoContentMsg = string.Empty;
            string mobTgiLimitationMessage = string.Empty;
            string mobTgiReadMessage = string.Empty;
            string mobTgiAndMessage = string.Empty;
            // Covid-19 Emergency WHO TPI content
            string mobTIMREmergencyMessage = string.Empty;
            string mobTIMREmergencyMessageUrltext = string.Empty;
            string mobTIMREmergencyMessagelinkUrl = string.Empty;
            string mobTIMBemergencyMessage = string.Empty;
            string mobTIMBemergencyMessageUrltext = string.Empty;
            string mobTIMBemergencyMessagelinkUrl = string.Empty;
            string mobTIMRWashingtonMessage = string.Empty;
            string mobOfferHeaderV2Message = string.Empty;
            string mobOfferTitleV2Message = string.Empty;

            foreach (var content in contents)
            {
                switch (content.Header.ToUpper().Trim())
                {
                    case "MOBOFFERHEADERMESSAGE":
                        tripInsuranceInfo.Title1 = content.Body.Trim();
                        break;
                    case "MOBOFFERTITLEMESSAGE":
                        tripInsuranceInfo.QuoteTitle = content.Body.Trim();
                        break;
                    case "MOBTRIPCOVEREDPRICEMESSAGE":
                        tripInsuranceInfo.CoverCost = content.Body.Trim();
                        break;
                    case "MOBOFFERFROMTMESSAGE":
                        tripInsuranceInfo.Title2 = content.Body.Trim();
                        break;
                    case "MOBOFFERIMAGE":
                        tripInsuranceInfo.Image = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBOFFERHEADERMESSAGE":
                        tripInsuranceInfo.TileTitle1 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBOFFERTITLEMESSAGE":
                        tripInsuranceInfo.TileQuoteTitle = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBOFFERFROMTMESSAGE":
                        tripInsuranceInfo.TileTitle2 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBOFFERCTAMESSAGE":
                        tripInsuranceInfo.TileLinkText = content.Body.Trim();
                        break;
                    case "MOBTGIVIEWHEADERMESSAGE":
                        tripInsuranceInfo.Headline1 = content.Body.Trim();
                        break;
                    case "MOBTIDETAILSTOTALCOVERCOSTMESSAGE":
                        tripInsuranceInfo.LineItemText = content.Body.Trim();
                        break;
                    case "MOBPAYMENTTANDCHEADER1MESSAGE": //By clicking on purchase I acknowledge that I have read and understand the
                        tncPaymentText1 = content.Body.Trim();
                        break;
                    case "MOBPAYMENTTANDCURLHEADERMESSAGE": //Certificate of Insurance
                        tncPaymentText2 = content.Body.Trim();
                        break;
                    case "MOBPAYMENTTANDCURLHEADER2MESSAGE": //, and agree to the terms and conditions of the insurance coverage provided.
                        tncPaymentText3 = content.Body.Trim();
                        break;
                    case "MOBPAYMENTTANDCBODYURLMESSAGE":
                        tncPaymentLinkMessage = content.Body.Trim();
                        break;
                    case "MOBTIDETAILSTANDCHEADER1MESSAGE": // Coverage is offered by Travel Guard Group, Inc. and limitations will apply;
                        tncProductPageText1 = content.Body.Trim();
                        break;
                    case "MOBTIDETAILSTANDCURLHEADERMESSAGE": // view details.
                        tncProductPageText2 = content.Body.Trim();
                        break;
                    case "MOBTIDETAILSTANDCURLMESSAGE":
                        tncProductPageLinkMessage = content.Body.Trim();
                        break;
                    case "MOBTGIVIEWBODY1MESSAGE": // Are you prepared?
                        tripInsuranceInfo.Body1 = content.Body.Trim();
                        break;
                    case "MOBTGIVIEWBODY2MESSAGE": // For millions of travelers every year...
                        tripInsuranceInfo.Body2 = content.Body.Trim();
                        break;
                    // used in payment confirmation page 
                    case "MOBTICONFIRMATIONBODY1MESSAGE":
                        confirmationResponseDetailMessage1 = content.Body.Trim();
                        break;
                    case "MOBTICONFIRMATIONBODY2MESSAGE":
                        confirmationResponseDetailMessage2 = content.Body.Trim();
                        break;
                    // used in booking path 
                    case "PREBOOKINGMOBOFFERIMAGE":
                        bookingImg = content.Body.Trim();
                        tripInsuranceInfo.TileImage = bookingImg;
                        break;
                    case "PREBOOKINGMOBTIDETAILSTANDCURLHEADERMESSAGE":
                        bookingTncContentMsg = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBTIDETAILSTANDCURLMESSAGE":
                        bookingTncLinkMsg = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBPAYMENTTANDCHEADER1MESSAGE":
                        bookingLegalInfoContentMsg = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGEHEADLINE1":
                        header1 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGEHEADLINE2":
                        header2 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGECONTENT1":
                        contentInBooking1 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGECONTENT2":
                        contentInBooking2 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGECONTENT3":
                        contentInBooking3 = content.Body.Trim();
                        break;
                    case "PREBOOKINGMOBBOOKINGPRODPAGEBOTTOMLINE":
                        legalInfo = content.Body.Trim();
                        break;
                    case "MOBTGILIMITATIONMESSAGE":
                        mobTgiLimitationMessage = content.Body.Trim();
                        break;
                    case "MOBTGIREADMESSAGE":
                        mobTgiReadMessage = content.Body.Trim();
                        break;
                    case "MOBTGIANDMESSAGE":
                        mobTgiAndMessage = content.Body.Trim();
                        break;
                    // Covid-19 Emergency WHO TPI content
                    case "MOBTIMREMERGENCYMESSAGETEXT":
                        mobTIMREmergencyMessage = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMREMERGENCYMESSAGELINKTEXT":
                        mobTIMREmergencyMessageUrltext = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMREMERGENCYMESSAGELINKURL":
                        mobTIMREmergencyMessagelinkUrl = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMBEMERGENCYMESSAGETEXT":
                        mobTIMBemergencyMessage = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMBEMERGENCYMESSAGELINKTEXT":
                        mobTIMBemergencyMessageUrltext = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMBEMERGENCYMESSAGELINKURL":
                        mobTIMBemergencyMessagelinkUrl = content != null ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBTIMRWASHINGTONMESSAGE":
                        mobTIMRWashingtonMessage = content != null && !string.IsNullOrEmpty(content.Body) ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBOFFERHEADERV2MESSAGE":
                        mobOfferHeaderV2Message = content != null && !string.IsNullOrEmpty(content.Body) ? content.Body.Trim() : string.Empty;
                        break;
                    case "MOBOFFERTITLEV2MESSAGE":
                        mobOfferTitleV2Message = content != null && !string.IsNullOrEmpty(content.Body) ? content.Body.Trim() : string.Empty;
                        break;
                    default:
                        break;
                }
            }

            //Covid-19 Emergency WHO TPI content
            if (_configuration.GetValue<bool>("ToggleCovidEmergencytextTPI") == true)
            {
                if (tripInsuranceInfo != null &&
                    !string.IsNullOrEmpty(mobTIMREmergencyMessage) && !string.IsNullOrEmpty(mobTIMREmergencyMessageUrltext)
                    && !string.IsNullOrEmpty(mobTIMREmergencyMessagelinkUrl))
                {
                    MOBItem tpiContentMessage = new MOBItem();
                    tpiContentMessage.Id = "COVID19EmergencyAlertManageRes";
                    tpiContentMessage.CurrentValue = mobTIMREmergencyMessage +
                        " <a href =\"" + mobTIMREmergencyMessagelinkUrl + "\" target=\"_blank\">" + mobTIMREmergencyMessageUrltext + "</a> ";
                    tripInsuranceInfo.tpiAIGReturnedMessageContentList = new List<MOBItem>();
                    tripInsuranceInfo.tpiAIGReturnedMessageContentList.Add(tpiContentMessage);
                }

                if (tripInsuranceInfo != null &&
                    !string.IsNullOrEmpty(mobTIMBemergencyMessage) && !string.IsNullOrEmpty(mobTIMBemergencyMessageUrltext)
                    && !string.IsNullOrEmpty(mobTIMBemergencyMessagelinkUrl))
                {
                    MOBItem tpiContentMessage = new MOBItem();
                    tpiContentMessage.Id = "COVID19EmergencyAlert";
                    tpiContentMessage.CurrentValue = mobTIMBemergencyMessage +
                        " <a href =\"" + mobTIMBemergencyMessagelinkUrl + "\" target=\"_blank\">" + mobTIMBemergencyMessageUrltext + "</a> ";
                    tripInsuranceInfo.tpiAIGReturnedMessageContentList = new List<MOBItem>();
                    tripInsuranceInfo.tpiAIGReturnedMessageContentList.Add(tpiContentMessage);
                }
            }

            string isNewTPIMessageHTML = appid == 2 ? "<br/><br/>" : "<br/>";
            string specialCharacter = HttpUtility.HtmlDecode(_configuration.GetValue<string>("TPIinfo-SpecialCharacter")) ?? "";
            if (tripInsuranceInfo != null && !string.IsNullOrEmpty(tripInsuranceInfo.Image) && !string.IsNullOrEmpty(tncProductPageLinkMessage) &&
                !string.IsNullOrEmpty(tncPaymentLinkMessage) &&
                tripInsuranceInfo.QuoteTitle != null && tripInsuranceInfo.QuoteTitle.Contains("(R)") && !isBookingPath)
            {
                //tripInsuranceInfo.Body3 = tncProductPageText1 + " <a href =\"" + tncProductPageLinkMessage + "\" target=\"_blank\">" + tncProductPageText2 + "</a>";
                //tripInsuranceInfo.TNC = tncPaymentText1 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + tncPaymentText3;

                tripInsuranceInfo.Body3 = (mobTgiLimitationMessage.IsNullOrEmpty() && mobTgiReadMessage.IsNullOrEmpty()) ?
                                          (tncProductPageText1 + " <a href =\"" + tncProductPageLinkMessage + "\" target=\"_blank\">" + tncProductPageText2 + "</a>")
                                          : (tncProductPageText1 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + mobTgiLimitationMessage + "</a> " + mobTgiReadMessage + " <a href =\"" + tncProductPageLinkMessage + "\" target=\"_blank\">" + tncProductPageText2 + "</a>");
                tripInsuranceInfo.Body3 = !_configuration.GetValue<bool>("IsDisableTPIForWashington") && !string.IsNullOrEmpty(tripInsuranceInfo.Body3) && !string.IsNullOrEmpty(mobTIMRWashingtonMessage) ? tripInsuranceInfo.Body3 + isNewTPIMessageHTML + mobTIMRWashingtonMessage : tripInsuranceInfo.Body3;
                tripInsuranceInfo.tNC = tncPaymentText1 + tncPaymentText3 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + mobTgiAndMessage +
                    " <a href =\"" + tncProductPageLinkMessage + "\" target=\"_blank\">" + tncProductPageText2 + "</a>";
                tripInsuranceInfo.QuoteTitle = @tripInsuranceInfo.QuoteTitle.Replace("(R)", specialCharacter);
                tripInsuranceInfo.PageTitle = HttpUtility.HtmlDecode(_configuration.GetValue<string>("TPIinfo-PageTitle")) ?? "";
                tripInsuranceInfo.Headline2 = _configuration.GetValue<string>("TPIinfo-Headline2") ?? "";
                tripInsuranceInfo.PaymentContent = _configuration.GetValue<string>("TPIinfo-PaymentContent") ?? "";
                if (_configuration.GetValue<bool>("EnableTravelOptionsInViewRes"))
                {
                    tripInsuranceInfo.OfferHeaderV2 = mobOfferHeaderV2Message.Replace("(R)", specialCharacter);
                    tripInsuranceInfo.OfferTitleV2 = mobOfferTitleV2Message;
                }
                // confirmation page use
                if (isShoppingCall)
                {
                    United.Mobile.Model.Shopping.Reservation bookingPathReservation = new United.Mobile.Model.Shopping.Reservation();
                    bookingPathReservation = await _sessionHelperService.GetSession<United.Mobile.Model.Shopping.Reservation>(sessionId, new United.Mobile.Model.Shopping.Reservation().GetType().FullName, new List<string> { sessionId, new United.Mobile.Model.Shopping.Reservation().GetType().FullName }).ConfigureAwait(false);
                    if (bookingPathReservation == null)
                    {
                        bookingPathReservation = new United.Mobile.Model.Shopping.Reservation();
                    }
                    if (bookingPathReservation.TripInsuranceFile == null)
                    {
                        bookingPathReservation.TripInsuranceFile = new TripInsuranceFile() { };
                    }
                    bookingPathReservation.TripInsuranceFile.ConfirmationResponseDetailMessage1 = @confirmationResponseDetailMessage1.Replace("(R)", specialCharacter);
                    bookingPathReservation.TripInsuranceFile.ConfirmationResponseDetailMessage2 = confirmationResponseDetailMessage2;
                    await _sessionHelperService.SaveSession<United.Mobile.Model.Shopping.Reservation>(bookingPathReservation, bookingPathReservation.SessionId, new List<string> { bookingPathReservation.SessionId, bookingPathReservation.ObjectName }, bookingPathReservation.ObjectName).ConfigureAwait(false);
                }
                else
                {
                    tripInsuranceInfo.Confirmation1 = @confirmationResponseDetailMessage1.Replace("(R)", specialCharacter);
                    tripInsuranceInfo.Confirmation2 = confirmationResponseDetailMessage2;
                }
            }
            else if (isShoppingCall && isBookingPath && !string.IsNullOrEmpty(contentInBooking1) && !string.IsNullOrEmpty(contentInBooking2) && !string.IsNullOrEmpty(contentInBooking3)
                        && !string.IsNullOrEmpty(header1) && !string.IsNullOrEmpty(header2) && !string.IsNullOrEmpty(tncProductPageText1)
                        && !string.IsNullOrEmpty(bookingTncLinkMsg) && !string.IsNullOrEmpty(bookingTncContentMsg) && !string.IsNullOrEmpty(bookingLegalInfoContentMsg)
                        && !string.IsNullOrEmpty(tncPaymentLinkMessage) && !string.IsNullOrEmpty(tncPaymentText2) && !string.IsNullOrEmpty(tncPaymentText3) && !string.IsNullOrEmpty(bookingImg)
                        && !string.IsNullOrEmpty(tripInsuranceInfo.CoverCost) && !string.IsNullOrEmpty(tncPaymentText1)
                        && !string.IsNullOrEmpty(confirmationResponseDetailMessage1)
                        && !string.IsNullOrEmpty(confirmationResponseDetailMessage2))
            {
                //tripInsuranceInfo.Body3 = tncProductPageText1 + " <a href =\"" + bookingTncLinkMsg + "\" target=\"_blank\">" + bookingTncContentMsg + "</a>";
                //tripInsuranceInfo.TNC = bookingLegalInfoContentMsg + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + tncPaymentText3;
                tripInsuranceInfo.Body3 = (mobTgiLimitationMessage.IsNullOrEmpty() && mobTgiReadMessage.IsNullOrEmpty()) ?
                                           tncProductPageText1 + " <a href =\"" + bookingTncLinkMsg + "\" target=\"_blank\">" + bookingTncContentMsg + "</a>"
                                         : (tncProductPageText1 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + mobTgiLimitationMessage + "</a> " + mobTgiReadMessage + " <a href =\"" + bookingTncLinkMsg + "\" target=\"_blank\">" + bookingTncContentMsg + "</a>");
                tripInsuranceInfo.Body3 = !_configuration.GetValue<bool>("IsDisableTPIForWashington") && !string.IsNullOrEmpty(tripInsuranceInfo.Body3) && !string.IsNullOrEmpty(mobTIMRWashingtonMessage) ? tripInsuranceInfo.Body3 + isNewTPIMessageHTML + mobTIMRWashingtonMessage : tripInsuranceInfo.Body3;
                tripInsuranceInfo.tNC = (mobTgiLimitationMessage.IsNullOrEmpty() && mobTgiReadMessage.IsNullOrEmpty()) ? bookingLegalInfoContentMsg + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + tncPaymentText3
                                        : bookingLegalInfoContentMsg + " " + tncPaymentText3 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + mobTgiAndMessage + " <a href =\"" + bookingTncLinkMsg + "\" target=\"_blank\">" + bookingTncContentMsg + "</a>";

                tripInsuranceInfo.PageTitle = HttpUtility.HtmlDecode(_configuration.GetValue<string>("TPIinfo-PageTitle")) ?? "";
                tripInsuranceInfo.Image = bookingImg;
                Mobile.Model.Shopping.Reservation bookingPathReservation = new Mobile.Model.Shopping.Reservation();
                bookingPathReservation = await _sessionHelperService.GetSession<Mobile.Model.Shopping.Reservation>(sessionId, bookingPathReservation.ObjectName, new List<string> { sessionId, bookingPathReservation.ObjectName }).ConfigureAwait(false);
                if (bookingPathReservation == null)
                {
                    bookingPathReservation = new Mobile.Model.Shopping.Reservation();
                }
                if (bookingPathReservation.TripInsuranceFile == null)
                {
                    bookingPathReservation.TripInsuranceFile = new TripInsuranceFile() { };
                }
                if (bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo == null)
                {
                    bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo = new United.Mobile.Model.Shopping.TPIInfoInBookingPath() { };
                }
                List<string> contentInBooking = new List<string>();
                contentInBooking.Add(contentInBooking1);
                contentInBooking.Add(contentInBooking2);
                contentInBooking.Add(contentInBooking3);
                bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo.Content = contentInBooking;
                bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo.Header = header1 + " <b>" + header2 + "</b>";
                bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo.LegalInformation = legalInfo;
                bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo.TncSecondaryFOPPage = (mobTgiLimitationMessage.IsNullOrEmpty() && mobTgiReadMessage.IsNullOrEmpty()) ? tncPaymentText1 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + tncPaymentText3
                       : tncPaymentText1 + " " + tncPaymentText3 + " <a href =\"" + tncPaymentLinkMessage + "\" target=\"_blank\">" + tncPaymentText2 + "</a> " + mobTgiAndMessage + " <a href =\"" + tncProductPageLinkMessage + "\" target=\"_blank\">" + tncProductPageText2 + "</a> ";
                bookingPathReservation.TripInsuranceFile.TripInsuranceBookingInfo.ConfirmationMsg = @confirmationResponseDetailMessage1.Replace("(R)", specialCharacter) + "\n\n" + confirmationResponseDetailMessage2;
                await _sessionHelperService.SaveSession<Mobile.Model.Shopping.Reservation>(bookingPathReservation, sessionId, new List<string> { sessionId, bookingPathReservation.ObjectName }, bookingPathReservation.ObjectName).ConfigureAwait(false);
            }
            else
            {
                tripInsuranceInfo = null;
            }
            return tripInsuranceInfo;
        }
        private static double GetAmount(ProductPriceOption priceOption)
        {
            if (priceOption == null ||
                priceOption.PaymentOptions == null ||
                !priceOption.PaymentOptions.Any() ||
                priceOption.PaymentOptions[0] == null ||
                priceOption.PaymentOptions[0].PriceComponents == null ||
                !priceOption.PaymentOptions[0].PriceComponents.Any() ||
                priceOption.PaymentOptions[0].PriceComponents[0].Price == null ||
                priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals == null ||
                !priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals.Any())
                return 0;

            return priceOption.PaymentOptions[0].PriceComponents[0].Price.Totals[0].Amount;
        }
        #endregion
    }
}
