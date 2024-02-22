using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IReferencedataService
    {
        Task<string> GetAndValidateStateCode(string token, string requestData, string sessionId);
        Task<T> GetDataPostAsync<T>(string actionName, string token, string sessionId, string request);
        Task<T> GetDataGetAsync<T>(string actionName, string token, string sessionId);
        Task<(T Response, long callDuartion)> RewardPrograms<T>(string token, string sessionId);
        Task<T> GetNationalityResidence<T>(string actionName, string token, string sessionId);
        Task<T> GetSpecialNeedsInfo<T>(string actionName, string request, string token, string sessionId);
        Task<string> GetRewardPrograms(string token, string urlPath, string sessionId);
    }
}
