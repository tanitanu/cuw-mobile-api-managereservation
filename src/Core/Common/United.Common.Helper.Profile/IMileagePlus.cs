using System.Threading.Tasks;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;

namespace United.Common.Helper.Profile
{
    public interface IMileagePlus
    {
        Task<(string employeeId, string displayEmployeeId)> GetEmployeeIdy(string mileageplusNumber, string transactionId, string sessionId, string displayEmployeeId);
        Task<MOBPlusPoints> GetPlusPointsFromLoyaltyBalanceService(MPAccountValidationRequest req, string dpToken);

    }
}
