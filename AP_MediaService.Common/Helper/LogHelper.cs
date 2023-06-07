using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper
{
    public class LogHelper
    {
        private TraceLogWriter _TraceLogWriter;
        private CultureInfo ci = new CultureInfo("en-US");
        string LogPath = "\\Trace";
        public LogHelper()
        {
            Init();
        }

        private void Init()
        {
            string BaseDirectory = AppContext.BaseDirectory;
            LogPath = (BaseDirectory + LogPath);
            _TraceLogWriter = new TraceLogWriter(LogPath, "", "", "", "log", ci);
            _TraceLogWriter.SubfolderType = TraceLogWriter.FolderOption.Date;
            _TraceLogWriter.TransId = DateTime.Now.ToString("yyyyMMdd", ci);
        }

        public void WriteTraceLog(string line)
        {
            if (_TraceLogWriter != null)
            {
                lock (_TraceLogWriter)
                {
                    _TraceLogWriter.WriteLine(line);
                }
            }
        }
    }
}
