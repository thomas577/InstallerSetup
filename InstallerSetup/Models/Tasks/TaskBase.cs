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
    public abstract class TaskBase<T, U> : BindableBase, ITask<T, U>
    {
        private readonly ILoggingService loggingService;
        private readonly IEnumerable<ITask<ITaskResult, U>> childTasks;
        private TaskStatus status;

        protected TaskBase(int nestingLevel, IEnumerable<ITask<ITaskResult, U>> childTasks, ILoggingService loggingService) 
        {
            this.NestingLevel = nestingLevel;
            this.childTasks = childTasks;
            this.HasChildTasks = this.childTasks.Any();
            this.loggingService = loggingService;
            this.Status = TaskStatus.NotStarted;
        }

        public abstract string Description { get; }

        public abstract string DescriptionSuccess { get; }

        public abstract string DescriptionFailure { get; }

        public TaskStatus Status
        {
            get { return this.status; }
            set { this.SetProperty(ref this.status, value); }
        }

        public int NestingLevel { get; }

        public bool HasChildTasks { get; }

        public T Execute(U context)
        {
            try
            {
                this.loggingService.Log(this.Description, LoggingType.Normal, this.NestingLevel);
                this.Status = TaskStatus.Running;

                // Executes child tasks first, if any
                bool allChildTasksSuccessful = true;
                foreach (ITask<ITaskResult, U> childTask in this.childTasks)
                {
                    ITaskResult childResult = childTask.Execute(context);
                    if (!childResult.IsSuccess)
                    {
                        allChildTasksSuccessful = false;
                        break;
                    }
                }

                // If any child task has failed, we exit
                if (!allChildTasksSuccessful)
                {
                    this.loggingService.Log(this.DescriptionFailure, LoggingType.Error, this.NestingLevel);
                    this.Status = TaskStatus.Faulted;
                    return default(T);
                }
                else
                {
                    // Executes the task internal logic
                    T result = this.ExecuteInternal(context);
                    if (result.IsSuccess)
                    {
                        this.loggingService.Log(this.DescriptionSuccess, LoggingType.Successful, this.NestingLevel);
                        this.Status = TaskStatus.CompletedSuccessfully;
                    }
                    else
                    {
                        this.loggingService.Log(this.DescriptionFailure, LoggingType.Error, this.NestingLevel);
                        this.Status = TaskStatus.Faulted;
                    }

                    return result;
                }
            }
            catch (Exception exception)
            {
                this.loggingService.Log(this.DescriptionFailure, LoggingType.Error, this.NestingLevel);
                this.loggingService.Log($"Exception details: {exception.Message}\r\n{exception.StackTrace}", LoggingType.Error, this.NestingLevel);
                this.Status = TaskStatus.Faulted;
                return default(T);
            }
        }

        protected abstract T ExecuteInternal(U context);
    }
}
