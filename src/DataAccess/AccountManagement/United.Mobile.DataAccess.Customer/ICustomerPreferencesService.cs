using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Customer
{
    public interface ICustomerPreferencesService
    {
        Task<T> GetCustomerPreferences<T>(string token, string mpNumber,string sessionId);
        Task<string> GetSharedItinerary(string token, string action, string requestData, string sessionId);
        Task<T> GetCustomerPrefernce<T>(string token, string action, string savedUnfinishedBookingActionName, string savedUnfinishedBookingAugumentName, int customerID, string sessionId);
        Task<T> GetUnfinishedCustomerPrefernce<T>(string token, string action, string savedUnfinishedBookingActionName, string savedUnfinishedBookingAugumentName, int customerID, string sessionId, string request);
        Task<T> GetSharedTrip<T>(string token, string sharedTripId, string sessionId);
        Task<string> PurgeAnUnfinishedBooking(string token, string action, string sessionId);
    }
}
