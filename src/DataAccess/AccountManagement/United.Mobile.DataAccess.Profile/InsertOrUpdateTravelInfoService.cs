using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.CSLModels;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Helper;
using United.Utility.Http;


namespace United.Mobile.DataAccess.Profile
{
    public class InsertOrUpdateTravelInfoService : IInsertOrUpdateTravelInfoService
    {
        private readonly ICacheLog<InsertOrUpdateTravelInfoService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;

        public InsertOrUpdateTravelInfoService(
            ICacheLog<InsertOrUpdateTravelInfoService> logger,
            IConfiguration configuration,
            [KeyFilter("CustomerProfileContactpointsKey")] IResilientClient resilientClient)
        {
            _logger = logger;
            _resilientClient = resilientClient;
            _configuration = configuration;
        }

        public async Task<string> InsertOrUpdateTravelerInfo(int customerId, string jsonRequest, string token, bool isAddress = false)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            var path = string.Format("{0}/v1", customerId);
            _logger.LogInformation("Insert or Update Address Service-  {insertOrUpdateAddress}", jsonRequest);

            using (_logger.BeginTimedOperation("Total time taken for insertOrUpdateAddress service call", transationId: string.Empty))
            {
                var responseData = await _resilientClient.PutAsync(path, jsonRequest, headers);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("Insert or Update Address Service {requestUrl} error {@response} for {sessionId}", responseData.url, responseData.response, string.Empty);
                    if (responseData.statusCode == HttpStatusCode.BadRequest)
                    {
                        var cslErrorResponseData = Newtonsoft.Json.JsonConvert.DeserializeObject<United.Mobile.Model.CSLModels.CslResponse<UpdateContactResponse>>(responseData.response);
                        if (isAddress)
                        {
                            if (cslErrorResponseData.Errors != null && cslErrorResponseData.Errors.Count() > 0)
                            {
                                var errorMessage = cslErrorResponseData.Errors.FirstOrDefault(error => error.Code == "400.82" || error.Code == "400.6");
                                if (errorMessage != null)
                                {
                                    string[] errorMessages = errorMessage.Description.Split('|');
                                    if (errorMessages.Count() == 1)
                                        throw new MOBUnitedException(errorMessages[0]);
                                    else
                                        throw new MOBUnitedException(errorMessages[2]);
                                }
                                else
                                {
                                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                                }
                            }
                            else
                            {
                                throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                            }
                        }
                        else
                        {
                            if (cslErrorResponseData.Errors != null && cslErrorResponseData.Errors.Count() > 0)
                            {

                                if (cslErrorResponseData.Errors.Any(error => error.Code == "400.94" || error.Code == "400.10"))
                                {
                                    throw new MOBUnitedException(_configuration.GetValue<string>("GenericInValidPhoneNumberMessage"));
                                }
                                else
                                {
                                    throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));

                                }
                            }
                        }

                    }
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(_configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }

                _logger.LogInformation("Insert or Update Address Service {requestUrl} , {@response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }


    }
}
