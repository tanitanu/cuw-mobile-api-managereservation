using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMapEngine
{
    [Serializable]
    public class MOBSHOPSegmentInfoAlerts
    {
        private string alertMessage;
        private bool alignLeft;
        private bool alignRight;
        private string alertType;
        private string sortOrder;
        private string visibility;
        private string key;

        public string AlertMessage
        {
            get { return alertMessage; }
            set { alertMessage = value; }
        }
        public bool AlignLeft
        {
            get { return alignLeft; }
            set { alignLeft = value; }
        }
        public bool AlignRight
        {
            get { return alignRight; }
            set { alignRight = value; }
        }
        public string AlertType
        {
            get { return alertType; }
            set { alertType = value; }
        }
        public string SortOrder
        {
            get { return sortOrder; }
            set { sortOrder = value; }
        }
        public string Visibility
        {
            get { return visibility; }
            set { visibility = value; }
        }

        public string Key
        {
            get { return key; }
            set { key = value; }
        }
    }

    public enum MOBSHOPSegmentInfoDisplay
    {
        FSRCollapsed, // Display only at flgiht block level
        FSRExpanded, // Display only at Flight Details level
        FSRAll // Display in both Flight block and details level
    }
}
