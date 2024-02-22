using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using United.Mobile.Model.Common;
using United.Persist.Definition.FlightStatus;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class PersistToken : IPersistToken
    {
        private readonly ICacheLog<PersistToken> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResilientClient _resilientClient;
        private readonly int _expiryTime;
        private readonly int _absoluteExpirationInMin;
        public PersistToken([KeyFilter("sessionConfigKey")] IResilientClient resilientClient, ICacheLog<PersistToken> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
            int expiryTime = _configuration.GetValue<int>("SlidingExpiration");
            _expiryTime = expiryTime == 0 ? 10 : expiryTime;
            int absoluteExpirationInMin = _configuration.GetValue<int>("absoluteExpirationInMin");
            _absoluteExpirationInMin = absoluteExpirationInMin == 0 ? 10 : absoluteExpirationInMin;
        }


        public async Task<string> GetDpToken(string sessionId, string transactionId, string deviceId, string appId)
        {
            TimeSpan expiry = TimeSpan.FromSeconds(_expiryTime);
            string sessionObjectName = typeof(United.Persist.Definition.FlightStatus.CSLToken).FullName;

            SessionRequest sessionRequest = new SessionRequest()
            {
                TransactionId = transactionId,
                ValidationParams = new List<string>() { appId, deviceId },
                SessionId = sessionId,
                ObjectName = sessionObjectName,
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }
            };
            var requestData = JsonConvert.SerializeObject(sessionRequest);
            var pData = await _resilientClient.PostAsync("GetSession", requestData);

            if (!string.IsNullOrEmpty(pData))
            {

                var sessionResponse = JsonConvert.DeserializeObject<SessionResponse>(pData);

                if (!sessionResponse.Succeed || string.IsNullOrEmpty(sessionResponse.Data))
                {
                    _logger.LogError("GetDPToken {@Errors} and {sessionObjectName} and {sessionId}", _configuration.GetSection("invalidSession").GetValue<string>("Message"), sessionObjectName, sessionId);
                    
                    return default;
                }

                try
                {
                    var xmlSerialize = new XmlSerializer(typeof(CSLToken));
                    CSLToken cslToken = (sessionResponse.Data.Contains(@"<?xml")) ?
                                      (CSLToken)xmlSerialize.Deserialize(new StringReader(sessionResponse.Data)) :
                                       JsonConvert.DeserializeObject<CSLToken>(sessionResponse.Data);

                    if (cslToken == null)
                    {
                        _logger.LogInformation("GetDPToken {@Errors} and {sessionObjectName} and {sessionId}", _configuration.GetSection("invalidSession").GetValue<string>("Message"), sessionObjectName, sessionId);

                        return default;
                    }
                    _logger.LogInformation("GetDPToken {@cssFLIFOToken}", cslToken.Token);

                    if (cslToken?.ExpirationTime > DateTime.Now && !string.IsNullOrEmpty(cslToken?.Token))
                    {
                        return cslToken.Token;
                    }
                }
                catch (Exception)
                {

                }

            }

            return string.Empty;
        }

    }
}
