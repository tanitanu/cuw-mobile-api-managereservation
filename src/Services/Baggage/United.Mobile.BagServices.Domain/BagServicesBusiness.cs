using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.ManageRes;
using United.Utility.Helper;

namespace United.Mobile.BagServices.Domain
{
    public class BagServicesBusiness:IBagServicesBusiness
    {
        private readonly ICacheLog<BagServicesBusiness> _logger;
        private readonly IConfiguration _configuration;
        private readonly IAuroraMySqlService _auroraMySqlService;
        public BagServicesBusiness(ICacheLog<BagServicesBusiness> logger, IConfiguration configuration,IAuroraMySqlService auroraMySqlService) //ICacheLog<BagServicesBusiness> logger,
        {
            _logger = logger;
            _configuration = configuration;
            _auroraMySqlService = auroraMySqlService;
        }
        public void PostBaggageEventMessage(dynamic request)
        {
            try
            {
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                string bagEventText = request.text;
                BaggageEvents baggageEvents = new BaggageEvents();
                if (!string.IsNullOrEmpty(bagEventText))
                {
                    baggageEvents = JsonConvert.DeserializeObject<BaggageEvents>(bagEventText, jsonSettings);
                }
               
                string pnrNumber = baggageEvents?.Passenger?.PnrNumber;
                string bagTagNumber = baggageEvents?.Bag?.BagTagNumber;
                Int64 bagTagUniqueKey = Convert.ToInt64(baggageEvents?.Bag?.BagTagUniqueKey);
                bool isActive = Convert.ToBoolean(baggageEvents?.Bag?.IsActive);
                string firstName = baggageEvents?.Passenger?.FirstName;
                string lastName = baggageEvents?.Passenger?.LastName;
               // await auroraMySqlService.InsertBaggageEventMessage(pnrNumber, bagTagNumber, bagTagUniqueKey, isActive, firstName, lastName);
                Task.Factory.StartNew(() => _auroraMySqlService.InsertBaggageEventMessage(pnrNumber, bagTagNumber, bagTagUniqueKey, isActive, firstName, lastName));
            }
            catch (Exception ex)
            {
                _logger.LogError("PostBaggageEventMessage - Exception{exception}", ex.Message);
            }
        }
    }
}
