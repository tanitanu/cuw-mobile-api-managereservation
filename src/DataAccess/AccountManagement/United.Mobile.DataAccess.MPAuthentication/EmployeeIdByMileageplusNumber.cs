using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.MPAuthentication
{
    public class EmployeeIdByMileageplusNumber:IEmployeeIdByMileageplusNumber
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<EmployeeIdByMileageplusNumber> _logger;

        public EmployeeIdByMileageplusNumber([KeyFilter("EmployeeIdByMileageplusNumberClientKey")] IResilientClient resilientClient, ICacheLog<EmployeeIdByMileageplusNumber> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> GetEmployeeIdy(string mileageplusNumber,string transactionId,string sessionId)
        {
            using (_logger.BeginTimedOperation("Total time taken for GetEmployeeIdy service call", sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          { "Authorization", string.Empty },
                          {"Accept","application/json" }
                     };
                string requestData = string.Format("?mileageplusid={0}", mileageplusNumber);
                _logger.LogInformation("LoyaltyWebserviceGetEmployeeIdURL service {@RequestUrl}", requestData);

                var responseData = await _resilientClient.GetHttpAsyncWithOptions(requestData, headers).ConfigureAwait(false);
                
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("LoyaltyWebserviceGetEmployeeIdURL service {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                
                _logger.LogInformation("LoyaltyWebserviceGetEmployeeIdURL service {@RequestUrl} {@Response}", responseData.url, responseData.response);
                return responseData.response;
            }
        }
    }
}
