using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.CachingAndSessionModels;
using United.Utility.Helper;
using United.Utility.Http;

namespace United.Mobile.DataAccess.Common
{
    public class CachingService : ICachingService
    {
        private readonly ICacheLog<CachingService> _logger;
        private readonly IResilientClient _resilientClient;
        private readonly IConfiguration _configuration;
        public CachingService([KeyFilter("cachingConfigKey")] IResilientClient resilientClient, ICacheLog<CachingService> logger, IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<string> GetDocument<T>(string key)
        {
            var dpData = await _resilientClient.GetAsync(key);
            return dpData;
        }
        public async Task<T> GetDocument<T>(string key, string transactionId)
        {
            CachingGetRequest cachingGetRequest = new CachingGetRequest()
            {
                Key = key,
                TransactionId = transactionId
            };

            string requestData = JsonConvert.SerializeObject(cachingGetRequest);
            var cachReturnData = await _resilientClient.PostHttpAsync("Get", requestData);

            if (cachReturnData.Item2 == HttpStatusCode.NotFound && !string.IsNullOrEmpty(cachReturnData.Item1))
            {
                var returnValue = JsonConvert.DeserializeObject<CachingErrorResponse>(cachReturnData.Item1);
                _logger.LogError("Caching Get service {requestUrl} error {response} for {key}", cachReturnData.Item3, returnValue, key);
                return JsonConvert.DeserializeObject<T>(string.Empty);
            }
            else if (cachReturnData.Item2 != HttpStatusCode.OK)
            {
                var returnValue = JsonConvert.DeserializeObject<CachingErrorResponse>(cachReturnData.Item1);
                _logger.LogError("Caching Get service {requestUrl} error {response} for {key}", cachReturnData.Item3, returnValue, key);
                if (cachReturnData.Item2 != HttpStatusCode.BadRequest)
                    throw new Exception(cachReturnData.Item1);
            }

            _logger.LogInformation("Caching Get service {requestUrl} and {key}", cachReturnData.Item3, key);
            return JsonConvert.DeserializeObject<T>(cachReturnData.Item1);
        }

        public async Task<bool> SaveDocument<T>(string key, T data, long eventTimestamp, TimeSpan expiry)
        {
            CachingRequest cachingData = new CachingRequest()
            {
                Data = data,
                Key = key,
                // EventTimestamp = eventTimestamp,
                ExpirationOptions = new ExpirationOptions()
                {
                    SlidingExpiration = expiry
                }

            };
            string requestData = JsonConvert.SerializeObject(cachingData);
            var dpData = await _resilientClient.PostAsync(string.Empty, requestData);
            if (!string.IsNullOrEmpty(dpData))
                return true;
            return false;
        }

        public async Task<bool> SaveDocument<T>(string key, T data, string transactionID)
        {
            int expiryTime = _configuration.GetValue<int>("SlidingExpiration");
            expiryTime = expiryTime == 0 ? 10 : expiryTime;
            int absoluteExpirationInMin = _configuration.GetValue<int>("absoluteExpirationInMin");
            absoluteExpirationInMin = absoluteExpirationInMin == 0 ? 10 : absoluteExpirationInMin;
            CachingRequest cachingData = new CachingRequest()
            {
                Data = data,
                Key = key,
                TransactionId = transactionID,
                ExpirationOptions = new ExpirationOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(absoluteExpirationInMin),
                    SlidingExpiration = new TimeSpan(0, expiryTime, 0)
                }
            };

            string requestData = JsonConvert.SerializeObject(cachingData);
            var cachReturnData = await _resilientClient.PostHttpAsync("Save", requestData);

            if (cachReturnData.Item2 != HttpStatusCode.OK)
            {
                var returnValue = JsonConvert.DeserializeObject<CachingErrorResponse>(cachReturnData.Item1);
                _logger.LogError("Caching Save service {requestUrl} error {response} for {key}", cachReturnData.Item3, returnValue, key);
                if (cachReturnData.Item2 != HttpStatusCode.BadRequest)
                    throw new Exception(cachReturnData.Item1);
            }

            _logger.LogInformation("Caching Save service {requestUrl} and {key}", cachReturnData.Item3, key);
            return Convert.ToBoolean(cachReturnData.Item1);
        }

        public async Task<bool> RemoveDocument(string key, string transactionId)
        {
            CachingGetRequest cachingGetRequest = new CachingGetRequest()
            {
                Key = key,
                TransactionId = transactionId
            };

            string requestData = JsonConvert.SerializeObject(cachingGetRequest);
            var cacheReturnData = await _resilientClient.PostHttpAsync("Remove", requestData);

            if (cacheReturnData.Item2 != HttpStatusCode.OK)
            {
                var returnValue = JsonConvert.DeserializeObject<CachingErrorResponse>(cacheReturnData.Item1);
                _logger.LogError("Caching Remove service {requestUrl} error {response} for {key}", cacheReturnData.Item3, returnValue, key);
                if (cacheReturnData.Item2 != HttpStatusCode.BadRequest)
                    throw new Exception(cacheReturnData.Item1);
            }

            _logger.LogInformation("Caching Remove service {requestUrl} and {key}", cacheReturnData.Item3, key);
            return Convert.ToBoolean(cacheReturnData.Item1);
        }

        public async Task<bool> SaveCache<T>(string key, T data, string transactionId, TimeSpan expiry)
        {
            SaveCacheRequest cachingData = new SaveCacheRequest()
            {
                Data = data,
                Key = key,
                TransactionId = transactionId,
                ExpirationOptions = new ExpirationOptions()
                {
                    AbsoluteExpiration = DateTime.UtcNow.AddMinutes(expiry.TotalMinutes),
                    SlidingExpiration = expiry
                }
            };
            string requestData = JsonConvert.SerializeObject(cachingData);
            var dpData = string.Empty;
            using (_logger.BeginTimedOperation("Total time taken for Service call Save caching", transationId: transactionId))
            {
                try
                {
                    dpData = await _resilientClient.PostAsync("Save", requestData);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while saving the document from caching service: {Key}, {@error}", key, ex);
                }
            }
            if (!string.IsNullOrEmpty(dpData))
                return true;
            return false;
        }

        public async Task<string> GetCache<T>(string key, string transactionId)
        {
            CacheRequest cachingData = new CacheRequest()
            {
                Key = key,
                TransactionId = transactionId
            };
            string requestData = JsonConvert.SerializeObject(cachingData);

            using (_logger.BeginTimedOperation("Total time taken for Service call Get caching", transationId: transactionId))
            {
                try
                {
                    var responseD = await _resilientClient.PostHttpAsync("Get", requestData);
                    var response = responseD.Item1;
                    var statusCode = responseD.Item2;
                    var url = responseD.Item3;

                    if (statusCode == HttpStatusCode.OK)
                    {
                        return response;
                    }
                    else if (statusCode == HttpStatusCode.NotFound)
                    {
                        _logger.LogWarning("Warning while getting the document from caching service: {Key}", key);
                        return string.Empty;
                    }
                    else
                    {
                        _logger.LogWarning("Warning while getting the document from caching service: {Key} and {message}", key,statusCode);
                        return string.Empty;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error while Get the document from caching service: {Key}, {@error}", key, ex);
                }
            }

            return string.Empty;
        }
    }
}