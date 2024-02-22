using System.Threading.Tasks;
using UAWSProfileMP2014;

namespace United.Mobile.DataAccess.Profile
{
    public interface IProfileService
    {
        Task<wsAccountResponse> GetOwnerProfileForMP2014(string mileagePlusAccountNumber);
        //Task<wsPaymentInfoResponse> GetPaymentInfo(string mileagePlusAccountNumber, bool getPartnerCardsOnly, string agentId, string accessCode, string version);
        Task<UAWSAccountProfile.wsAccountResponse> GetProfile(string mileagePlusAccountNumber);
    }
}
