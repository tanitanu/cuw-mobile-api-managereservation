using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable]
    public class MOBInstantUpgradeRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        private string lastName = string.Empty;
        private string sessionId = string.Empty;
        private string segmentIndexes = string.Empty;
        private int applicationId;
        private string flow;

        public string Flow
        {
            get { return flow; }
            set { flow = value; }
        }


        public string RecordLocator
        {
            get
            {
                return this.recordLocator;
            }
            set
            {
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string LastName
        {
            get
            {
                return this.lastName;
            }
            set
            {
                this.lastName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string SessionId
        {
            get { return this.sessionId; }
            set
            {
                this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string SegmentIndexes
        {
            get { return this.segmentIndexes; }
            set
            {
                this.segmentIndexes = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public int ApplicationId
        {
            get { return this.applicationId; }
            set
            {
                this.applicationId = value;
            }
        }
    }
}
