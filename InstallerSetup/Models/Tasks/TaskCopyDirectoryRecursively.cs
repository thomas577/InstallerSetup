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
    public class TaskCopyDirectoryRecursively : TaskBase<TaskCopyDirectoryRecursivelyResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo origin;
        private readonly DirectoryInfo targetParent;
        private readonly string targetName;

        public TaskCopyDirectoryRecursively(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo origin, DirectoryInfo targetParent, string targetName)
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.origin = origin;
            this.targetParent = targetParent;
            this.targetName = targetName;
        }

        protected override TaskCopyDirectoryRecursivelyResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskCopyDirectoryRecursivelyResult(isSuccess: false, originExists: false, parentExists: false);
        }

        protected override TaskCopyDirectoryRecursivelyResult ExecuteInternal()
        {
            bool originExists = this.fileService.CheckFullPathExists(this.origin.FullName);
            bool targetParentExists = this.fileService.CheckFullPathExists(this.targetParent.FullName);
            if (!originExists || !targetParentExists) return new TaskCopyDirectoryRecursivelyResult(isSuccess: false, originExists, targetParentExists);

            DirectoryInfo target = this.fileService.CreateDirectory(this.targetParent, this.targetName);
            if (target == null) return new TaskCopyDirectoryRecursivelyResult(isSuccess: false, originExists, targetParentExists);

            bool isSuccess = this.fileService.CopyDirectoryRecursively(this.origin, target);
            return new TaskCopyDirectoryRecursivelyResult(isSuccess, originExists, targetParentExists);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Copying folder '{this.origin.Name} to '{this.targetName}'...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to copy the folder!";
        }

        protected override string GetSuccessOutput(TaskCopyDirectoryRecursivelyResult output)
        {
            return $"Folder copy completed successfully.";
        }

        protected override string GetUnsuccessfullOutput(TaskCopyDirectoryRecursivelyResult output)
        {
            if (!output.OriginExists) return $"Could not find source folder {this.origin}.";
            if (!output.ParentExists) return $"Could not find target parent folder {this.targetParent.Name}.";
            return "Could not copy content of folder.";

        }
    }

    public class TaskCopyDirectoryRecursivelyResult : TaskResultBase
    {
        public TaskCopyDirectoryRecursivelyResult(bool isSuccess, bool originExists, bool parentExists) : base(isSuccess)
        {
            this.OriginExists = originExists;
            this.ParentExists = parentExists;
        }

        public bool OriginExists { get; }

        public bool ParentExists { get; }
    }
}
