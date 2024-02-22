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
    public class CCEDynamicOffersService : ICCEDynamicOffersService
    {
        private readonly IResilientClient _resilientClientDO;
        private readonly ICacheLog<CCEDynamicOffersService> _logger;
        public CCEDynamicOffersService(
             [KeyFilter("CCEDynamicOffersClientKey")] IResilientClient resilientClientDO
            , ICacheLog<CCEDynamicOffersService> logger
            )
        {
            _resilientClientDO = resilientClientDO;
            _logger = logger;
        }
        public async Task<string> GetDynamicOffers(string token, string request)
        {
            _logger.LogInformation("CSL service-GetDynamicOffers {token}, {request}", token, request);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            var responseData = await _resilientClientDO.PostHttpAsyncWithOptions("", request, headers).ConfigureAwait(false);
            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-GetDynamicOffers {@RequestUrl} error {Response}", responseData.url, responseData.response);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception("Service did not return any reponse");
            }

            _logger.LogInformation("CSL service-GetDynamicOffers {@RequestUrl}, {Response}", responseData.url, responseData.response);
            return (responseData.response);

        }
    }
}
