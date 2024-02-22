using System.Threading.Tasks;
using United.Mobile.Model;
using United.Mobile.Model.SeatMap;

namespace United.Mobile.SeatPreference.Domain
{
    public interface ISeatPreferenceBusiness
    {
        Task<PersistSeatPreferenceResponse> GetSeatPreferencefromCSL(PersistSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode);

        Task<PostSeatPreferenceResponse> SaveSeatPreferencetToCSL(PostSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode);

        Task<PersistSeatPreferenceResponse> GetSeatPreferencefromCSLV2(PersistSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode);

        Task<PostSeatPreferenceResponse> SaveSeatPreferencetToCSLV2(PostSeatPreferenceRequest request, int appId, string deviceId, string transactionId, string accessCode, MOBApplication mOBApplication, string langCode);
    }
}
