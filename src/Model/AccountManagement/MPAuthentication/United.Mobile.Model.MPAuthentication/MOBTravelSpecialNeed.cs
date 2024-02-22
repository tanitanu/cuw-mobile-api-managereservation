using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.MPAuthentication
{
    [Serializable()]
    public class MOBTravelSpecialNeed
    {
        private string value;
        private string displayDescription;
        private string type;
        private string code;
        private string displaySequence;
        private List<MOBTravelSpecialNeed> subOptions;
        private List<MOBItem> messages;
        private string registerServiceDescription;
        private string subOptionHeader;


        public string DisplaySequence
        {
            get { return this.displaySequence; }
            set { this.displaySequence = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
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

        public string RegisterServiceDescription
        {
            get { return registerServiceDescription; }
            set { registerServiceDescription = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }

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

        public List<MOBTravelSpecialNeed> SubOptions
        {
            get { return subOptions; }
            set { subOptions = value; }
        }

        public string SubOptionHeader
        {
            get { return subOptionHeader; }
            set { subOptionHeader = string.IsNullOrWhiteSpace(value) ? string.Empty : value; }
        }
    }
}
