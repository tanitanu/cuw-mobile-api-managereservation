using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class FeedbackResponse : MOBResponse
    {
        private FeedbackRequest request;
        private bool succeed;

        public FeedbackRequest Request
        {
            get
            {
                return this.request;
            }
            set
            {
                this.request = value;
            }
        }

        public bool Succeed
        {
            get
            {
                return this.succeed;
            }
            set
            {
                this.succeed = value;
            }
        }
    }
}
