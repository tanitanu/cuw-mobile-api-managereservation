using Autofac.Features.AttributeFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class CCEAncillaryMyTripsFeedbackService : ICCEAncillaryMyTripsFeedbackService
    {
        private readonly IResilientClient _resilientClientFeedback;
        private readonly ICacheLog<CCEAncillaryMyTripsFeedbackService> _logger;
        public CCEAncillaryMyTripsFeedbackService(
             [KeyFilter("CCEAncillaryMyTripsFeedbackClientKey")] IResilientClient resilientClientFeedback
            , ICacheLog<CCEAncillaryMyTripsFeedbackService> logger
            )
        {
            _resilientClientFeedback = resilientClientFeedback;
            _logger = logger;
        }

        public async Task<string> SendCCEAncillaryMyTripsFeedback(string token, string request)
        {
            _logger.LogInformation("CSL service-SendCCEAncillaryMyTripsFeedback {token}, {request}", token, request);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            var responseData = await _resilientClientFeedback.PostHttpAsyncWithOptions("", request, headers).ConfigureAwait(false);
            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-SendCCEAncillaryMyTripsFeedback {@RequestUrl} error {Response}", responseData.url, responseData.response);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    throw new Exception("Service did not return any reponse");
            }

            _logger.LogInformation("CSL service-SendCCEAncillaryMyTripsFeedback {@RequestUrl}, {Response}", responseData.url, responseData.response);
            return (responseData.response);

        }
    }
}
