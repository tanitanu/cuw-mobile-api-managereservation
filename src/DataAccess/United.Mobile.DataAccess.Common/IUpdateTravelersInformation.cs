using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IUpdateTravelersInformation
    {
        Task<T> UpdateTravelersInfo<T>(string request, string transactionId);
    }
}
