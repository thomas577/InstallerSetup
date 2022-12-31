using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstallerSetup.Models.Tasks
{
    public abstract class TaskResultBase : ITaskResult
    {
        protected TaskResultBase(bool isSuccess) 
        {
            this.IsSuccess = isSuccess;
        }

        public bool IsSuccess { get; }
    }
}
