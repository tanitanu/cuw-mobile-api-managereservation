using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.Booking;
using United.Mobile.Model.Internal.PassRiders;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.PassRiders
{
    public class TravelDocumentService : ITravelDocumentService
    {
        private readonly IResilientClient _travelDocumentResilientClient;
        private readonly ICacheLog<TravelDocumentService> _logger;
        public TravelDocumentService(ICacheLog<TravelDocumentService> logger, [KeyFilter("travelDocumentClientKey")] IResilientClient travelDocumentResilientClient)
        {
            _logger = logger;
            _travelDocumentResilientClient = travelDocumentResilientClient;
        }
        public async Task<string> SaveTravelDocument(string token, string path, string requestData, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var travelData = await _travelDocumentResilientClient.PostHttpAsync(path, requestData, headers);
            var response = travelData.Item1;
            var statusCode = travelData.Item2;
            var url = travelData.Item3;

            _logger.LogInformation("eRes-SaveTravelDocument-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-SaveTravelDocument-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<SaveTravelDocumentResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }

        public async Task<string> GetDefaultTravelDocuments(string token, string path, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };
            var documentData = await _travelDocumentResilientClient.GetHttpAsync(string.Empty, headers).ConfigureAwait(false);
            var response = documentData.Item1;
            var statusCode = documentData.Item2;
            var url = documentData.Item3;

            _logger.LogInformation("eRes-GetDefaultTravelDocuments-service {requestUrl} and {sessionId}", url, sessionId);

            if (statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("eRes-GetDefaultTravelDocuments-service {requestUrl} error {@response} for {sessionId}", url, JsonConvert.DeserializeObject<DefaultTravelDocumentResponse>(response), sessionId);
                if (statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(response);
            }
            return response;
        }
    }

}
