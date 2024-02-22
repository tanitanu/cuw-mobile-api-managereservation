using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.ManageReservation
{
    public class PNRRetrievalService : IPNRRetrievalService
    {
        private readonly ICacheLog<PNRRetrievalService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;
        private readonly IFeatureSettings _featureSettings;

        public PNRRetrievalService([KeyFilter("PNRRetrievalClientKey")] IResilientClient resilientClient, ICacheLog<PNRRetrievalService> logger
            , IConfiguration configuration
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _resilientClient = resilientClient;
            _configuration = configuration;
            _featureSettings = featureSettings;
        }

        public async Task<string> PNRRetrieval(string token, string requestData, string sessionId, string path = "")
        {
            _logger.LogInformation("CSL-PNRRetrieval-service {request} and {sessionId}", requestData, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var pnrData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers);

            _logger.LogInformation("CSL-PNRRetrieval-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

            if (pnrData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning("CSL-PNRRetrieval-service {requestUrl} warning {@response} for {sessionId}", pnrData.url, pnrData.response, sessionId);

                if (pnrData.statusCode == HttpStatusCode.InternalServerError && pnrData.response != null)
                {
                    var cslerror = JsonConvert.DeserializeObject<CSLError>(pnrData.response);
                    string errorMessage = cslerror?.Message;

                    if (cslerror?.Errors != null && cslerror?.Errors.Length > 0)
                    {
                        foreach (var error in cslerror.Errors)
                        {
                            errorMessage = errorMessage + " " + error.MinorDescription;

                            if (_configuration.GetValue<bool>("BugFixToggleForExceptionAnalysis") && !string.IsNullOrEmpty(error.MinorCode) && (error.MinorCode.Trim().Equals("40030") || error.MinorCode.Trim().Equals("10028")))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("UnableToPerformInstantUpgradeErrorMessage"));
                            }
                        }
                        if (errorMessage.Contains("Unable to retrieve PNR"))
                        {
                            _logger.LogInformation("CSL-PNRRetrieval-service {requestUrl} and {sessionId} {exception}", pnrData.url, sessionId, "Unable to retrieve PNR");
                            throw new MOBUnitedException("The confirmation number entered is invalid.");
                        }
                        else
                        {
                            _logger.LogInformation("CSL-PNRRetrieval-service {requestUrl} and {sessionId} {exception}", pnrData.url, sessionId, errorMessage);
                            if (await _featureSettings.GetFeatureSettingValue("PNRRetrivalGenericMessageFix_MOBILE-35481") && cslerror.InnerException.ToUpper().Contains("UNABLE TO RETRIEVE PNR"))
                                throw new MOBUnitedException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
                        }
                        throw new MOBUnitedException(errorMessage);
                    }
                }

                if (pnrData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(pnrData.response);
            }

            _logger.LogInformation("CSL-PNRRetrieval-service {response} and {sessionId}", pnrData.response, sessionId);
            return pnrData.response;
        }

        public async Task<T> GetOfferedMealsForItinerary<T>(string token, string action, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            using (_logger.BeginTimedOperation("Total time taken for CSL service-GetOfferedMealsForItinerary business call", transationId: sessionId))
            {
                _logger.LogInformation("CSL service-GetOfferedMealsForItinerary {@Request} {@RequestUrl}", request, action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetOfferedMealsForItinerary {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                _logger.LogInformation("CSL service-GetOfferedMealsForItinerary {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<string> UpdateTravelerInfo(string token, string requestData, string path, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var pnrData = await _resilientClient.PostHttpAsyncWithOptions(string.Empty, requestData, headers);

            _logger.LogInformation("UpdateTravelerInfo-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

            if (pnrData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("UpdateTravelerInfo-service {requestUrl} error {@response} for {sessionId}", pnrData.url, pnrData.response, sessionId);
                if (pnrData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(pnrData.response);
            }
            return pnrData.response;
        }

        public async Task<string> RetrievePNRDetail(string token, string requestData, string sessionId, string path)
        {
            _logger.LogInformation("RetrievePNRDetail-service {request} and {sessionId}", requestData, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", token }
                     };

            var pnrData = await _resilientClient.PostHttpAsyncWithOptions(path, requestData, headers);

            _logger.LogInformation("RetrievePNRDetail-service {requestUrl} and {sessionId}", pnrData.url, sessionId);

            if (pnrData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogWarning("RetrievePNRDetail-service {requestUrl} warning {@response} for {sessionId}", pnrData.url, pnrData.response, sessionId);

                if (pnrData.statusCode == HttpStatusCode.InternalServerError && pnrData.response != null)
                {
                    var cslerror = JsonConvert.DeserializeObject<CSLError>(pnrData.response);
                    string errorMessage = cslerror?.Message;

                    if (cslerror?.Errors != null && cslerror?.Errors.Length > 0)
                    {
                        foreach (var error in cslerror.Errors)
                        {
                            errorMessage = errorMessage + " " + error.MinorDescription;

                            if (_configuration.GetValue<bool>("BugFixToggleForExceptionAnalysis") && !string.IsNullOrEmpty(error.MinorCode) && (error.MinorCode.Trim().Equals("40030") || error.MinorCode.Trim().Equals("10028")))
                            {
                                throw new MOBUnitedException(_configuration.GetValue<string>("UnableToPerformInstantUpgradeErrorMessage"));
                            }
                        }
                        if (errorMessage.Contains("Unable to retrieve PNR"))
                        {
                            _logger.LogInformation("RetrievePNRDetail-service {requestUrl} and {sessionId} {exception}", pnrData.url, sessionId, "Unable to retrieve PNR");
                            throw new MOBUnitedException("The confirmation number entered is invalid.");
                        }
                        else
                        {
                            _logger.LogInformation("RetrievePNRDetail-service {requestUrl} and {sessionId} {exception}", pnrData.url, sessionId, errorMessage);
                            if (await _featureSettings.GetFeatureSettingValue("PNRRetrivalGenericMessageFix_MOBILE-35481") && cslerror.InnerException.ToUpper().Contains("UNABLE TO RETRIEVE PNR"))
                                throw new MOBUnitedException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
                        }
                        throw new MOBUnitedException(errorMessage);
                    }
                }

                if (pnrData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(pnrData.response);
            }

            _logger.LogInformation("RetrievePNRDetail-service {response} and {sessionId}", pnrData.response, sessionId);
            return pnrData.response;
        }

        public async Task<string> RetrievePNRDetailCSL(string path, string token, string requestData)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            var pnrData = await _resilientClient.PutAsync(path, requestData, headers);

            _logger.LogInformation("RetrievePNRDetailCSL-service {requestUrl} and {request}", pnrData.url, requestData);

            if (pnrData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("RetrievePNRDetailCSL-service {requestUrl} error {@response}", pnrData.url, pnrData.response);
                if (pnrData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception(pnrData.response);
            }

            _logger.LogInformation("RetrievePNRDetailCSL-service {response}", pnrData.response);
            return pnrData.response;
        }

    }
}