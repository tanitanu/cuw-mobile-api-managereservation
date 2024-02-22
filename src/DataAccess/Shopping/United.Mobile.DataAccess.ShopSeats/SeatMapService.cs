using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ShopSeats
{
    public class SeatMapService: ISeatMapService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<SeatMapService> _logger;
        private readonly IConfiguration _configuration;

        public SeatMapService(
            [KeyFilter("SeatMapClientKey")] IResilientClient resilientClient
            , ICacheLog<SeatMapService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<T> SeatEngine<T>(string token, string action, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},//"application/xml;"
                          { "Authorization", token }
                     };
            string requestData = string.Format("/{0}", action);
            _logger.LogInformation("CSL service-SeatEngine {@RequestData}, {@Request}", requestData, request);
            using (_logger.BeginTimedOperation("Total time taken for SeatEngine business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(requestData, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-SeatEngine {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                
                _logger.LogInformation("CSL service-SeatEngine {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
