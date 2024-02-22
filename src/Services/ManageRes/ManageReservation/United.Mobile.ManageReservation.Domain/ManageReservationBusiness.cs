using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Merchandize;
using United.Common.Helper.Shopping;
using United.Common.HelperSeatEngine;
using United.Ebs.Logging.Enrichers;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Customer;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.Loyalty;
using United.Mobile.DataAccess.ManageReservation;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Fitbit;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.FligtStatus.Internal;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.PersonalizationResponseModel;
using United.Service.Presentation.ReservationRequestModel;
using United.Service.Presentation.ReservationResponseModel;
using United.Service.Presentation.SegmentModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using Message = United.Service.Presentation.SegmentModel.Message;

namespace United.Mobile.ManageReservation.Domain
{
    public class ManageReservationBusiness : IManageReservationBusiness
    {
        private readonly ICacheLog<ManageReservationBusiness> _logger;
        private readonly IHeaders _headers;
        private readonly IConfiguration _configuration;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IManageReservation _manageReservation;
        private readonly ManageResUtility _manageResUtility;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IDPService _dPService;
        private readonly IPNRRetrievalService _pNRRetrievalService;
        private readonly IRequestReceiptByEmailService _requestReceiptByEmailService;
        private readonly ISendReceiptByEmailService _sendReceiptByEmailService;
        private readonly DataAccess.ReShop.IReservationService _reservationService;
        private readonly IProductInfoHelper _productInfoHelper;
        private readonly ICustomerProfileService _customerProfileService;
        private readonly ILoyaltyMemberProfileService _loyaltyMemberProfileService;
        private readonly ISeatEngine _seatEngine;
        private readonly IApplicationEnricher _requestEnricher;
        private readonly IAuroraMySqlService _auroraMySqlService;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly ISeatMapEngine _seatMapEngine;
        private readonly IFeatureSettings _featureSettings;

        public ManageReservationBusiness(ICacheLog<ManageReservationBusiness> logger
            , IHeaders headers
            , IConfiguration configuration
            , IShoppingSessionHelper shoppingSessionHelper
            , ISessionHelperService sessionHelperService
            , IManageReservation manageReservation
            , IDynamoDBService dynamoDBService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IDPService dPService
            , IPNRRetrievalService pNRRetrievalService
            , IRequestReceiptByEmailService requestReceiptByEmailService
            , ISendReceiptByEmailService sendReceiptByEmailService
            , DataAccess.ReShop.IReservationService reservationService
            , IProductInfoHelper productInfoHelper
            , ICustomerProfileService customerProfileService
            , ILoyaltyMemberProfileService loyaltyMemberProfileService
            , ISeatEngine seatEngine
            , IApplicationEnricher requestEnricher
            , IAuroraMySqlService auroraMySqlService
            , IShoppingUtility shoppingUtility
            , ISeatMapEngine seatMapEngine
            , IFeatureSettings featureSettings
            )
        {
            _logger = logger;
            _headers = headers;
            _configuration = configuration;
            _shoppingSessionHelper = shoppingSessionHelper;
            _sessionHelperService = sessionHelperService;
            _manageReservation = manageReservation;
            _dynamoDBService = dynamoDBService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _manageResUtility = new ManageResUtility(_configuration, _legalDocumentsForTitlesService, _dynamoDBService, _headers, _logger);
            _dPService = dPService;
            _pNRRetrievalService = pNRRetrievalService;
            _requestReceiptByEmailService = requestReceiptByEmailService;
            _sendReceiptByEmailService = sendReceiptByEmailService;
            _reservationService = reservationService;
            _productInfoHelper = productInfoHelper;
            _customerProfileService = customerProfileService;
            _loyaltyMemberProfileService = loyaltyMemberProfileService;
            _seatEngine = seatEngine;
            _requestEnricher = requestEnricher;
            _auroraMySqlService = auroraMySqlService;
            _shoppingUtility = shoppingUtility;
            _seatMapEngine = seatMapEngine;
            _featureSettings = featureSettings;
        }

        public async Task<MOBMileageAndStatusOptionsResponse> GetMileageAndStatusOptions(MOBMileageAndStatusOptionsRequest request)
        {
            var response = new MOBMileageAndStatusOptionsResponse();

            response.SessionId = request.SessionId;
            response.Accelerators = await GetMileageAndStatusOptions(request, request.SessionId, request.CorrelationId);
            return response;
        }

        public async Task<MOBPNRByRecordLocatorResponse> GetPNRByRecordLocator(MOBPNRByRecordLocatorRequest request)
        {
            MOBPNRByRecordLocatorResponse response = new MOBPNRByRecordLocatorResponse();

            if (_configuration.GetValue<bool>("EnableEncryptedPNRRequest")
                    && !string.IsNullOrEmpty(request.EncryptedRequest)
                    && !string.IsNullOrEmpty(request.Requestor))
            {
                MOBPNRByRecordLocatorRequest mobDecryptedRequest = DecryptPNRRequest(request);

                if (mobDecryptedRequest != null
                    && !string.IsNullOrEmpty(mobDecryptedRequest.RecordLocator)
                    && !string.IsNullOrEmpty(mobDecryptedRequest.LastName))
                {
                    request.RecordLocator = mobDecryptedRequest.RecordLocator;
                    request.LastName = mobDecryptedRequest.LastName;
                }
            }

            Session session = null;
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                session.Flow = request.Flow;
                if (!string.IsNullOrEmpty(session?.SessionId) && _headers.ContextValues != null)
                {
                    _headers.ContextValues.SessionId = session.SessionId;
                    _requestEnricher.Add(United.Mobile.Model.Constants.SessionId, session.SessionId);
                }
            }
            else
            {
                session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, string.Empty, false, true);
                session.Flow = request.Flow;
            }
            if (string.IsNullOrEmpty(request.SessionId))
                request.SessionId = session.SessionId;

