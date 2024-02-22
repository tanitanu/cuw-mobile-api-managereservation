using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;

namespace United.Mobile.Model.Shopping.UpgradeCabin
{
    [Serializable()]
    public class UpgradeCabinEligibilityResponse : MOBResponse
    {
        private string sessionId;
        private string cartId;
        private List<MOBSHOPTrip> trips;
        private List<UpgradeCabinSegment> segments;
        private List<UpgradeCabinSegment> milesUpgradeOption;
        private List<PNRPassenger> passengers;
        private List<MOBItem> contents;
        private string redirectUrl;
        private Boolean isEligible;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;
        private MOBSHOPResponseStatusItem responseStatusItem;
        private MOBPlusPoints plusPoints;
        private List<UpgradeCabinOptionContent> cabinOptionContents;
        private List<UpgradeCabinAdvisory> cabinOptionMessages;


        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string CartId { get { return this.cartId; } set { this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBSHOPTrip> Trips { get { return this.trips; } set { this.trips = value; } }
        public List<UpgradeCabinSegment> Segments { get { return this.segments; } set { this.segments = value; } }
        public List<UpgradeCabinSegment> MilesUpgradeOption { get { return this.milesUpgradeOption; } set { this.milesUpgradeOption = value; } }
        public List<PNRPassenger> Passengers { get { return this.passengers; } set { this.passengers = value; } }
        public List<MOBItem> Contents { get { return this.contents; } set { this.contents = value; } }
        public Boolean IsEligible { get { return this.isEligible; } set { this.isEligible = value; } }
        public string RedirectUrl { get { return this.redirectUrl; } set { this.redirectUrl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }
        public MOBSHOPResponseStatusItem ResponseStatusItem { get { return this.responseStatusItem; } set { this.responseStatusItem = value; } }
        public MOBPlusPoints PlusPoints { get { return this.plusPoints; } set { this.plusPoints = value; } }
        public List<UpgradeCabinOptionContent> CabinOptionContents { get { return this.cabinOptionContents; } set { this.cabinOptionContents = value; } }
        public List<UpgradeCabinAdvisory> CabinOptionMessages { get { return this.cabinOptionMessages; } set { this.cabinOptionMessages = value; } }
    }
}
