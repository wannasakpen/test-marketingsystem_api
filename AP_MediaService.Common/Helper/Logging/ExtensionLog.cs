using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper.Logging
{
    public class ExtensionLog
    {
        private DateTime StartTime;
        public ExtensionLog()
        {
            StartTime = DateTime.Now;
        }

        public static string? GetActualMethodName([CallerMemberName] string? name = null) => name;

    }
}
