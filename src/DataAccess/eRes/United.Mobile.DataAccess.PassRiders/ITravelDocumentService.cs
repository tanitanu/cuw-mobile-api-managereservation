using System.Threading.Tasks;

namespace United.Mobile.DataAccess.PassRiders
{
    public interface ITravelDocumentService
    {
        Task<string> SaveTravelDocument(string token,string path, string requestData, string sessionId);
        Task<string> GetDefaultTravelDocuments(string token, string path, string sessionId);
    }

}
