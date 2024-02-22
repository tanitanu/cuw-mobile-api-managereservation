using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Ebs.Logging.Enrichers;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.PreOrderMeals;
using United.Mobile.Model.Shopping;
using United.Mobile.Services.PreOrderMeals.Domain;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.Services.PreOrderMeals.Api.Controllers
{
    [Route("preordermealsservice/api")]
    [ApiController]
    public class PreOrderMealsController : ControllerBase
    {

        private readonly ICacheLog<PreOrderMealsController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly IPreOrderMealsBusiness _preOrderMealsBusiness;
        private readonly IRequestEnricher _requestEnricher;
        public readonly string Namespace = typeof(Program).Namespace;
        private readonly IFeatureSettings _featureSettings;
        public PreOrderMealsController(ICacheLog<PreOrderMealsController> logger
            ,IConfiguration configuration
            ,IHeaders headers
            ,IPreOrderMealsBusiness preOrderMealsBusiness
            ,IRequestEnricher requestEnricher
            , IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _preOrderMealsBusiness = preOrderMealsBusiness;
            _requestEnricher = requestEnricher;
            _featureSettings = featureSettings;
            _requestEnricher.Add("Application", Namespace);
        }

        [HttpGet]
        [Route("HealthCheck")]
        public string HealthCheck()
        {
            return "Healthy";
        }

        [HttpGet]
        [Route("version")]
        public virtual string Version()
        {
            string serviceVersionNumber = null;

            try
            {
                serviceVersionNumber = Environment.GetEnvironmentVariable("SERVICE_VERSION_NUMBER");
            }
            catch
            {
                // Suppress any exceptions
            }
            finally
            {
                serviceVersionNumber = (null == serviceVersionNumber) ? "0.0.0" : serviceVersionNumber;
            }

            return serviceVersionNumber;
        }
        [HttpGet]
        [Route("environment")]
        public virtual string ApiEnvironment()
        {
            try
            {
                return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            }
            catch
            {
            }
            return "Unknown";
        }
        [HttpPost]
        [Route("PreOrderMeals/GetInflightMealOffers")]
        public async Task<MOBInFlightMealsOfferResponse> GetInflightMealOffers(MOBInFlightMealsOfferRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBInFlightMealsOfferResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetInflightMealOffers - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetInflightMealOffers business call", transationId: request.TransactionId))
                {
                    response = await _preOrderMealsBusiness.GetInflightMealOffers(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetInflightMealOffers Warning {Exception} and {exceptionstack}",uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetInflightMealOffers Error {Exception} and {exceptionstack}",ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("10000",
                    _configuration.GetValue<string>("PreOrderMealMealAvailableUnhandledErrorMessage"));
            }
            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetInflightMealOffers {@clientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/GetInflightMealOffersForDeeplink")]
        public async Task<MOBInFlightMealsOfferResponse> GetInflightMealOffersForDeeplink(MOBInFlightMealsOfferRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBInFlightMealsOfferResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetInflightMealOffersForDeeplink - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetInflightMealOffersForDeeplink business call", transationId: request.TransactionId))
                {
                    response = await _preOrderMealsBusiness.GetInflightMealOffersForDeeplink(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetInflightMealOffersForDeeplink MOBUnitedException {Exception} and {exceptionstack}",uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));

            }
            catch (Exception ex)
            {
                _logger.LogError("GetInflightMealOffersForDeeplink Error {Exception} and {exceptionstack}",ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetInflightMealOffersForDeeplink {@clientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/GetInflightMealRefreshments")]
        public async Task<MOBInFlightMealsRefreshmentsResponse> GetInflightMealRefreshments(MOBInFlightMealsRefreshmentsRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBInFlightMealsRefreshmentsResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("GetInflightMealRefreshments - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for GetInflightMealRefreshments business call", transationId: request.TransactionId))
                {
                    response = await _preOrderMealsBusiness.GetInflightMealRefreshments(request);
                }
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetInflightMealRefreshments MOBUnitedException {Exception} and {exceptionstack}",uaex.Message, JsonConvert.SerializeObject(uaex));
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetInflightMealRefreshments Error {Exception} and {exceptionstack}",ex.Message, JsonConvert.SerializeObject(ex));
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetInflightMealRefreshments {@clientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/AddToCart")]
        public async Task<PreOrderMealCartResponse> AddToCart(PreOrderMealCartRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new PreOrderMealCartResponse();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for AddToCart business call", transationId: request.TransactionId);
                response = await _preOrderMealsBusiness.AddToCart(request);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("AddToCart MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("AddToCart Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("AddToCart {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/AddToCartV2")]
        public async Task<PreOrderMealCartResponse> AddToCartV2(PreOrderMealCartRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new PreOrderMealCartResponse();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for AddToCartV2 business call", transationId: request.TransactionId);
                response = await _preOrderMealsBusiness.AddToCartV2(request);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("AddToCartV2 MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("AddToCartV2 Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("AddToCartV2 {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/GetTripsForPerOrderMeal")]
        public async Task<PreOrderMealResponseContext> GetTripsForPerOrderMeal(MOBPNRByRecordLocatorRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new PreOrderMealResponseContext();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for GetTripsForPerOrderMeal business call", transationId: request.TransactionId);
                response = await _preOrderMealsBusiness.GetTripsForPerOrderMeal(request);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetTripsForPerOrderMeal MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId); 
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetTripsForPerOrderMeal Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetTripsForPerOrderMeal {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }

        [HttpPost]
        [Route("PreOrderMeals/GetTripsForPreOrderMealV2")]
        public async Task<PreOrderMealResponseContext> GetTripsForPreOrderMealV2(MOBPNRByRecordLocatorRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new PreOrderMealResponseContext();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for GetTripsForPreOrderMealV2 business call", transationId: request.TransactionId);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetTripsForPreOrderMealV2 MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetTripsForPreOrderMealV2 Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetTripsForPreOrderMealV2 {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }
        //confirm FromUri to FromRoute/FromQuery
        [HttpGet]
        [Route("PreOrderMeals/GetAvailableMeals")]
        public async Task<MealsDetailResponse> GetAvailableMeals([FromRoute] PreOrderMealListRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MealsDetailResponse();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for GetAvailableMeals business call", transationId: request.TransactionId);
                response = await _preOrderMealsBusiness.GetAvailableMeals(request);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetAvailableMeals MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAvailableMeals Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetAvailableMeals {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }

        [HttpGet]
        [Route("PreOrderMeals/GetAvailableMealsV2")]
        public async Task<MealsDetailResponse> GetAvailableMealsV2([FromRoute] PreOrderMealListRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MealsDetailResponse();
            IDisposable timer = null;
            try
            {
                timer = _logger.BeginTimedOperation("Total time taken for GetAvailableMealsV2 business call", transationId: request.TransactionId);
                response = await _preOrderMealsBusiness.GetAvailableMealsV2(request);
            }
            catch (MOBUnitedException uaex)
            {
                _logger.LogWarning("GetAvailableMealsV2 MOBUnitedException {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(uaex), request.SessionId);
                response.Exception = new MOBException("9999", !string.IsNullOrEmpty(uaex?.Message) ? uaex.Message : _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
            }
            catch (Exception ex)
            {
                _logger.LogError("GetAvailableMealsV2 Error {exceptionstack} and {sessionId}", JsonConvert.SerializeObject(ex), request.SessionId);
                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }

            response.CallDuration = 0;
            if (timer != null)
            {
                response.CallDuration = ((TimedOperation)timer).GetElapseTime();
                timer.Dispose();
            }
            _logger.LogInformation("GetAvailableMealsV2 {@clientResponse} {sessionId}", JsonConvert.SerializeObject(response), request.SessionId);

            return response;
        }
        [HttpGet("GetFeatureSettings")]
        public GetFeatureSettingsResponse GetFeatureSettings()
        {
            GetFeatureSettingsResponse response = new GetFeatureSettingsResponse();
            try
            {
                response = _featureSettings.GetFeatureSettings();
            }
            catch (Exception ex)
            {
                _logger.LogError("GetFeatureSettings Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), "GetFeatureSettings_TransId");
                response.Exception = new MOBException("9999", JsonConvert.SerializeObject(ex));
            }
            return response;
        }
        [HttpPost("RefreshFeatureSettingCache")]
        public async Task<MOBResponse> RefreshFeatureSettingCache(MOBFeatureSettingsCacheRequest request)
        {
            MOBResponse response = new MOBResponse();
            try
            {
                request.ServiceName = ServiceNames.PREORDERMEALS.ToString();
                await _featureSettings.RefreshFeatureSettingCache(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("RefreshFeatureSettingCache Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), "RefreshRetrieveAllFeatureSettings_TransId");
                response.Exception = new MOBException("9999", JsonConvert.SerializeObject(ex));
            }
            return response;
        }
    }
}
