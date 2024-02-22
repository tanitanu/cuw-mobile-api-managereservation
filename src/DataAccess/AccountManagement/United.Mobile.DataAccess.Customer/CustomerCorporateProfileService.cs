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

namespace United.Mobile.DataAccess.Customer
{
    public class CustomerCorporateProfileService : ICustomerCorporateProfileService
    {
        private readonly ICacheLog<CustomerCorporateProfileService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerCorporateProfileService(
              [KeyFilter("CSLCorporateGetServiceKey")] IResilientClient resilientClient
            , ICacheLog<CustomerCorporateProfileService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }
        public async Task<T> GetCorporateprofile<T>(string token, string request, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for Get Corporate profile service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
                string path = "/CorpProfile";
                _logger.LogInformation("CSL Call GetCorporateprofile {@RequestUrl} {@Request}", path, request);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Call GetCorporateprofile {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("CSL Call GetCorporateprofile {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
        public async Task<T> GetCorporateCreditCards<T>(string token, string request, string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for Corporate Creditcard  service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token },
                          {"Accept","application/json" }
                     };
                string path = "/CorpFOP";
                _logger.LogInformation("CSLRequest Corporate Creditcard {Request} and {sessionId}", request, sessionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL Corporate Creditcard  Request url for get profile {CSlRequest} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("CSL Call Response-Corporate Creditcard {CSlRequest} {CSLResponse} and {sessionId}", responseData.url, JsonConvert.SerializeObject(responseData), sessionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }
    }
}
