using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using United.Mobile.DataAccess.Common;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping;
using United.Mobile.Model.Shopping.Bundles;
using United.Mobile.Model.Shopping.FormofPayment;

namespace United.Mobile.Model.Travelers
{
    [Serializable()]
    public class MOBBookingRegisterOfferResponse : RegisterOfferResponse
    {
        private MOBSHOPReservation reservation;
        private List<MOBTypeOption> disclaimer;
        private MOBSection promoCodeRemoveAlertForProducts;

        public MOBSHOPReservation Reservation
        {
            get { return reservation; }
            set { reservation = value; }
        }
        public List<MOBTypeOption> Disclaimer
        {
            get { return disclaimer; }
            set { disclaimer = value; }
        }

        public MOBSection PromoCodeRemoveAlertForProducts
        {
            get { return promoCodeRemoveAlertForProducts; }
            set { promoCodeRemoveAlertForProducts = value; }
        }
        private BookingBundlesResponse bundleResponse;

        public BookingBundlesResponse BundleResponse
        {
            get { return bundleResponse; }
            set { bundleResponse = value; }
        }

    }
}
