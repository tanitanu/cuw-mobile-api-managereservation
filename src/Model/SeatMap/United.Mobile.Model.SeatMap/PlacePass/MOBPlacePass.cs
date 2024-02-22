﻿using System;

namespace United.Mobile.Model.SeatMap
{
    [Serializable()]
    public class MOBPlacePass
    {
        private int placePassID;
        private string destination;
        private string placePassImageSrc;
        private string offerDescription;
        private string placePassUrl;
        private string txtPoweredBy;
        private string txtPlacepass;

        public int PlacePassID
        {
            get { return placePassID; }
            set { placePassID = value; }
        }

        public string Destination
        {
            get { return destination; }
            set { destination = value; }
        }


        public string TxtPoweredBy
        {
            get { return this.txtPoweredBy; }
            set { this.txtPoweredBy = value; }
        }

        public string TxtPlacepass
        {
            get { return this.txtPlacepass; }
            set { this.txtPlacepass = value; }
        }
        public string PlacePassImageSrc
        {
            get
            {
                return this.placePassImageSrc;
            }
            set
            {
                this.placePassImageSrc = value;
            }
        }
        private string mobileImageUrl;

        public string MobileImageUrl
        {
            get { return mobileImageUrl; }
            set { mobileImageUrl = value; }
        }


        public string OfferDescription
        {
            get
            {
                return this.offerDescription;
            }
            set
            {
                this.offerDescription = value;
            }
        }
        public string PlacePassUrl
        {
            get
            {
                return this.placePassUrl;
            }
            set
            {
                this.placePassUrl = value;
            }
        }
    }
}
