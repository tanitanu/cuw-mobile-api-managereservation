using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.CancelReservation;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.CancelReservation
{
    public class CancelReservationService : ICancelReservationService
    {
        private readonly ICacheLog<CancelReservationService> _logger;
        private readonly IResilientClient _cancelReservationClient;

        public CancelReservationService(ICacheLog<CancelReservationService> logger, [KeyFilter("cancelReservationClientKey")]IResilientClient cancelReservationClient)
        {
            _logger = logger;
            _cancelReservationClient = cancelReservationClient;
        }

        public async Task<string> CancelReservation(string token, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var cancelData = await _cancelReservationClient.PostHttpAsync(string.Empty, requestData, headers);
            var response = cancelData.Item1;
            var statusCode = cancelData.Item2;
            var url = cancelData.Item3;
           
            _logger.LogInformation("eRes-CancelReservation-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                
                _logger.LogError("eRes-CancelReservation-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<EResCancelReservationResponse>(response), sessionId);

                if (statusCode != HttpStatusCode.BadRequest)
                {
                    if (statusCode == HttpStatusCode.InternalServerError)
                        return response;
                    else
                        throw new Exception(response);
                }
            }
            return response;
        }
    }
}
