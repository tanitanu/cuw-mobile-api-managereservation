using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopBundles
{
    public interface IBundleOfferService
    {
        Task<(T response, long callDuration)> DynamicOfferdetail<T>(string token, string sessionId, string action, string shopRequest);
        Task<(T response, long callDuration)> UnfinishedBookings<T>(string token, string sessionId, string action, string request);
        Task<(string response, long callDuration)> GetCCEContent<T>(string token, string sessionId, string action, string request);

    }
}
