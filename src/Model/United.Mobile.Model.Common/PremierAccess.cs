namespace United.Mobile.Model.Common
{
    public class PremierAccess
    {
        //public PremierAccess();
        public string ObjectName { get; set; } = "United.PA";
        public string RecordLocator { get; set; }
        public string LastName { get; set; }
        public string CallDuration { get; set; }
        public string RequestXml { get; set; }
        public string ResponseXml { get; set; }
        public string PnrCreateDate { get; set; }
    }
}