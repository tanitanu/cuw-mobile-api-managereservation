using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.Common
{
    public class CCEDynamicOfferDetailsService : ICCEDynamicOfferDetailsService
    {
        private readonly IResilientClient _resilientClientDOD;
        private readonly ICacheLog<CCEDynamicOffersService> _logger;
        public CCEDynamicOfferDetailsService(
             [KeyFilter("CCEDynamicOffersDetailClientKey")] IResilientClient resilientClientDOD
            , ICacheLog<CCEDynamicOffersService> logger
            )
        {
            _resilientClientDOD = resilientClientDOD;
            _logger = logger;
        }
       
        public async Task<string> GetCCEDynamicOffersDetail(string token, string request)
        {
            _logger.LogInformation("CSL service-GetCCEDynamicOffersDetail {token}, {request}", token, request);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            var responseData = await _resilientClientDOD.PostHttpAsyncWithOptions("", request, headers).ConfigureAwait(false);
            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetCCEDynamicOffersDetail {@RequestUrl} error {Response}", responseData.url, responseData.response);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception("Service did not return any reponse");
            }

            _logger.LogInformation("CSL service-GetCCEDynamicOffersDetail {@RequestUrl}, {Response}", responseData.url, responseData.response);
            return (responseData.response);

        }
    }
}
