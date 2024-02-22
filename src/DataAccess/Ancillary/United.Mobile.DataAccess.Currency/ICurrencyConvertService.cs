using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Currency
{
    public interface ICurrencyConvertService
    {
        Task<string> GetCurrency(string token, string request, string sessionId);
    }
}
