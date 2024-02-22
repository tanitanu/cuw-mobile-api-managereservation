using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Travelers
{
    public class MobileShoppingCart : IMobileShoppingCart
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<MobileShoppingCart> _logger;

        public MobileShoppingCart(
              [KeyFilter("MobileShoppingCartClientKey")] IResilientClient resilientClient
            , ICacheLog<MobileShoppingCart> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<string> UnRegisterAncillaryOffersForBooking(string token, string action, string request, string sessionId)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            using (_logger.BeginTimedOperation("Total time taken for UnRegisterAncillaryOffersForBooking business call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                _logger.LogInformation("MSCProductService-UnRegisterAncillaryOffersForBooking {@RequestUrl},{@Request}", action, request);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(action, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("MSCProductService-UnRegisterAncillaryOffersForBooking {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;
                _logger.LogInformation("MSCProductService-UnRegisterAncillaryOffersForBooking {@RequestUrl}, {@Response}", responseData.url, returnValue);
            }
            return returnValue;

        }
    }
}
