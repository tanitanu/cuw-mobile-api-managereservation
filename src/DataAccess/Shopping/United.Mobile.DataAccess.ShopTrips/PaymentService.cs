using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ShopTrips
{
    public class PaymentService : IPaymentService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<PaymentService> _logger;
        private readonly IConfiguration _configuration;
        public PaymentService([KeyFilter("PaymentServiceClientKey")] IResilientClient resilientClient
            , ICacheLog<PaymentService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> GetEligibleFormOfPayments(string token, string path, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-GetEligibleFormOfPayments parameters Request:{@Request} Path:{@Path}", request, path);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (_logger.BeginTimedOperation("Total time taken for GetEligibleFormOfPayments business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetEligibleFormOfPayments {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetEligibleFormOfPayments {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return (responseData.response == null) ? default : responseData.response;
            }
        }

        public async Task<string> GetFFCByEmail(string token, string path, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-GetFFCByEmail  parameters Token:{token}, Request:{request} Path:{path} SessionId:{sessionId}", token, request, path, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (_logger.BeginTimedOperation("Total time taken for GetFFCByEmail business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFFCByEmail {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetFFCByEmail {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return (responseData.response == null) ? default : responseData.response;
            }
        }

        public async Task<string> GetFFCByPnr(string token, string path, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-GetFFCByPnr  parameters Token:{token}, Request:{request} Path:{path} SessionId:{sessionId}", token, request, path, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (_logger.BeginTimedOperation("Total time taken for GetFFCByPnr business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFFCByPnr {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetFFCByPnr {requestUrl}  {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return (responseData.response == null) ? default : responseData.response;
            }
        }

        public async Task<string> GetLookUpTravelCredit(string token, string path, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-GetLookUpTravelCredit  parameters Token:{token}, Request:{request} Path:{path} SessionId:{sessionId}", token, request, path, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            using (_logger.BeginTimedOperation("Total time taken for GetLookUpTravelCredit business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetLookUpTravelCredit {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetLookUpTravelCredit {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return (responseData.response == null) ? default : responseData.response;
            }
        }

    }
}
