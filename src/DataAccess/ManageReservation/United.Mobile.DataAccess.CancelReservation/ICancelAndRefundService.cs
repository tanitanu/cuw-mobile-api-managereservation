using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.DataAccess.CancelReservation
{
    public interface ICancelAndRefundService
    {
        Task<string> PutCancelReservation(string token, string sessionId, string path,string requestData);
    }
}
