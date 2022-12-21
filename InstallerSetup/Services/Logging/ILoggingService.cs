using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Services.Logging
{
    public interface ILoggingService
    {
        LoggingLineList LogLines { get; }

        void Log(string message, LoggingType type, int indentation = 0);
    }

    public enum LoggingType
    {
        Normal,
        Successful,
        Error,
    }
}
