using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging.Interface
{
    public interface ITraceLog : ICommonLog
    {
        object custom1 { get; }
        void Setcustom1(object obj);
    }
}
