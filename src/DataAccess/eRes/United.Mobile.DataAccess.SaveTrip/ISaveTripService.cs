using System;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.SaveTrip
{
    public interface ISaveTripService
    {
        Task<string> SaveTrip(string token, string requestPayload, string sessionId);
    }
}
