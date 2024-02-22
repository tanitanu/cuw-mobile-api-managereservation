using System;
using System.Collections.Generic;
using System.Text;

namespace United.Mobile.Model.Common
{
    [Serializable()]
    public class MOBPNRRemarkRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        private string remarkDescription = string.Empty;
        private string flow = string.Empty;
        private string mileagePlus = string.Empty;
        private string hashPin = string.Empty;
        private string sessionId = string.Empty;
        private string lastName = string.Empty;

        public MOBPNRRemarkRequest()
            : base()
        {
        }

        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = value;
            }
        }
        public string RemarkDescription
        {
            get
            {
                return this.remarkDescription;
            }
            set
            {
                this.remarkDescription = value;
            }
        }

        public string Flow { get { return this.flow; } set { this.flow = value; } }
        public string MileagePlus { get { return this.mileagePlus; } set { this.mileagePlus = value; } }
        public string HashPin { get { return this.hashPin; } set { this.hashPin = value; } }
        public string SessionId { get { return this.sessionId; } set { this.sessionId = value; } }
        public string LastName { get { return this.lastName; } set { this.lastName = value; } }

    }
}
