using System.Threading.Tasks;
using United.Mobile.Model.Internal.HomePageContent;

namespace United.Mobile.DataAccess.HomePageContent
{
    public interface IHomePageContentService
   {
        Task<string> GetHomePageContents(string token, string requestData, string sessionId); 
    }
}
