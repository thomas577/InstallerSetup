using InstallerSetup.Services.FileSystem;
using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public class TaskDeleteFile : TaskBase<TaskDeleteFileResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo parent;
        private readonly string name;

        public TaskDeleteFile(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo parent, string name)
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.parent = parent;
            this.name = name;
        }

        protected override TaskDeleteFileResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskDeleteFileResult(isSuccess: false);
        }

        protected override TaskDeleteFileResult ExecuteInternal()
        {
            bool isDeletedSuccessfully = this.fileService.DeleteFile(this.parent, this.name);
            return new TaskDeleteFileResult(isSuccess: isDeletedSuccessfully);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Deleting file '{this.name}'...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to delete the file";
        }

        protected override string GetSuccessOutput(TaskDeleteFileResult output)
        {
            return $"File '{this.name}' deleted.";
        }

        protected override string GetUnsuccessfullOutput(TaskDeleteFileResult output)
        {
            return $"Unable to delete the file.";
        }
    }

    public class TaskDeleteFileResult : TaskResultBase
    {
        public TaskDeleteFileResult(bool isSuccess) : base(isSuccess)
        {
        }
    }
}
