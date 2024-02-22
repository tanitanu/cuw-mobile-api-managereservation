using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace United.Mobile.Model.Common.SSR
{
    [Serializable()]
    public class TravelSpecialNeed
    {
        private string value;
        private string displayDescription;
        private string type;
        private string code;
        public string displaySequence { get; set; }
        private List<TravelSpecialNeed> subOptions;
        private List<MOBItem> messages;
        private string subOptionHeader;
        private bool isDisabled;
        private string informationLink;

        public string SubOptionHeader
        {
            get { return subOptionHeader; }
            set { subOptionHeader = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }


        public string Value
        {
            get { return value; }
            set { this.value = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }

        public string Code
        {
            get { return code; }
            set { code = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }

        public string DisplayDescription
        {
            get { return displayDescription; }
            set { displayDescription = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }

        public string RegisterServiceDescription { get; set; }

        /// <summary>
        /// optional
        /// </summary>
        public string Type
        {
            get { return type; }
            set { type = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }

        public List<MOBItem> Messages
        {
            get { return messages; }
            set { messages = value; }
        }

        [XmlArrayItem("MOBTravelSpecialNeed")]
        public List<TravelSpecialNeed> SubOptions
        {
            get { return subOptions; }
            set { subOptions = value; }
        }

        public bool IsDisabled
        {
            get { return isDisabled; }
            set { isDisabled = value; }
        }

        public string InformationLink
        {
            get { return informationLink; }
            set { informationLink = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }
    }
}
