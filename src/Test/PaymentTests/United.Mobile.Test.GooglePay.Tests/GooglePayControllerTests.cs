using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Mobile.Model;
using United.Mobile.Model.GooglePay;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Services.GooglePay.Api.Controllers;
using United.Mobile.Services.GooglePay.Domain;
using United.Utility.Helper;
using Xunit;

namespace United.Mobile.Test.GooglePay.Tests
{
    public class GooglePayControllerTests
    {
            private readonly Mock<ICacheLog<GooglePayController>> _logger;
            private readonly IConfiguration _configuration;
            private readonly Mock<IHeaders> _headers;
            private readonly Mock<IGooglePayBusiness> _googlePayBusiness;
            private readonly GooglePayController _googlePayController;
            private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

            public GooglePayControllerTests()
            {
                _logger = new Mock<ICacheLog<GooglePayController>>();
                _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
                .Build();
                _headers = new Mock<IHeaders>();
                _googlePayBusiness = new Mock<IGooglePayBusiness>();
                _httpContextAccessor = new Mock<IHttpContextAccessor>();
                _googlePayController = new GooglePayController(_logger.Object, _configuration, _headers.Object, _googlePayBusiness.Object);
                SetupHttpContextAccessor();
                SetHeaders();

            }

            private void SetupHttpContextAccessor()
            {
                var guid = Guid.NewGuid().ToString();
                var context = new DefaultHttpContext();
                context.Request.Headers[Constants.HeaderAppIdText] = "1";
                context.Request.Headers[Constants.HeaderAppMajorText] = "1";
                context.Request.Headers[Constants.HeaderAppMinorText] = "0";
                context.Request.Headers[Constants.HeaderDeviceIdText] = guid;
                context.Request.Headers[Constants.HeaderLangCodeText] = "en-us";
                context.Request.Headers[Constants.HeaderRequestTimeUtcText] = DateTime.UtcNow.ToString();
                context.Request.Headers[Constants.HeaderTransactionIdText] = guid;
                _httpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            }

            private void SetHeaders(string deviceId = "589d7852-14e7-44a9-b23b-a6db36657579"
           , string applicationId = "2"
           , string appVersion = "4.1.29"
           , string transactionId = "589d7852-14e7-44a9-b23b-a6db36657579|8f46e040-a200-495c-83ca-4fca2d7175fb"
           , string languageCode = "en-US"
           , string sessionId = "17C979E184CC495EA083D45F4DD9D19D")
            {
            _headers.Setup(_ => _.ContextValues).Returns(
       new HttpContextValues
       {
           Application = new Application()
           {
               Id = Convert.ToInt32(applicationId),
               Version = new Mobile.Model.Version
               {
                   Major = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(0, 1)),
                   Minor = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(2, 1)),
                   Build = string.IsNullOrEmpty(appVersion) ? 0 : int.Parse(appVersion.ToString().Substring(4, 2))
               }
           },
           DeviceId = deviceId,
           LangCode = languageCode,
           TransactionId = transactionId,
           SessionId = sessionId
       });
        }

            [Fact]
            public void HealthCheck_Test()
            {
                string result = _googlePayController.HealthCheck();
                Assert.True(result == "Healthy");
            }

        [Fact]
        public void InsertFlight_Test()
        {
            var response = new MOBGooglePayFlightResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    },
                    TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44"
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.InsertFlight(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.InsertFlight(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
            Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void InsertFlight_SystemException_Test()
        {
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    }
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.InsertFlight(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.InsertFlight(request.Data);
            Assert.True(result.Result.Exception.Code == "9999");
            Assert.True(result.Result.CallDuration > 0);
        }
        [Fact]
        public void InsertFlight_MOBUnitedException_Test()
        {
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    }
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.InsertFlight(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.InsertFlight(request.Data);
            Assert.True(result.Result.Exception.Code == "9999");
            Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void UpdateFlightFromRequest_Test()
        {
            var response = new MOBGooglePayFlightResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    },
                    TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44"
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.UpdateFlightFromRequest(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.UpdateFlightFromRequest(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
            Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void UpdateFlightFromRequest_SystemException_Test()
        {
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    }
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.UpdateFlightFromRequest(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.UpdateFlightFromRequest(request.Data);
            Assert.True(result.Result.Exception.Code == "9999");
            Assert.True(result.Result.CallDuration > 0);
        }
        [Fact]
        public void UpdateFlightFromRequest_MOBUnitedException_Test()
        {
            var request = new Request<MOBGooglePayFlightRequest>()
            {
                Data = new MOBGooglePayFlightRequest()
                {
                    Application = new MOBApplication()
                    {
                        Id = 1,
                        Version = new MOBVersion()
                        {
                            Major = "4.1.25",
                            Minor = "4.1.25"
                        }
                    }
                }
            };
            _googlePayController.ControllerContext = new ControllerContext();
            _googlePayController.ControllerContext.HttpContext = new DefaultHttpContext();
            _googlePayController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _googlePayBusiness.Setup(p => p.UpdateFlightFromRequest(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _googlePayController.UpdateFlightFromRequest(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            Assert.True(result.Result.CallDuration > 0);
        }
    }
}
