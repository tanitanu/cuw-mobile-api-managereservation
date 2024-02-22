using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FeedbackParametersResponse : MOBResponse
    {
        private string categoryTitle = string.Empty;
        private List<MOBKVP> categories;
        private string taskQuestion = string.Empty;
        private List<MOBKVP> taskAnswers;

        public string CategoryTitle
        {
            get
            {
                return this.categoryTitle;
            }
            set
            {
                this.categoryTitle = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBKVP> Categories
        {
            get
            {
                return this.categories;
            }
            set
            {
                this.categories = value;
            }
        }

        public string TaskQuestion
        {
            get
            {
                return this.taskQuestion;
            }
            set
            {
                this.taskQuestion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public List<MOBKVP> TaskAnswers
        {
            get
            {
                return this.taskAnswers;
            }
            set
            {
                this.taskAnswers = value;
            }
        }
    }
}
