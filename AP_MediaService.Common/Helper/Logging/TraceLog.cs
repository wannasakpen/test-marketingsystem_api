using AP_MediaService.Common.Helper.Logging.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging
{
    public class TraceLog : ITraceLog
    {
        private const string DateTimeFormat = "dd/MM/yyyy HH:mm:ss.fff";
        public string systemTimestamp { get; set; } = DateTime.Now.ToString(DateTimeFormat);
        public string logType { get; private set; } = "Detail";
        public string logLevel { get; set; } = "INFO";
        public string @namespace { get; private set; } = "VestaAPI";
        public string applicationName { get; private set; } = AppDomain.CurrentDomain.FriendlyName;
        public string containerId { get; private set; } = System.Environment.MachineName;
        public string sessionId { get; set; } = Guid.NewGuid().ToString();
        public string tid { get; set; }
        public object custom1 { get; private set; }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this);
        }
        public void Setcustom1(object obj)
        {
            this.custom1 = obj;
        }

    }
}
