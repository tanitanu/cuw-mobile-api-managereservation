using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Utility.Middleware
{
    internal class SetCultureMiddleware
    {
        private readonly ILogger<SetCultureMiddleware> _logger;
        private readonly RequestDelegate _next;
        public SetCultureMiddleware(RequestDelegate next, ILogger<SetCultureMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.ToLower().Contains("/api"))
                {
                    var stopWatch = Stopwatch.StartNew();

                    //var bodyAsText = string.Empty;
                    //if (httpContext.Request.Method.ToUpper() == "POST")
                    //{
                    //    using StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                    //    bodyAsText = await reader.ReadToEndAsync();
                    //    httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(bodyAsText));
                    //}
                    //else if (httpContext.Request.Method.ToUpper() == "GET")
                    //{
                    //    var requestFeature = httpContext.Request.HttpContext.Features.Get<IHttpRequestFeature>();
                    //    bodyAsText = requestFeature.RawTarget;
                    //}

                    //var currentCulture = await GetCurrentCulture(httpContext.Request, bodyAsText);
                    var currentCulture = "en-US";
                    /// The below code snippet is to set the CultureInfo to the current thread extracted from the request : QueryString/Headers/Body
                    /// This will be dynamic as per user's current culture from UI.
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentCulture);
                    await _next(httpContext);
                    stopWatch.Stop();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred in SetCultureMiddleware");
                await _next(httpContext);
            }
        }

        private Task<string> GetCurrentCulture(HttpRequest request, string bodyAsText)
        {
            string currentCulture = null;
            if (!string.IsNullOrEmpty(request.HttpContext.Request.Headers["languageCode"].ToString()) && !string.IsNullOrWhiteSpace(request.HttpContext.Request.Headers["languageCode"].ToString()))
                currentCulture = request.HttpContext.Request.Headers["languageCode"].ToString();
            else if (request.Method == "POST")
            {
                var mobRequest = JsonConvert.DeserializeObject<MOBRequest>(bodyAsText);
                currentCulture = mobRequest.LanguageCode;
            }
            else if (request.Method == "GET")
            {
                var queryStringParameters = bodyAsText.Split("?").Last().Split("&");
                foreach (var qp in queryStringParameters)
                    if (qp.Contains("=") && qp.Contains("languageCode"))
                    {
                        currentCulture = qp.Split("=").Last();
                        break;
                    }
            }

            return Task.FromResult(currentCulture ?? "en-US");
        }
    }
}
