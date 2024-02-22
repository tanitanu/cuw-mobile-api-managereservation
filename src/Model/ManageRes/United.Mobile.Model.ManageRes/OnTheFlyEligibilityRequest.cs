namespace United.Mobile.Model.ManageRes
{
    public class OnTheFlyEligibilityRequest
    {
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public int Channel { get; set; }
        public bool LastNameBypass { get; set; }
        public bool ReservationBypass { get; set; }

    }
    public class OnTheFlyEligibility
    {
        private bool offerEligible;
        public bool OfferEligible
        {
            get { return offerEligible; }
            set { offerEligible = value; }
        }
    }
}
