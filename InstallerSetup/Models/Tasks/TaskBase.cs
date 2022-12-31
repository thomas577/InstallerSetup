using InstallerSetup.Controls;
using InstallerSetup.Services.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public abstract class TaskBase<T> : BindableBase, ITask<T> where T : ITaskResult
    {
        private readonly ILoggingService loggingService;
        private readonly ITask parentTask;
        private TaskStatus status;

        protected TaskBase(ILoggingService loggingService, ITask parentTask)
        {
            this.loggingService = loggingService;
            this.parentTask = parentTask;
            this.NestingLevel = (parentTask?.NestingLevel ?? -1) + 1;
            this.Status = TaskStatus.NotStarted;
        }

        public TaskStatus Status
        {
            get { return this.status; }
            set { this.SetProperty(ref this.status, value); }
        }

        public int NestingLevel { get; }

        public T Execute()
        {
            try
            {
                this.loggingService.Log(this.GetDescriptionOutput(), LoggingType.Normal, this.NestingLevel);
                this.Status = TaskStatus.Running;

                // Executes the task's internal logic
                T output = this.ExecuteInternal();
                if (output.IsSuccess)
                {
                    this.loggingService.Log(this.GetSuccessOutput(output), LoggingType.Successful, this.NestingLevel);
                    this.Status = TaskStatus.CompletedSuccessfully;
                }
                else
                {
                    this.loggingService.Log(this.GetUnsuccessfullOutput(output), LoggingType.Error, this.NestingLevel);
                    this.Status = TaskStatus.Faulted;
                }

                return output;
            }
            catch (Exception exception)
            {
                this.loggingService.Log(this.GetExceptionOutput(exception), LoggingType.Error, this.NestingLevel);
                this.loggingService.Log($"Exception details: {exception.Message}\r\n{exception.StackTrace}", LoggingType.Error, this.NestingLevel);
                this.Status = TaskStatus.Faulted;
                return this.CreateTaskFailedResult(exception);
            }
        }

        protected abstract string GetDescriptionOutput();

        protected abstract string GetExceptionOutput(Exception exception);

        protected abstract string GetUnsuccessfullOutput(T output);

        protected abstract string GetSuccessOutput(T output);

        protected abstract T CreateTaskFailedResult(Exception exception);

        protected abstract T ExecuteInternal();
    }
}
