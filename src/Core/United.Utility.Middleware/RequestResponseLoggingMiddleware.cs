using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using United.Ebs.Logging;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model;
using System.Globalization;
using System.Threading;
using United.Utility.Helper;

namespace United.Utility.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;
        private readonly RequestDelegate _next;
        private readonly IHttpContextAccessor _contextAccessor;

        public RequestResponseLoggingMiddleware(RequestDelegate next
            , ILogger<RequestResponseLoggingMiddleware> logger
            , IHttpContextAccessor contextAccessor)
        {
            _next = next;
            _logger = logger;
            _contextAccessor = contextAccessor;
        }

        public async Task InvokeAsync(HttpContext httpContext, CacheLogWriter cacheLogWriter)
        {
            try
            {
                if (httpContext.Request.Path.HasValue && httpContext.Request.Path.Value.ToLower().Contains("/api"))
                {
                    var stopWatch = Stopwatch.StartNew();

                    if (!httpContext.Request.Path.Value.ToLower().Contains("/api/healthcheck")
                        && !httpContext.Request.Path.Value.ToLower().Contains("/api/version"))
                    {
                        var bodyAsText = string.Empty;
                        if (httpContext.Request.Method.ToUpper() == "POST")
                        {
                            using StreamReader reader = new StreamReader(httpContext.Request.Body, Encoding.UTF8);
                            bodyAsText = await reader.ReadToEndAsync();
                            httpContext.Request.Body = new MemoryStream(Encoding.UTF8.GetBytes(bodyAsText));
                        }
                        else if (httpContext.Request.Method.ToUpper() == "GET")
                        {
                            var requestFeature = httpContext.Request.HttpContext.Features.Get<IHttpRequestFeature>();
                            bodyAsText = requestFeature.RawTarget;
                        }

                        SetRequestInformationDetailsToEnricher(httpContext.Request, bodyAsText);

                        await LogHttpRequest(httpContext.Request);

                        var originalBodyStream = httpContext.Response.Body;
                        using (var responseBody = new MemoryStream())
                        {
                            var response = httpContext.Response;
                            response.Body = responseBody;

                            var currentCulture = "en-US";
                            /// The below code snippet is to set the CultureInfo to the current thread extracted from the request : QueryString/Headers/Body
                            /// This will be dynamic as per user's current culture from UI.
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(currentCulture);
                            Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(currentCulture);
                            Thread.CurrentThread.CurrentUICulture.NumberFormat.CurrencyNegativePattern = 0;
                            Thread.CurrentThread.CurrentCulture.NumberFormat.CurrencyNegativePattern = 0;

                            await _next(httpContext);
                            stopWatch.Stop();

                            try
                            {
                                cacheLogWriter.SaveToLogs();
                            }
                            catch (Exception ex)
                            {
                                //var e = ex;
                            }

                            await LogHttpResponse(response, stopWatch.ElapsedMilliseconds);
                            await responseBody.CopyToAsync(originalBodyStream);
                        }
                    }
                    else
                    {
                        await _next(httpContext);
                    }

                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception occurred in RequestResponseLoggingMiddleware {@exceptionMessage}", ex);
                await _next(httpContext);
            }
        }

        private async Task LogHttpRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var bodyAsText = string.Empty;
            if (request.Method.ToUpper() == "POST")
            {
                var buffer = new byte[Convert.ToInt64(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                bodyAsText = Encoding.UTF8.GetString(buffer);
                request.Body.Seek(0, SeekOrigin.Begin);
            }
            else if (request.Method.ToUpper() == "GET")
            {
                var requestFeature = request.HttpContext.Features.Get<IHttpRequestFeature>();
                bodyAsText = requestFeature.RawTarget;
            }           
            //_logger.LogRequest(bodyAsText);            
        }

        private async Task LogHttpResponse(HttpResponse response, double elapsedTime)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            //_logger.LogResponse(bodyAsText, elapsedTime);
        }

        private void SetRequestInformationDetailsToEnricher(HttpRequest request, string bodyAsText)
        {
            var requestEnricher = _contextAccessor?.HttpContext?.RequestServices?.GetService(typeof(IRequestEnricher)) as IRequestEnricher;

            if (requestEnricher == null)
                return;

            if (request.Method == "POST")
            {
                JObject data = (JObject)JsonConvert.DeserializeObject(bodyAsText);
                if (data != null)
                {
                    Dictionary<string, object> keyValues = new Dictionary<string, object>(data.ToObject<IDictionary<string, object>>(), StringComparer.CurrentCultureIgnoreCase);
                    requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.DeviceIdText, GetValueFromToken("deviceId", keyValues)));
                    requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.SessionId, GetValueFromToken("sessionId", keyValues)));
                    requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.TransactionIdText, GetValueFromToken("transactionId", keyValues)));

                    var applicationString = GetValueFromToken("application", keyValues);
                    if (!string.IsNullOrWhiteSpace(applicationString))
                    {
                        var application = JsonConvert.DeserializeObject<MOBApplication>(applicationString);

                        requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.ApplicationIdText, application?.Id));
                        requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.ApplicationVersionText, application?.Version?.Major));
                    }
                }
            }
            else if (request.Method == "GET")
            {
                var queryStringParameters = bodyAsText.Split("?").Last().Split("&");

                foreach (var qp in queryStringParameters)
                {
                    if (qp.Contains("="))
                    {
                        if (qp.Contains("appversion", StringComparison.OrdinalIgnoreCase))
                            requestEnricher.Add(United.Mobile.Model.Constants.ApplicationVersionText, qp.Split("=").Last());
                        if (qp.Contains("applicationId", StringComparison.OrdinalIgnoreCase))
                            requestEnricher.Add(United.Mobile.Model.Constants.ApplicationIdText, qp.Split("=").Last());
                        if (qp.Contains("transactionid", StringComparison.OrdinalIgnoreCase))
                        {
                            var transcations = HttpUtility.UrlDecode(qp.Split("=").Last()).Split("|");
                            requestEnricher.Add(United.Mobile.Model.Constants.TransactionIdText, qp);
                            requestEnricher.Add(United.Mobile.Model.Constants.DeviceIdText, transcations.FirstOrDefault());
                        }
                        if (qp.Contains("sessionid", StringComparison.OrdinalIgnoreCase))
                        {
                            requestEnricher.Add(United.Mobile.Model.Constants.SessionId, qp.Split("=").Last());
                        }
                        if (qp.Contains("deviceid", StringComparison.OrdinalIgnoreCase))
                        {
                            requestEnricher.Add(United.Mobile.Model.Constants.DeviceIdText, qp.Split("=").Last());
                        }

                    }
                }
            }
            requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.HeaderLangCodeText, "en-US"));
            requestEnricher.KeyValues.Add(new KeyValuePair<string, dynamic>(United.Mobile.Model.Constants.ServerName, System.Environment.MachineName));
        }
        private string GetValueFromToken(string key, Dictionary<string, object> data)
        {
            if (data.ContainsKey(key))
            {
                return data[key].ToString();
            }
            return string.Empty;
        }
    }
}
