using MerchandizingServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ServiceModel;
using System.Threading.Tasks;
using United.Utility.Helper;

namespace United.Mobile.DataAccess.MerchandizeService
{
    public class MerchOffersService : IMerchOffersService
    {
        private readonly ICacheLog<MerchOffersService> _logger;
        private readonly IConfiguration _configuration;
        public MerchOffersService(ICacheLog<MerchOffersService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task<GetMerchandizingOffersOutput> GetOffers(GetMerchandizingOffersInput offerRequest)
        {
            string merchandizingServiceURL = _configuration.GetValue<string>("BasicHttpBinding_IMerchandizingServices");
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = _configuration.GetValue<long>("maxReceivedMessageSize");
            if (_configuration.GetValue<bool>("BasicHttpSecurityMode"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;
            }
            var endpoint = new EndpointAddress(new Uri(merchandizingServiceURL));
            var channelFactory = new ChannelFactory<IMerchandizingServices>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();      
            var response = await serviceClient.GetMerchandizingOffersAsync(offerRequest);
            channelFactory.Close();
            return response;
        }
        public async Task<MerchManagementOutput> GetSubscriptions(MerchManagementInput offerRequest)
        {
            string merchandizingServiceURL = _configuration.GetValue<string>("BasicHttpBinding_IMerchandizingServices");
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = _configuration.GetValue<long>("maxReceivedMessageSize");
            if (_configuration.GetValue<bool>("BasicHttpSecurityMode"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;                
            }
            var endpoint = new EndpointAddress(new Uri(merchandizingServiceURL));
            var channelFactory = new ChannelFactory<IMerchandizingServices>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetSubscriptionsAsync(offerRequest);
            channelFactory.Close();
            return response;
        }

        public async Task<GetPurchasedProductsOutput> GetPurchasedProducts(GetPurchasedProductsInput offerRequest)
        {
            string merchandizingServiceURL = _configuration.GetValue<string>("BasicHttpBinding_IMerchandizingServices");
            var binding = new BasicHttpBinding();
            binding.MaxReceivedMessageSize = _configuration.GetValue<long>("maxReceivedMessageSize");
            if (_configuration.GetValue<bool>("BasicHttpSecurityMode"))
            {
                binding.Security.Mode = BasicHttpSecurityMode.Transport;                
            }
            var endpoint = new EndpointAddress(new Uri(merchandizingServiceURL));
            var channelFactory = new ChannelFactory<IMerchandizingServices>(binding, endpoint);
            var serviceClient = channelFactory.CreateChannel();
            var response = await serviceClient.GetPurchasedProductsAsync(offerRequest);
            channelFactory.Close();
            return response;
        }
    }
}
