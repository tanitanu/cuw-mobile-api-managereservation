using System.Threading.Tasks;

namespace United.Mobile.DataAccess.UnitedClub
{
    public interface IUnitedClubMembershipV2Service
    {
        Task<string> GetCurrentMembershipInfo(string mPNumber,string transactionId);
    }

}
