using InstallerSetup.Services.FileSystem;
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
    public class TaskCheckFileExists : TaskBase<TaskCheckFileExistsResult>
    {
        private readonly string fullPath;
        private readonly IFileService fileService;

        public TaskCheckFileExists(ILoggingService loggingService, ITask parentTask, string fullPath, IFileService fileService) 
            : base(loggingService, parentTask)
        {
            this.fullPath = fullPath ?? throw new ArgumentNullException(nameof(fullPath));
            this.fileService = fileService;
        }

        protected override TaskCheckFileExistsResult ExecuteInternal()
        {
            if (this.fileService.CheckFullPathExists(this.fullPath)) return new TaskCheckFileExistsResult(isSuccess: true);
            return new TaskCheckFileExistsResult(isSuccess: false);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Check if file '{this.fullPath}' exists...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error while checking if the file exists!";
        }

        protected override string GetSuccessOutput(TaskCheckFileExistsResult output)
        {
            return $"File exists.";
        }

        protected override string GetUnsuccessfullOutput(TaskCheckFileExistsResult output)
        {
            return $"File '{this.fullPath}' not found!";
        }

        protected override TaskCheckFileExistsResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskCheckFileExistsResult(isSuccess: false);
        }
    }

    public class TaskCheckFileExistsResult : TaskResultBase
    {
        public TaskCheckFileExistsResult(bool isSuccess) : base(isSuccess)
        {
        }
    }
}
