using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IClubMembershipService
    {
        Task<T> GetClubMembership<T>(string token, string requestData, string sessionId);
    }
}
