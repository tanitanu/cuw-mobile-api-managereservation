using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.FlightReservation
{
    [Serializable()]
    public class MOBReceiptByEmailRequest : MOBRequest
    {
        private string recordLocator = string.Empty;
        private string creationDate = string.Empty;
        private string emailAdress = string.Empty;
        private string emailAddress = string.Empty;

        public MOBReceiptByEmailRequest()
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
                this.recordLocator = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CreationDate
        {
            get
            {
                return this.creationDate;
            }
            set
            {
                this.creationDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
        [JsonPropertyName("emailAdress")]
        public string EMailAddress
        {
            get
            {
                return this.emailAddress;
            }
            set
            {
                this.emailAddress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string EMailAdress
        {
            get
            {
                return this.emailAdress;
            }
            set
            {
                this.emailAdress = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

    }
    public class SendRcptRequest
    {
        public string Client { get; set; }
        public string RcptType { get; set; }
        public string Resend { get; set; }
        public string RecLoc { get; set; }
        public string PnrCreateDt { get; set; }
        public string DocId { get; set; }
        public string DocCreateDt { get; set; }

        public string DlvryType { get; set; }
        public List<string> Email { get; set; }
    }

    public class SendRcptResponse
    {
        public string Status { get; set; }
        public SendRcptRequest SendRcptRequest { get; set; }
        public string ErrMsg { get; set; }
        public string TxnId { get; set; }
        public string Guid { get; set; }
    }
}
