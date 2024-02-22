using System.Threading.Tasks;

namespace United.Mobile.DataAccess.TripPlannerService
{
    public interface IAddTripPlanVotingService
    {
        Task<string> TripPlanVoting(string token, string sessionId, string action, string request);
        Task<string> DeleteTripPlan(string token, string action, string sessionId);
    }
}
