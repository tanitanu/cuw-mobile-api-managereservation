using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.Model.Internal.Common;
using United.Service.Presentation.PersonalizationModel;
using United.Utility.Helper;
using United.Utility.Http;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace United.Mobile.DataAccess.Common
{
    public class DynamoDBService : IDynamoDBService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<DynamoDBService> _logger;
        public DynamoDBService([KeyFilter("DynamoDBClientKey")] IResilientClient resilientClient, ICacheLog<DynamoDBService> logger)
        {
            _resilientClient = resilientClient;
            _logger = logger;
        }

        public async Task<T> GetData<T>(string tableName, string docTitle, string transactionId)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var requestDataObject = new GetDataRequest() { TransactionId = transactionId, TableName = tableName, Key = docTitle };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - GetData {@requestData} for transactionid - {transactionId}", requestData, transactionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/GetData", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("DynamoDB - GetData {requestUrl} {@response} for transactionid - {transactionId} StatusCode - {httpStatusCode}", responseData.url, responseData.response, transactionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("DynamoDB - GetData {requestUrl} {@response} and {transactionId}", responseData.url, responseData.response, transactionId);

                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - GetData {@StackTrace} and {sessionId}", ex.StackTrace, transactionId);
            }

            return default;
        }

        public async Task<T> GetLegalDocumentsByTitle<T>(string tableName, string docTitle, string transactionId)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var requestDataObject = new GetDataRequest() { TransactionId = transactionId, TableName = tableName, Key = docTitle };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - GetLegalDocumentsByTitle {@requestData} for transactionid - {sessionId}", requestData, transactionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/DocumentLibrary/GetLegalDocumentsByTitle", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("DynamoDB - GetRecords {requestUrl} {@response} for transactionid - {sessionId} StatusCode - {httpStatusCode}", responseData.url, responseData.response, transactionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("DynamoDB - GetLegalDocumentsByTitle {requestUrl} {@response} and {transactionId}", responseData.url, responseData.response, transactionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - GetLegalDocumentsByTitle {@StackTrace} and {transactionId}", ex.StackTrace, transactionId);
            }

            return default;
        }

        public async Task<T> GetRecords<T>(string tableName, string transactionId, string key, string sessionId)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var requestDataObject = new GetDataRequest() { TransactionId = transactionId, TableName = tableName, Key = key };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - GetRecords {@requestData} for sessionid/transactionid - {sessionId}", requestData, transactionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/GetData", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("DynamoDB - GetRecords {requestUrl} {@response} for sessionid/transactionid - {sessionId} StatusCode- {httpStatusCode}", responseData.url, responseData.response, transactionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("DynamoDB - GetRecords {requestUrl} {@response} and {sessionId}", responseData.url, responseData.response, transactionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - GetRecords {@StackTrace} and {sessionId}", ex.StackTrace, sessionId);
            }

            return default;
        }

        public async Task<T> GetRecords<T>(string tableName, string[] childIds, string transactionId)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };

                var requestDataObject = new GetRecordsRequest() { TransactionId = transactionId, TableName = tableName, Keys = childIds };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - GetRecords {@requestData} for sessionid/transactionid - {sessionId}", requestData, transactionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/GetRecords", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("DynamoDB - GetRecords {requestUrl} {@response} for sessionid/transactionid - {sessionId} StatusCode- {httpStatusCode}", responseData.url, responseData.response, transactionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode == HttpStatusCode.NotFound)
                        return default;
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }

                _logger.LogInformation("DynamoDB - GetRecords {requestUrl} {@response} and {sessionId}", responseData.url, responseData.response, transactionId);
                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - GetRecords {@StackTrace} and {sessionId}", ex.StackTrace, transactionId);
            }

            return default;
        }

        public async Task<bool> SaveRecords<T>(string tableName, string transactionId, string key, T data, string sessionId)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                var requestDataObject = new SaveDataRequest<T>() { TransactionId = transactionId, TableName = tableName, Key = key, Data = data };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - SaveRecords {@requestData} for {sessionId}", requestData, sessionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/SaveData", requestData, headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("DynamoDB - SaveRecords {requestUrl} {@response} for {sessionId} StatusCode- {httpStatusCode}", responseData.url, responseData.response, sessionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("DynamoDB - SaveRecords {@response} for {sessionId}", responseData.response, sessionId);
                return JsonConvert.DeserializeObject<bool>(responseData.response);

            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - SaveRecords {StackTrace} and {sessionId}", ex.StackTrace, sessionId);
            }

            return default;
        }

        public async Task<bool> SaveRecords<T>(string tableName, string transactionId, string key, string secondaryKey, T data, string sessionId, double absoluteExpirationDays = 2)
        {
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"}
                     };
                var requestDataObject = new SaveDataRequest<T>() { TransactionId = transactionId, TableName = tableName, Key = key, SecondaryKey= secondaryKey, Data = data, AbsoluteExpiration= DateTime.UtcNow.AddDays(absoluteExpirationDays) };
                var requestData = JsonConvert.SerializeObject(requestDataObject);
                _logger.LogInformation("DynamoDB - SaveRecords {@requestData} for {sessionId}", requestData, sessionId);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions("/SaveData", requestData , headers).ConfigureAwait(false);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogWarning("Warning DynamoDB - SaveRecords {requestUrl} {response} for {sessionId} StatusCode- {httpStatusCode}", responseData.url, responseData.response, sessionId, Convert.ToInt32(responseData.statusCode));
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        throw new Exception(responseData.response);
                }
                _logger.LogInformation("DynamoDB - SaveRecords {@response} for {sessionId}", responseData.response, sessionId);
                return JsonConvert.DeserializeObject<bool>(responseData.response);

            }
            catch (Exception ex)
            {
                _logger.LogError("DynamoDB - SaveRecords {StackTrace} and {sessionId}", ex.StackTrace, sessionId);
            }

            return default;
        }
    }
}