using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.HomePageContent;
using United.Utility.Helper;
using United.Utility.Http;


namespace United.Mobile.DataAccess.MPAuthentication
{
    public class HomePageContentService : IHomePageContentService
    {
        private readonly IResilientClient _homePageContentResilientClient;
        private readonly ICacheLog<HomePageContentService> _logger;
        public HomePageContentService([KeyFilter("homePageContentClientKey")] IResilientClient homePageContentResilientClient, ICacheLog<HomePageContentService> logger)
        {
            _homePageContentResilientClient = homePageContentResilientClient;
            _logger = logger;
        }
        public async Task<string> GetHomePageContents(string token, string requestData, string sessionId)
        {

            using (_logger.BeginTimedOperation("Total time taken for GetHomePageContents service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
                var serviceResponse = await _homePageContentResilientClient.PostHttpAsyncWithOptions(string.Empty, requestData, headers);

                if (serviceResponse.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("AccountManagement-HomePageConents-service {requestUrl} error {@response} for {sessionId}", serviceResponse.url, JsonConvert.DeserializeObject<HomePageContentResponse>(serviceResponse.response), sessionId);
                    if (serviceResponse.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(serviceResponse.response);
                }
                
                _logger.LogInformation("AccountManagement-HomePageConents-service {requestUrl} and {sessionId}", serviceResponse.url, sessionId);
                return serviceResponse.response;
            }
        }
    }
}
