using System.Threading.Tasks;

namespace United.Mobile.DataAccess.FeedBack
{
    public interface ISendFeedBackService
    {
        Task<string> PostFeedback(string token, string request, string sessionId, string path);
        Task<string> GetFeedback(string token, string sessionId, string path);
    }
}
