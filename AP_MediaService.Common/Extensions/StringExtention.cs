using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Extensions
{
    public static class StringExtention
    {

        readonly static string _customFormat = "dd/MM/yyyy";
        readonly static string _customCulture = "en-US";

        public static string DateFormat(this object value)
        {
            return _customFormat;
        }

        public static string JsDateFormat(this object value)
        {
            return _customFormat.ToLower();
        }

        public static string ToDateString(this DateTime value)
        {
            if (value == DateTime.MinValue)
            {
                return "";
            }

            return value.ToString(_customFormat, new CultureInfo(_customCulture));

        }

        public static string ToTimeString(this DateTime value)
        {

            return value.ToString("HH:mm:ss", new CultureInfo(_customCulture));

        }

        public static string ToTimeString2(this DateTime value)
        {

            return value.ToString("HH:mm", new CultureInfo(_customCulture));

        }

        public static string ToYearString(this DateTime value)
        {

            return value.ToString("yy", new CultureInfo(_customCulture));
        }

        public static string ToDateTimeString(this DateTime value)
        {

            return value.ToString(_customFormat + " HH:mm:ss", new CultureInfo(_customCulture));

        }

        public static string ToDateTimeString(this DateTime? value)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(_customFormat + " HH:mm:ss", new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToDateTimeString2(this DateTime value)
        {

            return value.ToString(_customFormat + " HH:mm", new CultureInfo(_customCulture));

        }

        public static string ToDateTimeString2(this DateTime? value)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(_customFormat + " HH:mm", new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToCustomDateTime(this DateTime value, string CustomFormat)
        {
            return value.ToString(CustomFormat, new CultureInfo(_customCulture));
        }

        public static string ToCustomDateTime(this DateTime? value, string CustomFormat)
        {
            if (value.HasValue)
                return (value ?? DateTime.Now).ToString(CustomFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToDateString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString(_customFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;
        }

        public static string ToDateString(this DateTime? value, string _customFormat)
        {
            if (value.HasValue)
                return value.Value.ToString(_customFormat, new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static string ToTimeString(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("HH:mm:ss", new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static string ToTimeString2(this DateTime? value)
        {
            if (value.HasValue)
                return value.Value.ToString("HH:mm", new CultureInfo(_customCulture));
            else
                return string.Empty;

        }

        public static DateTime ToDate(this string value, bool throwIfInvalidCast = false)
        {


            DateTime returnValue = DateTime.MinValue;
            bool success = false;
            if (!string.IsNullOrEmpty(value))
            {

                if (value.Contains(":"))
                {
                    success = DateTime.TryParseExact(value, _customFormat + " HH:mm:ss", new CultureInfo(_customCulture), DateTimeStyles.AllowWhiteSpaces, out returnValue);//CultureInfo.InvariantCulture
                }
                else
                {

                    success = DateTime.TryParseExact(value, _customFormat, new CultureInfo(_customCulture), DateTimeStyles.AllowWhiteSpaces, out returnValue);//CultureInfo.InvariantCulture
                }
            }


            if (!success && throwIfInvalidCast)
                throw new FormatException("invalid date format");

            return returnValue;

        }

        public static DateTime? ToDateNullable(this string value)
        {

            if (string.IsNullOrEmpty(value))
                return null;
            else
                return value.ToDate();
        }

        public static string ToInt0DigitComma(this int? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDecimal0DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDecimal0DigitComma(this decimal value)
        {

            return value.ToString("#,##0");

        }

        public static string ToDecimal1DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.0");

        }

        public static string ToDecimal2DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.00");

        }

        public static string ToDecimal2DigitComma0(this decimal? value)
        {
            if (!value.HasValue)
                return "0.00";
            else
                return value.Value.ToString("#,##0.00");
        }

        public static string ToDecimal2DigitComma(this decimal value)
        {

            return value.ToString("#,##0.00");

        }

        public static string ToDecimal3DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.000");

        }

        public static string ToDecimal4DigitComma(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.0000");

        }

        public static string ToInt0Digit(this int? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0");

        }

        public static string ToDecimal0Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0");

        }

        public static string ToDecimal1Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.0");

        }

        public static string ToDecimal2Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.00");

        }

        public static string ToDecimal3Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.000");

        }

        public static string ToDecimal4Digit(this decimal? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("###0.0000");

        }

        public static decimal? GetStringToDecimal(string v)
        {
            decimal result = 0;
            decimal.TryParse(v.Replace(",", ""), out result);

            return result;
        }

        public static string ToDouble2DigitComma(this double? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0.00");

        }

        public static string ToDouble2DigitComma(this double value)
        {

            return value.ToString("#,##0.00");

        }

        public static string ToDouble0DigitComma(this double? value)
        {

            if (!value.HasValue)
                return null;
            else
                return value.Value.ToString("#,##0");

        }

        public static string ToDouble0DigitComma(this double value)
        {

            return value.ToString("#,##0");

        }

        public static string RenameFileWithTimeStamp(this string str)
        {
            string timestamp = Convert.ToString(DateTime.Today.Year) + DateTime.Now.ToString("MMdd_HHmmss");

            string objectName = "";

            var fileObj = str.Split(".");

            if (fileObj.Length > 1)
                objectName = $"{fileObj[0]}_{timestamp}.{fileObj[1]}";
            else
                objectName = $"{str}_{timestamp}";

            return objectName;
        }

        public static void ConvertToMinIOFileParam(ref string fileBucket, ref string fileName)
        {
            var minioBucket = fileBucket.Split("/")[0];

            var minioFileName = "";

            for (int i = 1; i < fileBucket.Split("/").Length; i++)
            {
                minioFileName += fileBucket.Split("/")[i] + "/";
            }

            minioFileName += fileName;

            fileBucket = minioBucket;
            fileName = minioFileName;
        }

        public static string DateToStringFormat(this DateTime? value, string format)
        {
            if (!value.HasValue)
                return "";
            else
            {
                var datetime = value ?? DateTime.Now;
                var strYear = datetime.Year.ToString();

                format = format.Replace("yyyy", strYear);
                format = format.Replace("yy", strYear.Substring(strYear.Length - 2, 2));
                var strDate = datetime.ToString(format, new CultureInfo(_customCulture));

                return strDate;
            }
        }








    }
}
