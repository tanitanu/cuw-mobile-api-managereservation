using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassRiderList;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PassRiderList
{
    public class PassRiderListService : IPassRiderListService
    {
        private readonly IResilientClient _passRiderListResilientClient;
        private readonly ICacheLog<PassRiderListService> _logger;
        public PassRiderListService([KeyFilter("passRiderListClientKey")] IResilientClient passRiderListResilientClient, ICacheLog<PassRiderListService> logger)
        {
            _passRiderListResilientClient = passRiderListResilientClient;
            _logger = logger;
        }

        public async Task<string> GetPassRiderList(string token, string requestData, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var passRiderListData = await _passRiderListResilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = passRiderListData.Item1;
            var statusCode = passRiderListData.Item2;
            var url = passRiderListData.Item3;

            _logger.LogInformation("eRes-GetPassRiderList-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetPassRiderList-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<PassRiderListResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }
}
