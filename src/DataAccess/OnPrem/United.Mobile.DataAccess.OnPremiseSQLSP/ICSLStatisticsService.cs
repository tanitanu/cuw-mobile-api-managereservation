using System.Threading.Tasks;
using United.Mobile.Model.Shopping;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface ICSLStatisticsService
    {
        Task<bool> AddCSLStatistics(string requestData, string sessionId);
    }
}
