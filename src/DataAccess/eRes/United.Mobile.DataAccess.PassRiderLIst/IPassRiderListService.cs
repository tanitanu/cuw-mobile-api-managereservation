using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassRiderList;

namespace United.Mobile.DataAccess.PassRiderList
{
    public  interface IPassRiderListService
   {
        Task<string> GetPassRiderList(string token, string requestData, string sessionId);
   }
}
