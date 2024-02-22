using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UAWSFlightReservation;
using United.Mobile.Model.Common;
using United.Service.Presentation.CommonModel;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Fitbit
{
    public class FlightReservationService : IFlightReservationService
    {
        private readonly ICacheLog<FlightReservationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IResilientClient _resilientClient;
        public FlightReservationService(ICacheLog<FlightReservationService> logger, IConfiguration configuration, [KeyFilter("CSLPNRManagementAddRemarksKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _configuration = configuration;
            _resilientClient = resilientClient;
        }
        public async Task<wsFlightResResponse> GetFlightReservation(string recordLoactor, string lastName, string accessCode, string version)
        {
            string flightReservationURL = _configuration.GetValue<string>("FlightReservationSoap");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(flightReservationURL));
            var channelFactory = new ChannelFactory<FlightReservationSoap>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetFlightReservationAsync(recordLoactor, lastName, accessCode, version);
            channelFactory.Close();
            return response;
        }

        public async Task<string> AddPNRRemark(string request, string token, string suffixUrl)
        {
            using (_logger.BeginTimedOperation("CSL PNR Management AddRemarks service call "))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(suffixUrl, request, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL PNR Management AddRemarks is {requestUrl} error {response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL PNR Management AddRemarks is {requestUrl} ", responseData.url);
                return responseData.response;
            }           
        }

    }
}
