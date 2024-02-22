using System.Collections.Generic;

namespace United.Mobile.Model.SeatMap
{
    public class ContextualCard : CouchbaseDocument
    {
        public string UserId { get; set; }
        public int CardType { get; set; }
        public ICollection<ActionItem> ActionItems { get; set; }
        public string FlightNumberAndStatus { get; set; }
        public string FlightStatusSegmentPredictableKey { get; set; }
        public ICollection<string> FlightRoute { get; set; }
        public string GateText { get; set; }
        public string GateValue { get; set; }
        public bool IsIrropPnr { get; set; }
        public bool IsDivertedSegment { get; set; }
        public SegmentToBeHighlighted SegmentToBeHighlighted { get; set; }
        public string TimeText { get; set; }
        public string TimeValue { get; set; }
        public string TravelDate { get; set; }
        public string TravelStatusText { get; set; }
        public string RecordLocator { get; set; }
        public ICollection<ActionItem> DisclosureItems { get; set; }
        public ICollection<UITemplateBlockContent> TripTipRows { get; set; }
        public InfoDoc InfoDoc { get; set; }
    }
}
