using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FeedBack
{
    [Serializable]
    public class MOBFeedbackParametersResponse : MOBResponse
    {
        public string CategoryTitle { get; set; } = string.Empty;

        public List<MOBKVP> Categories { get; set; }

        public string TaskQuestion { get; set; } = string.Empty;

        public List<MOBKVP> TaskAnswers { get; set; }

        public MOBFeedbackParametersResponse()
        {
            Categories = new List<MOBKVP>();
            TaskAnswers = new List<MOBKVP>();
        }
    }
}
