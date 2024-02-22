using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.TravelerMisConnect;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PassRiderList
{
    public class TravelerMisConnectService : ITravelerMisConnectService
    {
        private readonly IResilientClient _passengerMisConnectResilientClient;
        private readonly ICacheLog<TravelerMisConnectService> _logger;
        public TravelerMisConnectService([KeyFilter("travelerMisConnectClientKey")] IResilientClient passRiderListResilientClient, ICacheLog<TravelerMisConnectService> logger)
        {
            _passengerMisConnectResilientClient = passRiderListResilientClient;
            _logger = logger;
        }

        public async Task<string> GetPassengerMisConnectDetails(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var passRiderListData = await _passengerMisConnectResilientClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = passRiderListData.Item1;
            var statusCode = passRiderListData.Item2;
            var url = passRiderListData.Item3;

            _logger.LogInformation("eRes-PassengerMisConnect-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-PassengerMisConnect-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<TravelerMisConnectResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }

        
    }
}
