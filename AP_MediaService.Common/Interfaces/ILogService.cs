using AP_MediaService.Common.Helper.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Interfaces
{
    public interface ILogService
    {
        Task WriteLog(LogModel logModel);
    }
}
