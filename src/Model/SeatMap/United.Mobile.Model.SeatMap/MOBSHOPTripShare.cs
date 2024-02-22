using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable]
    public class MOBSHOPTripShare : MOBResponse
    {
        private string commonCaption;
        private string url;
        private MOBSHOPTripShareMessage email;
        private string placeholderTitle;

        private bool showShareTrip;

        public bool ShowShareTrip
        {
            get { return showShareTrip; }
            set { showShareTrip = value; }
        }


        public string PlaceholderTitle
        {
            get { return placeholderTitle; }
            set { placeholderTitle = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string CommonCaption
        {
            get { return commonCaption; }
            set { commonCaption = value; }
        }
        public MOBSHOPTripShareMessage Email
        {
            get { return email; }
            set { email = value; }
        }
    }

    [Serializable]
    public class MOBSHOPTripShareMessage
    {
        private string subject;

        private string body;

        public string Body
        {
            get { return body; }
            set { body = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { subject = value; }
        }
    }
}
