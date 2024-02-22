using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Fitbit
{
    [Serializable()]
    public class MOBWalletRequest : MOBRequest
    {
        public bool BackgroundRefresh { get; set; }
        public string MPNumber { get; set; } = string.Empty;
        public List<MOBWalletCategory> WalletCategories { get; set; }
        public string PushToken { get; set; } = string.Empty;
        public string LogTransactionId { get; set; } = string.Empty;
        public long CustomerId { get; set; }
        public long EmployeeId { get; set; }
        public MOBWalletRequest()
           : base()
        {
            WalletCategories = new List<MOBWalletCategory>();
        }

    }
}
