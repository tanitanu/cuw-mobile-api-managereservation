using MerchandizingServices;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.MerchandizeService
{
    public interface IMerchOffersService
    {
        Task<GetMerchandizingOffersOutput> GetOffers(GetMerchandizingOffersInput offerRequest);
        Task<MerchManagementOutput> GetSubscriptions(MerchManagementInput offerRequest);
        Task<GetPurchasedProductsOutput> GetPurchasedProducts(GetPurchasedProductsInput offerRequest);
    }
}
