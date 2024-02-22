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
    public class UpgradeEligibilityService : IUpgradeEligibilityService
    {
        private readonly ICacheLog<UpgradeEligibilityService> _logger;
        private readonly IResilientClient _resilientClient;

        public UpgradeEligibilityService(ICacheLog<UpgradeEligibilityService> logger, [KeyFilter("UpgradeEligibilityClientKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetUpgradeCabinEligibleCheck(string token, string request, string sessionId, string path)
        {
            _logger.LogInformation("CSL-GetUpgradeCabinEligibleCheck-service {@request} and {sessionId}", request, sessionId);

            using (_logger.BeginTimedOperation("Total time taken for GetUpgradeCabinEligibleCheck service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

                var response = await _resilientClient.PostHttpAsyncWithOptions("", request, headers);

                if (response.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL-UpgradeCabin-GetUpgradeCabinEligibleCheck-service {requestUrl} error {response} for {sessionId}", response.url, response.response, sessionId);
                    if (response.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(response.response);
                }

                _logger.LogInformation("CSL-UpgradeCabin-GetUpgradeCabinEligibleCheck-service {requestUrl}, {response} and {sessionId}", response.url, response.response, sessionId);

                return response.response;
            }
        }
    }
}
