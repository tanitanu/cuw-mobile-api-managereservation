using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public interface IAddFareLockEmailService
    {
        Task<bool> AddFareLockEmail(string farelockoffertype, string emaiaddr, string pnr, string pnrcreatedatetime, string sessionId);
    }
}
