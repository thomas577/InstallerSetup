using InstallerSetup.Services.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public class TaskCheckFileExists : TaskBase<TaskResultBasic, TaskExecutionContextBasic>
    {
        public TaskCheckFileExists(string fullPath, 
                                   int nestingLevel, 
                                   IEnumerable<ITask<ITaskResult, TaskExecutionContextBasic>> childTasks, 
                                   ILoggingService loggingService) 
            : base(nestingLevel, childTasks, loggingService)
        {
        }

        public override string Description { get; }

        public override string DescriptionSuccess { get; }

        public override string DescriptionFailure { get; }

        protected override TaskResultBasic ExecuteInternal(TaskExecutionContextBasic context)
        {
            throw new NotImplementedException();
        }
    }
}
