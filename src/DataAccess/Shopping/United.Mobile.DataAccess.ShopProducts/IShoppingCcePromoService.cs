using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ShopProducts
{
    public interface IShoppingCcePromoService
    {
        Task<string> ShoppingCcePromo(string token, string request, string sessionId);
        Task<string> MerchOffersCceDetails(string token, string request, string sessionId);
        Task<string> DynamicOffers(string token, string request, string transactionId);
        Task<string> ChasePromoFromCCE(string token, string request, string sessionId);
        Task<string> CCEDynamicOfferResponse(string token, string request, string sessionId);
    }
}
