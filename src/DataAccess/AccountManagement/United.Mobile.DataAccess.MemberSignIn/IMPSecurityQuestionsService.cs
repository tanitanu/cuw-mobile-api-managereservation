using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MemberSignIn
{
    public interface IMPSecurityQuestionsService
    {
        Task<string> GetMPPINPWDSecurityQuestions(string token, string requestData, string sessionId);
        Task<T> ValidateSecurityAnswer<T>(string token, string requestData, string sessionId, string path);
        Task<string> AddDeviceAuthentication(string token, string requestData, string sessionId, string path );
    }
}
