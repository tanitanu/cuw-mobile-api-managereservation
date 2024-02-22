using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal;
using United.Mobile.Model.Internal.HomePageContent;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.HomePageContent
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

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var serviceResponse = await _homePageContentResilientClient.PostHttpAsync(string.Empty, requestData, headers);

            var response = serviceResponse.Item1;
            var statusCode = serviceResponse.Item2;
            var url = serviceResponse.Item3;

            _logger.LogInformation("eRes-HomePageConents-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-HomePageConents-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<HomePageContentResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;

        }
    }
}
