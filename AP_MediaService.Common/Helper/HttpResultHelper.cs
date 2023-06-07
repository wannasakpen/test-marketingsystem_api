using AP_MediaService.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using AP_MediaService.Common.Helper.Interface;
using AP_MediaService.Common.Helper.Logging.Interface;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.Models.ResponseModels;
using AP_MediaService.Common.Helper.Logging;

namespace AP_MediaService.Common.Helper
{
    public class HttpResultHelper : IHttpResultHelper
    {
        private static LogHelper logger;
        private readonly ILogger _logger;
        private readonly ISummaryLog _summaryLog;
        private readonly ILogService _logService;

        public HttpResultHelper(ILogger<HttpResultHelper> logger, ISummaryLog summaryLog, ILogService logService)
        {
            _logger = logger;
            _summaryLog = summaryLog;
            _logService = logService;
        }

        public static void InitLogHelper()
        {
            logger = new LogHelper();

        }

        public static ActionResult CustomResult(HttpStatusCode statuscode, Object data)
        {
            return new ResponseResult(data, statuscode);
        }

        public async Task<ActionResult> CustomResult(HttpStatusCode statuscode, Response data, LogModel logModel)
        {
            logModel.ResponseData = data;
            _ = _logService.WriteLog(logModel);
            return new ResponseResult(data, statuscode);
        }

        #region oldcode
        public static ActionResult CustomResult(HttpRequest Request, HttpStatusCode code, Object data, Exception ex = null)
        {
            string logRequestPath = "\\Rest";

            string BaseDirectory = AppContext.BaseDirectory;
            logRequestPath = (BaseDirectory + logRequestPath);

            TraceLogWriter requestLog = new TraceLogWriter(logRequestPath, "", "yyyyMMdd", "", "log", new CultureInfo("en-US"));
            requestLog.SubfolderType = TraceLogWriter.FolderOption.Date;

            string paramResp = string.Empty;
            string paramRequest = string.Empty;
            string paramURI = string.Empty;

            if (data != null)
            {
                paramResp = JsonConvert.SerializeObject(data, Formatting.Indented);
                JObject jsonObj = (JObject)JsonConvert.DeserializeObject(paramResp);
                data = jsonObj;
            }

            if (Request != null)
            {
                var bodyStream = new StreamReader(Request.Body);
                //bodyStream.BaseStream.Seek(0, SeekOrigin.Begin);
                paramRequest = bodyStream.ReadToEnd();
                if (Extension.IsValidJson(paramRequest) && !string.IsNullOrWhiteSpace(paramRequest))
                {
                    paramRequest = JValue.Parse(paramRequest).ToString(Formatting.Indented);
                }
                //if (!string.IsNullOrWhiteSpace(paramRequest))
                //{
                //    paramRequest = JValue.Parse(paramRequest).ToString(Formatting.Indented);
                //}
            }
            paramURI = Request.GetDisplayUrl();

            string OrderRef = Request.Headers["userid"];
            string OrderRefResp = Request.HttpContext.Response.Headers["x-ais-OrderRef"];

            string writeRequest = paramURI + "\r\n" + "OrderRef Header : " + OrderRef + "\r\n" + paramRequest + "\r\n";
            string writeResponse = paramURI + "\r\n" + "OrderRef Header : " + OrderRefResp + "\r\n" + paramResp + "\r\n";
            lock (requestLog)
            {
                requestLog.WriteRestMessage(writeRequest, "Input");
                requestLog.WriteRestMessage(writeResponse, "Output");
            }

            if (ex != null)
            {
                if (logger == null)
                    InitLogHelper();
                logger.WriteTraceLog(GetExceptionMessage(ex, OrderRefResp));
            }

            // Request here is the property on the controller.
            return new ResponseResult(data, code);
            //CustomResult(httpContextAccessor, code, data);
        }
        #endregion



        public class ResponseResult : JsonResult
        {

            public ResponseResult(object data, HttpStatusCode code)
                           : base(data)
            {
                StatusCode = (int)code;
            }
        }


        public static string GetExceptionMessage(Exception eMessage, string OrderRefResp)
        {
            StringBuilder message = new StringBuilder();

            message.Append("\r\n");
            message.Append("Exception\r\n");
            message.Append("=======================\r\n");

            message.Append("Message: ");
            message.Append(OrderRefResp);
            message.Append("\r\n");

            message.Append("Message: ");
            if (eMessage.Message != null)
                message.Append(eMessage.Message);
            message.Append("\r\n");

            message.Append("DateTime: ");
            message.Append(DateTime.Now.ToString());
            message.Append("\r\n");

            message.Append("Source: ");
            if (eMessage.Source != null)
                message.Append(eMessage.Source);
            message.Append("\r\n");

            message.Append("TargetSite: ");
            if (eMessage.TargetSite != null)
                message.Append(eMessage.TargetSite.ToString());
            message.Append("\r\n");

            message.Append("Type: ");
            if (eMessage.GetType() != null)
                message.Append(eMessage.GetType().ToString());
            message.Append("\r\n");

            message.Append("StackTrace: ");
            if (eMessage.StackTrace != null)
                message.Append(eMessage.StackTrace);
            message.Append("\r\n");

            message.Append("InnerException: ");
            if (eMessage.InnerException != null)
                message.Append((eMessage.InnerException).ToString());
            message.Append("\r\n");

            message.Append("HelpLink: ");
            if (eMessage.HelpLink != null)
                message.Append(eMessage.HelpLink);
            message.Append("\r\n");

            return message.ToString();
        }

    }
}
