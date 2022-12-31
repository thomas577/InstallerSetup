using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public class TaskKillProcess : TaskBase<TaskKillProcessResult>
    {
        private readonly string processName;

        public TaskKillProcess(ILoggingService loggingService, ITask parentTask, string processName) 
            : base(loggingService, parentTask)
        {
            this.processName = processName;
        }

        protected override TaskKillProcessResult ExecuteInternal()
        {
            Process process = Process.GetProcesses().FirstOrDefault(x => x.ProcessName.ToLowerInvariant().Contains(this.processName));
            if (process == null)
            {
                return new TaskKillProcessResult(isSuccess: false, processFound: false);
            } 
            else
            {
                process.Kill();
                return new TaskKillProcessResult(isSuccess: true, processFound: true);
            }
        }

        protected override string GetDescriptionOutput()
        {
            return $"Killing proccess '{this.processName}' exists...";
        }

        protected override string GetExceptionOutput(Exception exception = null)
        {
            return $"Error while attempting to kill '{this.processName}'!";
        }

        protected override string GetUnsuccessfullOutput(TaskKillProcessResult output)
        {
            return output.ProcessFound ? $"Unable to kill process '{this.processName}'." : $"Process {this.processName} not found... Skipping.";
        }

        protected override string GetSuccessOutput(TaskKillProcessResult output)
        {
            return $"Process killed.";
        }

        protected override TaskKillProcessResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskKillProcessResult(isSuccess: false, processFound: false);
        }
    }

    public class TaskKillProcessResult : TaskResultBase
    {
        public TaskKillProcessResult(bool isSuccess, bool processFound) : base(isSuccess)
        {
            this.ProcessFound = processFound;
        }

        public bool ProcessFound { get; }
    }
}
