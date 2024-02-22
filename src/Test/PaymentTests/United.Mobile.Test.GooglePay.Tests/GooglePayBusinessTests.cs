using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using United.Common.Helper;
using United.Mobile.DataAccess.Common;
using United.Mobile.DataAccess.Payment;
using United.Mobile.Model;
using United.Mobile.Model.GooglePay;
using United.Mobile.Services.GooglePay.Domain;
using United.Utility.Helper;
using Xunit;

namespace United.Mobile.Test.GooglePay.Tests
{
    public class GooglePayBusinessTests
    {
        private readonly Mock<ICacheLog<GooglePayBusiness>> _logger;
        private readonly IConfiguration _configuration;
        private readonly Mock<IGooglePayAccessTokenService> _googlePayAccessTokenService;
        private readonly GooglePayBusiness _googlePayBusiness;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IFlightClassService> _flightClassService;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<ISessionHelperService> _sessionHelperService;
        private readonly Mock<Microsoft.Extensions.Hosting.IHostingEnvironment> _currentEnvironment;




        public GooglePayBusinessTests()
        {
            _logger = new Mock<ICacheLog<GooglePayBusiness>>();
            _sessionHelperService = new Mock<ISessionHelperService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _currentEnvironment = new Mock<Microsoft.Extensions.Hosting.IHostingEnvironment>();
            _headers = new Mock<IHeaders>();
            _googlePayAccessTokenService = new Mock<IGooglePayAccessTokenService>();
            _flightClassService = new Mock<IFlightClassService>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appSettings.test.json", optional: false, reloadOnChange: true)
            .Build();
            _googlePayBusiness = new GooglePayBusiness(_logger.Object, _configuration, _googlePayAccessTokenService.Object, _flightClassService.Object, _dynamoDBService.Object,_currentEnvironment.Object,_headers.Object);
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

        public static string GetFileContent(string fileName)
        {
            fileName = string.Format("..\\..\\..\\TestData\\{0}", fileName);
            var path = Path.IsPathRooted(fileName) ? fileName : Path.GetRelativePath(Directory.GetCurrentDirectory(), fileName);
            return File.ReadAllText(path);
        }

        [Theory]
        [MemberData(nameof(Input.InputGooglePay), MemberType = typeof(Input))]
        public void InsertFlight_Test(MOBGooglePayFlightRequest input, string accessToken, string FlightClass)
        {
            //var AccessTokenClassjson = GetFileContent("AccessTokenClass.json");
            var AccessToken = GetFileContent("GooglePayRequest.json");
            _googlePayAccessTokenService.Setup(p => p.GetGooglePayAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(accessToken);
            _flightClassService.Setup(p => p.InsertFlightClass(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(FlightClass);

            var result = _googlePayBusiness.InsertFlight(input);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }


        [Theory]
        [MemberData(nameof(Input.InputGooglePay1), MemberType = typeof(Input))]
        public void UpdateFlightFromRequest_Test(MOBGooglePayFlightRequest input, string accessToken, string FlightClass)
        {
            var AccessTokenClassjson = GetFileContent("UpdateFlightFromRequestRequest.json");
            _googlePayAccessTokenService.Setup(p => p.GetGooglePayAccessToken(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(accessToken);
            _flightClassService.Setup(p => p.InsertFlightClass(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(FlightClass);

            var result = _googlePayBusiness.UpdateFlightFromRequest(input);
            if (result?.Exception == null)
            {
                Assert.True(result.Result != null && result.Result.TransactionId != null);
            }
            else
                Assert.True(result.Exception != null && result.Exception.InnerException != null);
        }
    }
}

