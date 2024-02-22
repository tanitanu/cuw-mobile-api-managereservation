using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.Model.Internal;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class DPProdService : IDPProdService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<DPService> _logger;
        private readonly ICachingService _cachingService;
        private readonly IHeaders _headers;
        public DPProdService([KeyFilter("dpTokenConfigProdKey")] IResilientClient resilientClient
            , ICacheLog<DPService> logger
            , ICachingService cachingService
            , IHeaders headers)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _cachingService = cachingService;
            _headers = headers;

        }
        public async Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration)
        {
            return await GetAnonymousToken(applicationId, deviceId, configuration, "dpTokenRequest");
        }

        public async Task<string> GetAnonymousToken(int applicationId, string deviceId, IConfiguration configuration, string configSectionKey)

        {
            string dpTokenResponse = string.Empty;
            string key = string.Format(configuration.GetSection("dpTokenConfigProd").GetValue<string>("tokenKeyFormat"), deviceId, applicationId);
            _logger.LogInformation("Dp token {key}", key);
            try
            {
                var token = await _cachingService.GetCache<DPTokenResponse>(key, _headers.ContextValues?.TransactionId);
                var dptoken = JsonConvert.DeserializeObject<DPTokenResponse>(token);

                if (dptoken == null)
                {
                    var dpRequest = GetDPRequest(applicationId, configuration, configSectionKey);
                    Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                    string requestData = JsonConvert.SerializeObject(dpRequest);
                    token = await _resilientClient.PostAsync(string.Empty, requestData, headers);
                    dptoken = JsonConvert.DeserializeObject<DPTokenResponse>(token);
                    var expiry = TimeSpan.FromSeconds(dptoken.ExpiresIn - configuration.GetSection("dpTokenConfig").GetValue<double>("tokenExpInSec"));
                    var docSaved = await _cachingService.SaveCache<DPTokenResponse>(key, dptoken, _headers.ContextValues?.TransactionId, expiry);
                }
                if (dptoken != null)
                {
                    dpTokenResponse = $"{dptoken.TokenType} {dptoken.AccessToken}";
                }
                _logger.LogInformation("Dp token {@token}", dptoken);
                return dpTokenResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError("Dp token errors {@error}", ex);
                return null;
            }
        }

        public DPTokenRequest GetDPRequest(int applicationId, IConfiguration configuration, string configSectionKey = "dpTokenRequest")
        {
            DPTokenRequest dpRequest = null;

            switch (applicationId)
            {
                case 1: //1 - For IOS
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = configuration.GetSection(configSectionKey).GetValue<string>("grantType"),
                        ClientId = configuration.GetSection(configSectionKey).GetSection("ios").GetValue<string>("clientId"),
                        ClientSecret = configuration.GetSection(configSectionKey).GetSection("ios").GetValue<string>("clientSecret"),
                        Scope = configuration.GetSection(configSectionKey).GetSection("ios").GetValue<string>("clientScope"),
                        UserType = configuration.GetSection(configSectionKey).GetValue<string>("userType"),
                    };
                    break;

                case 2: // 2 - For Android
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = configuration.GetSection(configSectionKey).GetValue<string>("grantType"),
                        ClientId = configuration.GetSection(configSectionKey).GetSection("android").GetValue<string>("clientId"),
                        ClientSecret = configuration.GetSection(configSectionKey).GetSection("android").GetValue<string>("clientSecret"),
                        Scope = configuration.GetSection(configSectionKey).GetSection("android").GetValue<string>("clientScope"),
                        UserType = configuration.GetSection(configSectionKey).GetValue<string>("userType"),
                    };
                    break;

                default:
                    break;
            }
            return dpRequest;
        }


    }
}
