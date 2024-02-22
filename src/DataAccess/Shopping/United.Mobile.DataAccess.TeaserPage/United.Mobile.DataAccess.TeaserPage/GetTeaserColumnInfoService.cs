using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.TeaserPage
{
    public class GetTeaserColumnInfoService: IGetTeaserColumnInfoService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<GetTeaserColumnInfoService> _logger;
        private readonly IConfiguration _configuration;


        public GetTeaserColumnInfoService([KeyFilter("GetTeaserColumnInfoClientKey")] IResilientClient resilientClient, ICacheLog<GetTeaserColumnInfoService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<T> GetTeaserText<T>(string token, string cartID, string langCode, string countryCode, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetTeaserText service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
                string requestData = string.Format("?cartId={0}&langCode={1}&countryCode={2}", cartID, langCode, countryCode);

                _logger.LogInformation("CSL Service GetTeaserText {@Request}", requestData);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Service GetTeaserText {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("CSL Service GetTeaserText {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);

            }
        }

    }
}
