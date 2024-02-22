using Css.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Ebs.Logging.Enrichers;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.DynamoDB;
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Services.FlightShopping.Common.Extensions;
using United.Utility.Enum;
using United.Utility.Helper;

namespace United.Common.Helper.Shopping
{
    public class ShoppingSessionHelper : IShoppingSessionHelper
    {
        private readonly IConfiguration _configuration;
        private readonly ISessionHelperService _sessionHelperService;
        private readonly IDynamoDBService _dynamoDBService;
        private readonly IDPService _dPService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingUtility _shoppingUtility;
        private readonly IDataPowerFactory _dataPowerFactory;
        private readonly IHeaders _headers;
        private readonly ICacheLog<ShoppingSessionHelper> _logger;

        private Session _session;
        private readonly IApplicationEnricher _requestEnricher;
        private readonly IFeatureSettings _featureSettings;

        public Session CurrentSession
        {
            get
            {
                return _session;
            }
            set
            {
                _session = (value != null) ? value : _session;
            }
        }

        public ShoppingSessionHelper(IConfiguration configuration
            , ISessionHelperService sessionHelperService
            , IDynamoDBService dynamoDBService
            , IDPService dPService
            , IHttpContextAccessor httpContextAccessor
            , IShoppingUtility shoppingUtility,
            IDataPowerFactory dataPowerFactory,
            IApplicationEnricher requestEnricher
            , IHeaders headers
            , ICacheLog<ShoppingSessionHelper> logger
            , IFeatureSettings featureSettings)
        {
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _dynamoDBService = dynamoDBService;
            _dPService = dPService;
            _httpContextAccessor = httpContextAccessor;
            _shoppingUtility = shoppingUtility;
            _dataPowerFactory = dataPowerFactory;
            _requestEnricher = requestEnricher;
            _headers = headers;
            _logger = logger;
            _featureSettings = featureSettings;
        }

