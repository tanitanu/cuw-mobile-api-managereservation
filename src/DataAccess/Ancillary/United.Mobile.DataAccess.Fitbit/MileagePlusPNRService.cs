using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Fitbit
{
    public class MileagePlusPNRService : IMileagePlusPNRService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<MileagePlusPNRService> _logger;
        public MileagePlusPNRService([KeyFilter("MileagePlusPNRClientKey")] IResilientClient resilientClient
           , ICacheLog<MileagePlusPNRService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }
        public async Task<string> GetPNRStatus(string token, string accesscode, string transactionid, string PNRList, string ApplicationType, string messageformat, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("/getPNRStatusPSS?accesscode=ACCESSCODE&transactionid={1}&PNRList={2}&ApplicationType={3}&messageformat=XML&Language=en-US&SharesOption=&UID={4}", accesscode, transactionid, PNRList, ApplicationType, messageformat);
            var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
            _logger.LogInformation("CSL service-GetPNRStatus {requestUrl} and {sessionId}", responseData.url, sessionId);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetPNRStatus {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            return (responseData.response);
        }
        public async Task<string> GetBoardingPass(string token, string sessionId, string accessCode, string transactionId, string recordLocator, string applicationName, string applicationVersion)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetBoardingPass service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/MultipleEBPShortCutPNRsWallet?accesscode={0}&transactionid={1}&PNRList={2}&SharesOption=&applicationType={3}&MessageFormat={4}&UID={5}",
                        accessCode,
                        transactionId,
                        recordLocator,
                        applicationName, "JSON", applicationVersion);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetBoardingPass {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetBoardingPass {requestUrl} and {sessionId}", responseData.url, sessionId);
                return responseData.response;
            }
        }
        public async Task<string> GetCountryName(string token, string accesscode, string transactionid, string countryCode, string sessionId)
        {

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string requestData = string.Format("{0}/GetCountryName?accesscode={1}&transactionid={2}&CountryCode={3}&MessageFormat=json", accesscode, transactionid, countryCode);
            var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
            _logger.LogInformation("CSL service-GetCountryName {requestUrl} and {sessionId}", responseData.url, sessionId);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetCountryName {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            return (responseData.response);
        }
    }
}