            if(request.CatalogValues !=null && request.CatalogValues.Count > 0 && _manageResUtility.IsEnableTravelOptionsInViewRes(request.Application.Id, request.Application.Version.Major, request?.CatalogValues))
            {
                if (session.CatalogItems == null) session.CatalogItems = new List<MOBItem>();
                if (request.CatalogValues != null && request.CatalogValues.Count > 0)
                    session.CatalogItems.AddRange(request.CatalogValues);
                else
                    session.CatalogItems.AddRange(request.CatalogValues);

                await _sessionHelperService.SaveSession<Session>(session, session.SessionId, new List<string> { session.SessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);
            }

            response = await _manageReservation.GetPNRByRecordLocatorCommonMethod(request);
            response.Flow = request.Flow;

            response.SessionId = session.SessionId;
            var cslReservation = await _sessionHelperService.GetSession<ReservationDetail>(request.SessionId, new ReservationDetail().GetType().FullName, new List<string> { request.SessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);
            if (_configuration.GetValue<bool>("joinOneClickMileagePlusEnabled") && cslReservation != null && cslReservation.Detail != null)
            {
                response.ShowJoinOneClickEnrollment = ValidateUserEnrollementEligibility(response, cslReservation.Detail.Travelers, cslReservation.Detail.Prices, session);
            }
            if (_configuration.GetValue<bool>("countDownWidgetEnabled"))
            {
                response.CountDownWidgetInfo = GetCountDownWidgetInfoConfigValues(request.Application.Id);
            }

            if (response.PNR != null)
            {
                if (response.PNR.HasScheduleChanged && !response.PNR.ConsolidateScheduleChangeMessage)
                {
                    response.PNR.HasScheduleChanged = _manageResUtility.GetHasScheduledChanged(response.PNR.Segments);
                    response.PNR.StatusMessageItems = await SetScheduledChangeMessage(response.PNR.HasScheduleChanged);
                    SetupRedirectURL(request.RecordLocator, request.LastName, response.PNR.URLItems, "PNRURL");
                }
                if (response.PNR.AdvisoryInfo != null && response.PNR.AdvisoryInfo.Any())
                {
                    var scheduleChange
                        = response.PNR.AdvisoryInfo.Where(x => x.ContentType == ContentType.SCHEDULECHANGE).FirstOrDefault();
                    if (scheduleChange != null)
                    {
                        scheduleChange.Buttonlink =
                            (_configuration.GetValue<bool>("EnableTripDetailScheduleChangeRedirect3dot0Url"))
                            ? GetTripDetailRedirect3dot0Url
                            (request.RecordLocator, request.LastName, ac: "VI", channel: "mobile", languagecode: "en/US")
                            : GetPNRRedirectUrl(request.RecordLocator, request.LastName, reqType: "VI");
                    }
                }

                //Covid-19 Emergency WHO TPI content
                if (_configuration.GetValue<bool>("ToggleCovidEmergencytextTPI") == true)
                {
                    bool return_TPICOVID_19WHOMessage_For_BackwardBuilds = GeneralHelper.IsApplicationVersionGreater2(request.Application.Id, request.Application.Version.Major, "Android_Return_TPICOVID_19WHOMessage__For_BackwardBuilds", "iPhone_Return_TPICOVID_19WHOMessage_For_BackwardBuilds", "", "", _configuration);
                    if (!return_TPICOVID_19WHOMessage_For_BackwardBuilds && response.TripInsuranceInfo != null
                        && response.TripInsuranceInfo.tpiAIGReturnedMessageContentList != null
                        && response.TripInsuranceInfo.tpiAIGReturnedMessageContentList.Count > 0)
                    {
                        MOBItem tpiCOVID19EmergencyAlert = response.TripInsuranceInfo.tpiAIGReturnedMessageContentList.Find(p => p.Id.ToUpper().Trim() == "COVID19EmergencyAlertManageRes".ToUpper().Trim());
                        if (tpiCOVID19EmergencyAlert != null)
                        {
                            response.TripInsuranceInfo.Body3 = response.TripInsuranceInfo.Body3 +
                                "<br><br>" + tpiCOVID19EmergencyAlert.CurrentValue;
                        }
                    }
                }
                if (ConfigUtility.IsAddPetMREnabled(request.CatalogValues))
                {
                    response.PetEligibility = new MOBAddPetEligible
                    {
                        AddPetEligible = true, //Unblock ui mock data
                        AddPetRedirectURL = "https://qa9.united.com/en/us/petInCabin",
                        AddPetButtonText = _configuration.GetValue<string>("MRPetButtonText"),
                    };
                    if (response.PNR.IsUnaccompaniedMinor ||
                        cslReservation.Detail.FlightSegments.Exists(s => s.FlightSegment?.OperatingAirlineCode != "UA") ||
                        !response.PNR.MarketType.Equals("Domestic"))
                    {
                        response.PetEligibility.AddPetEligible = false;
                    }
                }
            }

            response.DOTBagrules(_configuration);

            return await Task.FromResult(response);

        }

        public async Task<MOBReceiptByEmailResponse> RequestReceiptByEmail(MOBReceiptByEmailRequest request)
        {

            MOBReceiptByEmailResponse response = new MOBReceiptByEmailResponse();
            response.TransactionId = request.TransactionId;

            if (GeneralHelper.ValidateAccessCode(request.AccessCode))
            {

                CommonDef commonDef = new CommonDef();
                CommonDef presistedCommonDef = null;
                if (_configuration.GetValue<bool>("DeviceIDPNRSessionGUIDCaseSensitiveFix"))
                {
                    presistedCommonDef = await _sessionHelperService.GetSession<CommonDef>
                        ((GetDeviceIdFromTransactionId(request.TransactionId) + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName, new List<string> { (GetDeviceIdFromTransactionId(request.TransactionId) + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName }).ConfigureAwait(false);
                }
                else
                {
                    presistedCommonDef = await _sessionHelperService.GetSession<CommonDef>
                        ((GetDeviceIdFromTransactionId(request.TransactionId) + request.RecordLocator).Replace("|", "").Replace("-", ""), commonDef.ObjectName, new List<string> { (GetDeviceIdFromTransactionId(request.TransactionId) + request.RecordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName }).ConfigureAwait(false);
                }

                MOBPNRByRecordLocatorResponse mobpnrbyrecordlocatorresponse;
                if (presistedCommonDef != null)
                {
                    mobpnrbyrecordlocatorresponse = JsonConvert.DeserializeObject<MOBPNRByRecordLocatorResponse>(presistedCommonDef.SampleJsonResponse);
                    if (mobpnrbyrecordlocatorresponse == null)
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("InvalidPNRLastName-ExceptionMessage").ToString());
                    }
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("InvalidPNRLastName-ExceptionMessage").ToString());
                }

                if (request.RecordLocator == null || request.RecordLocator.Trim().Length != 6)
                {
                    throw new MOBUnitedException("Confirmation number must be 6 alphanumeric in length");
                }

                if (!IsValidEmail(request.EMailAddress))
                {
                    throw new MOBUnitedException("You have entered an invalid e-mail address");
                }

                response.RecordLocator = request.RecordLocator;
                response.EMailAdress = request.EMailAddress;
                response.CreationDate = request.CreationDate;
                request.DeviceId = string.IsNullOrEmpty(request.DeviceId) ? mobpnrbyrecordlocatorresponse.DeviceId : request.DeviceId;

                //TO DO
                if (_configuration.GetValue<bool>("EnableNewSendReceiptByEmail"))
                {
                    if (await SendReceiptByEmail(request))
                    {
                        response.Message = string.Format
                            ("Ticket receipt for confirmation number {0} is sent to {1}", response.RecordLocator, response.EMailAdress);
                    }
                }
                else
                {
                    if (await RequestReceiptByEmailViaCSL(request))
                    {
                        response.Message = string.Format
                            ("Ticket receipt for confirmation number {0} is sent to {1}", response.RecordLocator, response.EMailAdress);
                    }
                }
            }
            else
            {
                throw new MOBUnitedException("Invalid access code");
            }

            return await Task.FromResult(response);

        }
        private string GetDeviceIdFromTransactionId(string transactionId)
        {
            string retDeviceId = transactionId;
            if (!string.IsNullOrEmpty(retDeviceId) && retDeviceId.IndexOf('|') > -1)
            {
                retDeviceId = retDeviceId.Split('|')[0];
            }
            return retDeviceId;
        }
        private bool IsValidEmail(string email)
        {
            string strRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                  @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                  @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
            Regex re = new Regex(strRegex);
            if (re.IsMatch(email))
                return (true);
            else
                return (false);
        }
        private async Task<bool> SendReceiptByEmail
          (MOBReceiptByEmailRequest request)
        {
            bool ok = false;
            List<string> email = new List<string>();
            email.Add(request.EMailAddress);

            var sendRcptRequest = new SendRcptRequest
            {
                Client = "clientid",
                RcptType = "T",
                Resend = "Y",
                DlvryType = "E",
                RecLoc = request.RecordLocator,
                PnrCreateDt = GetFormattedDate(request.CreationDate),
                Email = email,
            };

            string token = await _dPService.GetAnonymousToken(request.Application.Id, request.DeviceId, _configuration).ConfigureAwait(false);
            string action = "/SendReceipt";
            string jsonRequest = JsonConvert.SerializeObject(sendRcptRequest);
            var cslResponse = await _sendReceiptByEmailService.SendReceiptByEmailViaCSL(token, jsonRequest, _headers.ContextValues.SessionId, action);

            if (!string.IsNullOrEmpty(cslResponse))
            {
                SendRcptResponse
                   msgResponse = JsonConvert.DeserializeObject<SendRcptResponse>(cslResponse);

                if (msgResponse != null && msgResponse.Status.Contains("Success"))
                {
                    ok = true;
                }
            }
            return ok;
        }

        private string GetFormattedDate(string date)
        {
            string sDate = string.Empty;
            try
            {
                sDate = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
            }
            catch
            { }
            return sDate;
        }
        private async Task<bool> RequestReceiptByEmailViaCSL(MOBReceiptByEmailRequest request)
        {
            bool ok = false;

            Collection<United.Service.Presentation.CommonModel.EmailAddress> cslRequest = new Collection<United.Service.Presentation.CommonModel.EmailAddress>();
            cslRequest.Add(
                new Service.Presentation.CommonModel.EmailAddress
                {
                    Address = request.EMailAddress
                });

            _logger.LogInformation("RequestReceiptByEmailViaCSL Request {SessionId} and {cslRequest}", _headers.ContextValues.SessionId, cslRequest);

            string token = await _dPService.GetAnonymousToken(request.Application.Id, request.DeviceId, _configuration).ConfigureAwait(false);
            string jsonRequest = JsonConvert.SerializeObject(cslRequest);
            string confirmationID = request.RecordLocator;
            var cslResponse = await _requestReceiptByEmailService.PostReceiptByEmailViaCSL(token, jsonRequest, _headers.ContextValues.SessionId, confirmationID).ConfigureAwait(false);

            if (!string.IsNullOrEmpty(cslResponse))
            {
                List<United.Service.Presentation.CommonModel.Message>
                   msgResponse = JsonConvert.DeserializeObject<List<United.Service.Presentation.CommonModel.Message>>(cslResponse);

                _logger.LogInformation("RequestReceiptByEmailViaCSL Response {msgResponse}", msgResponse);

                if (msgResponse != null && msgResponse.Any()
                    && string.Equals(msgResponse[0].Status, "SHARE_RESPONSE", StringComparison.OrdinalIgnoreCase)
                    && msgResponse[0].Text.IndexOf("EMAIL RQSTD") >= 0)
                {
                    ok = true;
                }

            }
            return ok;
        }
        public async Task<MOBPNRByRecordLocatorResponse> PerformInstantUpgrade(MOBInstantUpgradeRequest request)
        {
            Session session = null;

            MOBPNRByRecordLocatorResponse response = new MOBPNRByRecordLocatorResponse(_configuration);

            response.TransactionId = request.TransactionId;
            response.RecordLocator = request.RecordLocator;
            response.LastName = request.LastName;
            response.Flow = !string.IsNullOrEmpty(request.Flow) ? request.Flow : "VIEWRES";
            response.SessionId = request.SessionId;

            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                session.Flow = request.Flow;
            }
            if (string.IsNullOrEmpty(request.SessionId))
                request.SessionId = session.SessionId;

            MOBPNRByRecordLocatorRequest pnrByRecordLocatorRequest = new MOBPNRByRecordLocatorRequest
            {
                AccessCode = request.AccessCode,
                TransactionId = request.TransactionId,
                RecordLocator = request.RecordLocator,
                LastName = request.LastName,
                LanguageCode = request.LanguageCode,
                Application = request.Application,
                SessionId = request.SessionId,
                Flow = request.Flow
            };
            //View Res XML to CSL migration
            bool isCSLPNRServiceOn = Convert.ToBoolean(_configuration.GetValue<string>("SwithToCSLPNRService") ?? "false");
            if (!isCSLPNRServiceOn)
            {
                if (GeneralHelper.ValidateAccessCode(request.AccessCode))
                {
                    //SOAP Call NOT used as the toggle is always true so it won't enter this block
                    //string instantUpgradeResponse = PerformInstantUpgrade(request.TransactionId, request.SessionId, request.RecordLocator, request.LastName, request.SegmentIndexes, request.LanguageCode, request.Application.Id);
                    //response = await _manageReservation.GetPNRByRecordLocatorCommonMethod(pnrByRecordLocatorRequest);
                    //response.PNR.UpgradeMessage = instantUpgradeResponse;
                }
                else
                {
                    throw new MOBUnitedException("Invalid access code");
                }
            }
            else
            {
                string instantUpgradeResponse = await PerformInstantUpgradeCSL(request.TransactionId, request.SessionId, request.RecordLocator, request.LastName, request.LanguageCode, request.Application.Id, request.Application.Version.Major);
                response = await _manageReservation.GetPNRByRecordLocatorCommonMethod(pnrByRecordLocatorRequest);

                response.PNR.UpgradeMessage = instantUpgradeResponse;
            }
            response.PNR.IsEnableEditTraveler = (!response.PNR.isgroup);
            response.Flow = !string.IsNullOrEmpty(request.Flow) ? request.Flow : "VIEWRES";
            response.PNR.IsEnableEditTraveler = (!response.PNR.isgroup);
            response.SessionId = request.SessionId;

            return await Task.FromResult(response);
        }
        public async Task<MOBOneClickEnrollmentResponse> GetOneClickEnrollmentDetailsForPNR(MOBPNRByRecordLocatorRequest request)
        {
            var oneClickEnrollmentResponse = GetOneClickEnrollmentConfigValues(request);
            Session session = null;
            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetValidateSession(request.SessionId, false, true);
                session.Flow = request.Flow;
            }
            else
            {
                session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, string.Empty, false, true);
                session.Flow = request.Flow;
            }
            if (string.IsNullOrEmpty(request.SessionId))
                request.SessionId = session.SessionId;

            ReservationDetail cslReservation = null;

            if (request.IsRefreshedUserData)
            {
                await _manageReservation.GetPNRByRecordLocatorCommonMethod(request);
                cslReservation = await _sessionHelperService.GetSession<ReservationDetail>
                                          (request.SessionId, (new ReservationDetail()).GetType().FullName, new List<string> { request.SessionId, (new ReservationDetail()).GetType().FullName }).ConfigureAwait(false);
            }
            else
            {
                cslReservation = await _sessionHelperService.GetSession<ReservationDetail>
                                          (request.SessionId, (new ReservationDetail()).GetType().FullName, new List<string> { request.SessionId, (new ReservationDetail()).GetType().FullName }).ConfigureAwait(false);
            }
            oneClickEnrollmentResponse.TravelersInfo = new List<TravelerInfo>();
            if (cslReservation != null && cslReservation.Detail != null && cslReservation.Detail.Travelers != null && cslReservation.Detail.Travelers.Count > 0)
            {
                var notEnrolledPassengers = cslReservation?.Detail?.Travelers?.Where(x => x.LoyaltyProgramProfile?.LoyaltyProgramMemberID == null).ToList();
                if (notEnrolledPassengers.Count() > 0)
                {
                    foreach (var traveler in notEnrolledPassengers)
                    {
                        TravelerInfo travelerInfo = new TravelerInfo();
                        if (!string.IsNullOrEmpty(traveler.Person?.GivenName) && !string.IsNullOrEmpty(traveler.Person?.Surname) &&
                            !string.IsNullOrEmpty(traveler.Person?.DateOfBirth) && traveler.Person?.Contact?.PhoneNumbers != null &&
                            traveler.Person?.Contact?.PhoneNumbers?.Count > 0 &&
                            !string.IsNullOrEmpty(traveler.Person?.Contact?.PhoneNumbers?.FirstOrDefault().PhoneNumber) && traveler.Person?.Type != "INF")
                        {
                            var middleName = (traveler.Person?.MiddleName?.Length > 1 ? traveler.Person?.MiddleName.Substring(1).ToLower() : string.Empty) + " ";
                            travelerInfo.TravelerName = traveler.Person?.GivenName?.Substring(0, 1).ToUpper() + traveler.Person?.GivenName?.Substring(1).ToLower() + " " + (traveler.Person?.MiddleName?.Length > 0 ? traveler.Person?.MiddleName.Substring(0, 1).ToUpper() : string.Empty) + (!string.IsNullOrEmpty(middleName) ? middleName : string.Empty) + traveler.Person?.Surname?.Substring(0, 1).ToUpper() + traveler.Person?.Surname?.Substring(1).ToLower();
                            travelerInfo.SharesPosition = traveler.Person?.Key;
                            var Age = !string.IsNullOrEmpty(traveler.Person?.DateOfBirth) ? System.DateTime.Now.Year - traveler.Person?.DateOfBirth?.ToDateTime().Year : 0;
                            travelerInfo.ShowMarketingEmailCheck = Age >= 18 ? true : false;
                            travelerInfo.ShowUnder18EnrollmentMessage = Age >= 18 ? false : true;
                            oneClickEnrollmentResponse.TravelersInfo.Add(travelerInfo);
                        }
                    };
                    oneClickEnrollmentResponse.IsOneTraveler = oneClickEnrollmentResponse.TravelersInfo.Count > 1 ? false : true;
                    oneClickEnrollmentResponse.IsGetPNRByRecordLocator = true;
                    oneClickEnrollmentResponse.IsGetPNRByRecordLocatorCall = true;
                    if (oneClickEnrollmentResponse.TravelersInfo.Count == 1)
                    {
                        oneClickEnrollmentResponse.SelectTravelerHeader = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusOneTravelerText")) ? _configuration.GetValue<string>("joinMileagePlusOneTravelerText") : string.Empty;
                    }
                    else if (oneClickEnrollmentResponse.TravelersInfo.Count == 0)
                    {
                        oneClickEnrollmentResponse.SelectTravelerHeader = string.Empty;
                    }
                }
            }

            oneClickEnrollmentResponse.DeviceId = request.DeviceId;
            if (oneClickEnrollmentResponse.Exception != null)
            {
                //ALM# 27193 - Changed the Message as petr the ALM comments by Hueramo, Carla 
                string exMessage = oneClickEnrollmentResponse.Exception.Message;
                //if (exMessage.Trim().IndexOf("The confirmation number entered is invalid.") > -1 || exMessage.Trim().IndexOf("Please enter a valid record locator.") > -1)
                if (exMessage.Trim().IndexOf("The confirmation number entered is invalid.") > -1 || exMessage.Trim().IndexOf("Please enter a valid record locator.") > -1 || exMessage.Trim().IndexOf("The last name you entered, does not match the name we have on file.") > -1 || exMessage.Trim().IndexOf("Please enter a valid last name") > -1)
                {
                    exMessage = _configuration.GetValue<string>("ExceptionMessageForInvalidPNROrInvalidLastName");
                }
                string exCode = oneClickEnrollmentResponse.Exception.Code;
                oneClickEnrollmentResponse = new MOBOneClickEnrollmentResponse();
                oneClickEnrollmentResponse.Exception = new MOBException(exCode, exMessage);
            }

            return await Task.FromResult(oneClickEnrollmentResponse);
        }

        private MOBCountDownWidgetInfo GetCountDownWidgetInfoConfigValues(int applicationId)
        {
            return new MOBCountDownWidgetInfo()
            {
                SectionTitle = !string.IsNullOrEmpty(_configuration.GetValue<string>("countDownWidgetSectionTitle")) ? _configuration.GetValue<string>("countDownWidgetSectionTitle") : string.Empty,
                SectionDescription = !string.IsNullOrEmpty(_configuration.GetValue<string>("countDownWidgetSectionDescription")) ? _configuration.GetValue<string>("countDownWidgetSectionDescription") : string.Empty,
                InstructionLinkText = !string.IsNullOrEmpty(_configuration.GetValue<string>("countDownWidgetInstructionLinkText")) ? _configuration.GetValue<string>("countDownWidgetInstructionLinkText") : string.Empty,
                InstructionPageTitle = !string.IsNullOrEmpty(_configuration.GetValue<string>("countDownWidgetInstructionPageTitle")) ? _configuration.GetValue<string>("countDownWidgetInstructionPageTitle") : string.Empty,
                InstructionPageContent = applicationId == 1 ? HttpUtility.HtmlDecode(_configuration.GetValue<string>("countDownWidgetInstructionPageContentiOS")) : HttpUtility.HtmlDecode(_configuration.GetValue<string>("countDownWidgetInstructionPageContentAndroid"))
            };
        }

        private MOBPNRByRecordLocatorRequest DecryptPNRRequest(MOBPNRByRecordLocatorRequest request)
        {
            try
            {
                MOBPNRByRecordLocatorRequest mobEncryptedRequest = new MOBPNRByRecordLocatorRequest();

                if (String.Equals(request.Requestor, "MOBILE", StringComparison.OrdinalIgnoreCase))
                {
                    //string urlDecodedString = System.Web.HttpUtility.UrlDecode(encryptedString);
                    string tildaRemovedString = request.EncryptedRequest.Replace("~~", "/");
                    string decryptedString = DecryptString(tildaRemovedString);
                    string[] splitpattern = { ";" };
                    string[] splititems = decryptedString.Split(splitpattern, System.StringSplitOptions.RemoveEmptyEntries);

                    if (splititems == null || !splititems.Any()) return null;

                    splititems.ForEach(splititem =>
                    {
                        if (!string.IsNullOrEmpty(splititem))
                        {
                            string[] item = splititem.Split('=');

                            if (!string.IsNullOrEmpty(item[0])
                              && !string.IsNullOrEmpty(item[1]))
                            {
                                switch (item[0])
                                {
                                    case "RecordLocator":
                                        mobEncryptedRequest.RecordLocator = item[1];
                                        break;
                                    case "LastName":
                                        mobEncryptedRequest.LastName = item[1];
                                        break;
                                }
                            }
                        }
                    });
                }
                else if (String.Equals(request.Requestor, "MARKETING", StringComparison.OrdinalIgnoreCase))
                {
                    string decryptedString;

                    byte[] buffer;
                    if (request.EncryptedRequest.IndexOf("%2F", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        buffer = Convert.FromBase64String(HttpUtility.UrlDecode(request.EncryptedRequest));
                    }
                    else
                    {
                        buffer = Convert.FromBase64String(request.EncryptedRequest);
                    }

                    // This is necessary as this is the style 1.0 uses for encryption
#pragma warning disable S2278 // Neither DES (Data Encryption Standard) nor DESede (3DES) should be used
                    using (TripleDESCryptoServiceProvider crypto = new TripleDESCryptoServiceProvider())
#pragma warning restore S2278 // Neither DES (Data Encryption Standard) nor DESede (3DES) should be used
                    {
                        crypto.Key = new MD5CryptoServiceProvider().ComputeHash(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("cryptoValue"))); ;
                        crypto.IV = Convert.FromBase64String(_configuration.GetValue<string>("cryptoIV"));
                        decryptedString = Encoding.UTF8.GetString(crypto.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length));
                    }
                    var decryptedJsonObject = new { Pnr = "", LastName = "" };
                    decryptedJsonObject = Newtonsoft.Json.JsonConvert.DeserializeAnonymousType(decryptedString, decryptedJsonObject);
                    mobEncryptedRequest.RecordLocator = decryptedJsonObject.Pnr;
                    mobEncryptedRequest.LastName = decryptedJsonObject.LastName;
                }
                return mobEncryptedRequest;
            }
            catch { return null; }
        }

        private string DecryptString(string data)
        {

            return United.ECommerce.Framework.Utilities.SecureData.DecryptString(data);

        }

        private bool ValidateUserEnrollementEligibility(MOBPNRByRecordLocatorResponse response, Collection<Service.Presentation.ReservationModel.Traveler> travelers, Collection<Service.Presentation.PriceModel.Price> prices, Session session)
        {
            try
            {
                bool showEnroll = false;
                var fareType = prices.Any(p => p.FareType == Service.Presentation.CommonEnumModel.FareType.Revenue);
                if (fareType)
                {
                    if (response.PNR.Passengers != null && !string.IsNullOrEmpty(response.PNR.NumberOfPassengers))
                    {
                        foreach (var pax in response.PNR.Passengers.Where(p => p.MileagePlus == null))
                        {
                            var validBillingAddress = BillingAddressValidation(travelers, session);
                            var validPhoneNumber = string.Empty;
                            if (pax.Contact?.PhoneNumbers != null && pax.Contact?.PhoneNumbers.Count() > 0)
                            {
                                var phoneCountryCode = pax.Contact?.PhoneNumbers?.FirstOrDefault().CountryCode;
                                var PhoneNumbers = pax?.Contact?.PhoneNumbers?.FirstOrDefault().PhoneNumber;
                                if (phoneCountryCode.ToUpper() == "US" || phoneCountryCode.ToUpper() == "CA")
                                {
                                    if (PhoneNumbers.Length.ToString() == _configuration.GetValue<string>("OnclickEnrollmentEligibilityCheckCountryCode"))
                                    {
                                        validPhoneNumber = PhoneNumbers;
                                    }
                                }
                                else
                                {
                                    validPhoneNumber = PhoneNumbers;
                                }
                            }
                            if (validBillingAddress != null && !string.IsNullOrEmpty(pax.PassengerName?.First) && !string.IsNullOrEmpty(pax.PassengerName?.Last) && !string.IsNullOrEmpty(pax.BirthDate) && !string.IsNullOrEmpty(validPhoneNumber))
                            {
                                showEnroll = true;
                                break;
                            }
                        }
                    }
                }
                return showEnroll;
            }
            catch
            {
                return false;
            }
        }
        private MOBAddress BillingAddressValidation(Collection<Service.Presentation.ReservationModel.Traveler> travelers, Session session)
        {
            MOBAddress address = null;
            //start
            try
            {
                var allpaymentaddress = travelers?.FirstOrDefault()?.Tickets?.Where(x => x.Payments != null)
                    .SelectMany(x => x.Payments.Where(y => y.BillingAddress != null).Select(z => z.BillingAddress));
                if (allpaymentaddress == null || !allpaymentaddress.Any()) return null;
                var billingaddress = allpaymentaddress.LastOrDefault(x => !string.IsNullOrEmpty(x.AddressLines?.FirstOrDefault(y => !string.IsNullOrEmpty(y)))
                                      && !string.IsNullOrEmpty(x.Country.CountryCode));
                if (billingaddress == null || (_configuration.GetValue<bool>("OneClickValidateAddressEnabled") && (string.IsNullOrEmpty(billingaddress.StateProvince?.StateProvinceCode) || string.IsNullOrEmpty(billingaddress.City) || string.IsNullOrEmpty(billingaddress.PostalCode))))
                    return null;
                address = new MOBAddress
                {
                    Country = new MOBCountry { Code = billingaddress.Country.CountryCode },
                    PostalCode = billingaddress.PostalCode,
                    City = billingaddress.City
                };
                if (!string.IsNullOrEmpty(billingaddress.StateProvince?.StateProvinceCode))
                {
                    address.State = new State { Code = billingaddress.StateProvince.StateProvinceCode };
                }
                foreach (string line in billingaddress.AddressLines)
                {
                    if (string.IsNullOrEmpty(address.Line1)) address.Line1 = line;
                    else if (string.IsNullOrEmpty(address.Line2)) address.Line2 = line;
                    else if (string.IsNullOrEmpty(address.Line3)) address.Line3 = line;
                }
                return address;
            }
            catch { return null; }
        }

        private void SetupRedirectURL(string recordLocator, string lastName, List<MOBItem> urlItems, string urlKey)
        {
            if (urlItems == null)
            {
                urlItems = new List<MOBItem>();
            }
            string urlValue = string.Empty;
            switch (urlKey)
            {
                case "PNRURL":
                    urlValue = "http://" + _configuration.GetValue<string>("DotComOneCancelURL") + "/web/en-US/apps/reservation/import.aspx?OP=1&CN=" +
                    recordLocator +
                    "&LN=" +
                    lastName +
                    "&T=F&MobileOff=1";
                    break;
                case "EDITTRAVELER":
                    urlValue = "https://integration.united.com/web/en-US/apps/reservation/main.aspx?TY=F&AC=ED&CN=" + EncryptString(recordLocator) + "&FLN=" + EncryptString(lastName);
                    break;
            }
            if (!string.IsNullOrEmpty(urlValue))
            {
                MOBItem pnrUrl = new MOBItem();
                pnrUrl.Id = urlKey;
                pnrUrl.CurrentValue = urlValue;
                pnrUrl.SaveToPersist = true;
                urlItems.Add(pnrUrl);
            }
        }

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
        private async Task<MOBAccelerators> GetMileageAndStatusOptions(MOBRequest mobRequest, string sessionId, string correlationId)
        {
            var productOfferFromPersist = new GetOffers();
            productOfferFromPersist = await _sessionHelperService.GetSession<GetOffers>(sessionId, productOfferFromPersist.ObjectName.ToString(), new List<string>() { sessionId, productOfferFromPersist.ObjectName.ToString() });

            var session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string>() { sessionId, session.ObjectName });

            DynamicOfferDetailResponse dodResponse = new DynamicOfferDetailResponse();            
            Service.Presentation.ProductResponseModel.ProductOffer productOffer = new Service.Presentation.ProductResponseModel.ProductOffer();

            if(mobRequest?.Application !=null && _manageResUtility.IsEnableTravelOptionsInViewRes(mobRequest.Application.Id, mobRequest.Application.Version.Major, session?.CatalogItems))
            {
                dodResponse = await GetCCEDODResponse(mobRequest, session, correlationId);
                productOffer = BuildIndividualProductOffer(dodResponse, ProductName.APA.ToString());
            }
            else
                productOffer = BuildIndividualProductOffer(productOfferFromPersist, "APA");

            var mileageAndStatusOptions = new MileageAndStatusOptions(productOffer, sessionId, mobRequest,
                _sessionHelperService, _configuration, _productInfoHelper, _legalDocumentsForTitlesService, _headers,
                _loyaltyMemberProfileService, _customerProfileService, _dynamoDBService);

            return (await (await (await mileageAndStatusOptions.AddTravelers().AddMiles(session.Token))
                   .RemoveMiles().AddCaptions())
                   .AddTermsAndCondtions()).GetAccelerators();
        }

        private async Task<DynamicOfferDetailResponse> GetCCEDODResponse(MOBRequest mobRequest, Session session, string correlationId)
        {
            DynamicOfferDetailResponse dodResponse = new DynamicOfferDetailResponse();
            ReservationDetail cslReservation = await _manageReservation.GetCslReservation(session.SessionId);
            MOBPNR mobPnr = await _manageReservation.GetPNRResponse(session.SessionId);

            if (session != null && cslReservation != null && cslReservation.Detail != null && cslReservation.Detail.FlightSegments != null && mobPnr != null)
            {
                TravelOptionsRequest travelOptionsRequest = new TravelOptionsRequest();
                travelOptionsRequest.TransactionId = mobRequest.TransactionId;
                travelOptionsRequest.SessionId = session.SessionId;
                travelOptionsRequest.Flow = FlowType.VIEWRES.ToString();
                travelOptionsRequest.ProductCode = ProductName.APA.ToString();
                travelOptionsRequest.ProductName = ProductName.APA.ToString();
                travelOptionsRequest.Application = mobRequest.Application;
                travelOptionsRequest.CorrelationId = correlationId;

                dodResponse = await _manageReservation.GetDynamicOfferDetailResponse(travelOptionsRequest, cslReservation, mobPnr, session.Token).ConfigureAwait(false);
            }
            return dodResponse;
        }

        private Service.Presentation.ProductResponseModel.ProductOffer BuildIndividualProductOffer(Service.Presentation.ProductResponseModel.ProductOffer productOffers, string productCode, bool isWaitListPNR = false)
        {
            if (productOffers == null)
                return productOffers;

            var offerResponse = new Service.Presentation.ProductResponseModel.ProductOffer();
            offerResponse.Offers = new Collection<United.Service.Presentation.ProductResponseModel.Offer>();
            United.Service.Presentation.ProductResponseModel.Offer offer = new United.Service.Presentation.ProductResponseModel.Offer();
            offer.ProductInformation = new United.Service.Presentation.ProductResponseModel.ProductInformation();
            offer.ProductInformation.ProductDetails = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, productCode)).ToCollection();
            offerResponse.Offers.Add(offer);
            offerResponse.Travelers = new Collection<United.Service.Presentation.ProductModel.ProductTraveler>();
            offerResponse.Travelers = productOffers.Travelers;
            offerResponse.Solutions = new Collection<United.Service.Presentation.ProductRequestModel.Solution>();
            offerResponse.Solutions = productOffers.Solutions;
            offerResponse.Response = productOffers.Response;
            offerResponse.FlightSegments = new Collection<Service.Presentation.SegmentModel.ProductFlightSegment>();
            offerResponse.FlightSegments = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isWaitListPNR ?
                                           productOffers.FlightSegments.Where(p => p != null && !p.FlightSegmentType.IsNullOrEmpty() && p.FlightSegmentType.ToUpper().Trim().Contains("HK")).ToCollection() :
                                           productOffers.FlightSegments;
            return offerResponse;
        }

        private Service.Presentation.ProductResponseModel.ProductOffer BuildIndividualProductOffer(DynamicOfferDetailResponse productOffers, string productCode, bool isWaitListPNR = false)
        {
            if (productOffers == null || productOffers?.Offers == null || !productOffers.Offers.Any())
                return null;

            var offerResponse = new Service.Presentation.ProductResponseModel.ProductOffer();
            offerResponse.Offers = new Collection<United.Service.Presentation.ProductResponseModel.Offer>();
            United.Service.Presentation.ProductResponseModel.Offer offer = new United.Service.Presentation.ProductResponseModel.Offer();
            offer.ProductInformation = new United.Service.Presentation.ProductResponseModel.ProductInformation();
            offer.ProductInformation.ProductDetails = productOffers.Offers[0].ProductInformation.ProductDetails.Where(x => IsProductMatching(x, productCode)).ToCollection();
            offerResponse.Offers.Add(offer);
            offerResponse.Travelers = new Collection<United.Service.Presentation.ProductModel.ProductTraveler>();
            offerResponse.Travelers = productOffers.Travelers;
            offerResponse.Solutions = new Collection<United.Service.Presentation.ProductRequestModel.Solution>();
            offerResponse.Solutions = productOffers.Solutions;
            offerResponse.Response = productOffers.Response;
            offerResponse.FlightSegments = new Collection<Service.Presentation.SegmentModel.ProductFlightSegment>();
            offerResponse.FlightSegments = _configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes") && isWaitListPNR ?
                                           productOffers.FlightSegments.Where(p => p != null && !p.FlightSegmentType.IsNullOrEmpty() && p.FlightSegmentType.ToUpper().Trim().Contains("HK")).ToCollection() :
                                           productOffers.FlightSegments;
            return offerResponse;
        }
        private bool IsProductMatching(United.Service.Presentation.ProductResponseModel.ProductDetail productDetail, string productCode)
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
        private string GetPNRRedirectUrl(string recordLocator, string lastlName, string reqType)
        {
            string retUrl = string.Empty;
            if (string.Equals(reqType, "EX", StringComparison.OrdinalIgnoreCase))
            {
                retUrl = string.Format("https://{0}/ual/en/US/flight-search/change-a-flight/changeflight/changeflight/rev?PNR={1}&RiskFreePolicy=&TYPE=rev&source=MOBILE",
                 _configuration.GetValue<string>("DotComChangeResBaseUrl", HttpUtility.UrlEncode(EncryptString(recordLocator))));
            }
            else
            {
                if (string.Equals(reqType, "AWARD_CA", StringComparison.OrdinalIgnoreCase))
                {
                    retUrl = string.Format("http://{0}/{1}?TY=F&CN={2}&FLN={3}&source=MOBILE",
                    _configuration.GetValue<string>("DotComOneCancelURL"),
                    _configuration.GetValue<string>("ReShopRedirectPath"),
                    EncryptString(recordLocator),
                    EncryptString(lastlName)
                   );
                }
                else
                {
                    retUrl = string.Format("http://{0}/{1}?TY=F&AC={2}&CN={3}&FLN={4}&source=MOBILE",
                    _configuration.GetValue<string>("DotComOneCancelURL"),
                    _configuration.GetValue<string>("ReShopRedirectPath"),
                    reqType,
                    EncryptString(recordLocator),
                    EncryptString(lastlName)
                   );
                }
            }
            return retUrl;
        }

        private string EncryptString(string data)
        {
            return United.ECommerce.Framework.Utilities.SecureData.EncryptString(data);
        }

        private async Task<List<MOBItem>> SetScheduledChangeMessage(bool hasScheduleChanged)
        {
            if (hasScheduleChanged)
            {
                return await GetCaptions("SCHEDULE_CHANGE_MESSAGES");
            }
            return null;
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

        private async Task<string> PerformInstantUpgradeCSL(string transactionId, string sessionId, string recordLocator, string lastName, string languageCode, int applicationId, string appVersion)
        {
            United.Service.Presentation.ReservationResponseModel.ReservationDetail pnrRetrievalResponse = null;
            string errorMessage = string.Empty;
            string cssToken = string.Empty;
            // call CSL PNR retrieval 
            var tupleRes = await GetPnrDetailsFromCSL(transactionId, recordLocator, lastName, applicationId, appVersion, "PerformInstantUpgradeCSL", cssToken);
            var jsonResponse = tupleRes.jsonResponse;
            cssToken = tupleRes.token;

            try
            {
                if (!string.IsNullOrEmpty(jsonResponse))
                {
                    pnrRetrievalResponse = DataContextJsonSerializer.DeserializeUseContract<United.Service.Presentation.ReservationResponseModel.ReservationDetail>(jsonResponse);
                    if (pnrRetrievalResponse != null && (pnrRetrievalResponse.Error == null || pnrRetrievalResponse.Error.Count == 0))
                    {
                        var instantUpgradeResFlightSegment = new System.Collections.ObjectModel.Collection<ReservationFlightSegment>();

                        if (pnrRetrievalResponse.Detail != null && pnrRetrievalResponse.Detail.FlightSegments != null)
                        {
                            foreach (var rSeg in pnrRetrievalResponse.Detail.FlightSegments)
                            {
                                if (rSeg.FlightSegment != null)
                                {
                                    if (rSeg.FlightSegment.InstantUpgradable) //Per csl need to check this 
                                    {
                                        if (rSeg.FlightSegment.BookingClasses != null && rSeg.FlightSegment.BookingClasses.Count > 0)
                                        {
                                            rSeg.FlightSegment.BookingClasses[0].Code = rSeg.FlightSegment.InstantUpgradeClass;
                                        }
                                        if (rSeg.FlightSegment.Message == null)
                                        {
                                            rSeg.FlightSegment.Message = new System.Collections.ObjectModel.Collection<Message>();
                                        }
                                        rSeg.FlightSegment.Message.Add(new United.Service.Presentation.SegmentModel.Message
                                        {
                                            Text = _configuration.GetValue<string>("performUpgradeCSLText") //"INSTANT UPGRADE" //per csl this text should be constant
                                        });
                                        if (rSeg.FlightSegment.TravelerCounts == null)// per cls need to initialize the TravelerCount
                                        {
                                            rSeg.FlightSegment.TravelerCounts = new System.Collections.ObjectModel.Collection<TravelerCount>();
                                        }

                                        instantUpgradeResFlightSegment.Add(rSeg);
                                    }
                                }

                            }

                        }

                        if (instantUpgradeResFlightSegment.Count > 0)
                        {
                            string instantUpgradeRequest = string.Empty;
                            if (pnrRetrievalResponse.Detail != null)
                            {
                                instantUpgradeRequest = DataContextJsonSerializer.Serialize(instantUpgradeResFlightSegment);
                            }

                            string instantUpgradeurl = string.Format("/{0}/FlightSegments?RetrievePNR=True&EndTransaction=True&RestoreFareQuote=False", recordLocator);

                            var jsonResponseFrominstantUpgrade = await _pNRRetrievalService.RetrievePNRDetailCSL(instantUpgradeurl, cssToken, instantUpgradeRequest);

                        }
                        else
                        {
                            //errorMessage = "We are unable to perform instant upgrade at this time.";
                            // Bugs 95495, 95496 - Instant upgrades are not informing customers that the upgrade is no longer available - j.srinivas
                            if (_configuration.GetValue<string>("PerformInstantupgradesnoLongerAvailable") != null)
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("PerformInstantupgradesnoLongerAvailable"));
                            }
                        }
                    }

                }
            }
            catch (WebException wex)
            {
                if (_configuration.GetValue<bool>("BugFixToggleFor18B"))
                {
                    try
                    {
                        HandleWebException(transactionId, "", applicationId, appVersion, wex, "PerformInstantUpgradeCSL");
                    }
                    catch
                    {  //Bug 236429: mAPPs: “Unexpected shares response “error message displayed when tap on Confirm upgrade button in Confirm upgrade screen for Premier instant upgrade in view Res
                        throw new MOBUnitedException(_configuration.GetValue<string>("UnableToPerformInstantUpgradeErrorMessage"));
                    }
                }
                else
                {
                    HandleWebException(transactionId, "", applicationId, appVersion, wex, "PerformInstantUpgradeCSL");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PerformInstantUpgradeCSL Exception{exception}", JsonConvert.SerializeObject(ex));

                errorMessage = "We are unable to perform instant upgrade at this time.";
            }

            return errorMessage;
        }

        public void HandleWebException(string transactionId, string deviceId, int applicationId, string appVersion,
            WebException wex, string methodName)
        {
            if (wex.Response != null)
            {
                var errorResponse = new StreamReader(wex.Response.GetResponseStream()).ReadToEnd();

                var message = new MOBFlightStatusError();

                string errorMessage = string.Empty;
                try
                {
                    // This is deserialized using a class created by Mobile team. We wantto deserialize using the same class used by CSL, but we are getting errors. United.Foundations.Practices.Framework.ServiceException
                    // Follow up 
                    message = DataContextJsonSerializer.DeserializeJsonDataContract<MOBFlightStatusError>(errorResponse);

                    //LogEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBFlightStatusError>(transactionId, methodName, "Exception", applicationId, appVersion, deviceId, message));

                    errorMessage = message.Message;
                }
                catch (System.Exception ex)
                {
                    var xmlError = ProviderHelper.SerializeXml(errorResponse);

                    //LogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(transactionId, methodName, "CSLResponse", applicationId, appVersion, deviceId, xmlError));

                    throw new System.Exception(ex.Message);
                }

                if (message.Errors != null && message.Errors.Length > 0)
                {
                    //string errorMessage = string.Empty;
                    foreach (var error in message.Errors)
                    {
                        errorMessage = errorMessage + " " + error.MinorDescription;

                        // Added By Ali as part of Task 264627 : Booking flow exception analysis - Perform Instant Upgrade CSL
                        if (_configuration.GetValue<bool>("BugFixToggleForExceptionAnalysis") && !string.IsNullOrEmpty(error.MinorCode) && (error.MinorCode.Trim().Equals("40030") || error.MinorCode.Trim().Equals("10028")))
                        {
                            throw new MOBUnitedException(_configuration.GetValue<string>("UnableToPerformInstantUpgradeErrorMessage"));
                        }
                    }

                    if (errorMessage.Contains("Unable to retrieve PNR"))
                    {
                        // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBFlightStatusError>(transactionId, methodName, "UnitedException", applicationId, appVersion, deviceId, message));
                        throw new MOBUnitedException("The confirmation number entered is invalid."); // bug bounty fix to show generic message
                    }
                    else
                    {
                        // LogEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBFlightStatusError>(transactionId, methodName, "Exception", applicationId, appVersion, deviceId, message));
                    }
                    throw new MOBUnitedException(errorMessage);
                }
                throw new System.Exception(wex.Message);
            }
            else
            {
                throw new System.Exception(wex.Message);
            }
        }
        private async Task<(string jsonResponse, string token)> GetPnrDetailsFromCSL(string transactionId, string recordLocator, string lastName, int applicationId, string appVersion, string actionName, string token, bool usedRecall = false)
        {
            var request = new RetrievePNRSummaryRequest();

            if (!usedRecall)
            {
                request.Channel = _configuration.GetValue<string>("ChannelName");
                request.IsIncludeETicketSDS = _configuration.GetValue<string>("IsIncludeETicketSDS");
                request.IsIncludeFlightRange = _configuration.GetValue<string>("IsIncludeFlightRange");
                request.IsIncludeFlightStatus = _configuration.GetValue<string>("IsIncludeFlightStatus");
                request.IncludeManageResDetails = _configuration.GetValue<string>("IncludeManageResDetails");
                request.IsUpgradeDetails = _configuration.GetValue<string>("IsUpgradeDetails");
                if (_configuration.GetValue<bool>("EnablePCUWaitListPNRManageRes"))
                {
                    request.IsUpgradeDetailsWithEMD = _configuration.GetValue<string>("IsUpgradeDetailsWithEMD");
                }
                request.IsIncludePNRChangeEligibility = _configuration.GetValue<string>("IsIncludePNRChangeEligibility");
                request.IsIncludeLMX = _configuration.GetValue<string>("IsIncludeLMX");
                request.IsIncludePNRDB = _configuration.GetValue<string>("IsIncludePNRDB");
                request.IsIncludeSegmentDuration = _configuration.GetValue<string>("IsIncludeSegmentDuration");
                request.ConfirmationID = recordLocator.ToUpper();
                request.LastName = lastName;
                request.PNRType = string.Empty; //per csl to get the data from cache use PNRType=”CACHED”
                request.FilterHours = _configuration.GetValue<string>("FilterHours");
                request.IsIncludeChangeFee = _configuration.GetValue<bool>("IsIncludeChangeFee");
                request.IsDestinationImagesRequired = _configuration.GetValue<string>("IsDestinationImagesRequired");
                if (_configuration.GetValue<bool>("IsIncludeTravelWaiverDetail"))
                {
                    request.IsIncludeTravelWaiverDetail = _configuration.GetValue<bool>("IsIncludeTravelWaiverDetail");
                }
            }
            else
            {
                request.Channel = _configuration.GetValue<string>("ChannelName");
                request.IsIncludeETicketSDS = _configuration.GetValue<string>("IsIncludeETicketSDS");
                request.IsIncludeFlightRange = _configuration.GetValue<string>("IsIncludeFlightRange");
                request.IsIncludeFlightStatus = _configuration.GetValue<string>("IsIncludeFlightStatus");
                request.IsIncludeLMX = _configuration.GetValue<string>("IsIncludeLMX");
                request.IsIncludePNRDB = _configuration.GetValue<string>("IsIncludePNRDB");
                request.IsIncludeSegmentDuration = _configuration.GetValue<string>("IsIncludeSegmentDuration");
                request.ConfirmationID = recordLocator.ToUpper();
                request.LastName = lastName;
                request.PNRType = string.Empty; //per csl to get the data from cache use PNRType=”CACHED”                
            }

            var jsonResponse = await RetrievePnrDetailsFromCsl(applicationId, transactionId, request, token);
            token = jsonResponse.token;
            return (jsonResponse.jsonResponse, token);
        }

        private async Task<(string jsonResponse, string token)> RetrievePnrDetailsFromCsl(int applicationId, string TransactionId, RetrievePNRSummaryRequest request, string token)
        {

            var jsonRequest = System.Text.Json.JsonSerializer.Serialize<RetrievePNRSummaryRequest>(request);

            token = await _dPService.GetAnonymousToken(applicationId, _headers.ContextValues.DeviceId, _configuration);

            string path = "/PNRRetrieval";
            var jsonResponse = string.Empty;
            jsonResponse = await _pNRRetrievalService.RetrievePNRDetail(token, jsonRequest, TransactionId, path);

            return (jsonResponse, token);
        }

        private MOBOneClickEnrollmentResponse GetOneClickEnrollmentConfigValues(MOBPNRByRecordLocatorRequest request)
        {
            var oneClickEnrollmentConfigValues = new MOBOneClickEnrollmentResponse()
            {
                SessionId = request.SessionId,
                DeviceId = request.DeviceId,
                LastName = request.LastName,
                RecordLocator = request.RecordLocator,
                DateCreated = System.DateTime.Now.ToString(),
                TransactionId = request.TransactionId,
                Title = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusCreateAccountText")) ? _configuration.GetValue<string>("joinMileagePlusCreateAccountText") : string.Empty,
                Header = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusHeader")) ? _configuration.GetValue<string>("joinMileagePlusHeader") : string.Empty,
                Description = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusDescription")) ? _configuration.GetValue<string>("joinMileagePlusDescription") : string.Empty,
                EmailAddressHint = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusEmailAddressText")) ? _configuration.GetValue<string>("joinMileagePlusEmailAddressText") : string.Empty,
                SelectTravelerHeader = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusSelectTravelerText")) ? _configuration.GetValue<string>("joinMileagePlusSelectTravelerText") : string.Empty,
                SendMarketEmailText = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusMarketingEmailsText")) ? _configuration.GetValue<string>("joinMileagePlusMarketingEmailsText") : string.Empty,
                EmailInvalidError = !string.IsNullOrEmpty(_configuration.GetValue<string>("emailInvalidError")) ? _configuration.GetValue<string>("emailInvalidError") : string.Empty,
                EmailDuplicateError = !string.IsNullOrEmpty(_configuration.GetValue<string>("emailDuplicateError")) ? _configuration.GetValue<string>("emailDuplicateError") : string.Empty,
                CancelButton = !string.IsNullOrEmpty(_configuration.GetValue<string>("cancelButton")) ? _configuration.GetValue<string>("cancelButton") : string.Empty,
                CreateAccountButton = !string.IsNullOrEmpty(_configuration.GetValue<string>("creatAccountButton")) ? _configuration.GetValue<string>("creatAccountButton") : string.Empty,
                TermsAndConditionsContent = !string.IsNullOrEmpty(_configuration.GetValue<string>("termsAndConditionsContent")) ? _configuration.GetValue<string>("termsAndConditionsContent") : string.Empty,
                TermsAndConditionsText = !string.IsNullOrEmpty(_configuration.GetValue<string>("termsAndConditionsText")) ? _configuration.GetValue<string>("termsAndConditionsText") : string.Empty,
                Under18EnrollmentMessage = !string.IsNullOrEmpty(_configuration.GetValue<string>("under18EnrollmentMessage")) ? _configuration.GetValue<string>("under18EnrollmentMessage") : string.Empty,
                PrivacyPolicyText = !string.IsNullOrEmpty(_configuration.GetValue<string>("privacyPolicyText")) ? _configuration.GetValue<string>("privacyPolicyText") : string.Empty
            };
            var benefit1 = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusSaveYourTripBenefit")) ? _configuration.GetValue<string>("joinMileagePlusSaveYourTripBenefit") : string.Empty;
            var benefit2 = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusUpcomingTripBenefit")) ? _configuration.GetValue<string>("joinMileagePlusUpcomingTripBenefit") : string.Empty;
            var benefit3 = !string.IsNullOrEmpty(_configuration.GetValue<string>("joinMileagePlusExclusiveDealsBenefit")) ? _configuration.GetValue<string>("joinMileagePlusExclusiveDealsBenefit") : string.Empty;
            oneClickEnrollmentConfigValues.Benefits = new List<MOBKVP>();
            var KeyValues = new List<MOBKVP>() {
                            new MOBKVP (){ Key="ImageUrl1", Value = benefit1},
                            new MOBKVP (){ Key="ImageUrl2", Value = benefit2},
                            new MOBKVP (){ Key="ImageUrl3", Value = benefit3}
                           };
            oneClickEnrollmentConfigValues.Benefits = KeyValues;
            oneClickEnrollmentConfigValues.Flow = request.Flow;
            return oneClickEnrollmentConfigValues;
        }

        //ConfirmScheduleChange

        public async Task<United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse> ConfirmScheduleChange(United.Mobile.Model.ReShop.MOBConfirmScheduleChangeRequest request)
        {
            Session session = null;

            var response = new United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse();

            _logger.LogInformation("ConfirmScheduleChange {@clientRequest} and {SessionId}", request, request.SessionId);

            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _sessionHelperService.GetSession<Session>(request.SessionId, new Session().ObjectName, new List<string>() { request.SessionId, new Session().ObjectName });

                //session = Utility.CreateShoppingSession
                //    (request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId,
                //    null, new List<LogEntry>(), _traceSwitch, string.Empty);
            }
            if (session == null)
            {
                if (response.Exception == null) response.Exception = new MOBException();
                response.Exception.Message = _configuration.GetValue<string>("GeneralSessionExpiryMessage");
                response.Exception.Code = "9999";
                return response;
            }

            request.Token = session.Token;
            response.SessionId = request.SessionId;

            response = await ConfirmScheduleChangeCSL(request);

            if (response.Exception == null)
            {
                var mobPnrRequest = new MOBPNRByRecordLocatorRequest();

                //GetPNRByRecordLocator - Request Mapping
                mobPnrRequest.Application = new MOBApplication();
                mobPnrRequest.Application = request.Application;
                mobPnrRequest.SessionId = request.SessionId;
                mobPnrRequest.DeviceId = request.DeviceId;
                mobPnrRequest.TransactionId = request.TransactionId;
                mobPnrRequest.RecordLocator = request.RecordLocator;
                mobPnrRequest.LastName = request.LastName;
                mobPnrRequest.MileagePlusNumber = request.MileagePlusNumber;
                mobPnrRequest.HashKey = request.HashKey;
                mobPnrRequest.Flow = Convert.ToString(FlowType.VIEWRES);

                response.PNRResponse = await GetPNRByRecordLocator(mobPnrRequest);
                response.SessionId = response.PNRResponse?.SessionId;
            }

            response.RecordLocator = request.RecordLocator;
            response.LastName = response.LastName;
            response.MileagePlusNumber = request.MileagePlusNumber;
            response.FlowType = request.FlowType;
            response.DeviceId = request.DeviceId;
            response.TransactionId = request.TransactionId;
            response.SelectedOption = request.SelectedOption;

            _logger.LogInformation("ConfirmScheduleChange {@clientResponse} and {SessionId}", response, request.SessionId);
            return response;
        }

        private async Task<United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse> ConfirmScheduleChangeCSL(United.Mobile.Model.ReShop.MOBConfirmScheduleChangeRequest schedulechangerequest)
        {
            var schedulechangeresponse = new United.Mobile.Model.ReShop.MOBConfirmScheduleChangeResponse();

            try
            {
                var cslResponse = await _reservationService.ConfirmScheduleChange<List<United.Service.Presentation.CommonModel.Message>>(schedulechangerequest.Token, schedulechangerequest.RecordLocator, schedulechangerequest.SessionId);

                if (!string.IsNullOrEmpty(cslResponse))
                {
                    var cslResponseString = JsonConvert.DeserializeObject<List<United.Service.Presentation.CommonModel.Message>>(cslResponse);
                    if (cslResponseString == null || !cslResponseString.Any())
                    {
                        schedulechangeresponse.Exception
                        = new MOBException("9999", _configuration.GetValue<string>("PNRConfmScheduleChangeExcMessage"));
                    }
                }
            }
            catch (Exception ex)
            {
                string errormessage = TopHelper.ExceptionMessages(ex);
                throw new MOBUnitedException(errormessage);
            }

            return schedulechangeresponse;
        }

        public async Task<MOBGetActionDetailsForOffersResponse> GetActionDetailsForOffers(MOBGetActionDetailsForOffersRequest request)
        {
            var response = new MOBGetActionDetailsForOffersResponse();
            var offerRequestData = JsonConvert.DeserializeObject<OfferRequestData>(request.Data);

            if (offerRequestData != null && !string.IsNullOrEmpty(offerRequestData.View) && offerRequestData.View.Equals("ViewResSeatMap", StringComparison.InvariantCultureIgnoreCase))
            {
                var reservation = await CompleteGetPNRByRecordLocatorAndReturnSessionId(request, offerRequestData);
                response.SessionId = reservation.SessionId;
                string sharesIndex = GetSharesIndex(reservation, offerRequestData.FocusRequestData);

                response.Data = await GetSeatMapResponseJson(request, offerRequestData, reservation.SessionId, sharesIndex).ConfigureAwait(false);
                response.ViewName = offerRequestData.View;
            }

            return await Task.FromResult(response);
        }

        public async Task<MOBPNR> CompleteGetPNRByRecordLocatorAndReturnSessionId(MOBGetActionDetailsForOffersRequest request, OfferRequestData offerRequestData)
        {

            var session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, string.Empty, false, true);

            var pnrByRecordLocatorRequest = new MOBPNRByRecordLocatorRequest
            {
                Application = request.Application,
                AccessCode = request.AccessCode,
                SessionId = session.SessionId,
                LanguageCode = request.LanguageCode,
                RecordLocator = offerRequestData.RecordLocator,
                LastName = offerRequestData.LastName,
                DeviceId = request.DeviceId,
                TransactionId = request.TransactionId,
                MileagePlusNumber = request.MileagePlusNumber,
                Flow = United.Utility.Enum.FlowType.VIEWRES_SEATMAP.ToString(),
                CatalogValues = request.CatalogValues
            };

            //MOBILE-32580. fix with UI driven catalog toggle enableBundlesInHomeScreen(12152,22152) & then only UI will send existing TravelOptions catalog values EnableBundlesInManageRes(11741,21741) in request - for Homescreen Free SeatChange
            if (request.CatalogValues != null && request.CatalogValues.Any())
            {
                session.CatalogItems = request.CatalogValues;
                await _sessionHelperService.SaveSession<Session>(session, session.SessionId, new List<string> { session.SessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);
            }

            _headers.ContextValues.SessionId = session.SessionId;
            var response = await _manageReservation.GetPNRByRecordLocatorCommonMethod(pnrByRecordLocatorRequest);
            return response.PNR;
        }

        private string GetSharesIndex(MOBPNR reservation, SeatFocusRequest focusRequestData)
        {
            string sharesIndex = string.Empty;

            if (reservation != null && reservation.Passengers != null && focusRequestData != null)
            {
                foreach (var passenger in reservation.Passengers)
                {
                    if (CompareStrings(passenger.PassengerName.First, focusRequestData.FirstName)
                        && CompareStrings(passenger.PassengerName.Last, focusRequestData.LastName)
                        && CompareStrings(passenger.PassengerName.Middle, focusRequestData.MiddleName))
                    {
                        sharesIndex = passenger.SHARESPosition;
                    }
                }
            }

            return sharesIndex;
        }

        private bool CompareStrings(string s1, string s2)
        {
            if (string.IsNullOrEmpty(s1) && string.IsNullOrEmpty(s2))
                return true;

            return string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase);
        }

        private async Task<string> GetSeatMapResponseJson(MOBGetActionDetailsForOffersRequest request, OfferRequestData offerRequestData, string sessionId, string sharesIndex = "")
        {
            //flagcheck 
            bool enableSeatChangeFromTripDetails = _configuration.GetValue<bool>("EnableSeatChangeFromTripDetails");

            MOBSeatFocus seatFocus = default(MOBSeatFocus);

            if (enableSeatChangeFromTripDetails && offerRequestData.FocusRequestData != null)
            {
                if (offerRequestData.FocusRequestData.FirstNameIndex > 0 && offerRequestData.FocusRequestData.LastNameIndex > 0)
                {
                    seatFocus = new MOBSeatFocus
                    {
                        SharesIndex = $"{offerRequestData.FocusRequestData.LastNameIndex}.{offerRequestData.FocusRequestData.FirstNameIndex}",
                        Origin = offerRequestData.Origin,
                        Destination = offerRequestData.Destination
                    };
                }
                else if (!string.IsNullOrEmpty(sharesIndex))
                {
                    seatFocus = new MOBSeatFocus
                    {
                        SharesIndex = sharesIndex,
                        Origin = offerRequestData.Origin,
                        Destination = offerRequestData.Destination
                    };
                }
            }

            var seatMapReq = new MOBSeatChangeInitializeRequest()
            {
                Application = request.Application,
                AccessCode = request.AccessCode,
                SessionId = sessionId,
                LanguageCode = request.LanguageCode,
                RecordLocator = offerRequestData.RecordLocator,
                LastName = offerRequestData.LastName,
                DeviceId = request.DeviceId,
                TransactionId = request.TransactionId,
                Flow = United.Utility.Enum.FlowType.VIEWRES_SEATMAP.ToString(),
                OffersRequestData = request.Data,
                SeatFocusRequest = seatFocus,
                CatalogValues = request.CatalogValues,
                TravelerSignInData = request.TravelerSignInData
            };

            var seatMapResponse = await _manageReservation.SeatChangeInitialize(seatMapReq).ConfigureAwait(false);

            if (seatMapResponse == null || seatMapResponse.Exception == null && seatMapResponse.SeatMap == null)
            {
                throw new MOBUnitedException("Currently unable to load the seat map for the selected flight");
            }


            bool IsTripTipSeatChangeEnumFixEnabled = await _featureSettings.GetFeatureSettingValue("EnableTripTipSeatChangeEnumFix");

            var formater = IsTripTipSeatChangeEnumFixEnabled ?
                 new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver(), Converters = new List<JsonConverter>() { new StringEnumConverter() } }
               : new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

            return JsonConvert.SerializeObject(seatMapResponse,formater);
        }

        public async Task<TravelOptionsResponse> GetProductOfferAndDetails(TravelOptionsRequest request)
        {
            Session session = null;

            var response = new TravelOptionsResponse();

            _logger.LogInformation("GetProductOfferAndDetails {@clientRequest} and {SessionId}", request, request.SessionId);

            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _sessionHelperService.GetSession<Session>(request.SessionId, new Session().ObjectName, new List<string>() { request.SessionId, new Session().ObjectName });
            }
            if (session == null)
            {
                if (response.Exception == null) response.Exception = new MOBException();
                response.Exception.Message = _configuration.GetValue<string>("ViewResSessionExpiredMessage");
                response.Exception.Code = "9999";
                return response;
            }

            if (request == null || string.IsNullOrEmpty(request.ProductName) || string.IsNullOrEmpty(request.ProductCode))
            {
                _logger.LogError("GetProductOfferAndDetails - Exception{exception}", _configuration.GetValue<string>("TravelOptions_ProductOfferDetails_LogErrorMessage"));
                response.Exception
                        = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
                return response;
            }
            response = await _manageReservation.GetProductOfferAndDetails(request, session);
            response.SessionId = request.SessionId;
            return await Task.FromResult(response);
        }

        public  void PostBaggageEventMessage(dynamic request)
        {
            try
            {
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                BaggageEvents baggageEvents = new BaggageEvents();
                dynamic bagEvent = JsonConvert.DeserializeObject(request.ToString(), jsonSettings);
                string bagEventText = bagEvent.text;
                baggageEvents = JsonConvert.DeserializeObject<BaggageEvents>(bagEventText, jsonSettings);
                if(baggageEvents?.Bag != null)
                {
                    string pnrNumber = baggageEvents.Bag.BsmPNRNumber;
                    string bagTagNumber = baggageEvents.Bag.BagTagNumber;
                    Int64 bagTagUniqueKey = baggageEvents.Bag.BagTagUniqueKey;
                    bool isActive = baggageEvents.Bag.IsActive;
                    string firstName = baggageEvents.Bag.BsmFirstName;
                    string lastName = baggageEvents.Bag.BsmLastName;
                    Task.Factory.StartNew(() => _auroraMySqlService.InsertBaggageEventMessage(pnrNumber, bagTagNumber, bagTagUniqueKey, isActive, firstName, lastName));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("PostBaggageEventMessage - Exception{exception}", ex.Message);
                // throw;
            }
        }

    }
}



