using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ETC
{
    public interface IETCBalanceEnquiryService
    {
        Task<string> GetETCBalanceInquiry(string path, string request, string sessionId, string token);
    }
}
