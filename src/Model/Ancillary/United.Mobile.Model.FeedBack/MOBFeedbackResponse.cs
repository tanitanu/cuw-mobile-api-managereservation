using System;

namespace United.Mobile.Model.FeedBack
{
    [Serializable]
    public class MOBFeedbackResponse : MOBResponse
    {
        public MOBFeedbackRequest Request { get; set; }
        public bool Succeed { get; set; }
    }
}
