using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.MappingErrors
{
    public enum ErrorCodes : int
    {
        [Description("Success")]
        Success = 20000,

        [Description("Data not found")]
        DataNotFound = 40400,

        [Description("BadRequestInvalid")]
        BadRequestInvalid = 40000,

        [Description("Internal Server Error")]
        InternalServerError = 50000,


        [Description("Forbidden")]
        Forbidden = 40300,

        [Description("Data Conflict")]
        Conflict = 40900,

        [Description("Block send OTP")]
        ForbiddenBlockOTP = 40301,

        [Description("Unauthorized")]
        Unauthorized = 40100,
    }

    public static class StatusCodeHelper
    {
        public static HttpStatusCode GetHttpStatusCode(this ErrorCodes enumCode)
        {
            HttpStatusCode Code = new HttpStatusCode();
            foreach (HttpStatusCode o in Enum.GetValues(typeof(HttpStatusCode)))
            {
                if (((int)o).Equals((enumCode.GetIntIndex3Digit())))
                    Code = o;
            }
            return Code;
        }

        public static string GetDescription(this ErrorCodes enumValue)
        {


            var description = enumValue.ToString();
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                var attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute)attrs[0]).Description;
                }
            }

            return description;
        }

        private static int GetIntIndex3Digit(this ErrorCodes eValue)
        {
            return Convert.ToInt32(((int)eValue).ToString().Substring(0, 3));
        }
    }
}
