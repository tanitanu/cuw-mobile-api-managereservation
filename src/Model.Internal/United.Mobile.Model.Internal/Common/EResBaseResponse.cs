using System;

namespace United.Mobile.Model.Internal.Common
{
    public class EResBaseResponse
    {
        public EResBaseResponse()
        {
            ServerName = Environment.MachineName;
        }
        public EResAlert BaseAlert { get; set; }
        public ErrorInfo Error { get; set; }
        public string LastCallDateTime { get; set; } = DateTime.Now.ToString();
        public string ServerName { get; set; }
        public string Status { get; set; }
        public string TransactionID { get; set; }
        public string TransferMessage { get; set; }
        public bool IsAllowedToSelectFlight { get; set; }
    }
}
