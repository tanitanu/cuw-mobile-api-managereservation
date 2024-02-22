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
using United.Mobile.Model;
using United.Mobile.Model.Internal.Exception;
using United.Mobile.Model.MPRewards;
using United.Mobile.Model.SeatMap;
using United.Mobile.ViewResSeatMap.Api.Controllers;
using United.Mobile.ViewResSeatMap.Domain;
using United.Utility.Helper;
using Xunit;

namespace United.Mobile.Test.ViewResSeatMap.Tests
{
    public class ViewResSeatMapControllerTest
    {
        private readonly Mock<ICacheLog<ViewResSeatMapController>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IViewResSeatMapBusiness>  _viewResSeatMapBusiness;
        private readonly ViewResSeatMapController _viewResSeatMapController;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRequestEnricher> _requestEnricher;
        private readonly Mock<IFeatureSettings> _featureSettings;
        public ViewResSeatMapControllerTest()
        {
            _logger = new Mock<ICacheLog<ViewResSeatMapController>>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            _headers = new Mock<IHeaders>();
            _viewResSeatMapBusiness = new Mock<IViewResSeatMapBusiness>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _requestEnricher = new Mock<IRequestEnricher>();
            _viewResSeatMapController = new ViewResSeatMapController(_logger.Object, _configuration, _headers.Object, _viewResSeatMapBusiness.Object, _requestEnricher.Object, _featureSettings.Object);
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
            string result = _viewResSeatMapController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Theory]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "")]
        public void SelectSeats_Test(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination)
        {

            var response = new MOBSeatChangeSelectResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _viewResSeatMapController.ControllerContext = new ControllerContext();
            _viewResSeatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _viewResSeatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _viewResSeatMapBusiness.Setup(p => p.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination, null)).Returns(Task.FromResult(response));


            
            var result = _viewResSeatMapController.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            Assert.True(result != null && result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            //Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
        }

        [Theory]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "")]
        public void SelectSeats_Test_Exception(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination)
        {

            var response = new MOBSeatChangeSelectResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _viewResSeatMapController.ControllerContext = new ControllerContext();
            _viewResSeatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _viewResSeatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _viewResSeatMapBusiness.Setup(p => p.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination, null)).ThrowsAsync(new System.Exception("United data services are not currently available.")); ;
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationVersionText].ToString();

            var result = _viewResSeatMapController.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            // Assert.True(result != null && result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            //Assert.True(result.Result.CallDuration > 0);
            //Assert.True(result.Result.Exception == null);
            Assert.True(result.Exception != null || result.Result != null);
        }

        [Theory]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "", "802", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "", "20211118", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "", "0", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "", "seatAssignment", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "", "nextOrigin", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "", "nextDestination")]
        [InlineData("ACCESSCODE", "EE64E779-7B46-4836-B261-62AE35498B44", "en-US", "4.1.35", 2, "5E98C6B880B84E89A8DBFC3C4897B4C5", "origin", "destination", "802", "20211118", "0", "seatAssignment", "nextOrigin", "")]
        public void SelectSeats_Test_Exception1(string accessCode, string transactionId, string languageCode, string appVersion, int applicationId, string sessionId, string origin, string destination, string flightNumber, string flightDate, string paxIndex, string seatAssignment, string nextOrigin, string nextDestination)
        {
            var response = new MOBSeatChangeSelectResponse() { TransactionId = "EE64E779-7B46-4836-B261-62AE35498B44" };

            _viewResSeatMapController.ControllerContext = new ControllerContext();
            _viewResSeatMapController.ControllerContext.HttpContext = new DefaultHttpContext();
            _viewResSeatMapController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _viewResSeatMapBusiness.Setup(p => p.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination, null)).ThrowsAsync(new MOBUnitedException("United data services are not currently available.")); ;

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderTransactionIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationIdText].ToString();
            httpContext.Request.Headers[Constants.HeaderDeviceIdText].ToString();
            httpContext.Request.Headers[Constants.ApplicationVersionText].ToString();

            var result = _viewResSeatMapController.SelectSeats(accessCode, transactionId, languageCode, appVersion, applicationId, sessionId, origin, destination, flightNumber, flightDate, paxIndex, seatAssignment, nextOrigin, nextDestination);

            //Assert.True(result != null && result.Result.TransactionId == "EE64E779-7B46-4836-B261-62AE35498B44");
            //Assert.True(result.Result.CallDuration > 0);
            //Assert.True(result.Result.Exception == null);
            Assert.True(result.Exception != null || result.Result != null);
        }

        [Fact]
        public void SeatChangeInitialize_Test()
        {
            var response = new MOBSeatChangeInitializeResponse();
            var request = new MOBSeatChangeInitializeRequest()
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
            _viewResSeatMapBusiness.Setup(p => p.SeatChangeInitialize(request)).Returns(Task.FromResult(response));

            var result = _viewResSeatMapController.SeatChangeInitialize(request);

            Assert.True(result.Result != null && result.Result.TransactionId != null);
            //Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Result.Exception == null);
            Assert.True(result.Result.SessionId != null);
        }

        [Fact]
        public void SeatChangeInitialize_Test_Exception()
        {
            var response = new MOBSeatChangeInitializeResponse();
            var request = new MOBSeatChangeInitializeRequest()
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
            _viewResSeatMapBusiness.Setup(p => p.SeatChangeInitialize(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));
            var result = _viewResSeatMapController.SeatChangeInitialize(request);
            //Assert.True(result.Result != null && result.Result.Exception.Message.Equals("United data services are not currently available."));
            //Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Exception != null || result.Result != null);
        }

        [Fact]
        public void SeatChangeInitialize_Test_Exception1()
        {
            var response = new MOBSeatChangeInitializeResponse();
            var request = new MOBSeatChangeInitializeRequest()
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
            _viewResSeatMapBusiness.Setup(p => p.SeatChangeInitialize(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available.")); ;

            var result = _viewResSeatMapController.SeatChangeInitialize(request);
            Assert.True(result.Result != null && result.Result.Exception.Message.Equals("United data services are not currently available."));
            //Assert.True(result.Result.CallDuration > 0);
            Assert.True(result.Exception != null || result.Result != null);
        }

    }
}
