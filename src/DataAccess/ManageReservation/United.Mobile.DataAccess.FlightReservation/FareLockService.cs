using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightReservation
{
    public class FareLockService: IFareLockService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<FareLockService> _logger;
        public FareLockService([KeyFilter("FareLockClientKey")] IResilientClient resilientClient
           , ICacheLog<FareLockService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> GetPNRManagement(string token, string mileagePlusNumber, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("/MileagePlus/{0}/0?IsCheckinEligible=false", mileagePlusNumber);
            var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
            _logger.LogInformation("CSL service-GetPNRManagement {requestUrl} and {sessionId}", responseData.url, sessionId);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetPNRManagement {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            return (responseData.response);
        }
    }
}
