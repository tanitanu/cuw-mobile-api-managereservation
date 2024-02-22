using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
namespace United.Mobile.DataAccess.FlightStatus
{
    public class FlightStatusService : IFlightStatusService
    {
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<FlightStatusService> _logger;

        public FlightStatusService([KeyFilter("FlightStatusClientKey")] IResilientClient resilientClient, ICacheLog<FlightStatusService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string> GetFlightStatusDetails(string token, string transactionId, string flightNumber, string flightDate, string origin)
        {
            _logger.LogInformation("CSL service-GetFlightStatusDetails {flightNumber},{flightDate},{origin} and {transactionId}", flightNumber, flightDate, origin, transactionId);

            using (_logger.BeginTimedOperation("Total time taken for GetFlightStatusDetails service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

                string requestData = string.Format("/{0}?FlightLegDepDate={1}&fetchInboundLeg=True&origin={2}", flightNumber, flightDate, origin);

                if (string.IsNullOrEmpty(origin))
                {
                    requestData = string.Format("/{0}?FlightOrigDate={1}&fetchInboundLeg=True", flightNumber, flightDate);
                }

                _logger.LogInformation("CSL service-GetFlightStatusDetails {requestPayLoad} and {transactionId}", requestData, transactionId);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFlightStatusDetails {requestUrl} error {response} for {transactionId}", responseData.url, responseData.response, transactionId);
                    throw new WebException(responseData.response);
                }

                _logger.LogInformation("CSL service-GetFlightStatusDetails {response} and {transactionId}", responseData.response, transactionId);

                return responseData.response;
            }
        }
        public async Task<string> GetFlightStatusDetails(string token, string transactionId, string flightNumber, string flightDate, string origin, string carrierCode)
        {
            if (!string.IsNullOrEmpty(flightNumber))
                flightNumber = flightNumber.Replace("\r", "").Replace("\n", "").Trim();
            else
                flightNumber = string.Empty;
            if (!string.IsNullOrEmpty(flightDate))
                flightDate = flightDate.Replace("\r", "").Replace("\n", "").Trim();
            else
                flightDate = string.Empty;
            if (!string.IsNullOrEmpty(origin))
                origin = origin.Replace("\r", "").Replace("\n", "").Trim();
            else
                origin = string.Empty;
            if (!string.IsNullOrEmpty(carrierCode))
                carrierCode = carrierCode.Replace("\r", "").Replace("\n", "").Trim();
            else
                carrierCode = string.Empty;
            if (!string.IsNullOrEmpty(transactionId))
                transactionId = transactionId.Replace("\r", "").Replace("\n", "").Trim();
            else
                transactionId = string.Empty;
            _logger.LogInformation("CSL service-GetFlightStatusDetails {flightNumber},{flightDate},{origin},{carrierCode} and {transactionId}", flightNumber, flightDate, origin, carrierCode, transactionId);
           
            using (_logger.BeginTimedOperation("Total time taken for GetFlightStatusDetails service call", transationId: transactionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string bufferVal = _configuration.GetValue<bool>("SetBuffer") == true ? "&buffer=true" : "";
                string requestData = string.Format("/{0}?FlightLegDepDate={1}&fetchInboundLeg=True&carrier={2}&origin={3}" + bufferVal, flightNumber, flightDate, carrierCode, origin);

                requestData = requestData.Replace("\r", "").Replace("\n", "").Trim();
                _logger.LogInformation("CSL service-GetFlightStatusDetails 01 {requestPayLoad} and {transactionId}", requestData, transactionId);
                
                if (string.IsNullOrEmpty(origin))
                {
                    requestData = string.Format("/{0}?FlightOrigDate={1}&fetchInboundLeg=True&carrier={2}" + bufferVal, flightNumber, flightDate, carrierCode);
                }
                
                if (_configuration.GetValue<bool>("EnableFlightStatusTerminalSpecificMessaging"))
                    requestData = $"{requestData}&SpclTrmnlMessages=true";

                requestData = requestData.Replace("\r", "").Replace("\n", "").Trim();
                _logger.LogInformation("CSL service-GetFlightStatusDetails 02 {requestPayLoad} and {transactionId}", requestData, transactionId);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFlightStatusDetails {requestUrl} error {response} for {transactionId}", responseData.url, responseData.response, transactionId);
                    throw new WebException(responseData.response);
                }

                _logger.LogInformation("CSL service-GetFlightStatusDetails {requestUrl} , {response} and {transactionId}", responseData.url, responseData.response, transactionId);

                return responseData.response;
            }
        }
    }
}
