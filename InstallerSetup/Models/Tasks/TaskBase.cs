using InstallerSetup.Controls;
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
        private TaskStatus status;

        protected TaskBase(string description, int nestingLevel) 
        {
            this.Description = description;
            this.NestingLevel = nestingLevel;
        }

        public string Description { get; }

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
                this.Status = TaskStatus.Running;
                return default(T);
            }
            catch (Exception exception)
            {
                this.Status = TaskStatus.Faulted;
                return default(T);
            }
            finally
            {

            }
        }

        public abstract T ExecuteInternal(U context);
    }
}
