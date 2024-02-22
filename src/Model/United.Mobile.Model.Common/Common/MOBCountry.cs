using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBCountry
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string PhoneCode { get; set; } = string.Empty;

    }
}
