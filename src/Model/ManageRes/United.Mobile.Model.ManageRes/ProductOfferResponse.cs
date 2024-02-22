using System;
using System.Collections.Generic;
using System.Text;
using United.Mobile.Model.Common;
using United.Mobile.Model.Shopping.Pcu;
using System.ComponentModel;

namespace United.Mobile.Model.ManageRes
{
    public class TravelOptionsResponse : MOBResponse
    {
        public string Title { get; set; }
        public List<ProductTile> ProductTiles { get; set; }
        public List<BundleDetails> BundleDetails { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public StandAloneProductResponse StandAloneProductResponse { get; set; }
        public string Flow { get; set; }
        public string SessionId { get; set; }
        public string CorrelationId { get; set; }
    }
    public class BundleDetails
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public List<BundleDescription> BundleDescriptions { get; set; }
        public BundleTrip TripDetails { get; set; }
        public List<Buttons> ActionButtons { get; set; }
        public MOBMobileCMSContentMessages TermsAndCondition { get; set; }
        private List<MOBMobileCMSContentMessages> AlertMessages { get; set; }
    }

    public class BundleDescription
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string PartialWarningMessage { get; set; }
        public List<EligibleAirport> EligibleAirportCodes { get; set; }
    }

    public class EligibleAirport
    {
        public bool IsEligibleAirport { get; set; }
        public string AirportName { get; set; }
        public string AirportCode { get; set; }
        public string Icon { get; set; }
    }

    public class BundleTrip
    {
        public string Title { get; set; }
        public string Header { get; set; }
        public int NumberOfTravelers { get; set; }
        public string TotalPriceLabel { get; set; }
        public List<SegmentDetail> SegmentDetails { get; set; }
    }

    public class SegmentDetail
    { 
        public decimal Price { get; set; }
        public decimal PriceForAllPax { get; set; }
        public string PriceText { get; set; }
        public string OriginDestination { get; set; }
        public bool IsEligibleSegment { get; set; }
        public bool IsChecked { get; set; }
        public string TripId { get; set; }
        public string ProductID { get; set; }
        public List<string> TripProductIDs { get; set; }
    }

    public class StandAloneProductResponse
    {
        public MOBAccelerators AwardAccelerators { get; set; }
        public MOBPremiumCabinUpgrade PremiumCabinUpgrade { get; set; }
        public United.Mobile.Model.MPRewards.MOBPriorityBoarding PriorityBoarding { get; set; }
        public MOBPremierAccess PremierAccess { get; set; }
    }

    public class Buttons
    {
        public string ActionText  { get; set; }
        public string ButtonText { get; set; }
        public int Rank { get; set; }
        public bool IsPrimary { get; set; }
        public bool IsEnabled { get; set; }
    }


    public enum ButtonActions
    {
        SEATMAP,
        PAYMENT,
        CANCEL
    }

    public class BundleDetailsPersist
    {
        public string Title { get; set; }
        public string ProductCode { get; set; }
        public List<BundleDescriptionPersist> BundleDescriptions { get; set; }
        public MOBMobileCMSContentMessages TermsAndCondition { get; set; }
    }

    public class BundleDescriptionPersist
    {
        public string Title { get; set; }
        public string ProductCode { get; set; }
        public string Description { get; set; }
    }

    public enum IneligibleReasonForBundle
    {

        [Description("PURCHASED")]
        PURCHASED,
        [Description("ONLY UA")]
        ONLYUA
    }
}

