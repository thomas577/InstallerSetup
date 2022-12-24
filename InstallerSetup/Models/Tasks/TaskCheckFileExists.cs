using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public class TaskCheckFileExists : TaskBase<bool>
    {
        private readonly string fullPath;

        public TaskCheckFileExists(int nestingLevel, ILoggingService loggingService, string fullPath) : base(nestingLevel, loggingService)
        {
            this.fullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
        }

        protected override (bool output, bool isSuccess) ExecuteInternal()
        {
            if (File.Exists(this.fullPath)) return (true, true);
            return (false, false);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Check if file '{this.fullPath}' exists...";
        }

        protected override string GetFailureOutput(Exception exception = null)
        {
            return $"File '{this.fullPath}' not found!";
        }

        protected override string GetSuccessOutput(bool output)
        {
            return $"File exists.";
        }
    }
}
