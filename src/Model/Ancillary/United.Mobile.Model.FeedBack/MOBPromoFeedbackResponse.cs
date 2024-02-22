using System;

namespace United.Mobile.Model.FeedBack
{

    [Serializable]
    public class MOBPromoFeedbackResponse : MOBResponse
    {
        public MOBPromoFeedbackRequest Request { get; set; }
        public bool Succeed { get; set; }
    }
}
