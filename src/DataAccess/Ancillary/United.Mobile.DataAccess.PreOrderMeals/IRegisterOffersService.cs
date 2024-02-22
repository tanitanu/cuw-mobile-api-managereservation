using System.Threading.Tasks;

namespace United.Mobile.DataAccess.PreOrderMeals
{
    public interface IRegisterOffersService
    {
        Task<string> RegisterOffers(string request, string sessionId, string path);
    }
}