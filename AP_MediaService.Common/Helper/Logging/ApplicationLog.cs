using AP_MediaService.Common.Helper.Logging.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging
{
    public class ApplicationLog : IApplicationLog
    {
        public ApplicationLog()
        {
            //Console.WriteLine("CTOR SummaryLog");
        }

        public string HttpMethod { get; set; }
        public string HttpStatusCode { get; set; }
        public string LogLevel { get; set; } = "INFO";
        public string LogMessage { get; set; }
        public string Module { get; set; }
        public string SubModule { get; set; }
        public DateTime? TimeStamp { get; set; } = DateTime.Now;
        public string User { get; set; }


        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void SetLogMessage(object obj)
        {
            this.LogMessage = JsonConvert.SerializeObject(obj);
        }
    }
}
