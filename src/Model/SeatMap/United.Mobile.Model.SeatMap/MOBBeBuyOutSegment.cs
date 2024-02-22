using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBBeBuyOutSegment
    {
        private string segment;
        private string message;
        private string warningIcon;
        private bool warning;

        public string Segment
        {
            get { return segment; }
            set { segment = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public string WarningIcon
        {
            get { return warningIcon; }
            set { warningIcon = value; }
        }

        public bool Warning
        {
            get { return warning; }
            set { warning = value; }
        }
    }
}
