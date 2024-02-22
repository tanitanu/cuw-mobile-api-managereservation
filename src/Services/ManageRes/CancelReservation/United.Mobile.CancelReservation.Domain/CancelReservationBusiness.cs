using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using United.Common.Helper;
using United.Common.Helper.FOP;
using United.Common.Helper.ManageRes;
using United.Common.Helper.Shopping;
using United.Mobile.DataAccess.CancelReservation;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.Shopping;
using United.Service.Presentation.ReservationResponseModel;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;
using Status = United.Mobile.Model.ManageRes.Status;
using United.Utility.Extensions;

namespace United.Mobile.CancelReservation.Domain
{
    public class CancelReservationBusiness : ICancelReservationBusiness
    {
        private readonly ICacheLog<CancelReservationBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IShoppingSessionHelper _shoppingSessionHelper;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly ManageResUtility _manageResUtility;
        private readonly string _CURRENCYCODE = "USD";
        private readonly string _keyBASEFARE = "BASEFARE";
        private readonly string _keyTAXANDFEE = "TAXANDFEE";
        private readonly string _keyCANCELFEE = "CANCELFEE";
        private readonly string _BASEFARE = "Base fare: {0}";
        private readonly string _TAXANDFEE = "Taxes and fees: {0}";
        private readonly string _CANCELFEE = "Cancellation fees: {0}";
        private readonly string _TOTALCREDIT = "Total Credit";
        private readonly string _TOTALREFUND = "Total Refund";
        private readonly string _AWARDCREDIT = "Award Credit";
        private readonly string CurrencyCode = "USD";
        private readonly IRegisterCFOP _registerCFOP;
        private readonly IPaymentUtility _paymentUtility;
        private readonly IDPService _dPService;
        private readonly ICancelRefundService _cancelRefundService;
        private readonly ICancelAndRefundService _cancelAndRefundService;
        private readonly IHeaders _headers;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly AirportDynamoDB _airportDynamoDB;
        private readonly IValidateHashPinService _validateHashPinService;
        private readonly ILegalDocumentsForTitlesService _legalDocumentsForTitlesService;
        private readonly IMPSignInCommonService _mPSignInCommonService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFeatureSettings _featureSettings;

        public CancelReservationBusiness(ICacheLog<CancelReservationBusiness> logger,
            IConfiguration configuration, IShoppingSessionHelper shoppingSessionHelper,
            ISessionHelperService sessionHelperService, IPaymentUtility paymentUtility,
            IRegisterCFOP registerCFOP, IDPService dPService,
            ICancelRefundService cancelRefundService,
            ICancelAndRefundService cancelAndRefundService
            , IHeaders headers
            , IDynamoDBService dynamoDBService
            , IValidateHashPinService validateHashPinService
            , ILegalDocumentsForTitlesService legalDocumentsForTitlesService
            , IMPSignInCommonService mPSignInCommonService
            , IHttpContextAccessor httpContextAccessor
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _shoppingSessionHelper = shoppingSessionHelper;
            _sessionHelperService = sessionHelperService;
            _paymentUtility = paymentUtility;
            _registerCFOP = registerCFOP;
            _dPService = dPService;
            _cancelRefundService = cancelRefundService;
            _cancelAndRefundService = cancelAndRefundService;
            _headers = headers;
            _dynamoDBService = dynamoDBService;
            _validateHashPinService = validateHashPinService;
            _legalDocumentsForTitlesService = legalDocumentsForTitlesService;
            _mPSignInCommonService = mPSignInCommonService;
            _httpContextAccessor = httpContextAccessor;
            _featureSettings = featureSettings;
            _airportDynamoDB = new AirportDynamoDB(_configuration, _dynamoDBService);
            _manageResUtility = new ManageResUtility(_configuration, _legalDocumentsForTitlesService, _dynamoDBService, _headers, _logger);
            ConfigUtility.UtilityInitialize(_configuration);
        }

