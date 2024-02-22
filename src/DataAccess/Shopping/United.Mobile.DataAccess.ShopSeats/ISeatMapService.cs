using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopSeats
{
    public interface ISeatMapService
    {
        Task<T> SeatEngine<T>(string token, string action, string request, string sessionId);
    }
}
