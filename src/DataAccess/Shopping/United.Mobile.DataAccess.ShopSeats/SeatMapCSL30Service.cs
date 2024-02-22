using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ShopSeats
{
    public class SeatMapCSL30Service : ISeatMapCSL30Service
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<SeatMapCSL30Service> _logger;
        private readonly IConfiguration _configuration;

        public SeatMapCSL30Service(
            [KeyFilter("SeatMapCSL30ClientKey")] IResilientClient resilientClient
            , ICacheLog<SeatMapCSL30Service> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> GetSeatMapDeatils(string token, string sessionId, string request, string channelId, string channelName, string path)
        {
            _logger.LogInformation("GetSeatMapDeatils-CSL call parameters {@Request},{@Url}", request, path);

            string[] channelInfo = _configuration.GetValue<string>("CSL30MBEChannelInfo").Split('|');
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token },
                           {"ChannelId", channelId },
                           {"ChannelName", channelName }
                     };

            var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetSeatMapDeatils {RequestUrl} error {Response}", responseData.url, responseData.response);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }

            _logger.LogInformation("CSL service-GetSeatMapDeatils {RequestUrl} {Response}", responseData.url, responseData.response);
            return responseData.response;
        }
    }
}
