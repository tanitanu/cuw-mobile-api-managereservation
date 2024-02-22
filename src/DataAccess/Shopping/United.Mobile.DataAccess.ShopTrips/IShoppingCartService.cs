using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopTrips
{
    public interface IShoppingCartService
    {
        Task<T> GetShoppingCartInfo<T>(string token, string action, string request, string sessionId);
        Task<T> GetCartInformation<T>(string token, string action, string request, string sessionId);
        Task<T> GetProductDetailsFromCartID<T>(string token, string cartID, string sessionId);
        Task<(T response, long callDuration)> RegisterOrRemove<T>(string token, string action, string request, string sessionId);
        Task<T> GetRegisterTravelers<T>(string token, string sessionId, string jsonRequest);
        Task<T> GetFormsOfPayments<T>(string token, string action, string sessionId, string jsonRequest, Dictionary<string, string> additionalHeaders);
        Task<string> CreateCart(string token, string jsonRequest, string sessionId);
        Task<T> FareLockReservation<T>(string token, string action, string sessionId, string jsonRequest);
        Task<(T response, long callDuration)> GetCart<T>(string token, string sessionId, string jsonRequest);
        Task<T> GetRegisterSeats<T>(string token, string action, string sessionId, string jsonRequest);
        Task<T> RegisterOrRemoveCoupon<T>(string token, string action, string request, string sessionId);
        //Task<string> GetCartInformation(string token, string action, string request, string sessionId);
        Task<T> RegisterFlights<T>(string token, string action, string request, string sessionId);
        Task<T> RegisterOffers<T>(string token, string action, string request, string sessionId);
        Task<string> RegisterFareLockReservation(string token, string action, string request, string sessionId);
        Task<string> RegisterCheckinSeats(string token, string action, string request, string sessionId);
        Task<string> RegisterBags(string token, string action, string request, string sessionId);
        Task<string> RegisterSameDayChange(string token, string action, string request, string sessionId);
        Task<string> RegisterFormsOfPayments_CFOP(string token, string action, string request, string sessionId);
        Task<string> ClearSeats(string token, string action, string request, string sessionId);
        Task<string> RegisterSeats_CFOP(string token, string action, string request, string sessionId);
    }
}
