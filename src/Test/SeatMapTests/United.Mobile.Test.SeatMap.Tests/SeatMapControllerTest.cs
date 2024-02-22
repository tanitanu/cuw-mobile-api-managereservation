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
using United.Mobile.Model.Common;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.SeatMap;
using United.Mobile.SeatMap.Api.Controllers;
using United.Mobile.SeatMap.Domain;
using Xunit;
using Constants = United.Mobile.Model.Constants;

namespace United.Mobile.Test.SeatMap.Tests
{
    public class SeatMapControllerTest
    {
        private readonly Mock<ILogger<SeatMapController>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<ISeatMapBusiness> _seatMapBusiness;
        private readonly SeatMapController _seatMapController;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;

        public SeatMapControllerTest()
        {
            _logger = new Mock<ILogger<SeatMapController>>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            _headers = new Mock<IHeaders>();
            _seatMapBusiness = new Mock<ISeatMapBusiness>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _seatMapController = new SeatMapController(_logger.Object, _configuration, _headers.Object, _seatMapBusiness.Object);
            SetupHttpContextAccessor();
        }
        private void SetupHttpContextAccessor()
        {
            var guid = Guid.NewGuid().ToString();
            var context = new DefaultHttpContext();
            context.Request.Headers[Constants.HeaderAppIdText] = "1";
            context.Request.Headers[Constants.HeaderAppMajorText] = "1";
            context.Request.Headers[Constants.HeaderAppMinorText] = "0";
            context.Request.Headers[Constants.HeaderDeviceIdText] = guid;
            _httpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
        }

        [Fact]
        public void HealthCheck_Test()
        {
            string result = _seatMapController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Theory]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "", "UA", "802", "20211117", "20211118", "20211118", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "", "", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211118", "")]
        public void GetSeatMap_Test(int applicationid, string appVersion, string accessCode, string transactionId, string carrierCode, string flightNumber, string flightDate, string departureAirportCode, string arrivalAirportCode, string languageCode)
        {
            var response = new MOBSeatMapResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _seatMapController.ControllerContext = new ControllerContext();
            _seatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _seatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            _seatMapBusiness.Setup(p => p.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode)).Returns(Task.FromResult(response));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationVersionText].ToString();

            var result = _seatMapController.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode);
            Assert.True(result != null && result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
        }

        [Theory]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "", "UA", "802", "20211117", "20211118", "20211118", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "", "", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211118", "")]
        public void GetSeatMap_Tes_Exception(int applicationid, string appVersion, string accessCode, string transactionId, string carrierCode, string flightNumber, string flightDate, string departureAirportCode, string arrivalAirportCode, string languageCode)
        {
            var response = new MOBSeatMapResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _seatMapController.ControllerContext = new ControllerContext();
            _seatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _seatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            _seatMapBusiness.Setup(p => p.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode)).ThrowsAsync(new System.Exception("United data services are not currently available."));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationVersionText].ToString();

            var result = _seatMapController.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode);
            Assert.True(result != null && result.Result.TransactionId != null);
            Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
        }

        [Theory]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "", "UA", "802", "20211117", "20211118", "20211118", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "802", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "", "20211117", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "", "20211118", "20211119", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "", "", "en-US")]
        [InlineData(2, "4.1.35", "ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "UA", "802", "20211117", "20211118", "20211118", "")]
        public void GetSeatMap_Tes_Exception1(int applicationid, string appVersion, string accessCode, string transactionId, string carrierCode, string flightNumber, string flightDate, string departureAirportCode, string arrivalAirportCode, string languageCode)
        {
            var response = new MOBSeatMapResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _seatMapController.ControllerContext = new ControllerContext();
            _seatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _seatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            _seatMapBusiness.Setup(p => p.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationVersionText].ToString();

            var result = _seatMapController.GetSeatMap(applicationid, appVersion, accessCode, transactionId, carrierCode, flightNumber, flightDate, departureAirportCode, arrivalAirportCode, languageCode);
            Assert.True(result != null && result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void RegisterSeats_Test()
        {
            var response = new MOBRegisterSeatsResponse();
            var request = new MOBRegisterSeatsRequest()
            {
                DeviceId = "7929a579 - 8a75 - 4981 - 84ba - 731c71c6bf02",
                Application = new MOBApplication()
                {
                    Id = 2,
                    Version = new MOBVersion()
                    {
                        Major = "4.1.23"
                    }
                },
                TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44",
                LanguageCode = "en-US",
                SessionId = "5E98C6B880B84E89A8DBFC3C4897B4C5"
            };
            _seatMapBusiness.Setup(p => p.RegisterSeats(request)).Returns(Task.FromResult(response));
            var result = _seatMapController.RegisterSeats(request);
            Assert.True(result.Result != null && result.Result.TransactionId != null);
            Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
        }

        [Fact]
        public void RegisterSeats_Test_Exception()
        {
            var response = new MOBRegisterSeatsResponse();
            var request = new MOBRegisterSeatsRequest()
            {
                DeviceId = "7929a579 - 8a75 - 4981 - 84ba - 731c71c6bf02",
                Application = new MOBApplication()
                {
                    Id = 2,
                    Version = new MOBVersion()
                    {
                        Major = "4.1.23"
                    }
                },
                TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44",
                LanguageCode = "en-US",
                SessionId = "5E98C6B880B84E89A8DBFC3C4897B4C5"
            };
            _seatMapBusiness.Setup(p => p.RegisterSeats(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));
            var result = _seatMapController.RegisterSeats(request);
            Assert.True(result.Result != null && result.Result.Exception.Message.Equals("United data services are not currently available."));
            Assert.True(result.Result.CallDuration > 0);
        }

        [Fact]
        public void RegisterSeats_Test_Exception1()
        {
            var response = new MOBRegisterSeatsResponse();
            var request = new MOBRegisterSeatsRequest()
            {
                DeviceId = "7929a579 - 8a75 - 4981 - 84ba - 731c71c6bf02",
                Application = new MOBApplication()
                {
                    Id = 2,
                    Version = new MOBVersion()
                    {
                        Major = "4.1.23"
                    }
                },
                TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44",
                LanguageCode = "en-US",
                SessionId = "5E98C6B880B84E89A8DBFC3C4897B4C5"
            };
            _seatMapBusiness.Setup(p => p.RegisterSeats(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));
            var result = _seatMapController.RegisterSeats(request);
            Assert.True(result.Result != null && result.Result.Exception.Message.Equals("United data services are not currently available."));
            Assert.True(result.Result.CallDuration > 0);
        }
    }
}
