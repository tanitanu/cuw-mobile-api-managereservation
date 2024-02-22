using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    public class Balances
    {

        //
        // Summary:
        //     Balance amount.       
        public decimal Amount { get; set; }
        //
        // Summary:
        //     Expiry date for balance     
        public DateTime? ExpirationDate { get; set; }
        //
        // Summary:
        //     Program currecy  
        public Constants.ProgramCurrencyType Currency { get; set; }
        //
        // Summary:
        //     Subbalances     
        public List<SubBalances> SubBalances { get; set; }
    }
}
