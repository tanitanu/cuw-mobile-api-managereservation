namespace United.Mobile.Model.Common
{
    public enum TravelType
    {
        RA,//Revenue Booking/AwardTravel
        YA,//Young adult
        CLB,//Coroporateleisurebooking
        CB,
        D, // eRes Deviation Travel (Authorization Required)
        T, // eRes Training Travel (Authorization Required)
        E, // eRes Emergency Travel
        TPSearch = MOBTripPlannerType.TPSearch,
        TPBooking = MOBTripPlannerType.TPBooking,
        TPEdit = MOBTripPlannerType.TPEdit,
        TPTripDelete = MOBTripPlannerType.TPTripDelete,
        TPDeepLink = MOBTripPlannerType.TPDeepLink

    }

    public enum MOBTripPlannerType
    {
        TPSearch = 8,
        TPBooking,
        TPEdit,
        TPTripDelete,
        TPDeepLink,
    }
}
