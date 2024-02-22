using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using United.Utility.AppVersion;

namespace United.Common.Helper
{
    public class Headers : IHeaders
    {
        private readonly ILogger<Headers> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRequestEnricher _requestEnricher;        
        public HttpContextValues ContextValues { get; set; }

        public Headers(ILogger<Headers> logger, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IRequestEnricher requestEnricher)
        {
            _logger = logger;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _requestEnricher = requestEnricher;
        }

        public void SetHeadersWhenNoHttpContext(string deviceId, string applicationId, string transactionId, string languageCode, string sessionId,string appVersion="4.1.55")
        {
            Regex regex = new Regex("[0-9.]");
            appVersion = string.Join("",
                regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());

            ContextValues = new HttpContextValues
            {
                Application = new Application()
                {
                    Id = string.IsNullOrEmpty(applicationId) ? 0 : Convert.ToInt32(applicationId),
                    Version = new Mobile.Model.Version
                    {
                        Major = 0,
                        Minor = 0
                    }
                },
                DeviceId = deviceId,
                LangCode = languageCode,
                TransactionId = transactionId,
                SessionId = sessionId
            };

            string[] versionLst = appVersion?.Split('.');
            if (versionLst?.Length == 3)
            {
                ContextValues.Application.Version.Major = int.Parse(versionLst[0]);
                ContextValues.Application.Version.Minor = int.Parse(versionLst[1]);
                ContextValues.Application.Version.Build = int.Parse(versionLst[2]);
            }
        }

        //Register at Context values at controller once Action/Method hit
        public async Task<bool> SetHttpHeader(string deviceId, string applicationId, string appVersion, string transactionId, string languageCode, string sessionId)
        {
            Regex regex = new Regex("[0-9.]");
            appVersion = string.Join("",
                regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());

            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppIdText] = applicationId;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText] = appVersion;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText] = appVersion;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderDeviceIdText] = deviceId;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderLangCodeText] = languageCode;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderRequestTimeUtcText] = DateTime.UtcNow.ToString();
            _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderTransactionIdText] = transactionId;
            _httpContextAccessor.HttpContext.Request.Headers[Constants.SessionId] = sessionId;

            if (!string.IsNullOrEmpty(sessionId))
            {
                _requestEnricher.Add(Constants.SessionId, sessionId);              
            }

            _requestEnricher.Add(Constants.TransactionIdText, transactionId);
            _requestEnricher.Add(Constants.ApplicationIdText, applicationId);
            _requestEnricher.Add(Constants.DeviceIdText, deviceId);
            _requestEnricher.Add(Constants.ApplicationVersionText, appVersion);

            ContextValues = new HttpContextValues
            {
                Application = new Application()
                {
                    Id = string.IsNullOrEmpty(applicationId) ? 0 : Convert.ToInt32(applicationId),
                    Version = new Mobile.Model.Version
                    {
                        Major = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString()) ? 0 : int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString().Substring(0, 1)),
                        Minor = string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText].ToString()) ? 0 : int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText].ToString().Substring(0, 1)),
                    }
                },
                DeviceId = deviceId,
                LangCode = languageCode,
                TransactionId = transactionId,
                SessionId = sessionId
            };

            string[] versionLst = appVersion?.Split('.');
            if (versionLst?.Length == 3)
            {
                ContextValues.Application.Version.Major = int.Parse(versionLst[0]);
                ContextValues.Application.Version.Minor = int.Parse(versionLst[1]);
                ContextValues.Application.Version.Build = int.Parse(versionLst[2]);
            }

            SetEnricherKeyValues(deviceId, applicationId, appVersion, transactionId, languageCode, sessionId);

            await Task.Delay(0);
            return true;
        }
        private void SetEnricherKeyValues(string deviceId, string applicationId, string appVersion, string transactionId, string languageCode, string sessionId)
        { 
            var appContextReqEnricher= _httpContextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestEnricher)) as IRequestEnricher;

            if (appContextReqEnricher == null)
                return;

            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.DeviceIdText, deviceId));
            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.SessionId, sessionId));
            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.TransactionIdText, transactionId));
            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.ApplicationVersionText, appVersion));
            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(Constants.HeaderLangCodeText, languageCode));
            appContextReqEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(Constants.ApplicationIdText, applicationId));

        }

        public void AddPropertyToEnricher(string key, dynamic value)
        {
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                _requestEnricher.Add(key, value);
            }
        }

        //Register at Context values at controller once Action/Method hit
        public async Task<bool> SetHttpHeader()
        {
            ContextValues = new HttpContextValues
            {
                Application = new Application()
                {
                    Id = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppIdText]),
                    Version = new Mobile.Model.Version
                    {
                        Major = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString().Substring(0, 1)),
                        Minor = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText].ToString().Substring(0, 1)),
                    }
                },
                DeviceId = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderDeviceIdText],
                LangCode = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderLangCodeText],
                RequestTimeUtc = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderRequestTimeUtcText],
                TransactionId = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderTransactionIdText],
                SessionId = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderSessionIdText]
            };
            _requestEnricher.Add(Constants.TransactionIdText, ContextValues.TransactionId);
            _requestEnricher.Add(Constants.ApplicationIdText, ContextValues.Application.Id);
            _requestEnricher.Add(Constants.DeviceIdText, ContextValues.DeviceId);
            _requestEnricher.Add(Constants.ApplicationVersionText, ContextValues.Application.Version.ToString());

            await Task.Delay(0);
            return true;
        }

    }
}
