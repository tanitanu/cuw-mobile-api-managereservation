using Autofac.Features.AttributeFilters;
using Newtonsoft.Json;
using System.Net;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.SeatPreference
{
    public class CustomerPreferenceService : ICustomerPreferenceService
    {
        private readonly ICacheLog<CustomerPreferenceService> _logger;
        private readonly IResilientClient _resilientClient;

        public CustomerPreferenceService([KeyFilter("AwsCustomerPreferenceClientKey")] IResilientClient resilientClient
            , ICacheLog<CustomerPreferenceService> logger)
        {
            _logger = logger;
            _resilientClient = resilientClient;
        }

        public async Task<string> GetAsync(string token, string recordLocator, string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" },
                          { "Authorization", token }
                     };

            var urlPath = $"/recordLocator/{recordLocator}";
            _logger.LogInformation("Get customer-preferences {@urlPath}", urlPath);

            try
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(urlPath, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Get customer-preferences {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("Get customer-preferences {@RequestUrl} and {@Response}", responseData.url, responseData.response);

                return responseData.response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Get customer-preferences error {Exception}", JsonConvert.SerializeObject(ex));
            }

            return string.Empty;
        }

        public async Task<string> SaveAsync(string token, string cslRequestDeserializeObject, string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept","application/json" },
                          { "Authorization", token }
                     };

            try
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, cslRequestDeserializeObject, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Save customer-preferences {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("Save customer-preferences {@RequestUrl} and {@Response}", responseData.url, responseData.response);

                return responseData.response;
            }
            catch (Exception ex)
            {
                _logger.LogError("Save customer-preferences error {Exception}", JsonConvert.SerializeObject(ex));
            }

            return string.Empty;
        }
    }
}
