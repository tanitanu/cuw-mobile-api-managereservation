using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class MOBPromoFeedbackRequest : MOBRequest
    {
        public MOBPromoFeedbackRequest()
            : base()
        {
        }

        private string mileagePlusAccountNumber = string.Empty;
        
        private string messageKey = string.Empty;
        private MOBPromoFeedbackEventType eventType;
        private string sessionId = string.Empty;
        public string SessionId
        {
            get
            {
                return this.sessionId;
            }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public MOBPromoFeedbackEventType EventType
        {
            get { return eventType; }
            set { eventType = value; }
        }

        public string MessageKey
        {
            get { return messageKey; }
            set { messageKey = value; }
        }

        public string MileagePlusAccountNumber
        {
            get
            {
                return this.mileagePlusAccountNumber;
            }
            set
            {
                this.mileagePlusAccountNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }

    public enum MOBPromoFeedbackEventType
    {
        PRESENTED,
        CLICK
    }

}
