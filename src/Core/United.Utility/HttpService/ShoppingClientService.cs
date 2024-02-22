using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using United.Csl.Ms.Common.Interfaces;
using System.Threading.Tasks;
using System.Net;
using Polly;
using Polly.CircuitBreaker;
using United.Utility.Config;
using System.IO;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using United.Utility.Helper;

namespace United.Utility.HttpService
{
    public class ShoppingClientService : IShoppingClientService
    {
        private readonly HttpClient _httpClient;
        private readonly ShoppingOptions _shoppingOptions;
        private readonly ICacheLog<ShoppingClientService> _logger;
        private readonly IAsyncPolicy _policyWrap;

        public ShoppingClientService(HttpClient httpClient, IOptions<ShoppingOptions> shoppingOptions, ICacheLog<ShoppingClientService> logger)
        {
            _httpClient = httpClient;
            _shoppingOptions = shoppingOptions.Value;
            _logger = logger;            

            _policyWrap = Policy.WrapAsync(
                Policy.Handle<Exception>()
                    .CircuitBreakerAsync(_shoppingOptions.CircuitBreakerAllowExceptions, TimeSpan.FromSeconds(_shoppingOptions.CircuitBreakerBreakDuration)),
                Policy.Handle<Exception>()
                    .WaitAndRetryAsync(retryCount: _shoppingOptions.RetryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))),
                Policy.TimeoutAsync((int)_shoppingOptions.TimeOut));
        }

        public async Task<T> PostAsync<T>(string token, string sessionId, string action, object request, string contentType = "application/json")
        {
            var headers = CreateHeaders(token);       
            using var streamToHoldRequest = new MemoryStream();
            Serialize(request, streamToHoldRequest);
            using var postRequest = CreatePostRequest(headers, action);
            using var content = CreateHttpContent(streamToHoldRequest, contentType);
            postRequest.Content = content;
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(_shoppingOptions.TimeOut));
            T response;

            try
            {
                IDisposable timer = null;

                using ((timer = _logger.BeginTimedOperation("Total time taken for PostAsync service call", transationId: sessionId)))
                {
                    _logger.LogInformation("CSL Shop {@RequestUrl} {@Request}", _shoppingOptions.Url, JsonConvert.SerializeObject(request));
                    using var httpResponseMessage = await CallService(postRequest, cts).ConfigureAwait(false);
                    if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                        _logger.LogError("CSL service-GetShop request {@RequestUrl} {@Response}", _shoppingOptions.Url, httpResponseMessage.Content);
                    using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    response = Utility.Helper.DataContextJsonSerializer.DeserializeUseContract<T>(responseStream);
                    //response = Deserialize<T>(responseStream);
                    _logger.LogInformation("CSL Shop {@RequestUrl} {@Response}", _shoppingOptions.Url, JsonConvert.SerializeObject(response));
                }
            }
            catch(Exception ex) {
                _logger.LogError("CSL service-GetShop request is failed {@Exception}", JsonConvert.SerializeObject(ex));
                throw ex;
            }
            //TO-DO::Check for valid Response
            //if (HasValidResponse(httpResponseMessage))
            //{
            //}
            //var errorContent = ReadErrorResponse(await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false));
            // throw new APIException(errorContent);

            return response;
            
        }

        private static Dictionary<string, string> CreateHeaders(string token)
        {
            return new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
        }

        private async Task<HttpResponseMessage> CallService(HttpRequestMessage postRequest, CancellationTokenSource cts)
        {
            return await _policyWrap.ExecuteAsync(async () =>
            {
                var response = await _httpClient.SendAsync(postRequest, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                response.EnsureSuccessStatusCode();
                return response;
            });
            
           
        }


        private static HttpContent CreateHttpContent(Stream serializedRequest, string contentType)
        {
            serializedRequest.Seek(0, SeekOrigin.Begin);
            var httpContent = new StreamContent(serializedRequest);
            httpContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
            return httpContent;
        }

        private T Deserialize<T>(Stream stream)
        {
            if (stream == null || stream.CanRead == false)
            {
                return default;
            }
            using var sr = new StreamReader(stream);
            using var jr = new JsonTextReader(sr);
            //jr.ArrayPool = JsonArrayPool.Instance;
            var ntSerializer = new JsonSerializer();
            var response = ntSerializer.Deserialize<T>(jr);
            return response;

        }
        private Stream Serialize<T>(T request, Stream stream) where T : class
        {
            using var sw = new StreamWriter(stream, new UTF8Encoding(false), 1024, true);
            using var jtw = new JsonTextWriter(sw) { Formatting = Newtonsoft.Json.Formatting.None };
            //jtw.ArrayPool = JsonArrayPool.Instance;
            var js = new JsonSerializer();
            js.Serialize(jtw, request);
            jtw.Flush();
            return stream;
        }


        private HttpRequestMessage CreatePostRequest(IDictionary<string, string> headers, string action)
        {
            var url = _shoppingOptions.Url + action;
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, url);
            if(headers != null)
            {
                foreach (string key in headers.Keys)
                    requestMessage.Headers.Add(key, headers[key]);
            }
            return requestMessage;
        }
        

        public async Task<T> PostAsyncForReShop<T>(string token, string sessionId, string action, object request, string contentType = "application/json")
        {
            var headers = CreateHeaders(token);
            using var streamToHoldRequest = new MemoryStream();
            Serialize(request, streamToHoldRequest);
            using var postRequest = CreatePostRequest(headers, action);
            using var content = CreateHttpContent(streamToHoldRequest, contentType);
            postRequest.Content = content;
            using var cts = new CancellationTokenSource(TimeSpan.FromMilliseconds(_shoppingOptions.TimeOut));
            T response;

            try
            {
                IDisposable timer = null;

                using ((timer = _logger.BeginTimedOperation("Total time taken for PostAsyncForReShop service call", transationId: sessionId)))
                {
                    _logger.LogInformation("CSL ReShop {@RequestUrl} {@Request}", _shoppingOptions.Url, JsonConvert.SerializeObject(request));
                    using var httpResponseMessage = await CallService(postRequest, cts).ConfigureAwait(false);
                    if (httpResponseMessage.StatusCode != HttpStatusCode.OK)
                        _logger.LogError("CSL service-ChangeEligibleCheckAndReShop request {@RequestUrl} {@Response}", _shoppingOptions.Url, httpResponseMessage.Content);
                    using var responseStream = await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    StreamReader streamReader = new StreamReader(responseStream);
                    response = Utility.Helper.DataContextJsonSerializer.NewtonSoftDeserialize<T>(streamReader.ReadToEnd());
                    //response = Deserialize<T>(responseStream);
                    _logger.LogInformation("CSL ReShop {@RequestUrl} {@Response}", _shoppingOptions.Url, JsonConvert.SerializeObject(response));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CSL service-ChangeEligibleCheckAndReShop request is failed {@Exception}", JsonConvert.SerializeObject(ex));
                throw ex;
            }
            //TO-DO::Check for valid Response
            //if (HasValidResponse(httpResponseMessage))
            //{
            //}
            //var errorContent = ReadErrorResponse(await httpResponseMessage.Content.ReadAsStreamAsync().ConfigureAwait(false));
            // throw new APIException(errorContent);

            return response;

        }
    }
}
