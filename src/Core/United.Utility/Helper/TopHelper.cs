using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using United.Mobile.Model;
using United.Mobile.Model.Internal.Exception;
using United.Utility.Enum;
using United.Utility.Extensions;

namespace United.Utility.Helper
{

    public class TopHelper
    {

        private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3);
        private static readonly Encoding _encoding = new UTF8Encoding(false, true);


        public static void SetHttpHeader(ref IHttpContextAccessor httpContextAccessor, string deviceId, string applicationId, string appVersion, string transactionId, string requestTime, string languageCode)
        {
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderAppIdText, applicationId);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderAppMajorText, appVersion);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderAppMinorText, appVersion);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderDeviceIdText, deviceId);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderLangCodeText, languageCode);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderRequestTimeUtcText, requestTime);
            ResetHttpHeader(ref httpContextAccessor, Constants.HeaderTransactionIdText, transactionId);
        }

        private static void ResetHttpHeader(ref IHttpContextAccessor httpContextAccessor, string headerKey, string headerValue)
        {
            if (httpContextAccessor.HttpContext.Request.Headers.ContainsKey(headerKey))
            {
                httpContextAccessor.HttpContext.Request.Headers.Remove(headerKey);
            }
            httpContextAccessor.HttpContext.Request.Headers.Add(headerKey, headerValue);
        }

        private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
        {
            // # of 0's = length of pin
            const int Mod = 1000000;

            // See https://tools.ietf.org/html/rfc4226
            // We can add an optional modifier
            var timestepAsBytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
            var hash = hashAlgorithm.ComputeHash(ApplyModifier(timestepAsBytes, modifier));

            // Generate DT string
            var offset = hash[hash.Length - 1] & 0xf;
            Debug.Assert(offset + 4 < hash.Length);
            var binaryCode = ((hash[offset] & 0x7f) << 24)
                             | ((hash[offset + 1] & 0xff) << 16)
                             | ((hash[offset + 2] & 0xff) << 8)
                             | (hash[offset + 3] & 0xff);

            return binaryCode % Mod;
        }

        private static byte[] ApplyModifier(byte[] input, string modifier)
        {
            if (string.IsNullOrEmpty(modifier)) return input;

            var modifierBytes = _encoding.GetBytes(modifier);
            var combined = new byte[checked(input.Length + modifierBytes.Length)];
            Buffer.BlockCopy(input, 0, combined, 0, input.Length);
            Buffer.BlockCopy(modifierBytes, 0, combined, input.Length, modifierBytes.Length);
            return combined;
        }

        // More info: https://tools.ietf.org/html/rfc6238#section-4
        private static ulong GetCurrentTimeStepNumber()
        {
            var delta = DateTime.UtcNow - _unixEpoch;
            return (ulong)(delta.Ticks / _timestep.Ticks);
        }

        public static int GenerateCode(byte[] securityToken, string modifier = null)
        {
            if (securityToken == null) throw new ArgumentNullException(nameof(securityToken));

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                return ComputeTotp(hashAlgorithm, currentTimeStep, modifier);
            }
        }

        public static bool ValidateCode(byte[] securityToken, int code, string modifier = null)
        {
            if (securityToken == null) throw new ArgumentNullException(nameof(securityToken));

            // Allow a variance of no greater than 90 seconds in either direction
            var currentTimeStep = GetCurrentTimeStepNumber();
            using (var hashAlgorithm = new HMACSHA1(securityToken))
            {
                for (var i = -2; i <= 2; i++)
                {
                    var computedTotp = ComputeTotp(hashAlgorithm, (ulong)((long)currentTimeStep + i), modifier);
                    if (computedTotp == code) return true;
                }
            }

            // No match
            return false;
        }

        public static int GenerateCode(string securityToken, string modifier = null)
        {
            return GenerateCode(Encoding.Unicode.GetBytes(securityToken), modifier);
        }

        public static bool ValidateCode(string securityToken, int code, string modifier = null)
        {
            return ValidateCode(Encoding.Unicode.GetBytes(securityToken), code, modifier);
        }

        public static HttpContextValues HttpHeaderValues(IHttpContextAccessor httpContextAccessor)
        {
            HttpContextValues httpContextValues = new HttpContextValues();
            HeaderValues headerValues = new HeaderValues(httpContextAccessor);
            headerValues.GetHeaderValues(httpContextValues);
            return httpContextValues;
        }

        public static string Encrypt(string employeeId, string key = "8080808080808080")
        {
            byte[] _secretKey = KeyFormat(key);

            byte[] cipherText;
            using (Aes aes = Aes.Create())
            {
                aes.Key = _secretKey;
                aes.IV = _secretKey;
                //aes.KeySize = 128;
                //aes.Padding = PaddingMode.PKCS7;
                //aes.Mode = CipherMode.CBC;               

                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(employeeId);
                        }
                    }
                    cipherText = ms.ToArray();

                }
            }
            return Convert.ToBase64String(cipherText).TrimStart().TrimEnd();
        }

        public static string Decrypt(string encryptedEmployeeId, string key)
        {
            byte[] _secretKey = KeyFormat(key);
            byte[] cipherText = Convert.FromBase64String(encryptedEmployeeId);
            string plainText;

            using (Aes aes = Aes.Create())
            {
                aes.Key = _secretKey;
                aes.IV = _secretKey;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                using (MemoryStream ms = new MemoryStream(cipherText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            plainText = sr.ReadToEnd();
                        }
                    }
                }
            }
            return plainText;
        }

        private static byte[] KeyFormat(string key)
        {
            byte[] _secretKey;

            if (key.Length > 16)
                key = key.Substring(0, 16);
            if (key.Length > 16)
                key = key.Substring(0, 16);
            if (key.Length < 16)
                key = key.PadRight(16);
            _secretKey = Encoding.UTF8.GetBytes(key);

            return _secretKey;

        }
        public static Response<T> HandleForceUpdateResponse<T>(Response<T> response, HttpContext httpContext, IConfiguration configuration)
        {
            var errorDescription = configuration.GetValue<string>("forceUpdateMessageGetFlightSearchResult");
            response.Title = "ForceUpdate";
            response.Status = (int)HttpStatusCode.BadRequest;
            response.Errors.Add("400", new List<string> { errorDescription });
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            return response;
        }

        public static T ExceptionResponse<T>(T response, HttpContext httpContext, IConfiguration configuration)
        {
            var errorDescription = configuration.GetSection("validationConfig")
                                                 .GetValue<string>("ServiceError");
            //TODO
            // response..Title = errorDescription;
            //response.Status = (int)HttpStatusCode.InternalServerError;
            //response.Errors.Add("500", new List<string> { errorDescription });
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return response;
        }

        public static Response<T> ExceptionResponse<T>(Response<T> response, HttpContext httpContext, IConfiguration configuration)
        {
            var errorDescription = configuration.GetSection("validationConfig")
                                                 .GetValue<string>("ServiceError");
            response.Title = errorDescription;
            response.Status = (int)HttpStatusCode.InternalServerError;
            response.Errors.Add("500", new List<string> { errorDescription });
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            return response;
        }
        public static MOBException ErrorHandling(string errorCode, string errorDescription)
        {
            return new MOBException() { Message = errorDescription, Code = errorCode };
        }

        public static Response<T> ErrorHandling<T>(Response<T> response, int? errorCode, string errorDescription)
        {
            errorCode = (errorCode.Equals(null) | errorCode.Equals(400)) ? 400 : errorCode;
            response.Status = (errorCode == 400) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;
            response.Errors.Add(errorCode.ToString(), new List<string> { errorDescription });
            return response;
        }
        public static T GetObjectFromXmlData<T>(string xmlSessionData)
        {
            if (string.IsNullOrEmpty(xmlSessionData))
                return default(T);

            var xmlSerialize = new System.Xml.Serialization.XmlSerializer(typeof(T));
            var xmlResult = (T)xmlSerialize.Deserialize(new StringReader(xmlSessionData));
            if (xmlResult != null)
                return xmlResult;
            else
                return default(T);
        }

        public static string FormatTimeToAmOrPm(string timeString)
        {
            if (string.IsNullOrEmpty(timeString))
                return string.Empty;
            return timeString.Contains("m") ? timeString : $"{timeString}m";
        }

        public static string FormatDuration(string hours, string minutes, string hourSuffix = "h", string minuteSuffix = "m")
        {
            var result = string.Empty;

            if (!string.IsNullOrEmpty(hours) && hours.Length > 0 && hours != "0")
            {
                result = $"{hours}{hourSuffix} ";
            }
            result = $"{result}{minutes}{minuteSuffix}";
            return result;
        }

        public static T GetHeaderMessage<T>(IConfiguration configuration, string sectionName)
        {
            return configuration.GetSection(sectionName).Get<T>();
        }

        public static string FormatFlightDate(string dateTimeString)
        {
            string result = string.Empty;

            DateTime dateTime = new DateTime(0);
            DateTime.TryParse(dateTimeString, out dateTime);
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
        public static Response<T> GetInvalidSessionResponse<T>(IConfiguration configuration)
        {
            var response = new Response<T>();
            response.Data = default;
            var statusCode = (int)HttpStatusCode.RequestTimeout;
            response.Errors.Add($"{statusCode}", new List<string> { configuration.GetSection("invalidSession").GetValue<string>("Message") });
            response.Status = statusCode;
            response.Title = configuration.GetSection("invalidSession").GetValue<string>("Title");
            return response;
        }

        public static string ConvertToGMTDateTime(string localTime, int gmtOffSetMinutes)
        {
            var dateTime = new DateTime(0);
            DateTime.TryParse(localTime, out dateTime);
            dateTime = dateTime.AddMinutes(-gmtOffSetMinutes);
            return dateTime.ToString("MM/dd/yyyy hh:mm tt");
        }
        //To Format Phone number to International Format
        public static string FormatPhoneNumber(string phoneNumber, string dialcode)
        {
            string formatNumber = string.Empty;
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                if (phoneNumber.Length > 10)
                {
                    if (!string.IsNullOrEmpty(dialcode))
                    {
                        if (phoneNumber.StartsWith(dialcode))
                        { phoneNumber = phoneNumber.Substring(dialcode.Length); }
                        dialcode = $"+{dialcode} ";
                    }
                    else
                    { phoneNumber = phoneNumber.TrimStart(phoneNumber.Substring(0, 1).ToCharArray()); }
                }
                if (!phoneNumber.ToLower().Contains("null"))
                {
                    phoneNumber = string.Format("({0}) {1}-{2}", phoneNumber.Substring(0, 3), phoneNumber.Substring(3, 3), phoneNumber.Substring(6));
                    formatNumber = (dialcode + phoneNumber);
                }
            }
            return formatNumber;
        }

        public static bool IsApplicationVersionGreaterorEqual(string appVersion, string configAppVersion)
        {
            if (!string.IsNullOrEmpty(appVersion) && !string.IsNullOrEmpty(configAppVersion))
            {
                Regex regex = new Regex("[0-9.]");
                appVersion = string.Join("",
                    regex.Matches(appVersion).Cast<Match>().Select(match => match.Value).ToArray());


                return IsVersionGreaterOrEqual(appVersion, configAppVersion);
            }

            return false;
        }

        public static bool IsVersionGreaterOrEqual(string version1, string version2)
        {
            System.Version localVersion1;
            System.Version localVersion2;
            if (System.Version.TryParse(version1, out localVersion1) &&
                System.Version.TryParse(version2, out localVersion2))
            {
                return localVersion1 >= localVersion2;
            }
            return false;
        }

        public static string FormatDatetime(string dataTime, string languageCode)
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

        public static string GetCSSPublicKeyPersistSessionStaticGUID(int applicationId, string configEntryKey)
        {
            if (!string.IsNullOrEmpty(configEntryKey))
            {
                foreach (var applicationSessionGUID in configEntryKey.Split('|'))
                {
                    if (Convert.ToInt32(applicationSessionGUID.Split('~')[0].ToString().ToUpper().Trim()) == applicationId)
                    {
                        return applicationSessionGUID.Split('~')[1].ToString().Trim();
                    }
                }
            }

            return "1CSSPublicKeyPersistStatSesion4IphoneApp";
        }
        public static string FormatAmountForDisplay(decimal amt, CultureInfo ci, bool roundup = true, bool isAward = false)
        {
            return FormatAmountForDisplay(amt.ToString(), ci, roundup, isAward);
        }

        public static string FormatAmountForDisplay(string amt, CultureInfo ci, /*string currency,*/ bool roundup = true, bool isAward = false)
        {
            string newAmt = amt;
            decimal amount = 0;
            decimal.TryParse(amt, out amount);

            try
            {
                RegionInfo ri = new RegionInfo(ci?.Name);

                switch (ri.ISOCurrencySymbol.ToUpper())
                {
                    case "JPY":
                    case "EUR":
                    case "CAD":
                    case "GBP":
                    case "CNY":
                    case "USD":
                    case "AUD":
                    default:
                        newAmt = GetCurrencySymbol(ci, amount, roundup);
                        break;
                }

            }
            catch { }

            return isAward ? "+ " + newAmt : newAmt;
        }
        public static string GetCurrencySymbol(CultureInfo ci, /*string currencyCode,*/ decimal amount, bool roundup)
        {
            string result = string.Empty;

            try
            {
                if (amount > -1)
                {
                    if (roundup)
                    {
                        int newTempAmt = (int)decimal.Ceiling(amount);
                        try
                        {
                            var ri = new RegionInfo(ci.Name);
                            CultureInfo tempCi = Thread.CurrentThread.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = ci;
                            result = newTempAmt.ToString("c0");
                            Thread.CurrentThread.CurrentCulture = tempCi;

                        }
                        catch { }
                    }
                    else
                    {
                        try
                        {
                            var ri = new RegionInfo(ci.Name);
                            CultureInfo tempCi = Thread.CurrentThread.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = ci;
                            result = amount.ToString("c");
                            Thread.CurrentThread.CurrentCulture = tempCi;

                        }
                        catch { }
                    }
                }
                else
                {
                    if (roundup)
                    {
                        int newTempAmt = (int)decimal.Ceiling(amount);
                        //var cultures = CultureInfo.GetCultures(CultureTypes.AllCultures);

                        //foreach (var ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
                        //{
                        try
                        {
                            var ri = new RegionInfo(ci.Name);
                            //if (ri.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper())
                            //{
                            //result = ri.CurrencySymbol;
                            CultureInfo tempCi = Thread.CurrentThread.CurrentCulture;
                            Thread.CurrentThread.CurrentCulture = ci;
                            //result = newTempAmt.ToString("c0", new CultureInfo("en-US"));
                            result = newTempAmt>0? newTempAmt.ToString("c0", new CultureInfo("en-US")): string.Format("({0})", (-newTempAmt).ToString("c0", new CultureInfo("en-US")));
                            Thread.CurrentThread.CurrentCulture = tempCi;
                            //break;
                            //}
                        }
                        catch { }
                        //}
                        //newAmt = newTempAmt.ToString();
                    }
                }

            }
            catch { }

            return result;
        }
        public static CultureInfo GetCultureInfo(string currencyCode)
        {
            CultureInfo culture = new CultureInfo("en-US");

            if (!string.IsNullOrEmpty(currencyCode))
            {
                var cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                foreach (var ci in cultures)
                {
                    try
                    {
                        var ri = new RegionInfo(ci.Name);
                        if (ri.ISOCurrencySymbol.ToUpper() == currencyCode.ToUpper())
                        {
                            culture = ci;
                            break;
                        }
                    }
                    catch { culture = new CultureInfo("en-US"); }
                }
            }

            return culture;
        }
        public static string GetCurrencySymbol(CultureInfo ci)
        {
            string result = string.Empty;

            try
            {
                int amt = 0;
                CultureInfo tempCi = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = ci;
                result = amt.ToString("c0").Substring(0, 1);
                if (string.IsNullOrEmpty(result))
                    result = "$";
                Thread.CurrentThread.CurrentCulture = tempCi;
            }

            catch { }

            return result;
        }
        public static int GetAgeByDOB(string birthDate, string firstLOFDepDate)
        {
            var travelDate = DateTime.Parse(firstLOFDepDate);

            var birthDate1 = DateTime.Parse(birthDate);
            // Calculate the age.
            var age = travelDate.Year - birthDate1.Year;
            // Go back to the year the person was born in case of a leap year
            if (birthDate1 > travelDate.AddYears(-age)) age--;

            return age;
        }

        public static bool Is24HourWindow(DateTime dtDeparturedatetimeGMT)
        {
            if (dtDeparturedatetimeGMT != null)
            {
                DateTime dtCurrentDateTime = System.DateTime.Now;
                DateTime dtGMTDatetime = dtCurrentDateTime.ToUniversalTime();
                double days = (dtDeparturedatetimeGMT - dtGMTDatetime).TotalDays;
                // double hours = (dtDeparturedatetimeGMT - dtGMTDatetime).TotalHours;
                if (days >= 1)
                {
                    return true;
                }
            }

            return false;
        }

        public static string ExceptionMessages(Exception ex)
        {
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return ex.Message + " | " + ExceptionMessages(ex.InnerException);
        }

        public static string GetSQLEnvironment(string environment)
        {
            switch (environment.ToUpper())
            {
                case "QA": return SQLEnvironment.QA.ToString();
                case "PREPROD": return SQLEnvironment.PREPROD.ToString();
                case "PROD": return SQLEnvironment.PROD.ToString();
                case "DEVELOPMENT":
                default: return SQLEnvironment.DEV.ToString();
            }
        }
    }
}
