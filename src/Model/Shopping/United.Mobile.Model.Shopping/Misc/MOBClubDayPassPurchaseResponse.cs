using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Shopping.Misc;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class ClubDayPassPurchaseResponse :MOBResponse
    {
        public ClubDayPassPurchaseResponse()
            : base()
        {
        }

        private ClubDayPassPurchaseRequest request;
        private List<ClubDayPass> passes;

        public ClubDayPassPurchaseRequest Request
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

        public List<ClubDayPass> Passes
        {
            get
            {
                return this.passes;
            }
            set
            {
                this.passes = value;
            }
        }
    }

    [Serializable()]
    public class OTPPurchaseResponse :MOBResponse
    {
        public OTPPurchaseResponse()
            : base()
        {
        }

        private OTPPurchaseRequest request;
        private List<ClubDayPass> passes;
        private string sessionId;

        public OTPPurchaseRequest Request
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

        public List<ClubDayPass> Passes
        {
            get
            {
                return this.passes;
            }
            set
            {
                this.passes = value;
            }
        }

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }
    }
}
