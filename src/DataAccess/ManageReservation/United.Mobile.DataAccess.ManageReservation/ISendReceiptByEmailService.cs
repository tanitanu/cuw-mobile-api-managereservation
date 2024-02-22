using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.ManageReservation
{
    public interface ISendReceiptByEmailService
    {
        Task<string> SendReceiptByEmailViaCSL(string token, string request, string sessionId, string path);
    }
}
