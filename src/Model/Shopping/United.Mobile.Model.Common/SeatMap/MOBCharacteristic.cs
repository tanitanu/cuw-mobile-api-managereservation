using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [XmlRoot("MOBCharacteristic")]
    public class MOBCharacteristic
    {
        private string code;
        public string Code
        {
            get { return this.code; }
            set { this.code = value; }
        }

        private string value;
        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}