using System;
using System.Collections.Generic;
using System.Text;

namespace United.Common.Helper.Enum
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
        TPBooking = MOBTripPlannerType.TPBooking
    }

    public enum MOBTripPlannerType
    {
        TPSearch,
        TPBooking
    }
}
