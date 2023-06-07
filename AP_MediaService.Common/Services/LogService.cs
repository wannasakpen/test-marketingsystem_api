using AP_MediaService.Common.Helper.Logging;
using AP_MediaService.Common.Helper;
using AP_MediaService.Common.Interfaces;
using AP_MediaService.Common.MappingErrors;
using AP_MediaService.Common.Models.ResponseModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AP_MediaService.Common.Utilities;
using Microsoft.AspNetCore.Http;

namespace AP_MediaService.Common.Services
{
    public class LogService : ILogService
    {
        private readonly IHeadersUtils _headersUtil;
        private readonly ILogger _logger;
        //private readonly ISummaryLog _summaryLog;
        //private readonly ITraceLog _traceLog;
        private static LogHelper loggertxt;
        SummaryLog _summaryLog;
        ApplicationLog _applicationLog;
        private bool cmdlog = false;

        public LogService(ILogger<LogService> logger, IHeadersUtils headersUtil)
        {
            _headersUtil = headersUtil;
            _logger = logger;

            // WriteLog(logModel);
        }


        public async Task WriteLog(LogModel logModel)
        {
            _summaryLog = new SummaryLog();
            _applicationLog = new ApplicationLog();
            #region Set Request  
            Dictionary<string, string> AuthHeader = null;
            if (_headersUtil.UserID == null)
            {
                AuthHeader = new Dictionary<string, string>() {
                { "UserID", _headersUtil.GetUserID().ToString()}
                };
            }

            var dics = new Dictionary<string, object>() {
                    { "Url", _headersUtil.GeturlPath()},
                    { "Method", _headersUtil.GetMethod() },
                    { "Header" , AuthHeader },
                    { "Body", JsonConvert.DeserializeObject<object>(await _headersUtil.GetBodyRequest()) },
                    { "QueryString", _headersUtil.GetQueryString() }
                };
            //filter dics not null
            var filtered = dics.Where(p => p.Value != null)
                .ToDictionary(p => p.Key, p => p.Value);

            logModel.RequestObject = filtered;
            #endregion

            PropertyInfo pi = logModel.ResponseData.GetType().GetProperty("Data");
            string strData = JsonConvert.SerializeObject(pi.GetValue(logModel.ResponseData));

            //convert data<T> to string
            Response respdata = new Response<string>()
            {
                RespCode = logModel.ResponseData.RespCode,
                Data = strData
            };
            logModel.ResponseData = respdata;

            logModel.ActivityLog = logModel.GetActivityLog();
            logModel.Command = (_headersUtil.GetHttpRequest().HttpContext.GetEndpoint() as RouteEndpoint).RoutePattern.Defaults.Values.FirstOrDefault().ToString();

            #region summaryLog
            _summaryLog.cmdName = logModel.Command;
            _summaryLog.tid = _headersUtil.GetUserID().ToString();
            _summaryLog.resultCode = ((int)logModel.ResponseData.RespCode).ToString();
            _summaryLog.resultDesc = logModel.ResponseData.RespCode.GetDescription();
            _summaryLog.SetresTimestamp(logModel.ActivityLog.EndTime);
            _summaryLog.usageTime = logModel.ActivityLog.ProcessTime.ToString();
            #endregion

            #region ApplicationLog
            _applicationLog.SetLogMessage(logModel);
            _applicationLog.HttpMethod = _headersUtil.GetMethod();
            _applicationLog.Module = logModel.Command;
            _applicationLog.HttpStatusCode = logModel.ResponseData.RespCode.GetHttpStatusCode().ToString();
            _applicationLog.User = _headersUtil.GetUserID().ToString();
            #endregion

            if (logModel.EndPoint != null)
            {
                foreach (var item in logModel.EndPoint)
                {
                    WriteLogEDR(item);
                }
            }
            logModel.EndPoint = null;
            if (logModel.ResponseData.RespCode == ErrorCodes.InternalServerError)
            {
                logModel.ExceptionMessage = new LogExceptionModel(new Exception());
            }
            WriteLogEDR(logModel);






            #region LogType Writelog
            _logger.LogInformation(_summaryLog.Serialize());
            _logger.LogInformation(_applicationLog.Serialize());

            //command log 
            if (cmdlog)
            {
                _logger.LogInformation(_summaryLog.Serialize());
            }
            else
            {
                loggertxt = new LogHelper();
                loggertxt.WriteTraceLog(_summaryLog.Serialize());
            }
            #endregion

            string controllerPath = ControllerHeader.ControllerName + "/" + logModel.Command;
            if (controllerPath == "/Authorize")
                MetricsHelper.TransactionsCounter.WithLabels(controllerPath, "", ((int)logModel.ResponseData.RespCode).ToString()).Inc();
            else
                MetricsHelper.TransactionsCounter.WithLabels(controllerPath, _headersUtil.GetMethod(), ((int)logModel.ResponseData.RespCode).ToString()).Inc();

        }


        public static string GetExceptionMessage(LogExceptionModel eMessage, string OrderRefResp)
        {
            StringBuilder message = new StringBuilder();

            message.Append("\r\n");
            message.Append("Exception\r\n");
            message.Append("=======================\r\n");

            message.Append("OrderRef: ");
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

            //message.Append("TargetSite: ");
            //if (eMessage.TargetSite != null)
            //    message.Append(eMessage.TargetSite.ToString());
            //message.Append("\r\n");

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

            //message.Append("HelpLink: ");
            //if (eMessage.HelpLink != null)
            //    message.Append(eMessage.HelpLink);
            //message.Append("\r\n");

            return message.ToString();
        }

        public void WriteLogEDR(object EndPoint)
        {

            TraceLog Model = new TraceLog();

            PropertyInfo pi = EndPoint.GetType().GetProperty("ExceptionMessage");
            if (pi != null)
            {
                object ex = (object)(pi.GetValue(EndPoint, null));

                if (ex == null)
                    Model.logLevel = "INFO";
                else
                    Model.logLevel = "ERROR";
            }

            Model.tid = _summaryLog.tid;
            Model.sessionId = _summaryLog.sessionId;

            object Vesta = new { Vesta = EndPoint };
            Model.Setcustom1(Vesta);
            //Model.Setcustom1(EndPoint);
            //_logger.LogInformation(Model.Serialize());

            #region LogType Writelog
            _logger.LogInformation(Model.Serialize());

            //KafkaProduceService logkafka = new KafkaProduceService();
            //logkafka.ProduceMessage("application-log", Model.Serialize());
            if (cmdlog)
            {
                _logger.LogInformation(Model.Serialize());
            }
            else
            {
                loggertxt = new LogHelper();
                loggertxt.WriteTraceLog(Model.Serialize());
            }
            #endregion
        }
    }
}
