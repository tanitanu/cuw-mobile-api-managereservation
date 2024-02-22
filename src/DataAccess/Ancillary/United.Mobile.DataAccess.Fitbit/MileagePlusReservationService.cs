using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UAWSMileagePlusReservation;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.Fitbit
{
    public class MileagePlusReservationService: IMileagePlusReservationService
    {
        private readonly ICacheLog<MileagePlusReservationService> _logger;
        private readonly IConfiguration _configuration;
        public MileagePlusReservationService(ICacheLog<MileagePlusReservationService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<wsReservationResponse> GetPNRsByMileagePlusNumber(string mileagePlusNumber, string accessCode, string version)
        {
            string flightReservationURL = _configuration.GetValue<string>("wsReservationSoap");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(flightReservationURL));
            var channelFactory = new ChannelFactory<wsReservationSoapClient>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetReservationsAsync(mileagePlusNumber, accessCode, version);
            channelFactory.Close();
            return response;
        }
    }
}