        public async Task<Session> CreateShoppingSession(int applicationId, string deviceId, string appVersion, string transactionId, string mileagPlusNumber, string employeeId, bool isBEFareDisplayAtFSR = false, bool isReshop = false, bool isAward = false, string travelType = "", string flow = "", string hashPin = "")
        {
            var session = new Session
            {
                DeviceID = deviceId,
                AppID = applicationId,
                SessionId = Guid.NewGuid().ToString().ToUpper().Replace("-", ""),
                CreationTime = DateTime.Now,
                LastSavedTime = DateTime.Now,
                MileagPlusNumber = mileagPlusNumber,
                IsBEFareDisplayAtFSR = isBEFareDisplayAtFSR,
                IsReshopChange = isReshop,
                IsAward = isAward,
                EmployeeId = employeeId
            };

            if (_configuration.GetValue<bool>("EnableCorporateLeisure"))
            {
                session.TravelType = travelType;
            }
            #region //**// LMX Flag For AppID change
            bool supressLMX = false;
            bool.TryParse(_configuration.GetValue<string>("SupressLMX"), out supressLMX); // ["SupressLMX"] = true to make all Apps Turn off. ["SupressLMX"] = false then will check for each app as below.
            if (!supressLMX && _configuration.GetValue<string>("AppIDSToSupressLMX") != null && _configuration.GetValue<string>("AppIDSToSupressLMX").ToString().Trim() != "")
            {
                string appIDS = _configuration.GetValue<string>("AppIDSToSupressLMX"); // AppIDSToSupressLMX = ~1~2~3~ or ~1~ or empty to allow lmx to all apps
                supressLMX = appIDS.Contains("~" + applicationId.ToString() + "~");
            }
            session.SupressLMXForAppID = supressLMX;
            #endregion
            var isValidToken = false;
            if (!string.IsNullOrEmpty(mileagPlusNumber))
            {
                if (flow == FlowType.BOOKING.ToString() && EnableVulnerabilityCheck("EnableVulnerabilityCheck", isReshop, applicationId, appVersion, _configuration.GetValue<string>("AndroidVulnerabilityCheckVersion"), _configuration.GetValue<string>("iPhoneVulnerabilityCheckVersion")))
                {
                    bool validAuthCall = await _shoppingUtility.Authorize(session.SessionId, applicationId, appVersion, deviceId, mileagPlusNumber, hashPin);
                    if (!validAuthCall) throw new MOBUnitedException(_configuration.GetValue<string>("VulnerabilityErrorMessage"));
                }
                var tupleRes = await GetMPAuthToken(mileagPlusNumber, applicationId, deviceId, appVersion, session);
                session.Token = tupleRes.validAuthToken;
                session = tupleRes.shopSession;
                var refreshShopTokenIfLoggedInTokenExpInThisMinVal = _configuration.GetValue<string>("RefreshShopTokenIfLoggedInTokenExpInThisMinVal") ?? "";
                if (string.IsNullOrEmpty(refreshShopTokenIfLoggedInTokenExpInThisMinVal))
                {
                    if (!string.IsNullOrEmpty(session.Token))
                    {
                        var tupleResponse =  await CheckIsCSSTokenValid(applicationId, deviceId, appVersion, transactionId, session, string.Empty);
                        isValidToken = tupleResponse.isTokenValid;
                        session = tupleResponse.shopTokenSession;
                    }
                }
                else
                {
                    var tupleResponse = await isValidTokenCheckWithExpireTime(applicationId, deviceId, appVersion, transactionId, session, refreshShopTokenIfLoggedInTokenExpInThisMinVal);
                    isValidToken = tupleResponse.isValidToken;
                    session = tupleResponse.session;
                }
            }

            if (!string.IsNullOrEmpty(session?.SessionId) && _headers.ContextValues != null)
            {
                _headers.ContextValues.SessionId = session.SessionId;
                _requestEnricher.Add(United.Mobile.Model.Constants.SessionId, session.SessionId);
            }

            if (isValidToken)
                return session;

            var token = await _dPService.GetAndSaveAnonymousToken(applicationId, deviceId, _configuration, "dpTokenRequest", session).ConfigureAwait(false);
            await _sessionHelperService.SaveSession<Session>(session, session.SessionId, new List<string> { session.SessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);
            session.Token = token;
            _session = session;
            return session;
        }


        private bool EnableVulnerabilityCheck(string enableConfigKey, bool isReshop, int applicationId, string appVersion, string androidVersionKey, string iOSVersionKey)
        {
            return _configuration.GetValue<bool>(enableConfigKey) && !isReshop
            && GeneralHelper.IsApplicationVersionGreaterorEqual(applicationId, appVersion, androidVersionKey, iOSVersionKey);
        }
        private async Task<(string validAuthToken, Session shopSession)> GetMPAuthToken(string mileagPlusNumber, int applicationId, string deviceId, string appVersion, Session shopSession)
        {
            string validAuthToken = string.Empty;
            try
            {
                var mileagePlusDynamoDB = new MileagePlusDynamoDB(_configuration, _dynamoDBService, _logger);
                var MPAuthToken = await mileagePlusDynamoDB.GetMPAuthTokenCSS<United.Mobile.Model.Internal.AccountManagement.MileagePlus>(mileagPlusNumber, applicationId, deviceId, appVersion, shopSession.SessionId).ConfigureAwait(false);
                if (MPAuthToken != null)
                {
                    if (!await _featureSettings.GetFeatureSettingValue("DisableLoggingHashPinCodeEmpty").ConfigureAwait(false)
                        && string.IsNullOrEmpty(MPAuthToken?.HashPincode))
                    {
                        _logger.LogWarning("DynamoDB - GetRecords Hashpincode is null/empty.");
                    }

                    validAuthToken = (_configuration.GetValue<bool>("EnableDPToken")) ? MPAuthToken.DataPowerAccessToken : MPAuthToken.AuthenticatedToken;
                    shopSession.Token = validAuthToken;
                    shopSession.IsTokenAuthenticated = true; // if the token is cached with MP at DB means its authenticated
                    shopSession.TokenExpirationValueInSeconds = Convert.ToDouble(MPAuthToken.TokenExpiryInSeconds);
                    shopSession.TokenExpireDateTime = Convert.ToDateTime(MPAuthToken.TokenExpireDateTime);
                    //  shopSession.CustomerID = Convert.ToInt32(MPAuthToken.CustomerID);
                }

            }
            catch (Exception ex) { string msg = ex.Message; }

            return (validAuthToken,shopSession);
        }

        private async Task<(bool isValidToken, Session session)> isValidTokenCheckWithExpireTime(int applicationId, string deviceId, string appVersion, string transactionId, Session session, string refreshShopTokenIfLoggedInTokenExpInThisMinVal)
        {
            bool isValidToken = false;
            try
            {
                if (!string.IsNullOrEmpty(session.Token) && session.TokenExpireDateTime.Subtract(DateTime.Now).TotalMinutes > Convert.ToInt32(refreshShopTokenIfLoggedInTokenExpInThisMinVal))
                {
                    var tupleResponse = await CheckIsCSSTokenValid(applicationId, deviceId, appVersion, transactionId, session, string.Empty);
                    isValidToken = tupleResponse.isTokenValid;
                    session = tupleResponse.shopTokenSession;
                }
            }
            catch
            {
                if (!string.IsNullOrEmpty(session.Token))
                {
                    var tupleResponse = await CheckIsCSSTokenValid(applicationId, deviceId, appVersion, transactionId, session, string.Empty);
                    isValidToken = tupleResponse.isTokenValid;
                    session = tupleResponse.shopTokenSession;
                }
            }

            return (isValidToken,session);
        }

        public async Task<(bool isTokenValid, Session shopTokenSession)> CheckIsCSSTokenValid(int applicationId, string deviceId, string appVersion, string transactionId, Session shopTokenSession, string tokenToValidate)
        {
            bool isTokenValid = false;
            bool iSDPAuthentication = _configuration.GetValue<bool>("EnableDPToken");
            if (iSDPAuthentication)
            {
                #region TFS 53524 - Added log for trace United Airlines sending loopback address for iOS apps 23/08/2016 - Srinivas - Auguest 23

                //  logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(shopTokenSession.SessionId, "HostIPAddress", "Request", applicationId, appVersion, deviceId, GetClientIPAddress()));

                #endregion
                isTokenValid = await _dataPowerFactory.CheckIsDPTokenValid(shopTokenSession.Token, shopTokenSession, transactionId);
            }
            else
            {
                #region Get Aplication and Profile Ids

                string request = string.Empty, guidForLogEntries = shopTokenSession != null ? (string.IsNullOrEmpty(shopTokenSession.SessionId) == true ? transactionId : shopTokenSession.SessionId) : transactionId;

                System.Guid appID = new Guid("643e1e47-1242-4b6c-ab7e-64024e4bc84c"); // default App Id
                System.Guid profID = new Guid("114bfe84-cc04-49b6-8d28-74294f1d21fc"); // default Profile Id
                try
                {
                    string[] cSSAuthenticationTokenServiceApplicationIDs = _configuration.GetValue<string>("CSSAuthenticationTokenServiceApplicationIDs").Split('|');
                    foreach (string applicationID in cSSAuthenticationTokenServiceApplicationIDs)
                    {
                        if (Convert.ToInt32(applicationID.Split('~')[0].ToString().ToUpper().Trim()) == applicationId)
                        {
                            appID = new Guid(applicationID.Split('~')[1].ToString().Trim());
                            break;
                        }
                    }
                    string[] cSSAuthenticationTokeServicenProfileIDs = _configuration.GetValue<string>("CSSAuthenticationTokenServiceProfileIDs").Split('|');
                    foreach (string profileID in cSSAuthenticationTokeServicenProfileIDs)
                    {
                        if (Convert.ToInt32(profileID.Split('~')[0].ToString().ToUpper().Trim()) == applicationId)
                        {
                            profID = new Guid(profileID.Split('~')[1].ToString().Trim());
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                    //logEntries.Add(United.Logger.LogEntry.GetLogEntry<MOBExceptionWrapper>(guidForLogEntries, "GetFLIFOSecurityToken", "Exception", applicationId, appVersion, deviceId, exceptionWrapper));
                }
                #endregion
                #region TFS 53524 - Added log for trace United Airlines sending loopback address for iOS apps 23/08/2016 - Srinivas - Auguest 23

                // logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(guidForLogEntries, "HostIPAddress", "Request", applicationId, appVersion, deviceId, GetClientIPAddress()));

                #endregion
                #region
                List<LogEntry> cssTokenlogEntries = new List<LogEntry>();
                string securityUrl = _configuration.GetValue<string>("CSSAuthenticationTokenGeneratorURL");
                var client = new Css.ChannelProxy.Client(securityUrl);
                List<Metadata> metadata = new List<Metadata>();
                RequestAttributeSet set = new RequestAttributeSet();
                set.Attributes.Add("RequestAttribute_DeviceId", deviceId);
                set.Attributes.Add("RequestAttribute_MobileAppID", applicationId.ToString());

                //As per Venkat this Flag needs to check to avoid multiple ip logging for same user
                //set.Attributes.Add("RequestAttribute_ClientIP", GetClientIPAddress());
                IsClientIP(set);
                set.Attributes.Add("RequestAttribute_Browser", _configuration.GetValue<string>("RequestAttribute_Browser"));
                set.Attributes.Add("RequestAttribute_BrowserPlatform", _configuration.GetValue<string>("RequestAttribute_BrowserPlatform"));
                set.Attributes.Add("RequestAttribute_BrowserVersion", _configuration.GetValue<string>("RequestAttribute_BrowserVersion"));
                set.Attributes.Add("RequestAttribute_Url", _configuration.GetValue<string>
                    ("RequestAttribute_Url"));
                metadata.Add(new Metadata("RequestAttributeSet", set.Serialize()));

                request = "Token Generated for Mobile AppID = " + applicationId.ToString() + "|Token Service Application GUID = " + appID + "| Profile GUID = " + profID;
                // logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(guidForLogEntries, "Request", "Get_Anonymous_CSS_Token", applicationId, appVersion, deviceId, request));

                //cssTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(guidForLogEntries, "Request", "Get_Anonymous_CSS_Token", applicationId, appVersion, deviceId, request));

                if (shopTokenSession != null)
                {
                    tokenToValidate = shopTokenSession.Token;
                }
                AcquireSessionContextCallWrapper scWrapper = client.AcquireSessionContext(appID, new Guid(tokenToValidate), true, true, true, true, metadata);
                //if (scWrapper.CallAuthenticationOperationResult == CallAuthenticationResult.Success && scWrapper.CallAuthorizationOperationResult == CallAuthorizationResult.Success && scWrapper.RequestIntegrityCheckOperationResult == RequestIntegrityCheckResult.Valid && (scWrapper.UseTokenValidationResult == UseTokenValidationResult.Valid || scWrapper.UseTokenValidationResult == UseTokenValidationResult.ValidAndExtended) && (scWrapper.SessionValidationResult == SessionValidationResult.Valid || scWrapper.SessionValidationResult == SessionValidationResult.ValidAndExtended))
                if (scWrapper.RequestIntegrityCheckOperationResult == RequestIntegrityCheckResult.Valid && (scWrapper.UseTokenValidationResult == UseTokenValidationResult.Valid || scWrapper.UseTokenValidationResult == UseTokenValidationResult.ValidAndExtended)) // As per Marwan Less check the better so as per his reply changed
                {
                    isTokenValid = true;
                    if (shopTokenSession != null)
                    {
                        shopTokenSession.IsTokenExpired = false;
                        shopTokenSession.TokenExpireDateTime = DateTime.Now.AddSeconds(shopTokenSession.TokenExpirationValueInSeconds);
                        int customerID = 0;
                        int.TryParse(scWrapper.UserName, out customerID);
                        shopTokenSession.CustomerID = customerID;
                    }
                }
                else
                {
                    isTokenValid = false;
                    if (shopTokenSession != null)
                    {
                        shopTokenSession.IsTokenExpired = true;
                    }
                }
                if (shopTokenSession != null)
                {

                   await _sessionHelperService.SaveSession<Session>(shopTokenSession, shopTokenSession.SessionId, new List<string> { shopTokenSession.SessionId, shopTokenSession.ObjectName }, shopTokenSession.ObjectName).ConfigureAwait(false);
                }
                #endregion
            }
            return (isTokenValid, shopTokenSession);
        }

        private void IsClientIP(RequestAttributeSet set)
        {
            if (_configuration.GetValue<string>("Get_ClientIP") == null || _configuration.GetValue<bool>("Get_ClientIP") == true)
            {
                set.Attributes.Add("RequestAttribute_ClientIP", GetClientIPAddress());
            }
            else
            {
                set.Attributes.Add("RequestAttribute_ClientIP", _configuration.GetValue<string>("RequestAttribute_ClientIP"));
            }
        }

        private string GetClientIPAddress()
        {
            string clientIP = string.Empty;
            //checking network availability for getting client host ipAddress.
            if (!string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString()))
            {
                clientIP = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            else
            {   //Assigning empty client ip address, if there is no network
                clientIP = _configuration.GetValue<string>("RequestAttribute_ClientIP");
            }
            return clientIP;
        }

        public async Task<Session> GetBookingFlowSession(string sessionId, bool isBookingFlow = true)
        {
            bool sessionExpiryMessageChange = _configuration.GetValue<bool>("sessionExpiryMessageChange");
            Session session = null;
            if (sessionExpiryMessageChange)
            {
                if (_configuration.GetValue<bool>("SessionExpiryMessageRedirectToHomerBookingMain"))
                {
                    session = await GetBookingShoppingSessionV2(sessionId, isBookingFlow);
                }
                else
                {
                    session = await GetBookingShoppingSession(sessionId, isBookingFlow);
                }
            }
            else
                session = await GetShoppingSession(sessionId, true);

            return await Task.FromResult(session);
        }

        private async Task<Session> GetBookingShoppingSessionV2(string sessionId, bool isBookingFlow)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }
            Session session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName });
            if (session == null)
            {
                if (isBookingFlow)
                    throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessageExceptionCode"), _configuration.GetValue<string>("BookingSessionExpiryMessage"));
                else
                    throw new MOBUnitedException(_configuration.GetValue<string>("GeneralSessionExpiryMessageExceptionCode"), _configuration.GetValue<string>("GeneralSessionExpiryMessage"));
            }
            if (GetTokenExpireDateTimeUTC(session))
            {
                session.IsTokenExpired = true;
                throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessageExceptionCode"), _configuration.GetValue<string>("BookingSessionExpiryMessage"));
            }

