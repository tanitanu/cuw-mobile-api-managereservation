using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBInfoWarningMessages
    {

        private string order;
        public string Order
        {
            get { return order; }
            set { order = value; }
        }

        private string iconType;
        public string IconType
        {
            get { return iconType; }
            set { iconType = value; }
        }

        private List<string> messages;

        public List<string> Messages
        {
            get { return messages; }
            set { messages = value; }
        }
        private string buttonLabel;

        public string ButtonLabel
        {
            get { return buttonLabel; }
            set { buttonLabel = value; }
        }
        private string headerMessage;
        public string HeaderMessage
        {
            get { return headerMessage; }
            set { headerMessage = value; }
        }

        private bool isCollapsable;

        public bool IsCollapsable
        {
            get { return isCollapsable; }
            set { isCollapsable = value; }
        }
        private bool isExpandByDefault;

        public bool IsExpandByDefault
        {
            get { return isExpandByDefault; }
            set { isExpandByDefault = value; }
        }

    }
}
