using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ManageReservation
{
    public interface IUpgradeEligibilityService
    {
        Task<string> GetUpgradeCabinEligibleCheck(string token, string request, string sessionId, string path);
    }
}
