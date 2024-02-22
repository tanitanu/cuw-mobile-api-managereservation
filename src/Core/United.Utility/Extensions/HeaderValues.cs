using Microsoft.AspNetCore.Http;
using System;
using United.Mobile.Model;

namespace United.Utility.Extensions
{
    public class HeaderValues
    {
        private IHttpContextAccessor _httpContextAccessor;
        public HeaderValues(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public HttpContextValues GetHeaderValues(HttpContextValues values)
        {
            if (
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppIdText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderDeviceIdText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderLangCodeText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderRequestTimeUtcText]) &&
                String.IsNullOrEmpty(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderTransactionIdText]) 
                )
            {
                return values;
            }
            values.Application = new Application
            {
                Id = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppIdText])
            };
            values.Application.Version = new Mobile.Model.Version
            {
                Major = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMajorText].ToString().Substring(0, 1)),
                Minor = int.Parse(_httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderAppMinorText].ToString().Substring(0, 1)),
            };

            values.DeviceId = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderDeviceIdText];
            values.LangCode = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderLangCodeText];
            values.RequestTimeUtc = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderRequestTimeUtcText];
            values.TransactionId = _httpContextAccessor.HttpContext.Request.Headers[Constants.HeaderTransactionIdText];

            return values;
        }
    }
}
