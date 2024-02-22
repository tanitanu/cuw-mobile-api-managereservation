using System;
using System.Collections.Generic;

namespace United.Mobile.Model.CodeTable
{
    [Serializable()]
    public class MOBCountryPhoneResponse : MOBResponse
    {
        public List<MOBCountryPhone> CountryPhoneList { get; set; }
        public MOBCountryPhoneResponse()
        {
            CountryPhoneList = new List<MOBCountryPhone>();
        }

    }

    [Serializable()]
    public class MOBCountryPhone
    {
       
        public int PointOfSaleCountryOrder { get; set; }

        public string PointOfSaleCountryName { get; set; } = string.Empty;

        public string CountryName { get; set; } = string.Empty;

        public string CountryCode { get; set; } = string.Empty;

        public string CountryPhoneCode { get; set; } = string.Empty;

        public string IsActive { get; set; } = string.Empty;

        public string IsActiveForNationality { get; set; } = string.Empty;

        public string IsActiveForPhoneCode { get; set; } = string.Empty;

        public string IsActiveForCountryOfResidence { get; set; } = string.Empty;

        public string IsActiveForPointOfSale { get; set; } = string.Empty;
        
    }
}
