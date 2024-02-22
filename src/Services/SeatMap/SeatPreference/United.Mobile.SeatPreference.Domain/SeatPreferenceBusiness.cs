using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UAWSFlightReservation;
using United.Common.Helper.Shopping;
using United.Csl.Common;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.SeatPreference;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.SeatMap;
using United.Services.FlightShopping.Common.Extensions;
using United.Services.FlightShopping.Common.MapViewSearch;
using United.Utility.AppVersion;
using United.Utility.Helper;
using Passenger = United.Mobile.Model.SeatMap.Passenger;

namespace United.Mobile.SeatPreference.Domain
{
    public class SeatPreferenceBusiness : ISeatPreferenceBusiness
    {
        private readonly ICacheLog<SeatPreferenceBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IDPService _dpTokenService;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ICustomerPreferenceService _customerPreferenceService;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IUpdateTravelersInformation _updateTravelersInformation;
        public SeatPreferenceBusiness(
            ICacheLog<SeatPreferenceBusiness> logger
            , IConfiguration configuration
            , IDPService dPTokenService
            , IShoppingSessionHelper shoppingSessionHelper
            , IDynamoDBService dynamoDBService
            , ICustomerPreferenceService customerPreferenceService
            , ISessionHelperService sessionHelperService
            , IUpdateTravelersInformation updateTravelersInformation
            )
        {
            _customerPreferenceService = customerPreferenceService;
            _logger = logger;
            _configuration = configuration;
            _dpTokenService = dPTokenService;
            _shoppingSessionHelper = shoppingSessionHelper;
            _dynamoDBService = dynamoDBService;
            _sessionHelperService = sessionHelperService;
            _updateTravelersInformation = updateTravelersInformation;
        }


        public async Task<PersistSeatPreferenceResponse> GetSeatPreferencefromCSL(PersistSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode)
        {
            var preferenceResponse = new PersistSeatPreferenceResponse
            {
                RecordLocator = request.RecordLocator,
                LastName = request.LastName
            };

            Session session = null;
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
            }

            if (session == null && accessCode != "ACCESSCODE")
            {
                _logger.LogInformation("GetSeatPreference - Unauthorized Request {Request}", request);

                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            try
            {
                var seatMapDynamoDB = new SeatMapDynamoDB(_configuration, _dynamoDBService);
                var captions = seatMapDynamoDB.GetSeatPreference<List<MOBItem>>("MR_SeatPreference", request.SessionId).Result.ToDictionary(s => s.Id, s => s.CurrentValue);

                preferenceResponse.Captions = captions;

                preferenceResponse.SeatPreferences = new List<SeatPreferenceOption>
                {
                    new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Radio.ToString(),
                        Title = captions["Preference_Seat_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                 IsSelected = false,
                                 PreferenceValue = PreferenceOption.Window.ToString(),
                                 DisplayText = captions["Preference_Seat_DisplayText_Window"]
                            },
                            new PreferenceDetail
                            {
                                 IsSelected = false,
                                 PreferenceValue = PreferenceOption.Aisle.ToString(),
                                 DisplayText = captions["Preference_Seat_DisplayText_Aisle"]
                            },
                        }
                    }
                };

                var isOtherSeatPreferenceOptionsEnabled = _configuration.GetValue<bool>("EnableOtherSeatPreferenceOptions");
                var isOtherSeatPreferenceCaptionsExist = captions.ContainsKey("Other_Seat_Preferences_Title");

