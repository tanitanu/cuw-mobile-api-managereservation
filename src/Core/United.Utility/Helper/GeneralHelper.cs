using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace United.Utility.Helper
{
    public class GeneralHelper
    {
        public static bool IsApplicationVersionGreaterorEqual(int applicationID, string appVersion, string androidnontfaversion, string iphonenontfaversion)
        {
            #region Nizam Code for version check
            bool ValidTFAVersion = false;
            if (!string.IsNullOrEmpty(appVersion))
            {
                string nonTFAVersion = string.Empty;
                nonTFAVersion = applicationID == 1 ? iphonenontfaversion : androidnontfaversion;

                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                if (appVersion != nonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, nonTFAVersion);
                }
                else
                    ValidTFAVersion = true;
            }
            #endregion

            return ValidTFAVersion;
        }

        public static bool IsVersion1Greater(string version1, string version2)
        {
            return SeperatedVersionCompareCommonCode(version1, version2);
        }

        public static bool IsVersion1Greater(string version1, string version2, bool regexAppVersion)
        {
            Regex regex = new Regex("[0-9.]");
            version1 = string.Join("", regex.Matches(version1).Cast<Match>().Select(match => match.Value).ToArray());
            return SeperatedVersionCompareCommonCode(version1, version2);
        }

        public static bool SeperatedVersionCompareCommonCode(string version1, string version2)
        {
            try
            {
                #region
                string[] version1Arr = version1.Trim().Split('.');
                string[] version2Arr = version2.Trim().Split('.');

                if (Convert.ToInt32(version1Arr[0]) > Convert.ToInt32(version2Arr[0]))
                {
                    return true;
                }
                else if (Convert.ToInt32(version1Arr[0]) == Convert.ToInt32(version2Arr[0]))
                {
                    if (Convert.ToInt32(version1Arr[1]) > Convert.ToInt32(version2Arr[1]))
                    {
                        return true;
                    }
                    else if (Convert.ToInt32(version1Arr[1]) == Convert.ToInt32(version2Arr[1]))
                    {
                        if (Convert.ToInt32(version1Arr[2]) > Convert.ToInt32(version2Arr[2]))
                        {
                            return true;
                        }
                        else if (Convert.ToInt32(version1Arr[2]) == Convert.ToInt32(version2Arr[2]))
                        {
                            if (!string.IsNullOrEmpty(version1Arr[3]) && !string.IsNullOrEmpty(version2Arr[3]))
                            {
                                if (Convert.ToInt32(version1Arr[3]) > Convert.ToInt32(version2Arr[3]))
                                {
                                    return true;
                                }
                            }

                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public static bool ValidateAccessCode(string accessCode)
        {
            bool ok = true; // Make this false after implemening the access code check.

            if (!string.IsNullOrEmpty(accessCode))
            {
                switch (accessCode.ToUpper())
                {
                    case "UAWS-MOBILE-ACCESSCODE":
                        ok = true;
                        break;

                    case "ACCESSCODE":
                        ok = true;
                        break;

                    case "59AD27EB-9B93-47C2-B275-D45EC7DC524F":
                        ok = true;
                        break;

                    case "UAWS-1456e190-87c3-4304-a068-d05a93c6695f":
                        ok = true;
                        break;
                }
            }
            return ok;
        }

        public static bool IsApplicationVersionGreater(int applicationID, string appVersion, string androidnontfaversion,
          string iphonenontfaversion, string windowsnontfaversion, string mWebNonELFVersion, bool ValidTFAVersion, IConfiguration _configuration)
        {
            #region Priya Code for version check

            if (!string.IsNullOrEmpty(appVersion))
            {
                string AndroidNonTFAVersion = _configuration.GetValue<string>(androidnontfaversion) ?? "";
                string iPhoneNonTFAVersion = _configuration.GetValue<string>(iphonenontfaversion) ?? "";
                string WindowsNonTFAVersion = _configuration.GetValue<string>(windowsnontfaversion) ?? "";
                string MWebNonTFAVersion = _configuration.GetValue<string>(mWebNonELFVersion) ?? "";

                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                if (applicationID == 1 && appVersion != iPhoneNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, iPhoneNonTFAVersion);
                }
                else if (applicationID == 2 && appVersion != AndroidNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, AndroidNonTFAVersion);
                }
                else if (applicationID == 6 && appVersion != WindowsNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, WindowsNonTFAVersion);
                }
                else if (applicationID == 16 && appVersion != MWebNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, MWebNonTFAVersion);
                }
            }
            #endregion

            return ValidTFAVersion;
        }

        public static bool IsApplicationVersionGreater2(int applicationID, string appVersion, string androidnontfaversion,
            string iphonenontfaversion, string windowsnontfaversion, string mWebNonELFVersion, IConfiguration _configuration)
        {
            #region Nizam Code for version check
            bool ValidTFAVersion = false;
            if (!string.IsNullOrEmpty(appVersion))
            {
                string AndroidNonTFAVersion = _configuration.GetValue<string>(androidnontfaversion) ?? "";
                string iPhoneNonTFAVersion = _configuration.GetValue<string>(iphonenontfaversion) ?? "";
                string WindowsNonTFAVersion = _configuration.GetValue<string>(windowsnontfaversion) ?? "";
                string MWebNonTFAVersion = _configuration.GetValue<string>(mWebNonELFVersion) ?? "";

                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                if (applicationID == 1 && appVersion != iPhoneNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, iPhoneNonTFAVersion);
                }
                else if (applicationID == 2 && appVersion != AndroidNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, AndroidNonTFAVersion);
                }
                else if (applicationID == 6 && appVersion != WindowsNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, WindowsNonTFAVersion);
                }
                else if (applicationID == 16 && appVersion != MWebNonTFAVersion)
                {
                    ValidTFAVersion = IsVersion1Greater(appVersion, MWebNonTFAVersion);
                }
            }
            #endregion

            return ValidTFAVersion;
        }

        public static bool isApplicationVersionGreater(int applicationID, string appVersion, string androidnontfaversion,
   string iphonenontfaversion, string windowsnontfaversion, string mWebNonELFVersion, bool ValidTFAVersion, IConfiguration _configuration)
        {
            #region Priya Code for version check

            if (!string.IsNullOrEmpty(appVersion))
            {
                string AndroidNonTFAVersion = _configuration.GetValue<string>(androidnontfaversion) ?? "";
                string iPhoneNonTFAVersion = _configuration.GetValue<string>(iphonenontfaversion) ?? "";
                string WindowsNonTFAVersion = _configuration.GetValue<string>(windowsnontfaversion) ?? "";
                string MWebNonTFAVersion = _configuration.GetValue<string>(mWebNonELFVersion) ?? "";

                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());
                if (applicationID == 1 && appVersion != iPhoneNonTFAVersion)
                {
                    ValidTFAVersion = GeneralHelper.IsVersion1Greater(appVersion, iPhoneNonTFAVersion);
                }
                else if (applicationID == 2 && appVersion != AndroidNonTFAVersion)
                {
                    ValidTFAVersion = GeneralHelper.IsVersion1Greater(appVersion, AndroidNonTFAVersion);
                }
                else if (applicationID == 6 && appVersion != WindowsNonTFAVersion)
                {
                    ValidTFAVersion = GeneralHelper.IsVersion1Greater(appVersion, WindowsNonTFAVersion);
                }
                else if (applicationID == 16 && appVersion != MWebNonTFAVersion)
                {
                    ValidTFAVersion = GeneralHelper.IsVersion1Greater(appVersion, MWebNonTFAVersion);
                }
            }
            #endregion

            return ValidTFAVersion;
        }

        public static string FormatDateTime(string dateTimeString)
        {
            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(dateTimeString, out dateTime);

            return string.Format("{0:MM/dd/yyyy hh:mm tt}", dateTime);
        }

        public static string FormatDate(string departureDate)
        {
            string result = string.Empty;

            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(departureDate, out dateTime);
            if (dateTime.Ticks != 0)
            {
                if (dateTime.ToString("MMMM").Length > 3)
                {
                    result = string.Format("{0:ddd., MMM. d, yyyy}", dateTime);
                }
                else
                {
                    result = string.Format("{0:ddd., MMM d, yyyy}", dateTime);
                }
            }

            return result;
        }

        public static string FormatDate(string dateString, bool isInvariantCulture)
        {
            return DateTime.ParseExact(dateString, "yyyyMMdd", CultureInfo.InvariantCulture).ToShortDateString();
        }

        public static string FormatTime(string timeString)
        {
            string hour = timeString.Substring(0, 2).Replace(@"/", string.Empty);
            string min = timeString.Substring(2, 2);
            int hourInt = Convert.ToInt32(hour);
            if (hourInt > 12)
            {
                hourInt = hourInt - 12;
                if (hourInt < 10)
                {
                    hour = string.Concat("0", hourInt);
                }
                else
                {
                    hour = hourInt.ToString();
                }
                return string.Concat(hour, ":", min, " pm");
            }
            else if (hourInt == 12)
            {
                return string.Concat(hourInt, ":", min, " pm");
            }
            else
            {
                if (hourInt == 0)
                    return string.Concat("12:", min, " am");
                else
                {
                    if (hourInt < 10)
                    {
                        hour = string.Concat("0", hourInt);
                    }
                    else
                    {
                        hour = hourInt.ToString();
                    }
                    return string.Concat(hour, ":", min, " am");
                }
            }
        }

        public static string FormatDatetime(string dataTime, string languageCode)
        {
            string formattedDateTime = string.Empty;

            if (DateTime.TryParseExact(dataTime, "yyyyMMdd hh:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
            {
                CultureInfo cultureInfo;
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

        public static string FormatDate(string data, string languageCode)
        {
            string formattedDate = string.Empty;

            DateTime d;
            if (DateTime.TryParseExact(data, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
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
                formattedDate = d.ToString("d", cultureInfo);
            }

            return formattedDate;
        }

        public static bool FeatureVersionCheck(int appId, string appVersion, string featureName, string androidVersion, string iosVersion, IConfiguration configuration)
        {
            if (string.IsNullOrEmpty(appVersion) || string.IsNullOrEmpty(featureName))
                return false;
            return configuration.GetValue<bool>(featureName)
                    && GeneralHelper.IsApplicationVersionGreater(appId, appVersion, androidVersion, iosVersion, "", "", true, configuration);
        }

        public static string FormatDatetime4(string dataTime, string languageCode)
        {
            string formattedDateTime = string.Empty;
            //Tue Jun 7 2011 14:53:00
            DateTime dt;
            if (DateTime.TryParseExact(dataTime, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
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

        public static string FormatDateFromDetails(string dateTimeString)
        {
            string result = string.Empty;

            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(dateTimeString, out dateTime);
            if (dateTime.Ticks != 0)
            {
                result = string.Format("{0:MM/dd/yyyy}", dateTime);
                result = result.Contains("-") ? result.Replace("-", "/") : result;
            }


            return result;
        }

        public static string GetDayDifference(String flightDepart, String flightArrive)
        {
            try
            {

                DateTime depart = DateTime.MinValue;
                DateTime arrive = DateTime.MinValue;

                DateTime.TryParse(flightDepart, out depart);
                DateTime.TryParse(flightArrive, out arrive);

                int days = (arrive.Date - depart.Date).Days;

                if (days == 0)
                {
                    return string.Empty;
                }
                else if (days > 0 && days < 2)
                {
                    return "+" + days.ToString() + " day";
                }
                else if (days > 0 && days > 1)
                {
                    return "+" + days.ToString() + " days";
                }
                else if (days < 0 && days > -2)
                {
                    return days.ToString() + " day";
                }
                else if (days < 0 && days < -1)
                {
                    return days.ToString() + " days";
                }
                else
                {
                    return string.Empty;
                }
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static string[] SplitConcatenatedConfigValue(IConfiguration configuration, string configkey, string splitchar)
        {
            try
            {
                string[] splitSymbol = { splitchar };
                string[] splitString = configuration.GetValue<string>(configkey)
                    .Split(splitSymbol, StringSplitOptions.None);
                return splitString;
            }
            catch { return null; }
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }

            return builder.ToString();
        }

        public static string GetPaxDescriptionByDOB(string date, string deptDateFLOF)
        {
            int age = TopHelper.GetAgeByDOB(date, deptDateFLOF);
            if ((18 <= age) && (age <= 64))
            {
                return "Adult (18-64)";
            }
            else
            if ((2 <= age) && (age < 5))
            {
                return "Child (2-4)";
            }
            else
            if ((5 <= age) && (age <= 11))
            {
                return "Child (5-11)";
            }
            else
            //if((12 <= age) && (age <= 17))
            //{

            //}
            if ((12 <= age) && (age <= 14))
            {
                return "Child (12-14)";
            }
            else
            if ((15 <= age) && (age <= 17))
            {
                return "Child (15-17)";
            }
            else
            if (65 <= age)
            {
                return "Senior (65+)";
            }
            else if (age < 2)
                return "Infant (under 2)";

            return string.Empty;
        }
        public static string formateDatetime(string date)
        {
            string formattedDateTime = string.Empty;
            //Wed, Aug 19, 2020
            DateTime dt;
            if (DateTime.TryParse(date, out dt))
            {
                formattedDateTime = dt.ToString("MM/dd/yyyy");
            }
            return formattedDateTime;
        }

        public static string FormatCurrency(string price, string langCode = "en-US")
        {
            var currencySymbol = TopHelper.GetCurrencySymbol(new CultureInfo(langCode));

            if (string.IsNullOrEmpty(price)|| price.StartsWith(currencySymbol))
            {
                return price;
            }
            return String.Format("{0}{1}", currencySymbol, price.Trim(new char[] { '¤', ' ' }));

        }

        public static string FormatCurrency(double price, string langCode = "en-US")
        {
            return price.ToString("C", new CultureInfo(langCode));
        }

        public static CultureInfo EnableUSCultureInfo()
        {
            var oldCultureInfo = CultureInfo.DefaultThreadCurrentCulture;
            var cultureInfo = new CultureInfo("en-US");
            // cultureInfo.NumberFormat.CurrencySymbol = "€";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
            return oldCultureInfo;
        }

        public static void DisableUSCultureInfo(CultureInfo ci)
        {
            if(ci != null)
            {
                CultureInfo.DefaultThreadCurrentCulture = ci;
                CultureInfo.DefaultThreadCurrentUICulture = ci;
            }
        }

        public static string FormatDateOfBirth(DateTime dateTime)
        {
            return $"{dateTime.Month.ToString("00")}/{ dateTime.Day.ToString("00")}/{ dateTime.Year}";
        }
    }
}
