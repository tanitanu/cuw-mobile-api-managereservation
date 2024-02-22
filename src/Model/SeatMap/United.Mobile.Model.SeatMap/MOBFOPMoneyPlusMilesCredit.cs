using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBFOPMoneyPlusMilesCredit
    {
        private List<MOBMobileCMSContentMessages> reviewMMCMessage;
        private List<MOBMobileCMSContentMessages> mmcMessages;
        private List<MOBFOPMoneyPlusMiles> milesPlusMoneyOptions;
        private MOBFOPMoneyPlusMiles selectedMoneyPlusMiles;
        private MOBSection promoCodeMoneyMilesAlertMessage;

        public MOBSection PromoCodeMoneyMilesAlertMessage
        {
            get { return promoCodeMoneyMilesAlertMessage; }
            set { promoCodeMoneyMilesAlertMessage = value; }
        }
        public MOBFOPMoneyPlusMiles SelectedMoneyPlusMiles
        {
            get { return selectedMoneyPlusMiles; }
            set { selectedMoneyPlusMiles = value; }
        }


        public List<MOBMobileCMSContentMessages> ReviewMMCMessage
        {
            get { return reviewMMCMessage; }
            set { reviewMMCMessage = value; }
        }

        public List<MOBMobileCMSContentMessages> MMCMessages
        {
            get { return mmcMessages; }
            set { mmcMessages = value; }
        }

        public List<MOBFOPMoneyPlusMiles> MilesPlusMoneyOptions
        {
            get { return milesPlusMoneyOptions; }
            set { this.milesPlusMoneyOptions = value; }
        }

        private string totalMilesAvailable;
        public string TotalMilesAvailable
        {
            get { return totalMilesAvailable; }
            set { totalMilesAvailable = value; }
        }

        private string mileagePlusTraveler;
        public string MileagePlusTraveler
        {
            get { return mileagePlusTraveler; }
            set { mileagePlusTraveler = value; }
        }

    }

}
