using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CancelReservation
{
    public interface ICancelRefundService
    {
        Task<string> GetRefund(string token, string sessionId, string path);
        Task<string> GetQuoteRefund(string token, string sessionId, string path);
    }
}
