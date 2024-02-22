using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Profile
{
    public class BaseEmployeeResService : IBaseEmployeeResService
    {

        private readonly ICacheLog<BaseEmployeeResService> _logger;
        private readonly IResilientClient _resilientClient;

        public BaseEmployeeResService(ICacheLog<BaseEmployeeResService> logger, [KeyFilter("BaseEmployeeResClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<T> UpdateTravelerBase<T>(string token, string request, string sessionId, string path)
        {
            using (_logger.BeginTimedOperation("Total time taken for UpdateTravelerBase service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},//"application/xml;"
                          { "Authorization", token }
                     };
                _logger.LogInformation("UpdateTravelerBase-service {@Request} and {@RequestPath}", request, path);
                var glbData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

                if (glbData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("UpdateTravelerBase-service {@RequestUrl} error {@Response}", glbData.url, glbData.response);
                    if (glbData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(glbData.response);
                }

                _logger.LogInformation("UpdateTravelerBase-service {@RequestUrl} and {@Response}", glbData.url, glbData.response);

                return JsonConvert.DeserializeObject<T>(glbData.response);
            }
        }
    }
}
