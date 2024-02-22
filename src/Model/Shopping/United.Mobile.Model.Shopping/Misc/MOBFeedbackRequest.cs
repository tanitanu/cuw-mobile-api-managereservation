using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping
{
    [Serializable()]
    public class FeedbackRequest : MOBRequest
    {
        public FeedbackRequest()
            : base()
        {
        }

        private string feedType = string.Empty;
        private string pageSize = string.Empty;
        private string requestedPage = string.Empty;
        private double starRating;
        private string category = string.Empty;
        private string taskAnswer = string.Empty;
        private string mileagePlusAccountNumber = string.Empty;
        private string deviceModel = string.Empty;
        private string deviceOSVersion = string.Empty;
        private double latitude; 
        private double longitude;
        private string pnrs = string.Empty;
        private string answer1 = string.Empty;
        private string answer2 = string.Empty;

        public string FeedType
        {
            get
            {
                return this.feedType;
            }
            set
            {
                this.feedType = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string PageSize
        {
            get
            {
                return this.pageSize;
            }
            set
            {
                this.pageSize = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string RequestedPage
        {
            get
            {
                return this.requestedPage;
            }
            set
            {
                this.requestedPage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public double StarRating
        {
            get
            {
                return this.starRating;
            }
            set
            {
                this.starRating = value;
            }
        }

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string TaskAnswer
        {
            get
            {
                return this.taskAnswer;
            }
            set
            {
                this.taskAnswer = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
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

        public string DeviceModel
        {
            get
            {
                return this.deviceModel;
            }
            set
            {
                this.deviceModel = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string DeviceOSVersion
        {
            get
            {
                return this.deviceOSVersion;
            }
            set
            {
                this.deviceOSVersion = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public double Latitude
        {
            get
            {
                return this.latitude;
            }
            set
            {
                this.latitude = value;
            }
        }

        public double Longitude
        {
            get
            {
                return this.longitude;
            }
            set
            {
                this.longitude = value;
            }
        }

        public string Pnrs
        {
            get
            {
                return this.pnrs;
            }
            set
            {
                this.pnrs = string.IsNullOrEmpty(value) ? string.Empty : value.Trim().ToUpper();
            }
        }

        public string Answer1
        {
            get
            {
                return this.answer1;
            }
            set
            {
                this.answer1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }

        public string Answer2
        {
            get
            {
                return this.answer2;
            }
            set
            {
                this.answer2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim();
            }
        }
    }
}
