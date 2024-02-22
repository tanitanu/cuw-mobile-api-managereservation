using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.ManageRes
{
    public class MOBCancelAndRefund
    {
        public string Pnr { get; set; }
        public string Email { get; set; }
        public string TransactionId { get; set; }
        public bool CancelSuccess { get; set; }
        public bool RefundSuccess { get; set; }
        public bool EmailSuccess { get; set; }
        public string SessionId { get; set; }
        public string QuoteType { get; set; }

        public MOBCancelAndRefund()
        {
            CancelSuccess = false;
            RefundSuccess = false;
            EmailSuccess = false;
            SessionId = string.Empty;
        } 
    }
}
