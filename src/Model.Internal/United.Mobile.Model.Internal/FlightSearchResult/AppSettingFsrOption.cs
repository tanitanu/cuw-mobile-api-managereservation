namespace United.Mobile.Model.Internal.FlightSearchResult
{
    public class AppSettingFsrOption
    {
        public int EmployeeFareWheelNumberDaysToDisplay { get; set; }
        public string FlightDateChangeMessage { get; set; }
        public string InvalidSessionMessage { get; set; }
        public string InvalidSessionTitle { get; set; }
        public string EResSessionObjName { get; set; }
        public string MpPinPwdValidateSessionName { get; set; }
        public string AdvanceBookingDaysXpath { get; set; }
        public string NoFlightFoundMessage { get; set; }
        public int FlightStatusDifferenceInHrs { get; set; }
        public string[] BoardingLegends { get; set; }
        public int InitialFlightSearchResultCount { get; set; }
        public int MaxFlightSearchResultCount { get; set; }
    }
}
