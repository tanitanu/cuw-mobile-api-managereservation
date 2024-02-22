using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using United.Mobile.Model;

namespace United.Utility.Middleware
{
    public class SecurityMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SecurityMiddleware> _logger;
       
        public SecurityMiddleware(RequestDelegate next, ILogger<SecurityMiddleware> logger)
        {
            _next = next;
            this._logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var responseObj = new Response<bool>();
            if (!context.Request.Path.Value.Contains("HealthCheck"))
            {
                responseObj.Errors = CheckAndGetErrorObject(context).Errors;

                if (responseObj.Errors.Count > 0)
                {
                    context.Response.StatusCode = (int)System.Net.HttpStatusCode.BadRequest;

                    context.Response.ContentType = Constants.ContentTypeJsonText;
                    responseObj.Status = (int)System.Net.HttpStatusCode.BadRequest;

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(responseObj));
                    return;
                }
            }
            await _next(context);
        }


        private Response<bool> CheckAndGetErrorObject(HttpContext context)
        {
            var responseObj = new Response<bool>();

            foreach (var headerText in Constants.HeaderTextList)
            {
                if (string.IsNullOrEmpty(context.Request.Headers[headerText]))
                {
                    responseObj.Errors.Add(headerText, new List<string> { string.Format(Constants.ErrorInvalidHeaderErrorDetail, headerText) });
                }
            }
            return responseObj;
        }
    }

}
