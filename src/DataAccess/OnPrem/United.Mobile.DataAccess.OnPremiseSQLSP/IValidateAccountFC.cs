using System.Threading.Tasks;

namespace United.Mobile.DataAccess.OnPremiseSQLSP
{
    public  interface IValidateAccountFC
    {
        Task<bool> ValidateAccount(string accountNumber, string pinCode, string sessionId);
    }
}
