using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Mobile.Model.Common.DPToken;
using United.Utility.Http;
using ExpirationOptions = United.Mobile.Model.Common.ExpirationOptions;
using DPTokenResponse = United.Mobile.Model.Internal.DPTokenResponse;
using DPTokenRequest = United.Mobile.Model.Internal.DPTokenRequest;

namespace United.Mobile.DataAccess.Common
{
    public class DPTokenService : IDPTokenService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ILogger<DPTokenService> _logger;
        private readonly ICachingService _cacheService;
        private readonly IConfiguration _config;
        private readonly bool enableDPTokenAgentId;
        private readonly string awsDPTokenEndPoint;

        public DPTokenService([KeyFilter("dpTokenServiceConfigKey")] IResilientClient resilientClient, 
            ILogger<DPTokenService> logger,
            ICachingService cacheService, 
            IConfiguration config)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _cacheService = cacheService;
            _config = config;
            enableDPTokenAgentId = _config.GetValue<bool>("EnableDPTokenAgentId");
            awsDPTokenEndPoint = _config.GetSection("dpTokenConfig").GetValue<string>("baseUrl");
        }

        public async Task<string> GetTokenFromAWSDPAsync(string loggingContext, int applicationId, string deviceId, bool checkCache = true)
        {
            string token = string.Empty;
           
            try
            {
                string tokenPredictableKey = $"ANONYMOUSTOKEN::{deviceId}::{applicationId}";

                TokenData tokenData = null;

                if (checkCache)
                {
                    var dpToken = await GetDPTokenFromCacheAsync(loggingContext, tokenPredictableKey);
                    tokenData = JsonConvert.DeserializeObject<TokenData>(dpToken);
                }

                if (tokenData == null || string.IsNullOrEmpty(tokenData.Token) || tokenData.TokenExpiration == null
                    || DateTime.UtcNow > tokenData.TokenExpirationDateTimeUtc)
                {
                    DPTokenRequest dPTokenRequest = GetAWSDPRequestObject(applicationId, deviceId);
                    string requestData = JsonConvert.SerializeObject(dPTokenRequest);

                    _logger.LogInformation(" To Get Request token From AWS - requestData:-{@requestData}", requestData);

                    var httpResponse = string.Empty;

                    using (_logger.BeginTimedOperation("Timed operation for GetTokenFromAWSDPAsync", transationId: loggingContext))
                    {
                        httpResponse = await _resilientClient.PostAsync(string.Empty, requestData);
                    }

                    if (!string.IsNullOrEmpty(httpResponse))
                    {
                        DPTokenResponse dpTokenResponse = JsonConvert.DeserializeObject<DPTokenResponse>(httpResponse);

                        tokenData = GetTokenDataFromDPToken(dpTokenResponse);
                        token = tokenData.Token;

                        await SaveDPTokenToCacheAsync(loggingContext, tokenPredictableKey, tokenData);
                    }
                }
                else
                {
                    token = tokenData.Token;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured at DPTokenService- GetTokenFromAWSDPAsync");
            }

            return token;
        }

        private DPTokenRequest GetAWSDPRequestObject(int applicationID, string deviceId = "")
        {
            DPTokenRequest dpRequest = null;

            string agentId = enableDPTokenAgentId ? deviceId : null;

            switch (applicationID)
            {
                case 1:
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = "client_credentials",
                        ClientId = _config.GetSection("dpTokenRequest").GetSection("ios").GetValue<string>("clientId"),
                        ClientSecret = _config.GetSection("dpTokenRequest").GetSection("ios").GetValue<string>("clientSecret"),
                        Scope = _config.GetSection("dpTokenRequest").GetSection("ios").GetValue<string>("clientScope"),
                        UserType = "guest",
                        endUserAgentID = agentId,
                        endUserAgentIP = string.Empty
                    };

                    break;

                case 2:
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = "client_credentials",
                        ClientId = _config.GetSection("dpTokenRequest").GetSection("android").GetValue<string>("clientId"),
                        ClientSecret = _config.GetSection("dpTokenRequest").GetSection("android").GetValue<string>("clientSecret"),
                        Scope = _config.GetSection("dpTokenRequest").GetSection("android").GetValue<string>("clientScope"),
                        UserType = "guest",
                        endUserAgentID = agentId,
                        endUserAgentIP = string.Empty
                    };

                    break;

                case 3:
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = "client_credentials",
                        ClientId = _config["Windows_ClientId_DP_AWS"],
                        ClientSecret = _config["Windows_ClientSecret_DP_AWS"],
                        Scope = _config["Windows_Scope_DP_AWS"],
                        UserType = "guest",
                        endUserAgentID = agentId,
                        endUserAgentIP = string.Empty
                    };

                    break;

                case 6:
                    dpRequest = new DPTokenRequest
                    {
                        GrantType = "client_credentials",
                        ClientId = _config["Mobile_ClientId_DP_AWS"],
                        ClientSecret = _config["Mobile_ClientSecret_DP_AWS"],
                        Scope = _config["Mobile_Scope_DP_AWS"],
                        UserType = "guest",
                        endUserAgentID = agentId,
                        endUserAgentIP = string.Empty
                    };

                    break;

                default:
                    break;
            }

            return dpRequest;
        }

        private async Task<string> GetDPTokenFromCacheAsync(string loggingContext, string cacheKey)
        {
            string tokenData = default;

            using (_logger.BeginTimedOperation("Timed operation for GetDPTokenFromCacheAsync", transationId: loggingContext))
            {
                try
                {
                    tokenData = await _cacheService.GetCache<TokenData>(cacheKey, loggingContext);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Response status code does not indicate success: 404 (Not Found)."))
                    {
                        _logger.LogWarning("Cache document is not found - GetDPTokenFromCacheAsync {key}", cacheKey);
                    }
                    else
                    _logger.LogError(ex, "Exception on retrieving cache document - GetDPTokenFromCacheAsync {key}", cacheKey);
                }
            }
            
            return tokenData;
        }

        private TokenData GetTokenDataFromDPToken(DPTokenResponse dpTokenResponse)
        {
            TokenData tokenData = default;

            if (dpTokenResponse != null && !string.IsNullOrEmpty(dpTokenResponse.AccessToken) &&
                !string.IsNullOrEmpty(dpTokenResponse.TokenType))
            {
                tokenData = new TokenData
                {
                    Token = $"{dpTokenResponse.TokenType} {dpTokenResponse.AccessToken}",
                    TokenExpiration = TimeSpan.FromSeconds(dpTokenResponse.ExpiresIn),
                    TokenExpirationDateTimeUtc = DateTime.UtcNow.AddSeconds(dpTokenResponse.ExpiresIn - 900)
                };
            }

            return tokenData;
        }

        private async Task SaveDPTokenToCacheAsync(string loggingContext, string cacheKey, TokenData tokenData)
        {
            using (_logger.BeginTimedOperation("Timed operation for SaveDPTokenToCacheAsync", transationId: loggingContext))
            {
                try
                {
                    if (tokenData != null && !string.IsNullOrEmpty(tokenData.Token) && tokenData.TokenExpirationDateTimeUtc != null)
                    {
                        ExpirationOptions expirationOptions = new ExpirationOptions
                        {
                            AbsoluteExpiration = tokenData.TokenExpirationDateTimeUtc
                        };

                        await _cacheService.SaveCache( cacheKey, tokenData, loggingContext, expirationOptions.AbsoluteExpiration.Value.TimeOfDay);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Exception - SaveDPTokenToCacheAsync, {cacheKey}/{ex}", JsonConvert.SerializeObject(cacheKey));
                }
            }                
        }
    }
}
