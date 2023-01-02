using InstallerSetup.Services.FileSystem;
using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public class TaskReadFile : TaskBase<TaskReadFileResult>
    {
        private readonly string fullpath;
        private readonly IFileService fileService;

        public TaskReadFile(ILoggingService loggingService, ITask parentTask, IFileService fileService, string fullpath)
            : base(loggingService, parentTask)
        {
            this.fullpath = fullpath;
            this.fileService = fileService;
        }

        protected override TaskReadFileResult ExecuteInternal()
        {
            if (!this.fileService.CheckFullPathExists(this.fullpath)) return new TaskReadFileResult(isSuccess: false, isFileFound: false, bytes: null);
            byte[] bytes = this.fileService.ReadFile(this.fullpath);
            return new TaskReadFileResult(isSuccess: true, isFileFound: true, bytes: bytes);
        }

        protected override string GetDescriptionOutput()
        {
            return $"Reading content of file '{this.GetFileName()}'...";
        }

        protected override string GetExceptionOutput(Exception exception = null)
        {
            return $"Error while attempting to read file '{this.GetFileName()}'!";
        }

        protected override string GetUnsuccessfullOutput(TaskReadFileResult output)
        {
            return $"Could not read file content.";
        }

        protected override string GetSuccessOutput(TaskReadFileResult output)
        {
            return $"Successfully read {output.Bytes.Length} bytes.";
        }

        protected override TaskReadFileResult CreateTaskFailedResult(Exception exception)
        {
            return new TaskReadFileResult(isSuccess: false, isFileFound: false, bytes: null);
        }

        private string GetFileName()
        {
            return this.fileService.GetFileNameFromFullPath(this.fullpath);
        }
    }

    public class TaskReadFileResult : TaskResultBase
    {
        public TaskReadFileResult(bool isSuccess, bool isFileFound, byte[] bytes) : base(isSuccess)
        {
            this.Bytes = bytes;
            this.IsFileFound = isFileFound;
        }

        public byte[] Bytes { get; }

        public bool IsFileFound { get; }

        public string GetAsciiString()
        {
            if (this.Bytes == null) return string.Empty;
            return Encoding.ASCII.GetString(this.Bytes);
        }
    }
}
