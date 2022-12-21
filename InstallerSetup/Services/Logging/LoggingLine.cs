using InstallerSetup.Controls;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.Logging
{
    public class LoggingLine : BindableBase, ILogViewerLine
    {
        public LoggingLine(DateTime timestamp, string text, LogViewerLineColor color)
        {
            this.Timestamp = timestamp;
            this.Text = text;
            this.Color = color;
        }

        public DateTime Timestamp { get; }

        public string Text { get; }

        public LogViewerLineColor Color { get; }
    }
}
