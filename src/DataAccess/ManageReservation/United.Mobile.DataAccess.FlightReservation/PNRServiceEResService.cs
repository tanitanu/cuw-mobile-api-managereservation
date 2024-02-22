using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using UAWSPNRServiceERes;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.FlightReservation
{
    public class PNRServiceEResService : IPNRServiceEResService
    {
        private readonly ICacheLog<PNRServiceEResService> _logger;
        private readonly IConfiguration _configuration;
        public PNRServiceEResService(ICacheLog<PNRServiceEResService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<COWSReturnType> GetEmployeePNRs(string employeeID, string accessCode, string activePNROnly)
        {
            string flightReservationURL = _configuration.GetValue<string>("BasicHttpBinding-PNRServiceSoap");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(flightReservationURL));
            var channelFactory = new ChannelFactory<PNRServiceSoapClient>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetPNRRecordsByEmployeeIDAsync(employeeID, accessCode, activePNROnly);
            channelFactory.Close();
            return response;
        }
    }
}
