using System;

namespace United.Utility
{
    public static partial class PredictableKey
    {
        public static string DocumentLibrary = Delimitters.DocumentLibraryPrefix;
     
        public static string FlightStatusKey(string carrierCode, string flightNumber, string flightDate, string departure, string arrival)
        {
            string formattedDateStringForFlifoPredicatbleKey;
            if (DateTime.TryParse(flightDate, out DateTime dateTime))
                formattedDateStringForFlifoPredicatbleKey = dateTime.ToString("yyyyMMdd");
            else
                formattedDateStringForFlifoPredicatbleKey = flightDate;

            return $"{Delimitters.FLIFOPrefix}{Delimitters.PredictableKeyDoubleSeparator}{carrierCode}{Delimitters.PredictableKeyDoubleSeparator}{flightNumber}{Delimitters.PredictableKeyDoubleSeparator}{formattedDateStringForFlifoPredicatbleKey}{Delimitters.PredictableKeyDoubleSeparator}{departure}{Delimitters.PredictableKeyDoubleSeparator}{arrival}";
        }

        public static string ToMobilePassportControlOfferKey(string user)
        {
            return $"{Delimitters.MobilePassportControlOfferPrefix}{Delimitters.PredictableKeyDoubleSeparator}{user}";
        }
        
        public static string LookUpAccountDetailsFromMileagePlusNumber(string mpNumber)
        {
            //LOOKUP::ACCOUNT::MPNUMBER
            return $"{Delimitters.LookupPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.AccountPrefix}{Delimitters.PredictableKeyDoubleSeparator}{mpNumber.ToUpper()}";
        }

        public static string ToContextualCardKey(string userId)
        {
            return $"{Delimitters.ContextualAwarenessCard}{Delimitters.PredictableKeyDoubleSeparator}{userId}";
        }

        public static string LookUpReservationsForUserKey(string user, string mpNumber = "")
        {
            if (string.IsNullOrEmpty(mpNumber))
                //LOOKUP::RESERVATION::SP_XD@DeviceId
                return $"{Delimitters.LookupPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.ReservationPrefix}{Delimitters.PredictableKeyDoubleSeparator}{user}";

            //LOOKUP::RESERVATION::MPNUMBER
            return $"{Delimitters.LookupPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.ReservationPrefix}{Delimitters.PredictableKeyDoubleSeparator}{mpNumber.ToUpper()}";
        }

        public static string ToInboxConfigKey(string mpNumber, string deviceId)
        {
            var key = $"{Delimitters.InboxPrefix}{Delimitters.PredictableKeySeparator}{deviceId}";

            if (!string.IsNullOrEmpty(mpNumber))
                key = $"{key}{Delimitters.PredictableKeySeparator}{mpNumber}";

            return key;
        }

        public static string ToPreBoardTrackerKey(string flifoPredictableKey)
        {
            return $"{Delimitters.PreBoardTrackerPrefix}{Delimitters.PredictableKeyDoubleSeparator}{flifoPredictableKey}";
        }

        public static string ToTokenPredictableKey(string deviceId, string applicationId)
        {
            //ANONYMOUSTOKEN::{0}::{1}
            return $"{Delimitters.TokenPrefix}{Delimitters.PredictableKeyDoubleSeparator}{deviceId}{Delimitters.PredictableKeyDoubleSeparator}{applicationId}";
        }

        public static string ToReservationKey(string recordLocator)
        {
            return $"{Delimitters.ReservationPrefix}{Delimitters.PredictableKeyDoubleSeparator}{recordLocator.ToUpper()}";
        }

        public static string ToDeviceConfigKey(string userId)
        {
            return $"{Delimitters.DevicePrefix}{Delimitters.PredictableKeyDoubleSeparator}{userId}";
        }

        public static string GetPreBoardTrackerStationsKey()
        {
            //PREBOARDTRACKER::PREBOARDTRACKERSTATIONS
            return $"{Delimitters.PreBoardTrackerPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.PreBoardTrackerStations}";
        }

        public static string FlightStatusPreBoardTrackerKey(string flightStatusKey)
        {
            return $"{Delimitters.PreBoardTrackerPrefix}{Delimitters.PredictableKeyDoubleSeparator}{flightStatusKey}";
        }
        
        public static string ToAirportInfoKey(string airportCode)
        {
            return $"{Delimitters.AirportPrefix}{Delimitters.PredictableKeyDoubleSeparator}{airportCode}";
        }

        public static string GetMessageDisclosureFiltersKey()
        {
            // FEATUREFILTERS::MESSAGEDISCLOSUREFILTERS
            return $"{Delimitters.FeatureFiltersPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.MessageDisclosureFilters}";
        }
        //Sync up code from onprem
        public static string GetConnectionDisclosureFiltersKey()
        {
            // FEATUREFILTERS::MESSAGEDISCLOSUREFILTERS
            return $"{Delimitters.FeatureFiltersPrefix}{Delimitters.PredictableKeyDoubleSeparator}{Delimitters.ConnectionDisclosureFilters}";
        }

        public static string FlightPredictableKey(string flightNumber, string flightDate)
        {
            return $"{Delimitters.FlightPrefix}{Delimitters.PredictableKeyDoubleSeparator}{flightNumber}{Delimitters.PredictableKeyDoubleSeparator}{flightDate}";
        }

        public static string FlightSegmentPredictableKey(string flightNumber, string flightDate, string origin, string destination)
        {
            return $"{Delimitters.ScheduledFlightSegment}{Delimitters.PredictableKeyDoubleSeparator}{flightNumber}{Delimitters.PredictableKeyDoubleSeparator}{flightDate}{Delimitters.PredictableKeyDoubleSeparator}{origin}{Delimitters.PredictableKeyDoubleSeparator}{destination}";
        }
    }
}
