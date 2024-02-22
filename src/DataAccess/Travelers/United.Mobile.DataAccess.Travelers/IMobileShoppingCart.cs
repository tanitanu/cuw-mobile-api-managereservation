using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Travelers
{
    public interface IMobileShoppingCart
    {
        Task<string> UnRegisterAncillaryOffersForBooking(string token, string action, string request, string sessionId);
    }
}
