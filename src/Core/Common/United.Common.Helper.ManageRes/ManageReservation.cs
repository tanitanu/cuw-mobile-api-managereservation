using EmployeeRes.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Common.Helper.SeatEngine;
using United.Common.HelperSeatEngine;
using United.Definition;
using United.Mobile.DataAccess.CMSContent;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.DataAccess.Profile;
using United.Mobile.DataAccess.ReShop;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Fitbit;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.CommonModel;
using United.Service.Presentation.LoyaltyModel;
using United.Service.Presentation.PersonalizationModel;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ReferenceDataRequestModel;
using United.Service.Presentation.ReferenceDataResponseModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using Characteristic = United.Service.Presentation.CommonModel.Characteristic;
using FlowType = United.Utility.Enum.FlowType;
using MOBPriorityBoarding = United.Mobile.Model.MPRewards.MOBPriorityBoarding;
using United.Mobile.DataAccess.ShopTrips;


namespace United.Common.Helper.ManageRes
{
    public class ManageReservation : IManageReservation
    {
        private readonly ICacheLog<ManageReservation> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IHeaders _headers;
        private readonly IDPService _dPService;
        private readonly ITravelerCSL _travelerCSL;
        private readonly IPNRRetrievalService _pNRRetrievalService;
        private readonly ISaveTriptoMyAccountService _saveTriptoMyAccountService;
        private readonly IReferencedataService _referencedataService;
        private readonly IFlightReservation _flightReservation;
        private readonly IMerchandizingServices _merchandizingServices;
        private readonly IDynamoDBService _dynamoDBService;
        private List<MOBLegalDocument> cachedLegalDocuments = null;
        private readonly ICachingService _cachingService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private const string DOTBaggageInfoDBTitle1 = "DOTBaggageInfoText1";
        private const string DOTBaggageInfoDBTitle1ELF = "DOTBaggageInfoText1 - ELF";
        private const string DOTBaggageInfoDBTitle1IBE = "DOTBaggageInfoText1 - IBE";
        private const string DOTBaggageInfoDBTitle2 = "DOTBaggageInfoText2";
        private const string DOTBaggageInfoDBTitle3 = "DOTBaggageInfoText3";
        private const string DOTBaggageInfoDBTitle3IBE = "DOTBaggageInfoText3IBE";
        private const string DOTBaggageInfoDBTitle4 = "DOTBaggageInfoText4";
        private readonly ManageResUtility _manageResUtility;
        private readonly IRefundService _refundService;
        private readonly ICMSContentService _cMSContentService;
        private readonly IFeatureSettings _featureSettings;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly ISeatEngine _seatEngine;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly ISeatMapCSL30 _seatMapCSL30;
        private readonly IShoppingCartService _shoppingCartService;

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


        public ManageReservation(ICacheLog<ManageReservation> logger
            , IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IHeaders headers
            , IDPService dPService
            , ITravelerCSL travelerCSL
            , IPNRRetrievalService pNRRetrievalService
            , ISaveTriptoMyAccountService saveTriptoMyAccountService
            , IReferencedataService referencedataService
            , IFlightReservation flightReservation
            , IMerchandizingServices merchandizingServices
            , IDynamoDBService dynamoDBService
            , ICachingService cachingService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IRefundService refundService
            , ICMSContentService cMSContentService
            , IFeatureSettings featureSettings
            , IShoppingSessionHelper shoppingSessionHelper
            , ISeatEngine seatEngine
            , IShoppingUtility shoppingUtility
            , ISeatMapCSL30 seatMapCSL30
            , IShoppingCartService shoppingCartService)
        {
            _logger = logger;
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _headers = headers;
            _dPService = dPService;
            _travelerCSL = travelerCSL;
            _pNRRetrievalService = pNRRetrievalService;
            _saveTriptoMyAccountService = saveTriptoMyAccountService;
            _referencedataService = referencedataService;
            _flightReservation = flightReservation;
            _merchandizingServices = merchandizingServices;
            _dynamoDBService = dynamoDBService;
            _cachingService = cachingService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _manageResUtility = new ManageResUtility(_configuration, _legalDocumentsForTitlesService, _dynamoDBService, _headers, _logger);
            _refundService = refundService;
            _cMSContentService = cMSContentService;
            _featureSettings = featureSettings;
            _shoppingSessionHelper = shoppingSessionHelper;
            _seatEngine = seatEngine;
            _shoppingUtility = shoppingUtility;
            _seatMapCSL30 = seatMapCSL30;
            _shoppingCartService = shoppingCartService;
        }

        public async Task<MOBPNRByRecordLocatorResponse> GetPNRByRecordLocatorCommonMethod(MOBPNRByRecordLocatorRequest request)
        {
            MOBPNRByRecordLocatorResponse response = new MOBPNRByRecordLocatorResponse();

            #region
            CommonDef commonDef = new CommonDef();
            commonDef.SampleJsonResponse = JsonConvert.SerializeObject(request);

            string data = (request.DeviceId + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim();
            await _sessionHelperService.SaveSession<CommonDef>(commonDef, data, new List<string> { data, commonDef.ObjectName }, commonDef.ObjectName).ConfigureAwait(false);

            response.TransactionId = request.TransactionId;

            new ForceUpdateVersion(_configuration).ForceUpdateForNonSupportedVersion(request.Application.Id, request.Application.Version.Major, FlowType.MANAGERES);

            //ALM 24932/25453 - Throwing exception, if last name is null
            //Srini Penmetsa - Dec 21, 2015
            if (string.IsNullOrEmpty(request.LastName))
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("InvalidPNRLastName-ExceptionMessage"));
            }
            //End ALM 24932

            #region
            if (GeneralHelper.ValidateAccessCode(request.AccessCode))
            {
                response.RecordLocator = request.RecordLocator;
                response.LastName = request.LastName;

                //Fix for MOBILE-9372 : Incorrect seat number for 2nd segment via seat link on Res detail -- Shashank
                if (!_configuration.GetValue<bool>("DisableFixforEmptyRequestFlowMOBILE9372"))
                {
                    response.Flow = !string.IsNullOrEmpty(request.Flow) ? request.Flow : FlowType.VIEWRES.ToString();
                }

                Session session = new Session();
                session = await _sessionHelperService.GetSession<Session>(request.SessionId, session.ObjectName, new List<string> { request.SessionId, session.ObjectName }).ConfigureAwait(false);
                var tupleRes = await LoadPnr(request, session);
                response.PNR = tupleRes.pnr;

                if (_configuration.GetValue<bool>("joinOneClickMileagePlusEnabled") && response.PNR != null)
                {
                    response.PNR.OneClickEnrollmentEligibility = new MOBOneClickEnrollmentEligibility()
                    {
                        JoinMileagePlus = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlus")) ? _configuration.GetValue<string>("joinMileagePlus") : string.Empty,
                        JoinMileagePlusHeader = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusHeader")) ? _configuration.GetValue<string>("joinMileagePlusHeader") : string.Empty,
                        JoinMileagePlusText = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusText")) ? _configuration.GetValue<string>("joinMileagePlusText") : string.Empty
                    };
                }

                await CheckForFuzziPNRAndSaveCommonDefPersistsFile(request.RecordLocator + "_" + request.SessionId, request.DeviceId, response.PNR.RecordLocator, commonDef);

                await AddAncillaryToPnrResponse(request, response, session, tupleRes.clsReservationDetail);
                SupressWhenScheduleChange(response);

                if (request.Flow != FlowType.VIEWRES_SEATMAP.ToString() && _configuration.GetValue<bool>("EnableMgnResUpdateTravelerInfo") == true)
                {
                    try
                    {
                        response = await RetrieveAndMapSpecialNeeds(session, request, tupleRes.clsReservationDetail.Detail.FlightSegments, response);
                        await _sessionHelperService.SaveSession<MOBPNR>(response.PNR, request.SessionId, new List<string> { request.SessionId, new MOBPNR().ObjectName }, new MOBPNR().ObjectName).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("GetPNRByRecordLocator_CFOP {ExceptionStackMessage}, {MPNumber}, {transactionID}", JsonConvert.SerializeObject(ex), response.PNR, response.TransactionId);
                    }
                }
                //Ends Here

                if (request.IsAddTriptoMyAccount && request.CatalogValues != null && request.CatalogValues.Count > 0 && IsEnableSaveTriptoMyAccount(request?.CatalogValues))
                {
                    System.Threading.Tasks.Task.Factory.StartNew(() => AddTriptoMyAccount(tupleRes.pnr, session, request.RecordLocator, request.MileagePlusNumber));
                }

                //Adding this to enable us to query logs with pnr
                var sessionDetails = GetSessionDetailsMessageForLogging(request.SessionId, request, response);
            }
            else
            {
                throw new MOBUnitedException("Invalid access code");
            }
            #endregion

            return response;
            #endregion
        }

        private void AddTriptoMyAccount(MOBPNR pnr, Session session, string recordLocator, string mileagePlusNumber)
        {
            var saveToMyAccountRequest = new MOBAddTriptoMyAccountRequest()
            {
                RecordLocator = recordLocator,
                ArrivalAirportName = pnr.Trips[0].DestinationName,
                ArrivalIATAcode = pnr.Trips[0].Destination,
                CreateDate = pnr.DateCreated,
                DepartureAirportName = pnr.Trips[0].OriginName,
                DepartureIAtACode = pnr.Trips[0].Origin,
                LoyaltyMemberID = mileagePlusNumber,
            };

            string jsonRequest = JsonConvert.SerializeObject(saveToMyAccountRequest);

            _saveTriptoMyAccountService.SaveTriptoMyAccount(session.Token, "/SavetoMyAccount", jsonRequest, session.SessionId).ConfigureAwait(false);
        }

