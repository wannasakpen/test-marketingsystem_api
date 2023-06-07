using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Utilities
{
    public class ControllerHeader
    {
        public static string ControllerName;
        public static string CommandName;

        public static void setControllerName(string name)
        {
            ControllerName = name;
        }

        public static void setCommandName(string name)
        {
            CommandName = name;
        }
    }
}
