using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Profile
{
    public class EServiceCheckin : IEServiceCheckin
    {

        private readonly ICacheLog<EServiceCheckin> _logger;
        private readonly IResilientClient _resilientClient;

        public EServiceCheckin(ICacheLog<EServiceCheckin> logger, [KeyFilter("EServiceCheckinClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> GetPhoneValidation<T>(string token, string path,string requestData, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetPhoneValidation service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var pnrData = await _resilientClient.PostHttpAsyncWithOptions(path,requestData ,headers);

                if (pnrData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("EServiceCheckin - GetPhoneValidation  {requestUrl} error {response} for {sessionId}", pnrData.url, pnrData.statusCode, sessionId);
                    if (pnrData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(pnrData.response);
                }

                _logger.LogInformation("EServiceCheckin - GetPhoneValidation {requestUrl} and {sessionId}", pnrData.url, sessionId);

                return JsonConvert.DeserializeObject<T>(pnrData.response);
            }
        }
    }
}
