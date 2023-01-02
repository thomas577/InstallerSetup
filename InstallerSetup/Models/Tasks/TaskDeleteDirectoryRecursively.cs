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
    public class TaskDeleteDirectoryRecursively : TaskBase<TaskDeleteDirectoryRecursivelyResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo parent;
        private readonly string name;

        public TaskDeleteDirectoryRecursively(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo parent, string name)
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.parent = parent;
            this.name = name;
        }

        protected override TaskDeleteDirectoryRecursivelyResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskDeleteDirectoryRecursivelyResult(isSuccess: false, folderFound: false);
        }

        protected override TaskDeleteDirectoryRecursivelyResult ExecuteInternal()
        {
            bool folderFound = this.fileService.CheckFullPathExists(this.fileService.GetFullPathFromRelativePath(this.parent, this.name));
            if (!folderFound) return new TaskDeleteDirectoryRecursivelyResult(isSuccess: false, folderFound: folderFound);
            bool isDeletedSuccessfully = this.fileService.DeleteDirectoryRecursively(this.parent, this.name);
            return new TaskDeleteDirectoryRecursivelyResult(isSuccess: isDeletedSuccessfully, folderFound: folderFound);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Deleting folder '{this.name}' and subfolders recursively...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to delete the folder";
        }

        protected override string GetSuccessOutput(TaskDeleteDirectoryRecursivelyResult output)
        {
            return $"Folder '{this.name}' and its subfolders deleted.";
        }

        protected override string GetUnsuccessfullOutput(TaskDeleteDirectoryRecursivelyResult output)
        {
            return output.FolderFound ? $"Unable to delete the folder or its subfolders." : $"Folder '{this.name}' was not found.";
        }
    }

    public class TaskDeleteDirectoryRecursivelyResult : TaskResultBase
    {
        public TaskDeleteDirectoryRecursivelyResult(bool isSuccess, bool folderFound) : base(isSuccess)
        {
            this.FolderFound = folderFound;
        }

        public bool FolderFound { get; }
    }
}
