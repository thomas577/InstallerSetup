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
    public class TaskCreateDirectory : TaskBase<TaskCreateDirectoryResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo parent;
        private readonly string name;

        public TaskCreateDirectory(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo parent, string name) 
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.parent = parent;
            this.name = name;
        }

        protected override TaskCreateDirectoryResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskCreateDirectoryResult(isSuccess: false, directoryCreated: null);
        }

        protected override TaskCreateDirectoryResult ExecuteInternal()
        {
            DirectoryInfo directory = this.fileService.CreateDirectory(this.parent, this.name);
            if (directory == null) return new TaskCreateDirectoryResult(isSuccess: false, directoryCreated: directory);
            return new TaskCreateDirectoryResult(isSuccess: true, directoryCreated: directory);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Creating new folder '{this.name}'...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to create the folder";
        }

        protected override string GetSuccessOutput(TaskCreateDirectoryResult output)
        {
            return $"Folder '{this.name}' created.";
        }

        protected override string GetUnsuccessfullOutput(TaskCreateDirectoryResult output)
        {
            return $"Unable to create the folder.";
        }
    }

    public class TaskCreateDirectoryResult : TaskResultBase
    {
        public TaskCreateDirectoryResult(bool isSuccess, DirectoryInfo directoryCreated) : base(isSuccess)
        {
            this.DirectoryCreated = directoryCreated;
        }

        public DirectoryInfo DirectoryCreated { get; }
    }
}
