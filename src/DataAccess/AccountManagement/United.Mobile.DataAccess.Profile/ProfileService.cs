using Microsoft.Extensions.Configuration;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using UAWSProfileMP2014;

namespace United.Mobile.DataAccess.Profile
{
    public class ProfileService : IProfileService
    {
        private readonly IConfiguration _configuration;

        public ProfileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<wsAccountResponse> GetOwnerProfileForMP2014(string mileagePlusAccountNumber)
        {
            string profileServiceURL = _configuration.GetValue<string>("AccountProfileSoap12");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(profileServiceURL));
            var channelFactory = new ChannelFactory<AccountProfileSoapClient>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetProfileWithPartnerCardOptionAsync(mileagePlusAccountNumber, true, "iPhone", _configuration.GetValue<string>("AccessCode - AccountProfile"), string.Empty);
            channelFactory.Close();
            return response;
        }

        //public async Task<wsPaymentInfoResponse> GetPaymentInfo(string mileagePlusAccountNumber, bool getPartnerCardsOnly, string agentId, string accessCode, string version)
        //{
        //    string profileServiceURL = _configuration.GetValue<string>("AccountProfileSoap");
        //    var binding = new BasicHttpBinding();
        //    var endpoint = new EndpointAddress(new Uri(profileServiceURL));
        //    var channelFactory = new ChannelFactory<AccountProfileSoapClient>(binding, endpoint);
        //    var serviceClient = channelFactory.CreateChannel();
        //    var response = await serviceClient.GetPaymentInfoAsync(mileagePlusAccountNumber, getPartnerCardsOnly, agentId, accessCode, version);
        //    channelFactory.Close();
        //    return response;
        //}

        public async Task<UAWSAccountProfile.wsAccountResponse> GetProfile(string mileagePlusAccountNumber)
        {
            string profileServiceURL = _configuration.GetValue<string>("AccountProfileSoap");
            var binding = new BasicHttpBinding();
            var endpoint = new EndpointAddress(new Uri(profileServiceURL));
            var channelFactory = new ChannelFactory<UAWSAccountProfile.AccountProfileSoapClient>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetProfileWithPartnerCardOptionAsync(mileagePlusAccountNumber, true, "iPhone", _configuration.GetValue<string>("AccessCode - AccountProfile"), string.Empty);
            channelFactory.Close();
            return response;
        }
    }
}

