using System;

namespace United.Mobile.Model.Shopping
{

    [Serializable()]
    public class AwardCalendarDay
    {
        private string week;

        public string Week
        {
            get { return week; }
            set { week = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private string priceMiles;
        public string PriceMiles
        {
            get { return priceMiles; }
            set { priceMiles = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

        private bool isLowest;
        public bool IsLowest
        {
            get { return isLowest; }
            set { isLowest = value; }
        }

        private string notAvailableText;
        public string NotAvailableText
        {
            get { return notAvailableText; }
            set { notAvailableText = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }


        private string originalDate;

        public string OriginalDate
        {
            get { return originalDate; }
            set { originalDate = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); }
        }

    }
}
