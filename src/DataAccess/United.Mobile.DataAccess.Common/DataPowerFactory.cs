using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Foundations.Practices.Framework.Security.DataPower;
using United.Foundations.Practices.Framework.Security.DataPower.Models;
using United.Mobile.Model.Common;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.Common
{
    public class DataPowerFactory : IDataPowerFactory
    {
        private readonly ICacheLog<DataPowerFactory> _logger;
        private readonly IConfiguration _configuration;
        private IDataPowerGateway dpGateway = null;
        private IDataPowerGateway dpSSOGateway = null;
        private readonly ISessionHelperService _sessionHelperService;
        public DataPowerFactory(IConfiguration configuration, ISessionHelperService sessionHelperService,
            ICacheLog<DataPowerFactory> logger)
        {
            _configuration = configuration;
            _sessionHelperService = sessionHelperService;
            _logger = logger;
            dpGateway = new DataPowerGateway(_configuration.GetValue<string>("DPDiscoveryDocumentEndPoint"));
            dpSSOGateway = new DataPowerGateway(_configuration.GetValue<string>("DPDiscoveryDocumentEndPoint"));
        }

        public async Task<bool> CheckIsDPTokenValid(string _dpAccessToken, Session shopTokenSession, string transactionId, bool SaveToPersist = true)
        {
            bool _bTokenValid = false;
            DpToken res = null;
            string LogSessionID = shopTokenSession != null ? shopTokenSession.SessionId : transactionId;
            #region Log Request
            try
            {
                //if (levelSwitch != null && levelSwitch.TraceInfo)
                //{
                //    request = "Save to Persist = " + SaveToPersist + "  Validate Token = " + _dpAccessToken;

                //    logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(LogSessionID, "CheckIsDPTokenValid", "Request", applicationId, appVersion, deviceId, request));
                //}
                //_dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(LogSessionID, "CheckIsDPTokenValid", "Request", applicationId, appVersion, deviceId, request));
            }
            catch { }
            #endregion
            try
            {
                res = dpGateway.VerifyAccessToken(_dpAccessToken);
                if (res.Active && string.IsNullOrWhiteSpace(res.Error))
                {
                    _bTokenValid = true;
                    if (shopTokenSession != null && SaveToPersist)
                    {
                        shopTokenSession.IsTokenExpired = false;
                        //shopTokenSession.TokenExpireDateTime = DateTime.Now.AddSeconds(shopTokenSession.TokenExpirationValueInSeconds);  ////**==>> get the expiration from here expstr=2018-03-11T01:01:12Z
                    }
                }

                else if (shopTokenSession != null)
                {
                    shopTokenSession.IsTokenExpired = true;
                }
                if (shopTokenSession != null)
                {
                    await _sessionHelperService.SaveSession<Session>(shopTokenSession, shopTokenSession.SessionId, new List<string> { shopTokenSession.SessionId, shopTokenSession.ObjectName }, shopTokenSession.ObjectName).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                //#region Exception Log

                //if (levelSwitch != null && levelSwitch.TraceInfo)
                //{
                //    logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(LogSessionID, "CheckIsDPTokenValid", "Exception", applicationId, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString(), true, true));
                //}

                //_dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(LogSessionID, "CheckIsDPTokenValid", "Exception", applicationId, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString()));
                //if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAnonymousDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAnonymousDPToken"]))
                //{
                //    Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                //}
                //throw ex;
                //#endregion
            }

            #region Log Response
            try
            {
                //if (levelSwitch != null && levelSwitch.TraceInfo)
                //{
                //    logEntries.Add(United.Logger.LogEntry.GetLogEntry<DpToken>(LogSessionID, "CheckIsDPTokenValid", "Response", applicationId, appVersion, deviceId, res, true, true));
                //}

                //  _dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<DpToken>(LogSessionID, "CheckIsDPTokenValid", "Response", applicationId, appVersion, deviceId, res));
                //if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAnonymousDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAnonymousDPToken"]))
                //{
                //    Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                //}
            }
            catch { }
            #endregion

            return _bTokenValid;
        }

        public DPAccessTokenResponse GetDPAuthenticatedToken(int applicationID, string deviceId, string transactionId, string appVersion, Session TokenSessionObj, string username, string password, string usertype, string anonymousToken, DpRequest dpRequest, bool SaveToPersist = true)
        {
            DPAccessTokenResponse _DpResponse = new DPAccessTokenResponse();
            if (applicationID != 0)
            {
                var DpReqObj = dpRequest;
                DpToken dpTokenResponse = null;
                //List<LogEntry> _dpTokenlogEntries = new List<LogEntry>();
                string request = string.Empty;

                #region Log Token Request
                try
                {
                    //_dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<DpRequest>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Request", applicationID, appVersion, deviceId, DpReqObj));
                    //if (levelSwitch != null && levelSwitch.TraceInfo)
                    //{
                    //    //request = "Token Generated for ClientID = " + DpReqObj.ClientId.ToString() + "|Scope = " + DpReqObj.Scope + "|UserName = " + username + "|UserType = " + usertype + "|anonymousToken = " + anonymousToken;

                    //    if (logEntries != null) logEntries.Add(United.Logger.LogEntry.GetLogEntry<DpRequest>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Request", applicationID, appVersion, deviceId, DpReqObj));
                    //}
                }
                catch { }
                #endregion

                try
                {
                    // Acquire Authenticated token
                    dpTokenResponse = dpGateway.AcquireAuthenticatedToken(DpReqObj.ClientId, DpReqObj.ClientSecret, DpReqObj.Scope, username, password, usertype, null, anonymousToken, DpReqObj.EndUserAgentIP, DpReqObj.EndUserAgentId);

                    if (dpTokenResponse != null)
                    {
                        IJwtTokenHandle idTokenHandle = DecryptIDTokenForMPInfo(dpTokenResponse.IdToken);
                        _DpResponse.CustomerId = Convert.ToInt32(idTokenHandle.CustomerId);
                        _DpResponse.MileagePlusNumber = idTokenHandle.LoyaltyId;
                        _DpResponse.Expires_in = dpTokenResponse.ExpiresIn;
                        _DpResponse.AccessToken = anonymousToken;//dpTokenResponse.TokenType + " " + dpTokenResponse.AccessToken;
                        if (TokenSessionObj != null && SaveToPersist)
                        {
                            TokenSessionObj.Token = anonymousToken;//_DpResponse.AccessToken;// we can keep using Anonymous Token as if this device isnot remembered https://csmc.qa.api.united.com/8.0/security/SecureProfile/api/ValidateDevice means first time customer signin on device we will sign out (grant_type=revoke_token) and then use the same Authenticated token
                            TokenSessionObj.MileagPlusNumber = _DpResponse.MileagePlusNumber;
                            TokenSessionObj.CustomerID = _DpResponse.CustomerId;
                            TokenSessionObj.IsTokenAuthenticated = true;
                            TokenSessionObj.IsTokenExpired = false;
                            TokenSessionObj.TokenExpirationValueInSeconds = Convert.ToDouble(_DpResponse.Expires_in);
                            TokenSessionObj.TokenExpireDateTime = DateTime.Now.AddSeconds(Convert.ToDouble(_DpResponse.Expires_in));
                            //todo.. United.Persist.FilePersist.Save<United.Persist.Definition.Shopping.Session>(TokenSessionObj.SessionId, TokenSessionObj.ObjectName, TokenSessionObj);
                        }
                        #region Log Response
                        //try
                        //{
                        //    if (levelSwitch != null && levelSwitch.TraceInfo)
                        //    {
                        //        logEntries.Add(United.Logger.LogEntry.GetLogEntry<DpToken>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Response", applicationID, appVersion, deviceId, dpTokenResponse, true, false));
                        //    }

                        //    _dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<DpToken>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Response", applicationID, appVersion, deviceId, dpTokenResponse));
                        //    if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]))
                        //    {
                        //        System.Threading.Tasks.Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                        //    }
                        //}
                        //catch { }
                        #endregion
                    }
                    else
                    {
                        //_dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Exception", applicationID, appVersion, deviceId, "Data Power call returned Null!"));
                        //if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]))
                        //{
                        //    Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                        //}
                    }
                }
                catch (Exception ex) //todo HttpException ex
                {
                    _DpResponse.IsDPThrownErrors = true;
                    try
                    {
                        DpErrorDetail dpError = JsonConvert.DeserializeObject<DpErrorDetail>(ex.Message);

                        if (dpError != null && dpError.ErrorDetail != null)
                        {
                            _DpResponse.ErrorCode = Convert.ToInt32(dpError.ErrorDetail.ErrorCode);
                            _DpResponse.ErrorDescription = dpError.ErrorDetail.ErrorDescription;
                            _DpResponse.FailedAttempts = dpError.ErrorDetail.FailedAttempts;
                        }
                    }
                    catch (Exception)
                    {
                        #region Exception Log

                        //if (levelSwitch != null && levelSwitch.TraceInfo)
                        //{
                        //    logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Exception", applicationID, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString(), true, true));
                        //}

                        //_dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Exception", applicationID, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString()));
                        //if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]))
                        //{
                        //    Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                        //}
                        //throw ex;
                        #endregion

                    }
                }
                //catch (Exception ex)
                //{
                //    #region Exception Log

                //    if (levelSwitch != null && levelSwitch.TraceInfo)
                //    {
                //        logEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Exception", applicationID, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString(), true, true));
                //    }

                //    _dpTokenlogEntries.Add(United.Logger.LogEntry.GetLogEntry<string>(TokenSessionObj.SessionId, "Get_Authenticate_DP_Token", "Exception", applicationID, appVersion, deviceId, ex.Message.ToString() + " :: " + ex.StackTrace.ToString()));
                //    if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]) && Convert.ToBoolean(ConfigurationManager.AppSettings["LogAuthenticationDPToken"]))
                //    {
                //        Task.Factory.StartNew(() => Authentication.Write(_dpTokenlogEntries));
                //    }
                //    throw ex;
                //    #endregion

                //}
            }
            return _DpResponse;
        }

        private IJwtTokenHandle DecryptIDTokenForMPInfo(string idToken)
        {
            IJwtTokenHandle idTokenHandle = new JwtTokenHandle(idToken);
            return idTokenHandle;
        }
    }
}
