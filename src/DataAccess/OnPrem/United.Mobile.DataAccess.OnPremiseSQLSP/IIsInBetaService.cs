using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface IIsInBetaService
    {
        Task<T> IsInBeta<T>(string mileagePlusAccountNumber, int featureId, int applicationId, string applicationVersion, string sessionId);
    }
}
