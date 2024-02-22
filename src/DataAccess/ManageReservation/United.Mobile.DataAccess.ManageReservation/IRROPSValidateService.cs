using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class IRROPSValidateService : IIRROPValidateService
    {
        private readonly ICacheLog<IRROPSValidateService> _logger;
        private readonly IResilientClient _resilientClient;

        public IRROPSValidateService(ICacheLog<IRROPSValidateService> logger, [KeyFilter("IRROPSValidateClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetIRROPSStatus(string token, string request, string sessionId, string path, string eServiceAuthorization)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                {"eServiceAuthorization", eServiceAuthorization }
                     };

            _logger.LogInformation("CSL-GetIRROPSStatus-service {request}", request);

            var response = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

            if (response.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL GetIRROPSStatus-service {requestUrl} error {response} for {sessionId}", response.url, response.response, sessionId);
                if (response.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response.response);
            }

            _logger.LogInformation("CSL GetIRROPSStatus-service {requestUrl}, {response} and {sessionId}", response.url, response.response, sessionId);

            return response.response;
        }
    }
}
