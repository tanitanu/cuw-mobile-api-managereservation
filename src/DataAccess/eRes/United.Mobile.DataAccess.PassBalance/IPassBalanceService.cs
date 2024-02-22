using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Internal.PassBalance;

namespace United.Mobile.DataAccess.PassBalance
{
    public interface IPassBalanceService
    {
        Task<string> GetPassBalance(string token, string requestData, string sessionId);
    }
}
