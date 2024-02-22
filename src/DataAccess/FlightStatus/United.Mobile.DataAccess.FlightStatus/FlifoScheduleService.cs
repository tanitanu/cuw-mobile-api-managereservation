using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.FlightStatus
{
    public class FlifoScheduleService : IFlifoScheduleService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<IFlifoScheduleService> _logger;

        public FlifoScheduleService([KeyFilter("FlifoScheduleClientKey")] IResilientClient resilientClient, ICacheLog<FlifoScheduleService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }


        public async Task<string> FlifoScheduleResponse(string inventoryRequest, string transactionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetFlightStatusDetails service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                           {"Accept", "application/json"},
                     };
                _logger.LogInformation("CSL service-FlifoSchedule-Request {request} and {transactionId}", inventoryRequest, transactionId);

                var responseData = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, inventoryRequest, headers).ConfigureAwait(false);

                _logger.LogInformation("CSL service-FlifoSchedule-Request {requestUrl}", responseData.url);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-FlifoSchedule-Response {requestUrl} error {response} for {transactionId}", responseData.url, responseData, transactionId);

                    if (responseData.statusCode == HttpStatusCode.BadRequest)
                        throw new WebException("BadRequest", new Exception(responseData.response), WebExceptionStatus.ConnectFailure, new CustomWebResponse());
                    else
                        throw new WebException("Unknown", new Exception(responseData.response), WebExceptionStatus.UnknownError, new CustomWebResponse());
                }

                _logger.LogInformation("CSL service-FlifoSchedule-Response {response} and {transactionId}", responseData.response, transactionId);
                return responseData.response;
            }
        }
    }
}