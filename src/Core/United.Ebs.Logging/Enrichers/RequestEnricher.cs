using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using United.Ebs.Logging.Common;

namespace United.Ebs.Logging.Enrichers
{
    public class RequestEnricher : Enricher, IRequestEnricher
    {
        private readonly IHttpContextAccessor accessor;

        public RequestEnricher(IHttpContextAccessor accessor)
        {
            this.accessor = accessor;
            RequestInfo = new List<KeyValuePair<string, dynamic>>();
            ResponseInfo = new List<KeyValuePair<string, dynamic>>();
        }

        public List<KeyValuePair<string, dynamic>> RequestInfo { get; set; }
        public List<KeyValuePair<string, dynamic>> ResponseInfo { get; set; }

        public void ProcessRequest(dynamic requestBody)
        {
            // Get Request Information
            GetRequestInfo(requestBody);
        }

        public void ProcessResponse(dynamic responseBody, double responseTime)
        {
            try
            {
                ResponseInfo.Add(new KeyValuePair<string, dynamic>(Constants.ResponsePayloadAttributes.Response, responseBody));
                ResponseInfo.Add(new KeyValuePair<string, dynamic>(Constants.ResponsePayloadAttributes.ResponseTime, responseTime));

                GetResponseInfo();
            }
            catch(Exception ex)
            {
                Log.Logger.Fatal(ex, Common.Constants.EbsLoggingException + "ProcessResponse");
            }
        }

        private void GetRequestInfo(dynamic requestBody)
        {
            if (accessor == null || accessor.HttpContext == null || accessor.HttpContext.Request == null)
                return;

            try
            {
                HttpRequest request = accessor.HttpContext.Request;
                RequestInfo = new List<KeyValuePair<string, dynamic>>();
                RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.RequestMethod, request.Method.ToString()));
                RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.RequestPath, request.Path.ToString()));
                RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.Request, requestBody));
                RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.RequestHeader, request.Headers));

                //AddRequestHeaders(request);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, Common.Constants.EbsLoggingException + "GetRequestInfo");
            }
        }

        private void AddRequestHeaders(HttpRequest request)
        {
            if (request == null || request.Headers == null)
                return;

            StringValues values;
            if (request.Headers.TryGetValue(Constants.RequestPayloadAttributes.RefererUri, out values))
            {
                RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.RefererUri, values));
            }

            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            foreach(var item in request.Headers)
            {
                if (item.Key == Constants.RequestHeaderAttributes.IdToken)
                    continue;

                dict.Add(item.Key, item.Value);
            }

            RequestInfo.Add(new KeyValuePair<string, dynamic>(Constants.RequestPayloadAttributes.RequestHeader, dict));
        }

        private void AddResponseHeaders(HttpResponse response)
        {
            if (response == null || response.Headers == null)
                return;

            Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
            foreach (var item in response.Headers)
            {
                if (item.Key == Constants.RequestHeaderAttributes.IdToken)
                    continue;

                dict.Add(item.Key, item.Value);
            }

            ResponseInfo.Add(new KeyValuePair<string, dynamic>(Constants.ResponsePayloadAttributes.ResponseHeader, dict));
        }

        private void GetResponseInfo()
        {
            if (accessor == null || accessor.HttpContext == null || accessor.HttpContext.Response == null)
                return;

            try
            {
                HttpResponse response = accessor.HttpContext.Response;

                ResponseInfo.Add(new KeyValuePair<string, dynamic>(Constants.ResponsePayloadAttributes.HttpStatusCode, response.StatusCode));
                ResponseInfo.Add(new KeyValuePair<string, dynamic>(Constants.ResponsePayloadAttributes.ResponseHeader, response.Headers));

                //AddResponseHeaders(response);
            }
            catch (Exception ex)
            {
                Log.Logger.Fatal(ex, Common.Constants.EbsLoggingException + "GetResponseInfo");
            }
        }
    }
}
