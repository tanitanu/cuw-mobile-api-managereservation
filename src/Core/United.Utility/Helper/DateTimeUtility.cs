using System;
using System.Globalization;
using TimeZoneConverter;

namespace United.Utility.Helper
{
    public static class DateTimeUtility
    {
        private static string[] dateFormats = new string[] { "M/d/yyyy h:mm:ss tt", "M/d/yyyy h:mm tt",
                         "MM/dd/yyyy hh:mm:ss", "M/d/yyyy h:mm:ss",
                         "M/d/yyyy hh:mm tt", "M/d/yyyy hh tt",
                         "M/d/yyyy h:mm","MM/dd/yyyy hh:mm", "M/dd/yyyy hh:mm","M-d-yyyy h:mm:ss tt", "M-d-yyyy h:mm tt",
                         "MM-dd-yyyy hh:mm:ss", "M-d-yyyy h:mm:ss",
                         "M-d-yyyy hh:mm tt", "M-d-yyyy hh tt",
                         "M-d-yyyy h:mm","MM-dd-yyyy hh:mm", "M-dd-yyyy hh:mm",
                         "yyyy/MM/dd h:mm:ss tt", "yyyy/MM/dd h:mm tt",
                         "yyyy/MM/dd hh:mm:ss", "yyyy/MM/dd h:mm:ss",
                         "yyyy/MM/dd hh:mm tt", "yyyy/MM/dd hh tt",
                         "yyyy/MM/dd h:mm","yyyy/MM/dd hh:mm",
                         "yyyy-MM-dd h:mm:ss tt", "yyyy-MM-dd h:mm tt",
                         "yyyy-MM-dd hh:mm:ss", "yyyy-MM-dd h:mm:ss",
                         "yyyy-MM-dd hh:mm tt", "yyyy-MM-dd hh tt",
                         "yyyy-MM-dd h:mm","yyyy-MM-dd hh:mm",
                         "M/d/yyyy","d/M/yyyy","yyyy/M/d",
                         "MM/dd/yyyy","dd/MM/yyyy","yyyy/MM/dd",
                         "M-d-yyyy","d-M-yyyy","yyyy-MM-d",
                         "dd-MM-yyyy","yyyy-MM-dd"
        };
        public static string FormatDateTimeToAmPmWithoutTimeZone(this string dateTimeString)
        {
            if (string.IsNullOrEmpty(dateTimeString) || string.IsNullOrWhiteSpace(dateTimeString)) return string.Empty;
            // Remove time zone from datetime.Eg:"2019-12-12T16:44:00-08:00" (remove SFO time zone (08:00) and convert into local time)
            dateTimeString = dateTimeString.Length > 19 ? dateTimeString.Remove(19) : dateTimeString;
            if (!DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTimeString;
            }

            var formattedDateTimeString = dateTime.ToString("MM/dd/yyyy hh:mm tt");

            return formattedDateTimeString;
        }

        public static string FormatDateTimeToAmPm(this DateTime dateTime)
        {
            var formattedDateTimeString = string.Empty;

            formattedDateTimeString = dateTime.ToString("MM/dd/yyyy hh:mm tt");

            return formattedDateTimeString;
        }

        public static string FormatDateTimeToAmPm(this string dateTimeString)
        {
            if (!DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTimeString;
            }

            var formattedDateTimeString = dateTime.ToString("MM/dd/yyyy hh:mm tt");

            return formattedDateTimeString;
        }

