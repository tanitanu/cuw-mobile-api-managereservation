using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.Model.GooglePay;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Services.GooglePay.Domain;
using United.Utility.Helper;
using United.Utility.Serilog;

namespace United.Mobile.Services.GooglePay.Api.Controllers
{
    [Route("googlepayservice/api")]
    [ApiController]
    public class GooglePayController : ControllerBase
    {
        private readonly ICacheLog<GooglePayController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHeaders _headers;
        private readonly IGooglePayBusiness _googlePayBusiness;

        public GooglePayController(ICacheLog<GooglePayController> logger
            , IConfiguration configuration
            , IHeaders headers
            , IGooglePayBusiness googlePayBusiness
            )
        {
            _logger = logger;
            _configuration = configuration;
            _headers = headers;
            _googlePayBusiness = googlePayBusiness;
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

        [HttpPost]
        [Route("GooglePay/InsertFlight")]
        public async Task<MOBGooglePayFlightResponse> InsertFlight(MOBGooglePayFlightRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBGooglePayFlightResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("InsertFlight {@clientRequest} {DeviceId} {SessionId} {TransactionId}", JsonConvert.SerializeObject(request), request.DeviceId, request.TransactionId, request.TransactionId);

                timer = _logger.BeginTimedOperation("Total time taken for InsertFlight business call", transationId: request.TransactionId);

                response = await _googlePayBusiness.InsertFlight(request);
            }
            catch (Exception ex)
            {
                MOBExceptionWrapper exceptionWrapper = new MOBExceptionWrapper(ex);
                _logger.LogError("GooglePay_InsertFlight Error {Exception}", JsonConvert.SerializeObject(exceptionWrapper));
                _logger.LogError("GooglePay_InsertFlight Error {Exception} and {transactionId}", ex.Message, request.TransactionId);

                response.Exception = (!_configuration.GetValue<bool>("SurfaceErrorToClient")) ?
                new MOBException("9999", _configuration.GetValue<String>("Booking2OGenericExceptionMessage")) :
                new MOBException("9999", "TransactionId :" + response.TransactionId + ex.Message);
            }
            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("GooglePay_InsertFlight {@clientResponse} {transactionId}", JsonConvert.SerializeObject(response), request.TransactionId);

            return response;
        }

        [HttpPost]
        [Route("GooglePay/UpdateFlightFromRequest")]
        public async Task<MOBGooglePayFlightResponse> UpdateFlightFromRequest(MOBGooglePayFlightRequest request)
        {
            await _headers.SetHttpHeader(request.DeviceId, request.Application.Id.ToString(), request.Application.Version.Major, request.TransactionId, request.LanguageCode, string.Empty);
            var response = new MOBGooglePayFlightResponse();
            IDisposable timer = null;
            try
            {
                _logger.LogInformation("UpdateFlightFromRequest {@clientRequest} {DeviceId} {SessionId} {TransactionId}", JsonConvert.SerializeObject(request), request.DeviceId, request.TransactionId, request.TransactionId);

                timer = _logger.BeginTimedOperation("Total time taken for UpdateFlightFromRequest business call", transationId: request.TransactionId);

                response = await _googlePayBusiness.UpdateFlightFromRequest(request);
            }
            catch (MOBUnitedException coex)
            {
                _logger.LogWarning("UpdateFlightFromRequest Warning {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(coex), request.TransactionId);
                _logger.LogWarning("UpdateFlightFromRequest Warning {exception} and {transactionId}", coex.Message, request.TransactionId);
                response.Exception = new MOBException();
                response.Exception.Message = coex.Message;
            }
            catch (Exception ex)
            {
                _logger.LogError("UpdateFlightFromRequest Error {exceptionstack} and {transactionId}", JsonConvert.SerializeObject(ex), request.TransactionId);
                _logger.LogError("UpdateFlightFromRequest Error {exception} and {transactionId}", ex.Message, request.TransactionId);

                response.Exception = new MOBException("9999", _configuration.GetValue<string>("GenericExceptionMessage"));
            }
            response.CallDuration = (timer != null) ? ((TimedOperation)timer).GetElapseTime() : 0;
            _logger.LogInformation("UpdateFlightFromRequest {@clientResponse} {transactionId}", JsonConvert.SerializeObject(response), request.TransactionId);

            return response;
        }
    }
}
