using System;
using System.Collections.Generic;
using United.Mobile.Model.Common;
using United.Mobile.Model.SeatMap;

namespace United.Mobile.Model.UpgradeCabin
{
    [Serializable()]
    public class MOBUpgradeCabinEligibilityResponse : MOBResponse
    {
        public string ObjectName = "United.Mobile.Model.UpgradeCabin.MOBUpgradeCabinEligibilityResponse";
        private string sessionId;
        private string cartId;
        private List<MOBTrip> trips;
        private List<MOBUpgradeCabinSegment> segments;
        private List<MOBUpgradeCabinSegment> milesUpgradeOption;
        private List<MOBPNRPassenger> passengers;
        private List<MOBItem> contents;
        private string redirectUrl;
        private Boolean isEligible;
        private string webShareToken = string.Empty;
        private string webSessionShareUrl = string.Empty;
        private MOBSHOPResponseStatusItem responseStatusItem;
        private MOBPlusPoints plusPoints;
        private List<MOBUpgradeCabinOptionContent> cabinOptionContents;
        private List<MOBUpgradeCabinAdvisory> cabinOptionMessages;


        public string SessionId { get { return this.sessionId; } set { this.sessionId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string CartId { get { return this.cartId; } set { this.cartId = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public List<MOBTrip> Trips { get { return this.trips; } set { this.trips = value; } }
        public List<MOBUpgradeCabinSegment> Segments { get { return this.segments; } set { this.segments = value; } }
        public List<MOBUpgradeCabinSegment> MilesUpgradeOption { get { return this.milesUpgradeOption; } set { this.milesUpgradeOption = value; } }
        public List<MOBPNRPassenger> Passengers { get { return this.passengers; } set { this.passengers = value; } }
        public List<MOBItem> Contents { get { return this.contents; } set { this.contents = value; } }
        public Boolean IsEligible { get { return this.isEligible; } set { this.isEligible = value; } }
        public string RedirectUrl { get { return this.redirectUrl; } set { this.redirectUrl = string.IsNullOrEmpty(value) ? string.Empty : value.Trim(); } }
        public string WebShareToken { get { return this.webShareToken; } set { this.webShareToken = value; } }
        public string WebSessionShareUrl { get { return this.webSessionShareUrl; } set { this.webSessionShareUrl = value; } }
        public MOBSHOPResponseStatusItem ResponseStatusItem { get { return this.responseStatusItem; } set { this.responseStatusItem = value; } }
        public MOBPlusPoints PlusPoints { get { return this.plusPoints; } set { this.plusPoints = value; } }
        public List<MOBUpgradeCabinOptionContent> CabinOptionContents { get { return this.cabinOptionContents; } set { this.cabinOptionContents = value; } }
        public List<MOBUpgradeCabinAdvisory> CabinOptionMessages { get { return this.cabinOptionMessages; } set { this.cabinOptionMessages = value; } }
    }
}
