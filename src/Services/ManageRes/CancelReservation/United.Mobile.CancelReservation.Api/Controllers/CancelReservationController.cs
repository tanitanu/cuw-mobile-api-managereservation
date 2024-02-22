using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Ebs.Logging.Enrichers;
using United.Mobile.CancelReservation.Domain;
using United.Mobile.Model;
using United.Mobile.Model.Common;
using United.Mobile.Model.Common.FeatureSettings;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.CancelReservation.Api.Controllers
{
    [Route("mrcancelreservationservice/api")]
    [ApiController]
    public class CancelReservationController : ControllerBase
    {
        private readonly ICacheLog<CancelReservationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly ICancelReservationBusiness _cancelReservationBusiness;
        private readonly IRequestEnricher _requestEnricher;
        public readonly string Namespace = typeof(Program).Namespace;
        private readonly IFeatureSettings _featureSettings;
        public CancelReservationController(ICacheLog<CancelReservationController> logger, IConfiguration configuration,
            IHeaders headers, ICancelReservationBusiness cancelReservationBusiness, IRequestEnricher requestEnricher, IFeatureSettings featureSettings)
        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _cancelReservationBusiness = cancelReservationBusiness;
            _featureSettings = featureSettings;
            _requestEnricher = requestEnricher;
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
        [Route("CancelReservation/CheckinCancelRefundInfo")]
        public async Task<MOBCancelRefundInfoResponse> CheckinCancelRefundInfo(MOBCancelRefundInfoRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);
            var response = new MOBCancelRefundInfoResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("CheckinCancelRefundInfo {@ClientRequest}", JsonConvert.SerializeObject(request));
                using (timer = _logger.BeginTimedOperation("Total time taken for CheckinCancelRefundInfo business call", transationId: request.TransactionId))
                {
                    response = await _cancelReservationBusiness.CheckinCancelRefundInfo(request);
                }
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("CheckinCancelRefundInfo Warning {@UnitedException} and {exceptionstack}", coex.Message, JsonConvert.SerializeObject(coex));
                MOBException mOBException = new MOBException();
                response.Exception = mOBException;
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("CheckinCancelRefundInfo Error {exceptionstack} and {exception}", JsonConvert.SerializeObject(ex), ex.Message);

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }
            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("CheckinCancelRefundInfo {@clientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }



        [HttpPost]
        [Route("CancelReservation/CancelRefundInfo")]
        public async Task<MOBCancelRefundInfoResponse> CancelRefundInfo(MOBCancelRefundInfoRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBCancelRefundInfoResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("CancelRefundInfo - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for CancelRefundInfo business call", transationId: request.TransactionId))
                {
                    response = await _cancelReservationBusiness.CancelRefundInfo(request);
                }
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("CancelRefundInfo Warning {exception} {exceptionstack}", coex.Message,JsonConvert.SerializeObject(coex));

                if (coex != null && !string.IsNullOrEmpty(coex.Message.Trim()))
                {
                    response.Exception = new MOBException();
                    response.Exception.Message = coex.Message;
                }
                else
                {
                    response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("CancelRefundInfo Error {exception} {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));

                if (!Convert.ToBoolean(_configuration.GetValue<string>("SurfaceErrorToClient")))
                {
                    response.Exception = new MOBException("9999", _configuration.GetValue<string>("Booking2OGenericExceptionMessage"));
                }
                else
                {
                    response.Exception = new MOBException("9999", ex.Message);
                }
            }
            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("CancelRefundInfo {@clientResponse}", JsonConvert.SerializeObject(response));

            return response;
        }

        [HttpPost]
        [Route("CancelReservation/CancelAndRefund")]
        public async Task<MOBCancelAndRefundReservationResponse> CancelAndRefund(MOBCancelAndRefundReservationRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, request.SessionId);

            var response = new MOBCancelAndRefundReservationResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("CancelAndRefund - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));

                using (timer = _logger.BeginTimedOperation("Total time taken for CancelAndRefund business call", transationId: request.TransactionId))
                {
                    response = await _cancelReservationBusiness.CancelAndRefund(request);
                }
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("CancelAndRefund Warning {exception} {exceptionstack}", coex.Message, JsonConvert.SerializeObject(coex));
                MOBException mOBException = new MOBException();
                response.Exception = mOBException;
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("CancelAndRefund Error {exception} {exceptionstack}", ex.Message, JsonConvert.SerializeObject(ex));

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }
            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("CancelAndRefund {@clientResponse}", JsonConvert.SerializeObject(response));

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
                request.ServiceName = ServiceNames.CANCELRESERVATION.ToString();
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
