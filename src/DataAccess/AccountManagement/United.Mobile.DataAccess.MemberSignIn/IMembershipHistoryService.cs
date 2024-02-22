using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public interface IMembershipHistoryService
    {
        Task<T> GetMembershipHistory<T>(string token, string mpNumber, string sessionId);
    }
}
