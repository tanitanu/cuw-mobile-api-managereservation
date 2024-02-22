using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public class LoyaltyPromotionsService : ILoyaltyPromotionsService
    {
        private readonly IResilientClient _statusLiftBannerResilientClient;
        private readonly ICacheLog<LoyaltyPromotionsService> _logger;
        public LoyaltyPromotionsService([KeyFilter("LoyaltyPromotionsClientKey")] IResilientClient statusLiftBannerResilientClient, ICacheLog<LoyaltyPromotionsService> logger)
        {
            _statusLiftBannerResilientClient = statusLiftBannerResilientClient;
            _logger = logger;
        }

        public async Task<string> GetStatusLiftBanner(string token, string path, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetStatusLiftBanner service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var serviceResponse = await _statusLiftBannerResilientClient.GetHttpAsyncWithOptions(path, headers);

                if (serviceResponse.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Accountmanagnement-GetStatusLiftBanner {requestUrl} error {@response} for {sessionId}", serviceResponse.url, serviceResponse.response, sessionId);
                    if (serviceResponse.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(serviceResponse.response);
                }
                
                _logger.LogInformation("Accountmanagnement-GetStatusLiftBanner-service {requestUrl} and {sessionId}", serviceResponse.url, sessionId);
                return serviceResponse.response;
            }
        }
    }
}
