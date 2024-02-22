using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FlightShopping
{
    public interface IFlightShoppingService
    {
        Task<string> GetShopPinDown(string token, string action, string request, string sessionId);
        Task<(T response, long callDuration)> GetLmxQuote<T>(string token, string sessionId, string cartId, string hashList);
        Task<(T response, long callDuration)> UpdateAmenitiesIndicators<T>(string token, string sessionId, string jsonRequest);
    }
}