        public bool IsEnableSaveTriptoMyAccount(List<MOBItem> catalogItems)
        {
            return catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableSaveTriptoMyAccount).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableSaveTriptoMyAccount).ToString())?.CurrentValue == "1";
        }

        private string GetSessionDetailsMessageForLogging(string transactionId, MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response)
        {
            if (transactionId != null && request != null &&
                response != null && response.PNR != null && response.PNR.RecordLocator != null)
            {
                return "RecordLocator = " + request.RecordLocator +
                       " | sessionId = " + response.PNR.SessionId +
                       " | transcationId = " + transactionId;
            }

            return "No details found";
        }

        private async Task<MOBPNRByRecordLocatorResponse> RetrieveAndMapSpecialNeeds
            (Session session, MOBPNRByRecordLocatorRequest request,
            Collection<ReservationFlightSegment> flightSegments,
            MOBPNRByRecordLocatorResponse mobResponse,
            string langType = "en-US")
        {
            if (session == null) return mobResponse;
            if (mobResponse.PNR.IsCanceledWithFutureFlightCredit) return mobResponse;

            bool IsAdvisoryMsgNeeded = false;
            var highTouchInclude = new List<string>();
            var hightouchitem = new List<string> { "DPNA_1", "DPNA_2", "WCHC" };
            List<MOBTravelSpecialNeed> selectedHighTouchItem = new List<MOBTravelSpecialNeed>();

            var specialNeeds = await GetItineraryAvailableSpecialNeeds
                            (session, request.Application.Id, request.Application.Version.Major, request.DeviceId, flightSegments, langType);

            if (!_configuration.GetValue<bool>("DisableServiceAnimalEnhancements"))
            {
                if (flightSegments.Any(s => s.FlightSegment.IsInternational.Equals("TRUE", StringComparison.OrdinalIgnoreCase))
                    || flightSegments.Any(s => _configuration.GetValue<string>("DisableServiceAnimalAirportCodes").Contains(s?.FlightSegment?.DepartureAirport?.IATACode))
                    || flightSegments.Any(s => _configuration.GetValue<string>("DisableServiceAnimalAirportCodes").Contains(s?.FlightSegment?.ArrivalAirport?.IATACode)))
                {
                    if (specialNeeds?.SpecialRequests != null && specialNeeds.SpecialRequests.Any())
                    {
                        specialNeeds.SpecialRequests.Remove(specialNeeds.SpecialRequests.Where
                        (sr => sr.Code.Equals("SVAN", StringComparison.OrdinalIgnoreCase)).FirstOrDefault());
                    }
                }
            }

            List<string> specialMealsItem = new List<string>();
            if (specialNeeds?.SpecialMeals != null && specialNeeds.SpecialMeals.Any())
                specialMealsItem = specialNeeds.SpecialMeals.Where(x => (!string.IsNullOrEmpty(x.Code))).Select(y => Convert.ToString(y.Code)).ToList();
            List<string> specialRequestsItem = new List<string>();
            if (specialNeeds?.SpecialRequests != null && specialNeeds.SpecialRequests.Any())
            {
                specialRequestsItem = specialNeeds.SpecialRequests.Where(x => ((!string.IsNullOrEmpty(x.Code) && (x.SubOptions == null))))
                    .Select(y => Convert.ToString(y.Code)).ToList();

                specialNeeds.SpecialRequests.ForEach(x =>
                {
                    if (x.SubOptions != null && x.SubOptions.Any())
                    {
                        x.SubOptions.ForEach(y =>
                        {
                            if (!string.IsNullOrEmpty(y.Code))
                            {
                                specialRequestsItem.Add(y.Code);
                            }
                        });
                    }
                });
            }

            if (mobResponse != null && mobResponse.PNR != null && mobResponse.PNR.Passengers != null)
            {
                mobResponse.PNR.Passengers.ForEach(passenger =>
                {
                    if (passenger.SelectedSpecialNeeds != null && passenger.SelectedSpecialNeeds.Any())
                    {
                        var filteredAllSelectedSpecialNeeds = passenger.SelectedSpecialNeeds
                        .Where(x => (specialMealsItem.Contains(x.Code) || specialRequestsItem.Contains(x.Code)));

                        var ssrDisplaySequenceList = filteredAllSelectedSpecialNeeds
                        .Where(x => (!string.IsNullOrEmpty(x.DisplaySequence))).GroupBy(y => y.DisplaySequence).Select(z => z.First())
                        .Select(u => u.DisplaySequence).ToList();

                        passenger.SSRDisplaySequence = string.Join("|", ssrDisplaySequenceList);

                        passenger.SelectedSpecialNeeds = new List<MOBTravelSpecialNeed>();
                        passenger.SelectedSpecialNeeds = filteredAllSelectedSpecialNeeds.GroupBy(x => x.Code).Select(y => y.First())
                        .Select(z => new MOBTravelSpecialNeed()
                        {
                            Code = z.Code,
                            DisplayDescription = z.DisplayDescription,
                            DisplaySequence = z.DisplaySequence,
                            Value = z.Value
                        }).ToList();

                        if (passenger.SelectedSpecialNeeds != null && passenger.SelectedSpecialNeeds.Any())
                        {
                            IsAdvisoryMsgNeeded = true;

                            var items = passenger.SelectedSpecialNeeds.Where(x => hightouchitem.Contains(x.Code));
                            if (items != null && items.Any())
                            {
                                selectedHighTouchItem.AddRange(items);
                                highTouchInclude.AddRange(items.Select(x => x.Code).ToList());
                            }

                            passenger.SelectedSpecialNeeds.ForEach(item =>
                            {
                                item = SetSpecialNeedProperties(item, specialNeeds);
                            });
                        }
                    }
                });
            }

            if (!IsAdvisoryMsgNeeded)
            {
                mobResponse.PNR.MealAccommodationAdvisory = string.Empty;
                mobResponse.PNR.MealAccommodationAdvisoryHeader = string.Empty;
            }

            if (specialNeeds != null && specialNeeds.SpecialRequests != null && specialNeeds.SpecialRequests.Any())
            {
                hightouchitem.RemoveAll(x => highTouchInclude.Contains(x));

                var specialRequest = specialNeeds.SpecialRequests.Where(sr => !hightouchitem.Contains(sr.Code)).ToList();

                if (specialRequest != null && specialRequest.Any())
                {
                    specialNeeds.SpecialRequests = new List<MOBTravelSpecialNeed>();
                    specialNeeds.SpecialRequests = specialRequest;
                }
            }

            specialNeeds.ServiceAnimals = null;
            mobResponse.SpecialNeeds = specialNeeds;
            specialNeeds.HighTouchItems = selectedHighTouchItem;
            specialNeeds.AccommodationsUnavailable = _configuration.GetValue<string>("travelNeedAttentionMessage");
            specialNeeds.MealUnavailable = _configuration.GetValue<string>("mealAttentionMessage");
            return mobResponse;
        }

        private MOBTravelSpecialNeed SetSpecialNeedProperties
            (MOBTravelSpecialNeed item, MOBTravelSpecialNeeds specialNeeds)
        {
            if (specialNeeds == null && item == null) return null;

            MOBTravelSpecialNeed specialmeal;
            specialmeal = (specialNeeds.SpecialMeals != null) ? specialNeeds.SpecialMeals.FirstOrDefault
                (x => string.Equals(x.Code, item.Code, StringComparison.OrdinalIgnoreCase)) : null;

            MOBTravelSpecialNeed specialrequest;
            specialrequest = (specialNeeds.SpecialRequests != null) ? specialNeeds.SpecialRequests.FirstOrDefault
            (x => string.Equals(x.Code, item.Code, StringComparison.OrdinalIgnoreCase)) : null;

            MOBTravelSpecialNeed specialrequestsuboption;
            specialrequestsuboption = specialNeeds.SpecialRequests.FirstOrDefault
            (x => !string.IsNullOrEmpty(x.SubOptionHeader));

            MOBTravelSpecialNeed suboption;

            if (specialrequestsuboption != null && specialrequestsuboption.SubOptions != null
            && specialrequestsuboption.SubOptions.Any())
            {
                suboption = specialrequestsuboption.SubOptions.FirstOrDefault
                (x => string.Equals(x.Code, item.Code, StringComparison.OrdinalIgnoreCase));
            }
            else
                suboption = null;

            if (suboption != null)
            {
                item.Code = !string.IsNullOrEmpty(specialrequestsuboption.Code) ? specialrequestsuboption.Code : string.Empty;
                item.DisplayDescription = !string.IsNullOrEmpty(specialrequestsuboption.DisplayDescription) ? specialrequestsuboption.DisplayDescription : string.Empty;
                item.RegisterServiceDescription = !string.IsNullOrEmpty(specialrequestsuboption.RegisterServiceDescription) ? specialrequestsuboption.RegisterServiceDescription : string.Empty;
                item.SubOptionHeader = !string.IsNullOrEmpty(specialrequestsuboption.SubOptionHeader) ? specialrequestsuboption.SubOptionHeader : string.Empty;
                item.SubOptions = new List<MOBTravelSpecialNeed>();
                item.SubOptions.Add(suboption);
                item.Type = !string.IsNullOrEmpty(specialrequestsuboption.Type) ? specialrequestsuboption.Type : string.Empty;
            }
            else
            {
                item.Type = (specialmeal != null) ? specialmeal.Type : (specialrequest != null) ? specialrequest.Type : string.Empty;
                item.DisplayDescription = (specialmeal != null) ? specialmeal.DisplayDescription : (specialrequest != null) ? specialrequest.DisplayDescription : string.Empty;
            }

            if (item.Messages != null && item.Messages.Any())
            {
                item.Messages = new List<MOBItem>();
                item.Messages = (specialmeal.Messages != null) ? specialmeal.Messages : (specialrequest.Messages != null) ? specialrequest.Messages : null;
            }

            return item;
        }

        private async Task<MOBTravelSpecialNeeds> GetItineraryAvailableSpecialNeeds(Session session, int appId, string appVersion, string deviceId, IEnumerable<ReservationFlightSegment> segments,
            string languageCode, MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null)
        {
            MultiCallResponse flightshoppingReferenceData = null;
            IEnumerable<ReservationFlightSegment> pnrOfferedMeals = null;
            var offersSSR = new MOBTravelSpecialNeeds();

            try
            {
                //Parallel.Invoke(() => flightshoppingReferenceData = GetSpecialNeedsReferenceDataFromFlightShopping(session, appId, appVersion, deviceId, languageCode),
                //                () => pnrOfferedMeals = GetOfferedMealsForItineraryFromPNRManagement(session, appId, appVersion, deviceId, segments));

                flightshoppingReferenceData = await GetSpecialNeedsReferenceDataFromFlightShopping(session, appId, appVersion, deviceId, languageCode);
                pnrOfferedMeals = await GetOfferedMealsForItineraryFromPNRManagement(session, appId, appVersion, deviceId, segments);
            }
            catch (Exception) // 'System.ArgumentException' is thrown when any action in the actions array throws an exception.
            {
                if (flightshoppingReferenceData == null) // unable to get reference data, POPULATE DEFAULT SPECIAL REQUESTS
                {
                    offersSSR.ServiceAnimalsMessages = new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSR_RefDataServiceFailure_ServiceAnimalMassage") } };

                    flightshoppingReferenceData = GetMultiCallResponseWithDefaultSpecialRequests();
                }
                else if (pnrOfferedMeals == null) // unable to get market restriction meals, POPULATE DEFAULT MEALS
                {
                    pnrOfferedMeals = PopulateSegmentsWithDefaultMeals(segments);
                }
            }

            Parallel.Invoke(() => offersSSR.SpecialMeals = GetOfferedMealsForItinerary(pnrOfferedMeals, flightshoppingReferenceData),
                            () => offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData),
                            async () => offersSSR.SpecialRequests = await GetOfferedSpecialRequests(flightshoppingReferenceData, reservation, selectRequest, session),
                            () => offersSSR.SpecialMealsMessages = GetSpecialMealsMessages(pnrOfferedMeals, flightshoppingReferenceData),
                            () => offersSSR.ServiceAnimals = GetOfferedServiceAnimals(flightshoppingReferenceData, segments, appId, appVersion),
                () => offersSSR.SpecialNeedsAlertMessages = GetPartnerAirlinesSpecialTravelNeedsMessage(session, reservation));


            if (!string.IsNullOrEmpty(_configuration.GetValue<string>("RemoveEmotionalSupportServiceAnimalOption_EffectiveDateTime"))
                && Convert.ToDateTime(_configuration.GetValue<string>("RemoveEmotionalSupportServiceAnimalOption_EffectiveDateTime")) <= DateTime.Now
                && offersSSR.ServiceAnimals != null && offersSSR.ServiceAnimals.Any())
            {
                offersSSR.ServiceAnimals.Remove(offersSSR.ServiceAnimals.FirstOrDefault(x => x.Code == "ESAN" && x.Value == "6"));
            }
            if (IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion)
                 && offersSSR?.ServiceAnimals != null && offersSSR.ServiceAnimals.Any(x => x.Code == "ESAN" && x.Value == "5"))
            {
                offersSSR.ServiceAnimals.Remove(offersSSR.ServiceAnimals.FirstOrDefault(x => x.Code == "ESAN" && x.Value == "5"));
            }

            await AddServiceAnimalsMessageSection(offersSSR, appId, appVersion, session, deviceId);

            if (offersSSR.ServiceAnimalsMessages == null || !offersSSR.ServiceAnimalsMessages.Any())
                offersSSR.ServiceAnimalsMessages = GetServiceAnimalsMessages(offersSSR.ServiceAnimals);


            return offersSSR;
        }

        private Mobile.Model.Common.MOBAlertMessages GetPartnerAirlinesSpecialTravelNeedsMessage(Session session, MOBSHOPReservation reservation)
        {
            if (_configuration.GetValue<bool>("EnableAirlinesFareComparison") && session.CatalogItems != null && session.CatalogItems.Count > 0 &&
                  session.CatalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableNewPartnerAirlines).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableNewPartnerAirlines).ToString())?.CurrentValue == "1"
                 && reservation?.Trips?.FirstOrDefault()?.FlattenedFlights?.FirstOrDefault()?.Flights?.FirstOrDefault().OperatingCarrier != null
                   && _configuration.GetValue<string>("SupportedAirlinesFareComparison").Contains(reservation.Trips?.FirstOrDefault()?.FlattenedFlights?.FirstOrDefault()?.Flights?.FirstOrDefault().OperatingCarrier.ToUpper())
                  )
            {

                Mobile.Model.Common.MOBAlertMessages specialNeedsAlertMessages = new Mobile.Model.Common.MOBAlertMessages
                {
                    HeaderMessage = _configuration.GetValue<string>("PartnerAirlinesSpecialTravelNeedsHeader"),
                    IsDefaultOption = true,
                    MessageType = MOBMESSAGETYPES.WARNING.ToString(),
                    AlertMessages = new List<MOBSection>
                        {
                            new MOBSection
                            {
                                MessageType = MOBMESSAGETYPES.WARNING.ToString(),
                                Text1 = _configuration.GetValue<string>("PartnerAirlinesSpecialTravelNeedsMessage"),
                                Order = "1"
                            }
                        }
                };
                return specialNeedsAlertMessages;
            }
            return null;
        }

        internal virtual List<MOBItem> GetServiceAnimalsMessages(List<MOBTravelSpecialNeed> serviceAnimals)
        {
            if (serviceAnimals != null && serviceAnimals.Any())
                return null;

            return new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSRItineraryServiceAnimalNotAvailableMsg") } };
        }


        private async System.Threading.Tasks.Task AddServiceAnimalsMessageSection(MOBTravelSpecialNeeds offersSSR, int appId, string appVersion, Session session, string deviceId)
        {
            if (IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion))
            {
                if (offersSSR?.ServiceAnimals == null) offersSSR.ServiceAnimals = new List<MOBTravelSpecialNeed>();
                MOBRequest request = new MOBRequest();
                request.Application = new MOBApplication();
                request.Application.Id = appId;
                request.Application.Version = new MOBVersion();
                request.Application.Version.Major = appVersion;
                request.DeviceId = deviceId;

                string cmsCacheResponse = await _cachingService.GetCache<CSLContentMessagesResponse>(_configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID") + "MOBCSLContentMessagesResponse", "trans0").ConfigureAwait(false);
                CSLContentMessagesResponse content = new CSLContentMessagesResponse();

                if (string.IsNullOrEmpty(cmsCacheResponse))
                    content = await _travelerCSL.GetBookingRTICMSContentMessages(request, session);//, LogEntries);
                else
                    content = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(cmsCacheResponse);

                string emotionalSupportAssistantContent = (content?.Messages?.FirstOrDefault(m => !string.IsNullOrEmpty(m.Title) && m.Title.Equals("TravelNeeds_TaskTrainedDog_Screen_Content_MOB"))?.ContentFull) ?? "";
                string emotionalSupportAssistantCodeVale = _configuration.GetValue<string>("TravelSpecialNeedInfoCodeValue");

                if (!string.IsNullOrEmpty(emotionalSupportAssistantContent) && !string.IsNullOrEmpty(emotionalSupportAssistantCodeVale))
                {
                    var codeValue = emotionalSupportAssistantCodeVale.Split('#');
                    offersSSR.ServiceAnimals.Add(new MOBTravelSpecialNeed
                    {
                        Code = codeValue[0],
                        Value = codeValue[1],
                        DisplayDescription = "",
                        Type = TravelSpecialNeedType.TravelSpecialNeedInfo.ToString(),
                        Messages = new List<MOBItem>
                        {
                            new MOBItem {
                                CurrentValue = emotionalSupportAssistantContent
                            }
                        }
                    });
                }
            }

            else if (_configuration.GetValue<bool>("EnableTravelSpecialNeedInfo")
                && GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("TravelSpecialNeedInfo_Supported_AppVestion_Android"), _configuration.GetValue<string>("TravelSpecialNeedInfo_Supported_AppVestion_iOS"))
                && offersSSR.ServiceAnimals != null && offersSSR.ServiceAnimals.Any())
            {
                string emotionalSupportAssistantHeading = _configuration.GetValue<string>("TravelSpecialNeedInfoHeading");
                string emotionalSupportAssistantContent = _configuration.GetValue<string>("TravelSpecialNeedInfoContent");
                string emotionalSupportAssistantCodeVale = _configuration.GetValue<string>("TravelSpecialNeedInfoCodeValue");

                if (!string.IsNullOrEmpty(emotionalSupportAssistantHeading) &&
                    !string.IsNullOrEmpty(emotionalSupportAssistantContent) &&
                    !string.IsNullOrEmpty(emotionalSupportAssistantCodeVale))
                {
                    var codeValue = emotionalSupportAssistantCodeVale.Split('#');

                    offersSSR.ServiceAnimals.Add(new MOBTravelSpecialNeed
                    {
                        Code = codeValue[0],
                        Value = codeValue[1],
                        DisplayDescription = emotionalSupportAssistantHeading,
                        Type = TravelSpecialNeedType.TravelSpecialNeedInfo.ToString(),
                        Messages = new List<MOBItem>
                        {
                            new MOBItem {
                                CurrentValue = emotionalSupportAssistantContent
                            }
                        }
                    });
                }
            }
        }


        private bool IsTaskTrainedServiceDogSupportedAppVersion(int appId, string appVersion)
        {
            return GeneralHelper.IsApplicationVersionGreaterorEqual(appId, appVersion, _configuration.GetValue<string>("TravelSpecialNeedInfo_TaskTrainedServiceDog_Supported_AppVestion_Android"), _configuration.GetValue<string>("TravelSpecialNeedInfo_TaskTrainedServiceDog_Supported_AppVestion_iOS"));
        }

        private List<MOBTravelSpecialNeed> GetOfferedServiceAnimals(MultiCallResponse flightshoppingReferenceData, IEnumerable<ReservationFlightSegment> segments, int appId, string appVersion)
        {
            if (!IsTaskTrainedServiceDogSupportedAppVersion(appId, appVersion) &&
               !_configuration.GetValue<bool>("ShowServiceAnimalInTravelNeeds"))
                return null;

            if (segments == null || !segments.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.ServiceAnimalResponses == null || !flightshoppingReferenceData.ServiceAnimalResponses.Any()
                || flightshoppingReferenceData.ServiceAnimalResponses[0].Animals == null || !flightshoppingReferenceData.ServiceAnimalResponses[0].Animals.Any()
                || flightshoppingReferenceData.ServiceAnimalTypeResponses == null || !flightshoppingReferenceData.ServiceAnimalTypeResponses.Any()
                || flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types == null || !flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types.Any())
                return null;

            if (!DoesItineraryHaveServiceAnimal(segments))
                return null;

            var SSRAnimalValueCodeDesc = _configuration.GetValue<string>("SSRAnimalValueCodeDesc").Split('|').ToDictionary(x => x.Split('^')[0], x => x.Split('^')[1]);
            var SSRAnimalTypeValueCodeDesc = _configuration.GetValue<string>("SSRAnimalTypeValueCodeDesc").Split('|').ToDictionary(x => x.Split('^')[0], x => x.Split('^')[1]);

            Func<string, string, string, string, string, MOBTravelSpecialNeed> createSpecialNeed = (code, value, desc, RegisterServiceDesc, type)
                => new MOBTravelSpecialNeed { Code = code, Value = value, DisplayDescription = desc, RegisterServiceDescription = RegisterServiceDesc, Type = type };

            List<MOBTravelSpecialNeed> animals = flightshoppingReferenceData.ServiceAnimalResponses[0].Animals
                                                .Where(x => !string.IsNullOrWhiteSpace(x.Description))
                                                .Select(x => createSpecialNeed(SSRAnimalValueCodeDesc[x.Value], x.Value, x.Description, x.Description, TravelSpecialNeedType.ServiceAnimal.ToString())).ToList();

            Func<Service.Presentation.CommonModel.Characteristic, MOBTravelSpecialNeed> createServiceAnimalTypeItem = animalType =>
            {
                var type = createSpecialNeed(SSRAnimalTypeValueCodeDesc[animalType.Value], animalType.Value,
                                             animalType.Description, animalType.Description.EndsWith("animal", StringComparison.OrdinalIgnoreCase) ? null : "Dog", TravelSpecialNeedType.ServiceAnimalType.ToString());
                type.SubOptions = animalType.Description.EndsWith("animal", StringComparison.OrdinalIgnoreCase) ? animals : null;
                return type;
            };

            return flightshoppingReferenceData.ServiceAnimalTypeResponses[0].Types.Where(x => !string.IsNullOrWhiteSpace(x.Description))
                                                                                  .Select(createServiceAnimalTypeItem).ToList();
        }

        private bool DoesItineraryHaveServiceAnimal(IEnumerable<ReservationFlightSegment> segments)
        {
            var statesDoNotAllowServiceAnimal = new HashSet<string>(_configuration.GetValue<string>("SSRStatesDoNotAllowServiceAnimal").Split('|'));
            foreach (var segment in segments)
            {
                if (segment == null || segment.FlightSegment == null || segment.FlightSegment.ArrivalAirport == null || segment.FlightSegment.DepartureAirport == null
                    || segment.FlightSegment.ArrivalAirport.IATACountryCode == null || segment.FlightSegment.DepartureAirport.IATACountryCode == null
                    || string.IsNullOrWhiteSpace(segment.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode) || string.IsNullOrWhiteSpace(segment.FlightSegment.DepartureAirport.IATACountryCode.CountryCode)
                    || segment.FlightSegment.ArrivalAirport.StateProvince == null || segment.FlightSegment.DepartureAirport.StateProvince == null
                    || string.IsNullOrWhiteSpace(segment.FlightSegment.ArrivalAirport.StateProvince.StateProvinceCode) || string.IsNullOrWhiteSpace(segment.FlightSegment.DepartureAirport.StateProvince.StateProvinceCode)

                    || !segment.FlightSegment.ArrivalAirport.IATACountryCode.CountryCode.Equals("US") || !segment.FlightSegment.DepartureAirport.IATACountryCode.CountryCode.Equals("US") // is international
                    || statesDoNotAllowServiceAnimal.Contains(segment.FlightSegment.ArrivalAirport.StateProvince.StateProvinceCode) // touches states that not allow service animal
                    || statesDoNotAllowServiceAnimal.Contains(segment.FlightSegment.DepartureAirport.StateProvince.StateProvinceCode)) // touches states that not allow service animal
                    return false;
            }

            return true;
        }

        private async Task<MOBMobileCMSContentMessages> GetCMSContentMessageByKey(string Key, MOBRequest request, Session session)
        {

            CSLContentMessagesResponse cmsResponse = new CSLContentMessagesResponse();
            MOBMobileCMSContentMessages cmsMessage = null;
            List<CMSContentMessage> cmsMessages = null;
            try
            {
                var cmsContentCache = await _cachingService.GetCache<string>(_configuration.GetValue<string>("BookingPathRTI_CMSContentMessagesCached_StaticGUID") + "MOBCSLContentMessagesResponse", session.SessionId);
                try
                {
                    if (!string.IsNullOrEmpty(cmsContentCache))
                        cmsResponse = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(cmsContentCache);
                }
                catch { cmsContentCache = null; }

                if (string.IsNullOrEmpty(cmsContentCache) || Convert.ToBoolean(cmsResponse.Status) == false || cmsResponse.Messages == null)
                    cmsResponse = await _travelerCSL.GetBookingRTICMSContentMessages(request, session);

                cmsMessages = (cmsResponse != null && cmsResponse.Messages != null && cmsResponse.Messages.Count > 0) ? cmsResponse.Messages : null;
                if (cmsMessages != null)
                {
                    var message = cmsMessages.Find(m => m.Title.Equals(Key));
                    if (message != null)
                    {
                        cmsMessage = new MOBMobileCMSContentMessages()
                        {
                            HeadLine = message.Headline,
                            ContentFull = message.ContentFull,
                            ContentShort = message.ContentShort
                        };
                    }
                }
            }
            catch (Exception)
            { }
            return cmsMessage;
        }

        public bool IsEnableWheelchairLinkUpdate(Session session)
        {
            return _configuration.GetValue<bool>("EnableWheelchairLinkUpdate") &&
                   session.CatalogItems != null &&
                   session.CatalogItems.Count > 0 &&
                   session.CatalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableWheelchairLinkUpdate).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableWheelchairLinkUpdate).ToString())?.CurrentValue == "1";
        }

        private async Task<List<MOBTravelSpecialNeed>> GetOfferedSpecialRequests(MultiCallResponse flightshoppingReferenceData,
             MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null, Session session = null)
        {
            if (flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialRequestResponses == null || !flightshoppingReferenceData.SpecialRequestResponses.Any()
                || flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests == null || !flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests.Any())
                return null;

            var specialRequests = new List<MOBTravelSpecialNeed>();
            var specialNeedType = TravelSpecialNeedType.SpecialRequest.ToString();
            MOBTravelSpecialNeed createdWheelChairItem = null;
            Func<string, string, string, MOBTravelSpecialNeed> createSpecialNeedItem = (code, value, desc)
                => new MOBTravelSpecialNeed { Code = code, Value = value, DisplayDescription = desc, RegisterServiceDescription = desc, Type = specialNeedType };


            foreach (var specialRequest in flightshoppingReferenceData.SpecialRequestResponses[0].SpecialRequests.Where(x => x.Genre != null && !string.IsNullOrWhiteSpace(x.Genre.Description) && !string.IsNullOrWhiteSpace(x.Code)))
            {
                if (specialRequest.Genre.Description.Equals("General"))
                {
                    var sr = createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description);

                    if (specialRequest.Code.StartsWith("DPNA", StringComparison.OrdinalIgnoreCase)) // add info message for DPNA_1, and DPNA_2 request
                        sr.Messages = new List<MOBItem> { new MOBItem { CurrentValue = _configuration.GetValue<string>("SSR_DPNA_Message") } };

                    SetTaskTrainedServiceAnimalMessage(specialRequest, sr, reservation, selectRequest, selectRequest, session);
                    if (sr.Code != "OTHS")
                        specialRequests.Add(sr);

                }
                else if (specialRequest.Genre.Description.Equals("WheelchairReason"))
                {
                    if (createdWheelChairItem == null)
                    {
                        createdWheelChairItem = createSpecialNeedItem(_configuration.GetValue<string>("SSRWheelChairDescription"), null, _configuration.GetValue<string>("SSRWheelChairDescription"));
                        createdWheelChairItem.SubOptionHeader = _configuration.GetValue<string>("SSR_WheelChairSubOptionHeader");

                        // MOBILE-23726
                        if (IsEnableWheelchairLinkUpdate(session))
                        {
                            var sdlKeyForWheelchairLink = _configuration.GetValue<string>("FSRSpecialTravelNeedsWheelchairLinkKey");
                            MOBMobileCMSContentMessages message = null;
                            if (!string.IsNullOrEmpty(sdlKeyForWheelchairLink))
                            {
                                message = await GetCMSContentMessageByKey(sdlKeyForWheelchairLink, selectRequest, session);
                            }
                            createdWheelChairItem.InformationLink = message?.ContentFull ?? (_configuration.GetValue<string>("WheelchairLinkUpdateFallback") ?? "");
                        }

                        specialRequests.Add(createdWheelChairItem);
                    }

                    var wheelChairSubItem = createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description);

                    if (createdWheelChairItem.SubOptions == null)
                    {
                        createdWheelChairItem.SubOptions = new List<MOBTravelSpecialNeed> { wheelChairSubItem };
                    }
                    else
                    {
                        createdWheelChairItem.SubOptions.Add(wheelChairSubItem);
                    }
                }
                else if (specialRequest.Genre.Description.Equals("WheelchairType"))
                {
                    specialRequests.Add(createSpecialNeedItem(specialRequest.Code, specialRequest.Value, specialRequest.Description));
                }
            }

            return specialRequests;
        }

        private bool IsServiceAnimalEnhancementEnabled(int id, string version, List<MOBItem> catalogItems)
        {
            if (!_configuration.GetValue<bool>("EnableServiceAnimalEnhancements")) return false;
            if (catalogItems != null && catalogItems.Count > 0 &&
                              catalogItems.FirstOrDefault(a => a.Id == ((int)IOSCatalogEnum.EnableTaskTrainedServiceAnimalFeature).ToString() || a.Id == ((int)AndroidCatalogEnum.EnableTaskTrainedServiceAnimalFeature).ToString())?.CurrentValue == "1")
                return GeneralHelper.IsApplicationVersionGreaterorEqual(id, version, _configuration.GetValue<string>("Android_EnableServiceAnimalEnhancements_AppVersion"), _configuration.GetValue<string>("IPhone_EnableServiceAnimalEnhancements_AppVersion"));
            else return false;
        }


        private async Task<List<CMSContentMessage>> GetSDLContentByGroupName(MOBRequest request, string sessionId, string token, string groupName, string docNameConfigEntry, bool useCache = false)
        {
            CSLContentMessagesResponse response = null;

            try
            {
                var getSDL = await _cachingService.GetCache<string>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", request.TransactionId).ConfigureAwait(false);
                if (!string.IsNullOrEmpty(getSDL))
                {
                    response = JsonConvert.DeserializeObject<CSLContentMessagesResponse>(getSDL);
                }
                if (response != null && response.Messages != null) { return response.Messages; }
            }
            catch { }

            MOBCSLContentMessagesRequest sdlReqeust = new MOBCSLContentMessagesRequest
            {
                Lang = "en",
                Pos = "us",
                Channel = "mobileapp",
                Listname = new List<string>(),
                LocationCodes = new List<string>(),
                Groupname = groupName,
                Usecache = useCache
            };

            string jsonRequest = JsonConvert.SerializeObject(sdlReqeust);

            response = await _cMSContentService.GetSDLContentByGroupName<CSLContentMessagesResponse>(token, "message", jsonRequest, sessionId).ConfigureAwait(false);

            if (response == null)
            {
                _logger.LogError("GetSDLContentByGroupName Failed to deserialize CSL response");
                return null;
            }

            if (response.Errors.Count > 0)
            {
                string errorMsg = String.Join(" ", response.Errors.Select(x => x.Message));
                _logger.LogError("GetSDLContentByGroupName {@CSLCallError}", errorMsg);
                return null;
            }

            if (response != null && (Convert.ToBoolean(response.Status) && response.Messages != null))
            {
                if (!_configuration.GetValue<bool>("DisableSDLEmptyTitleFix"))
                {
                    response.Messages = response.Messages.Where(l => l.Title != null).ToList();
                }
                var saveSDL = await _cachingService.SaveCache<CSLContentMessagesResponse>(_configuration.GetValue<string>(docNameConfigEntry) + "MOBCSLContentMessagesResponse", response, request.TransactionId, new TimeSpan(1, 30, 0)).ConfigureAwait(false);

            }

            return response.Messages;
        }


        private async System.Threading.Tasks.Task SetTaskTrainedServiceAnimalMessage(Characteristic specialRequest, MOBTravelSpecialNeed sr, MOBSHOPReservation reservation = null, SelectTripRequest selectRequest = null, MOBRequest request = null, Session session = null)
        {
            if (selectRequest != null && reservation != null && IsServiceAnimalEnhancementEnabled(selectRequest.Application.Id, selectRequest.Application.Version.Major, selectRequest.CatalogItems) && specialRequest.Code.StartsWith(_configuration.GetValue<string>("TasktrainedServiceAnimalCODE"), StringComparison.OrdinalIgnoreCase)) // add info message for Task-trained service animal
            {
                List<CMSContentMessage> lstMessages = await GetSDLContentByGroupName(request, session.SessionId, session.Token,
                                                                                       _configuration.GetValue<string>("CMSContentMessages_GroupName_BookingRTI_Messages"),
                                                                                                                  "BookingPathRTI_CMSContentMessagesCached_StaticGUID").ConfigureAwait(false);


                //First 2 lines only if International , HNL or GUM 
                if (reservation?.ISInternational == true
                    || (!string.IsNullOrWhiteSpace(reservation?.Trips?.FirstOrDefault()?.Origin) && _configuration.GetValue<string>("DisableServiceAnimalAirportCodes")?.Contains(reservation?.Trips?.FirstOrDefault()?.Origin) == true)
                    || (!string.IsNullOrWhiteSpace(reservation?.Trips?.FirstOrDefault()?.Destination) && _configuration.GetValue<string>("DisableServiceAnimalAirportCodes")?.Contains(reservation?.Trips?.FirstOrDefault()?.Destination) == true))
                {
                    sr.IsDisabled = true;
                    sr.Messages = new List<MOBItem> { new MOBItem {
                                Id = "ESAN_SUBTITLE",
                                CurrentValue =GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Content_INT_MOB")  } };
                }
                sr.SubOptions = new List<MOBTravelSpecialNeed>();

                sr.SubOptions.Add(new MOBTravelSpecialNeed
                {
                    Value = sr.Value,
                    Code = "SVAN",
                    Type = TravelSpecialNeedType.ServiceAnimalType.ToString(),
                    DisplayDescription = GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Content2_MOB"),
                    RegisterServiceDescription = GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB")
                });
                if (sr.Messages == null) sr.Messages = new List<MOBItem>();
                ServiceAnimalDetailsScreenMessages(sr.Messages);

                //TODO, ONCE FS changes are ready we do not need below
                sr.Code = "SVAN";
                sr.DisplayDescription = GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB");
                sr.RegisterServiceDescription = GetSDLStringMessageFromList(lstMessages, "TravelNeeds_TaskTrainedDog_Screen_Title_MOB");
            }
        }

        private void ServiceAnimalDetailsScreenMessages(List<MOBItem> messages)
        {

            messages.Add(new MOBItem
            {
                Id = "ESAN_HDR",
                CurrentValue = "Additional step required before traveling with a service dog. \n <br /> <br /> Please complete the service dog request form located in Trip Details before arriving at the airport.",
            });
            messages.Add(new MOBItem
            {
                Id = "ESAN_FTR",
                CurrentValue = "\n<br /><br />We no longer accept emotional support animals due to new Department of Transportation regulations.\n<br /><br /><a href=\"https://www.united.com/ual/en/US/fly/travel/special-needs/disabilities/assistance-animals.html\">Review our service animal policy</a>",
            });
        }

        private string GetSDLStringMessageFromList(List<CMSContentMessage> list, string title)
        {
            return list?.Where(x => x.Title.Equals(title))?.FirstOrDefault()?.ContentFull?.Trim();
        }

        private List<MOBItem> GetSpecialMealsMessages(IEnumerable<ReservationFlightSegment> allSegmentsWithMeals, MultiCallResponse flightshoppingReferenceData)
        {
            Func<List<MOBItem>> GetMealUnavailableMsg = () => new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSRItinerarySpecialMealsNotAvailableMsg"), "") } };

            if (allSegmentsWithMeals == null || !allSegmentsWithMeals.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialMealResponses == null || !flightshoppingReferenceData.SpecialMealResponses.Any()
                || flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals == null || !flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Any())
            {
                return GetMealUnavailableMsg();
            }

            // all meals from reference data
            var allRefMeals = flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Select(x => x.Type.Key);
            if (allRefMeals == null || !allRefMeals.Any())
                return GetMealUnavailableMsg();

            var segmentsHaveMeals = allSegmentsWithMeals.Where(seg => seg != null && seg.FlightSegment != null && seg.FlightSegment.Characteristic != null && seg.FlightSegment.Characteristic.Any()
                                                   && seg.FlightSegment.Characteristic[0] != null
                                                   && seg.FlightSegment.Characteristic.Exists(x => x.Code.Equals("SPML", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(x.Value)))
                                                   .Select(seg => new
                                                   {
                                                       segment = string.Join(" - ", seg.FlightSegment.DepartureAirport.IATACode, seg.FlightSegment.ArrivalAirport.IATACode),
                                                       meals = string.IsNullOrWhiteSpace(seg.FlightSegment.Characteristic[0].Value) ? new HashSet<string>() : new HashSet<string>(seg.FlightSegment.Characteristic[0].Value.Split('|', ' ').Intersect(allRefMeals))
                                                   })
                                                   .Where(seg => seg.meals != null && seg.meals.Any())
                                                   .Select(seg => seg.segment)
                                                   .ToList();

            if (segmentsHaveMeals == null || !segmentsHaveMeals.Any())
            {
                return GetMealUnavailableMsg();
            }

            if (segmentsHaveMeals.Count < allSegmentsWithMeals.Count())
            {
                var segments = segmentsHaveMeals.Count > 1 ? string.Join(", ", segmentsHaveMeals.Take(segmentsHaveMeals.Count - 1)) + " and " + segmentsHaveMeals.Last() : segmentsHaveMeals.First();
                return new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSR_MarketMealRestrictionMessage"), segments) } };
            }

            return null;
        }

        private List<MOBTravelSpecialNeed> GetOfferedMealsForItinerary(IEnumerable<ReservationFlightSegment> allSegmentsWithMeals, MultiCallResponse flightshoppingReferenceData)
        {
            if (allSegmentsWithMeals == null || !allSegmentsWithMeals.Any()
                || flightshoppingReferenceData == null || flightshoppingReferenceData.SpecialMealResponses == null || !flightshoppingReferenceData.SpecialMealResponses.Any()
                || flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals == null || !flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.Any())
                return null;

            Func<IEnumerable<string>, List<MOBItem>> generateMsg = flightSegments =>
            {
                var segments = flightSegments.Count() > 1 ? string.Join(", ", flightSegments.Take(flightSegments.Count() - 1)) + " and " + flightSegments.Last() : flightSegments.First();
                return new List<MOBItem> { new MOBItem { CurrentValue = string.Format(_configuration.GetValue<string>("SSR_MealRestrictionMessage"), segments) } };
            };

            // all meals from reference data
            var allRefMeals = flightshoppingReferenceData.SpecialMealResponses[0].SpecialMeals.ToDictionary(x => x.Type.Key, x => string.Join("^", x.Value[0], x.Description));
            if (allRefMeals == null || !allRefMeals.Any())
                return null;

            // Dictionary whose keys are segments (orig - dest) and values are list of all meals that are available for each segment
            // These contain only the segments that offer meals
            // These meals also need to exist in reference data table 
            var segmentAndMealsMap = allSegmentsWithMeals.Where(seg => seg != null && seg.FlightSegment != null && seg.FlightSegment.Characteristic != null && seg.FlightSegment.Characteristic.Any()
                                                   && seg.FlightSegment.Characteristic[0] != null
                                                   && seg.FlightSegment.Characteristic.Exists(x => x.Code.Equals("SPML", StringComparison.OrdinalIgnoreCase) && !string.IsNullOrWhiteSpace(x.Value))) // get all segments that offer meals
                                                   .Select(seg => new // project them
                                                   {
                                                       segment = string.Join(" - ", seg.FlightSegment.DepartureAirport.IATACode, seg.FlightSegment.ArrivalAirport.IATACode), // IAH - NRT if going from IAH to NRT 
                                                       meals = string.IsNullOrWhiteSpace(seg.FlightSegment.Characteristic[0].Value) ? null : new HashSet<string>(seg.FlightSegment.Characteristic[0].Value.Split('|', ' ').Intersect(allRefMeals.Keys)) // List of all meal codes that offer on the segment
                                                   })
                                                   .Where(segment => segment.meals != null && segment.meals.Any()) // filter out the segments that don't offer meals
                                                   .GroupBy(seg => seg.segment) // handle same market exist twice for MD
                                                   .Select(grp => grp.First()) // handle same market exist twice for MD 
                                                   .ToDictionary(seg => seg.segment, seg => seg.meals); // tranform them to dictionary of segment and meals

            if (segmentAndMealsMap == null || !segmentAndMealsMap.Any())
                return null;

            // Get common meals that offers on all segments after filtering out all segments that don't offer meals
            var mealsThatAvailableOnAllSegments = segmentAndMealsMap.Values.Skip(1)
                                                                            .Aggregate(new HashSet<string>(segmentAndMealsMap.Values.First()), (current, next) => { current.IntersectWith(next); return current; });

            // Filter out the common meals
            if (mealsThatAvailableOnAllSegments != null && mealsThatAvailableOnAllSegments.Any())
            {
                segmentAndMealsMap.Values.ToList().ForEach(x => x.RemoveWhere(item => mealsThatAvailableOnAllSegments.Contains(item)));
            }

            // Add the non-common meals, these will have message
            var results = segmentAndMealsMap.Where(kv => kv.Value != null && kv.Value.Any())
                                .SelectMany(item => item.Value.Select(x => new { mealCode = x, segment = item.Key }))
                                .GroupBy(x => x.mealCode, x => x.segment)
                                .ToDictionary(x => x.Key, x => x.ToList())
                                .Select(kv => new MOBTravelSpecialNeed
                                {
                                    Code = kv.Key,
                                    Value = allRefMeals[kv.Key].Split('^')[0],
                                    DisplayDescription = allRefMeals[kv.Key].Split('^')[1],
                                    RegisterServiceDescription = allRefMeals[kv.Key].Split('^')[1],
                                    Type = TravelSpecialNeedType.SpecialMeal.ToString(),
                                    Messages = mealsThatAvailableOnAllSegments.Any() ? generateMsg(kv.Value) : null
                                })
                                .ToList();

            // Add the common meals, these don't have messages
            if (mealsThatAvailableOnAllSegments.Any())
            {
                results.AddRange(mealsThatAvailableOnAllSegments.Select(m => new MOBTravelSpecialNeed
                {
                    Code = m,
                    Value = allRefMeals[m].Split('^')[0],
                    DisplayDescription = allRefMeals[m].Split('^')[1],
                    RegisterServiceDescription = allRefMeals[m].Split('^')[1],
                    Type = TravelSpecialNeedType.SpecialMeal.ToString()
                }));
            }

            return results == null || !results.Any() ? null : results; // return null if empty
        }

        private IEnumerable<ReservationFlightSegment> PopulateSegmentsWithDefaultMeals(IEnumerable<ReservationFlightSegment> segments)
        {
            var pnrOfferedMeals = GetOfferedMealsForItineraryFromPNRManagementRequest(segments);
            pnrOfferedMeals.Where(x => x.FlightSegment != null && x.FlightSegment.IsInternational.Equals("True", StringComparison.OrdinalIgnoreCase))
                           .ToList()
                           .ForEach(x => x.FlightSegment.Characteristic = new Collection<Service.Presentation.CommonModel.Characteristic> { new Service.Presentation.CommonModel.Characteristic {
                                       Code = "SPML",
                                       Description = "Default meals when service is down",
                                       Value = _configuration.GetValue<string>("SSR_DefaultMealCodes")
                                   } });

            return pnrOfferedMeals;
        }

        private MultiCallResponse GetMultiCallResponseWithDefaultSpecialRequests()
        {
            try
            {
                return new MultiCallResponse
                {
                    SpecialRequestResponses = new Collection<SpecialRequestResponse>
                    {
                        new SpecialRequestResponse
                        {
                            SpecialRequests = new Collection<Service.Presentation.CommonModel.Characteristic> (_configuration.GetValue<string>("SSR_DefaultSpecialRequests")
                                                                                                .Split('|')
                                                                                                .Select(request => request.Split('^'))
                                                                                                .Select(request => new Service.Presentation.CommonModel.Characteristic
                                                                                                {
                                                                                                    Code = request[0],
                                                                                                    Description = request[1],
                                                                                                    Genre = new Service.Presentation.CommonModel.Genre { Description = request[2]},
                                                                                                    Value = request[3]
                                                                                                })
                                                                                                .ToList())
                        }
                    }
                };
            }
            catch
            {
                return null;
            }
        }

        private async Task<IEnumerable<ReservationFlightSegment>> GetOfferedMealsForItineraryFromPNRManagement(Session session, int appId, string appVersion, string deviceId, IEnumerable<ReservationFlightSegment> segments)
        {
            string cslActionName = "/SpecialMeals/FlightSegments";

            string jsonRequest = JsonConvert.SerializeObject(GetOfferedMealsForItineraryFromPNRManagementRequest(segments));

            string token = await _dPService.GetAnonymousToken(appId, deviceId, _configuration).ConfigureAwait(false);

            var cslCallDurationstopwatch = new Stopwatch();
            cslCallDurationstopwatch.Start();

            var response = await _pNRRetrievalService.GetOfferedMealsForItinerary<List<ReservationFlightSegment>>(token, cslActionName, jsonRequest, session.SessionId).ConfigureAwait(false);

            if (cslCallDurationstopwatch.IsRunning)
            {
                cslCallDurationstopwatch.Stop();
            }

            if (response != null)
            {
                if (response == null)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                if (response.Count > 0)
                {
                    await _sessionHelperService.SaveSession<List<ReservationFlightSegment>>(response, session.SessionId, new List<string> { session.SessionId, new ReservationFlightSegment().GetType().FullName }, new ReservationFlightSegment().GetType().FullName).ConfigureAwait(false);
                }

                return response;
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }

        private IEnumerable<ReservationFlightSegment> GetOfferedMealsForItineraryFromPNRManagementRequest(IEnumerable<ReservationFlightSegment> segments)
        {
            if (segments == null || !segments.Any())
                return new List<ReservationFlightSegment>();

            return segments.Select(segment => new ReservationFlightSegment
            {
                FlightSegment = new Service.Presentation.SegmentModel.FlightSegment
                {
                    ArrivalAirport = new Service.Presentation.CommonModel.AirportModel.Airport { IATACode = segment.FlightSegment.ArrivalAirport.IATACode },
                    DepartureAirport = new Service.Presentation.CommonModel.AirportModel.Airport { IATACode = segment.FlightSegment.DepartureAirport.IATACode },
                    DepartureDateTime = segment.FlightSegment.DepartureDateTime,
                    FlightNumber = segment.FlightSegment.FlightNumber,
                    InstantUpgradable = false,
                    IsInternational = segment.FlightSegment.IsInternational,
                    OperatingAirlineCode = segment.FlightSegment.OperatingAirlineCode,
                    UpgradeEligibilityStatus = Service.Presentation.CommonEnumModel.UpgradeEligibilityStatus.Unknown,
                    UpgradeVisibilityType = Service.Presentation.CommonEnumModel.UpgradeVisibilityType.None,
                    BookingClasses = new Collection<BookingClass>(segment.FlightSegment.BookingClasses.Where(y => y != null && y.Cabin != null).Select(y => new BookingClass { Cabin = new Service.Presentation.CommonModel.AircraftModel.Cabin { Name = y.Cabin.Name }, Code = y.Code }).ToList())
                }
            }).ToList();
        }

        private async Task<MultiCallResponse> GetSpecialNeedsReferenceDataFromFlightShopping(Session session, int appId, string appVersion, string deviceId, string languageCode)
        {
            string cslActionName = "MultiCall";

            string jsonRequest = JsonConvert.SerializeObject(GetFlightShoppingMulticallRequest(languageCode));

            string token = await _dPService.GetAnonymousToken(appId, deviceId, _configuration).ConfigureAwait(false);

            var cslCallDurationstopwatch = new Stopwatch();
            cslCallDurationstopwatch.Start();

            var response = await _referencedataService.GetSpecialNeedsInfo<MultiCallResponse>(cslActionName, jsonRequest, token, session.SessionId).ConfigureAwait(false);

            if (cslCallDurationstopwatch.IsRunning)
            {
                cslCallDurationstopwatch.Stop();
            }

            if (response != null)
            {
                if (response == null || response.SpecialRequestResponses == null || response.ServiceAnimalResponses == null || response.SpecialMealResponses == null || response.SpecialRequestResponses == null)
                    throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));

                return response;
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
        }

        private MultiCallRequest GetFlightShoppingMulticallRequest(string languageCode)
        {
            var request = new MultiCallRequest
            {
                ServiceAnimalRequests = new Collection<ServiceAnimalRequest> { new ServiceAnimalRequest { LanguageCode = languageCode } },
                ServiceAnimalTypeRequests = new Collection<ServiceAnimalTypeRequest> { new ServiceAnimalTypeRequest { LanguageCode = languageCode } },
                SpecialMealRequests = new Collection<SpecialMealRequest> { new SpecialMealRequest { LanguageCode = languageCode } },
                SpecialRequestRequests = new Collection<SpecialRequestRequest> { new SpecialRequestRequest { LanguageCode = languageCode/*, Channel = _configuration.GetValue<string>("Shopping - ChannelType")*/ } },
            };

            return request;
        }

        private void SupressWhenScheduleChange(MOBPNRByRecordLocatorResponse response)
        {
            if (_configuration.GetValue<bool>("EnableSupressWhenScheduleChange") && response.PNR.HasScheduleChanged)
            {
                try
                {
                    response.ShowSeatChange
                        = response.ShowSeatChange ? false : response.ShowSeatChange;

                    response.PNR.IsEnableEditTraveler
                        = response.PNR.IsEnableEditTraveler ? false : response.PNR.IsEnableEditTraveler;

                    response.PNR.ShouldDisplayEmailReceipt = false;

                    response.PNR.ShouldDisplayUpgradeCabin = false;

                    response.PNR.SupressLMX = true;

                    if (response.PNR.AdvisoryInfo != null && response.PNR.AdvisoryInfo.Any())
                    {
                        //ContentType.SCHEDULECHANGE
                        response.PNR.AdvisoryInfo.RemoveAll(item => item.ContentType != ContentType.SCHEDULECHANGE);
                    }

                    response.ShowAddCalendar = false;
                    response.ShowBaggageInfo = false;
                    response.PNR.HasCheckedBags = false;
                }
                catch { }
            }
        }

        private void SetMerchandizeChannelValues(string merchChannel, ref string channelId, ref string channelName)
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

        private async System.Threading.Tasks.Task CheckPNRForOTFEligiblity(MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response, Session session)
        {
            try
            {
                if (!_configuration.GetValue<bool>("ExcludePNRForOTFEligiblity")
                    && (await _featureSettings.GetFeatureSettingValue("EnableIsChangeEligible").ConfigureAwait(false) ? response.PNR.IsChangeEligible : response.PNR.IsATREEligible) && !response.PNR.AwardTravel
                    && await CheckPNRForOTFEligiblity1(request, response, session))
                {
                    if (response?.PNR?.Futureflightcredit != null
                        && response?.PNR?.Futureflightcredit?.Messages != null
                        && response.PNR.Futureflightcredit.Messages.Any())
                    {
                        var includeffcOTFmessage = response.PNR.Futureflightcredit.Messages.FirstOrDefault
                                   (x => string.Equals(x.Id, "FFC_AGENCY_ALRTMSG", StringComparison.OrdinalIgnoreCase));

                        if (includeffcOTFmessage == null)
                        {
                            response.PNR.Futureflightcredit.Messages.Add(new MOBItem
                            {
                                Id = "FFC_AGENCY_ALRTMSG",
                                CurrentValue = _configuration.GetValue<string>("OTFFFC_ELIGIBILITY_ALRTMSG")
                            });
                        }
                    }
                }
            }
            catch { }
        }

        private async Task<bool> CheckPNRForOTFEligiblity1(MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response,
          Session session)
        {
            var cslRequest = new OnTheFlyEligibilityRequest
            {
                Channel = request.Application.Id,
                LastNameBypass = true,
                RecordLocator = request.RecordLocator,
                LastName = request.LastName,
                ReservationBypass = true
            };

            var cslStrResponse = await PNRForOTFEligiblity_Csl(request, cslRequest.ToJsonString(), session);

            if (!string.IsNullOrEmpty(cslStrResponse))
            {
                var cslResponse = JsonConvert.DeserializeObject<OnTheFlyEligibility>(cslStrResponse);
                if (cslResponse != null && cslResponse.OfferEligible)
                {
                    return true;
                }
            }
            return false;
        }

        private async Task<string> PNRForOTFEligiblity_Csl
           (MOBPNRByRecordLocatorRequest mobrequest, string jsonRequest, Session session)
        {
            try
            {
                string path = "/OnTheFly/OfferEligible";

                var cslstrResponse = await _refundService.PostEligibleCheck<string>(session.Token, session.SessionId, path, jsonRequest);

                return cslstrResponse;
            }
            catch (Exception exc)
            {

                _logger.LogError("ValidateIRROPSStatus CSL Exception{exception}", JsonConvert.SerializeObject(exc));
            }

            return string.Empty;
        }

        private async System.Threading.Tasks.Task AddAncillaryToPnrResponse(MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response, Session session, ReservationDetail cslReservationDetail)
        {
            if (response.PNR == null || response.Exception != null || cslReservationDetail == null || session == null) return;

            if (response.PNR.IsCanceledWithFutureFlightCredit)
            {
                await CheckPNRForOTFEligiblity(request, response, session);
                return;
            }

            if (_configuration.GetValue<bool>("EnableSupressWhenScheduleChange") && response.PNR.HasScheduleChanged) return;

            request.RecordLocator = response.PNR.RecordLocator; //To fix fuzzi pnr
            string channelId = string.Empty;
            string channelName = string.Empty;
            if (_configuration.GetValue<bool>("EnabledMERCHChannels"))
            {
                string merchChannel = "MOBMYRES";
                SetMerchandizeChannelValues(merchChannel, ref channelId, ref channelName);
            }

            if (request.SessionId != null)
            {
                //Emptying the existing Shopping Cart
                MOBShoppingCart persistShoppingCart = new MOBShoppingCart();
                await _sessionHelperService.SaveSession<MOBShoppingCart>(persistShoppingCart, request.SessionId, new List<string> { request.SessionId, persistShoppingCart.ObjectName }, persistShoppingCart.ObjectName).ConfigureAwait(false);

                GetOffers productOffer = new GetOffers();
                await _sessionHelperService.SaveSession<GetOffers>(productOffer, request.SessionId, new List<string> { request.SessionId, productOffer.ObjectName }, productOffer.ObjectName).ConfigureAwait(false);

                GetOffersCce productOfferCce = new GetOffersCce();
                await _sessionHelperService.SaveSession<GetOffersCce>(productOfferCce, request.SessionId, new List<string> { request.SessionId, productOfferCce.ObjectName }, productOfferCce.ObjectName).ConfigureAwait(false);

                GetVendorOffers productVendorOffer = new GetVendorOffers();
                await _sessionHelperService.SaveSession<GetVendorOffers>(productVendorOffer, request.SessionId, new List<string> { request.SessionId, productVendorOffer.ObjectName }, productVendorOffer.ObjectName).ConfigureAwait(false);
            }
            response.Ancillary = new MOBAncillary();
            SeatOffer seatOffer = null;
            MOBBundleInfo bundleInfo = null;
            DynamicOfferResponse doResponse = null;
            bool isEnableTravelOptionsInViewRes = _manageResUtility.IsEnableTravelOptionsInViewRes(request.Application.Id, request?.Application?.Version?.Major, session?.CatalogItems);

            try
            {
                System.Threading.Tasks.Task[] taskArray = new System.Threading.Tasks.Task[]
                {
                System.Threading.Tasks.Task.Factory.StartNew(()=>
                {
                     if (_configuration.GetValue<bool>("EnableShareResDetailAppWidget"))
                     {
                        MOBPNR pnr = response.PNR;
                        string url = GetTripDetailRedirect3dot0Url(response.RecordLocator, response.LastName, ac: "", channel: "mobile", languagecode: "en/US");
                        _manageResUtility.GetShareReservationInfo(response,cslReservationDetail,url);
                    }
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {

                           if(isEnableTravelOptionsInViewRes)
                             {
                                doResponse  = await _merchandizingServices.GetProductOffersFromCCE(session, cslReservationDetail.Detail, request, request.Flow, response.PNR);
                             }
                            if(!isEnableTravelOptionsInViewRes)
                             {
                                var productOffers = await _merchandizingServices.GetMerchOffersDetails(session, cslReservationDetail.Detail, request, request.Flow, response.PNR);
                                response.Ancillary.PremiumCabinUpgrade = await _merchandizingServices.GetPremiumCabinUpgrade_CFOP(productOffers, request, session.SessionId, cslReservationDetail.Detail); // need to call this to clear state even when productOffers is null.
                                response.Ancillary.AwardAccelerators = (_configuration.GetValue<bool>("SuppressAwardAcceleratorForBE") && (response.PNR.isELF || response.PNR.IsIBE)) ? null : await _merchandizingServices.GetMileageAndStatusOptions(productOffers, request, session.SessionId);

                                #region PremierAccess and PriorityBoarding
                                MOBPremierAccess premierAccess = new MOBPremierAccess();
                                MOBPriorityBoarding priorityBoarding = new MOBPriorityBoarding();
                                var showPremierAccess = false;
                                var tupleRes= await GetPremierAccessAndPriorityBoarding(request, session, cslReservationDetail, productOffers, priorityBoarding,  premierAccess,  showPremierAccess);
                                priorityBoarding=tupleRes.priorityBoarding;
                                response.PremierAccess = tupleRes.premierAccess;
                                response.ShowPremierAccess = tupleRes.showPremierAccess;
                                response.Ancillary.PriorityBoarding = priorityBoarding;
                                #endregion
                    }
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                    response.TripInsuranceInfo = await _merchandizingServices.GetTPIINfoDetails_CFOP(response.PNR.IsTPIIncluded,response.PNR.IsFareLockOrNRSA, request, session);
                    if(isEnableTravelOptionsInViewRes)
                    {
                         await _sessionHelperService.SaveSession<Mobile.Model.MPSignIn.MOBTPIInfo>(response.TripInsuranceInfo, request.SessionId, new List<string> { request.SessionId, ObjectNames.TPIResponse }, ObjectNames.TPIResponse).ConfigureAwait(false);
                    }
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                    if(request.Flow != FlowType.VIEWRES_SEATMAP.ToString())
                    {
                        var productOffersFromCce = await _merchandizingServices.GetMerchOffersDetailsFromCCE(session, cslReservationDetail.Detail, request, request.Flow, response.PNR);

                        var taskArrayMerchOffers = new System.Threading.Tasks.Task[]
                        {
                            System.Threading.Tasks.Task.Factory.StartNew(() => {
                                response.Ancillary.TravelOptionsBundle = _merchandizingServices.TravelOptionsBundleOffer(productOffersFromCce, request, session.SessionId);
                            }),
                            System.Threading.Tasks.Task.Factory.StartNew(async() => {
                                response.Ancillary.BasicEconomyBuyOut = await _merchandizingServices.BasicEconomyBuyOutOffer(productOffersFromCce, request, session.SessionId,response.PNR, session.CatalogItems);
                                if (response.Ancillary?.BasicEconomyBuyOut?.ElfRestrictionsBeBuyOutLink != null && (response?.PNR?.ELFLimitations?.Any() ?? false))
                                {
                                    response?.PNR?.ELFLimitations.Add(response.Ancillary.BasicEconomyBuyOut.ElfRestrictionsBeBuyOutLink);
                                }
                            })
                        };

                        System.Threading.Tasks.Task.WaitAll(taskArrayMerchOffers);
                    }
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                    try
                    {
                        if (request.Flow != FlowType.VIEWRES_SEATMAP.ToString() && _configuration.GetValue<string>("ShowViewReservationDOTBaggaeInfo") != null &&
                            _configuration.GetValue<string>("ShowViewReservationDOTBaggaeInfo").ToUpper().Trim() == "TRUE"
                            && !FeatureVersionCheck(request.Application.Id, request.Application.Version.Major, "EnablePrePaidBags", "AndroidPrePaidBagsVersion", "iPhonePrePaidBagsVersion"))
                        {
                            DOTBaggageInfoResponse dotBaggageInfoResponse = await _merchandizingServices.GetDOTBaggageInfoWithPNR(request.AccessCode, request.SessionId, request.LanguageCode, "XML",request.Application.Id, request.RecordLocator, "01/01/0001", channelId, channelName, null, cslReservationDetail.Detail);

                            response.DotBaggageInformation = new DOTBaggageInfo();
                            response.DotBaggageInformation.SetMerchandizingServicesBaggageInfo(dotBaggageInfoResponse.DotBaggageInfo);
                            if (response.DotBaggageInformation.ErrorMessage.IsNullOrEmpty())
                            {
                                var dotBaggageInformation = await GetBaggageInfo(response.PNR, request.TransactionId);
                                response.DotBaggageInformation.SetDatabaseBaggageInfo(dotBaggageInformation);
                            }

                        }
                    }
                    catch (System.Exception ex)
                    {

                            MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);

                        response.DotBaggageInformation = new DOTBaggageInfo();
                        response.DotBaggageInformation.ErrorMessage = _configuration.GetValue<string>("DOTBaggageGenericExceptionMessage");
                    }
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                  if(!isEnableTravelOptionsInViewRes)
                    seatOffer = await _flightReservation.GetSeatOffer_CFOP(response.PNR, request, cslReservationDetail.Detail, session.Token, request.Flow, session.SessionId, session).ConfigureAwait(false);
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                    MOBPremierAccess premierAccess = new MOBPremierAccess();
                    MOBPriorityBoarding priorityBoarding = new MOBPriorityBoarding();
                    var tupleRes = await GetBundleInfo(request, channelId, channelName);
                    premierAccess = tupleRes.premierAccess;
                    priorityBoarding = tupleRes.priorityBoarding;
                    bundleInfo = tupleRes.bundleInfo;

                    if(isEnableTravelOptionsInViewRes)
                    {
                        response.PremierAccess = premierAccess !=null && premierAccess.Segments != null && premierAccess.Segments.Count > 0 ? premierAccess : null;
                        response.Ancillary.PriorityBoarding = priorityBoarding !=null && priorityBoarding.Segments != null && priorityBoarding.Segments.Count > 0 ? priorityBoarding : null;
                    }

                }),
                // System.Threading.Tasks.Task.Factory.StartNew(() =>
                //{
                //     ValidateIRROPSStatus(request,response,cslReservationDetail, session);
                //}),
               await  System.Threading.Tasks.Task.Factory.StartNew(async()=>
                { //sandeep placepass changes
                     response.Ancillary.PlacePass = await GetPlacePass(request, response.PNR);
                }),
                await System.Threading.Tasks.Task.Factory.StartNew(async()=> {
                     try
                    {
                        if (_configuration.GetValue<bool>("EnableMgnResUpdateTravelerInfo") && request.Flow != FlowType.VIEWRES_SEATMAP.ToString())
                        {
                            response.RewardPrograms = await GetAllRewardProgramItems
                            (request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, session.SessionId, session.Token);
                        }
                    }
                    catch (Exception ex)
                    {
                       _logger.LogError("GetPNRByRecordLocator_CFOP- GetAllRewardProgramItems Error{error}", JsonConvert.SerializeObject(ex));

                    }
                }),
                  await System.Threading.Tasks.Task.Factory.StartNew(async() =>
                {
                    #region SAF changes
                     try
                     {
                      if(_manageResUtility.IsEnableSAFInViewRes(request.Application.Id,request.Application.Version.Major,request.CatalogValues))
                      {
                        var productOffers = await _merchandizingServices.GetMerchOffersDetails(session, cslReservationDetail.Detail, request, request.Flow, response.PNR,"SFC");

                       MOBSustainableAviationFuel saf=new MOBSustainableAviationFuel();
                       await AssignSAFContent(saf,session,productOffers).ConfigureAwait(false);
                       response.SustainableAviationFuel= _merchandizingServices.GetSAFPriceDetails(productOffers,saf);
                      }
                     }
                     catch (Exception ex)
                     {
                       _logger.LogError("GetPNRByRecordLocator_CFOP- GetSAF{error}", JsonConvert.SerializeObject(ex));
                     }
                    #endregion
                }),
                System.Threading.Tasks.Task.Factory.StartNew(()=>
                {
                     if (_configuration.GetValue<bool>("EnableAssociateMPNumber"))
                     {
                        MOBPNR pnr = response.PNR;
                        if (AssociateMPIDEligibilityCheck(pnr, request.MileagePlusNumber))
                        {
                                SetAssociateMPNumberDetails(response.PNR, request.MileagePlusNumber, request);
                        }
                    }
                }),
                };

                //Block until all tasks complete.
                await System.Threading.Tasks.Task.WhenAll(taskArray);

                await System.Threading.Tasks.Task.Factory.StartNew(() =>
                 {
                     if (_configuration.GetValue<bool>("EnableAssociateMPNumberSilentLogging"))
                     {
                         try
                         {
                             MOBPNR pnr = response.PNR;
                             if (AssociateMPIDEligibilityCheck(pnr, request.MileagePlusNumber))
                             {
                                 string pnrPaxCount = (pnr.Passengers != null && pnr.Passengers.Count() > 0) ? Convert.ToString(pnr.Passengers.Count()) : "0";
                                 string logStatement = string.Format("MP={0}, Lastname={1}, PNR={2},PaxInPNR={3}", request.MileagePlusNumber, request.LastName, response.PNR.RecordLocator, pnrPaxCount);
                                 SaveLogToTempAnalysisTable(logStatement, request.SessionId);
                             }
                         }
                         catch (Exception) { }
                     }
                 });

                //ValidateIRROPStatus
                await ValidateIRROPSStatus(request, response, cslReservationDetail, session);
            }

            catch (Exception ex)
            {
                _logger.LogError("Task Factory Error{error}", JsonConvert.SerializeObject(ex));
            }

            try
            {
                if (isEnableTravelOptionsInViewRes)
                {
                    response.TravelOptions = await GetTravelOptions(doResponse, response.TripInsuranceInfo, session.SessionId).ConfigureAwait(false);
                }
            }
            catch (Exception ex) { _logger.LogError("GetPNRByRecordLocator_CFOP- TravelOptions Error{error}", JsonConvert.SerializeObject(ex)); }

            #region
            response.PNR.SeatOffer = seatOffer;
            response.RecordLocator = response.PNR.RecordLocator;
            response.UARecordLocator = response.PNR.UARecordLocator;
            response.PNR.IsEnableEditTraveler = (!response.PNR.isgroup);
            response.ShowSeatChange = ShowSeatChangeButton(response.PNR);
            UpdatePnrWithBundleInfo(request, bundleInfo, ref response);
            #endregion
        }

        private async Task<TravelOptions> GetTravelOptions(DynamicOfferResponse doResponse, Mobile.Model.MPSignIn.MOBTPIInfo tripInsuranceInfo, string sessionId)
        {
            TravelOptions response = null;
            if (doResponse != null)
            {
                response = await GetMROffersResponse(doResponse, tripInsuranceInfo, sessionId).ConfigureAwait(false);
            }

            ProductTile tpiTile = GetTPITile(tripInsuranceInfo);
            if (tpiTile != null && tpiTile.Amount > 0)
            {
                if (response == null)
                    response = new TravelOptions();

                if (string.IsNullOrEmpty(response?.Title))
                    response.Title = _configuration.GetValue<string>("MRTravelOptionsTitle");

                if (response.ProductTiles != null && response.ProductTiles.Count > 0)
                    response.ProductTiles.Add(tpiTile);
                else
                {
                    response.ProductTiles = new List<ProductTile>();
                    response.ProductTiles.Add(tpiTile);
                }

            }

            return response;
        }

        private async Task<TravelOptions> GetMROffersResponse(DynamicOfferResponse doResponse, Mobile.Model.MPSignIn.MOBTPIInfo tripInsuranceInfo, string sessionId)
        {
            TravelOptions travelOption = new TravelOptions();
            ResponseData responseData = null;
            if (doResponse != null && doResponse.Error == null && doResponse.ResponseData != null)
            {
                responseData = doResponse.ResponseData.ToObject<ResponseData>();
            }

            if (responseData != null && responseData.Contents != null && responseData.Contents.Count > 0)
            {
                var details = responseData.Contents?.FirstOrDefault()?.Details;
                travelOption.Title = _configuration.GetValue<string>("MRTravelOptionsTitle");
                travelOption.SeeAllOffersLinkText = details.FirstOrDefault(x => x != null && x.Message != null && x.Message.Equals(_configuration.GetValue<string>("MRTravelOptionsFooter"), StringComparison.OrdinalIgnoreCase))?.Message;
                travelOption.CorrelationId = doResponse.CorrelationId;
                List<ProductTile> productTiles = new List<ProductTile>();

                foreach (var productDetail in details)
                {
                    ProductTile productTile = await GetProductTileFromDetail(productDetail, doResponse.GUIDs, sessionId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(productTile.ProductCode) && productTile.Amount > 0 && !string.IsNullOrEmpty(productTile.ProductName) && !string.IsNullOrEmpty(productTile.ProductCode))
                        productTiles.Add(productTile);
                }

                if (productTiles?.Count > 0)
                {
                    travelOption.ProductTiles = productTiles;
                }
                else
                    travelOption = new TravelOptions();
            }
            return travelOption;
        }

        private async Task<ProductTile> GetProductTileFromDetail(Detail productDetail, Collection<UniqueIdentifier> guid, string sessionId)
        {
            ProductTile productTile = new ProductTile();
            if (productDetail != null && productDetail.SubDetails != null && productDetail.Code != "PAC")
            {
                double amount = GetProductAmount(productDetail);

                productTile.ProductTitle = productDetail.Name;
                productTile.ProductCode = productDetail.Code;
                productTile.PriceText = "$" + amount.ToString();

                var priceSubText = !string.IsNullOrEmpty(productDetail.Message) ? productDetail.Message?.Split('/')[1] : string.Empty;
                productTile.PriceSubText = !string.IsNullOrEmpty(priceSubText) ? "/" + priceSubText.Trim() : _configuration.GetValue<string>("MRTravelOptionsPriceSubText");

                productTile.Amount = Convert.ToDecimal(amount);
                if (!string.IsNullOrEmpty(productDetail?.Caption))
                {
                    string caption = productDetail.Caption.ToUpper() + "BUNDLE";
                    productTile.BadgeText = GetTravelOptionBadge(caption);
                    productTile.BadgeBackgroundColor = GetTravelOptionBadgeBackgroundColor(caption);
                    productTile.BadgeFontColor = GetTravelOptionBadgeFontColor(caption);
                }
                productTile.showUpliftPerMonthPrice = amount >= MinimumPriceForUplift && (_configuration.GetValue<string>("EligibleProductsForUpliftInViewRes")?.Split(',')?.Contains(productDetail?.Code) ?? false);
                string isBundleProductText = _manageResUtility.GetCharactersticValue(productDetail?.Characteristics, "IsBundlePrdct");
                bool isBundleProduct = !string.IsNullOrEmpty(isBundleProductText) ? Convert.ToBoolean(isBundleProductText) : false;

                if (!isBundleProduct)
                {
                    var fromText = !string.IsNullOrEmpty(productDetail.Message) ? productDetail.Message?.Split('$')[0] : string.Empty;
                    productTile.FromText = !string.IsNullOrEmpty(fromText) ? fromText.Trim() : _configuration.GetValue<string>("MRTravelOptionsFromText");
                }

                productTile.ProductName = GetProductNameForOfferCode(productDetail.Code, isBundleProduct);
                productTile.ProductDetails = GetOfferTiles(productDetail, isBundleProduct);
                if (productDetail?.Code == ProductName.PCU.ToString())
                    productTile.AccessibilityText = _configuration.GetValue<string>("MRTravelOptionsPCUAccessbilityText");


                if ((await IsEnableCCEFeedBackCallForEplusTile().ConfigureAwait(false)) && productDetail.Code.Equals("E01", StringComparison.OrdinalIgnoreCase))
                {
                    await _sessionHelperService.SaveSession(guid, sessionId, new List<string> { sessionId, ObjectNames.CCEDOResponseGuidSession }, ObjectNames.CCEDOResponseGuidSession).ConfigureAwait(false);
                }
            }
            return productTile;
        }

        public async Task<bool> IsEnableCCEFeedBackCallForEplusTile()
        {
            if (_configuration.GetValue<bool>("EnableFeatureSettingsChanges"))
                return await _featureSettings.GetFeatureSettingValue("IsEnableCCEFeedBackCallForEplusTileMR").ConfigureAwait(false);

            return false;
        }
        
        private ProductTile GetTPITile(Mobile.Model.MPSignIn.MOBTPIInfo tripInsuranceInfo)
        {
            ProductTile productTile = null;
            if (tripInsuranceInfo != null && !string.IsNullOrEmpty(tripInsuranceInfo.ProductCode) && tripInsuranceInfo.Amount > 0)
            {
                productTile = new ProductTile()
                {
                    ProductTitle = tripInsuranceInfo.OfferTitleV2,
                    ProductCode = tripInsuranceInfo.ProductCode,
                    PriceText = tripInsuranceInfo.DisplayAmount,
                    Amount = Convert.ToDecimal(tripInsuranceInfo.Amount),
                    ProductName = ProductName.TPI.ToString(),
                    ProductDetails = new List<ProductComponent>()
                        {
                            new ProductComponent()
                            {
                                  Title = tripInsuranceInfo.OfferHeaderV2,
                                  Code = tripInsuranceInfo.ProductCode,
                            }
                        }
                };
            }
            return productTile;
        }

        private double GetProductAmount(Detail productDetail)
        {
            double amount = 0;
            try
            {
                if (productDetail != null)
                {
                    if (!string.IsNullOrEmpty(productDetail.Message))
                    {
                        string message = productDetail.Message?.Split('/')[0];
                        amount = Convert.ToDouble(message?.Split('$')[1]);
                    }
                    else if (productDetail.StartingFrom != null && productDetail.StartingFrom.Count > 0)
                    {
                        amount = productDetail.StartingFrom.FirstOrDefault().Amount;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return amount;
        }

        private List<ProductComponent> GetOfferTiles(Detail productDetail, bool isBundleProduct)
        {
            List<ProductComponent> tiles = new List<ProductComponent>();
            if (productDetail?.SubDetails != null && productDetail?.SubDetails.Count > 0)
            {
                foreach (var subDetail in productDetail.SubDetails)
                {
                    if (subDetail != null && subDetail.Genre != null)
                    {
                        ProductComponent tile = new ProductComponent()
                        {
                            Title = !isBundleProduct ? subDetail.Description : subDetail.Genre.Value,
                            Code = subDetail.Genre.Key,
                            Icon = subDetail.Genre.Description,
                            Description = subDetail.Description
                        };
                        tiles.Add(tile);
                    }
                }
            }
            return tiles;
        }

        private string GetProductNameForOfferCode(string code, bool isBundleProduct)
        {
            string productName = string.Empty;
            if (isBundleProduct)
                return ProductName.BUNDLE.ToString();
            if (!string.IsNullOrEmpty(code) && (code.ToUpper().Equals("AAC") || code.ToUpper().Equals("PAC")))
                return ProductName.APA.ToString();
            if (!string.IsNullOrEmpty(code) && (code.ToUpper().Equals("E01")))
                return ProductName.SEATS.ToString();
            if (!string.IsNullOrEmpty(code))
            {
                switch (code)
                {
                    case string str when code.Equals(ProductName.PCU.ToString(), StringComparison.OrdinalIgnoreCase):
                        productName = ProductName.PCU.ToString();
                        break;
                    case string str when code.Equals(ProductName.PAS.ToString(), StringComparison.OrdinalIgnoreCase):
                        productName = ProductName.PAS.ToString();
                        break;
                    case string str when code.Equals(ProductName.PBS.ToString(), StringComparison.OrdinalIgnoreCase):
                        productName = ProductName.PBS.ToString();
                        break;
                    case string str when code.Equals(ProductName.APA.ToString(), StringComparison.OrdinalIgnoreCase): // Revisit this code to check/change the product code
                        productName = ProductName.APA.ToString();
                        break;
                    case string str when code.Equals(ProductName.SEATS.ToString(), StringComparison.OrdinalIgnoreCase): // Revisit this code to check/change the product code
                        productName = ProductName.SEATS.ToString();
                        break;
                    case string str when code.Equals(ProductName.TPI.ToString(), StringComparison.OrdinalIgnoreCase):
                        productName = ProductName.TPI.ToString();
                        break;
                }
            }
            return productName;
        }
        private int MinimumPriceForUplift
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
        private string GetTravelOptionBadgeBackgroundColor(string caption)
        {
            string color = string.Empty;
            if (!string.IsNullOrEmpty(caption))
            {
                switch (caption)
                {
                    case string str when TravelOptionBadge.MOSTPOPULARBUNDLE.ToString().Equals(caption, StringComparison.OrdinalIgnoreCase):
                        color = TravelOptionBadgeBackgroundColor.BLUE.GetDescription();
                        break;
                    case string str when caption.ToUpper().Contains(TravelOptionBadge.PARTIAL.ToString()):
                        color = TravelOptionBadgeBackgroundColor.GREY.GetDescription();
                        break;
                }
            }
            return color;
        }

        private string GetTravelOptionBadgeFontColor(string caption)
        {
            string color = string.Empty;
            if (!string.IsNullOrEmpty(caption))
            {
                switch (caption)
                {
                    case string str when TravelOptionBadge.MOSTPOPULARBUNDLE.ToString().Equals(caption, StringComparison.OrdinalIgnoreCase):
                        color = TravelOptionBadgeFontColor.WHITE.GetDescription();
                        break;
                    case string str when caption.ToUpper().Contains(TravelOptionBadge.PARTIAL.ToString()):
                        color = TravelOptionBadgeFontColor.BLACK.GetDescription();
                        break;
                }
            }
            return color;
        }

        private string GetTravelOptionBadge(string caption)
        {
            string color = string.Empty;
            if (!string.IsNullOrEmpty(caption))
            {
                switch (caption)
                {
                    case string str when TravelOptionBadge.MOSTPOPULARBUNDLE.ToString().Equals(caption, StringComparison.OrdinalIgnoreCase):
                        color = TravelOptionBadge.MOSTPOPULARBUNDLE.GetDescription();
                        break;
                    case string str when caption.ToUpper().Contains(TravelOptionBadge.PARTIAL.ToString()):
                        color = TravelOptionBadge.PARTIAL.GetDescription();
                        break;
                }
            }
            return color;
        }

        private async System.Threading.Tasks.Task<(MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess, bool showPremierAccess)> GetPremierAccessAndPriorityBoarding(MOBPNRByRecordLocatorRequest request, Session session, ReservationDetail cslReservationDetail, Service.Presentation.ProductResponseModel.ProductOffer productOffers, MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess, bool showPremierAccess)
        {
            if (productOffers != null)
            {
                try
                {
                    #region Premier Access and Priority Boarding
                    MOBAncillary ancillary = new MOBAncillary();
                    string jsonRequest = JsonConvert.SerializeObject(request);
                    string pnrCreatedDate = _merchandizingServices.GetPnrCreatedDate(cslReservationDetail.Detail);
                    var jsonResponse = JsonConvert.SerializeObject(productOffers);
                    var tupleRes = await _merchandizingServices.PBAndPADetailAssignment_CFOP(session.SessionId, request.Application, request.RecordLocator, request.LastName, pnrCreatedDate, priorityBoarding, premierAccess, jsonRequest, jsonResponse);
                    ancillary.PriorityBoarding = tupleRes.priorityBoarding;
                    premierAccess = tupleRes.premierAccess;
                    _merchandizingServices.PBAndPAAssignment(request.SessionId, ref ancillary, request.Application, request.DeviceId, tupleRes.priorityBoarding, "GetPAandPBInfoInViewRes", ref premierAccess, ref showPremierAccess);
                    priorityBoarding = ancillary.PriorityBoarding;
                    #endregion
                }
                catch (MOBUnitedException uaex)
                {
                    MOBExceptionWrapper uaexWrapper = new MOBExceptionWrapper(uaex);
                }

                catch (Exception)
                { }
            }
            return (priorityBoarding, premierAccess, showPremierAccess);
        }

        private async System.Threading.Tasks.Task<(MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess, bool showPremierAccess)> GetPremierAccessAndPriorityBoarding(TravelOptionsRequest request, Session session, ReservationDetail cslReservationDetail, DynamicOfferDetailResponse productOffers, MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess, bool showPremierAccess)
        {
            if (productOffers != null)
            {
                try
                {
                    #region Premier Access and Priority Boarding
                    MOBAncillary ancillary = new MOBAncillary();
                    string jsonRequest = JsonConvert.SerializeObject(request);
                    string pnrCreatedDate = _merchandizingServices.GetPnrCreatedDate(cslReservationDetail.Detail);
                    var jsonResponse = JsonConvert.SerializeObject(productOffers);
                    var tupleRes = await _merchandizingServices.PBAndPADetailAssignment_CFOP(session.SessionId, request.Application, request.RecordLocator, request.LastName, pnrCreatedDate, priorityBoarding, premierAccess, jsonRequest, jsonResponse, true);
                    ancillary.PriorityBoarding = tupleRes.priorityBoarding;
                    premierAccess = tupleRes.premierAccess;
                    _merchandizingServices.PBAndPAAssignment(request.SessionId, ref ancillary, request.Application, request.DeviceId, tupleRes.priorityBoarding, "GetPAandPBInfoInViewRes", ref premierAccess, ref showPremierAccess);
                    priorityBoarding = ancillary.PriorityBoarding;
                    #endregion
                }
                catch (MOBUnitedException uaex)
                {
                    MOBExceptionWrapper uaexWrapper = new MOBExceptionWrapper(uaex);
                }

                catch (Exception)
                { }
            }
            return (priorityBoarding, premierAccess, showPremierAccess);
        }

        private bool FeatureVersionCheck(int appId, string appVersion, string featureName, string androidVersion, string iosVersion)
        {
            if (string.IsNullOrEmpty(appVersion) || string.IsNullOrEmpty(featureName))
                return false;
            return _configuration.GetValue<bool>(featureName)
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, androidVersion, iosVersion, "", "", true, _configuration);
        }

        private async Task<List<RewardProgram>> GetAllRewardProgramItems(int applicationId, string deviceId, string appVersion, string transactionId, string sessionID, string token)
        {
            //Check in Couchbase if it is available.
            var rewardProgram = await _cachingService.GetCache<List<RewardProgram>>(_configuration.GetValue<string>("FrequestFlyerRewardProgramListStaticGUID") + "Booking2.0FrequentFlyerList", transactionId).ConfigureAwait(false);//United.Persist.FilePersist.Load<List<MOBSHOPRewardProgram>>
            var rewardProgramList = JsonConvert.DeserializeObject<List<RewardProgram>>(rewardProgram);

            if (rewardProgramList == null || (rewardProgramList != null && rewardProgramList.Count == 0))
            {
                //If Not in Couchbase call CSL
                rewardProgramList = await GetRewardPrograms(applicationId, deviceId, appVersion, transactionId, sessionID, token).ConfigureAwait(false);

                //Finally save retrieved data to couchbase.
                await _cachingService.SaveCache<List<RewardProgram>>(_configuration.GetValue<string>("FrequestFlyerRewardProgramListStaticGUID") + "Booking2.0FrequentFlyerList", rewardProgramList, transactionId, new TimeSpan(1, 30, 0)).ConfigureAwait(false);

            }

            return rewardProgramList;
        }

        private async Task<List<RewardProgram>> GetRewardPrograms(int applicationId, string deviceId, string appVersion, string transactionId, string sessionID, string token)
        {
            var rewardPrograms = new List<RewardProgram>();
            var response = new Service.Presentation.ReferenceDataResponseModel.RewardProgramResponse();
            response.Programs = (await _referencedataService.RewardPrograms<Collection<Program>>(token, sessionID)).Response;

            if (response?.Programs?.Count > 0)
            {
                foreach (var reward in response.Programs)
                {
                    if (reward.ProgramID != 5)
                    {
                        rewardPrograms.Add(new RewardProgram() { Description = reward.Description, ProgramID = reward.ProgramID.ToString(), Type = reward.Code.ToString() });
                    }
                }
            }
            else
            {
                if (response.Errors != null && response.Errors.Count > 0)
                {
                    _logger.LogError("GetRewardPrograms - Response {Error}", response.Errors);
                }
            }

            return rewardPrograms;
        }

        private void SetAssociateMPNumberDetails(MOBPNR pnr, string mpNumber, MOBPNRByRecordLocatorRequest request)
        {
            try
            {
                var customerSyncRestAPI = new SyncGatewayRESTClient(_configuration.GetValue<string>("SyncGatewayAdminUrl"), _configuration.GetValue<string>("SyncGatewayMappedPrivateBucket"));

                if (!string.IsNullOrEmpty(mpNumber) && customerSyncRestAPI != null && pnr != null)
                {
                    string predictableKey = "LOOKUP::ACCOUNT::" + mpNumber.ToUpper();

                    //SignedIn Customer Profile data from Couchbase
                    var document = customerSyncRestAPI.GetDocument<LookupAccountDetails>("", predictableKey);


                    if (document != null && pnr.Passengers != null && pnr.Passengers.Count > 0)
                    {

                        var profileFullName = (document.Name.First + document.Name.Middle + document.Name.Last).Trim().ToLower().Replace(" ", "");
                        var profileFullNameWithSiffix = (document.Name.First + document.Name.Middle + document.Name.Last + document.Name.Suffix).Trim().ToLower().Replace(" ", "").Replace(".", "");
                        var profileFirstLastNames = (document.Name.First + document.Name.Last).Trim().ToLower().Replace(" ", "");
                        var profileFirstLastNamesWithSuffix = (document.Name.First + document.Name.Last + document.Name.Suffix).Trim().ToLower().Replace(" ", "").Replace(".", "");

                        foreach (var pax in pnr.Passengers.Where(p => p.MileagePlus == null))
                        {
                            var pnrFullNames = (pax.PassengerName.First + pax.PassengerName.Middle + pax.PassengerName.Last).Trim().ToLower().Replace(" ", "");

                            if (profileFullName == pnrFullNames ||
                                profileFirstLastNames == pnrFullNames ||
                                profileFullNameWithSiffix == pnrFullNames ||
                                profileFirstLastNamesWithSuffix == pnrFullNames)
                            {
                                pnr.AssociateMPId = "true";
                                pnr.AssociateMPIdSharesGivenName = pax.SharesGivenName;
                                pnr.AssociateMPIdSharesPosition = pax.SHARESPosition;
                                pnr.AssociateMPIdMessage = _configuration.GetValue<string>("AssociateMPNumberPopupMsg");
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("GetPNRByRecordLocator_CFOP - SyncGateway LOOKUP - Exception{exception}", JsonConvert.SerializeObject(ex));
            }
        }

        private bool AssociateMPIDEligibilityCheck(MOBPNR pnr, string MileagePlusNumber)
        {
            //AssociateMPtoPNR is eligible if
            //      MOBPNR pnr object has atleast 1 Passenger &&
            //      MOBPNR pnr Passenger object has atleast 1 Passenger whose MP number is not already associated &&
            //      Logged in MP number is not already associated to the pnr

            if (string.IsNullOrEmpty(MileagePlusNumber) || pnr == null)
                return false;

            try
            {
                return (pnr.Passengers != null &&
                            pnr.Passengers.Any(p => p.MileagePlus == null) &&
                            !pnr.Passengers.Any(p => p.MileagePlus != null && MileagePlusNumber.Equals(p.MileagePlus.MileagePlusId, StringComparison.OrdinalIgnoreCase)));
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void UpdatePnrWithBundleInfo(MOBPNRByRecordLocatorRequest request, MOBBundleInfo bundleInfo, ref MOBPNRByRecordLocatorResponse response)
        {
            if (_configuration.GetValue<bool>("GetBundleInfo"))
            {
                try
                {
                    var isAwardAcceleratorVersion = GeneralHelper.IsApplicationVersionGreater(request.Application.Id, request.Application.Version.Major, "AndroidAwardAcceleratorVersion", "iPhoneAwardAcceleratorVersion", "", "", true, _configuration);
                    response.PNR.BundleInfo = bundleInfo;
                    if (response.PNR.Segments != null && response.PNR.BundleInfo != null &&
                        response.PNR.BundleInfo.FlightSegments != null)
                    {
                        foreach (var seg in response.PNR.Segments)
                        {
                            //Duplicate Bundles are showing in Manage flow AB-2386
                            MOBBundleFlightSegment bundleSeg = null;
                            if (Convert.ToBoolean(_configuration.GetValue<string>("DuplicateBundlesInManageFlow")))
                            {
                                bundleSeg = response.PNR.BundleInfo.FlightSegments.Find(x => x.DepartureAirport == seg.Departure.Code &&
                                                                                                 x.ArrivalAirport == seg.Arrival.Code &&
                                                                                                 x.FlightNumber == seg.FlightNumber &&
                                                                                                 (string.IsNullOrEmpty(x.SegmentId) ||
                                                                                                 (!string.IsNullOrEmpty(x.SegmentId) && Convert.ToInt32(x.SegmentId) == seg.SegmentNumber)));
                            }
                            else
                            {
                                bundleSeg = response.PNR.BundleInfo.FlightSegments.Find(x => x.DepartureAirport == seg.Departure.Code &&
                                                                                                 x.ArrivalAirport == seg.Arrival.Code &&
                                                                                                 x.FlightNumber == seg.FlightNumber);
                            }

                            if (bundleSeg != null)
                            {
                                seg.Bundles = new List<MOBBundle>();
                                foreach (var passenger in response.PNR.Passengers)
                                {
                                    var bundleTraveler = bundleSeg.Travelers.Find(t => t != null && t.Id != null && ((t.Id + ".1") == passenger.SHARESPosition || ("1." + t.Id) == passenger.SHARESPosition));
                                    var bundleLabel = bundleTraveler != null ? bundleTraveler.BundleDescription : string.Empty;
                                    //Remove Award Accelerator and Premier Accelerator texts for unsupported versions.
                                    if (!isAwardAcceleratorVersion && !bundleLabel.Equals(_configuration.GetValue<string>("BundlesCodeCommonDescription")))
                                    {
                                        bundleLabel = string.Empty;
                                    }
                                    MOBBundle bundle = new MOBBundle
                                    {
                                        PassengerSharesPosition = passenger.SHARESPosition,
                                        BundleDescription = bundleLabel
                                    };
                                    seg.Bundles.Add(bundle);
                                }
                            }
                        }
                    }
                }
                catch (System.Exception ex1)
                {
                    _logger.LogError("GetBundleInfoWithPNR Exception{exception}", ex1);

                    response.PNR.BundleInfo = new MOBBundleInfo();
                    response.PNR.BundleInfo.ErrorMessage = "Bundle information is not available.";
                }
            }
        }

        private bool ShowSeatChangeButton(MOBPNR pnr)
        {
            if (pnr.Passengers != null && pnr.Passengers.Count > 9 || pnr.isgroup == true)
            {
                return false;
            }

            return pnr.IsEligibleToSeatChange && _configuration.GetValue<bool>("ShowSeatChange");
        }

        private async System.Threading.Tasks.Task CheckForFuzziPNRAndSaveCommonDefPersistsFile(string clientSentPNR, string DeviceId, string cslRecordLocator, CommonDef commonDef)
        {
            //to resolve the issue where within the PNR there is a character 0, 1 or 5  and client sends O, I, or S, then while reading the CommonDef, there was no file with Device + PNR
            // Combination.  Hence adding this condition to save second CommonDef file by the Device + PNR sent by CSL service combination
            if (_configuration.GetValue<bool>("EnableFuzziPNRCheckChanges") && clientSentPNR.ToUpper().Trim() != cslRecordLocator.ToUpper().Trim())
            {
                await _sessionHelperService.SaveSession<CommonDef>(commonDef, (DeviceId + cslRecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), new List<string> { (DeviceId + cslRecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName }, commonDef.ObjectName).ConfigureAwait(false);
            }
        }

        private async Task<(MOBPNR pnr, ReservationDetail clsReservationDetail)> LoadPnr(MOBPNRByRecordLocatorRequest request, Session session)
        {
            MOBPNR pnr;
            ReservationDetail clsReservationDetail = null;
            var tupleRes = await _flightReservation.GetPNRByRecordLocatorFromCSL(request.SessionId, request.DeviceId, request.RecordLocator, request.LastName, request.LanguageCode, request.Application.Id, request.Application.Version.Major, false, session, clsReservationDetail, request.IsOTFConversion, request.MileagePlusNumber);
            pnr = tupleRes.pnr;
            clsReservationDetail = tupleRes.response;
            if (pnr.IsIBELite || pnr.IsIBE)
            {
                new ForceUpdateVersion(_configuration).ForceUpdateForNonSupportedVersion(request.Application.Id, request.Application.Version.Major, FlowType.ALL);
            }

            var eligibility = new EligibilityResponse
            {
                IsElf = pnr.isELF,
                IsIBELite = pnr.IsIBELite,
                IsIBE = pnr.IsIBE,
                Passengers = pnr.Passengers,
                Segments = pnr.Segments,
                IsUnaccompaniedMinor = pnr.IsUnaccompaniedMinor,
                InfantInLaps = pnr.InfantInLaps,
                IsReshopWithFutureFlightCredit = pnr.IsCanceledWithFutureFlightCredit,
                IsAgencyBooking = pnr.IsAgencyBooking,
                AgencyName = pnr.AgencyName,
                IsCheckinEligible = pnr.IsCheckinEligible,
                IsCorporateBooking = pnr.IsCorporateBooking,
                CorporateVendorName = pnr.CorporateVendorName,
                IsBEChangeEligible = pnr.IsBEChangeEligible,
                HasScheduleChange = pnr.HasScheduleChanged,
                IsSCChangeEligible = pnr.IsSCChangeEligible,
                IsSCRefundEligible = pnr.IsSCRefundEligible,
                IsATREEligible = pnr.IsATREEligible,
                IsChangeEligible = pnr.IsChangeEligible,
                IsMilesAndMoney = pnr.IsMilesAndMoney,
                Is24HrFlexibleBookingPolicy = pnr.Is24HrFlexibleBookingPolicy,
                IsJSENonChangeableFare = pnr.IsJSENonChangeableFare
            };

            await _sessionHelperService.SaveSession<EligibilityResponse>(eligibility, pnr.SessionId, new List<string> { pnr.SessionId, eligibility.ObjectName }, eligibility.ObjectName).ConfigureAwait(false);
            await _sessionHelperService.SaveSession<Session>(session, (request.DeviceId + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), new List<string> { (request.DeviceId + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), session.ObjectName }, session.ObjectName).ConfigureAwait(false);

            return (pnr, clsReservationDetail);
        }

        private async Task<DOTBaggageInfo> GetBaggageInfo(MOBPNR pnr, string transactionId)
        {
            var isElf = pnr != null && pnr.isELF;
            var isIbe = pnr != null && pnr.IsIBE;
            return await GetBaggageInfo(isElf, isIbe, transactionId);
        }
        private async Task<DOTBaggageInfo> GetBaggageInfo(bool isElf, bool isIBE, string transactionId)
        {
            //var titleList = GetList(Titles);

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
                legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle1);
                if (isIBE)
                {
                    legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle1ELF);
                    legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle3);
                }
                else
                {
                    legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle1IBE);
                    legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle3IBE);
                }
            }
            else
            {
                legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle1ELF);
                legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle1IBE);
                legalDocuments.RemoveAll(l => l.Title == DOTBaggageInfoDBTitle3IBE);
            }

            var document1TitleAndDescription = legalDocuments.First(l => l.Title.Contains(DOTBaggageInfoDBTitle1)).LegalDocument.Split('|');
            var document2TitleAndDescription = legalDocuments.First(l => l.Title.Contains(DOTBaggageInfoDBTitle2)).LegalDocument.Split('|');
            var document3TitleAndDescription = legalDocuments.First(l => l.Title.Contains(DOTBaggageInfoDBTitle3)).LegalDocument.Split('|');
            var document4 = legalDocuments.First(l => l.Title.Contains(DOTBaggageInfoDBTitle4)).LegalDocument;

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

        private async Task<(MOBBundleInfo bundleInfo, MOBPriorityBoarding priorityBoarding, MOBPremierAccess premierAccess)> GetBundleInfo(MOBPNRByRecordLocatorRequest request, string channelId, string channelName)
        {
            if (!_configuration.GetValue<bool>("GetBundleInfo"))
                return (null, null, null);

            if (request == null || request.Flow == FlowType.VIEWRES_SEATMAP.ToString())
                return (null, null, null);

            MOBBundleInfo bundleInfo = null;
            MOBPriorityBoarding priorityBoarding = new MOBPriorityBoarding();
            MOBPremierAccess premierAccess = new MOBPremierAccess();
            try
            {
                MOBBundlesMerchangdizingRequest bundleRequest = new MOBBundlesMerchangdizingRequest();
                bundleRequest.Application = request.Application;
                bundleRequest.TransactionId = request.SessionId;
                bundleRequest.RecordLocator = request.RecordLocator;

                var bundleDetails = await _merchandizingServices.GetBundleInfoWithPNR(bundleRequest, channelId, channelName);
                MOBBundlesMerchandizingResponse bundleResponse = bundleDetails.response;
                bundleInfo = bundleResponse != null ? bundleResponse.BundleInfo : null;

                priorityBoarding = bundleDetails.priorityBoarding;
                premierAccess = bundleDetails.premierAccess;

            }
            catch (System.Exception ex1)
            {
                bundleInfo = new MOBBundleInfo();
                bundleInfo.ErrorMessage = "Bundle information is not available.";
                _logger.LogError("GetBundleInfo {exception} {errormessage}", JsonConvert.SerializeObject(ex1), bundleInfo.ErrorMessage);
            }

            return (bundleInfo, priorityBoarding, premierAccess);
        }

        private async System.Threading.Tasks.Task ValidateIRROPSStatus(MOBPNRByRecordLocatorRequest request, MOBPNRByRecordLocatorResponse response,
            ReservationDetail cslReservationDetail, Session session)
        {
            try
            {
                if (IRROPRedirectEnabled(request.Application.Id, request.Application.Version.Major) == false) return;

                if (_configuration.GetValue<bool>("IncludeValidateIRROPSCheck"))
                {
                    if (response.PNR.IsCanceledWithFutureFlightCredit || !response.PNR.IsETicketed
                        || !string.IsNullOrEmpty(response.PNR.SyncedWithConcur)) return;
                }
                else
                {
                    if (response.PNR.IsCanceledWithFutureFlightCredit
                        || !string.IsNullOrEmpty(response.PNR.SyncedWithConcur)) return;
                }

                response.PNR.IRROPSChangeInfo = response.PNR.IRROPSChangeInfo ?? new MOBIRROPSChange();
                response.PNR.IRROPSChangeInfo = await _flightReservation.ValidateIRROPSStatus(request, response, cslReservationDetail, session).ConfigureAwait(false);

                _logger.LogInformation("ValidateIRROPSStatus {irropschangeinfo}", JsonConvert.SerializeObject(response.PNR.IRROPSChangeInfo));
            }
            catch { response.PNR.IRROPSChangeInfo = null; }
        }

        private bool IRROPRedirectEnabled(int applicationId, string applicationVersion)
        {
            return !_configuration.GetValue<bool>("ExcludeValidateIRROPSStatus") &&
               GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, applicationVersion,
                    _configuration.GetValue<string>("Android_EnableIRROPRedirectAppVersion"), _configuration.GetValue<string>("iPhone_EnableIRROPRedirectAppVersion"));
        }

        private async Task<MOBPlacePass> GetPlacePass(MOBPNRByRecordLocatorRequest request, MOBPNR pnr)
        {
            if (request == null || request.Flow == FlowType.VIEWRES_SEATMAP.ToString())
                return null;

            //sandeep placepass changes
            MOBPlacePass placePass = null;
            if (_configuration.GetValue<bool>("PlacePassTurnOnToggle_Manageres") && !pnr.IsNullOrEmpty()
                && EnablePlacePassManageRes(request.Application.Id, request.Application.Version.Major)
                && !EnableViewResDynamicPlacePass(request.Application.Id, request.Application.Version.Major)
                ) // for 2.1.63 and Below condition shouldbe if( T && T && F) for 2.1.64 and Above if(T && T && T)
            {
                try
                {
                    string searchtype = pnr.JourneyType;
                    string destinationcode = pnr.Trips.Select(t => t.Destination).ToList()[0];
                    placePass = await GetEligiblityPlacePass(destinationcode, searchtype, request.TransactionId, request.Application.Id, request.Application.Version.Major, request.DeviceId, "GetPNRByRecordLocatorPlacePass");
                }
                catch (Exception ex)
                {
                    MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                    _logger.LogError("GetPNRByRecordLocator - Placepass Exception{exception}", ex);
                }
            }
            return placePass;
        }
        #region//utilitites
        private bool EnablePlacePassManageRes(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("PlacePassTurnOnToggle_Manageres")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPlacePassVersion", "iPhonePlacePassVersion", "", "", true, _configuration);
        }
        private bool EnableViewResDynamicPlacePass(int appId, string appVersion)
        {
            return _configuration.GetValue<bool>("PlacePassServiceTurnOnToggle_ViewReservation")
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, "AndroidPlacePassVersion_ViewResDynamic", "iPhonePlacePassVersion_ViewResDynamic", "", "", true, _configuration);
        }

        private async Task<MOBPlacePass> GetEligiblityPlacePass(string destinationAiportCode, string tripType, string sessionId, int appID, string appVersion, string deviceId, string logAction)
        {
            #region Load Macthed Place Pass from the Persist Place Passes List
            MOBPlacePass matchedPlacePass = new MOBPlacePass();
            try
            {
                #region
                int flag = 2;
                //Should be Cache not Session
                var response = await _cachingService.GetCache<string>(_configuration.GetValue<string>("GetAllEligiblePlacePassesAndSaveToPersistStaticGUID") + "AllEligiblePlacePasses", _headers.ContextValues?.TransactionId).ConfigureAwait(false); //change session
                List<MOBPlacePass> placePassListFromPersist = JsonConvert.DeserializeObject<List<MOBPlacePass>>(response);

                if (placePassListFromPersist == null)
                {
                    placePassListFromPersist = await GetAllEligiblePlacePasses(sessionId, appID, appVersion, deviceId, logAction, true);
                }

                //logEntries.Add(LogEntry.GetLogEntry<List<MOBPlacePass>>(sessionId, logAction, "MOBPlacepassResponseFromPersist", appID, appVersion, deviceId, placePassListFromPersist, true, false));

                if (!string.IsNullOrEmpty(destinationAiportCode) && !string.IsNullOrEmpty(tripType))
                {
                    switch (tripType.ToLower())
                    {
                        case "ow":
                        case "one_way":
                            flag = 1;
                            break;
                        case "rt":
                        case "round_trip":
                            flag = 1;
                            break;
                        case "md":
                        case "multi_city":
                            flag = 2;
                            break;
                    }

                    foreach (MOBPlacePass placePass in placePassListFromPersist)
                    {
                        if (placePass.PlacePassID == 1)
                        {
                            matchedPlacePass = placePass;
                        }
                        if (placePass.Destination.Trim().ToUpper() == destinationAiportCode.Trim().ToUpper() && flag == 1)
                        {
                            matchedPlacePass = placePass;
                            break;
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                //logEntries.Add(LogEntry.GetLogEntry(sessionId, logAction, "Exception", appID, appVersion, deviceId, exceptionWrapper));

                matchedPlacePass = new MOBPlacePass();
            }
            #endregion Load Mached Place Pass from the Persist Place Passes List
            return matchedPlacePass;
        }

        private async Task<List<MOBPlacePass>> GetAllEligiblePlacePasses(string sessionId, int appID, string appVersion, string deviceId, string logAction, bool saveToPersist)
        {
            List<MOBPlacePass> placepasses = new List<MOBPlacePass>();
            logAction = logAction == null ? "GetAllEligiblePlacePasses" : logAction + "- GetAllEligiblePlacePasses";

            var manageresDynamoDB = new ManageResDynamoDB(_configuration, _dynamoDBService);
            string destinationCode = "ALL";
            int flag = 3;
            var eligibleplace = await manageresDynamoDB.GetAllEligiblePlacePasses<List<MOBPlacePass>>(destinationCode, flag, sessionId).ConfigureAwait(false);

            try
            {
                //while (var eligibleplace)
                //{
                //    MOBPlacePass placepass = new MOBPlacePass();
                //    placepass.PlacePassID = Convert.ToInt32(eligibleplace[0]);
                //    placepass.Destination = dataReader["DestinationCode"].ToString().Trim();
                //    placepass.PlacePassImageSrc = dataReader["PlacePassImageSrc"].ToString().Trim();
                //    placepass.OfferDescription = dataReader["CityDescription"].ToString().Trim();
                //    placepass.PlacePassUrl = dataReader["PlacePassUrl"].ToString().Trim();
                //    placepass.TxtPoweredBy = "Powered by";
                //    placepass.TxtPlacepass = "PLACEPASS";
                //    placepasses.Add(placepass);
                //}
                foreach (var place in eligibleplace)
                {
                    MOBPlacePass placepass = new MOBPlacePass();
                    placepass.PlacePassID = Convert.ToInt32(place.PlacePassID);
                    placepass.Destination = place.Destination;
                    placepass.PlacePassImageSrc = place.PlacePassImageSrc;
                    placepass.OfferDescription = place.OfferDescription;
                    placepass.PlacePassUrl = place.PlacePassUrl;
                    placepass.TxtPoweredBy = "Powered by"; ;
                    placepass.PlacePassUrl = "PLACEPASS";
                    placepasses.Add(placepass);
                }

            }
            catch (Exception ex)
            {
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                //logEntries.Add(LogEntry.GetLogEntry(sessionId, logAction, "Exception", appID, appVersion, deviceId, exceptionWrapper));
            }
            return placepasses;
        }

        private async System.Threading.Tasks.Task SaveLogToTempAnalysisTable(string logStatement, string sessionId)
        {
            LogData data = new LogData
            {
                Comment = logStatement,
                Count = 0
            };

            var manageResDynamodb = new ManageResDynamoDB(_configuration, _dynamoDBService);
            await manageResDynamodb.SaveLogToTempAnalysisTable<LogData>(data, logStatement, sessionId);

        }
        #endregion

        public string GetTripDetailRedirect3dot0Url
    (string cn, string ln, string ac, int timestampvalidity = 0, string channel = "mobile",
    string languagecode = "en/US", string trips = "", string travelers = "", string ddate = "",
    string guid = "", bool isAward = false)
        {
            var retUrl = string.Empty;
            //REF:{0}/{1}/manageres/tripdetails/{2}/{3}?{4}
            //{env}/{en/US}/manageres/tripdetails/{encryptedStuff}/mobile?changepath=true
            var baseUrl = _configuration.GetValue<string>("TripDetailRedirect3dot0BaseUrl");
            var urlPattern = _configuration.GetValue<string>("TripDetailRedirect3dot0UrlPattern");
            var urlPatternFSR = _configuration.GetValue<string>("ReshopFSRRedirect3dot0UrlPattern");
            DateTime timestamp
                = (timestampvalidity > 0) ? DateTime.Now.ToUniversalTime().AddMinutes(timestampvalidity) : DateTime.Now.ToUniversalTime();
            var encryptedstring = string.Empty;
            if (_configuration.GetValue<bool>("EnableRedirect3dot0UrlWithSlashRemoved"))
            {
                encryptedstring = EncryptString
                (string.Format("RecordLocator={0};LastName={1};TimeStamp={2};", cn, ln, timestamp)).Replace("/", "~~");
            }
            else
            {
                encryptedstring = EncryptString
                (string.Format("RecordLocator={0};LastName={1};TimeStamp={2};", cn, ln, timestamp));
            }
            var encodedstring = HttpUtility.UrlEncode(encryptedstring);
            string encodedpnr = HttpUtility.UrlEncode(EncryptString(cn));
            string from = "mobilecheckinsdc";
            if (string.Equals(ac, "EX", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format
                    (urlPattern, baseUrl, languagecode, encodedstring, channel, "changepath=true");
            }
            else if (string.Equals(ac, "CA", StringComparison.OrdinalIgnoreCase))
            {
                return string.Format
                    (urlPattern, baseUrl, languagecode, encodedstring, channel, "cancelpath=true");
            }
            else if (string.Equals(ac, "CSDC", StringComparison.OrdinalIgnoreCase))
            {
                //&td1=01-29-2021&idx=1
                string inputdatapattern = "pnr={0}&trips={1}&travelers={2}&from={3}&guid={4}&td1={5}{6}";
                return string.Format(urlPatternFSR, baseUrl, languagecode, isAward ? "awd" : "rev",
                    string.Format(inputdatapattern, encodedpnr, trips, travelers, from, guid,
                    ddate, isAward ? string.Empty : "&TYPE=rev"));
            }
            else
            {
                return string.Format
                    (urlPattern, baseUrl, languagecode, encodedstring, channel, string.Empty).TrimEnd('?');
            }
        }

        private string EncryptString(string data)
        {
            return United.ECommerce.Framework.Utilities.SecureData.EncryptString(data);
        }

        public async Task<DynamicOfferDetailResponse> GetDODResponseFromCCE(TravelOptionsRequest request, Session session, ReservationDetail cslReservation, bool isLoadPCUOfferForSeatMap)
        {
            DynamicOfferDetailResponse dodResponse = new DynamicOfferDetailResponse();

            MOBPNR mobPnr = await GetPNRResponse(request.SessionId);
            dodResponse = await _merchandizingServices.GetProductOfferAndDetails(request, cslReservation.Detail, mobPnr, session.Token).ConfigureAwait(false);

            if (isLoadPCUOfferForSeatMap)
            {
                United.Mobile.Model.Shopping.Pcu.MOBPremiumCabinUpgrade premiumCabinUpgrade = new Mobile.Model.Shopping.Pcu.MOBPremiumCabinUpgrade();
                premiumCabinUpgrade = await _merchandizingServices.GetPremiumCabinUpgrade_CFOP(null, new MOBPNRByRecordLocatorRequest(), request.SessionId, cslReservation.Detail, dodResponse, request);
            }

            return dodResponse;
        }

        public async Task<TravelOptionsResponse> GetProductOfferAndDetails(TravelOptionsRequest request, Session session)
        {
            TravelOptionsResponse response = new TravelOptionsResponse();

            #region Call CCE and build offer based on product.
            ReservationDetail cslReservation = await GetCslReservation(session.SessionId);
            DynamicOfferDetailResponse dodResponse = null;
            if (cslReservation != null && cslReservation.Detail != null && cslReservation.Detail.FlightSegments != null)
            {
                dodResponse = new DynamicOfferDetailResponse();
                dodResponse = await GetDODResponseFromCCE(request, session, cslReservation, false);
            }

            if (dodResponse != null && dodResponse.Offers != null && dodResponse.Offers.Count > 0)
            {
                response.Title = _configuration.GetValue<string>("MRTravelOptionsBundleDetailTitle");
                response.Flow = request.Flow;
                response.SessionId = request.SessionId;
                response.CorrelationId = request.CorrelationId;

                if (request.ProductCode.Equals(ProductName.ANC.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    #region Load all offers from cce response and trip insurence from CCE response
                    var productTiles = await GetProductTileForAllOffers(dodResponse, request.SessionId).ConfigureAwait(false);

                    var tripInsuranceInfo = await GetTripInsurance(session.SessionId);
                    ProductTile tpiTile = GetTPITile(tripInsuranceInfo);

                    if (tpiTile != null && tpiTile.Amount > 0)
                    {
                        int tpiIndex = _configuration.GetValue<int>("MRTravelOptionsPageTPIIndex");
                        if (productTiles?.Count >= tpiIndex)
                            productTiles.Insert(tpiIndex, tpiTile);
                        else
                            productTiles.Add(tpiTile);
                    }

                    response.ProductTiles = productTiles;
                    response.ProductName = ProductName.ANC.ToString();
                    response.ProductCode = ProductName.ANC.ToString();
                    #endregion
                }
                else
                {
                    if (request.ProductName.Equals(ProductName.BUNDLE.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        #region Bundle detail page
                        response.ProductName = ProductName.BUNDLE.ToString();
                        response.ProductCode = request.ProductCode;

                        response.BundleDetails = GetBundleDetail(request, dodResponse);
                        await SaveBundleDetailsInPersist(request.SessionId, response);
                        if (response.BundleDetails == null || response.BundleDetails.Count == 0)
                        {
                            response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
                            return response;
                        }
                        #endregion
                    }
                    if (request.ProductCode.Equals(ProductName.PCU.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        #region Premium Cabin Upgrade, Building offers from CCE response and record locator from new api request but rest of code is same.
                        response.StandAloneProductResponse = new StandAloneProductResponse();
                        response.StandAloneProductResponse.PremiumCabinUpgrade = await _merchandizingServices.GetPremiumCabinUpgrade_CFOP(null, new MOBPNRByRecordLocatorRequest(), request.SessionId, cslReservation.Detail, dodResponse, request);

                        response.ProductName = ProductName.PCU.ToString();
                        response.ProductCode = ProductName.PCU.ToString();
                        #endregion

                    }
                    if (request.ProductCode.Equals(ProductName.PAS.ToString(), StringComparison.OrdinalIgnoreCase) || request.ProductCode.Equals(ProductName.PBS.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        #region Premier Access or Priority Boarding
                        /// Building offers from CCE response but rest of code is same.
                        MOBPremierAccess premierAccess = new MOBPremierAccess();
                        MOBPriorityBoarding priorityBoarding = new MOBPriorityBoarding();
                        var showPremierAccess = false;
                        var tupleRes = await GetPremierAccessAndPriorityBoarding(request, session, cslReservation, dodResponse, priorityBoarding, premierAccess, showPremierAccess);
                        priorityBoarding = tupleRes.priorityBoarding;
                        response.StandAloneProductResponse = new StandAloneProductResponse();
                        response.StandAloneProductResponse.PremierAccess = tupleRes.premierAccess;
                        response.StandAloneProductResponse.PriorityBoarding = priorityBoarding;
                        response.ProductCode = tupleRes.showPremierAccess ? ProductName.PAS.ToString() : priorityBoarding != null ? ProductName.PBS.ToString() : string.Empty;
                        response.ProductName = tupleRes.showPremierAccess ? ProductName.PAS.ToString() : priorityBoarding != null ? ProductName.PBS.ToString() : string.Empty;
                        #endregion
                    }
                }
            }
            else
            {
                throw new MOBUnitedException("10000",
                       _configuration.GetValue<string>("GenericExceptionMessage"));
            }
            #endregion
            return response;
        }

        private List<BundleDetails> GetBundleDetail(TravelOptionsRequest request, DynamicOfferDetailResponse dodResponse)
        {
            var bundleDetails = new List<BundleDetails>();
            if (dodResponse != null)
            {
                ResponseData responseData = dodResponse.ResponseData?.ToObject<ResponseData>();
                var content = responseData?.Contents?.Where(x => !string.IsNullOrEmpty(x?.Slot) && x.Slot.Equals("MobileMRAncillary", StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.Details?.FirstOrDefault(a => !string.IsNullOrEmpty(a?.Code) && a.Code.Equals(request.ProductCode));

                var bundleDetail = new BundleDetails();
                bundleDetail.Title = content?.Name;
                bundleDetail.Header = string.Empty;
                bundleDetail.BundleDescriptions = GetBundleDescriptions(content);
                bundleDetail.TripDetails = GetBundleTrip(request, dodResponse);
                bundleDetail.ActionButtons = GetBundleButtonOptions(content?.CTALabel, responseData?.Characteristics);
                Detail termsAndConditions = GetProductTermsAndCondtions(request.ProductCode, responseData);
                bundleDetail.TermsAndCondition = new MOBMobileCMSContentMessages()
                {
                    Title = termsAndConditions?.Name,
                    ContentFull = termsAndConditions?.Message
                };

                bundleDetails.Add(bundleDetail);
            }
            return bundleDetails;
        }

        private async System.Threading.Tasks.Task SaveBundleDetailsInPersist(string sessionId, TravelOptionsResponse response)
        {
            if (response.BundleDetails != null && response.BundleDetails.Count > 0)
            {
                List<BundleDescriptionPersist> bundleDescriptionPersist = new List<BundleDescriptionPersist>();
                response?.BundleDetails?.FirstOrDefault()?.BundleDescriptions?.ForEach(x =>
                {
                    bundleDescriptionPersist.Add(new BundleDescriptionPersist { Title = x.Title, Description = x.Description, ProductCode = x.Code });
                });
                BundleDetailsPersist bundleDetailsPersist = new BundleDetailsPersist
                {
                    Title = response?.BundleDetails?.FirstOrDefault().Title,
                    ProductCode = response?.ProductCode,
                    BundleDescriptions = bundleDescriptionPersist,
                    TermsAndCondition = new MOBMobileCMSContentMessages()
                    {
                        Title = response.BundleDetails?.FirstOrDefault()?.TermsAndCondition?.Title,
                        ContentFull = response.BundleDetails?.FirstOrDefault()?.TermsAndCondition?.ContentFull
                    }
                };
                await _sessionHelperService.SaveSession<BundleDetailsPersist>(bundleDetailsPersist, sessionId, new List<string> { sessionId, ObjectNames.BundleDetailsPersist }, ObjectNames.BundleDetailsPersist).ConfigureAwait(false);
            }
        }

        private BundleTrip GetBundleTrip(TravelOptionsRequest request, DynamicOfferDetailResponse dodResponse)
        {
            BundleTrip bundleTrip = new BundleTrip();
            if (dodResponse != null && dodResponse.Offers != null && dodResponse.Offers.Count > 0)
            {
                bundleTrip.Title = _configuration.GetValue<string>("MRTravelOptionsBundleTripTitle");
                bundleTrip.Header = dodResponse.Travelers.Count > 1 ? string.Format(_configuration.GetValue<string>("MRTravelOptionsBundleTripMultiTravelerText"), dodResponse.Travelers.Count) : string.Format(_configuration.GetValue<string>("MRTravelOptionsBundleTripTravelerText"), dodResponse.Travelers.Count);
                bundleTrip.NumberOfTravelers = dodResponse.Travelers.Count;
                bundleTrip.TotalPriceLabel = _configuration.GetValue<string>("MRTravelOptionsBundleTripTotalText");

                var productDetails = dodResponse.Offers.FirstOrDefault()?.ProductInformation?.ProductDetails;
                var productDetail = productDetails.FirstOrDefault(x => x?.Product?.Code == request.ProductCode);
                var bundlePrice = productDetail.Product?
                                          .SubProducts?.FirstOrDefault(sp => sp.InEligibleReason == null)?
                                          .Prices?.FirstOrDefault()?
                                          .PaymentOptions?.FirstOrDefault()?
                                          .PriceComponents?.FirstOrDefault()?
                                          .Price?
                                          .Totals?.FirstOrDefault()?
                                          .Amount;


                List<SegmentDetail> segmentDetails = new List<SegmentDetail>();
                #region Build Segment Selection screen for bundles details (Trip Details)
                var nonNullSubProducts = productDetail.Product != null && productDetail.Product.SubProducts?.Count > 0 ? productDetail.Product.SubProducts.Where(sp => sp != null) : new Collection<Service.Presentation.ProductModel.SubProduct>();

                foreach (var subProduct in nonNullSubProducts)
                {

                    SegmentDetail segmentDetail = new SegmentDetail();
                    segmentDetail.Price = Convert.ToDecimal(subProduct
                                                     .Prices?.FirstOrDefault()?
                                                     .PaymentOptions?.FirstOrDefault()?
                                                     .PriceComponents?.FirstOrDefault()?
                                                     .Price?
                                                     .Totals?.FirstOrDefault()?
                                                     .Amount);
                    if (subProduct.Prices?.Count > 0)
                    {
                        segmentDetail.ProductID = String.Join(",", subProduct.Prices.Select(p => p.ID).ToList()); //As we are getting multiple price items for multipax                     
                        segmentDetail.TripProductIDs = subProduct.Prices.Select(p => p.ID).ToList();
                        segmentDetail.PriceText = string.Concat("$", Convert.ToInt32(segmentDetail.Price), _configuration.GetValue<string>("MRTravelOptionsPriceSubText"));
                        segmentDetail.PriceForAllPax = segmentDetail.Price * bundleTrip.NumberOfTravelers;
                        segmentDetail.IsEligibleSegment = true;
                        if (dodResponse?.FlightSegments?.Count == 1)
                        {
                            segmentDetail.IsChecked = true;
                        }
                    }
                    else
                    {
                        segmentDetail.PriceText = BuildPriceTextForIneligibleProducts(subProduct);
                    }

                    segmentDetail.TripId = GetTripId(subProduct, dodResponse.ODOptions, dodResponse.FlightSegments);
                    segmentDetail.OriginDestination = GetOriginDestinationDesc(subProduct, dodResponse.ODOptions, dodResponse.FlightSegments);
                    segmentDetails.Add(segmentDetail);
                }
                #endregion
                bundleTrip.SegmentDetails = segmentDetails;
                bundleTrip.SegmentDetails?.Sort(x => x?.TripId);
            }
            return bundleTrip;
        }

        private string BuildPriceTextForIneligibleProducts(Service.Presentation.ProductModel.SubProduct subProduct)
        {
            string priceText = _configuration.GetValue<string>("MRTravelOptionsBundleTripPriceTextForNASegments");
            if (!string.IsNullOrEmpty(subProduct?.InEligibleReason?.Description))
            {
                if (subProduct.InEligibleReason.Description.ToUpper().Contains(IneligibleReasonForBundle.PURCHASED.GetDescription()))
                {
                    return _configuration.GetValue<string>("MRTravelOptionsBundleTripPriceTextForPurchasedSegments");
                }
                else if (subProduct.InEligibleReason.Description.ToUpper().Contains(IneligibleReasonForBundle.ONLYUA.GetDescription()))
                {
                    return _configuration.GetValue<string>("MRTravelOptionsBundleTripPriceTextForOASegments");
                }
            }
            return priceText;
        }

        private List<Buttons> GetBundleButtonOptions(string buttonText, Collection<Characteristic> characteristics)
        {
            string isNavigateToSeatMapText = _manageResUtility.GetCharactersticValue(characteristics, "IsSeatMapPricing");
            bool isNavigateToSeatMap = !string.IsNullOrEmpty(isNavigateToSeatMapText) ? Convert.ToBoolean(isNavigateToSeatMapText) : false;
            return new List<Buttons>() {
                                 new Buttons(){
                                    ButtonText = buttonText,
                                    ActionText = isNavigateToSeatMap ? ButtonActions.SEATMAP.ToString() : ButtonActions.PAYMENT.ToString(),
                                    IsPrimary = true,
                                    IsEnabled = _configuration.GetValue<bool>("EnableMRTravelOptionsBundleDetailButtonOnLaunch")
                                 },
                                new Buttons(){
                                    ButtonText = _configuration.GetValue<string>("MRTravelOptionsBundleCancelButtonText"),
                                    ActionText = ButtonActions.CANCEL.ToString(),
                                    IsPrimary = false,
                                    IsEnabled = true
                                }
                                };
        }

        private List<BundleDescription> GetBundleDescriptions(Detail content)
        {
            List<BundleDescription> bundleDescriptions = new List<BundleDescription>();
            if (content != null && content.SubDetails != null && content.SubDetails.Count > 0)
            {
                foreach (var detail in content.SubDetails)
                {
                    BundleDescription description = new BundleDescription();
                    description.Title = detail.Genre?.Value;
                    description.Code = detail.Genre?.Key;
                    description.Description = detail?.Description;
                    description.Icon = detail.Genre?.Description;

                    if (!string.IsNullOrEmpty(detail.Code) && detail.Code.ToUpper().Contains(TravelOptionBadge.PARTIAL.ToString()))
                    {
                        description.PartialWarningMessage = content?.Presentation;
                        List<EligibleAirport> eligibleAirportCodes = new List<EligibleAirport>();

                        var airports = detail.Value?.Split(',');
                        airports.ForEach(ap =>
                        {
                            EligibleAirport eligibleAirport = new EligibleAirport();
                            eligibleAirport.AirportCode = ap?.Split('-')[0];
                            eligibleAirport.IsEligibleAirport = Convert.ToBoolean(ap?.Split('-')[1]);

                            eligibleAirportCodes.Add(eligibleAirport);
                        });

                        description.EligibleAirportCodes = eligibleAirportCodes;
                    }
                    bundleDescriptions.Add(description);
                }
            }

            return bundleDescriptions;
        }

        private static Detail GetProductTermsAndCondtions(string code, ResponseData responseData)
        {
            return responseData?.Contents?.Where(x => !string.IsNullOrEmpty(x?.Slot) && x.Slot.Equals("TermsAndConditions", StringComparison.OrdinalIgnoreCase))?.FirstOrDefault()?.Details?.FirstOrDefault(a => !string.IsNullOrEmpty(a?.Code) && a.Code.Equals(code));
        }

        private async Task<List<ProductTile>> GetProductTileForAllOffers(DynamicOfferDetailResponse dodResponse, string sessionId)
        {
            List<ProductTile> productTiles = new List<ProductTile>();
            ResponseData responseData = dodResponse.ResponseData?.ToObject<ResponseData>(); //For offer tile which need to show on client

            var productDetails = dodResponse.Offers?.FirstOrDefault()?.ProductInformation?.ProductDetails; //Get offers from response
            if (productDetails == null || productDetails.Count == 0)
                productDetails = new Collection<Service.Presentation.ProductResponseModel.ProductDetail>();

            foreach (var productDetail in productDetails)
            {
                var productCode = productDetail?.Product?.SubProducts != null && productDetail.Product
                                          .SubProducts.Any(sp => sp?.InEligibleReason == null) ? productDetail?.Product?.Code : string.Empty;

                if (!string.IsNullOrEmpty(productCode))
                {
                    var detail = responseData?.Contents?.FirstOrDefault()?.Details.FirstOrDefault(d => d.Code.Equals(productCode, StringComparison.OrdinalIgnoreCase) && d.SubDetails != null);
                    ProductTile productTile = await GetProductTileFromDetail(detail, dodResponse?.Requester?.GUIDs, sessionId).ConfigureAwait(false);
                    if (!string.IsNullOrEmpty(productTile.ProductCode) && productTile.Amount > 0 && !string.IsNullOrEmpty(productTile.ProductName) && !string.IsNullOrEmpty(productTile.ProductCode))
                        productTiles.Add(productTile);
                }
            }
            return productTiles;
        }

        private async Task<Mobile.Model.MPSignIn.MOBTPIInfo> GetTripInsurance(string sessionId)
        {
            return await _sessionHelperService.GetSession<Mobile.Model.MPSignIn.MOBTPIInfo>(sessionId, ObjectNames.TPIResponse, new List<string> { sessionId, ObjectNames.TPIResponse }).ConfigureAwait(false);
        }

        public async Task<DynamicOfferDetailResponse> GetDynamicOfferDetailResponse(TravelOptionsRequest request, ReservationDetail cslReservation, MOBPNR mobPnr, string Token)
        {
            return await _merchandizingServices.GetProductOfferAndDetails(request, cslReservation.Detail, mobPnr, Token).ConfigureAwait(false);
        }

        public async Task<MOBPNR> GetPNRResponse(string sessionId)
        {
            return await _sessionHelperService.GetSession<MOBPNR>(sessionId, new MOBPNR().ObjectName, new List<string> { sessionId, new MOBPNR().ObjectName }).ConfigureAwait(false);
        }

        public async Task<ReservationDetail> GetCslReservation(string sessionId)
        {

            var reservationDetail = await _sessionHelperService.GetSession<ReservationDetail>(sessionId, new ReservationDetail().GetType().FullName, new List<string> { sessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);
            if (reservationDetail != null)
                return reservationDetail;

            return null;
        }

        private string GetTripId(Service.Presentation.ProductModel.SubProduct subProduct, Collection<Service.Presentation.ProductModel.ProductOriginDestinationOption> ODOptions, Collection<ProductFlightSegment> flightSegments)
        {
            string tripIndicator = string.Empty;
            if (ODOptions != null && ODOptions.Count > 0)
            {
                var odOption = ODOptions.First(od => od.ID == subProduct.Association.ODMappings.First().RefID);
                tripIndicator = odOption.FlightSegments.First().TripIndicator;
            }
            if (string.IsNullOrEmpty(tripIndicator))
            {
                var flightSeg = flightSegments.First(od => od.SegmentNumber == Convert.ToInt32(subProduct.Association.SegmentRefIDs.First()));
                tripIndicator = flightSeg.TripIndicator;
            }
            return tripIndicator;
        }

        private string GetOriginDestinationDesc(Service.Presentation.ProductModel.SubProduct subProduct, Collection<Service.Presentation.ProductModel.ProductOriginDestinationOption> ODOptions, Collection<ProductFlightSegment> flightSegments)
        {
            if (ODOptions != null && ODOptions.Count > 0)
            {
                var odOption = ODOptions.First(od => od.ID == subProduct.Association.ODMappings.First().RefID);
                return string.Concat(odOption.FlightSegments.First().DepartureAirport.IATACode, " - ", odOption.FlightSegments.Last().ArrivalAirport.IATACode);
            }
            var flightSeg = flightSegments.First(od => od.SegmentNumber == Convert.ToInt32(subProduct.Association.SegmentRefIDs.First()));
            return string.Concat(flightSeg.DepartureAirport.IATACode, " - ", flightSeg.ArrivalAirport.IATACode);
        }
        private async System.Threading.Tasks.Task AssignSAFContent(MOBSustainableAviationFuel saf, Session session, Service.Presentation.ProductResponseModel.ProductOffer productOffer)
        {
            if(productOffer?.Offers!=null)
            {
                List<CMSContentMessage> lstMessages = await GetSDLContentByGroupName(new MOBRequest { TransactionId = session.SessionId }, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_MANAGERESOffers_Messages"), "ManageReservation_Offers_CMSContentMessagesCached_StaticGUID");
                if (lstMessages != null)
                {
                    var safdescription = lstMessages.First(msg => msg.Title == "MOB_SAF_TileInfo");
                    saf.Description = new MOBSection();
                    saf.Description.Text1 = safdescription.Headline;
                    saf.Description.Text2 = safdescription.ContentFull;
                }
            }         
        }

        private async System.Threading.Tasks.Task<MOBOnScreenAlert> GenerateUnsavedSeatChangeAlertContent(Session session)
        {
            MOBOnScreenAlert unSavedSeatChangeAlert = null;
            
            List<CMSContentMessage> lstMessages = await GetSDLContentByGroupName(new MOBRequest { TransactionId = session.SessionId }, session.SessionId, session.Token, _configuration.GetValue<string>("CMSContentMessages_GroupName_VIEWRESSeatMap_Messages"), "ViewRes_SeatMap_CMSContentMessagesCached_StaticGUID");

            if (lstMessages != null)
            {
                var contentMsg = lstMessages.FirstOrDefault(msg => msg.Title == "MR.UnsavedSeatChanges.ContentMsg");
                var saveButtonContent = lstMessages.FirstOrDefault(msg => msg.Title == "MR.UnsavedSeatChanges.SaveButton");
                var continueButtonContent = lstMessages.FirstOrDefault(msg => msg.Title == "MR.UnsavedSeatChanges.ContinueButton");

                unSavedSeatChangeAlert = new MOBOnScreenAlert();
                unSavedSeatChangeAlert.Title = contentMsg?.ContentShort;
                unSavedSeatChangeAlert.Message = contentMsg?.ContentFull;
                unSavedSeatChangeAlert.Actions = new List<MOBOnScreenActions>()
                    {
                        new MOBOnScreenActions()
                        {
                             ActionTitle = saveButtonContent?.ContentFull,
                        },
                        new MOBOnScreenActions()
                        {
                             ActionTitle = continueButtonContent?.ContentFull,
                        }
                    };
            }

            return unSavedSeatChangeAlert;
        }

        public async Task<MOBSeatChangeInitializeResponse> SeatChangeInitialize(MOBSeatChangeInitializeRequest request)
        {
            MOBSeatChangeInitializeResponse response = new MOBSeatChangeInitializeResponse(_configuration);

            response.SessionId = request.SessionId;
            response.Request = request;
            response.Flow = request.Flow;
            bool isOneofTheSegmentSeatMapShownForMultiTripPNRMRes = false;

            //response.Request.Application.Version.Build = null;
            //response.Request.Application.Version.DisplayText = null;

            _logger.LogInformation("SeatChangeInitialize {Request} {SessionId} {ApplicationId} {ApplicationVersion} and {DeviceId}", JsonConvert.SerializeObject(request), request.SessionId, request.Application.Id, request.Application.Version.Major, request.DeviceId);

            Session session = null;
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);

                if (!_configuration.GetValue<bool>("DisableManageResChanges23C"))
                {
                    if (session.CatalogItems == null) session.CatalogItems = new List<MOBItem>();
                    if (request.CatalogValues != null && request.CatalogValues.Count > 0)
                    {
                        session.CatalogItems.AddRange(request.CatalogValues);
                        await _sessionHelperService.SaveSession<Session>(session, session.SessionId, new List<string> { session.SessionId, session.ObjectName }, session.ObjectName);
                    }
                }
                session.Flow = request.Flow;
            }
            else
            {
                _logger.LogInformation("SeatChangeInitialize : CFOP - Session Null {SessionId} {ApplicationId} {ApplicationVersion} {DeviceId} and {Request}", request.SessionId, request.Application.Id, request.Application.Version.Major, request.DeviceId, request);
                if (ConfigUtility.VersionCheck_NullSession_AfterAppUpgradation(request))
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOP_NullSession_AfterAppUpgradation).ToString(), _configuration.GetValue<string>("CFOPViewRes_ReloadAppWhileUpdateMessage").ToString());
                else
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOPSessionExpire).ToString(), _configuration.GetValue<string>("CFOPViewRes_ReloadAppWhileUpdateMessage").ToString());
            }

            try
            {
                if (ConfigUtility.EnableUMNRInformation(request.Application.Id, request.Application.Version.Major))
                {
                    var eligibility = new EligibilityResponse();
                    eligibility = await _sessionHelperService.GetSession<EligibilityResponse>(request.SessionId, eligibility.ObjectName, new List<string> { request.SessionId, eligibility.ObjectName }).ConfigureAwait(false);
                    if (eligibility != null && eligibility.IsUnaccompaniedMinor)
                    {
                        if (response.Segments == null) response.Segments = new List<TripSegment>() { };
                        response.Exception = new MOBException { Code = "9999", Message = _configuration.GetValue<string>("umnr_seat_message") };
                        return response;
                    }
                }

                if (GeneralHelper.ValidateAccessCode(request.AccessCode))
                {
                    if (ConfigUtility.IsVerticalSeatMapCatalogEnabled(request.CatalogValues))
                    {
                        response.IsVerticalSeatMapEnabled = await _featureSettings.GetFeatureSettingValue("EnableVerticalSeatmap") && ConfigUtility.IsVerticalSeatMapEnabledAppVersion(request.Application.Id, request.Application.Version.Major);
                    }

                    var seatChangeDAL1 = new United.Mobile.Model.MPRewards.SeatEngine();
                    var seatChangeDAL = await _seatEngine.GetFlightReservationCSL_CFOP(request, seatChangeDAL1, response.IsVerticalSeatMapEnabled);

                    response.SelectedTrips = seatChangeDAL.SelectedTrips;
                    response.BookingTravlerInfo = seatChangeDAL.BookingTravelerInfo;
                    response.Segments = seatChangeDAL.Segments;
                    response.IsVerticalSeatMapEnabled = seatChangeDAL.IsVerticalSeatMapEnabled;
                   
                    if (request.RecordLocator.IsNullOrEmpty())
                    {
                        request.RecordLocator = seatChangeDAL.RecordLocator;
                        request.LastName = seatChangeDAL.LastName;
                    }

                    bool isByPassVIEWRES24HrsCheckinWindow
                           = (_configuration.GetValue<bool>("IsByPassVIEWRES24HrsCheckinWindow") && request.Flow == Convert.ToString(FlowType.VIEWRES));


                    if (!isByPassVIEWRES24HrsCheckinWindow && response.Segments != null && response.Segments.Count == 1 && !string.IsNullOrEmpty(response.Segments[0].CheckInWindowText) && response.Segments[0].IsCheckInWindow)
                    {
                        if (ConfigUtility.EnableNewChangeSeatCheckinWindowMsg(request.Application.Id, request.Application.Version.Major))
                        {
                            if (response.Segments[0].IsCheckInWindow)
                                response.IsCheckedInChangeSeatEligible = "1";
                            else
                                response.IsCheckedInChangeSeatEligible = "0";

                            response.ContinueButtonText = response.Segments[0].ContinueButtonText;
                        }

                        throw new MOBUnitedException(response.Segments[0].CheckInWindowText);

                    }

                    //Load and save PCU for enabling upper cabin seats after migration to CCE
                    if (_shoppingUtility.IsEnableTravelOptionsInViewRes(request.Application.Id, request?.Application?.Version?.Major, session?.CatalogItems) && request.Flow != FlowType.VIEWRES_BUNDLES_SEATMAP.ToString())
                    {
                        try
                        {
                            bool hasPCUOffer = await _seatEngine.HasPCUOfferState(request);
                            if (!hasPCUOffer)
                            {
                                TravelOptionsRequest travelOptionsRequest = _seatEngine.BuildTravelOptionsRequest(request);
                                United.Service.Presentation.ReservationResponseModel.ReservationDetail reservationDetail = await GetCslReservation(request.SessionId);
                                await GetDODResponseFromCCE(travelOptionsRequest, session, reservationDetail, true);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    response.ExitAdvisory = UtilityHelper.GetExitAdvisory();
                    if (!ConfigUtility.EnableSSA(request.Application.Id, request.Application.Version.Major))
                    {
                       _seatEngine.CheckSegmentToRaiseExceptionForElf(response.Segments);
                    }

                    int totalEplusEligible = 0;
                    int ePlusSubscriberCount = 0;
                    if (response.Segments != null && response.Segments.Count > 0 && !(UtilityHelper.HasAllElfSegments(response.Segments) || UtilityHelper.HasAllIBE(response.Segments) || response.Segments.TrueForAll(s => ConfigUtility.IsSeatMapSupportedOa(s.OperatingCarrier, s.MarketingCarrier))))
                    {
                        bool hasEliteAboveGold = false;
                        bool doNotShowEPlusSubscriptionMessage = false;
                        bool showEPUSubscriptionMessage = false;
                        bool isEnablePreferredZoneSubscriptionMessages = _configuration.GetValue<bool>("isEnablePreferredZoneSubscriptionMessagesManageRes");
                        if (isEnablePreferredZoneSubscriptionMessages && _seatEngine.HasEconomySegment(response.SelectedTrips))
                        {
                            var tupleRes = await _seatEngine.PopulateEPlusSubscriberSeatMessage(response, request.Application.Id, request.TransactionId, ePlusSubscriberCount, response.IsVerticalSeatMapEnabled, isEnablePreferredZoneSubscriptionMessages);
                            totalEplusEligible = tupleRes.Item1;
                            response = tupleRes.response;
                            ePlusSubscriberCount = tupleRes.ePlusSubscriberCount;
                        }
                        else
                        {
                            int noFreeSeatCompanionCount = _seatEngine.GetNoFreeSeatCompanionCount(response.BookingTravlerInfo, response.SelectedTrips);

                            var tupleRestotalEplusEligible = await _seatEngine.PopulateEPlusSubscriberAndMPMemeberSeatMessage(response, request.Application.Id, request.TransactionId, ePlusSubscriberCount, hasEliteAboveGold, doNotShowEPlusSubscriptionMessage, showEPUSubscriptionMessage);
                            totalEplusEligible = tupleRestotalEplusEligible.Item1;
                            response = tupleRestotalEplusEligible.response;
                            ePlusSubscriberCount = tupleRestotalEplusEligible.ePlusSubscriberCount;
                            hasEliteAboveGold = tupleRestotalEplusEligible.hasEliteAboveGold;
                            doNotShowEPlusSubscriptionMessage = tupleRestotalEplusEligible.doNotShowEPlusSubscriptionMessage;
                            showEPUSubscriptionMessage = tupleRestotalEplusEligible.showEPUSubscriptionMessage;

                            if (ePlusSubscriberCount == 0)
                            {
                                doNotShowEPlusSubscriptionMessage = true;
                                if (response.BookingTravlerInfo != null && response.BookingTravlerInfo.Count > 0 && noFreeSeatCompanionCount > 0)
                                {
                                    this._seatEngine.PopulateEPAEPlusSeatMessage(ref response, noFreeSeatCompanionCount, ref doNotShowEPlusSubscriptionMessage);
                                }
                            }
                        }
                    }

                    if (!_configuration.GetValue<bool>("DisableSaveSessionDataNullFix") && _configuration.GetValue<bool>("FixNullReferenceExceptionInSeatChangeSelectSeatsAction"))
                        await _sessionHelperService.SaveSession<List<MOBSeatMap>>(null, request.SessionId, new List<string> { request.SessionId, ObjectNames.MOBSeatMapListFullName }, ObjectNames.MOBSeatMapListFullName, 5400, false);
                    else
                        await _sessionHelperService.SaveSession<List<MOBSeatMap>>(null, request.SessionId, new List<string> { request.SessionId, ObjectNames.MOBSeatMapListFullName }, ObjectNames.MOBSeatMapListFullName, 5400, true);

                    bool isSeatFocusShareIndexEnabled = (_configuration.GetValue<bool>("IsSeatNumberClickableEnabled")
                        && request.SeatFocusRequest != null && !string.IsNullOrEmpty(request.SeatFocusRequest.Destination) && !string.IsNullOrEmpty(request.SeatFocusRequest.Origin) && !string.IsNullOrEmpty(request.SeatFocusRequest.FlightNumber));


                    if (_configuration.GetValue<bool>("EnableSocialDistanceMessagingForSeatMap"))
                    {
                        if (response != null && response.SelectedTrips != null && response.SelectedTrips.Any())
                        {
                            foreach (United.Mobile.Model.Shopping.Booking.MOBBKTrip trip in response.SelectedTrips)
                            {
                                foreach (United.Mobile.Model.Shopping.Booking.MOBBKFlattenedFlight ff in trip.FlattenedFlights)
                                {
                                    foreach (United.Mobile.Model.Shopping.Booking.MOBBKFlight flight in ff.Flights)
                                    {
                                        flight.ShowEPAMessage = false;
                                        flight.EPAMessageTitle = "";
                                        flight.EPAMessage = "";
                                    }
                                }
                            }
                        }
                    }

                    string cartId = null;

                    if (_configuration.GetValue<bool>("EnableSeatMapCartIdFix"))
                    {
                        MOBShoppingCart persistShoppingCart = new MOBShoppingCart();

                        if (request.Flow == FlowType.VIEWRES_BUNDLES_SEATMAP.ToString())
                        {
                            var shoppingCart = _sessionHelperService.GetSession<MOBShoppingCart>(request.SessionId, persistShoppingCart.ObjectName, new List<string> { request.SessionId, persistShoppingCart.ObjectName }).Result;
                            cartId = shoppingCart.CartId;
                        }

                        if (string.IsNullOrEmpty(cartId))
                        {
                            cartId = await CreateCart(request, session);
                            persistShoppingCart.CartId = cartId;
                            await _sessionHelperService.SaveSession<MOBShoppingCart>(persistShoppingCart, session.SessionId, new List<string> { session.SessionId, persistShoppingCart.ObjectName }, persistShoppingCart.ObjectName);
                        }
                    }

                    TripSegment tripSegment = null;
                    bool isOADeepLinkSupportedOneSegment = false;
                    bool isOaSeatMapSegment = false;
                    bool isDeepLink = false;
                    bool isLandTransport = false;


                    bool isSeatMapSignInUserDataChangeEnabled = await _featureSettings.GetFeatureSettingValue("EnableSeatMapSignInUserDataChange");
                    bool isOANoSeatMapAvailableNewMessageEnabled = await _featureSettings.GetFeatureSettingValue("EnableNewOASeatMapUnavailableMessage");

                        if (response.Segments != null && response.Segments.Count == 1 || request.Flow == FlowType.VIEWRES_SEATMAP.ToString() || isSeatFocusShareIndexEnabled
                        || (response.IsVerticalSeatMapEnabled && response.Segments != null && response.Segments.Count > 0))
                    {
                        United.Mobile.Model.MPRewards.OfferRequestData requestedFlightSegment = null;
                        if (request.OffersRequestData != null)
                        {
                            requestedFlightSegment = JsonConvert.DeserializeObject<United.Mobile.Model.MPRewards.OfferRequestData>(request.OffersRequestData);
                        }


                        if (isSeatFocusShareIndexEnabled && request.Flow != FlowType.VIEWRES_SEATMAP.ToString())
                        {
                            tripSegment = (request.SeatFocusRequest != null)
                                                    ? response.Segments.FirstOrDefault(s => _seatEngine.IsMatchedFlight(s, request.SeatFocusRequest, response.Segments))
                                                    : response.Segments.FirstOrDefault();
                        }
                        else
                        {

                            tripSegment = (request.Flow == FlowType.VIEWRES_SEATMAP.ToString() && requestedFlightSegment != null)
                                                        ? response.Segments.FirstOrDefault(s => _seatEngine.IsMatchedFlight(s, requestedFlightSegment, response.Segments))
                                                        : response.IsVerticalSeatMapEnabled ?
                                                          response.Segments.FirstOrDefault(s => s.ShowSeatChange)
                                                          : response.Segments.FirstOrDefault();
                        }


                        List<United.Mobile.Model.Shopping.MOBSeatMap> seatMap = null;
                        int noOfTravelersWithNoSeat;
                        int noOfFreeEplusEligibleRemaining = _seatEngine.GetNoOfFreeEplusEligibleRemaining(response.BookingTravlerInfo, tripSegment.Departure.Code, tripSegment.Arrival.Code, totalEplusEligible, tripSegment.IsELF, out noOfTravelersWithNoSeat);
                        bool hideSeatMap = false;
                        
                        if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major))
                        {
                            bool isInterLine = _shoppingUtility.IsInterLine(tripSegment.OperatingCarrier, tripSegment.MarketingCarrier);
                            bool isOperatedByOtherAirlines = _shoppingUtility.IsOperatedByOtherAirlines(tripSegment.OperatingCarrier, tripSegment.MarketingCarrier, tripSegment.Equipment?.Code);
                            isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(tripSegment.OperatingCarrier);
                            hideSeatMap = (isOperatedByOtherAirlines && !isDeepLink);
                            isOaSeatMapSegment = ((isInterLine || isOperatedByOtherAirlines));
                        }
                        else if (_shoppingUtility.EnableOAMessageUpdate(request.Application.Id, request.Application.Version.Major))
                        {
                            isOaSeatMapSegment = _shoppingUtility.IsInterLine(tripSegment.OperatingCarrier, tripSegment.MarketingCarrier);
                            bool isOperatedByOtherAirlines = _shoppingUtility.IsOperatedByOtherAirlines(tripSegment.OperatingCarrier, tripSegment.MarketingCarrier, tripSegment.Equipment?.Code);
                            isDeepLink = ConfigUtility.IsDeepLinkSupportedAirline(tripSegment.OperatingCarrier);
                            if (isOperatedByOtherAirlines && !isDeepLink)
                            {
                                string OANoSeatMapAvailableMessage = isOANoSeatMapAvailableNewMessageEnabled ? await _seatMapCSL30.GetOANoSeatMapAvailableNewMessage(session) : _configuration.GetValue<string>("OANoSeatMapAvailableMessageBody");
                                throw new MOBUnitedException(HttpUtility.HtmlDecode(OANoSeatMapAvailableMessage));
                            }
                        }
                        else
                        {
                            isOaSeatMapSegment = ConfigUtility.IsSeatMapSupportedOa(tripSegment.OperatingCarrier, tripSegment.MarketingCarrier);
                        }

                        bool isSeatFocusEnabled = isSeatFocusShareIndexEnabled || (request.Flow == FlowType.VIEWRES_SEATMAP.ToString() && requestedFlightSegment != null && requestedFlightSegment.FocusRequestData != null);
                        int segmentIndex = isSeatFocusEnabled ? tripSegment.OriginalSegmentNumber : tripSegment.SegmentIndex;
                        if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major) && hideSeatMap)
                        {
                            seatMap = null;
                        }
                        else
                        {
                            seatMap = await _seatMapCSL30.GetCSL30SeatMapForRecordLocatorWithLastName(request.SessionId, request.RecordLocator,
                                    segmentIndex, request.LanguageCode, tripSegment.ServiceClassDescription,
                                    request.LastName, tripSegment.COGStop, tripSegment.Departure.Code, tripSegment.Arrival.Code,
                                    request.Application.Id, request.Application.Version.Major, tripSegment.IsELF, tripSegment.IsIBE,
                                    noOfTravelersWithNoSeat, noOfFreeEplusEligibleRemaining, isOaSeatMapSegment, response.Segments,
                                    tripSegment.OperatingCarrier, request.TransactionId, response.BookingTravlerInfo, request.Flow,
                                    response.IsVerticalSeatMapEnabled, isOANoSeatMapAvailableNewMessageEnabled, ePlusSubscriberCount, request.TravelerSignInData, isSeatMapSignInUserDataChangeEnabled,
                                    isOneofTheSegmentSeatMapShownForMultiTripPNRMRes, isSeatFocusEnabled, request.CatalogValues, cartId: cartId);
                        }
                        if (_shoppingUtility.EnableOAMessageUpdate(request.Application.Id, request.Application.Version.Major)
                            && (seatMap == null || (seatMap != null && seatMap.Count > 0 && seatMap.FirstOrDefault().HasNoComplimentarySeatsAvailableForOA))
                            && ((_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major) && isOaSeatMapSegment)
                                || ConfigUtility.IsDeepLinkSupportedAirline(tripSegment.OperatingCarrier, request.Application.Id, request.Application.Version.Major, request.CatalogValues)))
                        {
                            isOADeepLinkSupportedOneSegment = true;
                        }
                        else if (!_shoppingUtility.EnableOAMessageUpdate(request.Application.Id, request.Application.Version.Major) && isOaSeatMapSegment && ConfigUtility.IsDeepLinkSupportedAirline(tripSegment.OperatingCarrier, request.Application.Id, request.Application.Version.Major, request.CatalogValues) && (seatMap == null || (seatMap != null && seatMap.Count > 0 && seatMap.FirstOrDefault().HasNoComplimentarySeatsAvailableForOA)))
                        {
                            isOADeepLinkSupportedOneSegment = true;
                        }

                        if (seatMap != null && seatMap.Count == 1 && seatMap[0] != null && seatMap[0].IsOaSeatMap)
                        {
                            seatMap[0].OperatedByText = _seatEngine.GetOperatedByText(tripSegment.MarketingCarrier, tripSegment.FlightNumber, tripSegment.OperatingCarrierDescription);
                            if (_shoppingUtility.EnableOAMessageUpdate(request.Application.Id, request.Application.Version.Major))
                            {
                                var operatingCarrierDescription = UtilityHelper.RemoveString(tripSegment?.OperatingCarrierDescription, "Limited");
                                if (!string.IsNullOrEmpty(operatingCarrierDescription) && !string.IsNullOrEmpty(tripSegment?.MarketingCarrier))
                                {
                                    seatMap[0].ShowInfoTitleForOA = String.Format(_configuration.GetValue<string>("SeatMapMessageForEligibleOATitle"), tripSegment.MarketingCarrier, tripSegment.FlightNumber, operatingCarrierDescription);
                                }
                            }
                        }
                        if (seatChangeDAL.Seats != null)
                        {
                            IEnumerable<United.Mobile.Model.Shopping.Misc.Seat> seatList = from s in seatChangeDAL.Seats
                                                                                           where s.Origin == tripSegment.Departure.Code
                                                                                           && s.Destination == tripSegment.Arrival.Code
                                                                                           select s;

                            if (seatList.Count() > 0)
                            {
                                response.Seats = seatList.ToList();
                            }
                        }

                        if (response.IsVerticalSeatMapEnabled)
                        {
                            isLandTransport = _shoppingUtility.IsLandTransport(tripSegment.Equipment?.Code);

                            if (isLandTransport && (seatMap == null || (seatMap.Count == 1 && seatMap[0] == null)))
                            {
                                tripSegment.ShowInterlineAdvisoryMessage = true;
                                tripSegment.InterlineAdvisoryAlertTitle = _configuration.GetValue<string>("SelectSeats_BusServiceErrorTitle");
                                tripSegment.InterlineAdvisoryMessage = _configuration.GetValue<string>("SelectSeats_BusServiceError");
                            }
                        }

                        bool isPreferredZoneEnabled = ConfigUtility.EnablePreferredZone(request.Application.Id, request.Application.Version.Major);

                        response.SeatMap = _seatEngine.GetSeatMapWithPreAssignedSeats(seatMap, response.Seats, isPreferredZoneEnabled);

                        bool bookingTravelerOldSeatLocationEnabled = await _featureSettings.GetFeatureSettingValue("EnableBookingTravelerOldSeatLocation");

                        if (response.IsVerticalSeatMapEnabled && bookingTravelerOldSeatLocationEnabled)
                        {
                            response.BookingTravlerInfo = _seatEngine.GetBookingTravelerInfoWithSeatLocation(seatMap, response.BookingTravlerInfo);
                        }                       

                        await _sessionHelperService.SaveSession<List<United.Mobile.Model.Shopping.MOBSeatMap>>(response.SeatMap, request.SessionId, new List<string> { request.SessionId, ObjectNames.MOBSeatMapListFullName }, ObjectNames.MOBSeatMapListFullName);
                    }

                    if (isByPassVIEWRES24HrsCheckinWindow)
                    {
                        response?.SelectedTrips?.ForEach(trip =>
                        {
                            trip?.FlattenedFlights?.ForEach(flight =>
                            {
                                flight?.Flights?.ForEach(fflight =>
                                {
                                    fflight.CheckInWindowText = string.Empty;
                                    fflight.IsCheckInWindow = false;
                                });
                            });
                        });
                    }

                    var state = new SeatChangeState
                    {
                        RecordLocator = request.RecordLocator,
                        LastName = request.LastName,
                        SessionId = response.SessionId,
                        Seats = seatChangeDAL.Seats,
                        BookingTravelerInfo = response.BookingTravlerInfo,
                        Trips = response.SelectedTrips,
                        PNRCreationDate = seatChangeDAL.PNRCreationDate,
                        TotalEplusEligible = totalEplusEligible,
                        Segments = response.Segments,
                    };

                    if (_configuration.GetValue<bool>("EnableSeatMapCartIdFix"))
                    {
                        state.CartId = cartId;
                    }

                    if (isSeatMapSignInUserDataChangeEnabled)
                    {
                        state.TravelerSignInData = request.TravelerSignInData;
                    }

                    if (response.IsVerticalSeatMapEnabled)
                    {
                        bool isUnsavedSeatChangeAlertEnabled = await _featureSettings.GetFeatureSettingValue("EnableUnsavedSeatChangeAlert");

                        if (isUnsavedSeatChangeAlertEnabled)
                        {
                            response.UnsavedSeatChangeAlert = await GenerateUnsavedSeatChangeAlertContent(session);
                        }
                    }

                    _logger.LogInformation("Trace: {SessionId} {ObjectName} {ApplicationId} {ApplicationVersion} {DeviceId} and {SeatChangeState}", request.SessionId, state.GetType().Name.ToString(), request.Application.Id, request.Application.Version.Major, request.DeviceId, state);

                    await _sessionHelperService.SaveSession<SeatChangeState>(state, request.SessionId, new List<string> { request.SessionId, state.ObjectName }, state.ObjectName);
                    await _sessionHelperService.SaveSession<MOBSeatChangeInitializeResponse>(response, request.SessionId, new List<string> { request.SessionId, typeof(MOBSeatChangeInitializeResponse).Name }, typeof(MOBSeatChangeInitializeResponse).Name);

                    if (isOADeepLinkSupportedOneSegment)
                    {
                        if (_shoppingUtility.EnableOAMsgUpdateFixViewRes(request.Application.Id, request.Application.Version.Major) && !isDeepLink)
                        {
                            await _seatMapCSL30.GetOANoSeatAvailableMessage(response?.Segments, isOANoSeatMapAvailableNewMessageEnabled, session);
                        }
                        else
                        {
                            _seatMapCSL30.GetInterlineRedirectLink(response?.Segments, seatChangeDAL.PointOfSale, request, request.RecordLocator, request.LastName, request.CatalogValues);
                        }
                    }
                }
                else
                {
                    throw new MOBUnitedException("The access code you provided is not valid.");
                }
                if (request.Flow == FlowType.VIEWRES_SEATMAP.ToString() && response.SeatMap == null)
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("ManageResSeatmapUnavailable"));
                }

                if (!_seatEngine.ValidateResponse(response))
                {
                    _logger.LogInformation("SeatChangeInitialize {ValidateResponse} {SessionId} {ObjectName} {ApplicationId} {ApplicationVersion} and {DeviceId}", "Seat change response validation failed - all flattenedFlights are empty", request.SessionId, response.GetType().Name.ToString(), request.Application.Id, request.Application.Version.Major, request.DeviceId);

                    throw new MOBUnitedException(_configuration.GetValue<string>("ManageResSeatmapUnavailable_ResponseValidationFailed"));
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("SeatChangeInitialize Warning {exception} {exceptionstack} and {transactionId}", uaex.Message, JsonConvert.SerializeObject(uaex), request.TransactionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("SeatChangeInitialize Error {exception} {exceptionstack} and {transactionId}", ex.Message, JsonConvert.SerializeObject(ex), request.TransactionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            try
            {
                if (await _shoppingUtility.IsEnableCCEFeedBackIntrestedEventForE01OfferTile(request).ConfigureAwait(false) && session !=null)
                {
                    await _merchandizingServices.SendEplusStandAloneOfferFeedbackToCCE(request, session?.Token);
                }
            }
            catch
            { }
            return response;


        }
        private async Task<string> CreateCart(MOBRequest request, Session session)
        {
            string response = string.Empty;

            try
            {
                string jsonRequest = JsonConvert.SerializeObject(string.Empty);
                string cslResponse = await _shoppingCartService.CreateCart(session.Token, jsonRequest, session.SessionId).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(cslResponse))
                {
                    response = (cslResponse);
                }
            }
            catch (System.Exception ex)
            {
                response = null;
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                throw new System.Exception(exceptionWrapper.Message.ToString());
            }

            return response;
        }
    }
}
