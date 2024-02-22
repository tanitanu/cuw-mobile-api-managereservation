using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("MOBAddress")]
    public class MOBAddress
    {
        public string Key { get; set; } = string.Empty;
        public SHOPChannel Channel { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string Line1 { get; set; } = string.Empty;
        public string Line2 { get; set; } = string.Empty;
        public string Line3 { get; set; } = string.Empty;
        public string ApartmentNumber { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public State State { get; set; }
        public MOBCountry Country { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsDefault { get; set; }
        public bool IsPrimary { get; set; }
        public string PostalCode { get; set; } = string.Empty;
        public bool IsCorporate { get; set; }
        public bool IsValidForTPIPurchase { get; set; }
    }



    //[Serializable()]
    //public class Country
    //{
    //    public string Code { get; set; } = string.Empty;
    //    public string Name { get; set; } = string.Empty;
    //    public string PhoneCode { get; set; } = string.Empty;
    //}
}
