using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.InFlightAmenity
{
    public class AircraftAmenitiesService : IAircraftAmenitiesService
    {
        private readonly ICacheLog<AircraftAmenitiesService> _logger;
        private readonly IResilientClient _resilientClient;

        public AircraftAmenitiesService([KeyFilter("AircraftAmenitiesClientKey")] IResilientClient resilientClient
            , ICacheLog<AircraftAmenitiesService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetAircraftAmenities(string token, string sessionId, string flightNumber, string legDepartureDate, string legDepartureStation, string legArrivalStation, string shipNumber, string equipmentCode)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/xml"},
                          { "Authorization", token }
                     };


            using (_logger.BeginTimedOperation("Total time taken for GetAircraftAmenities service call", transationId: sessionId))
            {
                string path = "/getAircraftAmenities/?flightNumber=" + flightNumber + "&legDepartureDate=" + legDepartureDate + "&legDepartureStation=" + legDepartureStation + "&legArrivalStation=" + legArrivalStation + "&shipNumber=" + shipNumber + "&equipmentCode=" + equipmentCode;

                _logger.LogInformation("GetAircraftAmenities CSL Service {path} and {sessionId}", path, sessionId);
                
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("GetAircraftAmenities CSL Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("GetAircraftAmenities CSL Service {requestUrl} , {@response} for {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

    }
}
