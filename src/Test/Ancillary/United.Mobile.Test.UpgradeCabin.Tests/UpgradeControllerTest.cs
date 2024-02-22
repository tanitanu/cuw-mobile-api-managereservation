using Microsoft.Extensions.Configuration;
using System;
using United.Common.Helper.Shopping;
using United.Mobile.Services.UpgradeCabin.Api.Controllers;
using United.Mobile.Model;
using Xunit;
using Moq;
using United.Mobile.DataAccess.Common;
using United.Mobile.UpgradeCabin.Domain;
using United.Common.Helper;
using United.Mobile.DataAccess.OnPremiseSQLSP;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Http;
using United.Mobile.Model.UpgradeCabin;
using Microsoft.AspNetCore.Mvc;
using United.Mobile.Model.Internal.Exception;
using United.Ebs.Logging.Enrichers;
using United.Utility.Helper;

namespace United.Mobile.Test.UpgradeCabin.Tests
{
    public class UpgradeControllerTest
    {
        private readonly Mock<ICacheLog<UpgradeCabinController>> _logger;
        private readonly UpgradeCabinController _upgradeCabinController;
        private readonly Mock<IUpgradeCabinBusiness> _upgradeCabinBusiness;
        private readonly IConfiguration _configuration;
        private readonly Mock<IShoppingSessionHelper> _shoppingSessionHelper;
        private readonly Mock<IDynamoDBService> _dynamoDBService;
        private readonly Mock<IDPService> _dPService;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IValidateHashPinService> _validateHashPinService;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRequestEnricher> _requestEnricher;
        private readonly Mock<IFeatureSettings> _featureSettings;
        public UpgradeControllerTest()
        {
            _logger = new Mock<ICacheLog<UpgradeCabinController>>();
            _upgradeCabinBusiness = new Mock<IUpgradeCabinBusiness>();
            _shoppingSessionHelper = new Mock<IShoppingSessionHelper>();
            _dPService = new Mock<IDPService>();
            _headers = new Mock<IHeaders>();
            _dynamoDBService = new Mock<IDynamoDBService>();
            _validateHashPinService = new Mock<IValidateHashPinService>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _requestEnricher = new Mock<IRequestEnricher>();

            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
              .Build();

            _upgradeCabinController = new UpgradeCabinController(_logger.Object, _configuration, _shoppingSessionHelper.Object,  _upgradeCabinBusiness.Object,  _headers.Object, _requestEnricher.Object,_featureSettings.Object);

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
            string result = _upgradeCabinController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Fact]
        public void UpgradePlusPointWebMyTrip_Test()
        {

            var returns = new MOBUpgradePlusPointWebMyTripResponse() { TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c" };

            var request = new MOBUpgradePlusPointWebMyTripRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradePlusPointWebMyTrip(request)).ReturnsAsync(returns);


            // Act
            var result = _upgradeCabinController.UpgradePlusPointWebMyTrip(request);
            // Assert
            Assert.True(result != null && result.Result.TransactionId == "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c");
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void UpgradePlusPointWebMyTrip_MOBUnitedException()
        {
            var returns = new MOBUpgradePlusPointWebMyTripResponse() { };

            var request = new MOBUpgradePlusPointWebMyTripRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradePlusPointWebMyTrip(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _upgradeCabinController.UpgradePlusPointWebMyTrip(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void UpgradePlusPointWebMyTrip_Exception()
        {
            var returns = new MOBUpgradePlusPointWebMyTripResponse() { };

            var request = new MOBUpgradePlusPointWebMyTripRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradePlusPointWebMyTrip(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _upgradeCabinController.UpgradePlusPointWebMyTrip(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void UpgradeCabinEligibleCheck_Test()
        {

            var returns = new MOBUpgradeCabinEligibilityResponse() { TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c" };

            var request = new MOBUpgradeCabinEligibilityRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradeCabinEligibleCheck(request)).ReturnsAsync(returns);


            // Act
            var result = _upgradeCabinController.UpgradeCabinEligibleCheck(request);
            // Assert
            Assert.True(result != null && result.Result.TransactionId == "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c");
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void UpgradeCabinEligibleCheck_MOBUnitedException()
        {

            var returns = new MOBUpgradeCabinEligibilityResponse() { };

            var request = new MOBUpgradeCabinEligibilityRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradeCabinEligibleCheck(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _upgradeCabinController.UpgradeCabinEligibleCheck(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void UpgradeCabinEligibleCheck_Exception()
        {
            var returns = new MOBUpgradeCabinEligibilityResponse() { };

            var request = new MOBUpgradeCabinEligibilityRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _upgradeCabinController.ControllerContext = new ControllerContext();
            _upgradeCabinController.ControllerContext.HttpContext = new DefaultHttpContext();
            _upgradeCabinController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _upgradeCabinBusiness.Setup(p => p.UpgradeCabinEligibleCheck(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _upgradeCabinController.UpgradeCabinEligibleCheck(request);
            // Assert
            Assert.True(result != null);

        }

    }
}
