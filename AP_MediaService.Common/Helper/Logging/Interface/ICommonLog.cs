using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging.Interface
{
    public interface ICommonLog
    {
        string logType { get; }
        string systemTimestamp { get; set; }
        string containerId { get; }
        string @namespace { get; }
        string applicationName { get; }
        string sessionId { get; set; }
        string tid { get; set; }
        string Serialize();
    }
}
