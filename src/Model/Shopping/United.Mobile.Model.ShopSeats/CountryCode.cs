using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.ShopSeats
{
    [Serializable()]
    [XmlRoot("Country")]
    //renamed Country (actual MRest model name) to CountryZipCode
    public class CountryZipCode
    {
        public virtual string CountryCode { get; set; }
    }
}
