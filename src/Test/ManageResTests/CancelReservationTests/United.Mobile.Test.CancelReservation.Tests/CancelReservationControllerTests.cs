using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Ebs.Logging.Enrichers;
using United.Mobile.CancelReservation.Api.Controllers;
using United.Mobile.CancelReservation.Domain;
using United.Mobile.Model;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Utility.Helper;
using Xunit;

namespace United.Mobile.Test.CancelReservation.Tests
{
    public class CancelReservationControllerTests
    {
        private readonly Mock<ICacheLog<CancelReservationController>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<ICancelReservationBusiness> _cancelRreservationBusiness;
        private readonly CancelReservationController _cancelReservationController;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRequestEnricher> _requestEnricher;
        private readonly Mock<IFeatureSettings> _featureSettings;
        public CancelReservationControllerTests()
        {
            _logger = new Mock<ICacheLog<CancelReservationController>>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            _headers = new Mock<IHeaders>();
            _cancelRreservationBusiness = new Mock<ICancelReservationBusiness>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _requestEnricher = new Mock<IRequestEnricher>();
            _cancelReservationController = new CancelReservationController(_logger.Object, _configuration, _headers.Object, _cancelRreservationBusiness.Object, _requestEnricher.Object,_featureSettings.Object);
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
            string result = _cancelReservationController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Fact]
        public void CancelRefundInfo_Test()
        {
            var response = new MOBCancelRefundInfoResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
                {
                    SessionId = "0B4D8C69883C46EFB69177D68387BA73",
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelRefundInfo(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelRefundInfo(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void CancelRefundInfo_SystemException_Test()
        {
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelRefundInfo(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelRefundInfo(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void CancelRefundInfo_MOBUnitedException_Test()
        {
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
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
                }
            };
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelRefundInfo(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelRefundInfo(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void CancelAndRefund_Test()
        {
            var response = new MOBCancelAndRefundReservationResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBCancelAndRefundReservationRequest>()
            {
                Data = new MOBCancelAndRefundReservationRequest()
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelAndRefund(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelAndRefund(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void CancelAndRefund_SystemException_Test()
        {
            var request = new Request<MOBCancelAndRefundReservationRequest>()
            {
                Data = new MOBCancelAndRefundReservationRequest()
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelAndRefund(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelAndRefund(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void CancelAndRefund_MOBUnitedException_Test()
        {
            var request = new Request<MOBCancelAndRefundReservationRequest>()
            {
                Data = new MOBCancelAndRefundReservationRequest()
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
                }
            };
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CancelAndRefund(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CancelAndRefund(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void CheckinCancelRefundInfo_Test()
        {
            var response = new MOBCancelRefundInfoResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
                {
                    SessionId = "0B4D8C69883C46EFB69177D68387BA73",
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CheckinCancelRefundInfo(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CheckinCancelRefundInfo(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void CheckinCancelRefundInfo_SystemException_Test()
        {
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
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
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CheckinCancelRefundInfo(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CheckinCancelRefundInfo(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void CheckinCancelRefundInfo_MOBUnitedException_Test()
        {
            var request = new Request<MOBCancelRefundInfoRequest>()
            {
                Data = new MOBCancelRefundInfoRequest()
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
                }
            };
            _cancelReservationController.ControllerContext = new ControllerContext();
            _cancelReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _cancelReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _cancelRreservationBusiness.Setup(p => p.CheckinCancelRefundInfo(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _cancelReservationController.CheckinCancelRefundInfo(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

    }

}

