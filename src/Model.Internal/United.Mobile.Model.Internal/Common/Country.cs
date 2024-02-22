namespace United.Mobile.Model.Internal.Common
{
    public class Country
    {        
        public string CountryCode { get; set; }       
        public string ISOAlpha3Code { get; set; }        
        public string Name { get; set; }       
        public string ShortName { get; set; }      
        public string PhoneCountryCode { get; set; }      
        public Language DefaultLanguage { get; set; }      
        public virtual Currency DefaultCurrency { get; set; }      
        public virtual Status Status { get; set; }      
        public virtual int StateCount { get; set; }
    }
}
