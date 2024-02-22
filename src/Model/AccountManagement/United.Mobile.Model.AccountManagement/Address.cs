using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("Address")]
    public class OneClickEnrollmentAddress
    {
        // public string Type { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string StateCode { get; set; }
        public string PostalCode { get; set; }
        public string CountryCode { get; set; }
        //public string InsertId { get; set; }
    }
}