            session.LastSavedTime = DateTime.Now;

            await _sessionHelperService.SaveSession<Session>(session, sessionId, new List<string>() { sessionId, session.ObjectName }, session.ObjectName);

            return await Task.FromResult(session);
        }

        private async Task<Session> GetBookingShoppingSession(string sessionId, bool isBookingFlow)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }
            Session session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            if (session == null)
            {
                if (isBookingFlow)
                    throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessage"));
                else
                    throw new MOBUnitedException(_configuration.GetValue<string>("GeneralSessionExpiryMessage"));
            }
            if (GetTokenExpireDateTimeUTC(session))
            {
                session.IsTokenExpired = true;
                throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessage"));
            }
            session.LastSavedTime = DateTime.Now;

           await _sessionHelperService.SaveSession<Session>(session, sessionId, new List<string> { sessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);

            return session;
        }

        private bool GetTokenExpireDateTimeUTC(Session session)
        {
            var returnVal = false;
            try
            {
                DateTime localDateTime;

                var result = DateTime.TryParse(session.CreationTime.ToString(), out localDateTime);
                if (result)
                {
                    var expDatetime = localDateTime.AddSeconds(session.TokenExpirationValueInSeconds);
                    returnVal = (expDatetime.ToUniversalTime() <= DateTime.UtcNow && expDatetime != DateTime.MinValue);
                }


            }
            catch (FormatException)
            {
            }
            return returnVal;
        }

        private async Task<Session> GetShoppingSession(string sessionId, bool saveToPersist)
        {
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
            }

            Session session = new Session();
            session =  await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
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

        public async Task<Session> GetValidateSession(string sessionId, bool isBookingFlow, bool isViewRes_CFOPFlow)
        {
            if (string.IsNullOrEmpty(sessionId))
            {                
                sessionId =await _sessionHelperService.GetSessionId(_headers.ContextValues, string.Empty).ConfigureAwait(false);
            }

            Session session = new Session();
            session = await _sessionHelperService.GetSession<Session>(sessionId, session.ObjectName, new List<string> { sessionId, session.ObjectName }).ConfigureAwait(false);
            if (session == null)
            {
                if (isBookingFlow)
                    throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessage"));
                else if (isViewRes_CFOPFlow)
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOPSessionExpire).ToString(), _configuration.GetValue<string>("ViewResSessionExpiredMessage"));
                else
                    throw new MOBUnitedException(_configuration.GetValue<string>("GeneralSessionExpiryMessage"));
            }
            //if (session.TokenExpireDateTime <= DateTime.Now && session.TokenExpireDateTime != DateTime.MinValue)
            if (GetTokenExpireDateTimeUTC(session))
            {
                session.IsTokenExpired = true;
                if (isViewRes_CFOPFlow)
                    throw new MOBUnitedException(((int)MOBErrorCodes.ViewResCFOPSessionExpire).ToString(), _configuration.GetValue<string>("ViewResSessionExpiredMessage"));
                else
                    throw new MOBUnitedException(_configuration.GetValue<string>("BookingSessionExpiryMessage"));
            }

            session.LastSavedTime = DateTime.Now;
            await _sessionHelperService.SaveSession<Session>(session, sessionId, new List<string>() { sessionId, session.ObjectName }, session.ObjectName).ConfigureAwait(false);

            return session;
        }

        public async Task<Session> GetShoppingSession(string sessionId)
        {
            return await GetShoppingSession(sessionId, true);
        }
        public async Task<string> GetSessionWithValidToken(MOBModifyReservationRequest request)
        {
            string token = string.Empty;

            Session mySession = null;

            if (request.SessionId.IsNullOrEmpty())
            {
                mySession = GetNewSessionWithoutToken(request); //we weren't given an existing session, make a new one
            }
            else
            {
                mySession = await _sessionHelperService.GetSession<Session>(request.SessionId, new Session().ObjectName, new List<string> { request.SessionId, new Session().ObjectName }).ConfigureAwait(false); //load session from persist

                if (mySession.IsNullOrEmpty()) //if session doesn't exist then make a new one
                {
                    mySession = GetNewSessionWithoutToken(request);
                }
            }

            if (!await IsMileagPlusNumberTokenValid(request.Application.Id, request.DeviceId, request.Application.Version.Major, request.TransactionId, request.MileagePlusNumber, mySession))
            {
                //get a fresh token
                mySession.Token = await _dPService.GetAnonymousToken(request.Application.Id, request.DeviceId, _configuration);
            }

            return mySession.Token;
        }

        public async Task<bool> IsMileagPlusNumberTokenValid(int applicationId, string deviceId, string appVersion, string transactionId, string mileagPlusNumber, Session session)
        {
            bool isValidToken = false;
            if (!string.IsNullOrEmpty(mileagPlusNumber))
            {
                var mpAuthRes = await GetMPAuthToken(mileagPlusNumber, applicationId, deviceId, appVersion, session);
                if (!string.IsNullOrEmpty(mpAuthRes.validAuthToken))
                {
                    var cssRes = await CheckIsCSSTokenValid(applicationId, deviceId, appVersion, transactionId, session, mpAuthRes.validAuthToken);
                    isValidToken = cssRes.isTokenValid;
                }
            }
            return isValidToken;
        }


        private Session GetNewSessionWithoutToken(MOBRequest request)
        {
            var session = new Session
            {
                DeviceID = request.DeviceId,
                AppID = request.Application.Id,
                SessionId = NewSessionId(),
                CreationTime = DateTime.Now,
                LastSavedTime = DateTime.Now,
                SupressLMXForAppID = false,
                Token = string.Empty
            };

            return session;
        }

        public static string NewSessionId()
        {
            return System.Guid.NewGuid().ToString().Replace("{", "").Replace("-", "").Replace("}", "").ToUpper();
        }
    }
}
