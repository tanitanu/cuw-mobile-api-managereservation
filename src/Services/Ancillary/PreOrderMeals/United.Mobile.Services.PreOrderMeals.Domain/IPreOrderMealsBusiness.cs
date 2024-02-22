using System.Threading.Tasks;
using United.Mobile.Model.PreOrderMeals;
using United.Mobile.Model.Shopping;

namespace United.Mobile.Services.PreOrderMeals.Domain
{
    public interface IPreOrderMealsBusiness
    {
        public Task<MealsDetailResponse> GetAvailableMealsV2(PreOrderMealListRequest request);

        public Task<MealsDetailResponse> GetAvailableMeals(PreOrderMealListRequest request);

        public Task<PreOrderMealCartResponse> AddToCartV2(PreOrderMealCartRequest request);

        public Task<PreOrderMealCartResponse> AddToCart(PreOrderMealCartRequest request);

        public Task<MOBInFlightMealsOfferResponse> GetInflightMealOffersForDeeplink(MOBInFlightMealsOfferRequest request);

        public Task<MOBInFlightMealsOfferResponse> GetInflightMealOffers(MOBInFlightMealsOfferRequest request);

        public Task<MOBInFlightMealsRefreshmentsResponse> GetInflightMealRefreshments(MOBInFlightMealsRefreshmentsRequest request);

        public Task<PreOrderMealResponseContext> GetTripsForPerOrderMeal(MOBPNRByRecordLocatorRequest request);

        //public Task<PreOrderMealTripDetailResponseContext> GetPreOrderMealsTripDetailsV2(MOBPNRByRecordLocatorRequest mobilePnrRequest, Session session)
    }
}
