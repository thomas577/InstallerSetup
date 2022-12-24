using InstallerSetup.Controls;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        public LoggingService()
        {
            this.LogLines = new LoggingLineList();
        }

        public LoggingLineList LogLines { get; }

        public void Log(string message, LoggingType type, int indentation = 0)
        {
            LogViewerLineColor color;
            switch (type)
            {
                case LoggingType.Successful:
                    color = LogViewerLineColor.Green;
                    break;
                case LoggingType.Error:
                    color = LogViewerLineColor.Red;
                    break;
                default:
                    color = LogViewerLineColor.Normal;
                    break;
            }

            string indent = new string('a', indentation).Replace("a", "    ");
            IEnumerable<string> messageLines = message.Replace("\r", string.Empty)
                                                      .Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                                      .Select(x => indent + x.Trim(new[] { '\n' }));
            string messageIndented = string.Join("\r\n", messageLines);

            this.LogLines.Add(new LoggingLine(DateTime.Now, messageIndented, color));
        }
    }
}
