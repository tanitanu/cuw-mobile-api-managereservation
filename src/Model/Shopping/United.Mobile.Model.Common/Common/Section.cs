using System;
using System.Xml.Serialization;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    [XmlRoot("MOBSection")]
    public class Section
    {
        public string Text1 { get; set; } = string.Empty;
        public string Text2 { get; set; } = string.Empty;
        public string Text3 { get; set; }
        public string Order { get; set; } 
        private string messageType;
        private bool isDefaultOpen = true;
        public string MessageType
        {
            get
            {
                return this.messageType;
            }
            set
            {
                this.messageType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public bool IsDefaultOpen
        {
            get
            {
                return this.isDefaultOpen;
            }
            set
            {
                this.isDefaultOpen = value;
            }
        }
    }
}
