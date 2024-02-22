using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public interface IUtilitiesService
    {
        Task<string> ValidateMPNames(string token, string requestData, string sessionId, string path = "");
        Task<T> ValidateMileagePlusNames<T>(string token, string requestData, string sessionId, string path);
        Task<T> ValidatePhoneWithCountryCode<T>(string token, string path, string requestData, string sessionId);

    }
}