        public async Task<MOBCancelRefundInfoResponse> CheckinCancelRefundInfo(MOBCancelRefundInfoRequest request)
        {
            MOBCancelRefundInfoResponse cancelRefundInfoResponse = new MOBCancelRefundInfoResponse();
            string logAction = string.Empty;
            var session = new Session();
            var persisteligibility = new EligibilityResponse();

            session = await _shoppingSessionHelper.CreateShoppingSession(request.Application.Id, request.DeviceId, request.Application.Version.Major,
                request.TransactionId, request.MileagePlusNumber, string.Empty,
                isReshop: true, isAward: request.IsAward);

            session.Flow = Convert.ToString(FlowType.CHECKINSDC);
            request.SessionId = session.SessionId;
            logAction = request.IsAward ? "Award" : "Revenue";

            //if (!string.IsNullOrEmpty(request.SessionId))
            //{
            //session = Utility.GetBookingFlowSession(request.SessionId, false);
            //request.IsAward = session.IsAward;
            //logAction = request.IsAward ? "Award" : "Revenue";

            //persisteligibility
            //    = Persist.FilePersist.Load<United.Persist.Definition.Reshopping.EligibilityResponse>(request.SessionId, persisteligibility.ObjectName);
            //request.isPNRELF = (persisteligibility == null) ? false : persisteligibility.IsElf;
            //}

            if (!string.IsNullOrEmpty(request.PaxIndexes))
            {
                cancelRefundInfoResponse.Token = request.Token;
                cancelRefundInfoResponse.SessionId = request.SessionId;
                cancelRefundInfoResponse.RedirectURL
                    = GetTripDetailRedirect3dot0Url(request.PNR, request.LastName, ac: "CA", channel: "mobile", languagecode: "en/US");
                cancelRefundInfoResponse.Exception = new MOBException("8999", _configuration.GetValue<string>("Checkin-IneligibleMessage"));
                return cancelRefundInfoResponse;
            }

            //ValidateRequest(request);
            request.IsVersionAllowAwardCancel = true;

            cancelRefundInfoResponse = await CancelRefundInfo(request, persisteligibility);

            if (cancelRefundInfoResponse.IsAgencyBooking
                && cancelRefundInfoResponse?.ReservationDetail?.Detail?.Characteristic != null
                && cancelRefundInfoResponse.ReservationDetail.Detail.Characteristic.Any())
            {
                var agencyCharacteristics
                    = cancelRefundInfoResponse.ReservationDetail.Detail.Characteristic.FirstOrDefault
                    (x => string.Equals(x.Code, "Booking Source", StringComparison.OrdinalIgnoreCase));

                if (agencyCharacteristics != null && string.IsNullOrEmpty(agencyCharacteristics.Value) == false)
                {
                    if (!AllowAgencyToChangeCancel(agencyCharacteristics.Value, requesttype: "CANCEL"))
                    {
                        var agencymsginfo = await _manageResUtility.GetDBDisplayContent("Reshop_ChangeCancelMsg_Content");
                        var msg = (agencymsginfo != null) ? agencymsginfo.FirstOrDefault
                            (x => string.Equals(x.Id, "cancelAgency", StringComparison.OrdinalIgnoreCase)) : null;
                        if (msg != null)
                        {
                            cancelRefundInfoResponse.Exception = SetCustomMobExceptionMessage(msg.CurrentValue, string.Empty);
                            return cancelRefundInfoResponse;
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(cancelRefundInfoResponse.RedirectURL))
            {
                cancelRefundInfoResponse.RedirectURL
                    = GetTripDetailRedirect3dot0Url(request.PNR, request.LastName, ac: "CA", channel: "mobile", languagecode: "en/US");
            }


            if (request.IsAward)
            {
                cancelRefundInfoResponse.AwardTravel = request.IsAward;
                var cslReservation
                    = await _sessionHelperService.GetSession<ReservationDetail>(request.SessionId, new ReservationDetail().GetType().FullName, new List<string> { request.SessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);

                if (cslReservation != null
                    && cslReservation.Detail != null
                    && cslReservation.Detail.Sponsor != null)
                {
                    cancelRefundInfoResponse.SponsorMileagePlus = cslReservation.Detail.Sponsor.LoyaltyProgramMemberID;

                    if (cancelRefundInfoResponse.CancelPathEligible)
                    {
                        if (cancelRefundInfoResponse.Pricing != null)
                        {
                            cancelRefundInfoResponse.Pricing.RefundMilesLabel
                                = string.Format("To MileagePlus account ****{0}", cancelRefundInfoResponse?.SponsorMileagePlus?
                                .Substring(cancelRefundInfoResponse.SponsorMileagePlus.Length - 3));
                        }
                    }

                    if (_configuration.GetValue<bool>("EnableByPassSponsorMPCheck") &&
                        !string.IsNullOrEmpty(request.MileagePlusNumber))
                    {
                        cancelRefundInfoResponse.SponsorMileagePlus
                            = string.IsNullOrEmpty(request.MileagePlusNumber) ? string.Empty : request.MileagePlusNumber;
                    }
                }
            }

            if (!cancelRefundInfoResponse.CancelPathEligible)
            {
                if (EnableCancelWebSSO(request.Application.Id, request.Application.Version.Major))
                {
                    if (!string.IsNullOrEmpty(request.MileagePlusNumber) && !string.IsNullOrEmpty(request.HashPinCode))
                    {
                        bool validWalletRequest = false;
                        string authToken = string.Empty;

                        if (!_configuration.GetValue<bool>("DisableFixforHashPinCheckinCancelRefundInfo"))
                        {
                            HashPin hashPin = new HashPin(_logger, _configuration, _validateHashPinService, _dynamoDBService, _mPSignInCommonService, _headers, _httpContextAccessor, _featureSettings);
                            var tupleRes = await hashPin.ValidateHashPinAndGetAuthToken
                                            (request.MileagePlusNumber, request.HashPinCode, request.Application.Id, request.DeviceId, request.Application.Version.Major, authToken, session.SessionId);

                            validWalletRequest = tupleRes.returnValue;
                            authToken = tupleRes.validAuthToken;
                        }
                        else
                        {
                            var response = await new HashPin(_logger, _configuration, _validateHashPinService, _dynamoDBService, _mPSignInCommonService, _headers, _httpContextAccessor, _featureSettings).ValidateHashPinAndGetAuthTokenDynamoDB(request.MileagePlusNumber, request.HashPinCode, request.Application.Id, request.DeviceId, request.Application.Version.Major, request.SessionId).ConfigureAwait(false);
                        }
                        
                        if (!validWalletRequest)
                            throw new MOBUnitedException(_configuration.GetValue<string>("bugBountySessionExpiredMsg"));

                        if (validWalletRequest)
                        {
                            cancelRefundInfoResponse.TransactionId = request.TransactionId;
                            cancelRefundInfoResponse.LanguageCode = request.LanguageCode;

                            cancelRefundInfoResponse.WebShareToken = _dPService.GetSSOTokenString
                                (request.Application.Id, request.MileagePlusNumber, _configuration);
                            if (!String.IsNullOrEmpty(cancelRefundInfoResponse.WebShareToken))
                            {
                                cancelRefundInfoResponse.WebSessionShareUrl = _configuration.GetValue<string>("DotcomSSOUrl");

                                if (await _featureSettings.GetFeatureSettingValue("EnableRedirectURLUpdate").ConfigureAwait(false))
                                {
                                    cancelRefundInfoResponse.RedirectURL = $"{_configuration.GetValue<string>("NewDotcomSSOUrl")}?type=sso&token={cancelRefundInfoResponse.WebShareToken}&landingUrl={cancelRefundInfoResponse.RedirectURL}";
                                    cancelRefundInfoResponse.WebSessionShareUrl = cancelRefundInfoResponse.WebShareToken = string.Empty;
                                }
                            }

                        }
                    }
                }
            }
            // 676774 Presisting the quote refund object to recall Refund Miles in CancelAndRefund call
            if (request.SessionId != null)
            {
                await _sessionHelperService.SaveSession<MOBCancelRefundInfoResponse>(cancelRefundInfoResponse, request.SessionId, new List<string> { request.SessionId, cancelRefundInfoResponse.ObjectName }, cancelRefundInfoResponse.ObjectName).ConfigureAwait(false);
            }

            if (cancelRefundInfoResponse != null && cancelRefundInfoResponse.IsMultipleRefundFOP)
            {
                cancelRefundInfoResponse.Pricing = null;
            }

            return cancelRefundInfoResponse;
        }

        public async Task<MOBCancelRefundInfoResponse> CancelRefundInfo(MOBCancelRefundInfoRequest request)
        {
            MOBCancelRefundInfoResponse cancelRefundInfoResponse = new MOBCancelRefundInfoResponse();
            string logAction = string.Empty;
            var session = new Session();
            var persisteligibility = new EligibilityResponse();

            if (!string.IsNullOrEmpty(request.SessionId))
            {
                session = await _shoppingSessionHelper.GetBookingFlowSession(request.SessionId, false);
                request.IsAward = session.IsAward;
                logAction = request.IsAward ? "Award" : "Revenue";

                persisteligibility
                    = await _sessionHelperService.GetSession<EligibilityResponse>(request.SessionId, persisteligibility.ObjectName, new List<string> { request.SessionId, persisteligibility.ObjectName }).ConfigureAwait(false);
                request.isPNRELF = (persisteligibility == null) ? false : persisteligibility.IsElf;
            }

            //Old Client (3.0.0.32 >=) and Not Award will always redirect to .COM
            if (!EnableCancelRefundOptions
               (request.Application.Id, request.Application.Version.Major) && !request.IsAward)
            {
                if (_configuration.GetValue<bool>("EnableByPassEligibilityAlwaysRedirect"))
                {
                    cancelRefundInfoResponse.Token = request.Token;
                    cancelRefundInfoResponse.SessionId = request.SessionId;
                    cancelRefundInfoResponse.RedirectURL
                        = (_configuration.GetValue<bool>("EnableTripDetailCancelRedirect3dot0Url"))
                        ? GetTripDetailRedirect3dot0Url(request.PNR, request.LastName, ac: "CA", channel: "mobile", languagecode: "en/US")
                        : GetPNRRedirectUrl(request.PNR, request.LastName, reqType: "CA");

                    cancelRefundInfoResponse.Exception = new MOBException("8999", _configuration.GetValue<string>("Cancel-IneligibleMessage"));
                    return cancelRefundInfoResponse;
                }
            }


            if (persisteligibility != null && persisteligibility.IsAgencyBooking)
            {
                if (!AllowAgencyToChangeCancel(persisteligibility.AgencyName, requesttype: "CANCEL"))
                {
                    if (_configuration.GetValue<bool>("EnableAgencyCancelMessage"))
                    {
                        var agencymsginfo = await GetDBDisplayContent("Reshop_ChangeCancelMsg_Content");
                        var msg = (agencymsginfo != null) ? agencymsginfo.FirstOrDefault
                            (x => string.Equals(x.Id, "cancelAgency", StringComparison.OrdinalIgnoreCase)) : null;
                        if (msg != null)
                        {
                            cancelRefundInfoResponse.Exception = SetCustomMobExceptionMessage(msg.CurrentValue, string.Empty);
                            return cancelRefundInfoResponse;
                        }
                    }
                }
            }//End of IsAgencyBooking


            #region common logging code

            _logger.LogInformation("CancelRefundInfo - ClientRequest {@ClientRequest} , Log Action {@LogAction} and SessionId {@SessionId}", JsonConvert.SerializeObject(request), logAction, request.SessionId);

            #endregion
            await ValidateRequest(request);
            request.IsVersionAllowAwardCancel = IsVersionAllowAward(request.Application.Id, request.Application.Version,
                                                                            _configuration.GetValue<string>("AndroidCanceldAwardVersion"),
                                                                            _configuration.GetValue<string>("iPhoneCancelAwardVersion"));

            cancelRefundInfoResponse = await CancelRefundInfo(request, persisteligibility);

            if (_configuration.GetValue<bool>("EnableEncryptedRedirectUrl"))
            {
                if (!string.IsNullOrEmpty(cancelRefundInfoResponse.RedirectURL))
                {
                    if (request.IsAward)
                    {
                        cancelRefundInfoResponse.RedirectURL
                            = (_configuration.GetValue<bool>("EnableTripDetailCancelRedirect3dot0Url"))
                            ? GetTripDetailRedirect3dot0Url(request.PNR, request.LastName, ac: "CA", channel: "mobile", languagecode: "en/US")
                            : GetPNRRedirectUrl(request.PNR, request.LastName, reqType: "AWARD_CA");
                    }
                    else
                    {
                        cancelRefundInfoResponse.RedirectURL
                            = (_configuration.GetValue<bool>("EnableTripDetailCancelRedirect3dot0Url"))
                            ? GetTripDetailRedirect3dot0Url(request.PNR, request.LastName, ac: "CA", channel: "mobile", languagecode: "en/US")
                            : GetPNRRedirectUrl(request.PNR, request.LastName, reqType: "CA");
                    }
                }
            }

            if (request.IsAward)
            {
                cancelRefundInfoResponse.AwardTravel = request.IsAward;
                var cslReservation
                    = await _sessionHelperService.GetSession<Service.Presentation.ReservationResponseModel.ReservationDetail>(request.SessionId, new Service.Presentation.ReservationResponseModel.ReservationDetail().GetType().FullName,
                    new List<string> { request.SessionId, new Service.Presentation.ReservationResponseModel.ReservationDetail().GetType().FullName }).ConfigureAwait(false);

                if (cslReservation != null
                    && cslReservation.Detail != null
                    && cslReservation.Detail.Sponsor != null)
                {
                    cancelRefundInfoResponse.SponsorMileagePlus = cslReservation.Detail.Sponsor.LoyaltyProgramMemberID;

                    if (cancelRefundInfoResponse.CancelPathEligible)
                    {
                        if (cancelRefundInfoResponse.Pricing != null)
                        {
                            cancelRefundInfoResponse.Pricing.RefundMilesLabel
                                = string.Format("To MileagePlus account ****{0}", cancelRefundInfoResponse?.SponsorMileagePlus?
                                .Substring(cancelRefundInfoResponse.SponsorMileagePlus.Length - 3));
                        }
                    }

                    if (_configuration.GetValue<bool>("EnableByPassSponsorMPCheck") &&
                        !string.IsNullOrEmpty(request.MileagePlusNumber))
                    {
                        cancelRefundInfoResponse.SponsorMileagePlus
                            = string.IsNullOrEmpty(request.MileagePlusNumber) ? string.Empty : request.MileagePlusNumber;
                    }
                }
            }

            if (!cancelRefundInfoResponse.CancelPathEligible)
            {
                if (EnableCancelWebSSO(request.Application.Id, request.Application.Version.Major))
                {
                    if (!string.IsNullOrEmpty(request.MileagePlusNumber) && !string.IsNullOrEmpty(request.HashPinCode))
                    {
                        bool validWalletRequest = false;
                        string authToken = string.Empty;

                        HashPin hashPin = new HashPin(_logger, _configuration, _validateHashPinService, _dynamoDBService, _mPSignInCommonService, _headers, _httpContextAccessor, _featureSettings);

                        var tupleRes = await hashPin.ValidateHashPinAndGetAuthToken
                            (request.MileagePlusNumber, request.HashPinCode, request.Application.Id, request.DeviceId, request.Application.Version.Major, authToken, session.SessionId);

                        validWalletRequest = tupleRes.returnValue;
                        authToken = tupleRes.validAuthToken;

                        if (!validWalletRequest)
                            throw new MOBUnitedException(_configuration.GetValue<string>("bugBountySessionExpiredMsg"));

                        if (validWalletRequest)
                        {
                            cancelRefundInfoResponse.TransactionId = request.TransactionId;
                            cancelRefundInfoResponse.LanguageCode = request.LanguageCode;

                            cancelRefundInfoResponse.WebShareToken = _dPService.GetSSOTokenString
                                (request.Application.Id, request.MileagePlusNumber, _configuration);
                            if (!String.IsNullOrEmpty(cancelRefundInfoResponse.WebShareToken))
                            {
                                cancelRefundInfoResponse.WebSessionShareUrl = _configuration.GetValue<string>("DotcomSSOUrl");

                                if (await _featureSettings.GetFeatureSettingValue("EnableRedirectURLUpdate").ConfigureAwait(false))
                                {
                                    cancelRefundInfoResponse.RedirectURL = $"{_configuration.GetValue<string>("NewDotcomSSOUrl")}?type=sso&token={cancelRefundInfoResponse.WebShareToken}&landingUrl={cancelRefundInfoResponse.RedirectURL}";
                                    cancelRefundInfoResponse.WebSessionShareUrl = cancelRefundInfoResponse.WebShareToken = string.Empty;
                                }
                            }

                        }
                    }
                }
            }
            // 676774 Presisting the quote refund object to recall Refund Miles in CancelAndRefund call
            if (request.SessionId != null)
            {
                await _sessionHelperService.SaveSession(cancelRefundInfoResponse, request.SessionId, new List<string>
                { request.SessionId, cancelRefundInfoResponse.ObjectName }, cancelRefundInfoResponse.ObjectName).ConfigureAwait(false);
            }

            if (string.Equals(cancelRefundInfoResponse?.Pricing?.QuoteType,
                     "NonRefundable", StringComparison.OrdinalIgnoreCase) && cancelRefundInfoResponse.IsJapanStandardEconomy)
            {
                cancelRefundInfoResponse.Pricing = null;
            }

            if (cancelRefundInfoResponse != null && cancelRefundInfoResponse.IsMultipleRefundFOP)
            {
                cancelRefundInfoResponse.Pricing = null;
            }
            return cancelRefundInfoResponse;
        }

        public async Task<MOBCancelAndRefundReservationResponse> CancelAndRefund(MOBCancelAndRefundReservationRequest request)
        {
            string logAction = request.IsAward ? "Award" : "Revenue";


            decimal result;
            decimal.TryParse(request.RefundAmount, System.Globalization.NumberStyles.Currency, System.Globalization.NumberFormatInfo.CurrentInfo, out result);
            var parsedRefundAmount = Convert.ToString(result);
            string cslErrorCode = string.Empty;
            bool isETCAndEmailNotAvailable = false;
            request.RefundAmount = parsedRefundAmount;

            var response = new MOBCancelAndRefundReservationResponse
            {
                TransactionId = request.TransactionId
            };

            MOBCancelAndRefund cancelAndRefund = null;
            try
            {
                Session session = new Session();

                MOBCancelRefundInfoResponse persistQuote = null;
                MOBQuoteRefundResponse persistquoterefund = null;

                // 676774 Loading RefundMiles
                if (request.SessionId != null)
                {
                    response.SessionId = request.SessionId;
                    session = await _sessionHelperService.GetSession<Session>(request.SessionId, session.ObjectName, new List<string> { request.SessionId, session.ObjectName }).ConfigureAwait(false);
                    persistQuote = await _sessionHelperService.GetSession<MOBCancelRefundInfoResponse>(request.SessionId, new MOBCancelRefundInfoResponse().ObjectName, new List<string> { request.SessionId, new MOBCancelRefundInfoResponse().ObjectName }).ConfigureAwait(false);
                    persistquoterefund = await _sessionHelperService.GetSession<MOBQuoteRefundResponse>(request.SessionId, new MOBQuoteRefundResponse().ObjectName, new List<string> { request.SessionId, new MOBQuoteRefundResponse().ObjectName }).ConfigureAwait(false);

                    if (persistQuote == null || persistquoterefund == null)
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("GenericExceptionMessage"));
                    }
                    if (session.IsAward)
                    {
                        var cslReservation
                            = await _sessionHelperService.GetSession<ReservationDetail>(session.SessionId, new ReservationDetail().GetType().FullName, new List<string> { session.SessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);

                        request.IsAward = session.IsAward;
                        request.RefundAmount = persistQuote.RefundAmount.Amount;
                        request.RefundMiles = persistQuote.Pricing.RefundMiles.Replace("miles", string.Empty);
                        request.RefundMiles = request.RefundMiles.Replace(",", string.Empty);
                        request.AwardRedepositFee = Convert.ToDecimal(persistQuote.Pricing.RedepositFee);
                        request.AwardRedepositFeeTotal = Convert.ToDecimal(persistQuote.Pricing.RedepositFee);
                        request.PointOfSale = persistquoterefund.PointOfSale;
                        if (cslReservation.Detail != null)
                            request.sponsor = cslReservation.Detail.Sponsor != null ? cslReservation.Detail.Sponsor : null;
                    }
                    else
                    {
                        request.FormOfPayment = null;
                        request.PointOfSale = persistquoterefund.PointOfSale;
                        if (persistQuote.IsManualRefund)
                        {
                            request.QuoteType = persistquoterefund.QuoteType;
                        }

                        if (persistQuote.IsJapanStandardEconomy)
                        {
                            request.QuoteType = persistquoterefund.QuoteType;
                        }
                    }

                    if (persistQuote.IsMultipleRefundFOP)
                    {
                        if (persistQuote?.Pricing != null)
                        {
                            request.QuoteType = persistQuote.Pricing.QuoteType;
                            request.RefundAmount = persistQuote.Pricing.TotalPaid;
                            request.CurrencyCode = persistQuote.Pricing.CurrencyCode;
                        }
                    }
                    else if (persistQuote.IsJapanStandardEconomy)
                    {
                        response.Pricing = null;

                        MOBPNRAdvisory jseCancelContent
                                       = PopulateConfigContent
                                       ("RESHOP_JSE_NONCHANGEABLE_CANCEL_OUTSIDE24HRS", "||");

                        if (jseCancelContent != null)
                        {
                            jseCancelContent.ContentType = ContentType.JSENONCONVERTEABLEPNR;
                            jseCancelContent.AdvisoryType = AdvisoryType.CAUTION;
                            response.AdvisoryInfo = (response.AdvisoryInfo == null)
                                ? new List<MOBPNRAdvisory>() : response.AdvisoryInfo;
                            response.AdvisoryInfo.Add(jseCancelContent);
                        }
                    }
                    else
                    {
                        response.Pricing = persistQuote.Pricing;
                    }

                }

                /// Code clean up  - Old approach of validating whether the session is active or not
                /// start here
                //if (_configuration.GetValue<bool>("EnableValidateRequestWithDeveiceIdAndRecordlocator"))
                //{
                //    await ValidateRequestWithDeveiceIdAndRecordlocator(request.DeviceId, request.RecordLocator);
                //}
                //else
                //{
                //    await ValidateRequestWithDeveiceIdAndRecordlocatorWithVersionCheck(request.Application.Id, request.Application.Version.Major, request.DeviceId, request.RecordLocator);
                //}
                /// End here
                
                if (_configuration.GetValue<bool>("EnableCancelMoreRefundOptions")
                    && request.SelectedRefundOptions != null
                    && request.SelectedRefundOptions.Any(x => x.Type == United.Mobile.Model.ManageRes.RefundType.ETC))
                {
                    isETCAndEmailNotAvailable = request.EmailAddress.IsNullOrEmpty();
                }

                if (!isETCAndEmailNotAvailable)
                {
                    cancelAndRefund = await CancelAndRefund(request, persistquoterefund);
                    if (response.Pricing != null)
                    {
                        if (persistquoterefund.IsRevenueRefundable
                            && persistquoterefund.IsRefundfeeAvailable)
                        {
                            response.Pricing.TotalPaid
                                = request.SelectedRefundOptions?.FirstOrDefault()?.RefundAmount;
                        }
                        response.Pricing.QuoteType = cancelAndRefund.QuoteType;

                        if (Enum.TryParse(cancelAndRefund.QuoteType, out RefundType refundtype))
                        {
                            response.Pricing.QuoteDisplayText = refundtype.GetDisplayName();

                            if (refundtype != RefundType.OFOP
                              && refundtype != RefundType.CNRM)
                            {
                                response.Pricing.RefundFOPLabel = string.Empty;
                            }
                        }
                    }

                    response.Pnr = cancelAndRefund.Pnr;
                    response.Email = cancelAndRefund.Email.ToLower();
                    response.SessionId = cancelAndRefund.SessionId;
                    response.Quotes = persistQuote.Quotes;
                }

                response.SelectedRefundOptions = request.SelectedRefundOptions;

                if (!request.IsAward && persistQuote.PartiallyFlown)
                {
                    response.Pricing.QuoteDisplayText = "Farelock";
                    response.Pricing.QuoteType = "Unticketed";
                }
            }
            catch (MOBUnitedException coex)
            {
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(coex);

                _logger.LogWarning("CancelAndRefund- UnitedException {exceptionstack} {@sessionId}", JsonConvert.SerializeObject(coex), request.SessionId);

                cslErrorCode = coex.Message;

                if (_configuration.GetValue<bool>("CancelRefund-EnableDateOfBirthValidation") &&
                    (cslErrorCode == "637" || cslErrorCode == "638")) // DateOfBirth validation error code from CSL CancelAndRefund Service
                {
                    response.Exception = CSLErrorMappingCancelRefund(cslErrorCode);
                    response.CustomerServicePhoneNumber = _configuration.GetValue<string>("Cancel-CustomerServicePhoneNumber");
                }

                else if (coex.InnerException != null && !string.IsNullOrEmpty(coex.InnerException.Message))
                {
                    response.Exception = CSLErrorMappingCancelRefund(coex.InnerException.Message);
                }
            }
            catch (Exception ex)
            {
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);

                _logger.LogError("CancelAndRefund- Exception {exceptionstack} {@sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);

                response.Exception = new MOBException
                {
                    Code = "9999",
                    Message = _configuration.GetValue<string>("GenericExceptionMessage")
                };
            }

            if (cancelAndRefund != null)
            {
                if (!cancelAndRefund.CancelSuccess)
                {
                    var cancelGenericExceptionMessageFormat = _configuration.GetValue<string>("Cancel-GenericExceptionMessage");
                    var cancelCustomerServicePhoneNumber = _configuration.GetValue<string>("Cancel-CustomerServicePhoneNumber");
                    response.Exception = new MOBException
                    {
                        Code = "7999",
                        Message = string.Format(cancelGenericExceptionMessageFormat, cancelCustomerServicePhoneNumber)
                    };
                    response.CustomerServicePhoneNumber = cancelCustomerServicePhoneNumber;
                }
                else if (cancelAndRefund.CancelSuccess && !cancelAndRefund.RefundSuccess)
                {
                    var refundGenericExceptionMessageFormat = _configuration.GetValue<string>("Refund-GenericExceptionMessage");
                    var refundCustomerServicePhoneNumber = _configuration.GetValue<string>("Refund-CustomerServicePhoneNumber");
                    response.Exception = new MOBException
                    {
                        Code = "8999",
                        Message = string.Format(refundGenericExceptionMessageFormat, refundCustomerServicePhoneNumber)
                    };
                    response.CustomerServicePhoneNumber = refundCustomerServicePhoneNumber;
                }
            }
            else
            {
                if (isETCAndEmailNotAvailable)
                {
                    response.Exception = new MOBException
                    {
                        Code = "6999",
                        Message = _configuration.GetValue<string>("Refund-ETCEmailValidationMessage")
                    };
                }
                else
                {
                    response.Exception = CSLErrorMappingCancelRefund(cslErrorCode);
                }
            }
            return response;
        }
        private bool EnableCancelWebSSO(int appId, string appVersion)
        {
            return GeneralHelper.isApplicationVersionGreater(appId, appVersion, "AndroidVersionCancelWebSSO", "iPhoneVersionCancelWebSSO", "", "", true, _configuration);
        }
        private bool EnableCancelRefundOptions(int appId, string appVersion)
        {
            return GeneralHelper.isApplicationVersionGreater(appId, appVersion, "iPhoneCancelMoreRefundOptionsVersion", "AndroidCancelMoreRefundOptionsVersion", "", "", true, _configuration);
        }
        private string GetTripDetailRedirect3dot0Url
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
            return ECommerce.Framework.Utilities.SecureData.EncryptString(data);
        }
        private string GetPNRRedirectUrl(string recordLocator, string lastlName, string reqType)
        {
            string retUrl = string.Empty;

            if (string.Equals(reqType, "EX", StringComparison.OrdinalIgnoreCase))
            {
                retUrl = string.Format("https://{0}/ual/en/US/flight-search/change-a-flight/changeflight/changeflight/rev?PNR={1}&RiskFreePolicy=&TYPE=rev&source=MOBILE",
                 _configuration.GetValue<string>("DotComChangeResBaseUrl"), HttpUtility.UrlEncode(EncryptString(recordLocator)));
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
        private bool IsVersionAllowAward(int applicationId, MOBVersion appVersion, string androidVersion, string iOSVersion)
        {
            bool award = false;
            if (applicationId == 1)
            {
                appVersion.Major = appVersion.Major.Replace("I", string.Empty);
                award = GeneralHelper.SeperatedVersionCompareCommonCode(appVersion.Major, iOSVersion);
            }
            else if (applicationId == 2)
            {
                appVersion.Major = appVersion.Major.Replace("A", string.Empty);
                award = GeneralHelper.SeperatedVersionCompareCommonCode(appVersion.Major, androidVersion);
            }

            return award;
        }
        private async Task ValidateRequest(MOBCancelRefundInfoRequest request)
        {
            if (string.IsNullOrEmpty(request.PNR))
            {
                if (!Convert.ToBoolean(_configuration.GetValue<string>("SurfaceErrorToClient")))
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("InvalidPNRLastName-ExceptionMessage").ToString());
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("PNRNotFound-ExceptionMessage").ToString());
                }
            }

            if (string.IsNullOrEmpty(request.LastName))
            {
                if (!Convert.ToBoolean(_configuration.GetValue<string>("SurfaceErrorToClient")))
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("InvalidPNRLastName-ExceptionMessage").ToString());
                }
                else
                {
                    throw new MOBUnitedException(_configuration.GetValue<string>("LastNameNotFound-ExceptionMessage").ToString());
                }
            }
            if (await _featureSettings.GetFeatureSettingValue("EnableCancelRefundInfoVDP215Fix").ConfigureAwait(false) && !string.IsNullOrEmpty(request.PNR))
            {
                var cslReservationDetail = await _sessionHelperService.GetSession<ReservationDetail>(request.SessionId, new ReservationDetail().GetType().FullName, new List<string> { request.SessionId, new ReservationDetail().GetType().FullName }).ConfigureAwait(false);
                if (cslReservationDetail == null)
                {
                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
                if (!string.Equals(request.PNR, cslReservationDetail?.Detail?.ConfirmationID, StringComparison.OrdinalIgnoreCase))
                {
                    request.PNR = cslReservationDetail?.Detail?.ConfirmationID;
                }
            }
            /// Code clean up  - Old approach of validating whether the session is active or not
            /// start here
            //if (_configuration.GetValue<bool>("EnableValidateRequestWithDeveiceIdAndRecordlocator"))
            //{
            //    await ValidateRequestWithDeveiceIdAndRecordlocator(request.DeviceId, request.PNR);
            //}
            //else
            //{
            //    await ValidateRequestWithDeveiceIdAndRecordlocatorWithVersionCheck(request.Application.Id, request.Application.Version.Major, request.DeviceId, request.PNR);
            //}
            /// End here
        }
        private async Task ValidateRequestWithDeveiceIdAndRecordlocator(string deviceId, string recordLocator)
        {
            CommonDef commonDef = new CommonDef();
            CommonDef presistedCommonDef = null;
            if (_configuration.GetValue<bool>("DeviceIDPNRSessionGUIDCaseSensitiveFix"))
            {
                //presistedCommonDef = United.Persist.FilePersist.Load<United.Persist.Definition.Common.CommonDef>((deviceId + recordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName);
                presistedCommonDef = await _sessionHelperService.GetSession<CommonDef>((deviceId + recordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName, new List<string> { (deviceId + recordLocator).Replace("|", "").Replace("-", "").ToUpper().Trim(), commonDef.ObjectName }).ConfigureAwait(false);
            }
            else
            {
                //presistedCommonDef = United.Persist.FilePersist.Load<United.Persist.Definition.Common.CommonDef>((deviceId + recordLocator).Replace("|", "").Replace("-", ""), commonDef.ObjectName);
                presistedCommonDef = await _sessionHelperService.GetSession<CommonDef>((deviceId + recordLocator).Replace("|", "").Replace("-", ""), commonDef.ObjectName, new List<string> { (deviceId + recordLocator).Replace("|", "").Replace("-", ""), commonDef.ObjectName }).ConfigureAwait(false);
            }
            if (presistedCommonDef != null)
            {
                MOBPNRByRecordLocatorResponse mobpnrbyrecordlocatorresponse = JsonConvert.DeserializeObject<MOBPNRByRecordLocatorResponse>(presistedCommonDef.SampleJsonResponse);
                if (mobpnrbyrecordlocatorresponse == null)
                {
                    throw new MOBUnitedException(_configuration.GetValue<bool>("InvalidPNRLastName-ExceptionMessage").ToString());
                }
            }
            else
            {
                throw new MOBUnitedException(_configuration.GetValue<bool>("InvalidPNRLastName-ExceptionMessage").ToString());
            }
        }

        private async Task ValidateRequestWithDeveiceIdAndRecordlocatorWithVersionCheck(int appId, string appVersion, string deviceId, string recordLocator)
        {
            if (GeneralHelper.isApplicationVersionGreater(appId, appVersion, "AndroidEnableValidateRequestWithDeveiceIdAndRecordlocatorVersion", "iPhoneEnableValidateRequestWithDeveiceIdAndRecordlocatorVersion", "", "", true, _configuration))
            {
                await ValidateRequestWithDeveiceIdAndRecordlocator(deviceId, recordLocator);
            }
        }
        private bool AllowAgencyToChangeCancel(string agencyname, string requesttype)
        {
            if (!_configuration.GetValue<bool>("AllowSelectedAgencyChangeCancelPath")) return false;
            var agencynamearray
                = ConfigUtility.GetListFrmPipelineSeptdConfigString("ReshopChangeCancelEligibleAgencyNames");
            return (agencynamearray != null && agencynamearray.Any() && agencynamearray.Contains(agencyname)) ? true : false;
        }
        private MOBException SetCustomMobExceptionMessage(string message, string code)
        {
            var customexceptionmsg = new MOBException();
            customexceptionmsg.Code = string.IsNullOrEmpty(code) ? "9999" : code;
            customexceptionmsg.Message = message;
            return customexceptionmsg;
        }

        public async Task<MOBCancelRefundInfoResponse> CancelRefundInfo(MOBCancelRefundInfoRequest request, EligibilityResponse persisteligibility)
        {
            string validToken = await _shoppingSessionHelper.GetSessionWithValidToken(request);
            return await CancelRefundInfo(request, persisteligibility, validToken);
        }

        private async Task<MOBCancelRefundInfoResponse> CancelRefundInfo
           (MOBCancelRefundInfoRequest request, EligibilityResponse persisteligibility, string cslToken)
        {
            string logAction = request.IsAward ? "Award" : "Revenue";

            MOBCancelRefundInfoResponse refundReservationResponse = new MOBCancelRefundInfoResponse();

            if (_configuration.GetValue<bool>("CancelRefund_ValidateCorporateTravelIneligibility"))
            {

                if (await ValidateCorporateTravelCancelEligiblity(request, refundReservationResponse) == false)
                    return refundReservationResponse;
            }
            var path = string.Format("/Eligible?recordLocator={0}&channel={1}&AllowAward={2}", request.PNR, request.Application.Id, request.IsVersionAllowAwardCancel);

            request.Token = cslToken;

            // get eligibility response
            string cslResponse = await _cancelRefundService.GetRefund(request.Token, request.SessionId, path).ConfigureAwait(false);

            if (cslResponse != null)
            {
                bool IsEnableChangeEligibleResponseDeserialize = await _featureSettings.GetFeatureSettingValue("EnableChangeEligibleResponseDeserialize").ConfigureAwait(false);

                if (IsEnableChangeEligibleResponseDeserialize)
                {
                    byte[] jsonBytes = Encoding.UTF8.GetBytes(cslResponse);
                    using (var stream = new MemoryStream(jsonBytes))
                    {
                        refundReservationResponse = Deserialize<MOBCancelRefundInfoResponse>(stream);
                    }
                }
                else
                {
                    refundReservationResponse = JsonConvert.DeserializeObject<MOBCancelRefundInfoResponse>(cslResponse);
                }

                if (refundReservationResponse?.ReservationDetail?.Detail != null)
                {
                    refundReservationResponse.PnrInfo = new MOBPnrInfo();
                    if (await _featureSettings.GetFeatureSettingValue("RemoveRemarksVDPVulnerability").ConfigureAwait(false))
                    {
                        if (refundReservationResponse?.ReservationDetail?.Detail.Remarks != null)
                        {
                            refundReservationResponse.ReservationDetail.Detail.Remarks = null; //VDP-214
                        }
                    }
                    //request.IsAward = refundReservationResponse.PnrInfo.AwardTravel;

                    refundReservationResponse.PnrInfo.PnrTravelers = new List<MOBPNRPassenger>();
                    refundReservationResponse.PnrInfo.PnrTravelers
                        = GetAllPassangers(request, refundReservationResponse.ReservationDetail);

                    refundReservationResponse.PnrInfo.PnrTrips = new List<MOBTrip>();
                    refundReservationResponse.PnrInfo.PnrTrips
                        = await GetAllTrips(refundReservationResponse.ReservationDetail, refundReservationResponse.PnrInfo.AwardTravel);

                    refundReservationResponse.PnrInfo.PnrSegments = new List<MOBPNRSegment>();
                    refundReservationResponse.PnrInfo.PnrSegments
                        = await GetAllSegments(request, refundReservationResponse.ReservationDetail);

                    if (refundReservationResponse.ReservationDetail.Detail.EmailAddress != null
                        && refundReservationResponse.ReservationDetail.Detail.EmailAddress.Any())
                        refundReservationResponse.PnrInfo.EmailAddress = refundReservationResponse.ReservationDetail.Detail.EmailAddress.FirstOrDefault().Address;

                    if (refundReservationResponse.PnrInfo.PnrSegments != null
                         && refundReservationResponse.PnrInfo.PnrSegments.Any())
                    {
                        persisteligibility.HasScheduleChange
                            = _manageResUtility.GetHasScheduledChanged(refundReservationResponse.PnrInfo.PnrSegments);
                    }

                    await _sessionHelperService.SaveSession(refundReservationResponse.ReservationDetail, request.SessionId, new List<string>
                { request.SessionId, new ReservationDetail().GetType().FullName }, new ReservationDetail().GetType().FullName).ConfigureAwait(false);
                }

                var partiallyflown
                    = UtilityHelper.GetBooleanFromCharacteristics(refundReservationResponse.Characteristics, "IsPartiallyUsedTicket");
                refundReservationResponse.PartiallyFlown = (partiallyflown.HasValue) ? partiallyflown.Value : false;

                var manualRefund
                    = UtilityHelper.GetBooleanFromCharacteristics(refundReservationResponse.Characteristics, "IsManualRefund");
                refundReservationResponse.IsManualRefund = (manualRefund.HasValue) ? manualRefund.Value : false;

                var executiveBulkTicket
                    = UtilityHelper.GetBooleanFromCharacteristics(refundReservationResponse.Characteristics, "IsExecutiveBulkTicket");
                refundReservationResponse.IsExecutiveBulkTicket = (executiveBulkTicket.HasValue) ? executiveBulkTicket.Value : false;

                var isAgencyBooking
                    = UtilityHelper.GetBooleanFromCharacteristics(refundReservationResponse.Characteristics, "IsAgencyBooking");
                refundReservationResponse.IsAgencyBooking = (isAgencyBooking.HasValue) ? isAgencyBooking.Value : false;

                if (persisteligibility != null && persisteligibility.HasScheduleChange)
                {
                    refundReservationResponse.CancelPathEligible = false;
                    refundReservationResponse.FailedRule = "HasScheduleChange";
                }
                else if (_configuration.GetValue<bool>("EnableCancelRefundFareruleExcludeList") &&
                    refundReservationResponse.FailedRule.IsNullOrEmpty() == false)
                {
                    string fareruleExcludeList = _configuration.GetValue<string>("CancelRefundFareruleExcludeList");
                    if (!fareruleExcludeList.IsNullOrEmpty())
                    {
                        string[] arrayfareruleExcludeList = fareruleExcludeList.Split('|');
                        if (arrayfareruleExcludeList != null && arrayfareruleExcludeList.Any
                                (x => refundReservationResponse.FailedRule.IndexOf(x, StringComparison.OrdinalIgnoreCase) > -1))
                        {
                            refundReservationResponse.Exception = new MOBException
                            {
                                Code = "9999",
                                Message = _configuration.GetValue<string>("ChangeEligiblieBulkAlertMessage")
                            };
                            return refundReservationResponse;
                        }
                    }
                }
                else if (_configuration.GetValue<bool>("CancelEligibilityByPassFlag"))
                {
                    refundReservationResponse.CancelPathEligible = true;
                }

                if (refundReservationResponse.CancelPathEligible)
                {
                    // Calculate amount to refund
                    string channel = (request.Application.Id == 1) ? "iOS" : "Android";
                    bool? manualRefundFromQuoteService = false;
                    MOBQuoteRefundResponse quoteRefundResponse = new MOBQuoteRefundResponse();

                    (quoteRefundResponse, manualRefundFromQuoteService) = await QuoteRefund(request, cslToken, manualRefundFromQuoteService, channel);

                    if ((quoteRefundResponse.Exception != null) && (!quoteRefundResponse.Exception.IsNullOrEmpty()))
                    {
                        if (quoteRefundResponse.Exception.Code.Equals("501"))
                        {
                            var cancelGenericExceptionMessageFormat = _configuration.GetValue<string>("Cancel-GenericExceptionMessage");
                            var cancelCustomerServicePhoneNumber = _configuration.GetValue<string>("Cancel-CustomerServicePhoneNumber");

                            refundReservationResponse.Exception = new MOBException
                            {
                                Code = "501",
                                Message = string.Format(cancelGenericExceptionMessageFormat, cancelCustomerServicePhoneNumber)
                            };

                            refundReservationResponse.CustomerServicePhoneNumber = cancelCustomerServicePhoneNumber;
                        }
                        else
                        {
                            refundReservationResponse.Exception = quoteRefundResponse.Exception;
                        }
                    }
                    else
                    {
                        quoteRefundResponse.IsAwardTravel = request.IsAward;
                        // redirect to 1.0 web if we have a refund fee
                        if (!EnableCancelRefundOptions
                            (request.Application.Id, request.Application.Version.Major))
                        {
                            if (quoteRefundResponse.RefundFee != null &&
                            decimal.Parse(quoteRefundResponse.RefundFee.Amount) > 0)
                            {
                                SetupRedirectURI(request, refundReservationResponse);
                                return refundReservationResponse;
                            }
                        }

                        if (_configuration.GetValue<bool>("AwardCancelCloseInFeeException"))
                        {
                            if (quoteRefundResponse.AwardRedepositFee == null && quoteRefundResponse.PriceBreakDown[0].Fees != null)
                            {
                                if (quoteRefundResponse.PriceBreakDown[0].Fees.Exists(p => p.Description == "RBF"))
                                {
                                    SetupRedirectURI(request, refundReservationResponse);
                                    return refundReservationResponse;
                                }
                            }
                        }

                        if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund") &&
                            request.IsAward &&
                            quoteRefundResponse.AwardRedepositFeeTotal != null &&
                            Convert.ToDouble(quoteRefundResponse.AwardRedepositFeeTotal.Amount) > 0 &&
                            quoteRefundResponse.AwardRedepositFeeTotal.CurrencyCode != "USD")
                        {
                            SetupRedirectURI(request, refundReservationResponse);
                            refundReservationResponse.FailedRule = string.Format("{0}-{1}", "IsNonUSDAwd", quoteRefundResponse.AwardRedepositFeeTotal.CurrencyCode);
                            refundReservationResponse.Token = Convert.ToString(cslToken);
                            return refundReservationResponse;
                        }


                        refundReservationResponse.Pricing
                            = new RefundPricingInfoMapper(quoteRefundResponse, _configuration, request.LanguageCode).Map();

                        //FFC
                        if (RefundQuoteFOPEligibility(refundReservationResponse, quoteRefundResponse))
                        {
                            var multipleRefundFOP = CreateMultipleRefund(request, refundReservationResponse, quoteRefundResponse, "en-US", request.CatalogValues);

                            if (multipleRefundFOP != null && multipleRefundFOP.Any())
                            {
                                if (quoteRefundResponse.IsMilesMoneyRefundFOP)
                                {
                                    multipleRefundFOP.SortDescending
                                        (x => string.Equals(x.CurrencyCode, "USD", StringComparison.OrdinalIgnoreCase));
                                }

                                refundReservationResponse.IsMultipleRefundFOP = true;

                                refundReservationResponse.Quotes
                                    = (refundReservationResponse.Quotes == null) ? new List<MOBModifyFlowPricingInfo>() : refundReservationResponse.Quotes;

                                refundReservationResponse.Quotes.AddRange(multipleRefundFOP);
                            }
                        }

                        //Setting Policy Message
                        if (!refundReservationResponse.IsCancellationFee)
                        {
                            refundReservationResponse.PolicyMessage
                                = Construct(quoteRefundResponse.Policy, quoteRefundResponse);
                        }

                        refundReservationResponse.SessionId = request.SessionId;
                        refundReservationResponse.FormattedQuoteType = FormattedQuoteType(refundReservationResponse, request.isPNRELF);
                        refundReservationResponse.Pricing.QuoteDisplayText = FormattedQuoteDisplayText(quoteRefundResponse);
                        refundReservationResponse.Payment = quoteRefundResponse.FopDetails;
                        refundReservationResponse.RefundAmount = quoteRefundResponse.RefundAmount;
                        refundReservationResponse.IsManualRefund = (manualRefundFromQuoteService.HasValue) ? manualRefundFromQuoteService.Value : false;

                        if (quoteRefundResponse.Policy.Name == RefundPolicy.NonRefundableBasicEconomy.ToString()
                                   && string.Equals(quoteRefundResponse.QuoteType, "NonRefundable", StringComparison.OrdinalIgnoreCase))
                        {
                            refundReservationResponse.IsBasicEconomyNonRefundable = true;
                        }
                        string refundMilesLabel = string.Empty;

                        if (!string.IsNullOrEmpty(request.SessionId))
                        {
                            if (!quoteRefundResponse.QuoteType.IsNullOrEmpty())
                            {
                                if (quoteRefundResponse.IsFarelock)
                                {
                                    refundReservationResponse.Pricing.RefundFOPLabel = "Farelock fees are non-refundable.";
                                    refundReservationResponse.Pricing.QuoteDisplayText = "Farelock";
                                    refundReservationResponse.Pricing.ShowMessageOnly = true;

                                }
                                else if (refundReservationResponse.PartiallyFlown)
                                {
                                    //TODO : Temp solution - Need to Remove - QuoteDisplayText & QuoteType
                                    if (!request.IsAward)
                                    {
                                        refundReservationResponse.PolicyMessage = _configuration.GetValue<string>("PartiallyFlownPolicyMessage");
                                        refundReservationResponse.Pricing.RefundFOPLabel = _configuration.GetValue<string>("PartiallyFlownPolicyHeaderMessage");
                                        refundReservationResponse.Pricing.ShowMessageOnly = true;

                                        if (GeneralHelper.IsApplicationVersionGreaterorEqual(
                                                 request.Application.Id, request.Application.Version.Major,
                                                 "CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_Android",
                                                 "CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_iOS") == false
                                                 )
                                        {
                                            refundReservationResponse.Pricing.QuoteDisplayText = "Farelock";
                                            refundReservationResponse.Pricing.QuoteType = "Unticketed";
                                        }
                                    }
                                    else
                                    {
                                        var awardquote = new MOBModifyFlowPricingInfo
                                        {
                                            QuoteDisplayText = "Total refund",
                                            QuoteType = "Refundable",
                                            FormattedPricingDetail = "No Value",
                                            TotalPaid = "No Value",

                                        };
                                        refundReservationResponse.Quotes
                                            = (refundReservationResponse.Quotes == null) ? new List<MOBModifyFlowPricingInfo>() : refundReservationResponse.Quotes;
                                        refundReservationResponse.Quotes.Add(awardquote);
                                    }
                                }
                                else if (refundReservationResponse.IsManualRefund &&
                                    (quoteRefundResponse.IsRefundableTicket || refundReservationResponse.FormattedQuoteType == "24-hour policy"))
                                {
                                    refundReservationResponse.PolicyMessage = _configuration.GetValue<string>("ManualRefundPolicyMessage");
                                    refundReservationResponse.Pricing.RefundFOPLabel = _configuration.GetValue<string>("ManualRefundPolicyHeaderMessage");
                                    refundReservationResponse.Pricing.ShowMessageOnly = true;

                                    if (GeneralHelper.IsApplicationVersionGreaterorEqual(
                                        request.Application.Id, request.Application.Version.Major,
                                         _configuration.GetValue<string>("CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_Android"),
                                         _configuration.GetValue<string>("CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_iOS")) == false
                                        )
                                    {
                                        refundReservationResponse.Pricing.QuoteDisplayText = "Farelock";
                                        refundReservationResponse.Pricing.QuoteType = "Unticketed";
                                        refundReservationResponse.Payment.PaymentType = "Unknown";
                                    }


                                }
                                else if (!string.Equals(quoteRefundResponse.QuoteType, "FutureFlightCredit", StringComparison.OrdinalIgnoreCase)
                                    && !string.Equals(quoteRefundResponse.QuoteType, "NonRefundable", StringComparison.OrdinalIgnoreCase)
                                    && refundReservationResponse.IsExecutiveBulkTicket == false)
                                {
                                    refundReservationResponse.Pricing.RefundFOPLabel
                                        = ToOriginalFormOfPayment(quoteRefundResponse.FopDetails);
                                }
                                else if (refundReservationResponse.IsExecutiveBulkTicket)
                                {
                                    refundReservationResponse.Pricing.QuoteDisplayText = "Executive bulk";
                                    refundReservationResponse.Pricing.TotalPaid = string.Empty;
                                }
                                else if (refundReservationResponse.IsBasicEconomyNonRefundable)
                                {
                                    refundReservationResponse.PolicyMessage = _configuration.GetValue<string>("BasicEconomyNonRefundMessage");
                                    refundReservationResponse.Pricing.RefundFOPLabel = _configuration.GetValue<string>("BasicEconomyNonRefundHeadMessage");
                                    refundReservationResponse.Pricing.ShowMessageOnly = true;

                                    if (GeneralHelper.IsApplicationVersionGreaterorEqual(
                                      request.Application.Id, request.Application.Version.Major,
                                       _configuration.GetValue<string>("CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_Android"),
                                       _configuration.GetValue<string>("CancelRefundInfo_UnknownPaymentType_AppVersionsSupportedTill_iOS")) == false
                                      )
                                    {
                                        refundReservationResponse.Pricing.QuoteDisplayText = "Farelock";
                                        refundReservationResponse.Pricing.QuoteType = "Unticketed";
                                        refundReservationResponse.Payment.PaymentType = "Unknown";
                                    }
                                }
                            }

                            if (request.IsAward)
                            {
                                if (refundReservationResponse.Pricing.PricesPerTypes != null && refundReservationResponse.Pricing.PricesPerTypes.Any())
                                {
                                    refundReservationResponse.Pricing.PricesPerTypes[0].TotalBaseFare
                                        = string.Format("{0} {1}", FormatNumberPerLanguageCode(request.LanguageCode, quoteRefundResponse.RefundMiles.Amount.ToString()), "miles");
                                }
                                refundReservationResponse.AwardTravel = request.IsAward;
                                refundReservationResponse.Pricing.RefundMiles = string.Format("{0} {1}", FormatNumberPerLanguageCode(request.LanguageCode, quoteRefundResponse.RefundMiles.Amount.ToString()), "miles");

                                if (_configuration.GetValue<bool>("DisabledAwardRedepositPolicyMessage") && quoteRefundResponse.AwardRedepositFee != null)
                                {
                                    refundReservationResponse.Pricing.HasTotalDue = true;
                                    refundReservationResponse.Pricing.RedepositFee = Convert.ToDouble(quoteRefundResponse.AwardRedepositFeeTotal.Amount);

                                    var ci = TopHelper.GetCultureInfo("USD");
                                    refundReservationResponse.Pricing.TotalDue = TopHelper.FormatAmountForDisplay(quoteRefundResponse.AwardRedepositFeeTotal.Amount, ci, false);

                                    if (refundReservationResponse.Pricing.RedepositFee == 0)
                                    {
                                        refundReservationResponse.PolicyMessage = _configuration.GetValue<string>("CancelRedepositPolicyMessage");
                                    }
                                }
                            }
                            refundReservationResponse.Pricing.FormattedPricingDetail = GetFormattedQuoteAmount(request.IsAward, refundReservationResponse.Pricing);
                        }

                        //Refund Ancillary products
                        if (!refundReservationResponse.IsMultipleRefundFOP)
                        {
                            if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund"))
                            {
                                if (_configuration.GetValue<bool>("EnableETCCombinabilityCancelRefund"))
                                {
                                    GetAncillaryProductsFromPaymentMethods(quoteRefundResponse, refundReservationResponse, request.LanguageCode, request.CatalogValues);
                                }
                                else
                                {
                                    GetAncillaryProducts(quoteRefundResponse, refundReservationResponse, request.LanguageCode);

                                }
                            }
                            else
                            {
                                if (quoteRefundResponse.RefundAncillaryProducts.IsListNullOrEmpty() == false &&
                                    quoteRefundResponse.RefundAncillaryProducts.Any(x => x.PaymentMethod != null && x.PaymentMethod.PaymentIndex > 1))
                                {

                                    var groupbydisplayquotes
                                        = quoteRefundResponse.RefundAncillaryProducts.GroupBy
                                        (x => x.PaymentMethod?.PaymentIndex).Select(x => x.First());

                                    groupbydisplayquotes.ForEach(qte =>
                                    {
                                        if (qte.PaymentMethod?.PaymentIndex > 1)
                                        {
                                            var ancillaryquote = new MOBModifyFlowPricingInfo
                                            {
                                                TotalPaid = quoteRefundResponse.RefundAncillaryProducts.Where
                                                (x => x.PaymentMethod?.PaymentIndex == qte.PaymentMethod?.PaymentIndex)
                                                .Sum(x => x.Amount).ToString("c", GetCurrencyFormatProviderSymbolDecimals(CurrencyCode)),

                                                RefundFOPLabel = ToOriginalFormOfPaymentAncillary(qte?.PaymentMethod),

                                                QuoteDisplayText = "Total refund",
                                            };

                                            refundReservationResponse.Quotes
                                                = (refundReservationResponse.Quotes.IsListNullOrEmpty())
                                                ? new List<MOBModifyFlowPricingInfo>() : refundReservationResponse.Quotes;
                                            refundReservationResponse.Quotes.Add(ancillaryquote);
                                        }
                                    });


                                    if (refundReservationResponse.Quotes != null && refundReservationResponse.Quotes.Any())
                                    {
                                        decimal ancillaryProductsTotalPaid = 0;
                                        decimal totalPaidAmount = 0;

                                        if (quoteRefundResponse.RefundAmountTicket != null)
                                        {
                                            if (quoteRefundResponse.RefundAncillaryProducts.IsListNullOrEmpty() == false &&
                                        quoteRefundResponse.RefundAncillaryProducts.Any(x => x.PaymentMethod.PaymentIndex == 1))
                                            {
                                                ancillaryProductsTotalPaid = quoteRefundResponse.RefundAncillaryProducts.Where
                                                    (x => x.PaymentMethod?.PaymentIndex == 1)
                                                    .Sum(x => x.Amount);

                                            }

                                            totalPaidAmount = quoteRefundResponse.RefundAmountTicket.Amount + ancillaryProductsTotalPaid;
                                            //refundReservationResponse.Pricing.TotalPaid = totalPaidAmount.ToString("c", GetCurrencyFormatProviderSymbolDecimals(CurrencyCode));
                                            double.TryParse(totalPaidAmount.ToString(), out double amountInDouble);
                                            refundReservationResponse.Pricing.TotalPaid = ConfigUtility.GetCurrencyAmount(amountInDouble, quoteRefundResponse.RefundAmountTicket.CurrencyCode, 2, request.LanguageCode);
                                            refundReservationResponse.Pricing.FormattedPricingDetail = refundReservationResponse.Pricing.TotalPaid;
                                        }
                                    }
                                }
                            }
                        }


                        //TODO : check if any other takes priority.
                        if (string.Equals(refundReservationResponse?.Pricing?.QuoteType,
                            "NonRefundable", StringComparison.OrdinalIgnoreCase))
                        {
                            refundReservationResponse.Pricing.TotalPaid = "No value";

                            //Japan non-changeable pnr handling
                            if (!_configuration.GetValue<bool>("Disable_RESHOP_JSENONCHANGEABLEFARE"))
                            {
                                if (quoteRefundResponse.IsJapanStandardEconomy)
                                {
                                    refundReservationResponse.IsJapanStandardEconomy = quoteRefundResponse.IsJapanStandardEconomy;

                                    MOBPNRAdvisory jseCancelContent
                                        = PopulateConfigContent("RESHOP_JSE_NONCHANGEABLE_OUTSIDE24HRS", "||");

                                    if (jseCancelContent != null)
                                    {
                                        jseCancelContent.ContentType = ContentType.JSENONCONVERTEABLEPNR;
                                        jseCancelContent.AdvisoryType = AdvisoryType.CAUTION;

                                        refundReservationResponse.AdvisoryInfo = (refundReservationResponse.AdvisoryInfo == null)
                                            ? new List<MOBPNRAdvisory>() : refundReservationResponse.AdvisoryInfo;

                                        refundReservationResponse.AdvisoryInfo.Add(jseCancelContent);
                                    }
                                }
                            }
                        }

                        //Refund options
                        if (EnableCancelRefundOptions(request.Application.Id, request.Application.Version.Major)
                            && _configuration.GetValue<bool>("EnableCancelMoreRefundOptions")
                            && refundReservationResponse.IsManualRefund == false)
                        {
                            refundReservationResponse.RefundOptions
                                = await CreateCancelRefundUserContent(request.IsAward, quoteRefundResponse, refundReservationResponse);
                        }

                        if (_configuration.GetValue<bool>("ShowUpgradePNRsRedepositTable"))
                        {
                            refundReservationResponse.Quotes = GetMUAandPPRUpgradeDetails(quoteRefundResponse, refundReservationResponse.Quotes, request.LanguageCode);
                        }
                        if (_configuration.GetValue<bool>("EnableETCCombinabilityCancelRefund") && refundReservationResponse.IsManualRefund == false)
                            refundReservationResponse.ShowRefundEmail = IsShowRefundEmailFlag(refundReservationResponse.RefundOptions, quoteRefundResponse.PaymentMethods);

                        if (_configuration.GetValue<bool>("CancelRefund_AllowFullRefundForMeaningfulScheduleChange")
                            && refundReservationResponse.RefundOptions != null
                            && refundReservationResponse.RefundOptions.Where(x => x.Type == RefundType.ETC).Count() <= 0)
                        {
                            refundReservationResponse.ReservationValueHeader = string.Empty;
                        }

                        if (IsDateOfBirthValidationRequired(request.Application.Id, request.Application.Version.Major) &&
                                   string.IsNullOrEmpty(request.MileagePlusNumber) &&
                                   request.IsAward == false)
                        {
                            refundReservationResponse.RequireDateOfBirth = true;
                        }

                        await _sessionHelperService.SaveSession(quoteRefundResponse, request.SessionId, new List<string>
                { request.SessionId, quoteRefundResponse.ObjectName }, quoteRefundResponse.ObjectName).ConfigureAwait(false);
                    }
                }
                else
                {
                    SetupRedirectURI(request, refundReservationResponse);
                }

                refundReservationResponse.Token = Convert.ToString(cslToken);
                return refundReservationResponse;
            }

            _logger.LogError("CancelRefundPathEligibilityResponse: Error Calling Refund Service.");
            refundReservationResponse.Exception = new MOBException
            {
                Message = "Request failed to refund eligibility service."
            };
            refundReservationResponse.Token = Convert.ToString(cslToken);
            return refundReservationResponse;
        }
        public static T Deserialize<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }
            using var sr = new StreamReader(stream);
            using var jr = new JsonTextReader(sr);
            var ntSerializer = new Newtonsoft.Json.JsonSerializer();
            ntSerializer.Converters.Add(new TimeSpanConverter());
            var response = ntSerializer.Deserialize<T>(jr);
            return response;
        }
        private void SetupRedirectURI(MOBCancelRefundInfoRequest request, MOBCancelRefundInfoResponse refundReservationResponse)
        {
            refundReservationResponse.CancelPathEligible = false;
            refundReservationResponse.RefundPathEligible = false;

            refundReservationResponse.RedirectURL = "http://" + _configuration.GetValue<string>("DotComOneCancelURL") + "/web/en-US/apps/reservation/import.aspx?OP=1&CN=" +
                request.PNR +
                "&LN=" +
                request.LastName +
                "&T=F&MobileOff=1";
            refundReservationResponse.Exception = new MOBException("8999", _configuration.GetValue<string>("Cancel-IneligibleMessage"));
        }
        private async Task<bool> ValidateCorporateTravelCancelEligiblity(MOBCancelRefundInfoRequest request, MOBCancelRefundInfoResponse refundReservationResponse)
        {
            EligibilityResponse eligibility = new EligibilityResponse();

            eligibility = await _sessionHelperService.GetSession<EligibilityResponse>(request.SessionId, eligibility.ObjectName, new List<string> { request.SessionId, eligibility.ObjectName }).ConfigureAwait(false);

            if (eligibility != null && eligibility.IsCorporateBooking)
            {
                var vendorNameArray = _manageResUtility.GetListFrmPipelineSeptdConfigString("CancelRefund_IneligibleVendorNames");
                if (vendorNameArray != null && vendorNameArray.Any())
                {
                    if (vendorNameArray.Contains(eligibility.CorporateVendorName))
                    {
                        refundReservationResponse.CancelPathEligible = false;
                        refundReservationResponse.FailedRule = string.Format("CorpTravel-{0}", eligibility.CorporateVendorName);
                        SetupRedirectURI(request, refundReservationResponse);
                        return false;
                    }
                }
            }
            return true;
        }
        private async Task<(MOBQuoteRefundResponse response, bool? manualRefundFromQuoteService)> QuoteRefund(MOBCancelRefundInfoRequest request, string cslToken, bool? manualRefundFromQuoteService, string channel = "")
        {
            string logAction = request.IsAward ? "Award" : "Revenue";

            var cslRequest = new QuoteRefundRequest
            {
                RecordLocator = request.PNR,
            };


            string jsonResponse = string.Empty;

            //TODO DEFAULT
            if (_configuration.GetValue<bool>("EnableCancelMoreRefundOptions"))
            {

                var urlBase = string.Format("/Quote?recordLocator={0}&CHANNEL={1}", cslRequest.RecordLocator, channel);
                /********************/
                jsonResponse = await _cancelRefundService.GetQuoteRefund(request.Token, request.SessionId, urlBase).ConfigureAwait(false);
            }
            else
            {
                var urlBase = string.Format("/Quote?recordLocator={0}", cslRequest.RecordLocator);
                /********************/
                jsonResponse = await _cancelRefundService.GetQuoteRefund(request.Token, request.SessionId, urlBase).ConfigureAwait(false);
            }

            var cslResponse = JsonConvert.DeserializeObject<QuoteRefundResponse>(jsonResponse);
            manualRefundFromQuoteService = UtilityHelper.GetBooleanFromCharacteristics(cslResponse.Characteristics, "IsManualRefund");

            return ((HandleQuoteRefundResponse(cslResponse, request), manualRefundFromQuoteService));

        }

        private MOBQuoteRefundResponse HandleQuoteRefundResponse(QuoteRefundResponse response, MOBCancelRefundInfoRequest request)
        {
            var quoteRefund = new MOBQuoteRefundResponse
            {
                QuoteType = response.QuoteType,
                Policy = response.Policy,
                PriceBreakDown = response.PriceBreakDown,
                FopDetails = response.FopDetails,
                RefundAmount = response.RefundAmount,
                RefundFee = response.RefundFee,
                RefundMiles = response.RefundMiles,
                AwardRedepositFee = response.AwardRedepositFee,
                AwardRedepositFeeTotal = response.AwardRedepositFeeTotal,
                AncillaryProducts = response.AncillaryProducts,
                RefundAncillaryProducts = response.RefundAncillaryProducts,
                RefundAmountTicket = response.RefundAmountTicket,
                RefundUpgradeInstruments = response.RefundUpgradeInstruments,
                RefundUpgradeInstrumentsTotal = response.RefundUpgradeInstrumentsTotal,
                RefundUpgradePoints = response.RefundUpgradePoints,
                RefundUpgradePointsTotal = response.RefundUpgradePointsTotal,
                PointOfSale = response.PointOfSale,
                RefundAmountOtherCurrency = response.RefundAmountOtherCurrency,
                PaymentMethods = response.PaymentMethods

            };

            if (quoteRefund.Policy != null
                && Enum.TryParse(quoteRefund.Policy.Code, out RefundPolicy refundPolicy))
            {
                quoteRefund.IsRevenueRefundable
                    = (RefundPolicy.Refundable == refundPolicy || RefundPolicy.TwentyFourHour == refundPolicy);

                if (_configuration.GetValue<bool>("CancelRefund_AllowFullRefundForMeaningfulScheduleChange") &&
                   (RefundPolicy.ExpectionPolicy3 == refundPolicy || RefundPolicy.ScheduleChangeRefundPolicy == refundPolicy))
                {
                    quoteRefund.IsRevenueRefundable = true;
                }

                quoteRefund.IsRevenueNonRefundable = (RefundPolicy.FutureFlightCredit == refundPolicy);

                quoteRefund.IsFarelock = (RefundPolicy.FareLock == refundPolicy);

                quoteRefund.IsFutureflightCredit = (RefundPolicy.FutureFlightCredit == refundPolicy);
            }

            var isetceligible = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsETCRefundEligible");
            quoteRefund.IsETCEligible = (isetceligible.HasValue) ? isetceligible.Value : false;

            var iscancelonlyeligible = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsCancelOnlyEligible");
            quoteRefund.IsCancelOnlyEligible = (iscancelonlyeligible.HasValue) ? iscancelonlyeligible.Value : false;

            var isRefundableTicket = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsRefundableTicket");
            quoteRefund.IsRefundableTicket = (isRefundableTicket.HasValue) ? isRefundableTicket.Value : false;

            var isffcreligible = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsFFCRRefundEligible");
            quoteRefund.IsFFCREligible = (isffcreligible.HasValue) ? isffcreligible.Value : false;

            var isBECancelFeeEligible = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsBasicEconomyCancelFeeEligible");
            quoteRefund.IsBECancelFeeEligible = (isBECancelFeeEligible.HasValue) ? isBECancelFeeEligible.Value : false;

            var isJapanStandardEconomy = UtilityHelper.GetBooleanFromCharacteristics(response.Characteristics, "IsJapanStandardEconomy");
            quoteRefund.IsJapanStandardEconomy = (isJapanStandardEconomy.HasValue) ? isJapanStandardEconomy.Value : false;


            quoteRefund.IsMultipleRefundFOP = CheckMultipleRefundFOP(request, response, quoteRefund);

            quoteRefund.IsMilesMoneyPaidFOP = CheckMilesMoneyPaidFOP(request, response, quoteRefund);

            quoteRefund.IsMilesMoneyRefundFOP = CheckMilesMoneyRefundFOP(request, response, quoteRefund);

            quoteRefund.ShowETCConvertionInfo = ShowETCConvertionInfo(request, response, quoteRefund);


            quoteRefund.IsMilesMoneyFFCTRefundFOP = quoteRefund.IsMilesMoneyPaidFOP
               && string.Equals(response.QuoteType, "FutureFlightCredit", StringComparison.OrdinalIgnoreCase);

            if (_configuration.GetValue<bool>("staticCancelFee"))
            {
                quoteRefund.AwardRedepositFee = new MOBBasePrice() { Amount = "50", Code = "REF" };
                quoteRefund.AwardRedepositFeeTotal = new MOBBasePrice() { Amount = "50", Code = "REF" };
            }

            // As part of MRM - 1175 Error Handling: Cancel Service Fails to Return Quote : 
            // handle situations when the cancel service fails to return a quote response 
            if (_configuration.GetValue<bool>("CancelRefund-HandleQuoteServiceError"))
            {
                if (response.Error != null && response.Error.Count > 0)
                {
                    quoteRefund.Exception = new MOBException("501", response.Error[0].MajorDescription);
                }
                else if (_configuration.GetValue<bool>("IncludeQuotePriceBreakCheckWhenUnticketed") &&
                     (response.PriceBreakDown == null || response.PriceBreakDown.Count <= 0 ||
                     response.PriceBreakDown[0].BasePrice == null))
                {
                    quoteRefund.Exception = new MOBException("501", "Base price not found");
                }
            }
            else
            {
                if (response.Error != null && response.Error.Count > 0)
                {
                    var contains501 = response.Error.FirstOrDefault(element => element.MajorCode == "501");

                    if (contains501.IsNullOrEmpty())
                    {
                        throw new MOBUnitedException(_configuration.GetValue<string>("GenericExceptionMessage"));
                    }
                    else
                    {
                        quoteRefund.Exception = new MOBException(contains501.MajorCode, contains501.MajorDescription);
                    }
                }
            }

            return quoteRefund;
        }
        protected internal enum RefundPolicy
        {
            TwentyFourHour = 0001,
            Refundable = 0002,
            NonRefundable = 0003,
            FutureFlightCredit = 0004,
            NonRefundableBasicEconomy = 0005,
            ExpectionPolicy3 = 0006,
            UnTicketTTL = 0007,
            FareLock = 0008,
            TicketPending = 0009,
            ExecutiveBulk = 0010,
            Sev2WeatherPolicyRefundable = 0011,
            Sev2WeatherPolicy = 0012,
            Sev1WeatherPolicyRefundable = 0013,
            Sev1WeatherPolicy = 0014,
            ScheduleChangeRefundPolicy = 0015,
            IRROPS = 0016
        }
        private async Task<List<MOBItem>> GetDBDisplayContent(string contentname)
        {
            List<MOBItem> mobcontentitem;
            if (string.IsNullOrEmpty(contentname)) return null;
            try
            {
                var messageitems = await _legalDocumentsForTitlesService.GetNewLegalDocumentsForTitles(contentname, _headers.ContextValues.SessionId, true).ConfigureAwait(false);

                if (messageitems != null && messageitems.Any())
                {
                    mobcontentitem = new List<MOBItem>();
                    foreach (United.Definition.MOBLegalDocument doc in messageitems)
                    {
                        mobcontentitem.Add(new MOBItem() { Id = doc.Title, CurrentValue = doc.LegalDocument });
                    }
                    return mobcontentitem;
                }
            }
            catch (Exception ex) { return null; }
            return null;
        }
        private bool CheckMultipleRefundFOP(MOBCancelRefundInfoRequest request,
            QuoteRefundResponse cslQuoteResponse, MOBQuoteRefundResponse mobquoteResponse)
        {
            if (cslQuoteResponse?.PaymentMethods != null && cslQuoteResponse.PaymentMethods.Any())
            {
                return cslQuoteResponse.PaymentMethods.Where(x => x.IsTicketRefundFOP == true)?
                    .Select(x => x.PaymentType)?.Distinct()?.Count() > 1;
            }
            return false;
        }
        private bool CheckMilesMoneyPaidFOP(MOBCancelRefundInfoRequest request,
           QuoteRefundResponse cslQuoteResponse, MOBQuoteRefundResponse mobquoteResponse)
        {
            if (cslQuoteResponse?.PaymentMethods != null && cslQuoteResponse.PaymentMethods.Any())
            {
                return cslQuoteResponse.PaymentMethods.Where
                    (x => x.IsTicketPaidFOP == true && string.Equals(x.PaymentType, "MilesMoney", StringComparison.OrdinalIgnoreCase)).Any();
            }
            return false;
        }
        private bool CheckMilesMoneyRefundFOP(MOBCancelRefundInfoRequest request,
        QuoteRefundResponse cslQuoteResponse, MOBQuoteRefundResponse mobquoteResponse)
        {
            if (cslQuoteResponse?.PaymentMethods != null && cslQuoteResponse.PaymentMethods.Any())
            {
                return cslQuoteResponse.PaymentMethods.Where
                    (x => x.IsTicketRefundFOP == true && string.Equals(x.PaymentType, "MilesMoney", StringComparison.OrdinalIgnoreCase)).Any();
            }
            return false;
        }
        private List<MOBModifyFlowPricingInfo> CreateMultipleRefund(MOBCancelRefundInfoRequest refundInforequest,
            MOBCancelRefundInfoResponse refundReservationResponse, MOBQuoteRefundResponse quoteRefundResponse, string languageCode = "en-US", List<MOBItem> catalogValues = null)
        {
            var pricingitems = new List<MOBModifyFlowPricingInfo>();

            if (string.Equals(quoteRefundResponse.QuoteType, "nonrefundable", StringComparison.OrdinalIgnoreCase)) return null;

            if (quoteRefundResponse?.PaymentMethods == null || !quoteRefundResponse.PaymentMethods.Any()) return null;

            var refundFOPs = quoteRefundResponse.PaymentMethods.Where(x => x.IsTicketRefundFOP || x.IsAncillaryRefundFOP);
            bool isMFOPCatalogEnabled = ConfigUtility.IsMFOPCatalogEnabled(catalogValues);

            refundFOPs.ForEach(fop =>
            {
                if (fop != null)
                {
                    if (fop.RefundAmount != null || (isMFOPCatalogEnabled && fop.PaymentType == "Miles" && fop.RefundMiles != null))
                    {
                        var refundfop = new MOBModifyFlowPricingInfo { };
                        if (isMFOPCatalogEnabled && fop.PaymentType == "Miles")
                        {
                            refundfop.CurrencyCode = fop.RefundMiles.CurrencyCode;
                            
                            double.TryParse(Convert.ToString(fop.RefundMiles.Amount), out double fopamount);

                            refundfop.TotalPaid = refundfop.FormattedPricingDetail
                                = ConfigUtility.GetCurrencyAmount(fopamount, fop.RefundMiles.CurrencyCode, 2, languageCode);

                            refundfop.QuoteDisplayText = FormattedQuoteDisplayText(quoteRefundResponse, fop.RefundMiles.CurrencyCode);
                        }
                        else
                        {
                            refundfop.CurrencyCode = fop.RefundAmount.CurrencyCode;

                            double.TryParse(Convert.ToString(fop.RefundAmount.Amount), out double fopamount);

                            refundfop.TotalPaid = refundfop.FormattedPricingDetail
                                = ConfigUtility.GetCurrencyAmount(fopamount, fop.RefundAmount.CurrencyCode, 2, languageCode);

                            refundfop.QuoteDisplayText = FormattedQuoteDisplayText(quoteRefundResponse, fop.RefundAmount.CurrencyCode);
                        }

                        refundfop.QuoteType = fop.PaymentType;

                        refundfop.RefundFOPLabel = ToRefundFormOfPayment(fop, isMFOPCatalogEnabled);

                        refundfop.ConversionInfo = SetCancellationInfo(refundReservationResponse, quoteRefundResponse, fop, languageCode);

                        //Old Client Handling
                        if (refundReservationResponse.IsCancellationFee)
                        {
                            if (!GeneralHelper.IsApplicationVersionGreaterorEqual
                            (refundInforequest.Application.Id, refundInforequest.Application.Version.Major, _configuration.GetValue<string>("BECancellationFeeChangesAndroidversion"), _configuration.GetValue<string>("BECancellationFeeChangesiOSversion")))
                            {
                                refundfop.RefundFOPLabel = $"{refundfop.RefundFOPLabel} \n(Includes {refundfop.ConversionInfo.CancellationFees})";
                            }
                        }

                        pricingitems.Add(refundfop);
                    }
                }
            });
            return pricingitems;
        }
        private MOBConversionPricingInfo SetCancellationInfo(MOBCancelRefundInfoResponse refundReservationResponse,
           MOBQuoteRefundResponse quoteRefundResponse, Payment fop, string languageCode)
        {

            try
            {
                if (refundReservationResponse.IsCancellationFee
                || (quoteRefundResponse.ShowETCConvertionInfo && string.Equals(fop.PaymentType, "CERTIFICATE", StringComparison.OrdinalIgnoreCase)))
                {
                    Double.TryParse(Convert.ToString(fop.RefundAmountTicket?.Amount), out double refundAmount);

                    if (refundAmount > 0)
                    {
                        var pricebreakdown = quoteRefundResponse.PriceBreakDown?.FirstOrDefault();
                        if (pricebreakdown != null)
                        {
                            var numberoftravelers = pricebreakdown.TravelerCount;
                            string refundAmountTxt = ConfigUtility.GetCurrencyAmount(refundAmount, fop.RefundAmountTicket?.CurrencyCode, 2, languageCode);

                            Double.TryParse(Convert.ToString(pricebreakdown.RefundBasePrice?.Amount), out double baseFare);

                            baseFare = baseFare * numberoftravelers;

                            string baseFareTxt
                                = ConfigUtility.GetCurrencyAmount(baseFare, pricebreakdown.RefundBasePrice?.CurrencyCode, 2, languageCode);

                            Double.TryParse(Convert.ToString(pricebreakdown.RefundTaxesTotalPrice?.Amount), out double taxesAndFee);
                            taxesAndFee = taxesAndFee * numberoftravelers;

                            string taxesAndFeeTxt
                                = ConfigUtility.GetCurrencyAmount(taxesAndFee, pricebreakdown.RefundTaxesTotalPrice?.CurrencyCode, 2, languageCode);



                            double cancellationFee = 0.0d;
                            string cancellationFeeLanguageCode = string.Empty;
                            if (!_configuration.GetValue<bool>("DisableAtlanticBERefundFeePointOfOrigin") && pricebreakdown.RefundFeePointOfOrigin != null)
                            {
                                Double.TryParse(Convert.ToString(pricebreakdown.RefundFeePointOfOrigin?.Amount), out cancellationFee);
                                cancellationFeeLanguageCode = pricebreakdown.RefundFeePointOfOrigin?.CurrencyCode;
                            }
                            else
                            {
                                Double.TryParse(Convert.ToString(pricebreakdown.RefundFee?.Amount), out cancellationFee);
                                cancellationFeeLanguageCode = pricebreakdown.RefundFee?.CurrencyCode;
                            }


                            cancellationFee = cancellationFee * numberoftravelers;

                            string cancellationFeeTxt
                                = ConfigUtility.GetCurrencyAmount(cancellationFee, cancellationFeeLanguageCode, 2, languageCode);


                            var convertionInfo = new MOBConversionPricingInfo
                            {
                                BaseFare = $"Base fare : {baseFareTxt}",
                                TaxesAndFees = $"Taxes and fees : {ConfigUtility.GetCurrencyAmount(taxesAndFee, pricebreakdown.RefundTaxesTotalPrice?.CurrencyCode, 2, languageCode)}",
                            };

                            if (cancellationFee > 0)
                            {
                                convertionInfo.CancellationFees = (quoteRefundResponse.IsBECancelFeeEligible)
                                    ? $"Basic Economy cancellation charge : -{ConfigUtility.GetCurrencyAmount(cancellationFee, cancellationFeeLanguageCode, 2, languageCode)}"
                                    : $"Cancellation fees : -{ConfigUtility.GetCurrencyAmount(cancellationFee, cancellationFeeLanguageCode, 2, languageCode)}";
                            }

                            //Convertion Equivalent
                            if (fop.RefundAmountTicketEquivalent != null)//TODO
                            {
                                Double.TryParse(Convert.ToString(fop.RefundAmountTicketEquivalent?.Amount), out double refundFeeEquivalent);

                                if (refundFeeEquivalent > 0)
                                {
                                    string refundFeeEquivalentTxt
                                        = ConfigUtility.GetCurrencyAmount(refundFeeEquivalent, fop.RefundAmountTicketEquivalent?.CurrencyCode, 2, languageCode);

                                    string conversionDateTxt = DateTime.Now.ToString("MM/dd/yyyy");

                                    convertionInfo.ConversionEquivalent = $"(US{refundFeeEquivalentTxt})";

                                    var conversionInfo = PopulateConversionPricingInfo("BENonUSDConversionContent", "||", "|");
                                    convertionInfo.ToolTipInfo = new ConversionToolTipInfo
                                    {
                                        Title = conversionInfo.Title,
                                        Header = string.Format(conversionInfo.Header, refundFeeEquivalentTxt, refundAmountTxt),
                                        Body = string.Format(conversionInfo.Body, conversionDateTxt),
                                        ButtonText = conversionInfo.ButtonText
                                    };
                                }
                            }//Convertion ends

                            return convertionInfo;
                        }
                    }
                }
            }
            catch { return null; }

            return null;
        }
        public string FormattedQuoteType(MOBCancelRefundInfoResponse response, bool isPNRELF = false)
        {
            string formattedQuoteType = string.Empty;
            if (response != null)
            {
                string quoteType = response.Pricing.QuoteType;
                if (response.PolicyMessage.Contains("24-hour flexible"))
                {
                    formattedQuoteType = "24-hour policy";
                }
                else if (quoteType != null)
                {
                    switch (quoteType)
                    {
                        case "Refundable":
                            formattedQuoteType = "Refundable";
                            break;
                        case "FutureFlightCredit":
                            formattedQuoteType = "Non-refundable";
                            break;
                        case "NonRefunable":
                            formattedQuoteType = "Basic Economy";
                            break;
                        default:
                            formattedQuoteType = isPNRELF ? "Basic Economy" : "Unknown";
                            break;
                    }
                }
                else
                {
                    formattedQuoteType = isPNRELF ? "Basic Economy" : "Unknown";
                }
            }
            else
            {
                formattedQuoteType = isPNRELF ? "Basic Economy" : "Unknown";
            }
            return formattedQuoteType;
        }
        private string FormattedQuoteDisplayText(MOBQuoteRefundResponse quoteRefundResponse, string currencycode = "")
        {
            string displaytext = string.Empty;
            if (string.Equals(quoteRefundResponse.QuoteType, "nonrefundable", StringComparison.OrdinalIgnoreCase))
            {
                displaytext = "Future flight credit";
            }
            else if (string.Equals(quoteRefundResponse.QuoteType, "refundable", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(currencycode, "MIL", StringComparison.OrdinalIgnoreCase))
                {
                    displaytext = "Redeposit";
                }
                else
                {
                    displaytext = "Total refund";
                }
            }
            else if (string.Equals(quoteRefundResponse.QuoteType, "futureflightcredit", StringComparison.OrdinalIgnoreCase))
            {
                if (string.Equals(currencycode, "MIL", StringComparison.OrdinalIgnoreCase))
                {
                    displaytext = "Redeposit";
                }
                else if (quoteRefundResponse.IsMilesMoneyPaidFOP || quoteRefundResponse.IsBECancelFeeEligible)
                {
                    displaytext = "Total credit";
                }
                else
                {
                    displaytext = "Future flight credit";
                }
            }
            else if (string.Equals(quoteRefundResponse.QuoteType, "Unticketed", StringComparison.OrdinalIgnoreCase))
            {
                displaytext = string.Empty;
            }
            return displaytext;
        }
        private string Construct(MOBPolicy mobPolicy, MOBQuoteRefundResponse quoteRefundResponse)
        {
            var policyMessage = string.Empty;

            if (mobPolicy.IsNullOrEmpty()) return policyMessage;

            RefundPolicy refundPolicy;

            var isARefundPolicy = Enum.TryParse(mobPolicy.Code, out refundPolicy);

            if (!isARefundPolicy) return policyMessage;

            switch (refundPolicy)
            {
                case RefundPolicy.TwentyFourHour:
                    policyMessage = _configuration.GetValue<string>("Refund-24HourFlexibleBookingPolicyMessage");
                    break;
                case RefundPolicy.Refundable:
                    break;
                case RefundPolicy.NonRefundable:
                    break;
                case RefundPolicy.FutureFlightCredit:
                    {
                        if (!quoteRefundResponse.IsMilesMoneyPaidFOP)
                        {
                            policyMessage = _configuration.GetValue<string>("Refund-FutureFlightCreditMessage");
                        }
                        break;
                    }
                case RefundPolicy.NonRefundableBasicEconomy:
                    policyMessage = _configuration.GetValue<string>("Refund-BasicEconomyNonRefundable");
                    break;
                case RefundPolicy.ExpectionPolicy3:
                    policyMessage = string.Empty;
                    break;
                case RefundPolicy.ExecutiveBulk:
                    policyMessage = string.Empty;
                    break;
                case RefundPolicy.FareLock:
                    policyMessage = _configuration.GetValue<string>("Refund-FarelockMessage");
                    break;
                case RefundPolicy.ScheduleChangeRefundPolicy:
                case RefundPolicy.UnTicketTTL:
                case RefundPolicy.TicketPending:
                    policyMessage = string.Empty;
                    break;
                case RefundPolicy.Sev2WeatherPolicyRefundable:
                case RefundPolicy.Sev2WeatherPolicy:
                case RefundPolicy.Sev1WeatherPolicyRefundable:
                case RefundPolicy.Sev1WeatherPolicy:
                    policyMessage = _configuration.GetValue<string>("Refund-IRROPSMessage");
                    break;
                case RefundPolicy.IRROPS:
                    {
                        if (!quoteRefundResponse.IsAwardTravel)
                        {
                            policyMessage = _configuration.GetValue<string>("Refund-IRROPSMessage");
                        }
                        break;
                    }
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return policyMessage;
        }
        public static string FormatNumberPerLanguageCode(string languageCode, string number)
        {
            if (string.IsNullOrEmpty(languageCode))
                languageCode = "en-US";

            double.TryParse(number, out double numberInDouble);
            CultureInfo locCutlure = new CultureInfo(languageCode);
            Thread.CurrentThread.CurrentCulture = locCutlure;
            NumberFormatInfo LocalFormat = (NumberFormatInfo)NumberFormatInfo.CurrentInfo.Clone();

            return numberInDouble.ToString("N0", LocalFormat);
        }
        private string ToOriginalFormOfPayment(MOBPayment fopdetails)
        {
            if (fopdetails != null)
            {
                if (fopdetails.PaymentType == "CreditCard")
                {
                    string lastFourDigitsOfCC = fopdetails.AccountNumber.Length > 4 ? fopdetails.AccountNumber.Substring(fopdetails.AccountNumber.Length - 4) : string.Empty;
                    return string.Format("To original form of payment (**{0})", lastFourDigitsOfCC);
                }
                else if (fopdetails.PaymentType == "PayPal")
                    return "To original form of payment (PayPal)";
                else if (fopdetails.PaymentType == "Certificate")
                    return "To Electronic Travel Certificate";
                else if (fopdetails.PaymentType == "FutureFlightCredit")
                    return "To Future Flight Credit";
                else
                    return string.Empty;
            }
            return string.Empty;
        }
        private string GetFormattedQuoteAmount(bool isAwardTravel, MOBModifyFlowPricingInfo pricingInfo)
        {
            if (pricingInfo == null)
                return "";

            if (!isAwardTravel)
                return pricingInfo.TotalPaid;

            string awardQuote = pricingInfo.RefundMiles + " + " + pricingInfo.TotalPaid;
            if (pricingInfo.HasTotalDue)
                awardQuote += " (redeposit fee: " + pricingInfo.TotalDue + ")";

            return awardQuote;
        }
        private async void GetAncillaryProductsFromPaymentMethods(MOBQuoteRefundResponse quoteRefundResponse, MOBCancelRefundInfoResponse refundReservationResponse, string languageCode, List<MOBItem> catalogValues = null)
        {

            var refundCurrCode = quoteRefundResponse.RefundAmountTicket.CurrencyCode;

            if (quoteRefundResponse.RefundAncillaryProducts != null &&
                quoteRefundResponse.RefundAncillaryProducts.Count() > 0)
            {
                List<MOBModifyFlowPricingInfo> lineItems = new List<MOBModifyFlowPricingInfo>();
                if (_configuration.GetValue<bool>("EnableAncillaryRefundWithManualRefundCheck"))
                    lineItems = await GetPaymentMethodsLineItemsWithManualRefundCheck(quoteRefundResponse, refundCurrCode, quoteRefundResponse.RefundAmountTicket.Amount, refundReservationResponse.IsManualRefund, languageCode, catalogValues);
                else
                    lineItems = GetPaymentMethodsLineItems(quoteRefundResponse.RefundAncillaryProducts, refundCurrCode, quoteRefundResponse.RefundAmountTicket.Amount, languageCode);

                if (lineItems != null && lineItems.Count > 0)
                {
                    foreach (var item in lineItems)
                    {
                        if (item.QuoteDisplayText == "pricing")
                        {
                            refundReservationResponse.Pricing.TotalPaid = item.TotalPaid;
                            refundReservationResponse.Pricing.FormattedPricingDetail = refundReservationResponse.Pricing.TotalPaid;
                            if (item.PriceItems != null)
                                refundReservationResponse.Pricing.PriceItems.AddRange(item.PriceItems);
                        }
                        else
                        {
                            if (refundReservationResponse.Quotes == null)
                                refundReservationResponse.Quotes = new List<MOBModifyFlowPricingInfo>();

                            refundReservationResponse.Quotes.Add(item);
                        }
                    }
                }
            }

        }
        public async Task<List<MOBModifyFlowPricingInfo>> GetPaymentMethodsLineItemsWithManualRefundCheck
           (MOBQuoteRefundResponse quoteRefundResponse, string refundCurrCode,
           decimal RefundAmountTicket, bool IsManualRefund, string languageCode, List<MOBItem> catalogValues = null)
        {
            List<MOBModifyFlowPricingInfo> ancillaryquote = new List<MOBModifyFlowPricingInfo>();

            var refundAncillaryProducts = quoteRefundResponse.RefundAncillaryProducts;

            bool isPricingTotalPaidUpdated = false;

            if (refundAncillaryProducts != null)
            {

                var filtered = refundAncillaryProducts
                                         .SelectMany(_ => _.PaymentMethods)
                                         .GroupBy(_ => new { _.CurrencyCode, _.PaymentIndex });


                if (await _featureSettings.GetFeatureSettingValue("CancelRefundInfoNullPaymentMethodsFix").ConfigureAwait(false))
                {

                    filtered = refundAncillaryProducts
                                              .Where(product => product.PaymentMethods != null && product.PaymentMethods.Any())
                                              .SelectMany(_ => _.PaymentMethods)
                                              .GroupBy(_ => new { _.CurrencyCode, _.PaymentIndex });
                }

                foreach (var item in filtered)
                {
                    if (item.Key.CurrencyCode == refundCurrCode && item.Key.PaymentIndex == 1)
                    {
                        decimal refundCurrTotalSum = 0;
                        double totalPainInDouble = 0;


                        var refundCurrAncillary = refundAncillaryProducts
                                            .SelectMany(_ => _.PaymentMethods)
                                            .Where(x => x != null && x.PaymentIndex == item.Key.PaymentIndex && x.CurrencyCode == refundCurrCode);

                        refundCurrTotalSum = refundCurrAncillary.Sum(x => x.Amount);

                        //Check if IsManualRefund is set. 
                        // If it is IsManualRefund is TRUE - Display it as new row
                        // Else, Add Ancillary total amount (with paymentindex = 1 and x.CurrencyCode == refundCurrCode) to RefundAmountTIcket
                        if (quoteRefundResponse.IsMultipleRefundFOP || IsManualRefund)
                        {
                            RefundAmountTicket = 0;
                            refundCurrTotalSum = refundCurrTotalSum + RefundAmountTicket;
                            double.TryParse(refundCurrTotalSum.ToString(), out totalPainInDouble);
                            MOBModifyFlowPricingInfo priningInfo = new MOBModifyFlowPricingInfo();
                            priningInfo = new MOBModifyFlowPricingInfo
                            {
                                TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble, item.Key.CurrencyCode, 2, languageCode),
                                RefundFOPLabel = ToOriginalFormOfPaymentAncillary(refundCurrAncillary?.First()),
                                QuoteDisplayText = "Total refund",
                            };

                            if (IsManualRefund)
                            {
                                priningInfo.PriceItems = GetAncillaryPriceItems
                                    (refundAncillaryProducts, item.Key.CurrencyCode, item.Key.PaymentIndex, languageCode);
                            }

                            ancillaryquote.Add(priningInfo);
                        }
                        else
                        {
                            refundCurrTotalSum = refundCurrTotalSum + RefundAmountTicket;
                            double.TryParse(refundCurrTotalSum.ToString(), out totalPainInDouble);
                            MOBModifyFlowPricingInfo priningInfo = new MOBModifyFlowPricingInfo();
                            priningInfo = new MOBModifyFlowPricingInfo
                            {
                                TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble, item.Key.CurrencyCode, 2, languageCode),
                                RefundFOPLabel = "",
                                QuoteDisplayText = "pricing",
                            };
                            ancillaryquote.Add(priningInfo);
                        }
                        isPricingTotalPaidUpdated = true;
                    }
                    else
                    {
                        var otherAncillaries = refundAncillaryProducts
                                                    .SelectMany(_ => _.PaymentMethods)
                                                    .Where(x => x != null && x.PaymentIndex == item.Key.PaymentIndex && x.CurrencyCode == item.Key.CurrencyCode);
                        if (await _featureSettings.GetFeatureSettingValue("CancelRefundInfoNullPaymentMethodsFix").ConfigureAwait(false))
                        {

                            otherAncillaries = refundAncillaryProducts
                                          .Where(product => product.PaymentMethods != null && product.PaymentMethods.Any())
                                          .SelectMany(_ => _.PaymentMethods)
                                          .Where(x => x != null && x.PaymentIndex == item.Key.PaymentIndex && x.CurrencyCode == item.Key.CurrencyCode);
                        }
                        var sum = otherAncillaries.Sum(x => x.Amount);

                        double totalPainInDouble2 = 0;
                        double.TryParse(sum.ToString(), out totalPainInDouble2);


                        MOBModifyFlowPricingInfo ancillary = new MOBModifyFlowPricingInfo
                        {
                            TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble2, item.Key.CurrencyCode, 2, languageCode),
                            RefundFOPLabel = ToOriginalFormOfPaymentAncillary(otherAncillaries?.First(), catalogValues),
                            QuoteDisplayText = ConfigUtility.IsMFOPCatalogEnabled(catalogValues) && item.Key.CurrencyCode == "MIL" ? "Redeposit" : "Total refund",
                            PriceItems = IsManualRefund ? GetAncillaryPriceItems(refundAncillaryProducts, item.Key.CurrencyCode, item.Key.PaymentIndex, languageCode) : null
                        };
                        ancillaryquote.Add(ancillary);
                    }
                }

                // If, there are no Ancillary Products Purchased with PaymentIndex == 1 and quoteRefundResponse.RefundAmountTicket.CurrencyCode
                // Then,
                // Update the refundReservationResponse.Pricing.TotalPaid with quoteRefundResponse.RefundAmountTicket.Amount
                if (isPricingTotalPaidUpdated == false)
                {
                    double.TryParse(RefundAmountTicket.ToString(), out double refundAmountTicketInDouble);
                    MOBModifyFlowPricingInfo priningInfo = new MOBModifyFlowPricingInfo();
                    priningInfo = new MOBModifyFlowPricingInfo
                    {
                        TotalPaid = ConfigUtility.GetCurrencyAmount(refundAmountTicketInDouble, refundCurrCode, 2, languageCode),
                        RefundFOPLabel = "",
                        QuoteDisplayText = "pricing",
                    };
                    ancillaryquote.Add(priningInfo);
                }
            }
            return ancillaryquote;
        }

        public List<MOBModifyFlowPricingInfo> GetPaymentMethodsLineItems(List<AncillaryCharge> refundAncillaryProducts, string refundCurrCode,
                decimal RefundAmountTicket, string languageCode)
        {
            List<MOBModifyFlowPricingInfo> ancillaryquote = new List<MOBModifyFlowPricingInfo>();
            bool isPricingTotalPaidUpdated = false;

            if (refundAncillaryProducts != null)
            {

                var filtered = refundAncillaryProducts
                                          .SelectMany(_ => _.PaymentMethods)
                                          .GroupBy(_ => new { _.CurrencyCode, _.PaymentIndex });

                foreach (var item in filtered)
                {
                    if (item.Key.CurrencyCode == refundCurrCode && item.Key.PaymentIndex == 1)
                    {
                        decimal refundCurrTotalSum = 0;
                        double totalPainInDouble = 0;


                        refundCurrTotalSum = refundAncillaryProducts
                                            .SelectMany(_ => _.PaymentMethods)
                                            .Where(x => x != null && x.PaymentIndex == item.Key.PaymentIndex && x.CurrencyCode == refundCurrCode)
                                            .Sum(x => x.Amount);
                        refundCurrTotalSum = refundCurrTotalSum + RefundAmountTicket;
                        double.TryParse(refundCurrTotalSum.ToString(), out totalPainInDouble);
                        MOBModifyFlowPricingInfo priningInfo = new MOBModifyFlowPricingInfo();
                        priningInfo = new MOBModifyFlowPricingInfo
                        {
                            TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble, item.Key.CurrencyCode, 2, languageCode),
                            RefundFOPLabel = "",
                            QuoteDisplayText = "pricing",
                        };
                        ancillaryquote.Add(priningInfo);
                        isPricingTotalPaidUpdated = true;
                    }
                    else
                    {
                        var otherAncillaries = refundAncillaryProducts
                                          .SelectMany(_ => _.PaymentMethods)
                                          .Where(x => x != null && x.PaymentIndex == item.Key.PaymentIndex && x.CurrencyCode == item.Key.CurrencyCode);
                        var sum = otherAncillaries.Sum(x => x.Amount);

                        double totalPainInDouble2 = 0;
                        double.TryParse(sum.ToString(), out totalPainInDouble2);


                        MOBModifyFlowPricingInfo ancillary = new MOBModifyFlowPricingInfo
                        {
                            TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble2, item.Key.CurrencyCode, 2, languageCode),
                            RefundFOPLabel = ToOriginalFormOfPaymentAncillary(otherAncillaries?.First()),
                            QuoteDisplayText = "Total refund",
                        };
                        ancillaryquote.Add(ancillary);
                    }
                }

                // If, there are no Ancillary Products Purchased with PaymentIndex == 1 and quoteRefundResponse.RefundAmountTicket.CurrencyCode
                // Then,
                // Update the refundReservationResponse.Pricing.TotalPaid with quoteRefundResponse.RefundAmountTicket.Amount
                if (isPricingTotalPaidUpdated == false)
                {
                    double.TryParse(RefundAmountTicket.ToString(), out double refundAmountTicketInDouble);
                    MOBModifyFlowPricingInfo priningInfo = new MOBModifyFlowPricingInfo();
                    priningInfo = new MOBModifyFlowPricingInfo
                    {
                        TotalPaid = ConfigUtility.GetCurrencyAmount(refundAmountTicketInDouble, refundCurrCode, 2, languageCode),
                        RefundFOPLabel = "",
                        QuoteDisplayText = "pricing",
                    };
                    ancillaryquote.Add(priningInfo);
                }
            }
            return ancillaryquote;
        }
        private void GetAncillaryProducts(MOBQuoteRefundResponse quoteRefundResponse, MOBCancelRefundInfoResponse refundReservationResponse, string languageCode)
        {
            var refundCurrCode = quoteRefundResponse.RefundAmountTicket.CurrencyCode;

            if (quoteRefundResponse.RefundAncillaryProducts != null)
            {
                var groupbydisplayquotes = quoteRefundResponse.RefundAncillaryProducts
                                         .GroupBy(x => x.CurrencyCode);

                groupbydisplayquotes.ForEach(ancillaries =>
                {
                    if (ancillaries.First().PaymentMethod != null)
                    {
                        //Ancillary products with payment index = 1 will be added to Pricing.TotalPaid
                        decimal totalPaid = quoteRefundResponse.RefundAncillaryProducts.Where
                                                        (x => x.CurrencyCode == ancillaries.Key && x.PaymentMethod != null && x.PaymentMethod.PaymentIndex == 1)
                                                        .Sum(x => x.Amount);
                        double totalPainInDouble = 0;
                        double.TryParse(totalPaid.ToString(), out totalPainInDouble);

                        totalPaid = totalPaid + quoteRefundResponse.RefundAmountTicket.Amount;
                        double.TryParse(totalPaid.ToString(), out totalPainInDouble);
                        refundReservationResponse.Pricing.TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble, refundCurrCode, 2, languageCode);
                        refundReservationResponse.Pricing.FormattedPricingDetail = refundReservationResponse.Pricing.TotalPaid;

                        //Ancillary products with payment index > 1 and per currency code - will be added as a new block


                        var ancillariesProductsWithOtherPayments = quoteRefundResponse.RefundAncillaryProducts.Where
                                                      (x => x.CurrencyCode == ancillaries.Key && x.PaymentMethod != null && x.PaymentMethod.PaymentIndex > 1);

                        if (ancillariesProductsWithOtherPayments != null && ancillariesProductsWithOtherPayments.Count() > 0)
                        {


                            decimal totalPaid2 = ancillariesProductsWithOtherPayments
                                                           .Sum(x => x.Amount);
                            double totalPainInDouble2 = 0;
                            double.TryParse(totalPaid2.ToString(), out totalPainInDouble2);


                            var ancillaryquote = new MOBModifyFlowPricingInfo
                            {
                                TotalPaid = ConfigUtility.GetCurrencyAmount(totalPainInDouble2, ancillaries.Key, 2, languageCode),
                                RefundFOPLabel = ToOriginalFormOfPaymentAncillary(ancillaries?.First().PaymentMethod),
                                QuoteDisplayText = "Total refund",
                            };

                            if (refundReservationResponse.Quotes == null)
                                refundReservationResponse.Quotes = new List<MOBModifyFlowPricingInfo>();

                            refundReservationResponse.Quotes.Add(ancillaryquote);
                        }
                    }
                });
            }
        }
        private static NumberFormatInfo GetCurrencyFormatProviderSymbolDecimals(string currencyCode)
        {
            var currencyNumberFormat = (from culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                                        let region = new RegionInfo(culture.LCID)
                                        where String.Equals(region.ISOCurrencySymbol, currencyCode,
                                                            StringComparison.InvariantCultureIgnoreCase)
                                        select culture.NumberFormat).First();

            return currencyNumberFormat;
        }
        private string ToOriginalFormOfPaymentAncillary(Payment fopdetails, List<MOBItem> catalogValues = null)
        {
            if (fopdetails != null)
            {
                if (fopdetails.PaymentType == "CreditCard")
                {
                    string lastFourDigitsOfCC = fopdetails.AccountNumber.Length > 4 ? fopdetails.AccountNumber.Substring(fopdetails.AccountNumber.Length - 4) : string.Empty;
                    return string.Format("To original form of payment (**{0})", lastFourDigitsOfCC);
                }
                else if (fopdetails.PaymentType == "PayPal")
                    return "To original form of payment (PayPal)";
                else if (fopdetails.PaymentType == "Certificate")
                    return "To Electronic Travel Certificate";
                else if (ConfigUtility.IsMFOPCatalogEnabled(catalogValues) && fopdetails.PaymentType == "Miles")
                {
                    string lastFourDigitsOfMSign = fopdetails.MileagePlusNumber.Length >= 8 ? fopdetails.MileagePlusNumber.Substring(fopdetails.MileagePlusNumber.Length - 3) : string.Empty;
                    return string.Format("To MileagePlus account ****{0}", lastFourDigitsOfMSign);
                }
                else
                    return string.Empty;
            }
            return string.Empty;
        }
        private bool RefundQuoteFOPEligibility
    (MOBCancelRefundInfoResponse refundReservationResponse, MOBQuoteRefundResponse quoteRefundResponse)
        {
            //!refundReservationResponse.IsManualRefund
            //&& (quoteRefundResponse.IsMultipleRefundFOP || quoteRefundResponse.IsMilesMoneyFFCTRefundFOP)
            refundReservationResponse.IsCancellationFee
                = _configuration.GetValue<bool>("EnableBECancellationFeeChanges") && quoteRefundResponse.RefundFee != null;

            if (!refundReservationResponse.IsManualRefund
                && ((quoteRefundResponse.IsMultipleRefundFOP || quoteRefundResponse.IsMilesMoneyFFCTRefundFOP)
                || (refundReservationResponse.IsCancellationFee)
                || (quoteRefundResponse.ShowETCConvertionInfo)))
                return true;
            return false;
        }
        private bool ShowETCConvertionInfo(MOBCancelRefundInfoRequest request,
    QuoteRefundResponse cslQuoteResponse, MOBQuoteRefundResponse mobquoteResponse)
        {
            if (!GeneralHelper.IsApplicationVersionGreaterorEqual
                            (request.Application.Id, request.Application.Version.Major, "BECancellationFeeChangesAndroidversion", "BECancellationFeeChangesiOSversion"))
                return false;

            if (cslQuoteResponse?.PaymentMethods != null && cslQuoteResponse.PaymentMethods.Any())
            {
                return cslQuoteResponse.PaymentMethods.Where
                    (x => x.IsTicketRefundFOP == true
                    && string.Equals(x.PaymentType, "CERTIFICATE", StringComparison.OrdinalIgnoreCase)
                    && x.RefundAmountTicketEquivalent?.Amount > 0).Any();
            }
            return false;
        }
        private string GetTravelTime(string appVersion, TimeSpan journeyduration)
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
        private async Task<List<MOBPNRSegment>> GetAllSegments
    (MOBRequest request, Service.Presentation.ReservationResponseModel.ReservationDetail cslresponse)
        {
            //TODO null check
            List<MOBPNRSegment> segments = new List<MOBPNRSegment>();

            var flightsegments = cslresponse.Detail.FlightSegments;

            if (flightsegments == null || !flightsegments.Any()) return null;

            string confirmedActionCodes = _configuration.GetValue<string>("flightSegmentTypeCode");

            flightsegments.ToList().ForEach(async cslseg =>
            {
                if (confirmedActionCodes.IndexOf
                        (cslseg.FlightSegment.FlightSegmentType.Substring(0, 2), StringComparison.Ordinal) != -1)
                {
                    MOBPNRSegment segment = new MOBPNRSegment
                    {
                        SegmentNumber = cslseg.SegmentNumber,
                        TripNumber = cslseg.TripNumber,

                        ScheduledArrivalDateTime = cslseg.EstimatedArrivalTime,
                        ScheduledDepartureDateTime = cslseg.EstimatedDepartureTime,
                        ScheduledArrivalDateTimeGMT = cslseg.EstimatedArrivalUTCTime,
                        ScheduledDepartureDateTimeGMT = cslseg.EstimatedDepartureUTCTime,
                    };

                    if (cslseg.FlightSegment != null)
                    {
                        if (cslseg.FlightSegment.ArrivalAirport != null)
                        {
                            string airportName = string.Empty;
                            string cityName = string.Empty;
                            var tupleRes = await _airportDynamoDB.GetAirportCityName(cslseg.FlightSegment.ArrivalAirport.IATACode, _headers.ContextValues.SessionId, airportName, cityName);
                            airportName = tupleRes.airportName;
                            cityName = tupleRes.cityName;

                            segment.Arrival = new MOBAirport
                            {
                                Code = cslseg.FlightSegment.ArrivalAirport.IATACode,
                                City = cityName,
                                Name = airportName
                            };
                        }

                        if (cslseg.FlightSegment.DepartureAirport != null)
                        {
                            string airportName = string.Empty;
                            string cityName = string.Empty;
                            var tupleFlightRes = await _airportDynamoDB.GetAirportCityName(cslseg.FlightSegment.DepartureAirport.IATACode, _headers.ContextValues.SessionId, airportName, cityName);
                            airportName = tupleFlightRes.airportName;
                            cityName = tupleFlightRes.cityName;

                            segment.Departure = new MOBAirport
                            {
                                Code = cslseg.FlightSegment.DepartureAirport.IATACode,
                                City = cityName,
                                Name = airportName
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

                        segment.FlightTime
                        = GetTravelTime(request.Application.Version.Major, cslseg.FlightSegment.JourneyDuration);

                        segment.FlightNumber = cslseg.FlightSegment.FlightNumber;
                        //segment.FlightTime = Utility.GetCharactersticValue(                       

                        segment.OperationoperatingCarrier = new MOBAirline
                        {
                            Code = cslseg.FlightSegment.OperatingAirlineCode,
                            FlightNumber = cslseg.FlightSegment.OperatingAirlineFlightNumber,
                            Name = cslseg.FlightSegment.OperatingAirlineName,
                        };

                        var marketingcarrier = cslseg.FlightSegment.MarketedFlightSegment?.FirstOrDefault();

                        if (marketingcarrier != null)
                        {
                            segment.MarketingCarrier = new MOBAirline
                            {
                                Code = marketingcarrier.MarketingAirlineCode,
                                FlightNumber = marketingcarrier.FlightNumber,
                                Name = marketingcarrier.Description,
                            };
                        }
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
                    segment.CabinType = cslseg?.BookingClass?.Cabin?.Name;
                    segment.ClassOfServiceDescription = cslseg?.BookingClass?.Cabin?.Description;
                    segments.Add(segment);
                }
            });
            return segments;
        }
        private string GetHighestCabin
    (IEnumerable<United.Service.Presentation.SegmentModel.ReservationFlightSegment> totalTripSegments, bool isAward)
        {
            try
            {
                bool isFirst = totalTripSegments.Any
                    (x => x.BookingClass?.Cabin?.Name?.IndexOf("first", StringComparison.OrdinalIgnoreCase) > 0);
                bool isBusiness = totalTripSegments.Any
                    (x => x.BookingClass?.Cabin?.Name?.IndexOf("business", StringComparison.OrdinalIgnoreCase) > 0);

                return (isFirst) ? ((isAward) ? "awardFirst" : "first")
                    : (isBusiness) ? ((isAward) ? "awardBusinessFirst" : "businessFirst") : ((isAward) ? "awardEcon" : "econ");
            }
            catch { return string.Empty; }
        }
        private async Task<List<MOBTrip>> GetAllTrips
            (Service.Presentation.ReservationResponseModel.ReservationDetail cslresponse, bool isAward)
        {
            List<MOBTrip> trips = new List<MOBTrip>();

            var flightsegments = cslresponse.Detail.FlightSegments;

            if (flightsegments == null || !flightsegments.Any()) return null;

            bool enableNRSAEmgergencyPNRCancelFix = await _featureSettings.GetFeatureSettingValue("EnableNRSAEmgergencyPNRCancelFix").ConfigureAwait(false);

            if (flightsegments != null && flightsegments.Any())
            {
                flightsegments = enableNRSAEmgergencyPNRCancelFix ? flightsegments.Where(x => !string.IsNullOrEmpty(x.TripNumber)).OrderBy(x => x.TripNumber).ToCollection() : flightsegments.OrderBy(x => x.TripNumber).ToCollection();

                if ((flightsegments == null || !flightsegments.Any()) && enableNRSAEmgergencyPNRCancelFix) return null;

                int mintripnumber = Convert.ToInt32(flightsegments.Where
                    (x => !string.IsNullOrEmpty(x.TripNumber))?.Select(o => o.TripNumber).First());

                int maxtripnumber = Convert.ToInt32(flightsegments.Where
                    (x => !string.IsNullOrEmpty(x.TripNumber))?.Select(o => o.TripNumber).Last());

                for (int i = mintripnumber; i <= maxtripnumber; i++)
                {
                    MOBTrip pnrTrip = new MOBTrip();

                    var totalTripSegments = flightsegments.Where(o => o.TripNumber == i.ToString());

                    if (totalTripSegments == null) continue;

                    pnrTrip.Index = i;

                    foreach (United.Service.Presentation.SegmentModel.ReservationFlightSegment segment in totalTripSegments)
                    {
                        if (!string.IsNullOrEmpty(segment.TripNumber) && Convert.ToInt32(segment.TripNumber) == i)
                        {
                            string airportName = string.Empty;
                            string cityName = string.Empty;

                            pnrTrip.CabinType = GetHighestCabin(totalTripSegments, isAward);

                            if (segment.SegmentNumber == totalTripSegments.Min(x => x.SegmentNumber))
                            {
                                pnrTrip.Origin = segment.FlightSegment.DepartureAirport.IATACode;

                                var tupleFlightRes = await _airportDynamoDB.GetAirportCityName(pnrTrip.Origin, _headers.ContextValues.SessionId, airportName, cityName);
                                airportName = tupleFlightRes.airportName;
                                cityName = tupleFlightRes.cityName;
                                pnrTrip.OriginName = airportName;

                                pnrTrip.DepartureTime = segment.FlightSegment.DepartureDateTime;

                                pnrTrip.DepartureTimeGMT = segment.FlightSegment.DepartureUTCDateTime;

                            }
                            if (segment.SegmentNumber == totalTripSegments.Max(x => x.SegmentNumber))
                            {
                                pnrTrip.Destination = segment.FlightSegment.ArrivalAirport.IATACode;

                                var tupleResponse = await _airportDynamoDB.GetAirportCityName(pnrTrip.Destination, _headers.ContextValues.SessionId, airportName, cityName);
                                airportName = tupleResponse.airportName;
                                cityName = tupleResponse.cityName;
                                pnrTrip.DestinationName = airportName;

                                pnrTrip.ArrivalTime = segment.FlightSegment.ArrivalDateTime;

                                pnrTrip.ArrivalTimeGMT = segment.FlightSegment.ArrivalUTCDateTime;
                            }

                        }
                    }
                    trips.Add(pnrTrip);
                }
            }
            return trips;
        }
        private List<MOBPNRPassenger> GetAllPassangers
                   (MOBRequest request,
                   Service.Presentation.ReservationResponseModel.ReservationDetail cslresponse)
        {
            //TODO null check
            List<MOBPNRPassenger> passengers = new List<MOBPNRPassenger>();

            var travelers = cslresponse.Detail.Travelers;

            if (travelers == null || !travelers.Any()) return null;

            travelers.ToList().ForEach(pax =>
            {

                if (pax != null && pax.Person != null)
                {
                    MOBPNRPassenger passenger = new MOBPNRPassenger
                    {
                        SHARESPosition = pax.Person.Key,
                        TravelerTypeCode = pax.Person.Type,
                        SSRDisplaySequence = pax.Person.Key,
                        PricingPaxType = pax.Person.PricingPaxType
                    };

                    passenger.PNRCustomerID = pax.Person.CustomerID;
                    passenger.BirthDate = pax.Person.DateOfBirth;
                    passenger.SharesGivenName = pax.Person.GivenName;
                    passenger.PassengerName = new MOBName
                    {
                        First = pax.Person.GivenName,
                        Last = pax.Person.Surname,
                    };
                    passengers.Add(passenger);
                }
            });

            return passengers;
        }
        private async Task<List<MOBRefundOption>> CreateCancelRefundUserContent(Boolean isAward,
            MOBQuoteRefundResponse quoteRefundResponse, MOBCancelRefundInfoResponse refundReservationResponse)
        {
            string displaycontentid = "CANCEL_REFUND_OPTIONS_CONTENT";
            var refundoptions = new List<MOBRefundOption>();

            try
            {
                if (CheckEligibleRefundPolicy(quoteRefundResponse)) return null;

                quoteRefundResponse.IsRefundfeeAvailable =
                    (quoteRefundResponse.RefundFee != null && decimal.Parse(quoteRefundResponse.RefundFee.Amount) > 0);


                List<MOBItem> contentitems = await _manageResUtility.GetDBDisplayContent(displaycontentid);

                if (contentitems == null || !contentitems.Any()) return null;

                MapCancelUXSpecificContent(refundReservationResponse, contentitems, isAward);

                if (isAward)
                {
                    //refundReservationResponse.ReservationValueHeader = string.Empty;
                    if (quoteRefundResponse.IsCancelOnlyEligible)
                    {
                        refundoptions.Add(MapRefundTypeSpecificContent
                            (contentitems, RefundType.CNO, quoteRefundResponse, _AWARDCREDIT));

                        refundoptions.Add(MapRefundTypeSpecificContent
                            (contentitems, RefundType.CNRM, quoteRefundResponse, _TOTALREFUND));
                    }
                }
                else
                {
                    if (quoteRefundResponse.IsRevenueNonRefundable)
                    {
                        if (quoteRefundResponse.IsETCEligible)
                        {

                            refundReservationResponse.PolicyMessage = string.Empty;
                            //ETC
                            var etc = MapRefundTypeSpecificContent
                                (contentitems, RefundType.ETC, quoteRefundResponse, _TOTALCREDIT);
                            if (etc != null)
                            {
                                etc.RefundAmount = refundReservationResponse.Pricing.TotalPaid;
                                refundoptions.Add(etc);
                            }

                            //FFC
                            var ffc = MapRefundTypeSpecificContent
                               (contentitems, RefundType.FFC, quoteRefundResponse, _TOTALCREDIT);
                            if (ffc != null)
                            {
                                ffc.RefundAmount = refundReservationResponse.Pricing.TotalPaid;
                                refundoptions.Add(ffc);
                            }
                        }
                    }
                    else if (quoteRefundResponse.IsRevenueRefundable)
                    {
                        if (quoteRefundResponse.IsRefundfeeAvailable ||
                               _configuration.GetValue<bool>("CancelRefund_AllowFullRefundForMeaningfulScheduleChange"))
                        {

                            Enum.TryParse(quoteRefundResponse.Policy?.Code, out RefundPolicy refundpolicy);

                            if (refundpolicy != RefundPolicy.TwentyFourHour)
                            {
                                refundReservationResponse.PolicyMessage = string.Empty;
                            }

                            if (quoteRefundResponse.IsETCEligible)
                            {
                                //ETC
                                var etc = MapRefundTypeSpecificContent
                                    (contentitems, RefundType.ETC, quoteRefundResponse, _TOTALCREDIT);
                                if (etc != null)
                                {
                                    if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund"))
                                        GetRefundAmountWithNonUSD(refundReservationResponse, etc, quoteRefundResponse);
                                    else
                                        GetRefundAmount(refundReservationResponse, etc, quoteRefundResponse);
                                    refundoptions.Add(etc);
                                }
                            }

                            if (quoteRefundResponse.IsCancelOnlyEligible)
                            {
                                //FFC
                                var ffc = MapRefundTypeSpecificContent
                                   (contentitems, RefundType.FFC, quoteRefundResponse, _TOTALCREDIT);
                                if (ffc != null)
                                {
                                    if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund"))
                                        GetRefundAmountWithNonUSD(refundReservationResponse, ffc, quoteRefundResponse);
                                    else
                                        GetRefundAmount(refundReservationResponse, ffc, quoteRefundResponse);

                                    refundoptions.Add(ffc);
                                }
                            }

                            if (quoteRefundResponse.IsETCEligible || quoteRefundResponse.IsCancelOnlyEligible)
                            {
                                //OFOP
                                var ofop = MapRefundTypeSpecificContent
                                   (contentitems, RefundType.OFOP, quoteRefundResponse, _TOTALREFUND);
                                if (ofop != null)
                                {
                                    if (_configuration.GetValue<bool>("EnableNonUSDCurrencyChangesInCancelRefund"))
                                        GetRefundAmountWithNonUSD(refundReservationResponse, ofop, quoteRefundResponse);

                                    else
                                        GetRefundAmount(refundReservationResponse, ofop, quoteRefundResponse);
                                    refundoptions.Add(ofop);
                                }
                            }
                        }
                    }
                }

            }
            catch { return null; }
            return (refundoptions != null && refundoptions.Any()) ? refundoptions : null;
        }
        private bool CheckEligibleRefundPolicy(MOBQuoteRefundResponse quoteRefundResponse)
        {
            try
            {
                Enum.TryParse(quoteRefundResponse.Policy?.Code, out RefundPolicy refundpolicy);
                return (refundpolicy == RefundPolicy.UnTicketTTL
                    || refundpolicy == RefundPolicy.FareLock
                    || refundpolicy == RefundPolicy.TicketPending);
            }
            catch { return false; }
        }
        private static void MapCancelUXSpecificContent
            (MOBCancelRefundInfoResponse refundReservationResponse, List<MOBItem> contentitems, Boolean isAward)
        {
            try
            {
                var selecteditems
                    = contentitems.Where(x => x.Id.IndexOf("UX", StringComparison.OrdinalIgnoreCase) > -1);

                if (selecteditems != null && selecteditems.Any())
                {
                    if (!isAward)
                    {
                        refundReservationResponse.ReservationValueHeader
                            = selecteditems.Where(x => x.Id.IndexOf("ReservationValueLabel", StringComparison.OrdinalIgnoreCase) > -1)
                            ?.FirstOrDefault()
                            ?.CurrentValue;
                    }

                    refundReservationResponse.ScreenTitle
                        = selecteditems.Where(x => x.Id.IndexOf("ScreenTitle", StringComparison.OrdinalIgnoreCase) > -1)
                        ?.FirstOrDefault()
                        ?.CurrentValue;

                    refundReservationResponse.ConfirmButtonText
                        = selecteditems.Where(x => x.Id.IndexOf("ConfirmButtonText", StringComparison.OrdinalIgnoreCase) > -1)
                        ?.FirstOrDefault()
                        ?.CurrentValue;
                }
            }
            catch { }
        }
        private MOBRefundOption MapRefundTypeSpecificContent
            (List<MOBItem> contentitems, RefundType refundtype,
            MOBQuoteRefundResponse quoteRefundResponse, string quotedisplaytext)
        {
            var selectedcontent = new MOBRefundOption();
            string strrefundtype = Convert.ToString(refundtype);
            var selecteditems
                = contentitems.Where(x => x.Id.IndexOf(strrefundtype, StringComparison.OrdinalIgnoreCase) > -1);
            if (selecteditems != null && selecteditems.Any())
            {
                selectedcontent.Type = refundtype;
                selectedcontent.TypeDescription = refundtype.GetDisplayName();

                selectedcontent.QuoteDisplayText = quotedisplaytext;

                selectedcontent.DetailItems = new List<MOBItem>();
                selectedcontent.SubText = new List<MOBItem>();

                selecteditems.ForEach(item =>
                {
                    string[] split_id = item.Id.Split('|');
                    string itemid = (split_id.Length >= 2) ? split_id[1] : string.Empty;

                    switch (itemid)
                    {
                        case "ItemHeader":
                            selectedcontent.ItemHeader = item.CurrentValue;
                            break;
                        case "SubText":
                            {
                                string[] arrayCurrentValue = null;
                                if (!item.CurrentValue.IsNullOrEmpty())
                                {
                                    arrayCurrentValue = item.CurrentValue.Split('|');
                                }
                                if (selectedcontent.Type == RefundType.CNRM)
                                {
                                    string redepositfee = "0";
                                    if (quoteRefundResponse.AwardRedepositFee != null
                                    && !quoteRefundResponse.AwardRedepositFee.Amount.IsNullOrEmpty())
                                    {
                                        redepositfee = TrimDouble(Convert.ToString(quoteRefundResponse.AwardRedepositFee.Amount));
                                    }

                                    if (arrayCurrentValue.Length > 0)
                                    {
                                        if (arrayCurrentValue.Length == 1)
                                        {
                                            selectedcontent.SubText.Add(new MOBItem
                                            {
                                                CurrentValue = string.Format(arrayCurrentValue[0], redepositfee),
                                                Id = "SubText"
                                            });
                                        }
                                        else
                                        {
                                            arrayCurrentValue.ForEach(itemsubtext =>
                                            {
                                                selectedcontent.SubText.Add(new MOBItem
                                                {
                                                    CurrentValue = itemsubtext,
                                                    Id = "SubText"
                                                });
                                            });
                                        }
                                    }
                                }
                                else
                                {
                                    if (arrayCurrentValue.Length > 0)
                                    {
                                        arrayCurrentValue.ForEach(itemsubtext =>
                                        {
                                            selectedcontent.SubText.Add(new MOBItem
                                            {
                                                CurrentValue = itemsubtext,
                                                Id = "SubText"
                                            });
                                        });
                                    }
                                }
                                break;
                            }
                        case "DetailHeader":
                            selectedcontent.DetailHeader = item.CurrentValue;
                            break;
                        case "DetailText":
                            selectedcontent.DetailText = item.CurrentValue;
                            break;
                        default:
                            selectedcontent.DetailItems.Add(new MOBItem
                            {
                                Id = itemid,
                                CurrentValue = item.CurrentValue
                            });
                            break;
                    }

                });
            }
            return selectedcontent;
        }
        public string TrimDouble(string temp)
        {
            try
            {
                if (!_configuration.GetValue<bool>("DisableTrimDoubleIssue"))
                {
                    decimal result;
                    Decimal.TryParse(temp, out result);
                    var value = result.ToString("0.##");
                    return value == string.Empty ? "0" : value;
                }
                else
                {
                    var value = temp.IndexOf('.') == -1 ? temp : temp.TrimEnd('.', '0');
                    return value == string.Empty ? "0" : value;
                }

            }
            catch { return "0"; }
        }
        private void GetRefundAmountWithNonUSD(MOBCancelRefundInfoResponse refundReservationResponse,
            MOBRefundOption refundoption, MOBQuoteRefundResponse quoterefundresponse)
        {

            string refundamount = string.Empty;
            string formattedrefundfee = string.Empty;
            List<MOBItem> priceitems = new List<MOBItem>();
            try
            {
                string currencyCode = quoterefundresponse.RefundAmountTicket.CurrencyCode;
                string currencySymbol = ConfigUtility.GetCurrencyCode(currencyCode);

                if (string.IsNullOrEmpty(currencySymbol))
                    currencySymbol = currencyCode;

                var formattedbaseprice = refundReservationResponse.Pricing.PricesPerTypes.FirstOrDefault()?.TotalBaseFare;

                if (_configuration.GetValue<bool>("CancelRefund_FixFor_IncorrectBasePriceMultiPax") &&
                    quoterefundresponse.PriceBreakDown != null &&
                    quoterefundresponse.PriceBreakDown.Count > 0)
                {
                    double totalBasePrice = 0;

                    foreach (var price in quoterefundresponse.PriceBreakDown)
                    {
                        if (price.BasePrice != null)
                        {
                            double.TryParse(price.BasePrice.Amount, out double tempBasePrice);
                            tempBasePrice = tempBasePrice * price.TravelerCount;
                            totalBasePrice += tempBasePrice;
                        }
                    }
                    formattedbaseprice = ConfigUtility.GetCurrencyAmount(totalBasePrice, currencyCode, 2, "en-US");

                }

                string baseprice = formattedbaseprice.Replace(currencySymbol, "");

                priceitems.Add(new MOBItem
                { Id = _keyBASEFARE, CurrentValue = string.Format(_BASEFARE, formattedbaseprice) });

                string formattedtaxandfees = refundReservationResponse.Pricing.TaxesAndFeesTotal;
                string taxandfees = formattedtaxandfees.Replace(currencySymbol, "");

                priceitems.Add(new MOBItem
                { Id = _keyTAXANDFEE, CurrentValue = string.Format(_TAXANDFEE, formattedtaxandfees) });

                if (refundoption.Type == RefundType.OFOP)
                {
                    if (quoterefundresponse.RefundFee != null &&
                           double.TryParse(quoterefundresponse.RefundFee.Amount, out double refundfee))
                    {
                        formattedrefundfee = ConfigUtility.GetCurrencyAmount(refundfee, currencyCode, 2, "en-US");
                        priceitems.Add(new MOBItem
                        { Id = _keyCANCELFEE, CurrentValue = string.Format(_CANCELFEE, formattedrefundfee) });
                    }

                    refundoption.RefundAmount = refundReservationResponse.Pricing.TotalPaid;
                }
                else
                {
                    decimal.TryParse(baseprice, out decimal unformattedbaseprice);
                    decimal.TryParse(taxandfees, out decimal unformattedtaxandfees);
                    decimal totalAmount = (unformattedbaseprice + unformattedtaxandfees);

                    refundamount = ConfigUtility.GetCurrencyAmount(double.Parse(totalAmount.ToString()), currencyCode, 2, "");
                    refundoption.RefundAmount = refundamount;
                }
                refundoption.PriceItems = (priceitems.Any()) ? priceitems : null;
            }
            catch { }
        }

        private void GetRefundAmount(MOBCancelRefundInfoResponse refundReservationResponse,
            MOBRefundOption refundoption, MOBQuoteRefundResponse quoterefundresponse)
        {

            string refundamount = string.Empty;
            string formattedrefundfee = string.Empty;
            List<MOBItem> priceitems = new List<MOBItem>();
            try
            {
                var formattedbaseprice = refundReservationResponse.Pricing.PricesPerTypes.FirstOrDefault()?.TotalBaseFare;
                string baseprice
                   = (formattedbaseprice.IndexOf("$") > -1) ? formattedbaseprice.Replace("$", "") : formattedbaseprice;

                priceitems.Add(new MOBItem
                { Id = _keyBASEFARE, CurrentValue = string.Format(_BASEFARE, formattedbaseprice) });

                string formattedtaxandfees = refundReservationResponse.Pricing.TaxesAndFeesTotal;
                string taxandfees
                    = (formattedtaxandfees.IndexOf("$") > -1) ? formattedtaxandfees.Replace("$", "") : formattedtaxandfees;

                priceitems.Add(new MOBItem
                { Id = _keyTAXANDFEE, CurrentValue = string.Format(_TAXANDFEE, formattedtaxandfees) });

                if (refundoption.Type == RefundType.OFOP)
                {
                    if (quoterefundresponse.RefundFee != null &&
                           decimal.TryParse(quoterefundresponse.RefundFee.Amount, out decimal refundfee))
                    {
                        formattedrefundfee = (refundfee).ToString
                            ("c", GetCurrencyFormatProviderSymbolDecimals(_CURRENCYCODE));
                        priceitems.Add(new MOBItem
                        { Id = _keyCANCELFEE, CurrentValue = string.Format(_CANCELFEE, formattedrefundfee) });
                    }

                    refundoption.RefundAmount = refundReservationResponse.Pricing.TotalPaid;
                }
                else
                {
                    decimal.TryParse(baseprice, out decimal unformattedbaseprice);
                    decimal.TryParse(taxandfees, out decimal unformattedtaxandfees);

                    refundamount = (unformattedbaseprice + unformattedtaxandfees)
                        .ToString("c", GetCurrencyFormatProviderSymbolDecimals(_CURRENCYCODE));

                    refundoption.RefundAmount = refundamount;
                }
                refundoption.PriceItems = (priceitems.Any()) ? priceitems : null;
            }
            catch { }
        }
        private List<MOBModifyFlowPricingInfo> GetMUAandPPRUpgradeDetails(MOBQuoteRefundResponse quoteRefundResponse, List<MOBModifyFlowPricingInfo> quotes, string languageCode)
        {
            if (quoteRefundResponse.RefundUpgradeInstruments == null &&
                    quoteRefundResponse.RefundUpgradePoints == null)
                return quotes;

            if (quotes == null)
                quotes = new List<MOBModifyFlowPricingInfo>();

            // Calculate PPR PlusPoints
            if (quoteRefundResponse.RefundUpgradePoints != null
                    && quoteRefundResponse.RefundUpgradePoints.Count > 0)
            {
                var allUpgradePointsPerMP = quoteRefundResponse.RefundUpgradePoints
                    .GroupBy(r => r.RedeemingMileagePlusNumber)
                    .ToList();

                if (allUpgradePointsPerMP != null && allUpgradePointsPerMP.Count > 0)
                {
                    allUpgradePointsPerMP.ForEach(mp =>
                    {
                        float totalPlusPoints = quoteRefundResponse.RefundUpgradePoints
                                            .Where(x => x.RedeemingMileagePlusNumber == mp.Key)
                                            .Sum(x => x.Amount);

                        string refundFOPLabel = GetRefundFOPLabelForMUAAndPPR(mp.Key);
                        string formatedPlusPoints = FormatNumberPerLanguageCode(languageCode, totalPlusPoints.ToString());

                        var ancillaryquote = new MOBModifyFlowPricingInfo
                        {
                            TotalPaid = string.Format("{0} {1}", formatedPlusPoints, "PlusPoints"),

                            RefundFOPLabel = refundFOPLabel,

                            QuoteDisplayText = "Redeposit",
                        };


                        quotes.Add(ancillaryquote);
                    });
                }
            }

            // Calculate Miles (MUA)
            if (quoteRefundResponse.RefundUpgradeInstruments != null
                && quoteRefundResponse.RefundUpgradeInstruments.Count > 0)
            {
                var allUpgradeMilesPerMP = quoteRefundResponse.RefundUpgradeInstruments
                   .GroupBy(r => r.RedeemingMileagePlusNumber)
                   .ToList();

                if (allUpgradeMilesPerMP != null && allUpgradeMilesPerMP.Count > 0)
                {
                    allUpgradeMilesPerMP.ForEach(mp =>
                    {
                        float totalMiles = quoteRefundResponse.RefundUpgradeInstruments
                                            .Where(x => x.RedeemingMileagePlusNumber == mp.Key)
                                            .Sum(x => x.Amount);

                        string refundFOPLabel = GetRefundFOPLabelForMUAAndPPR(mp.Key);
                        string formatedMiles = FormatNumberPerLanguageCode(languageCode, totalMiles.ToString());
                        var ancillaryquote = new MOBModifyFlowPricingInfo
                        {
                            TotalPaid = string.Format("{0} {1}", formatedMiles, "miles"),

                            RefundFOPLabel = refundFOPLabel,

                            QuoteDisplayText = "Redeposit",
                        };


                        quotes.Add(ancillaryquote);
                    });
                }

            }


            return quotes;
        }
        private string GetRefundFOPLabelForMUAAndPPR(string mpNumber)
        {
            if (string.IsNullOrEmpty(mpNumber))
                return string.Empty;

            string maskedMpNumber = new string('*', mpNumber.Length - 3) + mpNumber.Substring(mpNumber.Length - 3);
            string fopLable = string.Format("{0} {1}", "To MileagePlus account", maskedMpNumber);
            return fopLable;
        }
        private bool IsShowRefundEmailFlag(List<MOBRefundOption> refundOptions, List<Payment> paymentMethods)
        {
            if (refundOptions == null && paymentMethods == null)
                return false;

            if (refundOptions != null)
            {
                var etcInRefundOptions = refundOptions.Where(x => x.Type == RefundType.ETC).Any();
                if (etcInRefundOptions)
                    return true;
            }
            if (paymentMethods != null)
            {
                var certificateInPaymentMethods = paymentMethods.Where(x => x.PaymentType == "Certificate").Any();
                if (certificateInPaymentMethods)
                    return true;
            }
            return false;
        }
        private bool IsDateOfBirthValidationRequired(int applicationId, string appVersion)
        {
            return _configuration.GetValue<bool>("CancelRefund-EnableDateOfBirthValidation")
                && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, _configuration.GetValue<string>("CancelRefund_DateOfBirthValidation_SupportedAppVersion_Android"), _configuration.GetValue<string>("CancelRefund_DateOfBirthValidation_SupportedAppVersion_iOS"));

        }
        private string ToRefundFormOfPayment(Payment fopdetails, bool isMFOPCatalogEnabled = false)
        {
            if (fopdetails.PaymentType == "CreditCard")
                return string.Format("To original form of payment (**{0})", fopdetails.AccountNumberLastFourDigits);
            else if (fopdetails.PaymentType == "PayPal")
                return "To original form of payment (PayPal)";
            else if (fopdetails.PaymentType == "Certificate")
                return "To Electronic Travel Certificate";
            else if (fopdetails.PaymentType == "FutureFlightCredit")
                return "To Future Flight Credit";
            else if (string.Equals(fopdetails.PaymentType, "MilesMoney", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(fopdetails.MileagePlusNumber))
                {
                    return string.Format("To MileagePlus account (****{0})",
                        fopdetails.MileagePlusNumber.Substring(fopdetails.MileagePlusNumber.Length - 2));
                }
                return string.Empty;
            }
            else if (isMFOPCatalogEnabled && string.Equals(fopdetails.PaymentType, "Miles", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.IsNullOrEmpty(fopdetails.MileagePlusNumber))
                {
                    return string.Format("To MileagePlus account (****{0})",
                        fopdetails.MileagePlusNumber.Substring(fopdetails.MileagePlusNumber.Length - 2));
                }
                return string.Empty;
            }
            else
                return string.Empty;
        }
        private List<MOBItem> GetAncillaryPriceItems(List<AncillaryCharge> refundAncillaryProducts, string curCode, int paymentIndex, string languageCode)
        {

            if (_configuration.GetValue<bool>("CancelRefund_DisableShowAncillaryRefundDetails") == false)
            {
                List<MOBItem> lstItems = new List<MOBItem>();

                refundAncillaryProducts.ForEach(a =>
                {
                    var products = a.PaymentMethods
                                 .Where(c => c.PaymentIndex == paymentIndex && c.CurrencyCode == curCode);

                    if (products != null && products.Count() > 0)
                    {
                        double.TryParse(a.Amount.ToString(), out double ancillaryAmountInDouble);
                        string formattedAmount = ConfigUtility.GetCurrencyAmount(ancillaryAmountInDouble, a.CurrencyCode, 2, languageCode);

                        MOBItem priceItem = new MOBItem
                        {
                            Id = "Description",
                            CurrentValue = string.Format("{0}: {1}", a.Description, formattedAmount)
                        };
                        lstItems.Add(priceItem);
                    }
                });
                return lstItems;
            }
            else
                return null;
        }
        public async Task<MOBCancelAndRefund> CancelAndRefund
         (MOBCancelAndRefundReservationRequest request, MOBQuoteRefundResponse quoterefundresponse)
        {
            string validToken = await _shoppingSessionHelper.GetSessionWithValidToken(request);
            return await CancelAndRefund(request, validToken, quoterefundresponse);
        }
        public async Task<MOBCancelAndRefund> CancelAndRefund
            (MOBCancelAndRefundReservationRequest request, string cslToken, MOBQuoteRefundResponse quoterefundresponse)
        {
            string logAction = request.IsAward ? "Award" : "Revenue";

            //var url = _configuration.GetValue<string>("ServiceEndPointBaseUrl - CSLRefundService") + "/CancelAndRefund";
            var url = "/CancelAndRefund";
            string quotetype = string.Empty;

            request.Token = cslToken;
            var cslRequest = new CancelAndRefundReservationRequest
            {
                RecordLocator = request.RecordLocator,
                EmailAddress = request.EmailAddress,
                LastName = request.LastName,
                Channel = request.Application.Id,
                PointOfSale = request.PointOfSale,
                MileagePlusNumber = request.MileagePlusNumber,
                CurrencyCode = request.CurrencyCode,
                QuoteType = request.QuoteType,
                RefundAmount = Convert.ToDecimal(quoterefundresponse.RefundAmount.Amount)
            };

            if (quoterefundresponse.RefundFee != null
                && decimal.TryParse(quoterefundresponse.RefundFee.Amount, out decimal refundfee))
            {
                cslRequest.RefundFee = refundfee;
            }

            quotetype = cslRequest.QuoteType;

            if (_configuration.GetValue<bool>("EnableCancelMoreRefundOptions")
                && request.SelectedRefundOptions != null && request.SelectedRefundOptions.Any())
            {
                if (request.IsAward)
                {
                    if (request.SelectedRefundOptions.Any(x => x.Type == RefundType.CNO))
                    {
                        cslRequest.CancelOnly = true;
                        quotetype = Convert.ToString(RefundPolicy.FutureFlightCredit);
                    }
                }
                else
                {
                    if (quoterefundresponse.IsRevenueNonRefundable)
                    {
                        if (request.SelectedRefundOptions.Any(x => x.Type == RefundType.ETC))
                        {
                            cslRequest.ETCRefund = true;
                            quotetype = Convert.ToString(RefundType.ETC);
                        }
                    }
                    else if (quoterefundresponse.IsRevenueRefundable)
                    {
                        if (quoterefundresponse.IsRefundfeeAvailable)
                        {
                            if (request.SelectedRefundOptions.Any(x => x.Type == RefundType.ETC))
                            {
                                cslRequest.ETCRefund = true;
                                quotetype = Convert.ToString(RefundType.ETC);
                            }
                            if (request.SelectedRefundOptions.Any(x => x.Type == RefundType.FFC))
                            {
                                cslRequest.CancelOnly = true;
                                quotetype = Convert.ToString(RefundType.FutureFlightCredit);
                            }
                        }
                        else
                        {
                            if (request.SelectedRefundOptions.Any(x => x.Type == RefundType.FFC))
                            {
                                cslRequest.CancelOnly = true;
                                quotetype = Convert.ToString(RefundType.FutureFlightCredit);
                            }
                        }
                    }
                }
            }

            if (quoterefundresponse.RefundMiles != null &&
                !string.IsNullOrEmpty(quoterefundresponse.RefundMiles.Amount) &&
                quoterefundresponse.RefundMiles.Amount != "0" &&
                quoterefundresponse.RefundMiles.Amount != "0.0")
            {
                cslRequest.RefundMiles = Convert.ToDouble(quoterefundresponse.RefundMiles.Amount);
                cslRequest.RefundUpgradeInstruments = quoterefundresponse.RefundUpgradeInstrumentsTotal;

            }

            if (quoterefundresponse.RefundUpgradePointsTotal != null &&
                !string.IsNullOrEmpty(quoterefundresponse.RefundUpgradePointsTotal.Amount))
            {
                cslRequest.RefundUpgradePoints = Convert.ToInt32(TrimDouble(quoterefundresponse.RefundUpgradePointsTotal.Amount));
            }
            if (quoterefundresponse.RefundAmountOtherCurrency != null)
            {
                double.TryParse(quoterefundresponse.RefundAmountOtherCurrency.Amount.ToString(), out double otherCurrencyAmountinDouble);
                cslRequest.RefundAmountOtherCurrency = otherCurrencyAmountinDouble;
                cslRequest.CurrencyCodeOther = quoterefundresponse.RefundAmountOtherCurrency.CurrencyCode;
            }


            if (request.IsAward)
            {
                cslRequest.AwardRedepositFee = request.AwardRedepositFee;
                cslRequest.AwardRedepositFeeTotal = request.AwardRedepositFeeTotal;
                cslRequest.RefundMiles = Convert.ToDouble(request.RefundMiles);

                if (request.FormOfPayment != null)
                    cslRequest.FormOfPayment = PopulateFormOfPayment(request.FormOfPayment, request.BillingAddress, request.AwardRedepositFee, request.sponsor, request.BillingPhone, request.EmailAddress);
                if (_configuration.GetValue<bool>("VormetricCancelRefundPathToggle") && cslRequest.FormOfPayment != null)
                {
                    //Mobile AppMOBILE-1220 Cancel / Refund Checkout
                    MOBVormetricKeys vormetricKeys = await _paymentUtility.GetVormetricPersistentTokenForViewRes(request.FormOfPayment.CreditCard, (request.DeviceId + request.MileagePlusNumber + "AWARDCANCELPATH").ToUpper(), cslToken);
                    if (!string.IsNullOrEmpty(vormetricKeys.PersistentToken))
                    {
                        cslRequest.FormOfPayment.CreditCard.PersistentToken = vormetricKeys.PersistentToken;
                        if (!string.IsNullOrEmpty(vormetricKeys.SecurityCodeToken))
                        {
                            cslRequest.FormOfPayment.CreditCard.SecurityCodeToken = vormetricKeys.SecurityCodeToken;
                            cslRequest.FormOfPayment.CreditCard.SecurityCode = vormetricKeys.SecurityCodeToken;
                        }

                        if (!string.IsNullOrEmpty(vormetricKeys.CardType) && string.IsNullOrEmpty(cslRequest.FormOfPayment.CreditCard.Code))
                        {
                            cslRequest.FormOfPayment.CreditCard.Code = vormetricKeys.CardType;
                        }
                    }
                }
            }

            if (IsDateOfBirthValidationRequired(request.Application.Id, request.Application.Version.Major) &&
                    request.DateOfBirth != null)
            {
                cslRequest.DateOfBirth = request.DateOfBirth;
            }
            var jsonRequest = JsonConvert.SerializeObject(cslRequest);

            var jsonResponse = await _cancelAndRefundService.PutCancelReservation(cslToken, request.SessionId, url, jsonRequest).ConfigureAwait(false);
            //TODO: Deserialize with GetResponseBodyAsJsonString if failed
            var cslResponse = JsonConvert.DeserializeObject<CancelAndRefundReservationResponse>(jsonResponse);

            MOBCancelAndRefund mOBCancelAndRefund = HandleCancelAndRefundResponse(cslResponse);
            mOBCancelAndRefund.QuoteType = quotetype;


            try
            {
                if (_configuration.GetValue<bool>("VormetricCancelRefundPathToggle"))
                {
                    if (!string.IsNullOrEmpty(request.RecordLocator) && request.FormOfPayment != null && request.AwardRedepositFee > 0)
                    {

                        _registerCFOP.AddPaymentNew
                        (request.TransactionId, request.Application.Id, request.Application.Version.Major,
                       "Cancel/Refund ReDeposit fee", (double)request.AwardRedepositFee, "USD", 0, jsonResponse,
                        request.Application.Id.ToString(), Convert.ToBoolean(_configuration.GetValue<string>("IsBookingTest")),
                        request.SessionId, request.DeviceId, request.RecordLocator, request.MileagePlusNumber,
                        request.FormOfPayment.FormOfPayment.ToString());
                    }
                }
            }
            catch { }

            return mOBCancelAndRefund;
        }
        United.Service.Presentation.PaymentModel.FormOfPayment PopulateFormOfPayment
           (MOBSHOPFormOfPayment formOfPayment, MOBAddress billingaddress, decimal amount,
           United.Service.Presentation.PersonModel.LoyaltyPerson sponsor, MOBCPPhone mobPhone, string emailAddress)
        {
            United.Service.Presentation.PaymentModel.CreditCard cc = new Service.Presentation.PaymentModel.CreditCard();
            cc.AccountNumber = formOfPayment.CreditCard.EncryptedCardNumber;
            cc.AccountNumberToken = formOfPayment.CreditCard.AccountNumberToken;
            cc.Amount = Convert.ToDouble(amount);
            cc.Code = formOfPayment.CreditCard.CardType;
            cc.Currency = new Service.Presentation.CommonModel.Currency();
            cc.Currency.Code = "USD";
            cc.ExpirationDate = formOfPayment.CreditCard.ExpireMonth.PadLeft(2, '0') + formOfPayment.CreditCard.ExpireYear.Substring(formOfPayment.CreditCard.ExpireYear.Length - 2);


            if (sponsor != null)
            {
                cc.Payor = new Service.Presentation.PersonModel.Person();
                cc.Payor.FirstName = sponsor.FirstName;
                cc.Payor.Contact = new Service.Presentation.PersonModel.Contact();

                if (emailAddress != null)
                {
                    cc.Payor.Contact.Emails = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.EmailAddress>();
                    Service.Presentation.CommonModel.EmailAddress email = new Service.Presentation.CommonModel.EmailAddress();
                    email.Address = emailAddress;
                    cc.Payor.Contact.Emails.Add(email);
                }


                if (mobPhone != null)
                {
                    cc.Payor.Contact.PhoneNumbers = new System.Collections.ObjectModel.Collection<Service.Presentation.CommonModel.Telephone>();
                    Service.Presentation.CommonModel.Telephone phone = new Service.Presentation.CommonModel.Telephone();
                    phone.Description = (mobPhone.ChannelCode != "H" ||
                                    mobPhone.ChannelCode != "B") ? mobPhone.ChannelCode = "H" : mobPhone.ChannelCode;

                    if (!string.IsNullOrEmpty(mobPhone.AreaNumber))
                    {
                        phone.AreaCityCode = mobPhone.AreaNumber;
                    }
                    phone.PhoneNumber = mobPhone.PhoneNumber;

                    phone.CountryAccessCode = mobPhone.CountryCode;
                    cc.Payor.Contact.PhoneNumbers.Add(phone);
                }

                cc.Payor.GivenName = sponsor.GivenName;
                cc.Payor.MiddleName = sponsor.MiddleName;
                cc.Payor.Suffix = sponsor.Suffix;
                cc.Payor.Surname = sponsor.Surname;
                cc.Payor.Title = sponsor.Title;
            }

            // Billing Address
            United.Service.Presentation.CommonModel.Address billingAddress = new Service.Presentation.CommonModel.Address();
            billingAddress.City = billingAddress.City;
            billingAddress.Country = billingAddress.Country;
            billingAddress.AddressLines = new System.Collections.ObjectModel.Collection<string>();
            billingAddress.AddressLines.Add(billingaddress.Line1);
            billingAddress.AddressLines.Add(billingaddress.Line2);
            billingAddress.AddressLines.Add(billingaddress.Line3);
            billingAddress.Country = new Service.Presentation.CommonModel.Country();
            billingAddress.Country.CountryCode = billingaddress.Country.Code;
            billingAddress.StateProvince = new Service.Presentation.CommonModel.StateProvince();
            billingAddress.StateProvince.Name = billingaddress.State.Name;
            billingAddress.StateProvince.StateProvinceCode = billingaddress.State.Code;
            billingAddress.PostalCode = billingaddress.PostalCode;

            United.Service.Presentation.PaymentModel.FormOfPayment paymentRequest = new United.Service.Presentation.PaymentModel.FormOfPayment();
            paymentRequest.CreditCard = cc;
            paymentRequest.CreditCard.BillingAddress = billingAddress;
            return paymentRequest;
        }
        private MOBCancelAndRefund HandleCancelAndRefundResponse(CancelAndRefundReservationResponse response)
        {
            var cancelAndRefund = new MOBCancelAndRefund
            {
                Pnr = response.Pnr,
                Email = response.Email
            };

            if (response.Error != null && response.Error.Count > 0)
            {
                throw new MOBUnitedException(response.Error[0].MajorCode);
            }

            if (response.StatusDetails == null || response.StatusDetails.Count <= 0) return cancelAndRefund;

            foreach (var status in response.StatusDetails)
            {
                switch (status.OperationName)
                {
                    case "FlightCancellation":
                        cancelAndRefund.CancelSuccess = GetStatusBoolean(status);
                        continue;
                    case "Refund":
                    case "AutoRedeposit":
                    case "Refund Ancillary":
                    case "Refund Instrument":
                    case "Refund Point":
                    case "FFCR Issue":
                    case "FFCR Reissue":
                    case "FFCR Void":
                    case "FFCT Issue":
                    case "FFCT Reissue":
                    case "FFCT Void":
                        cancelAndRefund.RefundSuccess = GetStatusBoolean(status);
                        continue;
                    case "Email":
                        cancelAndRefund.EmailSuccess = GetStatusBoolean(status);
                        continue;
                    default:
                        continue;
                }
            }

            return cancelAndRefund;
        }
        private bool GetStatusBoolean(Status status)
        {
            switch (status.StatusId)
            {
                case "Succeed":
                case "NotApplicable":
                    return true;
                default:
                    return false;
            }
        }
        public MOBException CSLErrorMappingCancelRefund(string errorcode)
        {
            MOBException exception = new MOBException();
            switch (errorcode)
            {
                case "503":
                    exception.Code = "5999";
                    exception.Message = "We were unable to charge your card as the authorization has been denied. Please contact your financial provider or use a different card.";
                    break;
                case "601":
                case "602":
                case "603":
                case "604":
                case "605":
                case "606":
                case "607":
                case "608":
                case "609":
                case "610":
                    exception.Code = "9999";
                    exception.Message = _configuration.GetValue<string>("GenericExceptionMessage");
                    break;
                case "637":
                case "638":
                    exception.Code = "7999";
                    exception.Message = string.Format(_configuration.GetValue<string>("CancelRefund_DOBValidation_ErrorMessage"), _configuration.GetValue<string>("Cancel-CustomerServicePhoneNumber"));
                    break;
                default:
                    return new MOBException
                    {
                        Code = "9999",
                        Message = _configuration.GetValue<string>("GenericExceptionMessage")
                    };
            }
            return exception;
        }
        public ConversionToolTipInfo PopulateConversionPricingInfo(string configkey, string splitchar1, string splitchar2)
        {
            try
            {
                string[] stringarray = _manageResUtility.SplitConcatenatedConfigValue(configkey, splitchar1);
                if (stringarray == null || !stringarray.Any()) return null;

                ConversionToolTipInfo content = new ConversionToolTipInfo();
                stringarray.ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] lineitem = ShopStaticUtility.SplitConcatenatedString(item, splitchar2);
                        if (lineitem?.Length > 1)
                        {
                            switch (lineitem[0])
                            {
                                case "Title":
                                    content.Title = lineitem[1];
                                    break;
                                case "Header":
                                    content.Header = lineitem[1];
                                    break;
                                case "Body":
                                    content.Body = lineitem[1];
                                    break;
                                case "ButtonText":
                                    content.ButtonText = lineitem[1];
                                    break;
                            }
                        }
                    }
                });

                return content;
            }
            catch { return null; }
        }

        public MOBPNRAdvisory PopulateConfigContent(string displaycontent, string splitchar)
        {
            try
            {
                string[] splitSymbol = { splitchar };

                string configentry = _configuration.GetValue<string>(displaycontent);

                if (string.IsNullOrEmpty(configentry)) return null;

                string[] items = configentry.Split(splitSymbol, StringSplitOptions.None);

                if (items == null || !items.Any()) return null;

                MOBPNRAdvisory content = new MOBPNRAdvisory();

                items.ToList().ForEach(item =>
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        string[] itemcontent = item.Split('|');

                        if (itemcontent != null && itemcontent.Length >= 2)
                        {
                            switch (itemcontent[0])
                            {
                                case "header":
                                    content.Header = itemcontent[1];
                                    break;
                                case "body":
                                    content.Body = itemcontent[1];
                                    break;
                                case "buttontext":
                                    content.Buttontext = itemcontent[1];
                                    break;
                                case "buttonlink":
                                    content.Buttonlink = itemcontent[1];
                                    break;
                            }
                        }
                    }
                });
                return content;
            }
            catch { return null; }
        }
    }
}