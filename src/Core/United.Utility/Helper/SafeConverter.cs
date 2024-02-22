using System;
using System.IO;
using System.Globalization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace United.Utility.Helper
{
    public static class SafeConverter
    {
        public const string DefaultDateFormat = "MM/dd/yyyy hh:mm:ss.ff";
        public static string DateToString(DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString(SafeConverter.DefaultDateFormat, DateTimeFormatInfo.InvariantInfo);
            }
            else
            {
                return "";
            }
        }
        public static string DateToString(DateTime dateTime)
        {
            return dateTime.ToString(SafeConverter.DefaultDateFormat, DateTimeFormatInfo.InvariantInfo);
        }
        public static bool IsDate(string value)
        {
            DateTime result;
            return DateTime.TryParse(value, out result);
        }
        public static bool IsNumeric(string value)
        {
            decimal result;
            return (decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result));
        }
        public static bool ToBoolean(int value)
        {
            return Convert.ToBoolean(value);
        }
        public static bool ToBoolean(string value)
        {
            int resultInteger;
            bool resultBoolean = false;

            // try converting to number first, if that works convert that to boolean, try parse doesn't work as expected on Booleans
            if (Boolean.TryParse(value, out resultBoolean))
            {
                return resultBoolean;
            }
            else if (int.TryParse(value, out resultInteger))
            {
                return Convert.ToBoolean(resultInteger);
            }
            switch (value)
            {
                case "N":
                case "n": resultBoolean = false;
                    break;
                case "Y":
                case "y": resultBoolean = true;
                    break;
                default: Boolean.TryParse(value, out resultBoolean);
                    break;
            }
            return resultBoolean;
        }
        public static DateTime ToDateTime(string value)
        {
            DateTime result;

            DateTime.TryParse(value, System.Globalization.NumberFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.AssumeLocal, out result);
            return result;
        }
        //DateTime.TryParseExact
        public static DateTime ToDateTimeExact(string value, string format)
        {
            DateTime result;
            DateTime.TryParseExact(value, format, NumberFormatInfo.CurrentInfo, DateTimeStyles.AssumeLocal, out result);
            return result;
        }
        public static decimal ToDecimal(string value)
        {
            decimal result;
            decimal.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result);
            return result;
        }
        public static double ToDouble(string value)
        {
            double result;
            Double.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result);
            return result;
        }
        public static int ToInt(string value)
        {
            int result = 0;

            if (int.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result) == false)
            {
                decimal decimalValue = decimal.Truncate(ToDecimal(value));
                if (decimalValue >= int.MinValue && decimalValue <= int.MaxValue)
                {
                    return decimal.ToInt32(decimalValue);
                }
                else
                {
                    return 0;
                }
            }
            return result;
        }
        public static long ToLong(string value)
        {
            long result = 0;
            if (long.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result) == false)
            {
                decimal decimalValue = decimal.Truncate(ToDecimal(value));
                if (decimalValue >= long.MinValue && decimalValue <= long.MaxValue)
                {
                    return decimal.ToInt64(decimalValue);
                }
                else
                {
                    return 0;
                }
            }
            return result;
        }
        public static DateTime? ToNullableDateTime(string value)
        {
            if (String.IsNullOrEmpty(value)) return null;

            DateTime result;
            if (DateTime.TryParse(value, System.Globalization.NumberFormatInfo.CurrentInfo, System.Globalization.DateTimeStyles.AssumeLocal, out result))
            {
                return result;
            }
            else
            {
                return null;
            }
        }
        public static short ToShort(string value)
        {
            short result = 0;
            if (short.TryParse(value, System.Globalization.NumberStyles.Any, System.Globalization.NumberFormatInfo.CurrentInfo, out result) == false)
            {
                decimal decimalValue = decimal.Truncate(ToDecimal(value));
                if (decimalValue >= short.MinValue && decimalValue <= short.MaxValue)
                {
                    return decimal.ToInt16(decimalValue);
                }
                else
                {
                    return 0;
                }
            }
            return result;
        }
    }
}
