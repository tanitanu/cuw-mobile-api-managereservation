using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.BagCalculator;
using United.Mobile.Model.Common;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightStatus
{
    public class AirlineCarrierService : IAirlineCarrierService
    {
        private readonly ICacheLog<AirlineCarrierService> _logger;
        private readonly IResilientClient _resilientClient;

        public AirlineCarrierService([KeyFilter("CarrierOnPremSqlClientKey")] IResilientClient resilientClient, ICacheLog<AirlineCarrierService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<T> GetCarriers<T>(string transactionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetCarriers OnPrem service call", transationId: transactionId))
            {
                string requestData = string.Format("/GetCarriers?transactionId={0}",transactionId);
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("BagCalculator - GetCarriers--OnPrem Service {requestUrl} error {response} for {transactionId} ", responseData.url, JsonConvert.SerializeObject(responseData.response), transactionId);
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("BagCalculator - GetCarriers-OnPrem Service {requestUrl} info {response} for {transactionId}", responseData.url, JsonConvert.SerializeObject(responseData.response), transactionId);

                var responseObject = JsonConvert.DeserializeObject<SessionResponse>(responseData.response);

                var responseObjectData = JsonConvert.DeserializeObject<List<CarrierInfo>>(responseObject?.Data);

                return responseObjectData;
            }
        }
    }
}
