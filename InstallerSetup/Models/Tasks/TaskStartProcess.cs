using InstallerSetup.Services.FileSystem;
using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace InstallerSetup.Models.Tasks
{
    public class TaskStartProcess : TaskBase<TaskStartProcessResult>
    {
        private readonly string exeFullPath;
        private readonly IFileService fileService;

        public TaskStartProcess(ILoggingService loggingService, ITask parentTask, string exeFullPath, IFileService fileService)
            : base(loggingService, parentTask)
        {
            this.exeFullPath = exeFullPath;
            this.fileService = fileService;
        }

        protected override TaskStartProcessResult ExecuteInternal()
        {
            if (!this.fileService.CheckFullPathExists(this.exeFullPath)) return new TaskStartProcessResult(isSuccess: false, exeFound: false);

            Process processStarted = Process.Start(this.exeFullPath);
            if (processStarted == null) return new TaskStartProcessResult(isSuccess: false, exeFound: true);

            return new TaskStartProcessResult(isSuccess: true, exeFound: true);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Starting '{this.exeFullPath}'...";
        }

        protected override string GetExceptionOutput(Exception exception = null)
        {
            return $"Error while attempting to start '{this.GetExeName()}'!";
        }

        protected override string GetUnsuccessfullOutput(TaskStartProcessResult output)
        {
            return output.ExeFound ? $"Unable to start process '{this.GetExeName()}'." : $"Path to {this.GetExeName()} does not exist... Skipping.";
        }

        protected override string GetSuccessOutput(TaskStartProcessResult output)
        {
            return $"{this.GetExeName()} successfuly started.";
        }

        protected override TaskStartProcessResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskStartProcessResult(isSuccess: false, exeFound: false);
        }

        private string GetExeName()
        {
            return this.fileService.GetFileNameFromFullPath(this.exeFullPath);
        }
    }

    public class TaskStartProcessResult : TaskResultBase
    {
        public TaskStartProcessResult(bool isSuccess, bool exeFound) : base(isSuccess)
        {
            this.ExeFound = exeFound;
        }

        public bool ExeFound { get; }
    }
}
