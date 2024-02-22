using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassRiders;

namespace United.Mobile.DataAccess.PassRiders
{
    public  interface IPassRidersService
   {
        Task<string> GetPassRiders(string token, string requestData, string sessionId);
    }
}
