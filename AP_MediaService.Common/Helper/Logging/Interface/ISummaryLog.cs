using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging.Interface
{
    public interface ISummaryLog : ICommonLog
    {
        string reqTimestamp { get; }
        string initInvoke { get; set; }
        string cmdName { get; set; }
        string identity { get; }
        string resultCode { get; set; }
        string resultDesc { get; set; }
        string resTimestamp { get; }
        string usageTime { get; set; }
        string custom { get; }


        void SetIdentity(object identity);
        void SetreqTimestamp(DateTime dateTime);
        void SetresTimestamp(DateTime dateTime);
        void Setcustom(object obj);
    }
}
