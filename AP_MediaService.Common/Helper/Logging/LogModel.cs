using AP_MediaService.Common.Helper.Logging.Interface;
using AP_MediaService.Common.Models.ResponseModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging
{
    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LogModel : ILogModel
    {

        public DateTime StartTime;
        public LogModel()
        {
            StartTime = DateTime.Now;
            EndPoint = new List<EndPointsModel>();
        }
        public LogModel(string command, object requestObject)
        {
            StartTime = DateTime.Now;
            EndPoint = new List<EndPointsModel>();
            Command = command;
            RequestObject = requestObject;
        }
        //public LogModel(string command, string orderRef, string orderRefResp, string paramURI, object requestObject)
        //{ 
        //    StartTime = DateTime.Now;
        //    EndPoint = new List<EndPointsModel>();
        //    Command = command;
        //    OrderRef = orderRef;
        //    OrderRefResp = orderRefResp;
        //    RequestObject = requestObject;
        //    ParamURI = paramURI;
        //}


        public LogModel(string command, RequestObjectModel requestObject)
        {
            StartTime = DateTime.Now;
            EndPoint = new List<EndPointsModel>();
            Command = command;
            RequestObject = requestObject;
        }

        public string Command { get; set; }
        public string Status { get; set; }
        public string OrderRef { get; set; }
        public string OrderRefResp { get; set; }
        public string ParamURI { get; set; }
        public object RequestObject { get; set; }
        public object ResponseObject { get; set; }
        public Response ResponseData { get; set; }
        public ActivityLogModel ActivityLog { get; set; }
        public List<EndPointsModel> EndPoint { get; set; }
        public LogExceptionModel ExceptionMessage { get; set; }

        public ActivityLogModel GetActivityLog()
        {
            DateTime time = DateTime.Now;
            return new ActivityLogModel()
            {
                StartTime = this.StartTime,
                EndTime = time,
                ProcessTime = Math.Round((time - this.StartTime).TotalMilliseconds, 4)
            };
        }

        public bool SetRequestData(string command, string orderRef, object requestObject)
        {
            Command = command;
            OrderRef = orderRef;
            RequestObject = requestObject;
            return true;
        }

        public bool SetRequestData(string command, string orderRef, RequestObjectModel requestObject)
        {
            Command = command;
            OrderRef = orderRef;
            RequestObject = requestObject;
            return true;
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class EndPointsModel
    {
        private DateTime StartTime;
        public EndPointsModel()
        {
            StartTime = DateTime.Now;
        }
        public EndPointsModel(string endpointname)
        {
            StartTime = DateTime.Now;
            //NodeName NodeName = nodename;
            this.EndPointName = endpointname;
        }

        public string EndPointName { get; set; }
        public object RequestObject { get; set; }
        public object ResponseObject { get; set; }
        public LogExceptionModel ExceptionMessage { get; set; }
        public ActivityLogModel ActivityLog { get; set; }
        public ActivityLogModel GetActivityLog()
        {
            DateTime time = DateTime.Now;
            return new ActivityLogModel()
            {
                StartTime = this.StartTime,
                EndTime = time,
                ProcessTime = Math.Round((time - this.StartTime).TotalMilliseconds, 4)
            };
        }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class ActivityLogModel
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double ProcessTime { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class SetRequestModel
    {
        public string url { get; set; }
        public string method { get; set; }
        public object headers { get; set; }
        public Dictionary<string, string> routeParamteters { get; set; }
        public object queryString { get; set; }
        public object body { get; set; }
    }

    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class RequestObjectModel
    {
        public RequestObjectModel(object _headers, object _body, string? _url = null)
        {
            headers = _headers;
            url = _url;
            body = _body;
        }

        public string? url { get; set; }

        public object headers { get; set; }

        public Dictionary<string, string> routeParamteters { get; set; }

        public object queryString { get; set; }

        public object body { get; set; }
    }



    [JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
    public class LogExceptionModel
    {
        public LogExceptionModel()
        {

        }
        public LogExceptionModel(Exception exception)
        {
            exception = GetInnerException(exception);
            Message = exception.Message;
            Source = exception.Source;
            StackTrace = exception.StackTrace;
            if (exception.InnerException != null) InnerException = exception.InnerException.ToString();
        }

        public string Message { get; set; }
        public string Source { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }

        public Exception GetInnerException(Exception ex)
        {
            return (ex.InnerException == null) ? ex : GetInnerException(ex.InnerException);
        }
    }
}
