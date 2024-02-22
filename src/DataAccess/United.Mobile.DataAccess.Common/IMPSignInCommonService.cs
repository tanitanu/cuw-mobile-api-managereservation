using System.Threading.Tasks;

namespace United.Mobile.DataAccess.Common
{
    public interface IMPSignInCommonService
    {
        Task<T> VerifyMileagePlusHashpin<T>(string request, string transactionId);
    }
}
