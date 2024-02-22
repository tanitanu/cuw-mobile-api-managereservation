using System;
using System.Xml.Serialization;


namespace United.Mobile.Model.Common
{
    [Serializable()]
    [XmlRoot("MOBState")]
    public class State
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
