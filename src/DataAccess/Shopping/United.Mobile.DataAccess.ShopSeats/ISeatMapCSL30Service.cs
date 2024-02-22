using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopSeats
{
    public interface ISeatMapCSL30Service
    {
        Task<string> GetSeatMapDeatils(string token, string sessionId, string request, string channelId, string channelName, string path);
    }
}
