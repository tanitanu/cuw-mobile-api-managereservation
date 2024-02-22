using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface IGetBagTrackingService
    {
        Task<int> HasCheckedBag(string request);
        Task<string> HasCheckedBagV2(string request, string path);
    }
}
