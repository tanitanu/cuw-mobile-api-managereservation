using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Common.Helper.Shopping
{
    public interface IShoppingSessionHelper
    {
        public Session CurrentSession { get; set; }
        Task<Session> CreateShoppingSession(int applicationId, string deviceId, string appVersion, string transactionId, string mileagPlusNumber, string employeeId, bool isBEFareDisplayAtFSR = false, bool isReshop = false, bool isAward = false, string travelType = "", string flow = "", string hashPin = "");
        Task<(bool isTokenValid, Session shopTokenSession)> CheckIsCSSTokenValid(int applicationId, string deviceId, string appVersion, string transactionId, Session shopTokenSession, string tokenToValidate);
        Task<Session> GetBookingFlowSession(string sessionId, bool isBookingFlow = true);
        Task<Session> GetShoppingSession(string sessionId);
        Task<Session> GetValidateSession(string sessionId, bool isBookingFlow, bool isViewRes_CFOPFlow);
        Task<string> GetSessionWithValidToken(MOBModifyReservationRequest request);

    }
}
