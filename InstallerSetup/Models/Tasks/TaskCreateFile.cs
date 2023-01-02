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
    public class TaskCreateFile : TaskBase<TaskCreateFileResult>
    {
        private readonly IFileService fileService;
        private readonly DirectoryInfo parent;
        private readonly string name;
        private readonly byte[] bytes;

        public TaskCreateFile(ILoggingService loggingService, ITask parentTask, IFileService fileService, DirectoryInfo parent, string name, byte[] bytes)
            : base(loggingService, parentTask)
        {
            this.fileService = fileService;
            this.parent = parent;
            this.name = name;
            this.bytes = bytes;
        }

        protected override TaskCreateFileResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskCreateFileResult(isSuccess: false, fileExistsAlready: false);
        }

        protected override TaskCreateFileResult ExecuteInternal()
        {
            if (this.fileService.CheckFullPathExists(this.fileService.GetFullPathFromRelativePath(this.parent, this.name))) return new TaskCreateFileResult(isSuccess: false, fileExistsAlready: true);
            FileInfo newFile = this.fileService.CreateFile(this.parent, this.name, this.bytes);
            if (newFile == null) return new TaskCreateFileResult(isSuccess: false, fileExistsAlready: false);
            return new TaskCreateFileResult(isSuccess: true, fileExistsAlready: false);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Creating new file '{this.name}' with {this.bytes.Length} bytes...";
        }

        protected override string GetExceptionOutput(Exception exception)
        {
            return $"Error occured while attempting to create the file!";
        }

        protected override string GetSuccessOutput(TaskCreateFileResult output)
        {
            return $"File '{this.name}' created.";
        }

        protected override string GetUnsuccessfullOutput(TaskCreateFileResult output)
        {
            return output.FileExistsAlready ? $"File '{this.name}' exists already." : $"Unable to create the file.";
        }
    }

    public class TaskCreateFileResult : TaskResultBase
    {
        public TaskCreateFileResult(bool isSuccess, bool fileExistsAlready) : base(isSuccess)
        {
            this.FileExistsAlready = fileExistsAlready;
        }

        public bool FileExistsAlready { get; }
    }
}
