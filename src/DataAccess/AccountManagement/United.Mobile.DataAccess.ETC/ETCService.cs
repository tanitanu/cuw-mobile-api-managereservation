using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using UAWSMPTravelCertificateService.ETCServiceSoap;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.ETC
{
    public class ETCService : IETCService
    {
        private readonly IConfiguration _configuration;
        private readonly ICacheLog<IETCService> _logger;

        public ETCService(IConfiguration configuration, ICacheLog<IETCService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ETCReturnType> ETCSearchByFreqFlyerNum(string freqFlyerNum, string stationID, string dutyCode, string agentSine, string lineIATA, string accessCode, string sessionID)
        {
            string profileServiceURL = _configuration.GetValue<string>("ETCServiceSoap");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(profileServiceURL));
            var channelFactory = new ChannelFactory<ETCServiceSoap>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            _logger.LogInformation("ETCSearchByFreqFlyerNum SOAP service -{@FreqFlyerNum} {@stationID} {@DutyCode} {@AgentSine} {@LineIATA} {@AccessCode}", freqFlyerNum, stationID, dutyCode, agentSine, lineIATA, accessCode);
            var response = await serviceClient.ETCSearchByFreqFlyerNumAsync(freqFlyerNum, stationID, dutyCode, agentSine, lineIATA, accessCode);
            channelFactory.Close();

            _logger.LogInformation("ETCSearchByFreqFlyerNum SOAP service -{@Response} {@MileagePlusNumber}", JsonConvert.SerializeObject(response), freqFlyerNum);

            return response;
        }
    }
}
