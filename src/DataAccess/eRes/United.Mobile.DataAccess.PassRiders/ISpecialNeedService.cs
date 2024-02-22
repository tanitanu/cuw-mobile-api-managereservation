using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.DataAccess.PassRiders
{
    public  interface ISpecialNeedService
    {
        Task<string> GetSpecialNeeds(string token, string sessionId);
    }
}
