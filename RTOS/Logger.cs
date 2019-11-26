using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTOS
{
    public enum LogLevel
    {
        Debug,
        Info,
        Interrupt,
        Warning,
        Error
    }

    public interface ILogger
    {
        void Log(string text, LogLevel level);
    }
    public class CsvFileLogger : ILogger
    {
        private string Path { get; }
        public CsvFileLogger(string path)
        {
            Path = path;
        }

        public void Log(string text, LogLevel level)
        {
            using (var fs = new FileStream(Path, FileMode.Append))
            using (var sw = new StreamWriter(fs))
            {
                sw.WriteLine($"{DateTime.Now},{level.ToString()},\"{text.Replace("\"", "\"\"")}\"");
            }
        }
    }
}
