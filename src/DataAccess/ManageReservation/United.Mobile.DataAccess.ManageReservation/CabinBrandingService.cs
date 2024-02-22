using Autofac.Features.AttributeFilters;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class CabinBrandingService: ICabinBrandingService
    {
        private readonly ICacheLog<CabinBrandingService> _logger;
        private readonly IResilientClient _resilientClient;

        public CabinBrandingService(ICacheLog<CabinBrandingService> logger, [KeyFilter("CabinBrandingServiceClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> CabinBranding(string token, string request, string sessionId, string path)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            _logger.LogInformation("CSL-CabinBranding-service {request}", request);

            var response = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);

            if (response.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL CabinBranding-service {requestUrl} error {response} for {sessionId}", response.url, response.response, sessionId);
                if (response.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response.response);
            }

            _logger.LogInformation("CSL CabinBranding-service {requestUrl}, {response} and {sessionId}", response.url, response.response, sessionId);

            return response.response;
        }
    }
}
