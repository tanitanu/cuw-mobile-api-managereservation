using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.IO;
using United.Common.Helper;
using United.Mobile.Services.PreOrderMeals.Api.Controllers;
using United.Mobile.Services.PreOrderMeals.Domain;
using Xunit;
using United.Mobile.Model;
using Microsoft.AspNetCore.Mvc;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Internal.Exception;
using United.Ebs.Logging.Enrichers;
using United.Utility.Helper;

namespace United.Mobile.Test.PreOrderMeals.Tests
{
    public class PreOrderControllerTest
    {

        private readonly Mock<ICacheLog<PreOrderMealsController>> _logger;
        private readonly PreOrderMealsController _preOrderMealsController;
        private readonly IConfiguration _configuration;
        private readonly Mock<IHeaders> _headers;
        private readonly Mock<IPreOrderMealsBusiness> _preOrderMealsBusiness;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessor;
        private readonly Mock<IRequestEnricher> _requestEnricher;
        private readonly Mock<IFeatureSettings> _featureSettings;
        public PreOrderControllerTest()
        {
            _logger = new Mock<ICacheLog<PreOrderMealsController>>();
            _headers = new Mock<IHeaders>();
            _preOrderMealsBusiness = new Mock<IPreOrderMealsBusiness>();
            _httpContextAccessor = new Mock<IHttpContextAccessor>();
            _requestEnricher = new Mock<IRequestEnricher>();
            _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.test.json", optional: false, reloadOnChange: true)
              .Build();

            _preOrderMealsController = new PreOrderMealsController(_logger.Object, _configuration, _headers.Object, _preOrderMealsBusiness.Object, _requestEnricher.Object,_featureSettings.Object);

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
            string result = _preOrderMealsController.HealthCheck();
            Assert.True(result == "Healthy");
        }

        [Fact]
        public void GetInflightMealOffers_Test()
        {

            var returns = new MOBInFlightMealsOfferResponse() { TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c" };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffers(request)).ReturnsAsync(returns);


            // Act
            var result = _preOrderMealsController.GetInflightMealOffers(request);
            // Assert
            Assert.True(result != null && result.Result.TransactionId == "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c");
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void GetInflightMealOffers_MOBUnitedException()
        {

            var returns = new MOBInFlightMealsOfferResponse() { };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffers(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealOffers(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void GetInflightMealOffers_Exception()
        {

            var returns = new MOBInFlightMealsOfferResponse() { };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffers(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealOffers(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void GetInflightMealOffersForDeeplink_Test()
        {

            var returns = new MOBInFlightMealsOfferResponse() { TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c" };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffersForDeeplink(request)).ReturnsAsync(returns);


            // Act
            var result = _preOrderMealsController.GetInflightMealOffersForDeeplink(request);
            // Assert
            Assert.True(result != null && result.Result.TransactionId == "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c");
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void GetInflightMealOffersForDeeplink_MOBUnitedException()
        {

            var returns = new MOBInFlightMealsOfferResponse() { };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffersForDeeplink(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealOffersForDeeplink(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void GetInflightMealOffersForDeeplink_Exception()
        {
            var returns = new MOBInFlightMealsOfferResponse() { };

            var request = new MOBInFlightMealsOfferRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealOffersForDeeplink(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealOffersForDeeplink(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void GetInflightMealRefreshments_Test()
        {

            var returns = new MOBInFlightMealsRefreshmentsResponse() { TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c" };

            var request = new MOBInFlightMealsRefreshmentsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealRefreshments(request)).ReturnsAsync(returns);


            // Act
            var result = _preOrderMealsController.GetInflightMealRefreshments(request);
            // Assert
            Assert.True(result != null && result.Result.TransactionId == "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c");
            Assert.True(result.Result.Exception == null);

        }

        [Fact]
        public void GetInflightMealRefreshments_MOBUnitedException()
        {
            var returns = new MOBInFlightMealsRefreshmentsResponse() { };

            var request = new MOBInFlightMealsRefreshmentsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealRefreshments(request)).ThrowsAsync(new MOBUnitedException("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealRefreshments(request);
            // Assert
            Assert.True(result != null);

        }

        [Fact]
        public void GetInflightMealRefreshments_Exception()
        {

            var returns = new MOBInFlightMealsRefreshmentsResponse() { };

            var request = new MOBInFlightMealsRefreshmentsRequest()
            {
                SessionId = "67945321097C4CF58FFC7DF9565CB276",
                TransactionId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685|a43aa991-9d57-40f0-9b83-79f34d880e7c",
                DeviceId = "446e943d-d749-4f2d-8bf2-e0ba5d6da685",
                Application = new MOBApplication() { Id = 1, Version = new MOBVersion() { Major = "4.1.48" } }
            };

            _preOrderMealsController.ControllerContext = new ControllerContext();
            _preOrderMealsController.ControllerContext.HttpContext = new DefaultHttpContext();
            _preOrderMealsController.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";

            _preOrderMealsBusiness.Setup(p => p.GetInflightMealRefreshments(request)).ThrowsAsync(new System.Exception("United data services are not currently available."));


            // Act
            var result = _preOrderMealsController.GetInflightMealRefreshments(request);
            // Assert
            Assert.True(result != null);

        }

    }
}