        public static DateTime ToDateTime(this string dateTimeString, string languageCode = "en-US")
        {
            DateTime dateTime;

            if (DateTime.TryParseExact(dateTimeString, new string[] { "MM/dd/yyyy hh:mm tt", "yyyy-MM-dd", "yyyyMMdd", "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                return dateTime;
            }
            else if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                return dateTime;
            }

            return dateTime;
        }

        public static DateTime ToUtcTime(this DateTime localTime, string timeZoneId)
        {
            var tzi = TZConvert.GetTimeZoneInfo(timeZoneId);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(localTime, tzi);
            return utcTime;
        }

        public static DateTime ToLocalTime(this DateTime utcTime, string timeZoneId)
        {
            var tzi = TZConvert.GetTimeZoneInfo(timeZoneId);
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(utcTime, tzi);
            return localTime;
        }

        public static string ToTimeAmPm(this string dateTimeString, string languageCode = "en-US")
        {
            string formattedTime = string.Empty;

            if (!DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTimeString;
            }

            formattedTime = dateTime.ToString("h:mmtt").ToLower();

            return formattedTime;
        }

        public static string MilitaryFormatDateTimeForTTL(this DateTime dateTime, string languageCode = "en-US")
        {
            var formattedDateTimeString = string.Empty;

            formattedDateTimeString = dateTime.ToString("yyyy-MM-dd~HH:mm:sszzz").Replace('~', 'T');

            return formattedDateTimeString;
        }

        public static bool TimeDifferenceFromUtcNowGreatherThanInterval(DateTime? dateTimeUtc, int timeInterval)
        {
            bool timeDifferenceGreterThanInterval = false;

            if (dateTimeUtc.HasValue)
            {
                TimeSpan timeDifference = DateTime.UtcNow - dateTimeUtc.Value;

                timeDifferenceGreterThanInterval = timeDifference.TotalMinutes > timeInterval;
            }

            return timeDifferenceGreterThanInterval;
        }

        public static DateTime GetUtcFromAnotherUtcLocalDateTimePair(DateTime inputLocalDateTime, DateTime inputUtcDateTime, DateTime outputLocalDateTime)
        {
            TimeSpan timeZoneTimeSpan = inputUtcDateTime - inputLocalDateTime;

            DateTime outputUtcDateTime = outputLocalDateTime.Add(timeZoneTimeSpan);

            return outputUtcDateTime;
        }

        public static string FormatDateTimeToMilitary(this DateTime dateTime, string languageCode = "en-US")
        {
            var formattedDateTimeString = string.Empty;

            formattedDateTimeString = dateTime.ToString("yyyy-MM-dd HH:mm:ss");

            return formattedDateTimeString;
        }

        public static string FormatDateTimeToMilitary(this string dateTimeString, string languageCode = "en-US")
        {
            if (string.IsNullOrEmpty(dateTimeString))
            {
                return dateTimeString;
            }

            if (!DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTimeString;
            }

            //2019 - 01 - 06T17: 16:40
            var formattedDateTimeString = dateTime.ToString("yyyy-MM-ddTHH:mm:ss");

            return formattedDateTimeString;
        }

        public static string FormatDateTimeToMilitary(this string dateTimeString, string dateformat, string languageCode = "en-US")
        {
            if (!DateTime.TryParseExact(dateTimeString, dateformat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime))
            {
                return dateTimeString;
            }

            //2019 - 01 - 06T17: 16:40
            var formattedDateTimeString = dateTime.ToString("yyyy-MM-dd~HH:mm:ssZ").Replace('~', 'T');

            return formattedDateTimeString;
        }

        public static string FormatDateTimeForTTL(this DateTime dateTime, string languageCode = "en-US")
        {
            return dateTime.ToString("yyyy-MM-dd~HH:mm:sszzz").Replace('~', 'T');
        }

        public static string FormatDatetime(string dataTime, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;
            //Tue Jun 7 2011 14:53:00
            DateTime dt;
            if (DateTime.TryParseExact(dataTime, new string[] { "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss", "yyyy-MM-ddTHH:mm:ss" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                CultureInfo cultureInfo = null;
                try
                {
                    cultureInfo = new CultureInfo(languageCode);
                }
                catch (System.Exception)
                {
                    cultureInfo = new CultureInfo("en-US");
                }
                formattedDateTime = dt.ToString("g", cultureInfo);
            }

            return formattedDateTime;
        }

        public static string FormatDateTime(DateTime dataTime, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;
            CultureInfo cultureInfo = null;
            try
            {
                cultureInfo = new CultureInfo(languageCode);
            }
            catch (System.Exception)
            {
                cultureInfo = new CultureInfo("en-US");
            }
            formattedDateTime = dataTime.ToString("g", cultureInfo);

            return formattedDateTime;
        }

        public static string FormatDateTimeForFLIFOLookUp(string dataTime, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;
            //20110730
            DateTime dt;
            if (DateTime.TryParseExact(dataTime, new string[] { "MM/dd/yyyy hh:mm tt", "yyyy-MM-dd" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                CultureInfo cultureInfo = null;

                try
                {
                    cultureInfo = new CultureInfo(languageCode);
                }
                catch (System.Exception)
                {
                    cultureInfo = new CultureInfo("en-US");
                }

                formattedDateTime = dt.ToString("yyyyMMdd", cultureInfo);
            }

            return formattedDateTime;
        }

        public static string To24HourTimeWithSeconds(this string dateTimeString, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;

            var dateTime = new DateTime();

            //20110730
            if (DateTime.TryParseExact(dateTimeString, new string[] { "yyyyMMdd", "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                formattedDateTime = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
            }
            else if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                formattedDateTime = dateTime.ToString("MM/dd/yyyy HH:mm:ss");
            }
            else
            {
                return dateTimeString;
            }

            return formattedDateTime;
        }

        public static string ToFormattedTimeOnlyAmPm(this string dateTimeString, string languageCode = "en-US")
        {
            var formattedTimeOnly = string.Empty;

            if (!DateTime.TryParse(dateTimeString, out DateTime dateTime))
            {
                return dateTimeString;
            }

            formattedTimeOnly = dateTime.ToString("hh:mm tt");

            return formattedTimeOnly;
        }

        public static string ToFlifoShortDate(this string dateTimeString, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;

            var dateTime = new DateTime();

            //20110730
            if (DateTime.TryParseExact(dateTimeString, new string[] { "yyyyMMdd", "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                formattedDateTime = dateTime.ToString("yyyy-MM-dd");
            }
            else if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                formattedDateTime = dateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                return dateTimeString;
            }

            return formattedDateTime;
        }

        public static string ToFlightDateFormat(this string dateString)
        {
            var formattedDateOnlyString = string.Empty;

            var dateTime = new DateTime();

            if (DateTime.TryParseExact(dateString, new string[] { "yyyyMMdd", "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                formattedDateOnlyString = dateTime.ToString("yyyy-MM-dd");
            }
            else if (DateTime.TryParse(dateString, out dateTime))
            {
                formattedDateOnlyString = dateTime.ToString("yyyy-MM-dd");
            }
            else
            {
                return dateString;
            }

            return formattedDateOnlyString;
        }


        public static string ToFlightDateFormatForBoardingNow(this string dateString)
        {
            var formattedDateOnlyString = string.Empty;

            var dateTime = new DateTime();

            if (DateTime.TryParseExact(dateString, new string[] { "yyyyMMdd", "ddMMyyyy" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
            {
                formattedDateOnlyString = dateTime.ToString("yyyyMMdd");
            }
            else if (DateTime.TryParse(dateString, out dateTime))
            {
                formattedDateOnlyString = dateTime.ToString("yyyyMMdd");
            }
            else
            {
                return dateString;
            }

            return formattedDateOnlyString;
        }




        public static string ToLongDateFormat(this string dateTimeString, string languageCode = "en-US")
        {
            string formattedDateTime = string.Empty;

            var dateTime = new DateTime();

            if (DateTime.TryParse(dateTimeString, out dateTime))
            {
                formattedDateTime = dateTime.ToString("ddd., MMM d, yyyy");
            }
            else
            {
                return dateTimeString;
            }

            return formattedDateTime;
        }

        public static string ValidateAnyDateToString(this string inputLocalDateTime, string outputFormat)
        {
            DateTime dt = DateTime.MinValue;
            DateTime.TryParseExact(inputLocalDateTime, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            return dt.ToString(outputFormat);

        }

        public static DateTime ValidateAnyDateToDate(this string inputLocalDateTime)
        {
            DateTime dt = DateTime.MinValue;
            DateTime.TryParseExact(inputLocalDateTime, dateFormats, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);
            return dt;

        }

    }
}
