using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Profile
{
    public interface IBaseEmployeeResService
    {
        Task<T> UpdateTravelerBase<T>(string token, string request, string sessionId, string path);
    }
}
