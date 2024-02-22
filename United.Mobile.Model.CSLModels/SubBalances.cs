using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace United.Mobile.Model.CSLModels
{
    [DataContract]
    public class SubBalances
    {
        //
        // Summary:
        //     Balance Amount associated with a particular program currency

        public decimal Amount { get; set; }
        //
        // Summary:
        //     Source of the fund

        public string Type { get; set; }
    }
}
