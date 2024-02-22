using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace United.Mobile.Model.ManageRes
{
    public class TravelOptions
    {
        public string Title { get; set; }
        public string SeeAllOffersLinkText { get; set; }
        public List<ProductTile> ProductTiles { get; set; }
        public string CorrelationId { get; set; }
    }

    public class ProductTile
    {
        public string ProductTitle { get; set; }
        public string ProductCode { get; set; }
        public string PriceText { get; set; }
        public string PriceSubText { get; set; }
        public List<ProductComponent> ProductDetails { get; set; }
        public string BadgeText { get; set; }
        public string BadgeBackgroundColor { get; set; }
        public string BadgeFontColor { get; set; }
        public string ProductName { get; set; }
        public bool showUpliftPerMonthPrice { get; set; }
        public decimal Amount { get; set; }
        public string AccessibilityText { get; set; }
        public string FromText { get; set; }
    }

    public class ProductComponent
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public enum ProductName
    {
        BUNDLE,
        TPI,
        PCU,
        PAS,
        PBS,
        APA,
        SEATS,
        ANC
    }
    public enum TravelOptionBadge
    {
        [Description("MOST POPULAR")]
        MOSTPOPULARBUNDLE,
        [Description(" PARTIAL BUNDLE")]
        PARTIAL
    }

    public enum TravelOptionBadgeBackgroundColor
    {
        [Description("#002244")]
        BLUE,
        [Description("#C6C6C6")]
        GREY
    }
    public enum TravelOptionBadgeFontColor
    {
        [Description("#FFFFFF")]
        WHITE,
        [Description("#000000")]
        BLACK
    }
}

