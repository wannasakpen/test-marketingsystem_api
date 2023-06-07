using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AP_MediaService.Common.Helper
{
    public class TraceLogWriter
    {
        public enum FolderOption { None, Year, Month, Date, Hour, Quater };
        public enum LogFileOption { None, Year, Month, Date, Hour, Quater };
        private string m_logPath;
        private string m_prefix = "";
        private string m_format = "yyyy-MM-dd";
        private string m_suffix = "";
        private string m_extension = "txt";
        private CultureInfo m_culture = CultureInfo.CurrentCulture;
        private FolderOption m_subfolderType = FolderOption.None;
        private LogFileOption m_logFileType = LogFileOption.None;
        private string m_transId;
        private string m_content;
        private string m_msg;
        private Encoding _enc;
        //thread pool variable
        //AutoResetEvent _ev;		
        CultureInfo ci = new CultureInfo("en-US", false);

        //private ThreadWriter t_writer;

        public FolderOption SubfolderType
        {
            get
            {
                return m_subfolderType;
            }
            set
            {
                m_subfolderType = value;
            }
        }
        public LogFileOption LogFileType
        {
            get
            {
                return m_logFileType;
            }
            set
            {
                m_logFileType = value;
            }

        }
        public string TransId
        {
            get
            {
                return m_transId;
            }
            set
            {
                m_transId = value;
            }
        }

        public string Content
        {
            get
            {
                return m_content;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DateLogWriter class for files on the specified directory path.
        /// </summary>
        /// <param name="dirPath">A relative or absolute directory path.</param>
        public TraceLogWriter(string logPath)
        {
            try
            {
                Init(logPath, m_prefix, m_format, m_suffix, m_extension, m_culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Initializes a new instance of the DateLogWriter class for files on the specified directory path.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="culture"></param>
        public TraceLogWriter(string logPath, CultureInfo culture)
        {
            try
            {
                Init(logPath, m_prefix, m_format, m_suffix, m_extension, culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Initializes a new instance of the DateLogWriter class for files on the specified directory path.
        /// </summary>
        /// <param name="dirPath">A relative or absolute directory path.</param>
        /// <param name="prefix"></param>
        /// <param name="format"></param>
        /// <param name="suffix"></param>
        /// <param name="extension"></param>
        public TraceLogWriter(string logPath, string prefix, string format, string suffix, string extension, CultureInfo culture)
        {
            try
            {
                Init(logPath, prefix, format, suffix, extension, culture);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Auxilary method to initialize a new instance of the DateLogWriter class for files on the specified directory path.
        /// </summary>
        /// <param name="dirPath"></param>
        /// <param name="prefix"></param>
        /// <param name="format"></param>
        /// <param name="suffix"></param>
        /// <param name="extension"></param>
        private void Init(string logPath, string prefix, string format, string suffix, string extension, CultureInfo culture)
        {
            m_logPath = logPath + "\\";
            m_prefix = prefix;
            m_format = format;
            m_suffix = suffix;
            m_extension = extension;
            m_culture = culture;
            _enc = Encoding.UTF8;
        }

        private string GetDirectoryPath(string logPath, DateTime now, FolderOption fo)
        {
            CultureInfo ci = new CultureInfo("en-US", false);
            string y = now.ToString("yyyy", ci);
            string ym = now.ToString("yyyyMM", ci);
            string ymd = now.ToString("yyyyMMdd", ci);
            string ymdh = now.ToString("HHmm", ci);
            int min = now.Minute;
            string dirPath;

            if (fo == FolderOption.Year)
                dirPath = String.Format(@"{0}\{1}\", logPath, y);   // ex., "..\log\2004\
            else if (fo == FolderOption.Month)
                dirPath = String.Format(@"{0}\{1}\{2}\", logPath, y, ym);
            else if (fo == FolderOption.Date)
                dirPath = String.Format(@"{0}\{1}\{2}\{3}\", logPath, y, ym, ymd);
            else if (fo == FolderOption.Hour)
                dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, ymdh.Substring(0, 3) + "0");
            else if (fo == FolderOption.Quater)
            {
                if (min <= 15)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "00");
                else if (min > 15 && min <= 30)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "15");
                else if (min > 30 && min <= 45)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "30");
                else if (min > 45)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "45");
                else
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "00");
            }
            else
                dirPath = logPath + @"\";       // ex., "..\log\

            return dirPath.Replace("\\", "/").Replace("//", "/");
        }

        public static string GetLogPath(string logPath, DateTime now, FolderOption fo)
        {
            CultureInfo ci = new CultureInfo("en-US", false);
            string y = now.ToString("yyyy", ci);
            string ym = now.ToString("yyyyMM", ci);
            string ymd = now.ToString("yyyyMMdd", ci);
            string ymdh = now.ToString("yyyyMMddHHm", ci);
            string dirPath;
            int min = DateTime.Now.Minute;

            if (fo == FolderOption.Year)
                dirPath = String.Format(@"{0}\{1}\", logPath, y);   // ex., "..\log\2004\
            else if (fo == FolderOption.Month)
                dirPath = String.Format(@"{0}\{1}\{2}\", logPath, y, ym);
            else if (fo == FolderOption.Date)
                dirPath = String.Format(@"{0}\{1}\{2}\{3}\", logPath, y, ym, ymd);
            else if (fo == FolderOption.Hour)
                dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, ymdh);
            else if (fo == FolderOption.Quater)
            {
                if (min <= 15)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "00");
                else if (min > 15 && min <= 30)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "15");
                else if (min > 30 && min <= 45)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "30");
                else if (min > 45)
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "45");
                else
                    dirPath = String.Format(@"{0}\{1}\{2}\{3}\{4}\", logPath, y, ym, ymd, "00");
            }
            else
                dirPath = logPath + @"\";       // ex., "..\log\

            return dirPath;
        }

        //		private string GenerateDirectoryPath(DateTime now)
        //		{
        //			CultureInfo ci = new CultureInfo("en-US", false);			
        //			string dirPath;
        //
        //			if (m_subfolderType == FolderOption.Year)
        //				dirPath = m_logPath + "\\" + now.ToString("yyyy", ci) + "\\";		// ex., "..\log\2004\
        //			else if (m_subfolderType == FolderOption.Month)
        //				dirPath = m_logPath + "\\" + now.ToString("yyyyMM", ci) + "\\";		// ex., "..\log\200412\
        //			else if (m_subfolderType == FolderOption.Date)
        //				dirPath = m_logPath + "\\" + now.ToString("yyyyMMdd", ci) + "\\";		// ex., "..\log\20041231\
        //			else
        //				dirPath = m_logPath + "\\";		// ex., "..\log\
        //
        //			return dirPath;
        //		}

        /// <summary>
        /// Create a directory if it does not exist.
        /// </summary>
        /// <param name="path">The directory path to create.</param>
        private void CreateDirectory(string path)
        {
            // Create the directory if not already existed.
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception ex)            // No permission to create directory
                {
                    throw new Exception("Unable to create a directory for logging at " + path, ex);
                }
            }
        }

        public string GetFileName(string transId)
        {
            string fileName = m_prefix + transId + m_suffix + "." + m_extension;
            return fileName;
        }

        public string GetFilePathPms(string logPath, DateTime now, FolderOption fo)
        {
            string fileName = GetDirectoryPath(logPath, now, fo);
            return fileName;
        }

        /// <summary>
        /// Write the description of an exception into the file.
        /// </summary>
        /// <param name="ex">An exception to be written.</param>
        public void WriteException(Exception ex)
        {
            WriteException(m_transId, ex);
        }


        /// <summary>
        /// Writes a string to the file.
        /// </summary>
        /// <param name="line">The string to write. If value is a null reference (Nothing in Visual Basic), nothing is written.</param>		
        public virtual void Write(string message)
        {
            Write(m_transId, message);
        }


        /// <summary>
        /// Writes a string to the file.
        /// </summary>
        /// <param name="value">The string to write. If value is a null reference (Nothing in Visual Basic), nothing is written.</param>		
        public void Write(string transId, string message)
        {
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string fileName = GetFileName(transId);
            string ymd = DateTime.Now.ToString("yyyyMMddHH", ci);
            int min = DateTime.Now.Minute;

            //if (m_logFileType == LogFileOption.Quater)
            //{
            if (min <= 15)
                fileName = String.Format("{0}{1}.{2}", ymd, "00", m_extension);
            else if (min > 15 && min <= 30)
                fileName = String.Format("{0}{1}.{2}", ymd, "15", m_extension);
            else if (min > 30 && min <= 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "30", m_extension);
            else if (min > 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "45", m_extension);
            else
                fileName = String.Format("{0}{1}", ymd, "00");
            //}

            string filePath = dirPath + fileName;

            FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
            StreamWriter ssw = new StreamWriter(fs, Encoding.UTF8);
            ssw.AutoFlush = true;
            TextWriter sw = TextWriter.Synchronized(ssw);

            sw.Write(message);
            sw.Flush();
            sw.Close();

            m_content += message;
        }

        /// <summary>
        /// threa pool section
        /// </summary>
        /// <param name="o"></param>
        /// <param name="signal"></param>
        //write log with thread pool
        public void WriteLine(string transId, string message)
        {
            m_msg = message + "\r\n";
            m_transId = transId;
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string fileName = GetFileName(transId);
            string ymd = DateTime.Now.ToString("yyyyMMddHH", ci);
            int min = DateTime.Now.Minute;

            if (min <= 15)
                fileName = String.Format("{0}{1}.{2}", ymd, "00", m_extension);
            else if (min > 15 && min <= 30)
                fileName = String.Format("{0}{1}.{2}", ymd, "15", m_extension);
            else if (min > 30 && min <= 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "30", m_extension);
            else if (min > 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "45", m_extension);
            else
                fileName = String.Format("{0}{1}", ymd, "00");

            string filePath = dirPath + fileName;
            StreamWriter ssw = null;
            TextWriter sw = null;

            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                ssw = new StreamWriter(fs, new System.Text.UTF8Encoding(false));
                ssw.AutoFlush = true;
                sw = TextWriter.Synchronized(ssw);

                sw.Write(m_msg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception)
            {
                try
                {
                    filePath = dirPath + "Exception_" + fileName;

                    FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                    ssw = new StreamWriter(fs, new System.Text.UTF8Encoding(false));
                    ssw.AutoFlush = true;
                    sw = TextWriter.Synchronized(ssw);

                    sw.Write(m_msg);
                    sw.Flush();
                    sw.Close();
                    ssw.Close();
                }
                catch (Exception)
                {

                }
            }
            finally
            {
                if (ssw != null)
                    ssw.Close();

                if (sw != null)
                    sw.Close();
            }
        }

        public void WriteLinePms(string transId, string message)
        {
            m_msg = message + "\r\n";
            m_transId = transId;
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string fileName = GetFileName(transId);

            string filePath = dirPath + fileName;
            StreamWriter ssw = null;
            TextWriter sw = null;
            try
            {
                //FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                //ssw = new StreamWriter(fs, Encoding.UTF8);
                //ssw.AutoFlush = true;
                //sw = TextWriter.Synchronized(ssw);

                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                ssw = new StreamWriter(fs, new System.Text.UTF8Encoding(false));
                ssw.AutoFlush = true;
                sw = TextWriter.Synchronized(ssw);

                sw.Write(m_msg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ssw != null)
                    ssw.Close();

                if (sw != null)
                    sw.Close();
            }
        }

        public void WriteLineAmf(string transId, string message)
        {
            m_msg = message + "\r\n";
            m_transId = transId;
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string fileName = GetFileName(transId);

            string filePath = dirPath + fileName;
            StreamWriter ssw = null;
            TextWriter sw = null;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                //ssw = new StreamWriter(fs, Encoding.UTF8);
                ssw = new StreamWriter(fs, new System.Text.UTF8Encoding(false));
                ssw.AutoFlush = true;
                sw = TextWriter.Synchronized(ssw);

                sw.Write(m_msg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ssw != null)
                    ssw.Close();

                if (sw != null)
                    sw.Close();
            }
        }
        //public void WriteLine(string transId, string[] arr)
        //{
        //    m_transId = transId;
        //    DateTime dt = DateTime.Now;
        //    string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);

        //    //tor code for test gen broadcast file
        //    //dirPath += dt.ToString("HHmm") + "\\";

        //    CreateDirectory(dirPath);
        //    string fileName = GetFileName(transId);
        //    string filePath = dirPath + fileName;

        //    stateObj[] o = new stateObj[1];
        //    AutoResetEvent[] evs = new AutoResetEvent[1];

        //    for (int i = 0; i < evs.Length; i++)
        //    {
        //        evs[i] = new AutoResetEvent(false);
        //        o[i] = new stateObj();
        //    }

        //    for (int j = 0; j < evs.Length; j++)
        //    {
        //        t_writer = new ThreadWriter(evs[j], filePath, arr);
        //        ThreadPool.RegisterWaitForSingleObject(evs[j], new WaitOrTimerCallback(t_writer.writeArray), o[j], 0, true);

        //    }
        //    WaitHandle.WaitAll(evs);
        //}

        public void WriteLine(Encoding enc, string transId, string message)
        {
            string msg = message + "\r\n";
            Write(enc, transId, msg);
        }

        public void WriteLine(Encoding enc, string message)
        {
            string msg = message + "\r\n";
            Write(enc, "", msg);
        }

        /// <summary>
        /// old code . Writes a string to the file. 
        /// </summary>
        /// <param name="value">The string to write. If value is a null reference (Nothing in Visual Basic), nothing is written.</param>		
        public void Write(Encoding enc, string transId, string message)
        {
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string fileName = GetFileName(transId);
            string filePath = dirPath + fileName;

            FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
            StreamWriter ssw = new StreamWriter(fs, enc);
            ssw.AutoFlush = true;
            TextWriter sw = TextWriter.Synchronized(ssw);

            sw.Write(message);
            sw.Flush();
            sw.Close();

            m_content += message;
        }

        /// <summary>
        /// Writes a string followed by a line terminator to the file.
        /// </summary>
        /// <param name="line">The string to write. If value is a null reference (Nothing 
        /// in Visual Basic), only the line termination characters are written.</param>
        public void WriteLine(string message)
        {
            WriteLine(m_transId, message);
        }

        //public void WriteLine(string[] message)
        //{			
        //    WriteLine(m_transId, message);
        //}

        /// <summary>
        /// Writes a string followed by a line terminator to the file.
        /// </summary>
        /// <param name="value">The string to write. If value is a null reference (Nothing 
        /// in Visual Basic), only the line termination characters are written.</param>
        public void WriteLines(string transId, string message)
        {
            string msg = message + "\r\n";
            Write(transId, msg);
        }
        //
        //		public void WriteLine(Encoding enc, string transId, string message)
        //		{
        //			string msg = message + "\r\n";
        //			Write(enc, transId, msg);
        //		}

        public void WriteException(string transId, Exception ex)
        {
            string msg = CreateMessage(ex) + "\r\n";
            Write(transId, msg);
        }


        private string CreateMessage(Exception ex)
        {
            StringBuilder message = new StringBuilder();

            message.Append("\r\n");
            message.Append("Exception\r\n");
            message.Append("=======================\r\n");

            message.Append("Message: ");
            if (ex.Message != null)
                message.Append(ex.Message);
            message.Append("\r\n");

            message.Append("DateTime: ");
            message.Append(DateTime.Now.ToString());
            message.Append("\r\n");

            message.Append("Source: ");
            if (ex.Source != null)
                message.Append(ex.Source);
            message.Append("\r\n");

            message.Append("TargetSite: ");
            if (ex.TargetSite != null)
                message.Append(ex.TargetSite.ToString());
            message.Append("\r\n");

            message.Append("Type: ");
            if (ex.GetType() != null)
                message.Append(ex.GetType().ToString());
            message.Append("\r\n");

            message.Append("StackTrace: ");
            if (ex.StackTrace != null)
                message.Append(ex.StackTrace);
            message.Append("\r\n");

            message.Append("InnerException: ");
            if (ex.InnerException != null)
                message.Append((ex.InnerException).ToString());
            message.Append("\r\n");

            message.Append("HelpLink: ");
            if (ex.HelpLink != null)
                message.Append(ex.HelpLink);
            message.Append("\r\n");

            return message.ToString();
        }

        public void WriteRestMessage(string message, string folderName)
        {
            m_msg = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", ci) + "\r\n" + message + "\r\n";
            string fileName = string.Empty;
            DateTime dt = DateTime.Now;
            string dirPath = GetDirectoryPath(m_logPath + "\\" + folderName, dt, m_subfolderType);
            CreateDirectory(dirPath);
            string ymd = DateTime.Now.ToString("yyyyMMddHH", ci);
            int min = DateTime.Now.Minute;

            if (min <= 15)
                fileName = String.Format("{0}{1}.{2}", ymd, "00", m_extension);
            else if (min > 15 && min <= 30)
                fileName = String.Format("{0}{1}.{2}", ymd, "15", m_extension);
            else if (min > 30 && min <= 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "30", m_extension);
            else if (min > 45)
                fileName = String.Format("{0}{1}.{2}", ymd, "45", m_extension);
            else
                fileName = String.Format("{0}{1}", ymd, "00");

            string filePath = dirPath + fileName;
            StreamWriter ssw = null;
            TextWriter sw = null;
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite, 500, false);
                //ssw = new StreamWriter(fs, Encoding.UTF8);
                ssw = new StreamWriter(fs, new System.Text.UTF8Encoding(false));
                ssw.AutoFlush = true;
                sw = TextWriter.Synchronized(ssw);

                sw.Write(m_msg);
                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            { }
            finally
            {
                if (ssw != null)
                    ssw.Close();

                if (sw != null)
                    sw.Close();
            }
        }
    }
}
