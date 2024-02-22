using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FlightStatusPushNotificationResponse : MOBResponse
    {
        private string apnsDeviceId = string.Empty;
        private string carrierCode = string.Empty;
        private string flightNumber = string.Empty;
        private string flightDate = string.Empty;
        private string origin = string.Empty;
        private string originName = string.Empty;
        private string destination = string.Empty;
        private string destinationName = string.Empty;
        private string requestId = "0";
        private string transId = string.Empty;
        private string succeed = string.Empty;
        private string flightStatusPushNotificationResponseFlag = "YES";

        public FlightStatusPushNotificationResponse()
            : base()
        {
        }

        public string APNSDeviceId
        {
            get
            {
                return this.apnsDeviceId;
            }
            set
            {
                this.apnsDeviceId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string CarrierCode
        {
            get
            {
                return this.carrierCode;
            }
            set
            {
                this.carrierCode = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string FlightNumber
        {
            get
            {
                return this.flightNumber;
            }
            set
            {
                this.flightNumber = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string FlightDate
        {
            get
            {
                return this.flightDate;
            }
            set
            {
                this.flightDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Origin
        {
            get
            {
                return this.origin;
            }
            set
            {
                this.origin = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string OriginName
        {
            get
            {
                return this.originName;
            }
            set
            {
                this.originName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RequestId
        {
            get
            {
                return this.requestId;
            }
            set
            {
                this.requestId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TransId
        {
            get
            {
                return this.transId;
            }
            set
            {
                this.transId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Destination
        {
            get
            {
                return this.destination;
            }
            set
            {
                this.destination = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string DestinationName
        {
            get
            {
                return this.destinationName;
            }
            set
            {
                this.destinationName = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Succeed
        {
            get
            {
                return this.succeed;
            }
            set
            {
                this.succeed = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string FlightStatusPushNotificationResponseFlag
        {
            get
            {
                return this.flightStatusPushNotificationResponseFlag;
            }
            set
            {
                this.flightStatusPushNotificationResponseFlag = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }
    }
}
