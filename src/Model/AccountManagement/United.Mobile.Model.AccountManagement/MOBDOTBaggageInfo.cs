using System;
using System.Collections.Generic;
using System.Text;
using United.Services.FlightShopping.Common.Extensions;

namespace United.Mobile.Model.MPSignIn
{
    [Serializable]
    public class MOBDOTBaggageInfo
    {
        private string title1 = string.Empty;
        private string description1 = string.Empty;
        private string title2 = string.Empty;
        private string description2 = string.Empty;
        private string title3 = string.Empty;
        private string description3 = string.Empty;
        private List<MOBBagFeesPerSegment> baggageFeesPerSegment;
        private string description4 = string.Empty;
        private string errorMessage = string.Empty;

        public string Title1
        {
            get { return title1; }
            set { title1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Description1
        {
            get { return description1; }
            set { description1 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Title2
        {
            get { return title2; }
            set { title2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Description2
        {
            get { return description2; }
            set { description2 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Title3
        {
            get { return title3; }
            set { title3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string Description3
        {
            get { return description3; }
            set { description3 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public List<MOBBagFeesPerSegment> BaggageFeesPerSegment
        {
            get { return baggageFeesPerSegment; }
            set { baggageFeesPerSegment = value; }
        }

        public string Description4
        {
            get { return description4; }
            set { description4 = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { errorMessage = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        public void SetDatabaseBaggageInfo(MOBDOTBaggageInfo databaseBaggageInfo)
        {
            Title1 = databaseBaggageInfo.Title1;
            Title2 = databaseBaggageInfo.Title2;
            Description1 = databaseBaggageInfo.Description1;
            Description2 = databaseBaggageInfo.Description2;
            Description4 = databaseBaggageInfo.Description4;
        }

        public void SetMerchandizingServicesBaggageInfo(MOBDOTBaggageInfo merchBaggageInfo)
        {
            if (!merchBaggageInfo.ErrorMessage.IsNullOrEmpty())
            {
                Title1 = string.Empty;
                Title2 = string.Empty;
                Title3 = string.Empty;
                Description1 = string.Empty;
                Description2 = string.Empty;
                Description3 = string.Empty;
                Description4 = string.Empty;
                ErrorMessage = merchBaggageInfo.ErrorMessage;
            }
            else
            {
                Title3 = merchBaggageInfo.Title3;
                Description3 = merchBaggageInfo.Description3;
                BaggageFeesPerSegment = merchBaggageInfo.BaggageFeesPerSegment;
            }
        }
    }
}
