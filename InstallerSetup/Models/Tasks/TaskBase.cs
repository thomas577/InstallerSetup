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
    public abstract class TaskBase<T> : BindableBase, ITask<T>
    {
        private readonly ILoggingService loggingService;
        private TaskStatus status;

        protected TaskBase(int nestingLevel, ILoggingService loggingService)
        {
            this.NestingLevel = nestingLevel;
            this.loggingService = loggingService;
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
                (T output, bool isSuccess) = this.ExecuteInternal();
                if (isSuccess)
                {
                    this.loggingService.Log(this.GetSuccessOutput(output), LoggingType.Successful, this.NestingLevel);
                    this.Status = TaskStatus.CompletedSuccessfully;
                }
                else
                {
                    this.loggingService.Log(this.GetFailureOutput(), LoggingType.Error, this.NestingLevel);
                    this.Status = TaskStatus.Faulted;
                }

                return output;
            }
            catch (Exception exception)
            {
                this.loggingService.Log(this.GetFailureOutput(exception), LoggingType.Error, this.NestingLevel);
                this.loggingService.Log($"Exception details: {exception.Message}\r\n{exception.StackTrace}", LoggingType.Error, this.NestingLevel);
                this.Status = TaskStatus.Faulted;
                return default(T);
            }
        }

        protected abstract string GetDescriptionOutput();

        protected abstract string GetFailureOutput(Exception exception = null);

        protected abstract string GetSuccessOutput(T output);

        protected abstract (T output, bool isSuccess) ExecuteInternal();
    }
}
