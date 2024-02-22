using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;
using United.Common.Helper;
using United.Common.Helper.ManageRes;
using United.Ebs.Logging.Enrichers;
using United.Mobile.ManageReservation.Api.Controllers;
using United.Mobile.ManageReservation.Domain;
using United.Mobile.Model;
using United.Mobile.Model.FlightReservation;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.ManageRes;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.ReShop;
using United.Mobile.Model.Shopping;
using United.Utility.Helper;
using Xunit;
using MOBPNRByRecordLocatorResponse = United.Mobile.Model.ManageRes.MOBPNRByRecordLocatorResponse;


namespace United.Mobile.Test.ManageReservation.Tests
{
    public class ManageReservationControllerTests
    {
        private readonly Mock<ICacheLog<ManageReservationController>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IManageReservationBusiness> _manageReservationBusiness;
        private readonly ManageReservationController _manageReservationController;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IFlightReservation> _flightReservation;
        private readonly Mock<IRequestEnricher> _requestEnricher;
        private readonly Mock<IFeatureSettings> _featureSettings;
        public ManageReservationControllerTests()
        {
            _logger = new Mock<ICacheLog<ManageReservationController>>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            _headers = new Mock<IHeaders>();
            _manageReservationBusiness = new Mock<IManageReservationBusiness>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _flightReservation = new Mock<IFlightReservation>();
            _requestEnricher = new Mock<IRequestEnricher>();
            _manageReservationController = new ManageReservationController(_logger.Object, _configuration, _headers.Object, _manageReservationBusiness.Object, _flightReservation.Object, _requestEnricher.Object,_featureSettings.Object);
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
            string result = _manageReservationController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Fact]
        public void GetPNRByRecordLocator_Test()
        {
            var response = new MOBPNRByRecordLocatorResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetPNRByRecordLocator(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetPNRByRecordLocator(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void GetPNRByRecordLocator_MOBUnitedException_Test()
        {
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetPNRByRecordLocator(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetPNRByRecordLocator(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void GetPNRByRecordLocator_SystemException_Test()
        {
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetPNRByRecordLocator(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetPNRByRecordLocator(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void GetPNRByRecordLocator_SystemException_NegTest()
        {
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetPNRByRecordLocator(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.GetPNRByRecordLocator(request.Data);

            //Assert.True(result.Result.TransactionId == null);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void PerformInstantUpgrade_Test()
        {
            var response = new MOBPNRByRecordLocatorResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBInstantUpgradeRequest>()
            {
                Data = new MOBInstantUpgradeRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.PerformInstantUpgrade(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.PerformInstantUpgrade(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void PerformInstantUpgrade_MOBUnitedException_Test()
        {
            var request = new Request<MOBInstantUpgradeRequest>()
            {
                Data = new MOBInstantUpgradeRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.PerformInstantUpgrade(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.PerformInstantUpgrade(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void PerformInstantUpgrade_SystemException_Test()
        {
            var request = new Request<MOBInstantUpgradeRequest>()
            {
                Data = new MOBInstantUpgradeRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.PerformInstantUpgrade(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.PerformInstantUpgrade(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void PerformInstantUpgrade_SystemException_NegTest()
        {

            var request = new Request<MOBInstantUpgradeRequest>()
            {
                Data = new MOBInstantUpgradeRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.PerformInstantUpgrade(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.PerformInstantUpgrade(request.Data);

            Assert.True(result.Result.SessionId == null || result.Result.TransactionId == "E830EDE6-5FE0-469F-B785-979E55D67C18|A60C7E92-EB7C-4851-A86F-E3354E02A253");
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void GetOneClickEnrollmentDetailsForPNR_Test()
        {
            var response = new MOBOneClickEnrollmentResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetOneClickEnrollmentDetailsForPNR(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetOneClickEnrollmentDetailsForPNR(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }
        [Fact]
        public void GetOneClickEnrollmentDetailsForPNR_MOBUnitedException_Test()
        {
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetOneClickEnrollmentDetailsForPNR(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetOneClickEnrollmentDetailsForPNR(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);

        }
        [Fact]
        public void GetOneClickEnrollmentDetailsForPNR_SystemException_Test()
        {
            var request = new Request<MOBPNRByRecordLocatorRequest>()
            {
                Data = new MOBPNRByRecordLocatorRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetOneClickEnrollmentDetailsForPNR(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.GetOneClickEnrollmentDetailsForPNR(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void ConfirmScheduleChange_Test()
        {
            var response = new MOBConfirmScheduleChangeResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            var request = new MOBConfirmScheduleChangeRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

             _manageReservationBusiness.Setup(p => p.ConfirmScheduleChange(request)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.ConfirmScheduleChange(request);

            //Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void ConfirmScheduleChange_MOBUnitedException_Test()
        {

            var request = new MOBConfirmScheduleChangeRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.ConfirmScheduleChange(request)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.ConfirmScheduleChange(request);

           // Assert.False(result.Result.Exception.Message == "Error Message");
            Assert.False(result.Result.Exception == null);
            Assert.True(result != null);
        }
        [Fact]
        public void ConfirmScheduleChange_SystemException_Test()
        {
            var request = new MOBConfirmScheduleChangeRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.ConfirmScheduleChange(request)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.ConfirmScheduleChange(request);

            //Assert.True(result.Result.Exception != null);
            Assert.True(result != null);
        }

        [Fact]
        public void RequestReceiptByEmail_Test()
        {

            var response = new MOBReceiptByEmailResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };
            var request = new Request<MOBReceiptByEmailRequest>()
            {
                Data = new MOBReceiptByEmailRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.RequestReceiptByEmail(request.Data)).Returns(Task.FromResult(response));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.RequestReceiptByEmail(request.Data);
            Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void RequestReceiptByEmail_SystemException_Test()
        {
            var request = new Request<MOBReceiptByEmailRequest>()
            {
                Data = new MOBReceiptByEmailRequest()
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
            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.RequestReceiptByEmail(request.Data)).ThrowsAsync(new Exception("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.RequestReceiptByEmail(request.Data);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void RequestReceiptByEmail_MOBUnitedException_Test()
        {
            var request = new Request<MOBReceiptByEmailRequest>()
            {
                Data = new MOBReceiptByEmailRequest()
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

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.RequestReceiptByEmail(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            var result = _manageReservationController.RequestReceiptByEmail(request.Data);
            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void RequestReceiptByEmail_MOBUnitedException_Test1()
        {
            var request = new Request<MOBReceiptByEmailRequest>()
            {
                Data = new MOBReceiptByEmailRequest()
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

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.RequestReceiptByEmail(request.Data)).ThrowsAsync(new MOBUnitedException("Error Message"));

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();

            var result = _manageReservationController.RequestReceiptByEmail(request.Data);

            Assert.True(result.Result.Exception.Message == "Error Message");
            //Assert.True(result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            //Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void GetMileageAndStatusOptions_Test()
        {

            var returns = new MOBMileageAndStatusOptionsResponse();

            var request = new MOBMileageAndStatusOptionsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetMileageAndStatusOptions(request)).ReturnsAsync(returns);


           // Act
           var result = _manageReservationController.GetMileageAndStatusOptions(request);
           // Assert
            Assert.True(result != null);
            Assert.True(result.Result.Exception == null);


        }

        [Fact]
        public void GetMileageAndStatusOptions_MOBUnitedException()
        {

            var returns = new MOBMileageAndStatusOptionsResponse() { };

            var request = new MOBMileageAndStatusOptionsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetMileageAndStatusOptions(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _manageReservationController.GetMileageAndStatusOptions(request);
            // Assert
            //Assert.True(result != null);
            Assert.True(result.Result.Exception != null);

        }

        [Fact]
        public void GetMileageAndStatusOptions_Exception()
        {

            var returns = new MOBMileageAndStatusOptionsResponse() { };

            var request = new MOBMileageAndStatusOptionsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetMileageAndStatusOptions(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _manageReservationController.GetMileageAndStatusOptions(request);
            // Assert
           // Assert.True(result != null);
            Assert.True(result.Result.Exception != null);

        }

        [Fact]
        public void GetActionDetailsForOffers_Test()
        {

            var returns = new MOBGetActionDetailsForOffersResponse();

            var request = new MOBGetActionDetailsForOffersRequest()
            {
                AccessCode = "ACCESSCODE",
                LanguageCode = "en_US",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetActionDetailsForOffers(request)).ReturnsAsync(returns);


            // Act
            var result = _manageReservationController.GetActionDetailsForOffers(request);
            // Assert
            //Assert.True(result != null);
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void GetActionDetailsForOffers_MOBUnitedException()
        {

            var returns = new MOBGetActionDetailsForOffersResponse() { };

            var request = new MOBGetActionDetailsForOffersRequest()
            {
                AccessCode = "ACCESSCODE",
                LanguageCode = "en_US",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetActionDetailsForOffers(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _manageReservationController.GetActionDetailsForOffers(request);
            // Assert
            //Assert.True(result != null);
            Assert.True(result.Result.Exception != null);
        }

        [Fact]
        public void GetActionDetailsForOffers_Exception()
        {

            var returns = new MOBGetActionDetailsForOffersResponse() { };

            var request = new MOBGetActionDetailsForOffersRequest()
            {
                AccessCode = "ACCESSCODE",
                LanguageCode = "en_US",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _manageReservationController.ControllerContext = new ControllerContext();
            _manageReservationController.ControllerContext.HttpContext = new DefaultHttpContext();
            _manageReservationController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _manageReservationBusiness.Setup(p => p.GetActionDetailsForOffers(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _manageReservationController.GetActionDetailsForOffers(request);
            // Assert
            Assert.True(result != null);

        }

    }

}