                if (isOtherSeatPreferenceOptionsEnabled && isOtherSeatPreferenceCaptionsExist && (IsBasicEconomyPnr(request) == false))
                {
                    var otherSeatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Checkbox.ToString(),
                        Title = captions["Other_Seat_Preferences_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                 IsSelected = false,
                                 PreferenceValue = PreferenceOption.MiddleSeatWithExtraLegRoom.ToString(),
                                 DisplayText = captions["Other_Seat_Preferences_MiddleSeat_With_ExtraLegRoom"]
                            },
                            new PreferenceDetail
                            {
                                 IsSelected = false,
                                 PreferenceValue = PreferenceOption.AvoidExitRowSeats.ToString(),
                                 DisplayText = captions["Other_Seat_Preferences_Avoid_ExitRows"]
                            },
                            new PreferenceDetail
                            {
                                 IsSelected = false,
                                 PreferenceValue = PreferenceOption.AvoidBulkheadSeats.ToString(),
                                 DisplayText =  captions["Other_Seat_Preferences_Avoid_Bulkhead"],
                                 DisplaySubText = captions["Other_Seat_Preferences_Avoid_Bulkhead_Seat_Desc"]
                            },
                        }
                    };
                    preferenceResponse.SeatPreferences.Add(otherSeatPreferenceOption);
                }

                //var token = await _dpTokenService.GetTokenFromAWSDPAsync(loggingContext, appId, deviceId);
                var token = await _dpTokenService.GetAnonymousToken(appId, deviceId, _configuration);

                preferenceResponse = await GetSeatPreferenceFromCslAws(request, preferenceResponse, token, transactionId).ConfigureAwait(false);

                //apply default when nothing is selected
                foreach (var selectedPref in preferenceResponse.SeatPreferences)
                {
                    if (selectedPref.OptionType == ButtonType.Radio.ToString() && !selectedPref.Details.Any(a => a.IsSelected))
                    {
                        selectedPref.Details.FirstOrDefault().IsSelected = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception - GetSeatPreference Exception {@Message} and {StackTrace}", ex.Message, ex.StackTrace.ToString());

                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            return preferenceResponse;
        }

        private async Task<PersistSeatPreferenceResponse> GetSeatPreferenceFromCslAws(PersistSeatPreferenceRequest request, PersistSeatPreferenceResponse preferenceResponse, string token, string transactionId)
        {


            var jsonResponse = await _customerPreferenceService.GetAsync(token, request.RecordLocator, transactionId).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var response = JsonConvert.DeserializeObject<PreferencesResult<PersistCustomerPreferences>>(jsonResponse);
                if (response != null && response.Data != null && !string.IsNullOrEmpty(response.Data.RecordLocator) && response.Data.Travelers.Count > 0)
                {
                    var cslReferences = response.Data?.Travelers?.FirstOrDefault()?.PreferenceSegments?.FirstOrDefault()?.Preferences;
                    if (cslReferences != null)
                    {
                        foreach (var cslPreference in cslReferences)
                        {
                            if (cslPreference.PreferenceType == PreferenceType.Seat)
                            {
                                switch (cslPreference.PreferenceValue)
                                {
                                    case PreferenceOption.Window:
                                    case PreferenceOption.Aisle:
                                    case PreferenceOption.AvoidExitRowSeats:
                                    case PreferenceOption.AvoidBulkheadSeats:
                                    case PreferenceOption.MiddleSeatWithExtraLegRoom:
                                        //SetSelectedPreference(preferenceResponse, cslPreference.PreferenceValue);
                                        var detail = preferenceResponse?.SeatPreferences?
                                            .SelectMany(sp => sp.Details)?
                                            .FirstOrDefault(pd => pd.PreferenceValue == cslPreference.PreferenceValue.ToString());
                                        if (detail != null)
                                            detail.IsSelected = true;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }

            return preferenceResponse;
        }

        public async Task<PostSeatPreferenceResponse> SaveSeatPreferencetToCSL(PostSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode)
        {
            var reponse = new PostSeatPreferenceResponse();

            try
            {
                Session session = null;
                if (!string.IsNullOrEmpty(request.SessionId))
                {
                    session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                }

                if (session == null && accessCode != "ACCESSCODE")
                {
                    _logger.LogInformation("SaveSeatPreference - Unauthorized Request {Request}", request);

                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                var cslRequest = new PersistCustomerPreferences
                {
                    RecordLocator = request.RecordLocator,
                    Travelers = new List<Model.SeatMap.Traveler>()
                };

                var preferenceSegments = new List<PreferenceSegment>();
                foreach (var resSeg in request.Segments)
                {
                    var prefSeg = new PreferenceSegment
                    {
                        CarrierCode = resSeg.MarketingCarrier.Code,
                        DepartureDate = Convert.ToDateTime(resSeg.FlightDate),
                        Destination = resSeg.Arrival.Code,
                        FlightNumber = Convert.ToInt32(resSeg.FlightNumber),
                        Origin = resSeg.Departure.Code,

                        Preferences = new List<Preferences>()
                    };

                    foreach (var userPreference in request.SeatPreferences)
                    {
                        PreferenceType type;
                        PreferenceOption value;
                        Enum.TryParse<PreferenceType>(userPreference.PreferenceType, out type);
                        Enum.TryParse<PreferenceOption>(userPreference.PreferenceValue, out value);
                        var pref = new Preferences
                        {
                            PreferenceType = type,
                            PreferenceValue = value
                        };

                        prefSeg.Preferences.Add(pref);
                    }

                    preferenceSegments.Add(prefSeg);
                }

                var passenger = request.Segments.SelectMany(p => p.Passengers).FirstOrDefault();

                //foreach (var pax in passengers)
                //{
                var traveler = new Model.SeatMap.Traveler
                {
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    PreferenceSegments = preferenceSegments
                };
                cslRequest.Travelers.Add(traveler);
                //}

                //var token = await _dpTokenService.GetTokenFromAWSDPAsync(loggingContext, appId, deviceId);
                var token = await _dpTokenService.GetAnonymousToken(appId, deviceId, _configuration);

                reponse = await PostSeatPreferenceToCslAws(request, reponse, cslRequest, token, transactionId);

            }
            catch (Exception ex)
            {
                _logger.LogError("SeatPreferenceBusiness - Exception occurred in Save SeatPreferencetToCSL {@Message} and {@StackTrace}", ex.Message, ex.StackTrace);
                throw ex;
            }

            return reponse;
        }

        private async Task<PostSeatPreferenceResponse> PostSeatPreferenceToCslAws(PostSeatPreferenceRequest request, PostSeatPreferenceResponse reponse, PersistCustomerPreferences cslRequest, string token, string transactionId)
        {

            try
            {


                var cslRequestJson = JsonConvert.SerializeObject(cslRequest);
                var jsonResponse = await _customerPreferenceService.SaveAsync(token, cslRequestJson, transactionId);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var responseObj = JsonConvert.DeserializeObject<PreferencesResult<PersistCustomerPreferencesResponse>>(jsonResponse);

                    if (responseObj != null)
                        if (responseObj.ErrorNumber == 0 && responseObj.Data != null && responseObj.Data.RecordLocator.Equals(request.RecordLocator, StringComparison.OrdinalIgnoreCase))
                            reponse.IsSuccess = true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SeatPreferenceBusiness - Exception occurred in Post SeatPreferenceToCslAws {@exception} and {@stacktrace}", ex.Message, ex.StackTrace);

                throw ex;
            }

            return reponse;
        }

        #region enhanced version 
        public async Task<PersistSeatPreferenceResponse> GetSeatPreferencefromCSLV2(PersistSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode)
        {
            var preferenceResponse = new PersistSeatPreferenceResponse
            {
                RecordLocator = request.RecordLocator,
                LastName = request.LastName,
            };

            Session session = null;
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                var pnr = await _sessionHelperService.GetSession<MOBPNR>(request.SessionId, new MOBPNR().ObjectName, new List<string> { request.SessionId, new MOBPNR().ObjectName }).ConfigureAwait(false);
                if (pnr != null)
                {
                    preferenceResponse.Passengers = pnr.Passengers;
                    if (preferenceResponse.Passengers == null || preferenceResponse.Passengers.Count <= 0)
                    {
                        _logger.LogError("GetSeatPreference - Passengers details is empty from session {Request}", request);
                        throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                    }
                    else
                    {
                        int paxCount = preferenceResponse.Passengers.Count;
                        request.RequestSeatsTogether = request.IsMultiPax = paxCount > 1 ? true : false;
                    }
                    // flight segments
                    if (pnr.Segments != null && pnr.Segments.Count > 0)
                    {
                        var flightSeg = pnr.Segments;
                        List<FlightDetails> flightDetails = new List<FlightDetails>();
                        FlightDetails flightDtls = null;
                        foreach (var flights in flightSeg)
                        {
                            flightDtls = new FlightDetails();
                            flightDtls.FlightDate = flights.ScheduledDepartureDateTime;
                            flightDtls.FlightNumber = flights.FlightNumber;
                            flightDtls.MarketingCarrierCode = flights.MarketingCarrier.Code;
                            flightDtls.scheduledDepartureTime = flights.ScheduledDepartureDateTime;
                            flightDtls.EstimatedDepartureTime = "";
                            flightDetails.Add(flightDtls);
                        }
                        if (flightDetails.Count > 0)
                            request.FlightDetails = flightDetails;
                    }
                    else
                    {
                        _logger.LogError("GetSeatPreference - flight Segments details is empty from session {Request}", request);
                        throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                    }
                }
            }


            if (accessCode != "ACCESSCODE" && session == null)
            {
                _logger.LogError("GetSeatPreference - Unauthorized Request {Request}", request);
                throw new MOBUnitedException(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            try
            {
                var seatMapDynamoDB = new SeatMapDynamoDB(_configuration, _dynamoDBService);
                var captions = seatMapDynamoDB.GetSeatPreference<List<MOBItem>>("MR_SeatPreference", request.SessionId).Result.ToDictionary(s => s.Id, s => s.CurrentValue);

                if (captions == null || captions.Count() == 0)
                {
                    _logger.LogError("GetSeatPreference - Captions not showing in database");

                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                // OnboardingScreenCaptions
                //var onboardingCaptions = captions.Where(c => c.Key.Contains("Onboarding")).Select(c => new OnboardingScreen { Title = c.Key, Content = c.Value }).ToList();
                //preferenceResponse.OnboardingScreenCaptions = onboardingCaptions;

                //OnboardingScreen steps
                OnboardingScreen onBoardStep = new OnboardingScreen()
                {
                    Title = captions.First(a => a.Key.Equals("headerText_Onboarding")).Value.ToString() + " | Step1",
                    ImageURL = captions.First(a => a.Key.Equals("SeatPref_Step1_ImageURL")).Value.ToString(),
                    SubTitle = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step1_SubTitle_SinglePax")).Value.ToString()
                    : captions.First(a => a.Key.Equals("SeatPref_Step1_SubTitle_MultiPax")).Value.ToString(),
                    Content = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step1_Content_SinglePax")).Value.ToString()
                    : captions.First(a => a.Key.Equals("SeatPref_Step1_Content_MultiPax")).Value.ToString()
                };
                preferenceResponse.OnboardingScreenCaptions.Add(onBoardStep);
                onBoardStep = new OnboardingScreen();
                onBoardStep.Title = captions.First(a => a.Key.Equals("headerText_Onboarding")).Value.ToString() + " | Step2";
                onBoardStep.ImageURL = captions.First(a => a.Key.Equals("SeatPref_Step2_ImageURL")).Value.ToString();
                if (!request.IsStandby)
                {
                    onBoardStep.SubTitle = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step2_SubTitle_SinglePax")).Value.ToString()
                          : captions.First(a => a.Key.Equals("SeatPref_Step2_SubTitle_MultiPax")).Value.ToString();
                    onBoardStep.Content = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step2_Content_SinglePax")).Value.ToString()
                     : captions.First(a => a.Key.Equals("SeatPref_Step2_Content_MultiPax")).Value.ToString();
                }
                else
                {
                    onBoardStep.SubTitle = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step2_SubTitle_SinglePax_Standby")).Value.ToString()
                        : captions.First(a => a.Key.Equals("SeatPref_Step2_SubTitle_MultiPax_Standby")).Value.ToString();
                    onBoardStep.Content = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step2_Content_SinglePax_Standby")).Value.ToString()
                     : captions.First(a => a.Key.Equals("SeatPref_Step2_Content_MultiPax_Standby")).Value.ToString();
                }
                preferenceResponse.OnboardingScreenCaptions.Add(onBoardStep);
                onBoardStep = new OnboardingScreen();
                onBoardStep.Title = captions.First(a => a.Key.Equals("headerText_Onboarding")).Value.ToString() + " | Step3";
                onBoardStep.ImageURL = captions.First(a => a.Key.Equals("SeatPref_Step3_ImageURL")).Value.ToString();
                if (!request.IsStandby)
                {
                    onBoardStep.SubTitle = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step3_SubTitle_SinglePax")).Value.ToString()
                          : captions.First(a => a.Key.Equals("SeatPref_Step3_SubTitle_MultiPax")).Value.ToString();
                    onBoardStep.Content = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step3_Content_SinglePax")).Value.ToString()
                     : captions.First(a => a.Key.Equals("SeatPref_Step3_Content_MultiPax")).Value.ToString();
                }
                else
                {
                    onBoardStep.SubTitle = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step3_SubTitle_SinglePax_Standby")).Value.ToString()
                        : captions.First(a => a.Key.Equals("SeatPref_Step3_SubTitle_MultiPax_Standby")).Value.ToString();
                    onBoardStep.Content = !request.IsMultiPax ? captions.First(a => a.Key.Equals("SeatPref_Step3_Content_SinglePax_Standby")).Value.ToString()
                     : captions.First(a => a.Key.Equals("SeatPref_Step3_Content_MultiPax_Standby")).Value.ToString();
                }
                preferenceResponse.OnboardingScreenCaptions.Add(onBoardStep);
                //Captions
                //captions.RemoveWhere(c => c.Key.Contains("Onboarding"));
                //preferenceResponse.Captions = captions;
                captions.RemoveWhere(c => c.Key.Contains("Step"));
                preferenceResponse.Captions = captions;
                //var token = await _dpTokenService.GetTokenFromAWSDPAsync(loggingContext, appId, deviceId);
                var token = await _dpTokenService.GetAnonymousToken(appId, deviceId, _configuration);

                var seatPreferences = new List<SeatPreferenceOption>();

                if (request.RequestSeatsTogether)
                {
                    var seatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = "Toggle",
                        Title = captions["Preference_Seat_Adjacency_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                IsSelected = request.RequestSeatsTogether,
                                PreferenceValue = PreferenceOption.Adjacency.ToString(),
                                DisplayText = captions["Preference_Seat_Adjacency"], //Request seats together
                            }
                        }
                    };

                    seatPreferences.Add(seatPreferenceOption);
                }
                else
                {
                    var seatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Radio.ToString(),
                        Title = captions["Preference_Seat_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                IsSelected = true,
                                PreferenceValue = PreferenceOption.Window.ToString(),
                                DisplayText = captions["Preference_Seat_DisplayText_Window"]
                            },
                            new PreferenceDetail
                            {
                                IsSelected = false,
                                PreferenceValue = PreferenceOption.Aisle.ToString(),
                                DisplayText = captions["Preference_Seat_DisplayText_Aisle"]
                            }
                        }
                    };

                    seatPreferences.Add(seatPreferenceOption);
                }
                if (request.IsStandby && request.RequestSeatsTogether)
                {
                    var seatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Radio.ToString(),
                        Title = captions["Preference_Seat_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                IsSelected = true,
                                PreferenceValue = PreferenceOption.Window.ToString(),
                                DisplayText = captions["Preference_Seat_DisplayText_Window"]
                            },
                            new PreferenceDetail
                            {
                                IsSelected = false,
                                PreferenceValue = PreferenceOption.Aisle.ToString(),
                                DisplayText = captions["Preference_Seat_DisplayText_Aisle"]
                            }
                        }
                    };

                    seatPreferences.Add(seatPreferenceOption);
                }

                if (((request.IsLegRoom || request.IsStandby) && !request.RequestSeatsTogether) || (request.IsStandby && request.RequestSeatsTogether))
                {
                    var seatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Radio.ToString(),
                        Title = captions["Other_Seat_Preferences_LegRoom_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                IsSelected = true,
                                PreferenceValue = PreferenceOption.AvoidMiddleSeatWithExtraLegRoom.ToString(),
                                DisplayText = captions["Other_Seat_Preferences_Avoid_MiddleSeat_With_ExtraLegRoom"]
                            },
                            new PreferenceDetail
                            {
                                IsSelected = false,
                                PreferenceValue = PreferenceOption.MiddleSeatWithExtraLegRoom.ToString(),
                                DisplayText = captions["Other_Seat_Preferences_MiddleSeat_With_ExtraLegRoom"]
                            }
                        }
                    };

                    seatPreferences.Add(seatPreferenceOption);
                }

                var isOtherSeatPreferenceCaptionsExist = captions.ContainsKey("Other_Seat_Preferences_Title");

                if ((isOtherSeatPreferenceCaptionsExist && !request.RequestSeatsTogether) || (request.RequestSeatsTogether && request.IsStandby))// && (IsBasicEconomyPnr(request) == false))
                {
                    var otherSeatPreferenceOption = new SeatPreferenceOption
                    {
                        OptionType = ButtonType.Checkbox.ToString(),
                        Title = captions["Other_Seat_Preferences_Title"],
                        PreferenceType = PreferenceType.Seat.ToString(),
                        Details = new List<PreferenceDetail>
                        {
                            new PreferenceDetail
                            {
                                IsSelected = false,
                                PreferenceValue = PreferenceOption.AvoidExitRowSeats.ToString(),
                                DisplayText = captions["Other_Seat_Preferences_Avoid_ExitRows"]
                            },
                            new PreferenceDetail
                            {
                                IsSelected = false,
                                PreferenceValue = PreferenceOption.AvoidBulkheadSeats.ToString(),
                                DisplayText = captions["Other_Seat_Preferences_Avoid_Bulkhead"],
                                DisplaySubText = captions["Other_Seat_Preferences_Avoid_Bulkhead_Seat_Desc"]
                            }
                        }
                    };
                    if ((!request.IsLegRoom && !request.IsStandby))
                    {
                        otherSeatPreferenceOption.Details.Insert(0, new PreferenceDetail
                        {
                            IsSelected = false,
                            PreferenceValue = PreferenceOption.AvoidMiddleSeat.ToString(),
                            DisplayText = captions["Other_Seat_Preferences_Choose_window_Aisle"],
                            DisplaySubText = captions["Other_Seat_Preferences_Window_Aisle_Desc"]
                        });
                    }
                    //if (request.IsBE)
                    //{
                    //    otherSeatPreferenceOption.Details.Add(new PreferenceDetail
                    //    {
                    //        IsSelected = false,
                    //        PreferenceValue = PreferenceOption.AvoidMiddleSeat.ToString(),
                    //        DisplayText = captions["Other_Seat_Preferences_Avoid_MiddleSeat"]
                    //    });
                    //}
                    seatPreferences.Add(otherSeatPreferenceOption);
                }

                preferenceResponse = await GetSeatPreferenceFromCslAwsV2(request, preferenceResponse, seatPreferences, captions, token, transactionId).ConfigureAwait(false);

                if (preferenceResponse.Segments != null && preferenceResponse.Segments.Count() > 0)
                    preferenceResponse.OnboardingScreenCaptions = null;
                preferenceResponse.SeatPreferences = seatPreferences;
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception - GetSeatPreference Exception {@Message} and {StackTrace}", ex.Message, ex.StackTrace.ToString());

                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }

            return preferenceResponse;
        }

        private async Task<PersistSeatPreferenceResponse> GetSeatPreferenceFromCslAwsV2(PersistSeatPreferenceRequest request, PersistSeatPreferenceResponse preferenceResponse, List<SeatPreferenceOption> seatPreferences, Dictionary<string, string> captions, string token, string transactionId)
        {
            var jsonResponse = await _customerPreferenceService.GetAsync(token, request.RecordLocator, transactionId).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(jsonResponse))
            {
                var response = JsonConvert.DeserializeObject<PreferencesResult<PersistCustomerPreferences>>(jsonResponse);
                if (response != null && response.Data != null && !string.IsNullOrEmpty(response.Data.RecordLocator) && response.Data.Travelers != null && response.Data.Travelers.Count > 0)
                {
                    var cslTravelers = response.Data.Travelers;

                    foreach (var traveler in cslTravelers)
                    {
                        if (request.FlightDetails != null && request.FlightDetails.Count > 0)
                        {
                            foreach (var flightDts in request.FlightDetails)
                            {
                                var cslSegments = traveler.PreferenceSegments;
                                if (request.FlightDetails != null && !string.IsNullOrEmpty(flightDts.FlightNumber) && !string.IsNullOrEmpty(flightDts.FlightDate) && !string.IsNullOrEmpty(flightDts.MarketingCarrierCode))
                                {
                                    DateTime dpDate = Convert.ToDateTime(flightDts.FlightDate.Trim());
                                    int FltNumber = Convert.ToInt32(flightDts.FlightNumber.Trim());
                                    if (!string.IsNullOrEmpty(flightDts.MarketingCarrierCode) && FltNumber > 0 && (dpDate != DateTime.MinValue || dpDate != DateTime.MaxValue))
                                        cslSegments = traveler.PreferenceSegments.Where(a => a.FlightNumber == FltNumber && a.DepartureDate.Date == dpDate.Date
                                        && a.CarrierCode == flightDts.MarketingCarrierCode).ToList();
                                }
                                if (cslSegments != null && cslSegments.Count() > 0)
                                {
                                    foreach (var cslSegment in cslSegments)
                                    {
                                        var passenger = new Passenger();

                                        var resSegment = new ReservationSegment()
                                        {
                                            MarketingCarrier = new Airline() { Code = cslSegment.CarrierCode },
                                            FlightNumber = cslSegment.FlightNumber.ToString(),
                                            FlightDate = cslSegment.DepartureDate.ToString(),
                                            Departure = new Model.SeatMap.Airport()
                                            {
                                                Code = cslSegment.Origin
                                            },
                                            Arrival = new Model.SeatMap.Airport()
                                            {
                                                Code = cslSegment.Destination
                                            },
                                            HasSavedSeatPref = false
                                        };
                                        if (request.FlightDetails != null && !string.IsNullOrEmpty(flightDts.EstimatedDepartureTime) || !string.IsNullOrEmpty(flightDts.scheduledDepartureTime))
                                        {
                                            DateTime depTime = DateTime.MinValue;
                                            bool boolDeptime = !string.IsNullOrEmpty(flightDts.EstimatedDepartureTime) ? DateTime.TryParse(flightDts.EstimatedDepartureTime, out depTime) :
                                                 DateTime.TryParse(flightDts.scheduledDepartureTime, out depTime);
                                            int restrictVal = _configuration.GetValue<int>("EnableSeatPrefSaveButtonBeforeDeparure");
                                            TimeSpan diff = depTime.ToUniversalTime() - DateTime.UtcNow;
                                            if (diff.TotalHours > 12) resSegment.enableSaveButton = true;
                                        }

                                        if (!resSegment.Passengers.Any(p => p.LastName == traveler.LastName && p.FirstName == traveler.FirstName))
                                        {
                                            passenger.FirstName = traveler.FirstName;
                                            passenger.LastName = traveler.LastName;
                                            passenger.SeatPreferences = new List<SeatPreferenceOption>();
                                        }
                                        else
                                        {
                                            passenger = resSegment.Passengers.Where(p => p.LastName == traveler.LastName && p.FirstName == traveler.FirstName).FirstOrDefault();
                                        }

                                        var cslReferences = cslSegment.Preferences;

                                        if (cslReferences != null && cslReferences.Count() > 0)
                                        {
                                            resSegment.HasSavedSeatPref = true;

                                            var seatPreferencesOption = new List<SeatPreferenceOption>(seatPreferences.Clone());

                                            var seatPreferenceOption = seatPreferencesOption.Where(p => p.Title == captions["Preference_Seat_Title"]).ToList().FirstOrDefault();
                                            var seatPreferenceLegRoom = seatPreferencesOption.Where(p => p.Title == captions["Other_Seat_Preferences_LegRoom_Title"]).ToList().FirstOrDefault();
                                            var seatPreferenceAdjacency = seatPreferencesOption.Where(p => p.Title == "").ToList().FirstOrDefault();
                                            var otherSeatPreferenceOption = seatPreferencesOption.Where(p => p.Title == captions["Other_Seat_Preferences_Title"]).ToList().FirstOrDefault();

                                            foreach (var cslPreference in cslReferences)
                                            {
                                                if (cslPreference.PreferenceType == PreferenceType.Seat)
                                                {
                                                    switch (cslPreference.PreferenceValue)
                                                    {
                                                        case PreferenceOption.Window:
                                                            if ((request.IsStandby && request.RequestSeatsTogether) || !request.RequestSeatsTogether) seatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.Window.ToString()).FirstOrDefault().IsSelected = true;
                                                            if ((request.IsStandby && request.RequestSeatsTogether) || !request.RequestSeatsTogether) seatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.Aisle.ToString()).FirstOrDefault().IsSelected = false;
                                                            break;
                                                        case PreferenceOption.Aisle:
                                                            if ((request.IsStandby && request.RequestSeatsTogether) || !request.RequestSeatsTogether) seatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.Aisle.ToString()).FirstOrDefault().IsSelected = true;
                                                            if ((request.IsStandby && request.RequestSeatsTogether) || !request.RequestSeatsTogether) seatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.Window.ToString()).FirstOrDefault().IsSelected = false;
                                                            break;
                                                        case PreferenceOption.AvoidExitRowSeats:
                                                            if (otherSeatPreferenceOption != null && otherSeatPreferenceOption.Details.Count() > 0) otherSeatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.AvoidExitRowSeats.ToString()).FirstOrDefault().IsSelected = true;
                                                            break;
                                                        case PreferenceOption.AvoidBulkheadSeats:
                                                            if (otherSeatPreferenceOption != null && otherSeatPreferenceOption.Details.Count() > 0) otherSeatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.AvoidBulkheadSeats.ToString()).FirstOrDefault().IsSelected = true;
                                                            break;
                                                        case PreferenceOption.MiddleSeatWithExtraLegRoom:
                                                            if (request.IsLegRoom) seatPreferenceLegRoom.Details.Where(d => d.PreferenceValue == PreferenceOption.MiddleSeatWithExtraLegRoom.ToString()).FirstOrDefault().IsSelected = true;
                                                            if (request.IsLegRoom) seatPreferenceLegRoom.Details.Where(d => d.PreferenceValue == PreferenceOption.AvoidMiddleSeatWithExtraLegRoom.ToString()).FirstOrDefault().IsSelected = false;
                                                            break;
                                                        case PreferenceOption.AvoidMiddleSeat:
                                                            if (otherSeatPreferenceOption != null && otherSeatPreferenceOption.Details.Count() > 0 && otherSeatPreferenceOption.Details.Exists(d => d.PreferenceValue == PreferenceOption.AvoidMiddleSeat.ToString()))
                                                                otherSeatPreferenceOption.Details.Where(d => d.PreferenceValue == PreferenceOption.AvoidMiddleSeat.ToString()).FirstOrDefault().IsSelected = true;
                                                            break;
                                                        case PreferenceOption.AvoidMiddleSeatWithExtraLegRoom:
                                                            if (request.IsLegRoom) seatPreferenceLegRoom.Details.Where(d => d.PreferenceValue == PreferenceOption.AvoidMiddleSeatWithExtraLegRoom.ToString()).FirstOrDefault().IsSelected = true;
                                                            if (request.IsLegRoom) seatPreferenceLegRoom.Details.Where(d => d.PreferenceValue == PreferenceOption.MiddleSeatWithExtraLegRoom.ToString()).FirstOrDefault().IsSelected = false;
                                                            break;
                                                        case PreferenceOption.Adjacency:
                                                            if (request.RequestSeatsTogether) seatPreferenceAdjacency.Details.Where(d => d.PreferenceValue == PreferenceOption.Adjacency.ToString()).FirstOrDefault().IsSelected = true;
                                                            break;
                                                        default:
                                                            break;
                                                    }
                                                }
                                            }

                                            if (seatPreferenceAdjacency != null && seatPreferenceAdjacency.Details.Count() > 0) passenger.SeatPreferences.Add(seatPreferenceAdjacency);
                                            if (seatPreferenceOption != null && seatPreferenceOption.Details.Count() > 0) passenger.SeatPreferences.Add(seatPreferenceOption);
                                            if (seatPreferenceLegRoom != null && seatPreferenceLegRoom.Details.Count() > 0) passenger.SeatPreferences.Add(seatPreferenceLegRoom);
                                            if (otherSeatPreferenceOption != null && otherSeatPreferenceOption.Details.Count() > 0) passenger.SeatPreferences.Add(otherSeatPreferenceOption);
                                        }

                                        if (!resSegment.Passengers.Contains(passenger)) resSegment.Passengers.Add(passenger);
                                        preferenceResponse.Segments.Add(resSegment);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return preferenceResponse;
        }

        public async Task<PostSeatPreferenceResponse> SaveSeatPreferencetToCSLV2(PostSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode, MOBApplication mOBApplication, string langCode)
        {
            var reponse = new PostSeatPreferenceResponse();

            try
            {
                Session session = null;
                if (!string.IsNullOrEmpty(request.SessionId))
                {
                    session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                }

                if (session == null && accessCode != "ACCESSCODE")
                {
                    _logger.LogError("SaveSeatPreference - Unauthorized Request {Request}", request);

                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                if (request.Segments == null || request.Segments.Count == 0)
                {
                    _logger.LogError("SaveSeatPreference - Segments missing from Request {Request}", request);

                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                var cslRequest = new PersistCustomerPreferences
                {
                    RecordLocator = request.RecordLocator,
                    Travelers = new List<Model.SeatMap.Traveler>()
                };
                var mobPNRPax = new List<MOBPNRPassenger>();
                foreach (var resSeg in request.Segments)
                {
                    if (!string.IsNullOrEmpty(resSeg.FlightDate) && !string.IsNullOrEmpty(resSeg.FlightNumber) && !string.IsNullOrEmpty(resSeg.MarketingCarrier.Code)
                        && !string.IsNullOrEmpty(resSeg.Arrival.Code) && !string.IsNullOrEmpty(resSeg.Departure.Code) &&
                        resSeg.Passengers != null)
                    {
                        var prefSeg = new PreferenceSegment
                        {
                            CarrierCode = resSeg.MarketingCarrier.Code,
                            DepartureDate = Convert.ToDateTime(resSeg.FlightDate),
                            Destination = resSeg.Arrival.Code,
                            FlightNumber = Convert.ToInt32(resSeg.FlightNumber),
                            Origin = resSeg.Departure.Code,

                            Preferences = new List<Preferences>()
                        };

                        var traveler = new Model.SeatMap.Traveler();
                        var passengers = resSeg.Passengers;


                        foreach (var pax in passengers)
                        {
                            if (!string.IsNullOrEmpty(pax.LastName) && !string.IsNullOrEmpty(pax.FirstName))
                            {
                                if (!cslRequest.Travelers.Any(t => t.LastName == pax.LastName && t.FirstName == pax.FirstName))
                                {
                                    traveler.FirstName = pax.FirstName;
                                    traveler.LastName = pax.LastName;
                                    traveler.PreferenceSegments = new List<PreferenceSegment>();
                                }
                                else
                                {
                                    traveler = cslRequest.Travelers.Where(t => t.LastName == pax.LastName && t.FirstName == pax.FirstName).FirstOrDefault();
                                }

                                if (pax.SeatPreferences != null && pax.SeatPreferences.Count() > 0)
                                {
                                    foreach (var preference in pax.SeatPreferences)
                                    {
                                        PreferenceType type;
                                        PreferenceOption value;
                                        Enum.TryParse<PreferenceType>(preference.PreferenceType, out type);
                                        Enum.TryParse<PreferenceOption>(preference.PreferenceValue, out value);

                                        var pref = new Preferences
                                        {
                                            PreferenceType = type,
                                            PreferenceValue = value
                                        };

                                        prefSeg.Preferences.Add(pref);
                                    }
                                }
                                else
                                {
                                    var pref = new Preferences
                                    {
                                        PreferenceType = PreferenceType.Seat,
                                        PreferenceValue = PreferenceOption.Unknown
                                    };

                                    prefSeg.Preferences.Add(pref);
                                }
                                if (pax.TravelersInfo != null && pax.TravelersInfo.Contact != null)
                                {
                                    mobPNRPax.Add(pax.TravelersInfo);
                                }
                            }
                            traveler.PreferenceSegments.Add(prefSeg);
                        }
                        if (!cslRequest.Travelers.Contains(traveler)) cslRequest.Travelers.Add(traveler);
                    }
                }

                //var token = await _dpTokenService.GetTokenFromAWSDPAsync(loggingContext, appId, deviceId);
                var token = await _dpTokenService.GetAnonymousToken(appId, deviceId, _configuration);
                reponse = await PostSeatPreferenceToCslAwsV2(request, reponse, cslRequest, token, transactionId);
                if (reponse.IsSuccess == true)
                {
                    if (mobPNRPax != null && mobPNRPax.Count > 0)
                    {
                        var data = new MOBUpdateTravelerInfoRequest();
                        try
                        {
                            data = new MOBUpdateTravelerInfoRequest
                            {
                                AccessCode = accessCode,
                                LanguageCode = langCode,
                                RecordLocator = request.RecordLocator,
                                TransactionId = transactionId,
                                TravelersInfo = mobPNRPax,
                                SessionId = request.SessionId,
                                DeviceId = deviceId,
                                Application = mOBApplication,
                            };

                            _logger.LogInformation("SaveSeatPreference updateTravelerInfo (TCD) initiated - {Request}, {TransactionId}", data, transactionId);
                            var updateTravelerInfoResponse = await _updateTravelersInformation.UpdateTravelersInfo<MOBUpdateTravelerInfoResponse>(JsonConvert.SerializeObject(data), transactionId).ConfigureAwait(false);
                            if (updateTravelerInfoResponse.Exception == null)
                            {
                                _logger.LogInformation("SaveSeatPreference TravelerInfo successfully saved {TransactionId}", transactionId);
                                reponse.isTCDSaved = true;
                                reponse.tCDMessage = "TravelerInfo saved successfully";

                            }
                            else
                            {
                                reponse.isTCDSaved = false;
                                reponse.tCDMessage = _configuration.GetValue<string>("GenericExceptionMessage");
                                _logger.LogError("SaveSeatPreference updateTravelerInfo save failed - {Request}, {Exception}", data, updateTravelerInfoResponse.Exception);
                            }
                        }
                        catch (Exception ex) { _logger.LogError("SaveSeatPreference updateTravelerInfo failed - {Request}, {Exception}", data, ex); }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SeatPreferenceBusiness - Exception occurred in Save SeatPreferencetToCSL {@Message} and {@StackTrace}", ex.Message, ex.StackTrace);
                throw ex;
            }
            return reponse;
        }

        private async Task<PostSeatPreferenceResponse> PostSeatPreferenceToCslAwsV2(PostSeatPreferenceRequest request, PostSeatPreferenceResponse response, PersistCustomerPreferences cslRequest, string token, string transactionId)
        {
            try
            {
                bool isDeletePref = false;
                var req = cslRequest.Travelers.Select(a => a.PreferenceSegments).ToList();
                foreach (var segment in req)
                {
                    var segs = segment.Select(a => a.Preferences).ToList();
                    if (segs != null && segs.Count > 0)
                    {
                        foreach (var seg in segs)
                        {
                            isDeletePref = seg.Any(a => a.PreferenceValue == PreferenceOption.Unknown);
                        }
                    }
                }
                var cslRequestJson = JsonConvert.SerializeObject(cslRequest);
                var jsonResponse = await _customerPreferenceService.SaveAsync(token, cslRequestJson, transactionId);

                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    var responseObj = JsonConvert.DeserializeObject<PreferencesResult<PersistCustomerPreferencesResponse>>(jsonResponse);
                    if (responseObj != null)
                    {
                        if (responseObj.ErrorNumber == 0 && responseObj.Data != null && responseObj.Data.RecordLocator.Equals(request.RecordLocator, StringComparison.OrdinalIgnoreCase))
                        {
                            response.IsSuccess = true;
                            string msg;
                            response.MessageDesc = GetSaveORRemovedDesc(request, out msg, isDeletePref);
                            response.Message = msg;
                        }
                        else
                        {
                            response.IsSuccess = false;
                            response.MessageDesc = string.Empty;
                            response.Message = _configuration.GetValue<string>("GenericExceptionMessage");

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SeatPreferenceBusiness - Exception occurred in Post SeatPreferenceToCslAws {@exception} and {@stacktrace}", ex.Message, ex.StackTrace);

                throw ex;
            }

            return response;
        }
        #endregion

        //private void SetSelectedPreference(PersistSeatPreferenceResponse preferenceResponse, PreferenceOption preferenceOption)
        //{
        //    var detail = preferenceResponse?.SeatPreferences?
        //                                    .SelectMany(sp => sp.Details)?
        //                                    .FirstOrDefault(pd => pd.PreferenceValue == preferenceOption.ToString());
        //    if (detail != null)
        //        detail.IsSelected = true;
        //}


        //private bool IsValidPnr(string recordLocator, string lastName, Model.SeatMap.Reservation reservationDoc)
        //{
        //    return (reservationDoc != null && recordLocator.Equals(reservationDoc.RecordLocator, StringComparison.OrdinalIgnoreCase) && reservationDoc.PassengerNames.Any(pax => pax.LastName.Equals(lastName, StringComparison.OrdinalIgnoreCase)));
        //}

        //private bool? IsBasicEconomyPnr(PersistSeatPreferenceRequest request)
        //{
        //    return string.Equals("UA", request.CarrierCode, StringComparison.OrdinalIgnoreCase)
        //        && string.Equals("N", request.BookingCode, StringComparison.OrdinalIgnoreCase);
        //}
        private bool? IsBasicEconomyPnr(PersistSeatPreferenceRequest request)
        {
            return string.Equals("UA", request.MarketingCarrierCode, StringComparison.OrdinalIgnoreCase)
                && string.Equals("N", request.BookClassOfServiceCode, StringComparison.OrdinalIgnoreCase);
        }

        private string GetSaveORRemovedDesc(PostSeatPreferenceRequest request, out string msg, bool removePref)
        {
            var seatMapDynamoDB = new SeatMapDynamoDB(_configuration, _dynamoDBService);
            var captions = seatMapDynamoDB.GetSeatPreference<List<MOBItem>>("MR_SeatPreference", request.SessionId).Result.ToDictionary(s => s.Id, s => s.CurrentValue);
            msg = string.Empty;
            if (captions != null && captions.Count > 0)
            {
                if (removePref)
                {
                    if (!request.IsMultiPax)
                    {
                        msg = captions["Seat_Preference_Removed"];
                        return captions["Seat_Preference_Removed_SinglePax_Desc"];
                    }
                    else
                    {
                        msg = captions["Seat_Preferences_Removed"];
                        return captions["Seat_Preference_Removed_MultiPax_Desc"];
                    }
                }
                if (!request.IsMultiPax)
                {
                    msg = captions["Seat_Preference_Saved"];
                    return (!request.IsStandby) ? captions["Seat_Preference_Saved_SinglePax_Desc"] : captions["Seat_Preference_Saved_SinglePax_Standby_Desc"];
                }
                else
                {
                    msg = captions["Seat_Preferences_Saved"];
                    return (!request.IsStandby) ? captions["Seat_Preference_Saved_MultiPax_Desc"] : captions["Seat_Preference_Saved_MultiPax_Standby_Desc"];
                }
            }
            else { msg = "Seat preference saved"; }
            return string.Empty;
        }
    }
}
