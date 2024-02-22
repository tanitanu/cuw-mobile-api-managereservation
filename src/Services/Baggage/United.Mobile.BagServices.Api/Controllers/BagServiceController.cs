using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using United.Mobile.BagServices.Domain;
using United.Utility.Helper;
using United.Mobile.Model.Common;


namespace United.Mobile.BagServices.Api.Controllers
{
    [Route("Baggageservice/api")]
    [ApiController]
    public class BagServiceController : ControllerBase
    {
       

        private readonly ICacheLog<BagServiceController> _logger;
        private readonly IBagServicesBusiness _bagServicesBusiness;
        public BagServiceController(ICacheLog<BagServiceController> logger, IBagServicesBusiness bagServicesBusiness) //ICacheLog<BagServiceController> logger, , IBagServicesBusiness bagServicesBusiness
        {
            _logger = logger;
           _bagServicesBusiness = bagServicesBusiness;
        }

        [HttpGet]
        [Route("HealthCheck")]
        public string HealthCheck()
        {
            return "Healthy";
        }

        [HttpPost]
        [Route("Baggage/PostBaggageEventMessage")]
        public void PostBaggageEventMessage(dynamic request)
        {
            try
            {
                _logger.LogInformation("PostBaggageEventMessage - ClientRequest {@ClientRequest}", JsonConvert.SerializeObject(request));
                _bagServicesBusiness.PostBaggageEventMessage(request);
            }
            catch (Exception ex)
            {
                _logger.LogError("PostBaggageEventMessage - Exception{exception}", ex.Message);
            }
        }

    }
}
