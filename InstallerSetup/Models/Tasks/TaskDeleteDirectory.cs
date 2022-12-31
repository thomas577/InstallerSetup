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
    public class TaskDeleteDirectory : TaskBase<TaskDeleteDirectoryResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo parent;
        private readonly string name;

        public TaskDeleteDirectory(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo parent, string name)
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.parent = parent;
            this.name = name;
        }

        protected override TaskDeleteDirectoryResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskDeleteDirectoryResult(isSuccess: false);
        }

        protected override TaskDeleteDirectoryResult ExecuteInternal()
        {
            bool isDeletedSuccessfully = this.fileService.DeleteDirectory(this.parent, this.name);
            return new TaskDeleteDirectoryResult(isSuccess: isDeletedSuccessfully);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Deleting folder '{this.name}'...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to delete the folder";
        }

        protected override string GetSuccessOutput(TaskDeleteDirectoryResult output)
        {
            return $"Folder '{this.name}' deleted.";
        }

        protected override string GetUnsuccessfullOutput(TaskDeleteDirectoryResult output)
        {
            return $"Unable to delete the folder.";
        }
    }

    public class TaskDeleteDirectoryResult : TaskResultBase
    {
        public TaskDeleteDirectoryResult(bool isSuccess) : base(isSuccess)
        {
        }
    }
}
