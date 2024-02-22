using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ShopAward
{
    public class AwardCalendarAzureService : IAwardCalendarAzureService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<AwardCalendarAzureService> _logger;
        private readonly IConfiguration _configuration;

        public AwardCalendarAzureService(
              [KeyFilter("AwardCalendarClientKey")] IResilientClient resilientClient
            , ICacheLog<AwardCalendarAzureService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<T> AwardDynamicCalendar<T>(string token, string sessionId, string request)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string requestData = string.Format("/award/Calendar");
            _logger.LogInformation("CSL service-AwardDynamicCalendar {@RequestData} {@Request}", requestData, request);
            using (_logger.BeginTimedOperation("Total time taken for AwardDynamicCalendar business call", transationId: sessionId))
            {
                var response = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers);

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-AwardDynamicCalendar {@RequestUrl} error {@Response}", response.url, response.response);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-AwardDynamicCalendar {@RequestUrl}, {@Response}", response.url, response.response);
                return JsonConvert.DeserializeObject<T>(response.response);
            }
        }
    }
}
