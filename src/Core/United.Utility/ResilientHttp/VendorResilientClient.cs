using Polly;
using Polly.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using United.Utility.Config;
using United.Utility.Http;

namespace United.Utility.ResilientHttp
{
    public class VendorResilientClient : IResilientClient
    {
        private readonly HttpClient _httpClient;
        private readonly IAsyncPolicy _policyWrap;
        private readonly string _baseUrl;
        private AsyncCircuitBreakerPolicy _circuitBreakerPolicy;
        private string circuitBreakerStatus;

        public string BaseURL { get { return _baseUrl; } }

        public VendorResilientClient(string baseUrl, TimeoutPolicyConfig timeoutPolicyConfig = null, RetryPolicyConfig retryPolicyConfig = null, CircuitBreakerPolicyConfig cbPolicyConfig = null)
        {
            timeoutPolicyConfig ??= new TimeoutPolicyConfig();
            retryPolicyConfig ??= new RetryPolicyConfig();
            cbPolicyConfig ??= new CircuitBreakerPolicyConfig();

            _baseUrl = baseUrl;
            //_httpClient = new HttpClient("HttpClientWithSSLUntrusted");

            //ServicePointManager.Expect100Continue = false;
            //ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            _httpClient = new HttpClient(new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            });

            _circuitBreakerPolicy = Policy.Handle<Exception>()
                     .CircuitBreakerAsync(cbPolicyConfig.AllowExceptions, TimeSpan.FromSeconds(cbPolicyConfig.BreakDuration));

            _policyWrap = Policy.WrapAsync(
                _circuitBreakerPolicy,
                Policy.Handle<Exception>()
                    .WaitAndRetryAsync(retryCount: retryPolicyConfig.RetryCount,
                        sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))),
                Policy.TimeoutAsync(timeoutPolicyConfig.Seconds, Polly.Timeout.TimeoutStrategy.Optimistic));
            //Policy.TimeoutAsync(timeoutPolicyConfig.Seconds, Polly.Timeout.TimeoutStrategy.Optimistic));
            //Policy.TimeoutAsync(TimeSpan.FromMilliseconds(400)));
        }

        public VendorResilientClient(IResilientClientOptions rcOptions)
            : this(rcOptions.BaseUrl, rcOptions.TimeoutPolicyConfig, rcOptions.RetryPolicyConfig, rcOptions.CircuitBreakerPolicyConfig)
        {
        }

        private void SetHeaders(IDictionary<string, string> requestHeaders)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (requestHeaders != null)
            {
                foreach (string key in requestHeaders.Keys)
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(key, requestHeaders[key]);
            }
        }

        /// <summary>
        /// Send a Post request
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<string> PostAsync(string endPoint, string requestData, IDictionary<string, string> requestHeaders = null, string contentType = "application/json", bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            string responseMessage = null;
            using (var stringContent = new StringContent(requestData, Encoding.UTF8, contentType))
            {
                responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
                {
                    var requestUrl = $"{_baseUrl}{endPoint}";
                    //if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains("https://"))
                    //{
                    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //}
                    var response = await _httpClient.PostAsync(requestUrl, stringContent, ct);
                    if (throwWebException) response.EnsureSuccessStatusCode();

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }, CancellationToken.None).ConfigureAwait(false);
            }

            return responseMessage;
        }

        /// <summary>
        /// Send a Post request
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode, string>> PostHttpAsync(string endPoint, string requestData, IDictionary<string, string> requestHeaders = null, string contentType = "application/json", bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            string responseMessage = null;
            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";
            using (var stringContent = new StringContent(requestData, Encoding.UTF8, contentType))
            {
                responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
                {
                    //if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains("https://"))
                    //{
                    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //}
                    var responseTuplePost = await _httpClient.PostAsync(requestUrl, stringContent, ct);
                    if (throwWebException) responseTuplePost.EnsureSuccessStatusCode();
                    statusCode = responseTuplePost.StatusCode;

                    return await responseTuplePost.Content.ReadAsStringAsync().ConfigureAwait(false);

                }, CancellationToken.None).ConfigureAwait(false);
            }

            return new Tuple<string, HttpStatusCode, string>(responseMessage, statusCode, requestUrl);
        }

        public async Task<(string, HttpStatusCode, string)> PostHttpAsyncWithOptions(string endPoint, string requestData, IDictionary<string, string> requestHeaders = null, string contentType = "application/json", bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            string responseMessage = null;
            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";
            //var ct1 = new CancellationTokenSource(400).Token;

            circuitBreakerStatus = $"CircuitState: {_circuitBreakerPolicy.CircuitState.ToString()}";

            using (var stringContent = new StringContent(requestData, Encoding.UTF8, contentType))
            {
                responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
                {
                    var responseTuplePost = await _httpClient.PostAsync(requestUrl, stringContent, ct);
                    statusCode = responseTuplePost.StatusCode;

                    if (throwWebException)
                    {
                        if (statusCode == HttpStatusCode.Forbidden //403
                            || statusCode == HttpStatusCode.RequestTimeout //408
                            || statusCode == HttpStatusCode.InternalServerError //500
                            || statusCode == HttpStatusCode.NotImplemented //501
                            || statusCode == HttpStatusCode.BadGateway //502
                            || statusCode == HttpStatusCode.ServiceUnavailable //503
                            || statusCode == HttpStatusCode.GatewayTimeout //504
                            || statusCode == HttpStatusCode.HttpVersionNotSupported //505
                            || statusCode == HttpStatusCode.VariantAlsoNegotiates //506
                            || statusCode == HttpStatusCode.InsufficientStorage //507
                            || statusCode == HttpStatusCode.LoopDetected //508
                            || statusCode == HttpStatusCode.NotExtended //510
                            || statusCode == HttpStatusCode.NetworkAuthenticationRequired) //511
                        {
                            responseTuplePost.EnsureSuccessStatusCode();
                        }
                    }

                    return await responseTuplePost.Content.ReadAsStringAsync().ConfigureAwait(false);

                }, CancellationToken.None).ConfigureAwait(false);
            }

            return (responseMessage, statusCode, requestUrl);
        }

        /// <summary>
        /// Send a Get request
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<string> GetAsync(string serviceUrl, IDictionary<string, string> requestHeaders = null, bool throwWebException = false)
        {
            SetHeaders(requestHeaders);
            var responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
            {
                var requestUrl = $"{_baseUrl}{serviceUrl}";
                //if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains("https://"))
                //{
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //}
                var response = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
                if (throwWebException) response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            }, CancellationToken.None).ConfigureAwait(false);

            return responseMessage;
        }

        /// <summary>
        /// Get call to REST Service return message with HttpStatus Code & Calling URL
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<Tuple<string, HttpStatusCode, string>> GetHttpAsync(string endPoint, IDictionary<string, string> requestHeaders = null, bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";

            var responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
            {
                //if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains("https://"))
                //{
                //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                //}
                var responseTuple = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
                if (throwWebException) responseTuple.EnsureSuccessStatusCode();
                statusCode = responseTuple.StatusCode;

                return await responseTuple.Content.ReadAsStringAsync().ConfigureAwait(false);

            }, CancellationToken.None).ConfigureAwait(false);

            return new Tuple<string, HttpStatusCode, string>(responseMessage, statusCode, requestUrl);
        }

        /// <summary>
        /// Get call to REST Service return message with HttpStatus Code & Calling URL
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<(string, HttpStatusCode, string)> GetHttpAsyncWithOptions(string endPoint, IDictionary<string, string> requestHeaders = null, bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";

            var responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
            {
                var responseTuple = await _httpClient.GetAsync(requestUrl).ConfigureAwait(false);
                if (throwWebException) responseTuple.EnsureSuccessStatusCode();
                statusCode = responseTuple.StatusCode;

                return await responseTuple.Content.ReadAsStringAsync().ConfigureAwait(false);

            }, CancellationToken.None).ConfigureAwait(false);

            return (responseMessage, statusCode, requestUrl);
        }

        /// <summary>
        /// Send a Put request
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<(string, HttpStatusCode, string)> PutAsync(string endPoint, string requestData, IDictionary<string, string> requestHeaders = null, string contentType = "application/json", bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";
            string responseMessage = null;
            using (var stringContent = new StringContent(requestData, Encoding.UTF8, contentType))
            {
                responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
                {
                    //if (!string.IsNullOrEmpty(requestUrl) && requestUrl.ToLower().Contains("https://"))
                    //{
                    //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    //}
                    var response = await _httpClient.PutAsync(requestUrl, stringContent).ConfigureAwait(false);
                    if (throwWebException) response.EnsureSuccessStatusCode();
                    statusCode = response.StatusCode;

                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                }, CancellationToken.None).ConfigureAwait(false);
            }

            return (responseMessage, statusCode, requestUrl);
        }

        /// <summary>
        /// Send a Delete request
        /// </summary>
        /// <exception cref="HttpRequestException">The request failed due to an underlying issue such as network connectivity, DNS failure, server certificate validation or timeout.</exception>
        /// <exception cref="BrokenCircuitException">Circuit breaker is open, not allowing request.</exception>
        /// <returns></returns>
        public async Task<(string, HttpStatusCode, string)> DeleteAsync(string endPoint, IDictionary<string, string> requestHeaders = null, bool throwWebException = true)
        {
            SetHeaders(requestHeaders);

            var statusCode = HttpStatusCode.OK;
            var requestUrl = $"{_baseUrl}{endPoint}";

            var responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
            {
                var response = await _httpClient.DeleteAsync(requestUrl).ConfigureAwait(false);
                if (throwWebException) response.EnsureSuccessStatusCode();
                statusCode = response.StatusCode;

                return await response.Content.ReadAsStringAsync().ConfigureAwait(false);

            }, CancellationToken.None).ConfigureAwait(false);

            return (responseMessage, statusCode, requestUrl);
        }

        public async Task<string> PostAsyncCache(string endPoint, string requestData, IDictionary<string, string> requestHeaders = null, string contentType = "application/json", bool throwWebException = false)
        {
            SetHeaders(requestHeaders);

            string responseMessage = null;
            using (var stringContent = new StringContent(requestData, Encoding.UTF8, contentType))
            {
                responseMessage = await _policyWrap.ExecuteAsync(async (ct) =>
                {
                    var response = await _httpClient.PostAsync(endPoint, stringContent, ct);
                    if (throwWebException) response.EnsureSuccessStatusCode();
                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default;
                    }
                    return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                }, CancellationToken.None).ConfigureAwait(false);
            }

            return responseMessage;
        }

        public string GetCircuitBreakerStatus()
        {
            return circuitBreakerStatus;
        }
    }
}
