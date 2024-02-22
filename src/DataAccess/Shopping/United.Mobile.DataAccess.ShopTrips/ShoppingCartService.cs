using Autofac.Features.AttributeFilters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using United.Utility.Helper;
using United.Utility.Http;
using United.Utility.Serilog;

namespace United.Mobile.DataAccess.ShopTrips
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IResilientClient _resilientClient;
        private readonly ICacheLog<ShoppingCartService> _logger;
        private readonly IConfiguration _configuration;
        public ShoppingCartService([KeyFilter("ShoppingCartClientKey")] IResilientClient resilientClient
            , ICacheLog<ShoppingCartService> logger
            , IConfiguration configuration)
        {
            _resilientClient = resilientClient;
            _logger = logger;
            _configuration = configuration;

        }

        public async Task<T> GetShoppingCartInfo<T>(string token, string action, string request, string sessionId)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", action);

            _logger.LogInformation("CSL service-GetShoppingCartInfo {@Request} {@Url}", request, path);

            using (_logger.BeginTimedOperation("Total time taken for GetShoppingCartInfo business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetShoppingCartInfo {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetShoppingCartInfo {@RequestUrl}, {@Response}", responseData.url, responseData.response);

                return JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<T> GetCartInformation<T>(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service- GetCartInformation parameters Request:{@Request}, Action:{@Action}", request, action);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", action);

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for GetCartInformation business call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-GetCartInformation {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-GetCartInformation {@RequestUrl}, {@Response}", responseData.url, responseData.response);
                    return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-GetCartInformation error {@Exception}", JsonConvert.SerializeObject(ex));
                }

                return default;

            }
        }

        public async Task<T> GetProductDetailsFromCartID<T>(string token, string cartID, string sessionId)
        {
            _logger.LogInformation("CSL service-GetProductDetailsFromCartID {token}, {cartID} and {sessionId}", token, cartID, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", cartID);

            using (_logger.BeginTimedOperation("Total time taken for GetProductDetailsFromCartID business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.GetHttpAsyncWithOptions(path, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetProductDetailsFromCartID {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-GetProductDetailsFromCartID {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
            }
        }

        public async Task<(T response, long callDuration)> RegisterOrRemove<T>(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterOrRemove {token}, {action}, {request} and {sessionId}", token, action, request, sessionId);
            IDisposable timer = null;
            string returnValue = string.Empty;
            string path = string.Format("/{0}", action);


            using (timer = _logger.BeginTimedOperation("Total time taken for RegisterOrRemove business call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };


                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterOrRemove {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;
                _logger.LogInformation("CSL service-RegisterOrRemove {requestUrl}, {response} and {sessionId}", responseData.url, responseData.response, sessionId);
            }
            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }

        public async Task<T> GetRegisterTravelers<T>(string token, string sessionId, string jsonRequest)
        {
            IDisposable timer = null;
            string returnValue = string.Empty;
            string path = string.Format("/RegisterTravelers");
            _logger.LogInformation("CSL service-GetRegisterTravelers parameters Request:{@Request} Path:{@Path}", jsonRequest, path);

            using (timer = _logger.BeginTimedOperation("Total time taken for GetRegisterTravelers service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetRegisterTravelers {@RequestUrl} error {@Response}", responseData.url, responseData.response);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetRegisterTravelers {@RequestUrl} {@Response}", responseData.url, responseData.response);
            }

            return (returnValue == null) ? default : JsonConvert.DeserializeObject<T>(returnValue);
        }

        public async Task<T> GetFormsOfPayments<T>(string token, string action, string sessionId, string jsonRequest, Dictionary<string, string> additionalHeaders)
        {
            _logger.LogInformation("CSL service-GetFormsOfPayments {token}, {action}, {jsonRequest} for {sessionId}", token, action, jsonRequest, sessionId);
            IDisposable timer = null;

            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetFormsOfPayments service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                if (_configuration.GetValue<bool>("EnableAdditionalHeadersForMosaicInRFOP"))
                {
                    if (additionalHeaders != null && additionalHeaders.Any())
                    {
                        foreach (var item in additionalHeaders)
                        {
                            headers.Add(item.Key, item.Value);
                        }
                    }
                }

                string path = string.Format("/{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetFormsOfPayments {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetFormsOfPayments {requestUrl}, {response} for {sessionId}", responseData.url, responseData.response, sessionId);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue));
        }

        public async Task<string> CreateCart(string token, string jsonRequest, string sessionId)
        {
            _logger.LogInformation("CSL service-CreateCart {token} {jsonRequest} for {sessionId}", token, jsonRequest, sessionId);
            IDisposable timer = null;
            string returnValue = string.Empty;

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            var responseData = await _resilientClient.PostHttpAsyncWithOptions("", jsonRequest, headers).ConfigureAwait(false);

            if (responseData.statusCode != HttpStatusCode.OK)
            {
                _logger.LogError("CSL service-CreateCart {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                if (responseData.statusCode != HttpStatusCode.BadRequest)
                    return default;
            }
            returnValue = responseData.response;

            _logger.LogInformation("CSL service-CreateCart  {requestUrl}, {response} for {sessionId}", responseData.url, responseData.response, sessionId);

            return JsonConvert.DeserializeObject<string>(returnValue);
        }

        public async Task<T> FareLockReservation<T>(string token, string action, string sessionId, string jsonRequest)
        {
            _logger.LogInformation("CSL service-FareLockReservation {token}, {action}, {jsonRequest} for {sessionId}", token, action, jsonRequest, sessionId);
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for FareLockReservation service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-FareLockReservation {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-FareLockReservation {requestUrl} {response} for {sessionId}", responseData.url, responseData.response, sessionId);
            }

            return JsonConvert.DeserializeObject<T>(returnValue);
        }

        public async Task<(T response, long callDuration)> GetCart<T>(string token, string sessionId, string jsonRequest)
        {
            _logger.LogInformation("CSL service-GetCart  {token}, {request} and {sessionId}", token, jsonRequest, sessionId);
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetCart service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("/{0}");
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetCart {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetCart {requestUrl} {response} for {sessionId}", responseData.url, responseData.response, sessionId);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue), ((TimedOperation)timer).GetElapseTime());
        }

        public async Task<T> GetRegisterSeats<T>(string token, string action, string sessionId, string jsonRequest)
        {
            _logger.LogInformation("CSL service-GetRegisterSeats {token}, {action}, {request} for {sessionId}", token, action, jsonRequest, sessionId);
            IDisposable timer = null;
            string returnValue = string.Empty;

            using (timer = _logger.BeginTimedOperation("Total time taken for GetRegisterSeats service call", transationId: sessionId))
            {
                Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
                string path = string.Format("{0}", action);
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, jsonRequest, headers).ConfigureAwait(false);

                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-GetRegisterSeats {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }
                returnValue = responseData.response;

                _logger.LogInformation("CSL service-GetRegisterSeats  {requestUrl} {response}for {sessionId}", responseData.url, responseData.response, sessionId);
            }

            return (returnValue == null) ? default : (JsonConvert.DeserializeObject<T>(returnValue));
        }

        public async Task<T> RegisterFlights<T>(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service - RegisterFlights parameters Token:{token}, Request:{request}, Action:{action} SessionId:{sessionId} ", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", action);


            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for RegisterFlights business call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-RegisterFlights {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-RegisterFlights {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                    return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
                }

                catch (Exception ex)
                {
                    _logger.LogError("CSL service-RegisterFlights error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }

                return default;
            }
        }

        public async Task<T> RegisterOrRemoveCoupon<T>(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterOrRemoveCoupon  parameters Token:{token},Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", action);


            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for RegisterOrRemoveCoupon business call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-RegisterOrRemoveCoupon {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-RegisterOrRemoveCoupon {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                    return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
                }
                catch (Exception ex)
                {
                    _logger.LogError("CSL service-RegisterOrRemoveCoupon error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }

                return default;


            }
        }

        public async Task<T> RegisterOffers<T>(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterOffers  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };
            string path = string.Format("/{0}", action);

            IDisposable timer = null;
            using (timer = _logger.BeginTimedOperation("Total time taken for RegisterOffers business call", transationId: sessionId))
            {
                try
                {
                    var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                    if (responseData.statusCode != HttpStatusCode.OK)
                    {
                        _logger.LogError("CSL service-RegisterOffers {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                        if (responseData.statusCode != HttpStatusCode.BadRequest)
                            return default;
                    }

                    var CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
                    _logger.LogInformation("CSL service-RegisterOffers {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                    return (responseData.response == null) ? default : JsonConvert.DeserializeObject<T>(responseData.response);
                }

                catch (Exception ex)
                {
                    _logger.LogError("CSL service-RegisterOffers error {stackTrace} for {sessionId}", ex.StackTrace, sessionId);
                }

                return default;
            }
        }

        public async Task<string> RegisterFareLockReservation(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterFareLockReservation  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);


            using (_logger.BeginTimedOperation("Total time taken for RegisterFareLockReservation business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterFareLockReservation {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterFareLockReservation {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> RegisterCheckinSeats(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterCheckinSeats  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);


            using (_logger.BeginTimedOperation("Total time taken for RegisterCheckinSeats business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterCheckinSeats {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterCheckinSeats {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> RegisterBags(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterBags  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for RegisterBags business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterBags {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterBags {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> RegisterSameDayChange(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterSameDayChange  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for RegisterSameDayChange business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterSameDayChange {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterSameDayChange {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> RegisterFormsOfPayments_CFOP(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterFormsOfPayments_CFOP  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for RegisterFormsOfPayments_CFOP business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterFormsOfPayments_CFOP {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterFormsOfPayments_CFOP {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> RegisterSeats_CFOP(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-RegisterSeats_CFOP  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);
            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("/{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for RegisterSeats_CFOP business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-RegisterSeats_CFOP {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-RegisterSeats_CFOP {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }

        public async Task<string> ClearSeats(string token, string action, string request, string sessionId)
        {
            _logger.LogInformation("CSL service-ClearSeats  parameters Token:{token}, Request:{request} Action:{action} SessionId:{sessionId}", token, request, action, sessionId);

            Dictionary<string, string> headers = new Dictionary<string, string>
                     {
                          {"Accept", "application/json"},
                          { "Authorization", token }
                     };

            string path = string.Format("{0}", action);

            using (_logger.BeginTimedOperation("Total time taken for ClearSeats business call", transationId: sessionId))
            {
                var responseData = await _resilientClient.PostHttpAsyncWithOptions(path, request, headers);
                if (responseData.statusCode != HttpStatusCode.OK)
                {
                    _logger.LogError("CSL service-ClearSeats {requestUrl} error {response} for {sessionId}", responseData.url, responseData.response, sessionId);
                    if (responseData.statusCode != HttpStatusCode.BadRequest)
                        return default;
                }

                _logger.LogInformation("CSL service-ClearSeats {requestUrl} {response} and {sessionId}", responseData.url, responseData.response, sessionId);
                return responseData.response;
            }
        }
    }
}
